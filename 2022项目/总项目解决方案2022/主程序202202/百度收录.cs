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

namespace 主程序202202
{
    public partial class 百度收录 : Form
    {
        public 百度收录()
        {
            InitializeComponent();
        }

        string COOKIE = "";
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public  string GetUrl(string Url, string charset)
        {
            string html = "";
          
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                //request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
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
        public string PostUrlDefault(string url, string postData)
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
       // 第一步
        public List<string> geturls()
        {
            List<string> lists = new List<string>();
            try
            {
               
                string url = "https://ziyuan.baidu.com/site/validateSites";
                string html = GetUrl(url, "utf-8");
              
                string sites = Regex.Match(html, @"list"":\[([\s\S]*?)\]").Groups[1].Value;
                string[] text = sites.Split(new string[] { "," }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {

                    lists.Add(text[i].Replace("\\", "").Replace("\"", ""));
                }
                return lists;

            }
            catch (Exception ex)
            {

                return lists;
            }
        }

        //第二步
        public List<string> getsites(string site)
        {
            List<string> lists = new List<string>();
            try
            {
                
                string url = "https://ziyuan.baidu.com/linksubmit/sitemaplist?site=" + System.Web.HttpUtility.UrlEncode(site);
                string html = GetUrl(url, "utf-8");
               
                MatchCollection uid = Regex.Matches(html, @"""uid"":""([\s\S]*?)""");
                for (int i = 0; i < uid.Count; i++)
                {
                    lists.Add(uid[i].Groups[1].Value);
                }
                return lists;
            }
            catch (Exception ex)
            {
                
              
                return lists;
            }
        }

       

        public void update()
        {
            try
            {

                textBox2.Text = "开始启动";
                List<string> sites = geturls();
                foreach (string site in sites)
                {
                    Thread.Sleep(Convert.ToInt32(textBox4.Text) * 1000);
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        List<string> uids = getsites(site);
                        foreach (string uid in uids)
                        {
                            sb.Append("ids%5B%5D=" + uid + "&");
                        }

                        string url = "https://ziyuan.baidu.com/linksubmit/update?site=" + System.Web.HttpUtility.UrlEncode(site);
                        string postdata = sb.ToString().Remove(sb.ToString().Length - 1, 1);
                        string html = PostUrlDefault(url, postdata);

                        textBox2.Text = DateTime.Now.ToString() + "：" + html;
                       
                    }
                    catch (Exception)
                    {

                        continue;
                    }

                }
            }
            catch (Exception ex)
            {

              
            }


        }
        private void 百度收录_Load(object sender, EventArgs e)
        {

        }

        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text=="")
            {
                MessageBox.Show("请输入COOKIE");
                return;
            }


            timer1.Start();
            COOKIE = textBox1.Text;
            if (radioButton1.Checked == true)
            {
                timer1.Interval = 100;
            }
            if (radioButton2.Checked==true)
            {
                timer1.Interval = Convert.ToInt32(textBox3.Text) * 1000 * 60;
            }


            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(update);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            COOKIE = textBox1.Text;
            if (radioButton1.Checked==true)
            {
                if (DateTime.Now == dateTimePicker1.Value)
                {
                    if (thread == null || !thread.IsAlive)
                    {
                        thread = new Thread(update);
                        thread.Start();
                        Control.CheckForIllegalCrossThreadCalls = false;
                    }
                }
            }

            if (radioButton2.Checked == true)
            {
               
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(update);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }

            }


        }
    }
}
