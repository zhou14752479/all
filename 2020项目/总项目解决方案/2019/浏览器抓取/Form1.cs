using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 浏览器抓取
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.webBrowser1.DocumentCompleted += First_DocumentCompleted;
        }
        public class session
        {
            public int 页号;
            public Uri uri;
        }

        private List<session> Pages;

        private int TryInt(string s)
        {
            int x;
            if (!int.TryParse(s, out x))
                return -1;

            return x;
        }

        void First_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (this.webBrowser1.ReadyState == WebBrowserReadyState.Complete)
            {
                this.webBrowser1.DocumentCompleted -= First_DocumentCompleted;
                var nav_panel = (from x in this.webBrowser1.Document.GetElementsByTagName("div").OfType<HtmlElement>()
                                 where x.GetAttribute("className") == "page_nav"
                                 select x).First();
                Pages = (from li in nav_panel.GetElementsByTagName("LI").OfType<HtmlElement>()
                         from a in li.GetElementsByTagName("A").OfType<HtmlElement>()
                         let num = TryInt(a.InnerText)
                         where num > 0
                         select new session
                         {
                             页号 = num,
                             uri = new Uri(a.GetAttribute("href"))
                         }).ToList();
                this.webBrowser1.DocumentCompleted += Flip_DocumentCompleted;
                Go();
            }
        }

        void Flip_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (this.webBrowser1.ReadyState == WebBrowserReadyState.Complete)
                Go();
        }

        private void Go()
        {
            var index = Pages.Count - 1;
            if (index >= 0)
            {
                var page = Pages[index];
                Pages.RemoveAt(index);
                this.webBrowser1.Navigate(page.uri);
                this.Text = string.Format("正在加载第 {0} 页：{1}", page.页号, page.uri.ToString());
            }
        }



    }
}
