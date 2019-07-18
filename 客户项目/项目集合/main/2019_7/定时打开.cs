using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_7
{
    public partial class 定时打开 : Form
    {
        public 定时打开()
        {
            InitializeComponent();
        }

        private void 定时打开_Load(object sender, EventArgs e)
        {
            this.webBrowser1.ScriptErrorsSuppressed = true;  //屏蔽IE脚本弹出错误
            this.webBrowser1.DocumentCompleted += WebBrowser1_DocumentCompleted;  //屏蔽IE脚本弹出错误
            method.SetIE(0);  //设置浏览器版本为枚举值第一个值
            timer1.Start();
        }

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.webBrowser1.Document.Window.Error += OnWebBrowserDocumentWindowError;
        }
        private void OnWebBrowserDocumentWindowError(object sender, HtmlElementErrorEventArgs e)
        {
            e.Handled = true;
        }

        public void run()

        {
            string h= DateTime.Now.Hour.ToString();
            string m = DateTime.Now.Minute.ToString();
            string s = DateTime.Now.Second.ToString();
            if (textBox2.Text == h && textBox3.Text == m && textBox4.Text == s)
            {
                webBrowser1.Navigate(textBox1.Text);
               
            }
            if (textBox7.Text == h && textBox6.Text == m && textBox5.Text == s)
            {
                webBrowser1.Navigate(textBox8.Text);
              
            }
            if (textBox11.Text == h && textBox10.Text == m && textBox9.Text == s)
            {
                webBrowser1.Navigate(textBox12.Text);
               
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            run();

        }
    }
}
