using OpenQA.Selenium;
using OpenQA.Selenium.IE;
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
using myDLL;
using OpenQA.Selenium.Chrome;

namespace seleniumIE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            ////ie浏览器驱动
            //var cdSvc = InternetExplorerDriverService.CreateDefaultService();
            //cdSvc.HideCommandPromptWindow = true;//关闭cmd窗口

            //IWebDriver driver = new InternetExplorerDriver(cdSvc);
            //driver.Manage().Window.Maximize();
            //driver.Navigate().GoToUrl("http://www.baidu.com");

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getCookies);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }


        }

         IWebDriver driver = new InternetExplorerDriver();
      
        public void getCookies()
        {

            // driver.Navigate().GoToUrl("https://perbank.abchina.com/EbankSite/startup.do?r=E52BD98D01B8A34D");
            driver.Navigate().GoToUrl("http://www.baidu.com");
            //options.AddArgument("--lang=en"); 




        }



        private void button2_Click(object sender, EventArgs e)
        {
            StringBuilder sb;

            //while (true)
            //{
            //    sb = new StringBuilder();
            //    var _cookies = driver.Manage().Cookies.AllCookies;

            //    foreach (Cookie cookie in _cookies)
            //    {

            //        sb.Append(cookie.Name + "=" + cookie.Value + ";");
            //        // driver.Manage().Cookies.AddCookie(cookie);
            //    }

            //    if (sb.ToString().Contains("_mobile_id"))
            //    {
            //        textBox1.Text = sb.ToString();
            //        break;
            //    }
            //}

       
                sb = new StringBuilder();
                var _cookies = driver.Manage().Cookies.AllCookies;
          
                foreach (Cookie cookie in _cookies)
                {
              
                    sb.Append(cookie.Name + "=" + cookie.Value + ";");
                    // driver.Manage().Cookies.AddCookie(cookie);
                }
            textBox1.Text = sb.ToString();
            MessageBox.Show("1");

           
        }







    }
}
