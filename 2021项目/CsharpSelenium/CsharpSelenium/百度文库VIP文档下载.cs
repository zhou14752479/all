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
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CsharpSelenium
{
    public partial class 百度文库VIP文档下载 : Form
    {
        public 百度文库VIP文档下载()
        {
            InitializeComponent();
        }
       
        private void 百度文库VIP文档下载_Load(object sender, EventArgs e)
        {
            ListViewItem lv = listView2.Items.Add("13295270680");
            lv.SubItems.Add("Xc0RHZGdkp3V2RTSjd0VTBSQWtIOWZSRUVubXZjc3JIZzEzZzBCekVUdVh2dGRoSVFBQUFBJCQAAAAAAQAAAAEAAADx0IFOw8Cd4rLKu6pqNHY0AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAJcxsGGXMbBhL");
            string path = AppDomain.CurrentDomain.BaseDirectory;
            DataTable dt = method.ExcelToDataTable(path + "link.xlsx", false);

            for (int i = 1; i < dt.Rows.Count; i++)
            {

                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                lv1.SubItems.Add(dt.Rows[i][1].ToString().Trim());
                lv1.SubItems.Add("");
            }
        }

        public void run()
        {
            //浏览器初始化
            ChromeOptions options = new ChromeOptions();

            IWebDriver driver = new ChromeDriver(options);

            driver.Manage().Window.Maximize();

            for (int a = 0; a < listView2.Items.Count; a++)
            {


                string user = listView2.Items[a].SubItems[0].Text;
                string usercookie = listView2.Items[a].SubItems[1].Text.Replace(" ", "").Trim();


                driver.Navigate().GoToUrl("https://wenku.baidu.com/");

                Cookie cookie = new Cookie("BDUSS", usercookie, "", DateTime.Now.AddDays(9999));
                driver.Manage().Cookies.AddCookie(cookie);
                Thread.Sleep(200);
                driver.Navigate().GoToUrl("https://wenku.baidu.com/shopmis#/commodityManage/documentation");
                //浏览器初始化
                Thread.Sleep(500);




                for (int i = 200; i <listView1.Items.Count; i++)
                {
                    button2.Text = i.ToString();
                    try
                    {
                        //if (driver.PageSource.Contains("频繁"))
                        //{
                        //    MessageBox.Show("频繁");
                        //    break;
                        //}

                        if (driver.PageSource.Contains("扫码付款"))
                        {
                            MessageBox.Show("购买");
                            break;
                        }
                        string doclink = listView1.Items[i].SubItems[1].Text;
                        driver.Navigate().GoToUrl(doclink);
                        Thread.Sleep(1000);
                        driver.FindElement(By.XPath("/html/body/div/div[2]/div[1]/div[4]/div/div[1]/div")).Click();
                        Thread.Sleep(1000);
                        driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[4]/div[1]")).Click();
                        Thread.Sleep(1000);
                       

                    }
                    catch (Exception)
                    {

                        continue;
                    }
                }

            }

          

        }
        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
