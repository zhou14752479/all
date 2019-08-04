using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_8
{
    public partial class 浏览器淘宝 : Form
    {
        public 浏览器淘宝()
        {
            InitializeComponent();
        }
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);

        #region 非常重要获取当前存在浏览器的cookie，可以登陆wbbbrowser更新cookie。
        /// <summary>
        /// 非常重要获取当前存在浏览器的cookie，可以登陆wbbbrowser更新cookie。
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
        public static string COOKIE;

        private void 浏览器淘宝_Load(object sender, EventArgs e)
        {
            #region 通用导出

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "7.7.7.7")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {
                

            }
            else
            {
                MessageBox.Show("请登录您的账号！");
                System.Diagnostics.Process.Start("http://www.acaiji.com");
                this.Close();
                return;
            }
            #endregion



            webBrowser1.Navigate("https://www.taobao.com");

            this.webBrowser1.ScriptErrorsSuppressed = true;  //屏蔽IE脚本弹出错误
            this.webBrowser1.DocumentCompleted += WebBrowser1_DocumentCompleted;  //屏蔽IE脚本弹出错误
            method.SetWebBrowserFeatures(method.IeVersion.IE11);  //设置浏览器版本为枚举值第一个值
        }

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.webBrowser1.Document.Window.Error += OnWebBrowserDocumentWindowError;
        }
        private void OnWebBrowserDocumentWindowError(object sender, HtmlElementErrorEventArgs e)
        {
            e.Handled = true;
        }

        private void WebBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;//防止弹窗；
            string url = this.webBrowser1.StatusText;
            this.webBrowser1.Url = new Uri(url); //打开链接
            textBox1.Text = url;
        }

        private void Label1_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void Label2_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(textBox1.Text);
        }

        public void taobao()
        {
            try
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("请打开网址");
                    return;

                }

                COOKIE = GetCookies(textBox1.Text);

                textBox2.Text = "";
                string html = method.GetUrlWithCookie(textBox1.Text, COOKIE, "gb2312");

                Match title = Regex.Match(html, @"<title>([\s\S]*?)</title>");  //主图
                MatchCollection zhupics = Regex.Matches(html, @"<a href=""#""><img([\s\S]*?)src=""([\s\S]*?)jpg");  //主图
                MatchCollection prices = Regex.Matches(html, @"""price"":""([\s\S]*?)""");  //价格


                // Match  aURL = Regex.Match(html, @"descnew([\s\S]*?),");  //详情图来源网址
                string path = AppDomain.CurrentDomain.BaseDirectory + title.Groups[1].Value + "\\";

                string subPath = path + "产品介绍图\\";
                //string ahtml = method.GetUrl("http://descnew"+aURL.Groups[1].Value.Replace("'","").Replace("\"",""), "gb2312");
                //MatchCollection xqpics = Regex.Matches(ahtml, @"<img src=""([\s\S]*?)""");  //详情图


                if (false == System.IO.Directory.Exists(path))
                {
                    //创建pic文件夹
                    System.IO.Directory.CreateDirectory(path);

                    System.IO.Directory.CreateDirectory(subPath);
                }
                if (prices.Count != 0)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int a = 0; a < prices.Count; a++)
                    {
                        sb.Append(prices[a].Groups[1].Value + "   ");
                    }

                    FileStream fs2 = new FileStream(path + "价格_" + prices[0].Groups[1].Value + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw2 = new StreamWriter(fs2);
                    sw2.WriteLine(sb.ToString());
                    sw2.Close();
                    fs2.Close();
                }

                else if (prices.Count == 0)
                {
                    Match price = Regex.Match(html, @"<em class=""tb-rmb-num"">([\s\S]*?)<");  //价格

                    FileStream fs2 = new FileStream(path + "价格_" + price.Groups[1].Value + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw2 = new StreamWriter(fs2);
                    sw2.WriteLine(price.Groups[1].Value);
                    sw2.Close();
                    fs2.Close();
                }

                FileStream fs1 = new FileStream(path + "网址.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1);
                sw.WriteLine(textBox1.Text);
                sw.Close();
                fs1.Close();

              

                for (int i = 0; i < zhupics.Count; i++)
                {

                    method.downloadFile("http:" + zhupics[i].Groups[2].Value.Replace("https:","") + "jpg", subPath, i + ".jpg");
                    textBox2.Text = "正在下载主图" + i + "\r\n";
                }

                //for (int j = 0; j < xqpics.Count; j++)
                //{

                //    method.downloadFile(xqpics[j].Groups[1].Value , subPath, j + ".jpg");
                //    textBox2.Text = "正在下载详情图" + j + "\r\n";
                //}
                MessageBox.Show("下载完成");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {


            Thread thread = new Thread(new ThreadStart(taobao));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }
    }
}
