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

namespace 域名信息更新
{
    public partial class 域名信息更新 : Form
    {
        public 域名信息更新()
        {
            InitializeComponent();
        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
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
                request.ContentType = "application/x-www-form-urlencoded";
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


        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl_ym(string Url)
        {
            string html = "";
           
            try
            {
                string charset = "utf-8";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                request.KeepAlive = true;
                request.Headers.Add("sec-ch-ua", @""" Not A;Brand"";v=""99"", ""Chromium"";v=""100"", ""Google Chrome"";v=""100""");
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.Headers.Add("X-Requested-With", @"XMLHttpRequest");
                request.Headers.Add("sec-ch-ua-mobile", @"?0");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.127 Safari/537.36";
                request.Headers.Add("sec-ch-ua-platform", @"""Windows""");
                request.Headers.Add("Sec-Fetch-Site", @"same-origin");
                request.Headers.Add("Sec-Fetch-Mode", @"cors");
                request.Headers.Add("Sec-Fetch-Dest", @"empty");
                request.Referer = "https://whois.west.cn/00893.com";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.9");
                request.Headers.Set(HttpRequestHeader.Cookie, @"Hm_lvt_d0e33fc3fbfc66c95d9fdcc4c93a8288=1657502152; _gcl_au=1.1.10513843.1657502709; west263guid=181eadc35cd0%2D0d816b5eada431%2D6b3e555b%2D384000%2D181eadc35ce862; __root_domain_v=.west.cn; _qddaz=QD.742557502710486; PHPSESSID=7bipet0dk2f089mgdvsv3p2jv5; ads_n_tongji_ftime=2022-7-11%2010%3A31%3A10; Hm_lpvt_d0e33fc3fbfc66c95d9fdcc4c93a8288=1657506670");

               HttpWebResponse response = (HttpWebResponse)request.GetResponse();

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

        #region unicode转中文
        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        #endregion;
        public string getym()
        {
            try
            {
                string html = GetUrl(textBox1.Text,"utf-8");
                string data = Regex.Match(html, @"data"":\[([\s\S]*?)\]").Groups[1].Value;
                return data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return "";
            }
        }


        public void run(object ym)
        {
           

                string url = "https://whois.west.cn/?domain=" + ym.ToString() + "&server=&refresh=1";
                string html = GetUrl_ym(url);

                string regdate = Regex.Match(html, @"""regdate"":""([\s\S]*?)""").Groups[1].Value;
                string expdate = Regex.Match(html, @"""expdate"":""([\s\S]*?)""").Groups[1].Value;
                string status = Regex.Match(html, @"""status"":""([\s\S]*?)""").Groups[1].Value;
                status = status.Replace("http://www.icann.org/epp#clientDeleteProhibited", "");
                status = status.Replace("http://www.icann.org/epp#clientTransferProhibited", "");
                status = status.Replace("http://www.icann.org/epp#clientUpdateProhibited", "");
                status = status.Replace("https://icann.org/epp#clientDeleteProhibited", "");
                status = status.Replace("https://icann.org/epp#clientTransferProhibited", "");
                status = status.Replace("https://icann.org/epp#clientUpdateProhibited", "");
                status = status.Replace("https://icann.org/epp#ok", "");
                status = status.Replace("ok", "状态正常(ok)").Trim();
                string registrer = Regex.Match(html, @"""registrer"":""([\s\S]*?)""").Groups[1].Value;



                string datastatus = "获取成功";
                string msg = "成功";
                if (regdate == "")
                {
                    datastatus = "获取失败";
                    msg = "未提交";
                }

               
                if (datastatus == "获取成功")
                {
                    string tjurl = textBox2.Text + ym;
                    string postdata = "register_at=" + regdate + "&expire_at=" + expdate + "&remove_at=&is_beian=&is_expire=" + status + "&is_can_register=&registrar=" + registrer;
                    string tjhtml = PostUrlDefault(tjurl, postdata, "");
                    msg = Regex.Match(tjhtml, @"""msg"":""([\s\S]*?)""").Groups[1].Value;
                }


                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString());
                lv1.SubItems.Add(ym.ToString());
                lv1.SubItems.Add(datastatus);
                lv1.SubItems.Add(Unicode2String(msg));
                lv1.SubItems.Add(regdate + "-" + expdate + "-" + status + "-" + registrer);

              
            

        }


        public void main()
        {
            for (int i = 0; i < 9999999; i++)
            {
                string ym = getym();
                while (zanting == false)
                {
                    Application.DoEvents();//等待本次加载完毕才执行下次循环.
                }
                if (astatus == false)
                    return;
                string[] text = ym.Split(new string[] { "," }, StringSplitOptions.None);

                foreach (var item in text)
                {
                    Thread thread = new Thread(new ParameterizedThreadStart(run));
                    string o = item.Replace("\"", "").Trim();
                    thread.Start((object)o);
                    Control.CheckForIllegalCrossThreadCalls = false;
                }

                Thread.Sleep(2000);
            }
        }

        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            astatus = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(main);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }



        }
        bool zanting = true;
        bool astatus = false;
        private void button3_Click(object sender, EventArgs e)
        {
            astatus = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
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
        private void 域名信息更新_Load(object sender, EventArgs e)
        {
            #region 通用检测


            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"e7aG"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion
        }

        private void 域名信息更新_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
