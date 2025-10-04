using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 学科网下载
{
    public partial class KeyHandler : System.Web.UI.Page
    {

        // 数据库连接字符串，请根据实际情况修改
        private string connectionString = "Host =localhost;Database=xueke;Username=root;Password=YB29ts9bWM7nKnChvfd3";
        //private string connectionString = "Host =localhost;Database=xueke;Username=root;Password=root";
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";

            // 获取操作类型
            string action = Request.QueryString["action"] ?? "";
            
            try
            {
                // 检查访问权限（仅限sq地区）
                if (!CheckAccessPermission())
                {
                    Response.Write("{\"success\":false,\"message\":\"区域拒绝访问\"}");
                    Response.End();
                    Response.Redirect("http://www.baidu.com", false);
                    return;
                }

                
                switch (action.ToLower())
                {
                    case "list":
                        HandleListRequest();
                        break;
                    case "generate":
                        HandleGenerateRequest();
                        break;
                    case "addbatch":
                        HandleAddBatchRequest();
                        break;
                    case "get":
                        HandleGetRequest();
                        break;
                    case "update":
                        HandleUpdateRequest();
                        break;
                    case "delete":
                        HandleDeleteRequest();
                        break;
                    default:
                        Response.Write("{\"success\":false,\"message\":\"无效的操作类型\"}");
                        break;
                }
            }
            catch (Exception ex)
            {
                Response.Write("{\"success\":false,\"message\":\"" + ex.Message.Replace("\"", "\\\"") + "\"}");
            }

            Response.End();

        }








        /// <summary>
        /// 检查访问权限（仅限宿迁地区）
        /// </summary>
        private bool CheckAccessPermission()
        {
            string userIp = Request.UserHostAddress;
            string area = method.getareaFromIp(userIp);  //获取IP信息
            return area.Contains("宿迁市");
        }

        /// <summary>
        /// 处理卡密列表请求
        /// </summary>
        private void HandleListRequest()
        {
            try
            {
                // 获取分页参数
                int page = int.TryParse(Request.QueryString["page"], out int p) ? p : 1;
                int pageSize = int.TryParse(Request.QueryString["pageSize"], out int ps) ? ps : 20;
                string searchKey = Request.QueryString["searchKey"] ?? "";

                // 计算偏移量
                int offset = (page - 1) * pageSize;

                // 构建查询条件
                string whereClause = "";
                if (!string.IsNullOrEmpty(searchKey))
                {
                    whereClause = "WHERE mykey LIKE '%" + MySqlHelper.EscapeString(searchKey) + "%'";
                }

                // 获取总记录数
                int totalCount = GetTotalRecordCount(whereClause);
                int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                // 获取当前页数据
                List<KeyInfo> keys = GetKeysByPage(whereClause, pageSize, offset);

                // 构建响应
                var response = new
                {
                    success = true,
                    totalCount = totalCount,
                    totalPages = totalPages,
                    currentPage = page,
                    data = keys
                };

                Response.Write(new JavaScriptSerializer().Serialize(response));
            }
            catch (Exception ex)
            {
                Response.Write("{\"success\":false,\"message\":\"" + ex.Message.Replace("\"", "\\\"") + "\"}");
            }
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        private int GetTotalRecordCount(string whereClause = "")
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = $"SELECT COUNT(*) FROM mykeys {whereClause}";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// 分页获取卡密数据
        /// </summary>
        private List<KeyInfo> GetKeysByPage(string whereClause, int pageSize, int offset)
        {
            List<KeyInfo> keys = new List<KeyInfo>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = $"SELECT * FROM mykeys {whereClause} LIMIT @pageSize OFFSET @offset";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@offset", offset);

                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        KeyInfo key = new KeyInfo
                        {
                            id = reader.GetInt32("id"),
                            mykey = reader.GetString("mykey"),
                            cishu = reader.GetInt32("cishu"),
                            day = reader.GetInt32("day"),
                            extime = reader.IsDBNull(reader.GetOrdinal("extime")) ? null : reader.GetDateTime("extime").ToString("yyyy-MM-dd HH:mm:ss"),
                            isvip = reader.GetInt32("isvip")
                        };
                        keys.Add(key);
                    }
                }
            }

            return keys;
        }

        /// <summary>
        /// 处理生成卡密请求
        /// </summary>
        private void HandleGenerateRequest()
        {
            try
            {
                // 获取参数
                int count = int.TryParse(Request.QueryString["count"], out int c) ? c : 10;
                int length = int.TryParse(Request.QueryString["length"], out int l) ? l : 16;
                string prefix = Request.QueryString["prefix"] ?? "";

                // 验证参数
                if (count <= 0 || count > 10000)
                    throw new Exception("数量必须在1-10000之间");

                if (length < 6 || length > 100)
                    throw new Exception("卡密长度必须在6-100之间");

                // 生成卡密
                List<string> keys = GenerateUniqueKeys(count, length, prefix);

                // 构建响应
                var response = new
                {
                    success = true,
                    count = keys.Count,
                    keys = keys
                };

                Response.Write(new JavaScriptSerializer().Serialize(response));
            }
            catch (Exception ex)
            {
                Response.Write("{\"success\":false,\"message\":\"" + ex.Message.Replace("\"", "\\\"") + "\"}");
            }
        }

        /// <summary>
        /// 生成唯一的卡密列表
        /// </summary>
        private List<string> GenerateUniqueKeys(int count, int length, string prefix)
        {
            List<string> keys = new List<string>();
            HashSet<string> existingKeys = GetExistingKeys(); // 获取数据库中已有的卡密

            int retryCount = 0;
            const int maxRetry = 1000; // 最大重试次数

            while (keys.Count < count)
            {
                // 拼接前缀 + 随机串
                string newKey = prefix + GenerateRandomKey(length);

                // 检查是否已存在
                if (!existingKeys.Contains(newKey) && !keys.Contains(newKey))
                {
                    keys.Add(newKey);
                    retryCount = 0; // 重置重试计数
                }
                else
                {
                    retryCount++;
                    if (retryCount > maxRetry)
                    {
                        throw new Exception("生成卡密时遇到重复问题，无法生成足够的唯一卡密");
                    }
                }
            }

            return keys;
        }

        /// <summary>
        /// 生成随机卡密
        /// </summary>
        private string GenerateRandomKey(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            char[] keyChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                keyChars[i] = chars[rnd.Next(chars.Length)];
            }

            return new string(keyChars);
        }

        /// <summary>
        /// 获取数据库中已存在的卡密
        /// </summary>
        private HashSet<string> GetExistingKeys()
        {
            HashSet<string> keys = new HashSet<string>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT mykey FROM mykeys";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            keys.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return keys;
        }

        /// <summary>
        /// 处理批量添加卡密请求
        /// </summary>
        private void HandleAddBatchRequest()
        {
            try
            {
                // 读取请求数据
                string requestBody;
                using (StreamReader reader = new StreamReader(Request.InputStream))
                {
                    requestBody = reader.ReadToEnd();
                }

                // 解析JSON数据
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                dynamic data = serializer.Deserialize<dynamic>(requestBody);

                // 提取参数
                List<string> keys = serializer.Deserialize<List<string>>(serializer.Serialize(data["keys"]));
                int cishu = int.Parse(data["cishu"].ToString());
                int day = int.Parse(data["day"].ToString());
                string extime = data["extime"] != null ? data["extime"].ToString() : null;
                int isvip = int.Parse(data["isvip"].ToString());

                // 验证参数
                if (keys == null || keys.Count == 0)
                    throw new Exception("没有可添加的卡密");

                if (cishu < 0)
                    throw new Exception("次数不能为负数");

                if (day < 0)
                    throw new Exception("天数不能为负数");

                // 转换过期时间
                DateTime? expireTime = null;
                if (!string.IsNullOrEmpty(extime))
                {
                    if (DateTime.TryParse(extime, out DateTime parsedTime))
                    {
                        expireTime = parsedTime;
                    }
                    else
                    {
                        throw new Exception("无效的过期时间格式");
                    }
                }

                // 批量插入数据库
                int successCount = 0;
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            string query = @"INSERT INTO mykeys (mykey, cishu, day, extime, isvip) 
                                            VALUES (@mykey, @cishu, @day, @extime, @isvip)";

                            MySqlCommand cmd = new MySqlCommand(query, conn, transaction);
                            cmd.Parameters.Add("@mykey", MySqlDbType.VarChar);
                            cmd.Parameters.AddWithValue("@cishu", cishu);
                            cmd.Parameters.AddWithValue("@day", day);

                            if (expireTime.HasValue)
                                cmd.Parameters.AddWithValue("@extime", expireTime.Value);
                            else
                                cmd.Parameters.AddWithValue("@extime", DBNull.Value);

                            cmd.Parameters.AddWithValue("@isvip", isvip);

                            // 批量执行
                            foreach (string key in keys)
                            {
                                cmd.Parameters["@mykey"].Value = key;
                                cmd.ExecuteNonQuery();
                                successCount++;
                            }

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                // 构建响应
                var response = new
                {
                    success = true,
                    count = successCount
                };

                Response.Write(serializer.Serialize(response));
            }
            catch (Exception ex)
            {
                Response.Write("{\"success\":false,\"message\":\"" + ex.Message.Replace("\"", "\\\"") + "\"}");
            }
        }

        /// <summary>
        /// 处理获取单个卡密请求
        /// </summary>
        private void HandleGetRequest()
        {
            try
            {
                // 获取ID参数
                if (!int.TryParse(Request.QueryString["id"], out int id) || id <= 0)
                {
                    Response.Write("{\"success\":false,\"message\":\"无效的卡密ID\"}");
                    return;
                }

                // 查询卡密信息
                KeyInfo key = GetKeyById(id);
                if (key == null)
                {
                    Response.Write("{\"success\":false,\"message\":\"卡密不存在\"}");
                    return;
                }

                // 构建响应
                var response = new
                {
                    success = true,
                    data = key
                };

                Response.Write(new JavaScriptSerializer().Serialize(response));
            }
            catch (Exception ex)
            {
                Response.Write("{\"success\":false,\"message\":\"" + ex.Message.Replace("\"", "\\\"") + "\"}");
            }
        }

        /// <summary>
        /// 根据ID获取卡密信息
        /// </summary>
        private KeyInfo GetKeyById(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM mykeys WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new KeyInfo
                        {
                            id = reader.GetInt32("id"),
                            mykey = reader.GetString("mykey"),
                            cishu = reader.GetInt32("cishu"),
                            day = reader.GetInt32("day"),
                            extime = reader.IsDBNull(reader.GetOrdinal("extime")) ? null : reader.GetDateTime("extime").ToString("yyyy-MM-dd HH:mm:ss"),
                            isvip = reader.GetInt32("isvip")
                        };
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 处理更新卡密请求
        /// </summary>
        private void HandleUpdateRequest()
        {
            try
            {
                // 读取请求数据
                string requestBody;
                using (StreamReader reader = new StreamReader(Request.InputStream))
                {
                    requestBody = reader.ReadToEnd();
                }

                // 解析JSON数据
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                dynamic data = serializer.Deserialize<dynamic>(requestBody);

                // 提取参数
                int id = int.Parse(data["id"].ToString());
                string mykey = data["mykey"].ToString().Trim();
                int cishu = int.Parse(data["cishu"].ToString());
                int day = int.Parse(data["day"].ToString());
                string extime = data["extime"] != null ? data["extime"].ToString() : null;
                int isvip = int.Parse(data["isvip"].ToString());

                // 验证参数
                if (string.IsNullOrEmpty(mykey))
                    throw new Exception("卡密不能为空");

                if (cishu < 0)
                    throw new Exception("次数不能为负数");

                if (day < 0)
                    throw new Exception("天数不能为负数");

                // 检查卡密是否已存在（排除当前记录）
                if (IsKeyExists(mykey, id))
                    throw new Exception("卡密已存在，请更换");

                // 转换过期时间
                DateTime? expireTime = null;
                if (!string.IsNullOrEmpty(extime))
                {
                    if (DateTime.TryParse(extime, out DateTime parsedTime))
                    {
                        expireTime = parsedTime;
                    }
                    else
                    {
                        throw new Exception("无效的过期时间格式");
                    }
                }

                // 更新卡密
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE mykeys SET 
                                    mykey = @mykey, 
                                    cishu = @cishu, 
                                    day = @day, 
                                    extime = @extime, 
                                    isvip = @isvip 
                                    WHERE id = @id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@mykey", mykey);
                    cmd.Parameters.AddWithValue("@cishu", cishu);
                    cmd.Parameters.AddWithValue("@day", day);

                    if (expireTime.HasValue)
                        cmd.Parameters.AddWithValue("@extime", expireTime.Value);
                    else
                        cmd.Parameters.AddWithValue("@extime", DBNull.Value);

                    cmd.Parameters.AddWithValue("@isvip", isvip);
                    cmd.Parameters.AddWithValue("@id", id);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                        throw new Exception("更新失败，卡密可能已被删除");
                }

                // 构建响应
                var response = new
                {
                    success = true,
                    message = "卡密更新成功"
                };

                Response.Write(serializer.Serialize(response));
            }
            catch (Exception ex)
            {
                Response.Write("{\"success\":false,\"message\":\"" + ex.Message.Replace("\"", "\\\"") + "\"}");
            }
        }

        /// <summary>
        /// 检查卡密是否已存在
        /// </summary>
        private bool IsKeyExists(string key, int excludeId = -1)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM mykeys WHERE mykey = @mykey";

                // 如果指定了排除ID，则不检查该记录
                if (excludeId > 0)
                {
                    query += " AND id != @excludeId";
                }

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@mykey", key);

                if (excludeId > 0)
                {
                    cmd.Parameters.AddWithValue("@excludeId", excludeId);
                }

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        /// <summary>
        /// 处理删除卡密请求
        /// </summary>
        private void HandleDeleteRequest()
        {
            try
            {
                // 获取ID参数
                if (!int.TryParse(Request.QueryString["id"], out int id) || id <= 0)
                {
                    Response.Write("{\"success\":false,\"message\":\"无效的卡密ID\"}");
                    return;
                }

                // 检查记录是否存在
                if (!IsRecordExists(id))
                {
                    Response.Write("{\"success\":false,\"message\":\"卡密不存在或已被删除\"}");
                    return;
                }

                // 删除卡密
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM mykeys WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                        throw new Exception("删除失败");
                }

                // 构建响应
                var response = new
                {
                    success = true,
                    message = "卡密删除成功"
                };

                Response.Write(new JavaScriptSerializer().Serialize(response));
            }
            catch (Exception ex)
            {
                Response.Write("{\"success\":false,\"message\":\"" + ex.Message.Replace("\"", "\\\"") + "\"}");
            }
        }

        /// <summary>
        /// 检查记录是否存在
        /// </summary>
        private bool IsRecordExists(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM mykeys WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

















    }


    /// <summary>
    /// 卡密信息实体类
    /// </summary>
    public class KeyInfo
    {
        public int id { get; set; }
        public string mykey { get; set; }
        public int cishu { get; set; }
        public int day { get; set; }
        public string extime { get; set; }
        public int isvip { get; set; }
    }
}