using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 常用软件非客户
{
    public partial class 浏览器cookie获取 : Form
    {
        public 浏览器cookie获取()
        {
            InitializeComponent();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(textBox1.Text);
        }

        private void 浏览器cookie获取_Load(object sender, EventArgs e)
        {
            method.SetFeatures(10000);
           webBrowser1.ScriptErrorsSuppressed = true;
        }

        string url="";
        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text= method.GetCookies(url);
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            url = e.Url.ToString();
            textBox1.Text = url;
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            //防止弹窗；
            e.Cancel = true;
            string url = this.webBrowser1.StatusText;
            this.webBrowser1.Url = new Uri(url);
        }
    }
}
