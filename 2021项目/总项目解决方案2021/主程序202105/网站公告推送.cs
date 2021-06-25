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

namespace 主程序202105
{
    public partial class 网站公告推送 : Form
    {
        public 网站公告推送()
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
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

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData)
        {
            try
            {
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                // request.ContentType = "application/x-www-form-urlencoded";
                
                request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", "");

                request.Referer = "";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding("utf-8"));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
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

       
        public string getuids()
        {
            StringBuilder sb = new StringBuilder();
            string url = "http://wxpusher.zjiecode.com/api/fun/wxuser/v2?appToken=AT_Zwbx5uVZTIpxJ2OPaCGXXOZNuiWHmTKQ&page=1";
          
            string html = GetUrl(url,"utf-8");

           MatchCollection uids = Regex.Matches(html, @"""uid"":""([\s\S]*?)""");
            foreach (Match item in uids)
            {
                sb.Append("\""+item.Groups[1].Value+"\""+",");

            }

            return sb.ToString().Remove(sb.ToString().Length-1,1) ;
        }


        List<string> titleslist= new List<string>();
        public void sendmsg(string title, string neirong)
        {
            if (title.Trim() != "")
            {
                if (!titleslist.Contains(title.Trim()))
                {
                    titleslist.Add(title.Trim());
                    string uids = getuids();
                    string url = "http://wxpusher.zjiecode.com/api/send/message";
                    string postdata = "{\"appToken\":\"AT_Zwbx5uVZTIpxJ2OPaCGXXOZNuiWHmTKQ\",\"content\":\"" + title + neirong + "\",\"contentType\":1,\"uids\":[" + uids + "]}";
                    string html = PostUrl(url, postdata);

                    textBox1.Text += DateTime.Now.ToString("HH:mm:ss") + "：出现新公告，已发送" + "\r\n";
                }
            }
        }


        string huobititle = "";
        string okextitle = "";
        string binancezhtitle = "";
        string gateiotitle = "";
        string mxctitle = "";

        #region  huobi
        public void huobi()
        {

            try
            {
                string url =textBox2.Text.Trim();
                string html = GetUrl(url, "utf-8");
                Match title = Regex.Match(html, @"title:""([\s\S]*?)""([\s\S]*?)id:([\s\S]*?)}");
                string link = "https://www.huobi.pe/support/zh-cn/detail/" + title.Groups[3].Value;
                if (huobititle == "")
                {
                    huobititle = title.Groups[1].Value;
                    textBox1.Text += DateTime.Now.ToString("HH:mm:ss") + "：正在监控...huobi未出现新公告" + "\r\n";
                    
                }
                else
                {
                    if (huobititle == title.Groups[1].Value)
                    {
                        textBox1.Text += DateTime.Now.ToString("HH:mm:ss") + "：正在监控...huobi未出现新公告" + "\r\n";
                    }
                    else
                    {
                        textBox1.Text = "货币出现新公告";
                        sendmsg(title.Groups[1].Value,link );
                        huobititle = title.Groups[1].Value;
                    }
                }
            }
            catch (Exception ex)
            {

               textBox1.Text+=ex.ToString()+"\r\n";
            }
        }

        #endregion

        #region  okex
        public void okex()
        {
            try
            {
                string url = textBox3.Text.Trim();
                string html = GetUrl(url, "utf-8");
               string title = Regex.Match(html, @"class=""article-list-link"">([\s\S]*?)<").Groups[1].Value;
                string link = "https://www.okex.win/support"+Regex.Match(html, @"<li class=""article-list-item([\s\S]*?)<a href=""([\s\S]*?)""").Groups[2].Value;
             
                if (okextitle == "")
                {
                    okextitle = title;
                    textBox1.Text += DateTime.Now.ToString("HH:mm:ss") + "：正在监控...okex未出现新公告" + "\r\n";
                   
                }
                else
                {
                    if (okextitle == title)
                    {
                        textBox1.Text += DateTime.Now.ToString("HH:mm:ss") + "：正在监控...okex未出现新公告" + "\r\n";
                    }
                    else
                    {
                        textBox1.Text = "okex出现新公告";
                        sendmsg(title, link);
                        okextitle = title;
                    }
                }
            }
            catch (Exception ex)
            {

                textBox1.Text += ex.ToString() + "\r\n";
            }
        }

        #endregion

        #region  binance
        public void binance()
        {
            try
            {
                string url = textBox4.Text.Trim();
                string html = GetUrl(url, "utf-8");
             
                string title = Regex.Match(html, @"class=""css-1ej4hfo"">([\s\S]*?)<").Groups[1].Value;
                string link = "https://www.binancezh.co/zh-CN/support/announcement/" + Regex.Match(html, @"<a data-bn-type=""link"" href=""/zh-CN/support/announcement/([\s\S]*?)""").Groups[1].Value;
                //MessageBox.Show(title);
                //MessageBox.Show(link);
                if (binancezhtitle == "")
                {
                    binancezhtitle = title;
                    textBox1.Text += DateTime.Now.ToString("HH:mm:ss") + "：正在监控...binancezh未出现新公告" + "\r\n";
                   
                }
                else
                {
                    if (binancezhtitle == title)
                    {
                        textBox1.Text += DateTime.Now.ToString("HH:mm:ss") + "：正在监控...binancezh未出现新公告" + "\r\n";
                    }
                    else
                    {
                        textBox1.Text = "binance出现新公告";
                        sendmsg(title, link);
                        binancezhtitle = title;
                    }
                }
            }
            catch (Exception ex)
            {

                textBox1.Text += ex.ToString() + "\r\n";
            }
        }

        #endregion
     
        #region  gateio
        public void gateio()
        {
            try
            {
                string url = textBox5.Text.Trim();
                string html = GetUrl(url, "utf-8");
                string title = Regex.Match(html, @"<h3>([\s\S]*?)<").Groups[1].Value;
                string link = "https://www.gateio.rocks" + Regex.Match(html, @"<div class=""entry"">([\s\S]*?)<a href=""([\s\S]*?)""").Groups[2].Value;

             
                if (gateiotitle == "")
                {
                    gateiotitle = title;
                    textBox1.Text += DateTime.Now.ToString("HH:mm:ss") + "：正在监控...gateio未出现新公告" + "\r\n";
                    
                }
                else
                {
                    if (gateiotitle == title)
                    {
                        textBox1.Text += DateTime.Now.ToString("HH:mm:ss") + "：正在监控...gateio未出现新公告" + "\r\n";
                    }
                    else
                    {
                        textBox1.Text = "gateio出现新公告";
                        sendmsg(title, link);
                        gateiotitle = title;
                    }
                }
            }
            catch (Exception ex)
            {

                textBox1.Text += ex.ToString() + "\r\n";
            }
        }

        #endregion

        #region  mxc
        public void mxc()
        {
            try
            {
                string url = textBox6.Text.Trim();
                string html = GetUrl(url, "utf-8");
                string title = Regex.Match(html, @"article-list-link"">([\s\S]*?)<").Groups[1].Value;
                string link = "https://support.mxc-exchange.com/hc/zh-cn/articles/" + Regex.Match(html, @"<a href=""/hc/zh-cn/articles/([\s\S]*?)""").Groups[1].Value;

               
                if (mxctitle == "")
                {
                    mxctitle = title;
                    textBox1.Text += DateTime.Now.ToString("HH:mm:ss") + "：正在监控...mxc-exchange未出现新公告" + "\r\n";
                  
                }
                else
                {
                    if (mxctitle == title)
                    {
                        textBox1.Text += DateTime.Now.ToString("HH:mm:ss") + "：正在监控...mxc-exchange未出现新公告" + "\r\n";
                    }
                    else
                    {
                        textBox1.Text = "mxc出现新公告";
                        sendmsg(title, link);
                        mxctitle = title;
                    }
                }
            }
            catch (Exception ex)
            {

                textBox1.Text += ex.ToString() + "\r\n";
            }
        }

        #endregion

        private void 网站公告推送_Load(object sender, EventArgs e)
        {
            System.Net.WebClient webClient = new System.Net.WebClient();
            System.IO.Stream stream = webClient.OpenRead(@"https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=gQFd8TwAAAAAAAAAAS5odHRwOi8vd2VpeGluLnFxLmNvbS9xLzAyLTJIdFVLSVFjWWoxLWNiNmh3YzkAAgSMPp9gAwQAjScA");
            this.pictureBox1.Image = Image.FromStream(stream);
            stream.Dispose();
        }

        Thread thread1;
        Thread thread2;
        Thread thread3;
        Thread thread4;
        Thread thread5;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测


            //string html = GetUrl("http://www.acaiji.com:8080/api/vip.html", "utf-8");

            //if (!html.Contains(@"YYOaO6"))
            //{

            //    return;
            //}

            #endregion
            timer1.Start();
            //if (thread1 == null || !thread1.IsAlive)
            //{
            //    thread1 = new Thread(huobi);
            //    thread1.Start();
            //    Control.CheckForIllegalCrossThreadCalls = false;
            //}
            //if (thread2 == null || !thread2.IsAlive)
            //{
            //    thread2 = new Thread(okex);
            //    thread2.Start();

            //}
            if (thread3 == null || !thread3.IsAlive)
            {
                thread3 = new Thread(binance);
                thread3.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }
            //if (thread4 == null || !thread4.IsAlive)
            //{
            //    thread4 = new Thread(gateio);
            //    thread4.Start();

            //}
            //if (thread5 == null || !thread5.IsAlive)
            //{
            //    thread5 = new Thread(mxc);
            //    thread5.Start();

            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
          
            //sendmsg("测试1","http://www.acaiji.com");
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thread1 == null || !thread1.IsAlive)
            {
                thread1 = new Thread(huobi);
                thread1.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            if (thread2 == null || !thread2.IsAlive)
            {
                thread2 = new Thread(okex);
                thread2.Start();

            }
            if (thread3 == null || !thread3.IsAlive)
            {
                thread3 = new Thread(binance);
                thread3.Start();

            }
            if (thread4 == null || !thread4.IsAlive)
            {
                thread4 = new Thread(gateio);
                thread4.Start();

            }
            if (thread5 == null || !thread5.IsAlive)
            {
                thread5 = new Thread(mxc);
                thread5.Start();

            }
        }
    }
}
