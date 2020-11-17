using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CefSharp谷歌
{
    public partial class 速卖通 : Form
    {
        public 速卖通()
        {
            InitializeComponent();
           
        }
        public ChromiumWebBrowser browser = new ChromiumWebBrowser("https://www.aliexpress.com/");

      











        /// <summary>
        /// 在自己窗口打开链接开始代码
        /// </summary>
        internal class OpenPageSelf : ILifeSpanHandler
        {
            public bool DoClose(IWebBrowser browserControl, IBrowser browser)
            {
                return false;
            }

            public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
            {

            }
           
            public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
            {

            }

            public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl,
    string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures,
    IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
            {
                newBrowser = null;
                var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;
                //chromiumWebBrowser.Load(targetUrl);                 //打开点击的URL 如果不打开，此条注释掉
                myValue = targetUrl;

                
                return true; //Return true to cancel the popup creation copyright by codebye.com.
            }
        }

        //在自己窗口打开链接结束代码



        private static string myValue = "";

      






        private void 速卖通_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            browser.Load("https://www.aliexpress.com/");
            browser.Parent = this.splitContainer1.Panel1;
            browser.Dock = DockStyle.Fill;

            browser.LifeSpanHandler = new OpenPageSelf();  //让页面在当前窗口打开

            // this.browser.AddressChanged += Browser_AddressChanged;  //获取打开的URL


        
        }

        //处理打开的URL
        //private void Browser_AddressChanged(object sender, AddressChangedEventArgs e)
        //{
            
        //    if (checkBox1.Checked == true && e.Address.Contains("item"))

        //    {
                
        //        ListViewItem lv1 = listView1.Items.Add(e.Address); //使用Listview展示数据  
                
        //    }
            
        //}
       

        private void button1_Click(object sender, EventArgs e)
        {
            method.ListViewToCSV(listView1, true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            MessageBox.Show(myValue);
        }
    }
}
