using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Support.UI;

namespace CsharpSelenium
{
    public partial class 数据爬取 : Form
    {
        public 数据爬取()
        {
            InitializeComponent();
        }

        #region 百度搜索
        public void run()
        {

            ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--lang=en"); 
            options.AddArgument("--save-page-as-mhtml");
            options.AddUserProfilePreference("download.default_directory", @"C:\Users\zhou\Desktop\");
            options.AddUserProfilePreference("intl.accept_languages", "nl");
            options.AddUserProfilePreference("disable-popup-blocking", "true");

            IWebDriver driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl("https://www.baidu.com");

            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            ////等待页面上的元素加载完成
            //IWebElement element = wait.Until((d) =>
            //{
            //    try
            //    {
            //        return driver.FindElement(By.TagName("</html>"));
            //    }
            //    catch (Exception ex)
            //    {
            //        return null;
            //    }
            //});
            var _cookies = driver.Manage().Cookies.AllCookies;
            StringBuilder sb = new StringBuilder();
          
            foreach (Cookie cookie in _cookies)
            {

                sb.Append(cookie.Name+"="+cookie.Value+";");
                // driver.Manage().Cookies.AddCookie(cookie);
            }
            MessageBox.Show(sb.ToString());
            StreamReader keysr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));

            string ReadTxt = keysr.ReadToEnd();
            string[] text = ReadTxt.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string keyword in text)
            {
                driver.FindElement(By.Id("kw")).SendKeys(keyword);
                Thread.Sleep(200);
                driver.FindElement(By.Id("su")).Click();
                Thread.Sleep(1000);
                MatchCollection titles = Regex.Matches(driver.PageSource, @"<div class=""op_express_delivery_timeline_info"">([\s\S]*?)</div>");
              

                for (int j = 0; j < titles.Count; j++)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据


                    lv1.SubItems.Add(titles[j].Groups[1].Value);
                    lv1.SubItems.Add(keyword);


                }


                Thread.Sleep(300);

                driver.Navigate().Back();
                Thread.Sleep(1000);
            }

            driver.Quit();
        }
        #endregion


        #region 江苏税务局
        public void jsshuiwu()
        {

            ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--lang=en"); 
            options.AddArgument("--save-page-as-mhtml");
            options.AddUserProfilePreference("download.default_directory", @"C:\Users\zhou\Desktop\");
            options.AddUserProfilePreference("intl.accept_languages", "nl");
            options.AddUserProfilePreference("disable-popup-blocking", "true");

            IWebDriver driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://jiangsu.chinatax.gov.cn/col/col16044/index.html");
           
            Thread.Sleep(1000);
            for (int i = 0; i < 35; i++)
            {
                

               
                
                MatchCollection titles = Regex.Matches(driver.PageSource, @"<li><a title=""([\s\S]*?)""");

                for (int j = 0; j < titles.Count; j++)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据


                    lv1.SubItems.Add(titles[j].Groups[1].Value);
                    lv1.SubItems.Add(i.ToString());


                }

                driver.FindElement(By.XPath("/html/body/div[2]/div/div[4]/div[2]/div/div[2]/ul/div/table/tbody/tr/td/table/tbody/tr/td[8]/a")).Click();

                Thread.Sleep(2000);
            }

            driver.Quit();
        }
        #endregion

        private void 数据爬取_Load(object sender, EventArgs e)
        {

        }

        Thread thread;
        private void button6_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                timer1.Start();
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }
    }
}
