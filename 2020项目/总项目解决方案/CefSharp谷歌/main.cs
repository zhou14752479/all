using CefSharp;
using CefSharp.WinForms;
using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CefSharp谷歌
{
    public partial class main : Form
    {
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
                chromiumWebBrowser.Load(targetUrl);
                return true; //Return true to cancel the popup creation copyright by codebye.com.
            }
        }

        ArrayList htmls = new ArrayList();

        public ChromiumWebBrowser browser = new ChromiumWebBrowser("https://fz.meituan.com/meishi/pn1/");
        public main()
        {
            InitializeComponent();
            //browser = new ChromiumWebBrowser("https://fz.meituan.com/meishi/");
            //browser.Parent = this.splitContainer1.Panel2;
            //browser.Dock = DockStyle.Fill;
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
           

            browser.Load("https://fz.meituan.com/meishi/pn1/");
            browser.Parent = this.splitContainer1.Panel2;
            browser.Dock = DockStyle.Fill;
            browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(FrameEndFunc);

            //textBox1.Text = browser.Address;

        }


        private void FrameEndFunc(object sender, FrameLoadEndEventArgs e)
        {
            try
            {
                
                this.BeginInvoke(new Action(() =>
                {
                    String  html = browser.GetSourceAsync().Result;
                   

                    HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                    htmlDocument.LoadHtml(html);
                    HtmlNodeCollection collection = null;
                    collection = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"app\"]/section/div/div[2]/div[2]/div[1]/ul/li[1]").ParentNode.ChildNodes;//跟Xpath一样，轻松的定位到相应节点下

                    if (collection.Count > 0)
                    {
                        foreach (HtmlNode nodeav in collection)
                        {


                            textBox2.Text += nodeav.InnerText + "\r\n";


                        }
                    }

                }));

       
                   
                


            }

            catch (Exception ex)
            {
                //  oThread.Abort();//结束线程

            }
        }








            








    }
}
