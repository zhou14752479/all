using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 学科网下载
{
    public partial class admin : System.Web.UI.Page
    {

        public static string constr = "Host =localhost;Database=xueke;Username=root;Password=root";
        protected void Page_Load(object sender, EventArgs e)
        {
            string method = Request["method"];
            string page = Request["page"];
            string key = Request["key"];
            if (method == "getkey"  && page != "")
            {
                getkey(Convert.ToInt32(page));
            }

            if (method == "getlog")
            {
                getlog(Convert.ToInt32(page));
            }

            
        }


        #region 获取key列表

        public void getkey(int page)
        {
            Response.ContentType = "application/json";

            try
            {

                // 获取页码参数

                if (!string.IsNullOrEmpty(Request.QueryString["page"]))
                {
                    if (!int.TryParse(Request.QueryString["page"], out page) || page < 1)
                    {
                        page = 1;
                    }
                }



                // 获取数据
                var result = GetPagedData(page);

                // 序列化为JSON并返回
                var serializer = new JavaScriptSerializer();
                Response.Write(serializer.Serialize(result));
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.Write("{\"error\":\"" + ex.Message.Replace("\"", "\\\"") + "\"}");
            }

        }


        private PagedResult GetPagedData(int page)
        {
            int itemsPerPage = 50;
            var result = new PagedResult();
            result.items = new List<DataItem>();

            using (MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection(constr))
            {
                // 计算偏移量
                int offset = (page - 1) * itemsPerPage;

                // 获取总记录数
                string countQuery = "SELECT COUNT(*) FROM mykeys";


                MySqlCommand countCommand = new MySqlCommand(countQuery, connection);

                // 获取当前页数据
                string dataQuery = @"
    SELECT * 
    FROM mykeys
    ORDER BY extime DESC
    LIMIT @ItemsPerPage OFFSET @Offset ";

                MySqlCommand dataCommand = new MySqlCommand(dataQuery, connection);
                dataCommand.Parameters.AddWithValue("@Offset", offset);
                dataCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);

                try
                {
                    connection.Open();

                    // 获取总记录数
                    result.totalItems = Convert.ToInt32(countCommand.ExecuteScalar());
                    result.totalPages = (int)Math.Ceiling((double)result.totalItems / itemsPerPage);

                    // 获取数据
                    using (MySqlDataReader reader = dataCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new DataItem
                            {
                                id = Convert.ToInt32(reader["id"]),
                                mykey = reader["mykey"].ToString(),

                                cishu = reader["cishu"].ToString(),
                                extime = reader["extime"].ToString(),
                                day = reader["day"].ToString(),
                                isvip = reader["isvip"].ToString()
                            };
                            result.items.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("数据库查询错误: " + ex.ToString());
                }
            }

            return result;
        }


        // 数据模型类
        public class PagedResult
        {
            public List<DataItem> items { get; set; }
            public int totalItems { get; set; }
            public int totalPages { get; set; }
        }

        public class DataItem
        {
            public int id { get; set; }
            public string mykey { get; set; }
            public string extime { get; set; }
            public string cishu { get; set; }
            public string day { get; set; }
            public string isvip { get; set; }
        }
        #endregion


        #region 获取日志列表

        public void getlog(int page)
        {
            Response.ContentType = "application/json";

            try
            {

                // 获取页码参数

                if (!string.IsNullOrEmpty(Request.QueryString["page"]))
                {
                    if (!int.TryParse(Request.QueryString["page"], out page) || page < 1)
                    {
                        page = 1;
                    }
                }



                // 获取数据
                var result = GetPagedData2(page);

                // 序列化为JSON并返回
                var serializer = new JavaScriptSerializer();
                Response.Write(serializer.Serialize(result));
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.Write("{\"error\":\"" + ex.Message.Replace("\"", "\\\"") + "\"}");
            }

        }


        private PagedResult2 GetPagedData2(int page)
        {
            int itemsPerPage = 50;
            var result = new PagedResult2();
            result.items = new List<DataItem2>();

            using (MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection(constr))
            {
                // 计算偏移量
                int offset = (page - 1) * itemsPerPage;

                // 获取总记录数
                string countQuery = "SELECT COUNT(*) FROM logs";


                MySqlCommand countCommand = new MySqlCommand(countQuery, connection);

                // 获取当前页数据
                string dataQuery = @"
    SELECT * 
    FROM logs
    ORDER BY time DESC
    LIMIT @ItemsPerPage OFFSET @Offset ";

                MySqlCommand dataCommand = new MySqlCommand(dataQuery, connection);
                dataCommand.Parameters.AddWithValue("@Offset", offset);
                dataCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);

                try
                {
                    connection.Open();

                    // 获取总记录数
                    result.totalItems = Convert.ToInt32(countCommand.ExecuteScalar());
                    result.totalPages = (int)Math.Ceiling((double)result.totalItems / itemsPerPage);

                    // 获取数据
                    using (MySqlDataReader reader = dataCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new DataItem2
                            {
                                id = Convert.ToInt32(reader["id"]),
                                mykey = reader["mykey"].ToString(),

                                ip= reader["ip"].ToString(),
                                time = reader["time"].ToString(),
                                area = reader["area"].ToString(),
                                
                            };
                            result.items.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("数据库查询错误: " + ex.ToString());
                }
            }

            return result;
        }


        // 数据模型类
        public class PagedResult2
        {
            public List<DataItem2> items { get; set; }
            public int totalItems { get; set; }
            public int totalPages { get; set; }
        }

        public class DataItem2
        {
            public int id { get; set; }
            public string mykey { get; set; }
            public string time { get; set; }
            public string ip { get; set; }
            public string area{ get; set; }
           
        }
        #endregion



    }
}