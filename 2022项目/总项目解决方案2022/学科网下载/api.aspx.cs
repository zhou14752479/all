using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace 学科网下载
{
    public partial class api : System.Web.UI.Page
    {
        public static string constr = "Host =localhost;Database=xueke;Username=root;Password=root";

        public static string cookie = "";
       
        protected void Page_Load(object sender, EventArgs e)
        {


            cookie = File.ReadAllText(Server.MapPath("~/") + "cookie.txt").Trim();

           
            string method = Request["method"];
            string key = Request["key"];
            string link = Request["link"];

            string cishu = Request["cishu"];
            string day = Request["day"];
            string vip = Request["vip"];


           
            if (method == "getfile" && link != "" && key != "")
            {
                getfile(key, link);
            }
            if (method == "getkey"  && key != "")
            {
                getkey(key, vip);
            }


        }












        #region GET请求带COOKIE
        public string GetUrlWithCookie(string Url, string COOKIE)
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


        #region 获取文件地址
        public void getfile(string key, string link)
        {
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            string query = "SELECT * FROM mykeys where mykey= '" + key + "' ";
            MySqlCommand command = new MySqlCommand(query, mycon);
            MySqlDataReader reader = command.ExecuteReader();



            if (reader.Read())
            {
                string cishu = reader["cishu"].ToString().Trim();
                string extime = reader["extime"].ToString().Trim();
                string day = reader["day"].ToString().Trim();
                string isvip = reader["isvip"].ToString().Trim();
                string fileid = Regex.Match(link, @"\d{6,}").Groups[0].Value;

                mycon.Close();
                reader.Close();


                if(fileid=="")
                {
                    Response.Write("{\"status\":\"0\",\"msg\":\"资料地址错误，请复制一篇资料网址，而不是专辑\"}");
                    return;
                }

                if (Convert.ToInt32(cishu) <= 0)
                {
                    Response.Write("{\"status\":\"0\",\"msg\":\"剩余次数不足，请联系客服\"}");
                    return;
                }

                if (extime!="")
                {
                    if (Convert.ToDateTime(extime) < DateTime.Now)
                    {
                        Response.Write("{\"status\":\"0\",\"msg\":\"当前购买的秘钥已过期！\"}");
                        return;
                    }
                   
                }

                if (isvip == "0" || isvip == "")
                {
                    if (link.Contains("zhijiao"))
                    {
                        Response.Write("{\"status\":\"0\",\"msg\":\"当前购买的权限不能下载教辅及中职资料，请拍下对应选项的秘钥！（当前账户可下：精品、特供、普通、≤5储值的资料）\"}");
                        return;
                    }
                }

                if (extime == "")
                {
                    extime =  DateTime.Now.AddDays(Convert.ToInt32(day)).ToString("yyyy-MM-dd HH:mm:ss");
                    editetime(key, extime);  //最后修改时间
                }


                string url = "https://user.zxxk.com/creator/api/v1/creator-resource/get-resource-detail?resourceId=" + fileid;

                string html = GetUrlWithCookie(url, cookie);
                string fileurl = Regex.Match(html, @"""fileUrl"":""([\s\S]*?)""").Groups[1].Value.Trim();
                string filename = Regex.Match(html, @"""fileName"":""([\s\S]*?)""").Groups[1].Value.Trim();
                string fileSize = Regex.Match(html, @"""fileSize"":([\s\S]*?),").Groups[1].Value.Trim();
                string provider = Regex.Match(html, @"""provider"":""([\s\S]*?)""").Groups[1].Value.Trim();
                string price = Regex.Match(html, @"""price"":([\s\S]*?),").Groups[1].Value.Trim();
                string shopId = Regex.Match(html, @"""shopId"":([\s\S]*?),").Groups[1].Value.Trim();

                if (isvip == "0" || isvip =="")
                {
                    if (provider.Contains("公司") || provider.Contains("店"))
                    {
                        Response.Write("{\"status\":\"0\",\"msg\":\"当前购买的权限不能下载教辅及中职资料，请拍下对应选项的商品！\"}");
                        return;
                    }

                   
                }



                if (fileurl == "")
                {
                    Response.Write("{\"status\":\"0\",\"msg\":\"下载失败，登录失效，请联系客服\"}");
                    return;
                }
                else
                {
                    int cishu_new = Convert.ToInt32(cishu) - 1;


                    string protocol = Request.Url.Scheme; // "http" 或 "https"
                    string host = Request.Url.Host;       // 域名或IP
                    int port = Request.Url.Port;
                    string jiamifileurl = protocol + "://" + host + ":" + port + "/download.aspx?filekey=" + Base64Encode(Encoding.GetEncoding("utf-8") ,fileurl);
        
                    Response.Write("{\"status\":\"1\",  \"cishu\":\"" + cishu_new + "\", \"extime\":\"" + extime + "\",   \"filename\":\"" + filename + "\",\"fileurl\":\"" + jiamifileurl + "\",\"fileSize\":\"" + fileSize + "\",\"msg\":\"下载成功,请查看浏览器下载列表\"}");
                    
                    editekey(key); //下载成功  减去次数

                }

             


            }
            else
            {
                Response.Write("{\"status\":\"0\",\"msg\":\"秘钥错误或不存在，请联系客服！\"}");
                mycon.Close();
                reader.Close();

            }



        }
        #endregion

        #region  修改次数

        public void editekey(string key)
        {

            try
            {

                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("update mykeys SET cishu = cishu - 1  where mykey='" + key + " ' ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                mycon.Close();
            }

            catch (System.Exception ex)
            {

            }
        }
        #endregion


        #region  修改时间

        public void editetime(string key,string time)
        {

            try
            {

                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

               
                
                MySqlCommand cmd = new MySqlCommand("update mykeys SET extime = '" + time+ " '  where mykey='" + key + " ' ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                mycon.Close();
            }

            catch (System.Exception ex)
            {

            }
        }
        #endregion

        #region  MD5加密
        public static string GetMD5(string txt)
        {
            string result;
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                result = sb.ToString();
            }
            return result;
        }
        #endregion

        #region  客户端购买

        public void getkey(string mykey, string vip)
        {

            try
            {
                int day = 30;
                int cishu = 999999;
                string isvip = "0";
                string extime = DateTime.Now.AddDays(day).ToString("yyyy-MM-dd HH:mm:ss");
               
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                switch (vip)
                {
                    case "0":
                       
                        isvip = "0";
                        break;

                    case "1":
                       
                        isvip = "1";
                        break;
                  


                }



                MySqlCommand cmd = new MySqlCommand("INSERT INTO mykeys (mykey,cishu,extime,day,isvip)VALUES('" + mykey + " ', '" + cishu + " ', '" + extime + " ', '" + day + " ', '" + isvip + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {

                    mycon.Close();
                    Response.Write("{\"status\":\"1\",\"msg\":\"开通成功！\"}");

                }
                else
                {
                    Response.Write("{\"status\":\"0\",\"msg\":\"开通失败，请联系客服！\"}");
                }


            }

            catch (System.Exception ex)
            {
                Response.Write("{\"status\":\"0\",\"msg\":\"开通失败，请联系客服！\"}");
            }
        }
        #endregion

        #region Base64编码
        public static string Base64Encode(Encoding encodeType, string source)
        {
            string encode = string.Empty;
            byte[] bytes = encodeType.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }
        #endregion
    }
}