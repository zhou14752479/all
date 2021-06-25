using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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

namespace 淘宝列表页
{
    public partial class 阿里代发 : Form
    {
        public 阿里代发()
        {
            InitializeComponent();
        }

        int nowlinnk = 0;

        #region 淘宝列表页
        public void run()
        {

            driver = new ChromeDriver();
            string[] links = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = nowlinnk; i <links.Length; i++)
            {

                driver.Navigate().GoToUrl(links[i]);

                Random rd = new Random(Guid.NewGuid().GetHashCode()); //生成不重复的随机数，默认的话根据时间戳如果太快会相同
                int second = rd.Next(2, 5);
                Thread.Sleep((second * 1000));
                try
                {
                    driver.FindElement(By.XPath("//*[text()=\"我要代发\"]")).Click();
                    nowlinnk = nowlinnk + 1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }



        }
        #endregion

        Thread thread;

        IWebDriver driver;
        private void button6_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 阿里代发_Load(object sender, EventArgs e)
        {
          


        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ChromeOptions options = new ChromeOptions();
           
            //driver = new ChromeDriver(options);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("Object.defineProperties(navigator, {webdriver:{get:()=>undefined}});");
            js.ExecuteScript("window.navigator.chrome = { runtime: {}, }; ");
            js.ExecuteScript("Object.defineProperty(navigator, 'languages', { get: () => ['en-US', 'en'] });");
            js.ExecuteScript("Object.defineProperty(navigator, 'plugins', { get: () => [1, 2, 3, 4, 5,6], });");

            driver.Navigate().GoToUrl("https://login.1688.com/member/signin.htm?tracelog=member_signout_signin");
            js.ExecuteScript("Object.defineProperties(navigator, {webdriver:{get:()=>undefined}});");
            js.ExecuteScript("window.navigator.chrome = { runtime: {}, }; ");
            js.ExecuteScript("Object.defineProperty(navigator, 'languages', { get: () => ['en-US', 'en'] });");
            js.ExecuteScript("Object.defineProperty(navigator, 'plugins', { get: () => [1, 2, 3, 4, 5,6], });");
        }
    }
}
