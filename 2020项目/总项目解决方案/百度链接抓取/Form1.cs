using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 百度链接抓取
{
    public partial class Form1 : Form
    {
        public Form1()
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


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "session-id=144-7450304-7580635; session-id-time=2082787201l; ubid-main=133-9411273-8184314; x-wl-uid=1AR+eCj1iY57TRhM7A2m5KF9SEb1ho13Om87l60jAFJAp10qHX8GNgnZcOFTknCbmNkftPnMho/k=; aws-priv=eyJ2IjoxLCJldSI6MCwic3QiOjB9; aws-target-static-id=1536650638823-915613; s_fid=16BD3861C3483809-386224FB67B4E94E; regStatus=pre-register; s_dslv=1536656308918; i18n-prefs=USD; lc-main=zh_CN; sp-cdn=\"L5Z9: CN\"; session-token=/8/yst6nJSzUghSOya1omO6MEhQ/Moyyq2FsFStf5zcm4cZPhl38RIpfC+UZyiw//J9HubG+McoZMSB4hRyykQZ0SH1X07eSi5nxcOjmHQshqSmCJD6tL8cgFOFCByRnF1EJMjmxRfVwTkZZ/4yLqjzBQ2Ik6WclU4tG1u7+4UCFeGDYa//WLb3fCGfB6RuU; csm-hit=tb:DT2JH7KAE9BTWY50PJA8+s-DT2JH7KAE9BTWY50PJA8|1585472314824&t:1585472314824&adb:adblk_no";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://www.amazon.com/s?k=6Q0+959+856&__mk_zh_CN=%E4%BA%9A%E9%A9%AC%E9%80%8A%E7%BD%91%E7%AB%99&ref=nb_sb_noss";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("sec-fetch-mode:navigate");
                headers.Add("sec-fetch-site:same-origin");
                headers.Add("sec-fetch-user:?1");
                headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                // request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();


                reader.Close();
                response.Close();
                return content;



            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            //webBrowser1.Navigate("https://www.baidu.com/");
            webBrowser1.Navigate("http://app.jiaqun8.cn/#/home/list");
           
            webBrowser1.ScriptErrorsSuppressed = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HtmlDocument dc = webBrowser1.Document;
            HtmlElementCollection es = dc.GetElementsByTagName("input");   //GetElementsByTagName返回集合
            foreach (HtmlElement e1 in es)
            {
                if (e1.GetAttribute("name") == "wd")
                {
                    e1.SetAttribute("value", textBox1.Text.Trim());
                }
              
            }

            //点击登陆

            HtmlElementCollection es2 = dc.GetElementsByTagName("input");   //GetElementsByTagName返回集合
            foreach (HtmlElement e1 in es2)
            {
                if (e1.GetAttribute("id") == "su")
                {
                    e1.InvokeMember("click");
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"baiduzhuaqu"))
            {

                geturl();

            }

            else
            {
                MessageBox.Show("验证失败");
                Environment.Exit(0);
                return;
            }


            #endregion
           
        }
        private void geturl()
        {

            System.IO.StreamReader getReader = new System.IO.StreamReader(this.webBrowser1.DocumentStream, System.Text.Encoding.GetEncoding("utf-8"));

            string html = getReader.ReadToEnd();
           
            MatchCollection urls = Regex.Matches(html, @"<H3 class=""t([\s\S]*?)href([\s\S]*?)target");
            foreach (Match url in urls)
            {
                if (url.Groups[2].Value.Contains("link"))
                {
                    textBox2.Text += url.Groups[2].Value.Replace("\"", "").Trim().Remove(0, 1) + "\r\n";
                    this.textBox2.Focus();
                    this.textBox2.Select(this.textBox2.TextLength, 0);
                    this.textBox2.ScrollToCaret();
                }
            }

           
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            //防止弹窗；
            e.Cancel = true;
            string url = this.webBrowser1.StatusText;
            this.webBrowser1.Url = new Uri(url);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HtmlDocument dc = webBrowser1.Document;


            //点击上一页

            HtmlElementCollection es2 = dc.GetElementsByTagName("a");   //GetElementsByTagName返回集合
            foreach (HtmlElement e1 in es2)
            {
                if (e1.OuterText != null)
                {
                    if (e1.OuterText.Contains("上一页"))
                    {
                        e1.InvokeMember("click");
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            HtmlDocument dc = webBrowser1.Document;


            //点击下一页

            HtmlElementCollection es2 = dc.GetElementsByTagName("a");   //GetElementsByTagName返回集合
            foreach (HtmlElement e1 in es2)
            {
                if (e1.OuterText != null)
                {
                    if (e1.OuterText.Contains("下一页"))
                    {
                        e1.InvokeMember("click");
                    }
                }
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
           

            System.Windows.Forms.SaveFileDialog objSave = new System.Windows.Forms.SaveFileDialog();
            objSave.Filter = "(*.txt)|*.txt|" + "(*.*)|*.*";

            objSave.FileName = "文件" + DateTime.Now.ToString("yyyyMMddHHmm") + ".txt";

            if (objSave.ShowDialog() == DialogResult.OK)
            {
                StreamWriter FileWriter = new StreamWriter(objSave.FileName, true); //写文件

                FileWriter.Write(textBox2.Text);//将字符串写入
                FileWriter.Close(); //关闭StreamWriter对象
            }


        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";

            HtmlElement element2 = webBrowser1.Document.CreateElement("script");
            element2.SetAttribute("type", "text/javascript");
            element2.SetAttribute("text", "function _func(){var test=document.getElementsByTagName('html')[0].innerHTML; return test}");   //这里写JS代码
            HtmlElement head = webBrowser1.Document.Body.AppendChild(element2);

            string str = webBrowser1.Document.InvokeScript("_func").ToString();
            
            textBox2.Text = str;


            //Type obj = Type.GetTypeFromProgID("ScriptControl");
            //if (obj == null) return; //单身？ 找不到对象啊，
            //object ScriptControl = Activator.CreateInstance(obj);
            //obj.InvokeMember("Language", BindingFlags.SetProperty, null, ScriptControl, new object[] { "JScript" });
            //string js = "function time(){return new Date().getTime()}";
            //obj.InvokeMember("AddCode", BindingFlags.InvokeMethod, null, ScriptControl, new object[] { js });
            //string str = obj.InvokeMember("Eval", BindingFlags.InvokeMethod, null, ScriptControl, new object[] { "time()" }).ToString();
            //MessageBox.Show(str);



        }
    }
}
