using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace helper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string cookie = "";
        public static string SID= "";
        public void getmseeage(string name)
        {
            MessageBox.Show(name);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //method.SetWebBrowserFeatures(method.IeVersion.IE10);
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;

             webBrowser1.Navigate("https://item.manager.taobao.com/taobao/manager/render.htm?tab=in_stock&table.sort.endDate_m=desc&spm=a217wi.openworkbeanchtmall");
              // webBrowser1.Navigate("http://www.yanxiu.com/login.html?l=true");
            // webBrowser1.Navigate("https://new-wl.jdwl.com/");
           // webBrowser1.Navigate("https://mail.qq.com/");
        }
        
        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void button1_Click(object sender, EventArgs e)
        {

             cookie = method.GetCookies("https://item.manager.taobao.com/taobao/manager/render.htm?pagination.current=1&pagination.pageSize=20&tab=in_stock&table.sort.endDate_m=desc");
            // cookie = method.GetCookies("http://i.yanxiu.com/?j=true&fl=true");
            //cookie = method.GetCookies("https://biz-wb.jdwl.com/business/waybillmanage/toDeliveryDetail");


            //cookie = method.GetCookies(webBrowser1.Url.ToString());
            //Match sid = Regex.Match(webBrowser1.Url.ToString(), @"sid=([\s\S]*?)&");
            //SID = sid.Groups[1].Value;
            //this.Hide();
            textBox1.Text = cookie;
            //System.IO.File.WriteAllText(path+textBox1.Text.Trim()+".txt",cookie, Encoding.UTF8);
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            string url = this.webBrowser1.StatusText;
            this.webBrowser1.Url = new Uri(url);
        }

      
    }
}
