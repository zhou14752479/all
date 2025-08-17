using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace 学科网下载
{
    public class method
    {
        public static string constr = "Host =localhost;Database=xueke;Username=root;Password=root";

        #region GET请求带COOKIE
        public static string GetUrlWithCookie(string Url, string COOKIE)
        {
            string charset = "utf-8";
            string result;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36";
                request.Referer = Url;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("version:TYC-XCX-WX");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
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
                result = ex.ToString();

            }
            return result;
        }
        #endregion


        #region 获取Ip等信息
        public static string getip(string key,string userIp)
        {

            try
            {

                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string area = getareaFromIp(userIp);
                MySqlConnection mycon = new MySqlConnection(method.constr);
                mycon.Open();

                string sql = "INSERT INTO logs (mykey,ip,time,area)VALUES(@key, @userIp, @time, @area)";


                MySqlCommand cmd = new MySqlCommand(sql, mycon);
                cmd.Parameters.AddWithValue("@key", key);
                cmd.Parameters.AddWithValue("@userIp", userIp);
                cmd.Parameters.AddWithValue("@time", time);
                cmd.Parameters.AddWithValue("@area", area);

                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.

                mycon.Close();
                return area;

            }
            catch (Exception)
            {
                return "";

            }
        }

        public static string getareaFromIp(string ip)
        {
            string url = "https://www.ipshudi.com/" + ip + ".htm";
            string html = method.GetUrlWithCookie(url, "");
            string area = Regex.Match(html, @"归属地</td>([\s\S]*?)</span>").Groups[1].Value;
            area = Regex.Replace(area, "<[^>]+>", "").Replace(" ", "").Trim();
            return area;
        }


        #endregion


        #region 下载文件
        public static string downloadFile(string URLAddress, string subPath, string name)
        {
            try
            {
                string path = Directory.GetCurrentDirectory();
                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
               // client.Headers.Add("Cookie", COOKIE);
                //client.Headers.Add("Referer", "https://m.mm131.net/chemo/89_5.html");
                bool flag = !Directory.Exists(subPath);
                if (flag)
                {
                    Directory.CreateDirectory(subPath);
                }
                client.DownloadFile(URLAddress, subPath + "\\" + name);
                return "";
            }
            catch (WebException ex)
            {
               return ex.ToString();
            }


        }
        #endregion
    }
}