using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
namespace 淘宝店铺商品采集
{
    public partial class 淘宝店铺商品 : Form
    {
        public 淘宝店铺商品()
        {
            InitializeComponent();
        }
        Thread thread;

        bool status = true;
        List<string> lists = new List<string>();

        #region 琴房预约
        public void run()
        {

            ChromeOptions options = new ChromeOptions();
            IWebDriver driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();

            string loginurl = "https://login.taobao.com/member/login.jhtml";

            driver.Navigate().GoToUrl(loginurl);

            Thread.Sleep(40000);
           
            string[] links = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string link in links)
            {
                driver.Navigate().GoToUrl(link);
                for (int i = 0; i < 80; i++)
                {

                    MatchCollection urls;
                    while (true)
                    {
                        urls = Regex.Matches(driver.PageSource, @"item-name J_TGoldData"" href=""([\s\S]*?)""");
                        if (urls.Count > 0)
                            break;
                        if (status == false)
                            return;
                    }

                    foreach (Match url in urls)
                    {
                        if (!lists.Contains(url.Groups[1].Value))
                        {
                            lists.Add(url.Groups[1].Value);
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add("https" + url.Groups[1].Value);
                        }
                    }
                    if (status == false)
                        return;



                    Thread.Sleep(2000);
                    try
                    {
                        driver.FindElement(By.XPath("//*[@id=\"J_ShopSearchResult\"]/div/div[2]/div[10]/a[11]")).Click();
                    }
                    catch (Exception)
                    {

                        break;
                    }
                }
            }



        }
        #endregion
        private void button6_Click(object sender, EventArgs e)
        {
            status = true;
            if (thread == null || !thread.IsAlive)
            {
               
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 淘宝店铺商品_Load(object sender, EventArgs e)
        {

        }
    }
}
