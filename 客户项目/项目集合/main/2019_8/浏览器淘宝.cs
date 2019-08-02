using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        private void 浏览器淘宝_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://www.taobao.com");

            this.webBrowser1.ScriptErrorsSuppressed = true;  //屏蔽IE脚本弹出错误
            this.webBrowser1.DocumentCompleted += WebBrowser1_DocumentCompleted;  //屏蔽IE脚本弹出错误
            method.SetIE(method.IeVersion.强制ie10);  //设置浏览器版本为枚举值第一个值
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
            webBrowser1.Navigate("https://" + textBox1.Text);
        }

        public void taobao()
        {
            string html = method.GetUrl(textBox1.Text, "gb2312");
            
            Match  title = Regex.Match(html, @"<title>([\s\S]*?)</title>");  //主图
            MatchCollection zhupics = Regex.Matches(html, @"<a href=""#""><img data-src=""([\s\S]*?)jpg");  //主图
            Match  aURL = Regex.Match(html, @"descnew([\s\S]*?)'");  //详情图来源网址
            string path = AppDomain.CurrentDomain.BaseDirectory+title.Groups[1].Value+"\\";
           
            string subPath = path + "产品介绍图\\";
            string ahtml = method.GetUrl(aURL.Groups[1].Value, "gb2312");
            MatchCollection xqpics = Regex.Matches(html, @"<img src=""([\s\S]*?)""");  //主图

            if (false == System.IO.Directory.Exists(path))
            {
                //创建pic文件夹
                System.IO.Directory.CreateDirectory(path);
                
                System.IO.Directory.CreateDirectory(subPath);
            }
            for (int i = 0; i < zhupics.Count; i++)
            {
                
                method.downloadFile("http:"+zhupics[i].Groups[1].Value+"jpg",subPath,i+".jpg");
            }

            for (int j = 0; j < xqpics.Count; j++)
            {

                method.downloadFile(xqpics[j].Groups[1].Value , subPath, j + ".jpg");
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
