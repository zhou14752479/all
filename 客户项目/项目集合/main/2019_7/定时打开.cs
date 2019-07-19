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
            //string h= DateTime.Now.Hour.ToString();
            //string m = DateTime.Now.Minute.ToString();
            //string s = DateTime.Now.Second.ToString();
            if (dateTimePicker1.Text == DateTime.Now.ToLongTimeString())
            {
                webBrowser1.Navigate(textBox1.Text);
            }
            if (dateTimePicker2.Text == DateTime.Now.ToLongTimeString())
            {
                webBrowser1.Navigate(textBox2.Text);
            }

            if (dateTimePicker3.Text == DateTime.Now.ToLongTimeString())
            {
                webBrowser1.Navigate(textBox3.Text);
            }
         
        }

        public void run1()

        {
           
            if (dateTimePicker4.Value < Convert.ToDateTime( DateTime.Now.ToLongTimeString()) && Convert.ToDateTime(DateTime.Now.ToLongTimeString())< dateTimePicker5.Value)
            {
                timer2.Start();
            }

            if (dateTimePicker7.Value < Convert.ToDateTime(DateTime.Now.ToLongTimeString()) && Convert.ToDateTime(DateTime.Now.ToLongTimeString()) < dateTimePicker6.Value)
            {
                timer3.Start();
            }


        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            run();

        }

        private void Button1_Click(object sender, EventArgs e)
        {

            timer2.Interval = Convert.ToInt32(textBox4.Text) * 1000;
            timer3.Interval = Convert.ToInt32(textBox7.Text) * 1000;
            run1();
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            webBrowser1.Navigate(textBox5.Text);
        }

        private void Timer3_Tick(object sender, EventArgs e)
        {
            webBrowser1.Navigate(textBox6.Text);
        }
    }
}
