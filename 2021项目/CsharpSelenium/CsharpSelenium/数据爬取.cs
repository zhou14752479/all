﻿using OpenQA.Selenium;
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
            options.AddArgument("--headless");//设置无界面
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
           // MessageBox.Show(sb.ToString());
            StreamReader keysr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));

            string ReadTxt = keysr.ReadToEnd();
            string[] text = ReadTxt.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string keyword in text)
            {
                label1.Text = keyword;
                driver.FindElement(By.Id("kw")).SendKeys(keyword);
                Thread.Sleep(200);
                driver.FindElement(By.Id("su")).Click();
                Thread.Sleep(1000);
                MatchCollection titles = Regex.Matches(driver.PageSource, @"<div class=""op_express_delivery_timeline_info"">([\s\S]*?)</div>");
              

                for (int j = 0; j < titles.Count; j++)
                {
                    Match name= Regex.Match(titles[j].Groups[1].Value, @"【([\s\S]*?)】");
                    MatchCollection tels = Regex.Matches(titles[j].Groups[1].Value, @"\d{11}|\d{4}-\d{7,}");

                   
                    for (int i = 0; i < tels.Count; i++)
                    {
                        if (name.Groups[1].Value != "")
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                            lv1.SubItems.Add(keyword);
                            lv1.SubItems.Add(name.Groups[1].Value+ tels[i].Groups[0].Value);
                            lv1.SubItems.Add(tels[i].Groups[0].Value);

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }

                            if (status == false)
                                return;
                        }
                    }

                    

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


        #region 广东办事大厅
        public void guangdong()
        {

            ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--lang=en"); 
       

            IWebDriver driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://210.76.73.15/Portal/BAList.aspx?ColID=xmgk");

            Thread.Sleep(5000);


            for (int i = 0; i < 400; i++)
            {

   
             
                MatchCollection titles = Regex.Matches(driver.PageSource, @"ColID=xmgk&amp;Id=([\s\S]*?)""");
               
                Match page = Regex.Match(driver.PageSource, @"lblCurrentPage"" class=""list_page_statis"">([\s\S]*?)</span>");

                for (int j = 0; j < titles.Count; j++)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据


                    lv1.SubItems.Add(titles[j].Groups[1].Value);
                    lv1.SubItems.Add(page.Groups[1].Value);


                }

              

               // Thread.Sleep(1000);
                driver.FindElement(By.Id("ctl00_masterContent_lbtNextPage")).Click();
            }

           
        }
        #endregion
        private void 数据爬取_Load(object sender, EventArgs e)
        {

        }

        Thread thread;
        bool zanting = true;
        bool status = true;
        private void button6_Click(object sender, EventArgs e)
        {
            //#region 通用检测

            //string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            //if (!html.Contains(@"kuaidichaxun"))
            //{
            //    MessageBox.Show("验证失败");
            //    return;
            //}



            //#endregion
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                timer1.Start();
                thread = new Thread(guangdong);
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

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {
                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
