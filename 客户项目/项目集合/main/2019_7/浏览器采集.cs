using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_7
{
    public partial class 浏览器采集 : Form
    {
        public 浏览器采集()
        {
            InitializeComponent();
        }
        public static string URL;

        bool loaded = false;   //该变量表示网页是否加载完成.默认未加载完成
        private void 浏览器采集_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://hotel.meituan.com/beijing/");

            this.webBrowser1.ScriptErrorsSuppressed = true;  //屏蔽IE脚本弹出错误
            this.webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;  //屏蔽IE脚本弹出错误
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.loaded = true; //加载完成传值

            this.webBrowser1.Document.Window.Error += OnWebBrowserDocumentWindowError;
            textBox1.Text = e.Url.ToString();
            URL = textBox1.Text;
        }
        private void OnWebBrowserDocumentWindowError(object sender, HtmlElementErrorEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// 点击下一页函数
        /// </summary>
        public void clicknext()
        {
            HtmlDocument doc = this.webBrowser1.Document;
            HtmlElementCollection nexts = doc.GetElementsByTagName("a");

            foreach (HtmlElement next in nexts)
            {
                //if (next.InnerText == "下一页")   //如果没有ID或者特异性的标志，通过标签的值去找标签<a>下一页</a>
                //{

                //    next.InvokeMember("click");
                //}

                if (next.GetAttribute("data-index").ToLower() == "next")   //a标签的date-index值为next的a标签然后点击， 必须唯一性
                {
                    next.InvokeMember("click");
                }
            }
        }

        /// <summary>
        /// 获取当前页的列表值
        /// </summary>
        /// <returns></returns>
        public ArrayList getList()
        {
            ArrayList lists = new ArrayList();
            HtmlDocument doc = this.webBrowser1.Document;
            HtmlElementCollection hrefs = doc.GetElementsByTagName("a");

            foreach (HtmlElement href in hrefs)
            {
                if (href.GetAttribute("class").ToLower() == "poi-title")   //a标签的date-index值为next的a标签然后点击， 必须唯一性
                {
                    MessageBox.Show(href.OuterHtml);
                    lists.Add(href.OuterHtml);
                }
            }

            return lists;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            HtmlAgilityPack.HtmlDocument hd = new HtmlAgilityPack.HtmlDocument();
            //加载Html文档
            hd.LoadHtml(strhtml);
            string str = hd.DocumentNode.SelectSingleNode("//*[@id='e_font']").OuterHtml;

        }



        }
    }
}
