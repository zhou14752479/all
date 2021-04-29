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

namespace 生意网宝贝发布淘宝
{
    public partial class 淘宝发布宝贝 : Form
    {
        public 淘宝发布宝贝()
        {
            InitializeComponent();
        }

        IWebDriver driver=null;
        public void openurl()
        {

            try
            {
                ChromeOptions options = new ChromeOptions();

                //伪装浏览器，去掉淘宝检测selenium
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("Object.defineProperties(navigator, {webdriver:{get:()=>undefined}});");
                driver.Navigate().GoToUrl("http://www.3e3e.cn/newstyle/");
                js.ExecuteScript("Object.defineProperties(navigator, {webdriver:{get:()=>undefined}});");

                js.ExecuteScript("window.open('https://login.taobao.com/member/login.jhtml','_blank');");

                js.ExecuteScript("var a = document.querySelector('a');a.onclick = function() {alert('1');}");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        public void run()
        {
            try
            {
                //driver.PageSource;
                string currentURL = driver.Url;


                string userid = Regex.Match(currentURL, @"userId=([\s\S]*?)&").Groups[1].Value;


                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                lv1.SubItems.Add(currentURL);
                lv1.SubItems.Add("正在采集..");
            }
            catch (Exception)
            {

                throw;
            }
        }
        Thread thread;
        private void 淘宝发布宝贝_Load(object sender, EventArgs e)
        {
            driver = new ChromeDriver();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openurl();
        }
    }
}
