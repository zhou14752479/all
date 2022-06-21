using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 客户文档下载
{
    public partial class 客户文档下载 : Form
    {
        public 客户文档下载()
        {
            InitializeComponent();
        }

        #region 下载文件  【好用】
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="URLAddress">图片地址</param>
        /// <param name="subPath">图片所在文件夹</param>
        /// <param name="name">图片名称</param>
        public  void downloadFile(string URLAddress, string subPath, string name, string COOKIE)
        {
            try
            {
                string path = System.IO.Directory.GetCurrentDirectory();

                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
                client.Headers.Add("Cookie", COOKIE);
               // client.Headers.Add("Referer", "https://m.mm131.net/chemo/89_5.html");
               // client.Headers.Add("authorization: Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Il9DSkFPdHlzWVZtNXhjMVlvSzBvUTdxeUJDUSJ9.eyJhdWQiOiI3MjY0MzcwNC1iMmU3LTRiMjYtYjg4MS1iZDU4NjVlN2E3YTUiLCJpc3MiOiJodHRwczovL2xvZ2luLnBhcnRuZXIubWljcm9zb2Z0b25saW5lLmNuLzQ4MDdlOWNmLTg3YjgtNDE3NC1hYTViLWU3NjQ5N2Q3MzkyYi92Mi4wIiwiaWF0IjoxNjUyNDMwMDY2LCJuYmYiOjE2NTI0MzAwNjYsImV4cCI6MTY1MjQzMzk2NiwiYWlvIjoiNDJKZ1lKakw2L3BMOUhlOGM3V0cwcGJpdTE5ekFBPT0iLCJhenAiOiJjNzMxN2Y4OC03Y2VhLTRlNDgtYWM1Ny1hMTYwNzFmN2I4ODQiLCJhenBhY3IiOiIxIiwib2lkIjoiZDQ5YmI3ODItNmRjMi00OTM5LTg2NzgtZDMzYzI2NzE5YmY5IiwicmgiOiIwLkFBQUF6LWtIU0xpSGRFR3FXLWRrbDljNUt3UTNaSExuc2laTHVJRzlXR1hucDZVQkFBQS4iLCJyb2xlcyI6WyJJRVMiXSwic3ViIjoiZDQ5YmI3ODItNmRjMi00OTM5LTg2NzgtZDMzYzI2NzE5YmY5IiwidGlkIjoiNDgwN2U5Y2YtODdiOC00MTc0LWFhNWItZTc2NDk3ZDczOTJiIiwidXRpIjoieHBMOV9kc05IRWlvZVNfX2MwY1pBQSIsInZlciI6IjIuMCJ9.G19BChYBWDHu7uoiwpZdWVSBrjEL7DkIUWlr5Z5HcQxPd2Y1F2zXw69RkZSrTtiSNg79lhASxzkwhN7HzX_sq_pRk9GR1odRBNtRctZRX9BTa-Jvsrz5e2bmUOuyEjmeHQhh8jw3IuVXIiHnWm2kqEy_yJDLmERa8IAbeBjryaBpolPogRJeIBwz-S1nqdqrTQn75VMfqPfDiJIq2OcKJIFc6YOlP8thb-hbbLbkwF4iNJUx_H-RUyG1Nbc3-jzPYbiVKYee3223OZRyZH4qF3RhG8DQCBjl5N8q6e4VpCn_zsqqcLgNDzNR4PAwtCWLEtAxpaz8XELS3x7zQnkryQ");
               // client.Headers.Add("x-auth-authtoken: eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJ3d3cudGVhbW1vZGVsLmNuIiwic3ViIjoiMTY0MDc2NjUxMyIsImF6cCI6ImNkcm1ieCIsImV4cCI6IjE2NTI0MzM5NjciLCJuYW1lIjoi6b6Z5am3IiwicGljdHVyZSI6bnVsbCwicm9sZXMiOlsidGVhY2hlciJdLCJwZXJtaXNzaW9ucyI6W10sInN0YW5kYXJkIjoic3RhbmRhcmQ0Iiwic2NvcGUiOiJ0ZWFjaGVyIiwiYXJlYSI6ImYzNWUwMDMxLWE1M2YtNDVlNS1iMzA3LTFjZDM5NDQ2YTJjZiIsIndlYnNpdGUiOiJJRVMifQ.VDyM3RkGyYIwNCPIYP1dPQKaXdowxqmIR4IJsMHDybQ");
                if (false == System.IO.Directory.Exists(subPath))
                {
                    //创建pic文件夹
                    System.IO.Directory.CreateDirectory(subPath);
                }

                client.DownloadFile(URLAddress, subPath + "\\" + name);
            }
            catch (WebException ex)
            {
               MessageBox.Show(ex.ToString());
              // textBox1.Text= ex.ToString();
            }
        }



        #endregion

        #region POST默认请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrlDefault(string url, string postData, string COOKIE,string contype)
        {
            try
            {

                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;//防止代理抓包
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("authorization: Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Il9DSkFPdHlzWVZtNXhjMVlvSzBvUTdxeUJDUSJ9.eyJhdWQiOiI3MjY0MzcwNC1iMmU3LTRiMjYtYjg4MS1iZDU4NjVlN2E3YTUiLCJpc3MiOiJodHRwczovL2xvZ2luLnBhcnRuZXIubWljcm9zb2Z0b25saW5lLmNuLzQ4MDdlOWNmLTg3YjgtNDE3NC1hYTViLWU3NjQ5N2Q3MzkyYi92Mi4wIiwiaWF0IjoxNjUyNDk4MDI2LCJuYmYiOjE2NTI0OTgwMjYsImV4cCI6MTY1MjUwMTkyNiwiYWlvIjoiNDJKZ1lQajM5WSsveG9ZcTV2L3FiTE5iTnBwVkFBQT0iLCJhenAiOiJjNzMxN2Y4OC03Y2VhLTRlNDgtYWM1Ny1hMTYwNzFmN2I4ODQiLCJhenBhY3IiOiIxIiwib2lkIjoiZDQ5YmI3ODItNmRjMi00OTM5LTg2NzgtZDMzYzI2NzE5YmY5IiwicmgiOiIwLkFBQUF6LWtIU0xpSGRFR3FXLWRrbDljNUt3UTNaSExuc2laTHVJRzlXR1hucDZVQkFBQS4iLCJyb2xlcyI6WyJJRVMiXSwic3ViIjoiZDQ5YmI3ODItNmRjMi00OTM5LTg2NzgtZDMzYzI2NzE5YmY5IiwidGlkIjoiNDgwN2U5Y2YtODdiOC00MTc0LWFhNWItZTc2NDk3ZDczOTJiIiwidXRpIjoiRmNuQkEyVzEyVWVoVUQtQWVoY3NBUSIsInZlciI6IjIuMCJ9.O-vWr0CJ6hikeIMsdZxYi5R3gQ0fEAB3zydfI7a3x_FIk9oDnTqb-Um8mg9t442Al2vAzccqNiAf8FCUzF0NUlxUIH5qsccdVM0MRZ3UeiVMfEkL61r4SzGO1Nd0TWn7USeySxsVXINXkqKYFMNZ_8Jjf0bBhgrtyRXreGA4JeqTz3TruYs07BKk0NIiYvHH7ayw-aabueIfqvHCrUgINMUOO9pZ17qygQF4IVOtvEESjGzAXqhnJsx1hz1JQLzWfXbSNpkiRBoh5J6M98efbod1aWlcvgc8xG8O-9t6QO_Q_g9k0YvhfGKBBjuumNVhI3fj8T3bNXaQ6CT12O1VkA");
                headers.Add("x-auth-authtoken: eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJ3d3cudGVhbW1vZGVsLmNuIiwic3ViIjoiMTY0MDc2NjUxMyIsImF6cCI6ImNkcm1ieCIsImV4cCI6IjE2NTI1MDE5MjgiLCJuYW1lIjoi6b6Z5am3IiwicGljdHVyZSI6bnVsbCwicm9sZXMiOlsidGVhY2hlciJdLCJwZXJtaXNzaW9ucyI6W10sInN0YW5kYXJkIjoic3RhbmRhcmQ0Iiwic2NvcGUiOiJ0ZWFjaGVyIiwiYXJlYSI6ImYzNWUwMDMxLWE1M2YtNDVlNS1iMzA3LTFjZDM5NDQ2YTJjZiIsIndlYnNpdGUiOiJJRVMifQ.1lpYA6IlTUctQLinzq9xGQ-aDBtJH4Uk7QmIkJ6nQ8o");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                request.ContentType = contype;
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                //request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = url;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }


                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion

        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlWithCookie(string Url, string COOKIE, string charset)
        {
            string html = "";

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                // request.Proxy = null;//防止代理抓包
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈


                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {

                return ex.ToString();

            }

        }
        #endregion


        #region  我的下载
       
        public void myrun()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string cookie = textBox2.Text.Trim();

            try
            {
                StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\page.txt",Encoding.Default);
                //一次性读取完 
                string lastpage = sr.ReadToEnd();
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存

                for (int page = Convert.ToInt32(lastpage); page < 19107; page++)
                {
                    FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\page.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                    sw.WriteLine(page);
                    sw.Close();
                    fs1.Close();
                    sw.Dispose();

                    string url = "http://www.bcvet.cn/admin/rest/resource/resourceList/dataList";
                    string postdata = "{\"pageNo\":"+page+",\"title\":\"\",\"sortBy\":\"create_time\",\"sortDesc\":\"desc\",\"typeList\":[{\"value\":\"all\",\"typeId\":\"resource_type\"},{\"value\":\"\",\"typeId\":\"STUDYSUBJ\"},{\"value\":\"\",\"typeId\":\"STUDYGRADE\"},{\"value\":\"\",\"typeId\":\"region\"}],\"ifGood\":0}";
                    string html = PostUrlDefault(url, postdata, cookie, "application/json");
                  
                    MatchCollection ids = Regex.Matches(html, @"""id"":""([\s\S]*?)""");
                   
                    for (int i = 0; i < ids.Count; i++)
                    {
      
                     
                        string sPath = path  + "//我的文档//";
                        if (!Directory.Exists(sPath))
                        {
                            Directory.CreateDirectory(sPath); //创建文件夹
                        }

                        string aurl = "http://www.bcvet.cn/admin/rest/resource/info/view?id="+ids[i].Groups[1].Value+"&_=1652512165493";

                        string ahtml = GetUrlWithCookie(aurl,cookie,"utf-8");

                        string downurl =  Regex.Match(ahtml, @"""fastdfsPath"":""([\s\S]*?)""").Groups[1].Value ;
                        string title = Regex.Match(ahtml, @"""title"":""([\s\S]*?)""").Groups[1].Value.ToLower();
                        string filesize = Regex.Match(ahtml, @"""fileSize"":""([\s\S]*?)""").Groups[1].Value.ToLower();


                        textBox1.Text += "正在下载：页码：  " + page + "------ " + title+"\r\n" ;
                        if (downurl == "")
                        {
                           // MessageBox.Show("下载地址为空");
                            
                            continue;
                        }

                        if (filesize!="")
                        {
                            //超过30MB不下载
                            if(Convert.ToDouble(filesize)>30720)
                            {
                                textBox1.Text += title + "文件超过30MB" + "\r\n";
                                continue;
                            }
                          
                        }

                        if (title.Contains("png")|| title.Contains("jpg")|| title.Contains("jpeg")|| title.Contains("mp4") || title.Contains("mp3") || title.Contains("zip") || title.Contains("rar"))
                        {
                            textBox1.Text += title + "格式不符合" + "\r\n";
                            continue;
                        }
                        downloadFile("http://www.bcvet.cn/" + downurl, sPath, title, cookie);
                        
                    }

                }



            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            MessageBox.Show("完成");

        }

        #endregion

        #region  福建幼儿园
        //福建幼儿园
        //https://hep.edueva.org/Project/TaskIndex/7fee383c127e4b86b291ad6400ff63d9?NavTo=11
        public void run_fujianyoueryuan()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "//福建幼儿园//";
            string cookie = "";

            try
            {

                for (int page = 1; page < 34; page++)
                {

                    string url = "https://hep.edueva.org/Project/WrzPaged";
                    string postdata = "prjId=7fee383c127e4b86b291ad6400ff63d9&pageNo=" + page;
                    string html = PostUrlDefault(url, postdata, cookie, "application/x-www-form-urlencoded");

                    MatchCollection ids = Regex.Matches(html, @"showDetail\('([\s\S]*?)'");
                    MatchCollection titles = Regex.Matches(html, @"float:left;"">([\s\S]*?)</span>");

                   
                    for (int i = 0; i < ids.Count; i++)
                    {


                        textBox1.Text += "正在下载：" + titles[(3 * i)].Groups[1].Value + "\r\n";
                        string aurl = "https://hep.edueva.org/Project/WrzDetail/" + ids[i].Groups[1].Value;

                        string sPath = path + titles[(3 * i)].Groups[1].Value + "//" + titles[(3 * i) + 1].Groups[1].Value;
                        if (!Directory.Exists(sPath))
                        {
                            Directory.CreateDirectory(sPath); //创建文件夹
                        }
                        string ahtml = GetUrlWithCookie(aurl, cookie, "utf-8");

                        MatchCollection fileNames = Regex.Matches(ahtml, @"<div class=""layer_resMaterialUrls"">([\s\S]*?)"">([\s\S]*?)</a>");
                        MatchCollection fileUrls = Regex.Matches(ahtml, @"<div class=""layer_resMaterialUrls"">([\s\S]*?)<a href=""([\s\S]*?)""");

                        for (int a = 0; a < fileNames.Count; a++)
                        {
                            string downurl = fileUrls[a].Groups[2].Value;
                            if (!fileUrls[a].Groups[2].Value.Contains("http"))
                            {
                               string mp4key= Regex.Match(fileUrls[a].Groups[2].Value, @"WrzPlay/([\s\S]*?)_").Groups[1].Value;
                                downurl = "https://dpv.videocc.net/a2cf165d12/"+mp4key.Substring(mp4key.Length-1,1)+"/"+mp4key+"_1.mp4";
                            }

                          
                            downloadFile(downurl, sPath, fileNames[a].Groups[2].Value, cookie);
                        }
                    }

                }



            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            MessageBox.Show("优秀认证材料完成");

        }

        #endregion

        #region  福建幼儿园教师互评
        //福建幼儿园
        //https://hep.edueva.org/Project/TaskIndex/7fee383c127e4b86b291ad6400ff63d9?NavTo=11
        public void run_fujianyoueryuan_2()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "//福建幼儿园//";

            string cookie = "ASP.NET_SessionId=ynlcnaigsaftaqi20gm4zqvc; HepGuoPei=3885655a2c0c4c049e5dae1a00fcc4e5_18876318231_2; SERVERID=df9135ae2d6a9d609812b71c66db3b03|1652059155|1652058259";

            try
            {



                string url = "https://hep.edueva.org/PrjStudent/FindWrzReview";
                string postdata = "projectId=7fee383c127e4b86b291ad6400ff63d9&stateId=0&mySchool=1&fieldList%5B%5D=988994d4f3e24e878b1cace900fae724&fieldList%5B%5D=0817afd1c1e24cd98676ace900fe1a24&fieldList%5B%5D=8cef423e4ac4447a99c4ace901013d82&fieldList%5B%5D=02ef6b780c0049c2ad92ace90101be3a&fieldList%5B%5D=e5f9232c67cd41039545ace9010260a4&fieldList%5B%5D=9ec4307ef38f40bdb046ace90102d6df&fieldList%5B%5D=7b6a84d63997405b8c81ace90103597f&fieldList%5B%5D=ee696146372147279702ace901044283&fieldList%5B%5D=7c2f433f30644db99b7bace90104b069&fieldList%5B%5D=468805d79d56451db1e5ace901051031&fieldList%5B%5D=18956ee7ea674bc1906dace90105a3e7&fieldList%5B%5D=049e2a9753714e108a2bace90106005a&fieldList%5B%5D=ee36a8337efd4cdc9308ace901065f33&fieldList%5B%5D=50558e59b2dd4abfabd1ace90106ca5e&fieldList%5B%5D=b2e78fa1232f48f4b265ace9010c87fd&fieldList%5B%5D=beaa89d2f6184b489265ace9010e2c8a&fieldList%5B%5D=c03e5a299b06416f9996ace901164025&pageIndex=1&timeId=1652058455157";
                string html = PostUrlDefault(url, postdata, cookie, "application/x-www-form-urlencoded");

                MatchCollection ids = Regex.Matches(html, @"editExamWrz\('([\s\S]*?)'");
                MatchCollection titles = Regex.Matches(html, @"<div class=""mutualItemTit"">([\s\S]*?)</div>");
                MatchCollection subUserNames = Regex.Matches(html, @"<div class=""mutualItem-l"">([\s\S]*?)</p>");


                for (int i = 0; i < ids.Count; i++)
                {

                    string author = Regex.Replace(subUserNames[i].Groups[1].Value, "<[^>]+>", "").Trim();
                    textBox1.Text += "正在下载：" + titles[i].Groups[1].Value + "\r\n";

                    string sPath = path + titles[i].Groups[1].Value + "//" + author;
                    if (!Directory.Exists(sPath))
                    {
                        Directory.CreateDirectory(sPath); //创建文件夹
                    }
                    string aurl = "https://hep.edueva.org/PrjStudent/EditWrzReview/" + ids[i].Groups[1].Value;

                    string ahtml = GetUrlWithCookie(aurl, cookie, "utf-8");

                    MatchCollection fileNames = Regex.Matches(ahtml, @"<div class=""layer_resMaterialUrls"">([\s\S]*?)"">([\s\S]*?)</a>");
                    MatchCollection fileUrls = Regex.Matches(ahtml, @"<div class=""layer_resMaterialUrls"">([\s\S]*?)<a href=""([\s\S]*?)""");

                    for (int a = 0; a < fileNames.Count; a++)
                    {
                        string downurl = fileUrls[a].Groups[2].Value;
                        if (!fileUrls[a].Groups[2].Value.Contains("http"))
                        {
                            string mp4key = Regex.Match(fileUrls[a].Groups[2].Value, @"WrzPlay/([\s\S]*?)_").Groups[1].Value;
                            downurl = "https://dpv.videocc.net/a2cf165d12/a/" + mp4key + "_1.mp4";
                        }


                        downloadFile(downurl, sPath, fileNames[a].Groups[2].Value, cookie);
                    }
                }



            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            MessageBox.Show("教师互评完成");

        }

        #endregion

        #region  安徽幼儿园
        //福建幼儿园
        //http://www.bcvet.cn/ability/result/eval?projectId=2c914c0a7f366d88017f72cd656b6cf5
        public void run_anhuiyoueryuan()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "//安徽幼儿园//";

            string cookie = "manageAuth=false; uid=NP_qGKMGMQZudoH4971PExb4HI-EB2M6sB5zHD7LDjM0w24q54gDaMyPJR04Dxpum9wg6UcCFzmcizLfl0FyLPh9OoKFnOyPCMAv5Ee_vqWzwLBsrytWFHpVngnb7d6iKj7lhUP2BxZd4RFyeuo8o9h73SfGtXaaI3Me3VeVA2I2iiPJz0aBd852-pjda30pSq0eqtSMmC_IEhXJ6ON2IENljowjsTdB7Q4KUWvi-Y0.; userId=2c914c0a7f97335b017fb120deba34d7; userName=%E7%94%B0%E6%B5%B7%E8%89%B3; acw_tc=2760777f16520565451508410e326110e6a6e376c0652077736b8d21cbc66f; Hm_lvt_b049ea6874eee4b0ae0f7df4b5f89f28=1652014936,1652018588,1652056551; Hm_lpvt_b049ea6874eee4b0ae0f7df4b5f89f28=1652056551; SERVERID=334f4995e3177bff14703bd5a561e933|1652056553|1652056545";

            try
            {

                for (int page = 1; page < 226; page++)
                {

                    string url = "http://www.bcvet.cn/cep-abs/ability/2c914c0a7f366d88017f72cd656b6cf5/my/eval/list?projectId=2c914c0a7f366d88017f72cd656b6cf5&pageNum="+page+"&pageSize=100";
                  
                    string html = GetUrlWithCookie(url,cookie,"utf-8");

                    MatchCollection ids = Regex.Matches(html, @"resultId"":([\s\S]*?),");
                    MatchCollection titles = Regex.Matches(html, @"""name"":""([\s\S]*?)""");
                    MatchCollection subUserNames = Regex.Matches(html, @"""subUserName"":""([\s\S]*?)""");

                
                    for (int i = 0; i < ids.Count; i++)
                    {


                        textBox1.Text += "正在下载：" + titles[i].Groups[1].Value + "\r\n";
                        string aurl = "http://www.bcvet.cn/cep-abs/ability/result/" + ids[i].Groups[1].Value+"/eval" ;

                        string sPath = path + titles[i].Groups[1].Value + "//" + subUserNames[i].Groups[1].Value;
                        if (!Directory.Exists(sPath))
                        {
                            Directory.CreateDirectory(sPath); //创建文件夹
                        }
                        string ahtml = GetUrlWithCookie(aurl, cookie, "utf-8");

                        MatchCollection fileNames = Regex.Matches(ahtml, @"""fileName"":""([\s\S]*?)""");
                        MatchCollection fileUrls = Regex.Matches(ahtml, @"""downloadUrl"":""([\s\S]*?)""");

                        for (int a = 0; a < fileNames.Count; a++)
                        {
                            string downurl = fileUrls[a].Groups[1].Value;
                           
                            downloadFile(downurl, sPath, fileNames[a].Groups[1].Value, cookie);
                        }
                    }



                }
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            MessageBox.Show("完成");

        }

        #endregion


        #region 校本-福建
        //福建幼儿园
        //https://hep.edueva.org/Project/TaskIndex/7fee383c127e4b86b291ad6400ff63d9?NavTo=11
        public void run_xiaoben_fujian()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "//校本福建//";

            string cookie = "c=HMImeFhN-1652066429893-801118a5032cf-840509130; _fmdata=bTnVzj%2Bv7r4zVfiJTkoApAy9wdA%2BS7njfavhwr96e2V5UDxYTy7t6UZ5hRFgYDNMU%2BI0A6iXB2HYlmXm%2BB%2FTXoODVghpRI2vBm6L4YFHbmI%3D; sensorsdata2015jssdkcross=%7B%22distinct_id%22%3A%22c703ab2f-4893-4962-9f37-14fc6147b6de%22%2C%22first_id%22%3A%22180a6d52a1d775-0c5fbbac1aad0e-6b3e555b-3686400-180a6d52a1edc7%22%2C%22props%22%3A%7B%22%24latest_traffic_source_type%22%3A%22%E7%9B%B4%E6%8E%A5%E6%B5%81%E9%87%8F%22%2C%22%24latest_search_keyword%22%3A%22%E6%9C%AA%E5%8F%96%E5%88%B0%E5%80%BC_%E7%9B%B4%E6%8E%A5%E6%89%93%E5%BC%80%22%2C%22%24latest_referrer%22%3A%22%22%7D%2C%22%24device_id%22%3A%22180a6d52a1d775-0c5fbbac1aad0e-6b3e555b-3686400-180a6d52a1edc7%22%7D; username=352124197908201612; connect.sid=s%3A29lWZK_-JYGs1BYDhtAWIbNS_6TR7Qdy.POtNw6IhUSQXO3rvz333m%2BDgMFnnrH5m8SgmYlzbR8o; Hm_lvt_65c62952e8f76079dda87ee4803828bd=1652066465,1652165310; TDpx=32; _xid=Jgj0eXwnQyvtqq3Y7%2BCpf6xTGg%2Bdf5K8Wx2G15gHSvp69t5tnwzMQJ4GLyLxAWZkBIM0eUidmqwb5d3f9YOGoA%3D%3D; SchoolBasic=eyJ0b2tlbiI6IkUzRDkwQUNGNzJGMjM5RTgzODQyREFFODg3NjdDMjc2MURBRkY0MDU2RDU4NjNERDc2QTBBMTk2NDE2M0I5QTVFRjBDRDc4RTE0QjQ5QzIxMzU4NEFGMDhCRkQ3NDlGMjZENDYwMzAzRjMxOEQ4MTI0QUE0QkExRDc1QTEwMEIwMjg3OUU0NEQ5QzQ0ODc2ODFGQTkzOTdCMzEwODgyOTkxQjdERUNGM0U3MEFBRjZDQ0JFOEEwRUE5NDFENjBCN0RDRjk2NUM3RTdDNDAxMERCOUZCM0YxMzcwNDJGMDg5QjBBNjEyQjZDNTlENTVFODZEMTk4NjBBREM0RENFOTQ2MzM1MDU3RjhDRDhCNDg5RERDRjRCNEJCQjhDRDM2RDIyNDEyQzEyQkM0MEYxQTJDOEMwMzhFNkMyQ0JCN0Y5NjYyNDc1N0UyMzQyMjlDOTQ2QkRCQkREMkEyMkQxMjZFM0UxNDFCQTYyMEU0QzgwRTg1QTUwNjNDRkYwQ0ZBRjE2NTZGOEI0MUVEQzU5MTE4QTk5NUQ4OTYwQ0IzMTRGQ0ZCQTU1NjEyMzUwQjRGNkI1NjI2RjZDRERFNkYxMjcyMzFFNzAzMDBCRUZGMjRCNTg1QjlGREQ5MDlDMEM1N0U0OTZERjJCNEU5MDQyOEM3Qjc4OENCOTVDMTg5RUEwNzQ2Mjc3RDQ3OEE3RDNFNUUxQUU3NDc4NkM0QUVGMjgzMDQyMDVFMTI4MEExRDg1OUMyQjkxMzJDQTJFQ0M3NkY5MjA5OUY0RTExNERBQjQ3QkI4Q0RBMkFCQzUzNURCOEFDOTEyMTMxREIwOTJCM0Q3QjQ0RUVFNjA4QjYzREM5Nzg4NkRGMzEwRjVDNUY5RkQ4ODc1MEQ5M0M2NDY2NkZEQTZDNzQ5NTE2QkNGMDQ3ODQ5RkMzM0UzRkQyMzQzMjA4RTY5MEVEM0I3MjYwN0QwQTI2MEQxOEUyQUI2ODlDMkQ4OUMxMzQ4QjZCNTIyNzJGQTRDRTA5ODUyN0YyRkM1RjEyQzdGRjc5RjAwQjY3MDY2Mzc0M0ZCMUU2MzhCQzNBMEIwNUM2M0M5RTRDMUVFMzU1M0Q0NEFDREUxMzVEQjg3MzU4OTc1OEJFQzc0MkNDQ0NDRjNFN0M4MDJGQzkyNzNEMDEyMzlCNjUzMDcxNTczMDU5MDAwNDlENjU1NTQ2M0VCOTAwQ0Y4ODZCRDQ5N0ZFODVFNUJDRTkyOEQwNzcxODlCODQ1N0E1MzlENUY3QTMzQjAxNkY3RjhBQUJEMDAwRTE4QUY4RDkyQkEzQkJEN0Q4QjI0ODk1N0ZBOUU1MzA0RUE2RDJGRUM3QkMwNkZEMEUxMUE4NTA5RDMzQkQzQjE2MEE4NkRCQTQxQTFDNkZGQjczOUQzOUNFQkI4MTI0RDEzREUxODY0OTNEOThBQzQ2NUUyREMyMjk2QzRCNjc3NzBDQTY1QjEzQzg5QTI4MjEyMDE1NUMzMjNFQkY2NjQwNUQ0NzMxRkFCMjI4ODE0REE0NzhDMUZBQkI1OTBBRkFENzVGMDgyQThCMTFGRDJENDczNDE2MjkwQzYyQTg1OUQ4OUIwOUY3Qzc4RjA4OEI3QjlBRTIzMjEzOTQ2QjgwMzNFNjM5NkI3RkUxRkNDMEQ2QzRGRkYyNDU4QTE3REEyRUYyMDM4RjE1RjkwNTA4NDVFMDNBNzZBODNBNzQ3ODcxREYyMDdGQTJCNDA3MDkxMDY3RTAzQ0QyQzQzOTUzMjdFNEIwMUVDOTg4RDk4REMzQjQ0QTNEQTg2REJERkQ5MEVERUFCNDdFNkQxN0NDNEM2RTY3QjRFMUY4NzAxMzZBRURBNTY3QzBBOTYyMzRFQjlBRkYxNzJDQjkzMThGMTJFREMyRDUxNzZCQTAyMUVERkRCNEJDMTE4NkE1RTY2REM4RURCNkVGMkM4NzExNjdCMjM0NUQyNjU1RTVDRjFGNTdGRDk3MDg5MTNDNUI0Q0Y3N0MyMDI2REUzNjQ5M0MxNkQxMzI4OUM0MUM4OUU0REY3RTY3NUVBODdDRUVDN0EzNkY5OTM5QTYxQ0YzQ0RCNkRCRThDIn0%3D; looyu_id=25aa1338d9095719b2593741bd425ac0_20003318%3A2; looyu_20003318=v%3A02a877c79dfb7695d566b7f3566f275e%2Cref%3A%2Cr%3A%2Cmon%3A//m6817.talk99.cn/monitor%2Cp0%3Ahttps%253A//passport.xiaoben365.com/; Hm_lpvt_65c62952e8f76079dda87ee4803828bd=1652165315";
            try
            {

                for (int page = 0; page < 7016; page++)
                {
                    string url = "https://studio.xiaoben365.com/assessment/teacher/getOthersAssessmentList";
                    string postdata = "{\"areaCode\":\"\",\"schoolCode\":\"\",\"abilityPointID\":\"-1\",\"state\":\"-1\",\"termID\":\"1\",\"score\":\"-1\",\"recommendstate\":\"-1\",\"limit\":10,\"offset\":"+page+"0,\"loading\":\"othersAssessmentLoading\"}";
                    string html = PostUrlDefault(url, postdata, cookie, "application/json");

                    MatchCollection ids = Regex.Matches(html, @"abilityPointEvatuationID"":([\s\S]*?),");
                    MatchCollection titles = Regex.Matches(html, @"""abilityPointName"":""([\s\S]*?)""");
                    MatchCollection subUserNames = Regex.Matches(html, @"""realName"":""([\s\S]*?)""");
                    MatchCollection hege = Regex.Matches(html, @"""scoreName"":""([\s\S]*?)""");

                    for (int i = 0; i < ids.Count; i++)
                    {
                        if (hege[i].Groups[1].Value != "优秀" && hege[i].Groups[1].Value != "合格")
                        {
                            continue;
                        }
                        string author = Regex.Replace(subUserNames[i].Groups[1].Value, "<[^>]+>", "").Trim();
                        textBox1.Text += "正在下载：" + titles[i].Groups[1].Value + "\r\n";

                        string sPath = path + titles[i].Groups[1].Value + "//" + author;
                        if (!Directory.Exists(sPath))
                        {
                            Directory.CreateDirectory(sPath); //创建文件夹
                        }
                        string aurl = "https://2021dpfj01009.xiaoben365.com/assessment/lookappraisal/" + ids[i].Groups[1].Value;

                        string ahtml = GetUrlWithCookie(aurl, cookie, "utf-8");

                        MatchCollection fileNames = Regex.Matches(ahtml, @"<i class=""fz-20([\s\S]*?)</i>([\s\S]*?)</a>");
                        MatchCollection fileUrls = Regex.Matches(ahtml, @"<a data-url=""([\s\S]*?)""");

                        for (int a = 0; a < fileNames.Count; a++)
                        {
                            string downurl = fileUrls[a].Groups[1].Value.Trim();
                            string filename = fileNames[a].Groups[2].Value.Trim();
                          
                            downloadFile(downurl.Trim(), sPath, filename, cookie);
                        }
                    }


                }


            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            MessageBox.Show("完成");

        }

        #endregion


        #region 四川金牛区
      
        public void scedu_jinniuqu()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "//四川金牛区//";

            try
            {
                string[] abilityIds = { "12846dff-02e0-4261-b076-9f76484f03e3", "f97356dd-22e5-490d-9353-353764eda51d", "beb7fc73-a562-4aea-9d2f-0b712a5941a8" };
                for (int i = 0; i < abilityIds.Length; i++)
                {
                    string mainpath = "A1";
                    if(i==0)
                    {
                        mainpath = "A1技术支持的学情分析";
                    }
                    if (i == 1)
                    {
                        mainpath = "A3演示文稿设计与制作";
                    }
                    if (i == 2)
                    {
                        mainpath = "B6技术支持的展示交流";
                    }
                    string url = "https://jinniu.teammodel.cn/research/ability/get-same-subs";
                    string postdata = "{\"tmdid\":\"1640766513\",\"school\":\"cdrmbx\",\"abilityId\":\""+ abilityIds[i] + "\"}";
                    string html = PostUrlDefault(url, postdata,"", "application/json");

                    MatchCollection ahtmls = Regex.Matches(html, @"sub"":{([\s\S]*?)self""");
                    MatchCollection subUserNames = Regex.Matches(html, @"""nickname"":""([\s\S]*?)""");
                
                    for (int j= 0; j< ahtmls.Count; j++)
                    {
                       
                        string author = Regex.Replace(subUserNames[j].Groups[1].Value, "<[^>]+>", "").Trim();
                        textBox1.Text += "正在下载：" + mainpath+ "\r\n";

                        string sPath = path + mainpath + "//" + author;
                        if (!Directory.Exists(sPath))
                        {
                            Directory.CreateDirectory(sPath); //创建文件夹
                        }
                       
                        string ahtml = ahtmls[j].Groups[1].Value;

                        MatchCollection fileNames = Regex.Matches(ahtml, @"""name"":""([\s\S]*?)""");
                        MatchCollection fileUrls = Regex.Matches(ahtml, @"""url"":""([\s\S]*?)""");

                        for (int a = 0; a < fileNames.Count; a++)
                        {
                            string downurl = fileUrls[a].Groups[1].Value.Trim()+ "?sv=2021-04-10&st=2022-05-13T08%3A11%3A07Z&se=2022-05-14T08%3A41%3A07Z&sr=c&sp=rwl&sig=pI%2FrKYGkRouuv5MWmIEO1%2FeqUWHHn0DCI7AJrGq0e6k%3D&timestamp=1652432953074";
                            string filename = fileNames[a].Groups[1].Value.Trim();
                           
                            downloadFile(downurl.Trim(), sPath, filename, "ARRAffinity=626144c37b560762f9f3f964c3d77bfad8f40bb755dd4b516be824fd798e6774; ARRAffinitySameSite=626144c37b560762f9f3f964c3d77bfad8f40bb755dd4b516be824fd798e6774");
                        }
                    }


                }


            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            MessageBox.Show("完成");

        }

        #endregion
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                //福建幼儿园350628198908185048

                Thread thread = new Thread(run_fujianyoueryuan);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;


                Thread thread1 = new Thread(run_fujianyoueryuan_2);
                thread1.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }

            if (radioButton2.Checked == true)
            {
                //福建幼儿园350628198908185048
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run_anhuiyoueryuan);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }

            if (radioButton3.Checked == true)
            {
                //校本
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run_xiaoben_fujian);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }

            if (radioButton4.Checked == true)
            {
                //四川金牛区
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(scedu_jinniuqu);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
        }

        private void 客户文档下载_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void 客户文档下载_Load(object sender, EventArgs e)
        {
            #region 通用检测


            string html = GetUrlWithCookie("http://acaiji.com/index/index/vip.html","", "utf-8");

            if (!html.Contains(@"2y2Z5"))
            {

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            #endregion
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(myrun);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }
    }
}
