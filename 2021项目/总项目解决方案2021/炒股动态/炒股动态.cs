using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Media;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 炒股动态
{
    public partial class 炒股动态 : Form
    {
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int InternetSetCookieEx(string lpszURL, string lpszCookieName, string lpszCookieData, int dwFlags, IntPtr dwReserved);
        [DllImport("kernel32.dll")]

        public static extern bool Beep(int freq, int duration);
        public 炒股动态()
        {
            InitializeComponent();
        }

        string COOKIE = "user=MDp3YXl0b3dpbmZvY3VzOjpOb25lOjUwMDo3NjI3ODUxNTo3LDExMTExMTExMTExLDQwOzQ0LDExLDQwOzYsMSw0MDs1LDEsNDA7MSwxMDEsNDA7MiwxLDQwOzMsMSw0MDs1LDEsNDA7OCwwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMSw0MDsxMDIsMSw0MDo6Ojo2NjI3ODUxNToxNjMxMjUxNzMwOjo6MTI0NTIxNzIwMDo2MDQ4MDA6MDoxNDc0MmEzMzRlNzA0NzZhZDEwZjA1MGViOTU2ZDNjYTQ6ZGVmYXVsdF80OjA%3D; userid=66278515; u_name=waytowinfocus; escapename=waytowinfocus; ticket=badab2d162e496632d5813dd61916e7a; user_status=0; utk=eac80726c3a26a4af1ba5e752877e8c5; Hm_lvt_da7579fd91e2c6fa5aeb9d1620a9b333=1631251702,1631269151; Hm_lpvt_da7579fd91e2c6fa5aeb9d1620a9b333=1631269151; Hm_lvt_78c58f01938e4d85eaf619eae71b4ed1=1631251702,1631269151; Hm_lpvt_78c58f01938e4d85eaf619eae71b4ed1=1631269151; v=A3Kf8JZGR7abXnuKKzAIbZn3w7NXA3TIqAdqxTxKniUQzxzlJJPGrXiXurYP";
        string path = AppDomain.CurrentDomain.BaseDirectory;

        SoundPlayer player = new SoundPlayer();

        #region  获取cookie
        /// <summary>
        /// 获取cookie
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetCookies(string url)
        {
            uint datasize = 256;
            StringBuilder cookieData = new StringBuilder((int)datasize);
            if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x2000, IntPtr.Zero))
            {
                if (datasize < 0)
                    return null;


                cookieData = new StringBuilder((int)datasize);
                if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x00002000, IntPtr.Zero))
                    return null;
            }
            return cookieData.ToString();
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
     
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;

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


        private DateTime ConvertStringToDateTime(string timeStamp)
        {

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(Convert.ToDouble(timeStamp));
        }

        public string topStr = "";
        public string fontcolor = "black";
        private void ShowArticles()
        {


            string url = "http://t.10jqka.com.cn/trace/trade/getEntrust/?_=1631269150510";
            string html = GetUrl(url,"utf-8") ;
            html = Unicode2String(html);
            MatchCollection time = Regex.Matches(html, @"""time"":""([\s\S]*?)""");
            MatchCollection zqmc = Regex.Matches(html, @"""zqmc"":""([\s\S]*?)""");
            MatchCollection zqdm = Regex.Matches(html, @"""zqdm"":""([\s\S]*?)""");

            MatchCollection type = Regex.Matches(html, @"""type"":""([\s\S]*?)""");
            MatchCollection wtjg = Regex.Matches(html, @"""wtjg"":""([\s\S]*?)""");
            MatchCollection wtsl = Regex.Matches(html, @"""wtsl"":""([\s\S]*?)""");

            MatchCollection rtime = Regex.Matches(html, @"""rtime"":([\s\S]*?)}");
            StringBuilder sb = new StringBuilder();
           
            sb.AppendLine("<table width=100% style=\"font-family:Microsoft YaHei; font-size:16px;\" > ");
            sb.AppendLine("<tr>");
            sb.AppendLine("<th align=left>股票名称</th>");
            sb.AppendLine("<th align=left>代码</th>");
            //sb.AppendLine("<th align=left>盈亏率</td>");
            sb.AppendLine("<th align=left>日期</td>");
            sb.AppendLine("<th align=left>时间</td>");
            sb.AppendLine("<th align=left>交易类型</td>");
            sb.AppendLine("<th align=left>现价</td>");
            sb.AppendLine("<th align=left>股票数量</td>");
            //sb.AppendLine("<th align=left>市值</td>");
            //sb.AppendLine("<th align=left>持仓占比</td>");
            //sb.AppendLine("<th align=left>浮动盈亏额</td>");
            sb.AppendLine("</tr>");

            if (time.Count == 0)
            {
                MessageBox.Show("请重新登录");
                return;
            }
            for (int i = 0; i<time.Count;i++)
            {
                string type2 = "委托买入";
                if (type[i].Groups[1].Value == "S")
                {
                    type2 = "委托卖出";
                }

                if (topStr == "")
                {
                    topStr = zqmc[0].Groups[1].Value;
                }
                else
                {
                    if (topStr == zqmc[0].Groups[1].Value)
                    {
                        fontcolor = "black";
                        this.TopMost = false;
                        
                    }
                    else
                    {
                        fontcolor = textBox2.Text;
                        player.SoundLocation = path + "Data_Founded-1.wav";
                        player.Load();
                        player.Play();
                        // Beep(800, 800);
                        this.TopMost = true;
                        topStr = zqmc[0].Groups[1].Value;


                    }

                }
                if (i == 0)
                {
                    sb.AppendLine("<tr style=\"color:" + fontcolor + ";\">");  //第一行设置字体颜色
                }
                else
                {
                    if(i%2==0)
                    {
                        sb.AppendLine("<tr style=\"background-color:rgb(235,235,235);\">");
                    }
                    if (i % 2 == 1)
                    {
                        sb.AppendLine("<tr style=\"background-color:rgb(200,220,235);\">");
                    }
                }
                if (ConvertStringToDateTime(rtime[i].Groups[1].Value) > DateTime.Now.AddDays(-30))
                {
                    // style=\"border-bottom:1px solid #0099CC;\" 
                    sb.AppendLine("<td><span style=\"font-size:20px;\">" + zqmc[i].Groups[1].Value + "</span></td>");
                    sb.AppendLine("<td>" + zqdm[i].Groups[1].Value + "</td>");
                    sb.AppendLine("<td>" + ConvertStringToDateTime(rtime[i].Groups[1].Value).ToString("MM-dd") + "</td>");
                    sb.AppendLine("<td>" + time[i].Groups[1].Value + "</td>");
                    sb.AppendLine("<td>" + type2 + "</td>");
                    sb.AppendLine("<td>" + wtjg[i].Groups[1].Value + "</td>");
                    sb.AppendLine("<td>" + wtsl[i].Groups[1].Value + "</td>");

                    sb.AppendLine("</tr>");
                }
            }
            sb.AppendLine("</table>");
            webBrowser1.DocumentText = sb.ToString();

        }

        Thread thread;
        private void 炒股动态_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("http://t.10jqka.com.cn/trace/user/myService/");


          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"pZbH"))
            {

                return;
            }

            #endregion
            COOKIE = GetCookies("http://t.10jqka.com.cn/trace/trade/getEntrust/?_=1631269150510");
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(ShowArticles);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            label1.Text = "正在监控...";
            timer1.Interval = Convert.ToInt32(textBox1.Text)*1000;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            webBrowser1.DocumentText = "";
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(ShowArticles);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
          
            label1.Text = "未监控";
            timer1.Stop();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = colorDialog1.ShowDialog();
            //如果选中颜色，单击“确定”按钮则改变文本框的文本颜色
            if (dr == DialogResult.OK)
            {
                // textBox1.ForeColor = colorDialog1.Color;
               textBox2.Text = colorDialog1.Color.Name;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
            webBrowser1.Navigate("http://t.10jqka.com.cn/");
        }
    }
}
