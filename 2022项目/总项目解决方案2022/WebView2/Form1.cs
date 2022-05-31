using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebView2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
         
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {

            //webView21.Source = new System.Uri("https://www.baidu.com/", System.UriKind.Absolute);
            
        }


        void EnsureHttps(object sender, CoreWebView2NavigationStartingEventArgs args)
        {
            String uri = args.Uri; //get request url
         
            CoreWebView2HttpRequestHeaders headers = args.RequestHeaders; //get request headers
            //MessageBox.Show(headers.);


        }

        private void button1_Click(object sender, EventArgs e)
        {
           WebView2.Form1 form = new WebView2.Form1();  
           

        }


        //获取网页加载完成的源代码
        //private async void webView21_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        //{
        //    object obj = await webView21.CoreWebView2.ExecuteScriptAsync("document.body.outerHTML");//第一次获取没法获取后端的数据
        //    textBox1.Text = obj.ToString();


        //}



    }
}
