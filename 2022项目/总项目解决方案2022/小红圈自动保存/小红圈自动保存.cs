using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

namespace 小红圈自动保存
{
    public partial class 小红圈自动保存 : Form
    {
        public 小红圈自动保存()
        {
            InitializeComponent();
        }

        #region 去掉路径中非法字符
        public string removeValid(string illegal)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                illegal = illegal.Replace(c.ToString(), "");
            }
            return illegal;
        }

        #endregion
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                WebHeaderCollection headers = request.Headers;
                headers.Add("access_token:"+ access_token);
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;

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

        #region POST默认请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrlDefault(string url, string postData, string COOKIE)
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
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
               // request.ContentType = "application/x-www-form-urlencoded";
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                request.ContentType = "application/json";
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

        #region 下载文件  【好用】
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="URLAddress">图片地址</param>
        /// <param name="subPath">图片所在文件夹</param>
        /// <param name="name">图片名称</param>
        public static void downloadFile(string URLAddress, string subPath, string name, string COOKIE)
        {
            try
            {
                string path = System.IO.Directory.GetCurrentDirectory();

                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
                client.Headers.Add("Cookie", COOKIE);
                client.Headers.Add("Referer", "https://m.mm131.net/chemo/89_5.html");
                if (false == System.IO.Directory.Exists(subPath))
                {
                    //创建pic文件夹
                    System.IO.Directory.CreateDirectory(subPath);
                }

                client.DownloadFile(URLAddress, subPath + "\\" + name);
            }
            catch (WebException ex)
            {

                ex.ToString();
            }
        }



        #endregion
        string access_token = "";
        public void gettoken()
        {
            string url = "https://api.redringvip.com/api/mobile/login";
            string postdata = "{\"mobilePrefix\":\"86\",\"mobile\":\"13913343942\",\"code\":\"xzh051219\"}";
            string html = PostUrlDefault(url,postdata,"");
            access_token = Regex.Match(html, @"\\""access_token\\"":\\""([\s\S]*?)\\""").Groups[1].Value.Replace("\\", "");
            
        }
        #region unicode转中文
        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        #endregion;

        Thread thread;
        string path = AppDomain.CurrentDomain.BaseDirectory+"//result//";

        string lasttime = "1646698824";
        public void run()
        {
            try
            {
                for (int page= 1; page<999; page++)
                {

                    string url = "https://api.redringvip.com/api/content/list/get?gid=7853&plateId=1&type=1&index="+page+"&pageSize=20&lastTime="+lasttime;
                    string html = GetUrl(url, "utf-8");
                   if(html.Contains("请登录"))
                    {
                        page = page - 1;
                        gettoken();
                        continue;
                    }
                    MatchCollection cids = Regex.Matches(html, @"\],\\""cid\\"":([\s\S]*?),");
                    MatchCollection lastTimes = Regex.Matches(html, @"\\""lastTime\\"":([\s\S]*?),");
                    lasttime = lastTimes[lastTimes.Count - 1].Groups[1].Value;
                    if (cids.Count == 0)
                        break;
                    for (int i = 1; i <cids.Count; i++)
                    {

                        try
                        {
                            string cid = cids[i].Groups[1].Value;
                            //cid = "884855";
                            string aurl = "https://api.redringvip.com/api/content/detail/get?gid=7853&cid=" + cid;
                            string ahtml = GetUrl(aurl, "utf-8");
                            if (ahtml.Contains("请登录"))
                            {
                                i = i - 1;
                                gettoken();
                                continue;
                            }
                            //textBox1.Text = ahtml;
                            MatchCollection nicknames = Regex.Matches(ahtml, @"groupName\\"":\\""([\s\S]*?)\\""");

                            string title = Regex.Match(ahtml, @"\\""title\\"":\\""([\s\S]*?)\\""").Groups[1].Value.Replace("\\", "");
                            string mp3 = Regex.Match(ahtml, @"attachUrl\\"":\[\\""([\s\S]*?)\\""").Groups[1].Value.Replace("\\", "");
                            string nickname = Regex.Match(ahtml, @"myNickname([\s\S]*?)nickname\\"":\\""([\s\S]*?)\\""").Groups[2].Value.Replace("\\", "");
                            string time = Regex.Match(ahtml, @"time\\"":\\""([\s\S]*?)\\""").Groups[1].Value.Replace("\\", "");
                            string data = Regex.Match(ahtml, @"content\\"":\\""([\s\S]*?),\\""getOnlookFee").Groups[1].Value.Replace("\\", "").Replace("<br>", "").Replace("<p>t</p>", "").Replace("<p>tu00A0</p>", "").Trim();



                            string pichtml = Regex.Match(ahtml, @"picList\\"":\[([\s\S]*?)\]").Groups[1].Value.Replace("\\", "");
                            string[] pics = pichtml.Split(new string[] { "," }, StringSplitOptions.None);
                            StringBuilder sb = new StringBuilder();
                            foreach (string pic in pics)
                            {
                                if (pic != "")
                                {
                                    sb.Append("<img src=" + pic + " width=\"100%\" height=\"500\" />");
                                }
                            }
                            string comment = getcomment(cid);

                            string subtitle = "<div>" + nickname + "</div>" + "<div style=\"font-size: 12px; color:#999\">" + ConvertStringToDateTime(time).ToString("yyyy-MM-dd HH:mm") + "</div>";
                            subtitle = subtitle + DateTime.Now.ToString("yyyy年MM月dd日") + title;
                            string body = subtitle + data + "<div>" + sb.ToString() + "</div>" + comment;



                            System.IO.File.WriteAllText(path + DateTime.Now.ToString("MM月dd日HH时mm分ss秒") + removeValid(nickname) + removeValid(title) + ".html", body, Encoding.UTF8);
                            if (mp3 != "")
                            {
                                downloadFile(mp3, path, "\\" + DateTime.Now.ToString("MM月dd日HH时mm分ss秒") + removeValid(nickname) + removeValid(title) + ".m4a", "");
                            }


                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒") + nickname + title);
                            lv1.SubItems.Add(cid);
                            if (listView1.Items.Count > 2)
                            {
                                this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                            }
                            if (status == false)
                                return;
                            Thread.Sleep(1000);
                        }
                        catch (Exception ex)
                        {

                           continue;
                        }
                    }
                 
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private DateTime ConvertStringToDateTime(string timeStamp)
        {

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(Convert.ToDouble(timeStamp));
        }

        public string getcomment(string cid)
        {
            StringBuilder sb = new StringBuilder();
            string url = "https://api.redringvip.com/api/content/detail/comment/list?gid=7853&cid="+cid+"&index=1&pageSize=9999";
            string html = GetUrl(url, "utf-8");
            MatchCollection comTime = Regex.Matches(html, @"\\""comTime\\"":([\s\S]*?),");
            MatchCollection fromNickname = Regex.Matches(html, @"\\""fromNickname\\"":\\""([\s\S]*?)\\""");
            MatchCollection comContent = Regex.Matches(html, @"\\""comContent\\"":\\""([\s\S]*?)\\""");
           // textBox1.Text = html;
            for (int i = 0; i < comTime.Count; i++)
            {
                sb.Append("<div style=\"font-size: 12px;\">" + fromNickname[i].Groups[1].Value+" " + "<span style=\"font-size: 12px; color:#999\">" + ConvertStringToDateTime(comTime[i].Groups[1].Value).ToString("MM-dd HH:mm")+"</div>");
                sb.Append("<div style=\"font-size: 12px;\">" + Unicode2String(comContent[i].Groups[1].Value)+ "</div>");
            }
            return sb.ToString();
        }
        bool status = true;
        private void button1_Click(object sender, EventArgs e)
        {
            //gettoken();
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }
        #endregion
        private void 小红圈自动保存_Load(object sender, EventArgs e)
        {
            
            #region 通用检测


            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"4gSqP"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
