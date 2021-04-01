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
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace 淘宝列表页
{
    public partial class 淘宝列表页 : Form
    {
        public 淘宝列表页()
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

            string loginurl = "https://login.taobao.com/member/login.jhtml?redirectURL=" + textBox1.Text;

            driver.Navigate().GoToUrl(loginurl);

            Thread.Sleep(20000);

            string[] links = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string link in links)
            {
                driver.Navigate().GoToUrl(link);
                for (int i = 0; i < 80; i++)
                {

                    MatchCollection urls;
                    while (true)
                    {
                        urls = Regex.Matches(driver.PageSource, @"J_Itemlist_Pic_([\s\S]*?)""");
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
                            lv1.SubItems.Add("https://item.taobao.com/item.htm?id="+url.Groups[1].Value);
                        }
                    }
                    if (status == false)
                        return;

                    Random rd = new Random(Guid.NewGuid().GetHashCode()); //生成不重复的随机数，默认的话根据时间戳如果太快会相同
                    int second = rd.Next(2, 5);
                    Thread.Sleep((second*1000));
                    try
                    {
                        driver.FindElement(By.XPath("//*[text()=\"下一页\"]")).Click();
                    }
                    catch (Exception ex)
                    {
                       
      
                        MessageBox.Show(ex.ToString());
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

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
