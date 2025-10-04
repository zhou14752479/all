using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;


namespace 视频号web端
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            getgg();
            //GenerateQRCode();
        }


        public static string constr = "Host=localhost;Database=sph;Username=root;Password=eM2bN0mE4lD2";

        private static readonly HttpClient _httpClient = new HttpClient(new HttpClientHandler
        {
            UseCookies = true,
            AllowAutoRedirect = true,
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
        });
        /// <summary>
        /// 生成二维码
        /// </summary>
        private void GenerateQRCode()
        {
            try
            {
                var loginUrl = "https://channels.weixin.qq.com/cgi-bin/mmfinderassistant-bin/auth/auth_login_code";
                var timestamp = GetTimestamp();

                var formData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("timestamp", timestamp.ToString()),
                    new KeyValuePair<string, string>("_log_finder_id", "null"),
                    new KeyValuePair<string, string>("rawKeyBuff", "null")
                });

                var response = _httpClient.PostAsync(loginUrl, formData).Result;
                var responseJson = response.Content.ReadAsStringAsync().Result;
                var json = JObject.Parse(responseJson);

                var token = json["data"]["token"].ToString();
                tokenField.Value = token;

                var qrCodeUrl = $"https://channels.weixin.qq.com/mobile/confirm_login.html?token={token}";

                // 生成二维码并转换为Base64
                using (var qrGenerator = new QRCodeGenerator())
                {
                    var qrCodeData = qrGenerator.CreateQrCode(qrCodeUrl, QRCodeGenerator.ECCLevel.Q);
                    using (var qrCode = new QRCode(qrCodeData))
                    {
                        using (var bitmap = qrCode.GetGraphic(20))
                        {
                            using (var stream = new MemoryStream())
                            {
                                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                byte[] imageBytes = stream.ToArray();
                                string base64String = Convert.ToBase64String(imageBytes);
                                qrCodeImage.ImageUrl = "data:image/png;base64," + base64String;
                            }
                        }
                    }
                }

                statusLabel.Text = "请扫描二维码登录";
                statusTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"生成二维码失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 刷新二维码按钮点击事件
        /// </summary>
        protected void refreshButton_Click(object sender, EventArgs e)
        {
            GenerateQRCode();
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        private static long GetTimestamp()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        /// <summary>
        /// 定时检查登录状态
        /// </summary>
        protected void statusTimer_Tick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tokenField.Value))
                return;

            try
            {
                var token = tokenField.Value;
                var timestamp = GetTimestamp();
                var statusCheckUrl = $"https://channels.weixin.qq.com/cgi-bin/mmfinderassistant-bin/auth/auth_login_status?token={token}&timestamp={timestamp}";

                var response = _httpClient.PostAsync(statusCheckUrl, null).Result;
                var statusJson = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                var status = statusJson["data"]["status"].ToString();

                switch (status)
                {
                    case "0":
                        statusLabel.Text = "二维码未失效，请扫码！";
                        break;
                    case "5":
                        statusLabel.Text = "已扫码，请在手机上确认！";
                        break;
                    case "1":
                        // 登录成功，获取用户信息并保存Cookie
                        string cookies2 = "";
                        if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
                        {
                            foreach (var cookie in cookies)
                            {
                                cookies2 += (cookie);
                            }
                        }
                        statusLabel.Text = "登录成功";
                        SaveLoginCookies(cookies2);
                       
                        statusTimer.Enabled = false;
                        break;
                    default:
                        statusLabel.Text = $"登录状态异常: {status}";
                        break;
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"检查状态失败: {ex.Message}";
            }
        }







        /// <summary>
        /// 保存登录Cookie
        /// </summary>
        private void SaveLoginCookies(string cookies)
        {
            try
            {
                string time = "Expires=Wed, 22 Sep 10055 05:51:47 GMT;";
                string sessionid = Regex.Match(cookies, @"sessionid=([\s\S]*?);").Groups[1].Value;
                string wxuin = Regex.Match(cookies, @"wxuin=([\s\S]*?);").Groups[1].Value;
                string ck = "sessionid="+sessionid + ";wxuin=" + wxuin+";";
                string url = "https://channels.weixin.qq.com/cgi-bin/mmfinderassistant-bin/auth/auth_data";
              
                string html = PostUrlDefault(url,"",ck);
                string nickname = Regex.Match(html, @",""nickname"":""([\s\S]*?)""").Groups[1].Value;
                string uniqId = Regex.Match(html, @"""uniqId"":""([\s\S]*?)""").Groups[1].Value;
                string fansCount = Regex.Match(html, @"""fansCount"":([\s\S]*?),").Groups[1].Value;
                statusLabel.Text = "登录成功："+nickname;
                adduser(nickname,uniqId,ck,time, fansCount);
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"保存登录信息失败: {ex.Message}";
            }
        }

        #region  添加到数据库
       
        public static void adduser(string nickname,string uniqId, string cookies,string extime,string fansCount)
        {
            try
            {

                
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                string sql = "INSERT INTO users (nickname,uniqId,cookies,expireTime,fansCount)VALUES(@nickname, @uniqId, @cookies, @extime,@fansCount)";


                MySqlCommand cmd = new MySqlCommand(sql, mycon);
                cmd.Parameters.AddWithValue("@nickname", nickname);
                cmd.Parameters.AddWithValue("@uniqId", uniqId);
                cmd.Parameters.AddWithValue("@cookies", cookies);
                cmd.Parameters.AddWithValue("@extime", extime);
                cmd.Parameters.AddWithValue("@fansCount", fansCount);

                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.


                mycon.Close();


            }
            catch (Exception)
            {


            }
        }
        #endregion

        #region POST默认请求
        public static string PostUrlDefault(string url, string postData, string COOKIE)
        {
            string result;
            try
            {
                string charset = "utf-8";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("version:TYC-XCX-WX");
                //request.ContentType = "application/x-www-form-urlencoded";
                request.ContentType = "application/json";
                request.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
                request.Referer = "https://channels.weixin.qq.com/platform";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                response.GetResponseHeader("Set-Cookie");
                bool flag = response.Headers["Content-Encoding"] == "gzip";
                string html;
                if (flag)
                {
                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader2 = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
                    html = reader2.ReadToEnd();
                    reader2.Close();
                }
                response.Close();
                result = html;
            }
            catch (WebException ex)
            {
                //result = ex.ToString();
                //400错误也返回内容
                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }
        #endregion


        #region

        public void getgg()
        {
            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                try
                {
                    string query = "SELECT * FROM gg";
                    conn.Open(); // 打开数据库连接
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader(); // 执行查询并获取数据阅读器

                    while (reader.Read()) // 逐行读取数据
                    {
                        gonggaoLabel.Text = reader["gonggao"].ToString();
                    }
                    reader.Close(); // 关闭阅读器
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message); // 显示错误信息
                }
            }
        }
        #endregion
    }
}