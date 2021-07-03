using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main
{
    public partial class getUrlbyWeb : Form
    {
        public getUrlbyWeb()
        {
            InitializeComponent();
        }

        public static string URL;

        private void GetUrlbyWeb_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://www.vitalsource.com/textbooks");

            this.webBrowser1.ScriptErrorsSuppressed = true;  //屏蔽IE脚本弹出错误
            this.webBrowser1.DocumentCompleted += WebBrowser1_DocumentCompleted;  //屏蔽IE脚本弹出错误
        }

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.webBrowser1.Document.Window.Error += OnWebBrowserDocumentWindowError;
            textBox1.Text = e.Url.ToString();
            URL = textBox1.Text;
        }
        private void OnWebBrowserDocumentWindowError(object sender, HtmlElementErrorEventArgs e)
        {
            e.Handled = true;
        }

        private void WebBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;//防止弹窗；
            string url = this.webBrowser1.StatusText;
            textBox1.Text = url;
            this.webBrowser1.Url = new Uri(url); //打开链接
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://" + textBox1.Text);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            URL = textBox1.Text;
            this.Close();
        }
    }
}
