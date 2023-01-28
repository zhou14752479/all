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


        //WebView2.exe.WebView2文件夹为缓存可以删除，每次自动会创建
        //runtimes文件夹不可以删除
        /*
         * 
         * 获取打开的网址
          private void webView21_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
           textBox1.Text= webView21.Source.ToString();
        }

        //获取cookie
        async  void getcookies()
        {
            var result = await webView21.ExecuteScriptAsync("document.cookie");
            this.cookie = result;
            //MessageBox.Show(result);
        }


        */

        private void Form1_Load(object sender, EventArgs e)
        {

            // webView21.Source = new System.Uri("https://mnks.jxedt.com/", System.UriKind.Absolute);
            webView21.Source = new System.Uri("http://www.iy6.cn/?c=Public&action=verify");
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
