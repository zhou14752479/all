using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main
{

    public partial class 谷歌浏览器 : Form
    {

        public string URL;

        public 谷歌浏览器(string url)
        {
            URL = url;
            InitializeComponent();
            InitBrowser(this.URL);
        }

        public static string cookie { get; set; }


       
        private void 谷歌浏览器_Load(object sender, EventArgs e)
        {
           
        }
        public ChromiumWebBrowser browser;
        public void InitBrowser(string URL)
        {
            Cef.Initialize(new CefSettings());
            browser = new ChromiumWebBrowser(URL);
              browser.Parent=this.splitContainer1.Panel2;
            browser.Dock = DockStyle.Fill;
           
        }


        

        private void button1_Click(object sender, EventArgs e)
        {
            var cookieManager = CefSharp.Cef.GetGlobalCookieManager();
           
         cookie = textBox1.Text;
           
           // this.Hide();

        }


    }
}
