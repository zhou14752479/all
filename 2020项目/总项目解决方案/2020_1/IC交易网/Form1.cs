using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IC交易网
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8","");

            if (!html.Contains(@"ictwang"))
            {

                MessageBox.Show("验证失败");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;


            }
            

            #endregion
            webBrowser1.Navigate("https://member.ic.net.cn/login.php");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HtmlDocument dc = webBrowser1.Document;
            HtmlElementCollection es = dc.GetElementsByTagName("input");   //GetElementsByTagName返回集合
            foreach (HtmlElement e1 in es)
            {
                if (e1.GetAttribute("id") == "username")
                {
                    e1.SetAttribute("value", textBox1.Text.Trim());
                }
                if (e1.GetAttribute("id") == "password")
                {
                    e1.SetAttribute("value", textBox2.Text.Trim());
                }
            }

            //点击登陆

            HtmlElementCollection es2 = dc.GetElementsByTagName("input");   //GetElementsByTagName返回集合
            foreach (HtmlElement e1 in es2)
            {
                if (e1.GetAttribute("id") == "btn_login")
                {
                    e1.InvokeMember("click");
                }

            }
        }

        
        public static string GetUrl(string Url, string charset,string COOKIE)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; 
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                 request.KeepAlive = true;
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
 

        private void button2_Click(object sender, EventArgs e)
        {

           
             HtmlElement element2 = webBrowser1.Document.CreateElement("script");
            element2.SetAttribute("type", "text/javascript");
            element2.SetAttribute("text", "function _func(){return document.cookie}");   //这里写JS代码
            HtmlElement head = webBrowser1.Document.Body.AppendChild(element2);

            string str = webBrowser1.Document.InvokeScript("_func").ToString();
            string url = "https://member.ic.net.cn/search.php?IC_Method=icsearch&key=STM32F103C8T6&isExact=0&mfg=&pack=&dc=&qty=&searchAreaCode=0&stockDate=90&stockType=0";
            textBox3.Text = GetUrl(url, "utf-8",str);

        }
    }
}
