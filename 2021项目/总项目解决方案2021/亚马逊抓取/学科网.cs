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

namespace 亚马逊抓取
{
    public partial class 学科网 : Form
    {
        public 学科网()
        {
            InitializeComponent();
        }
        public static IWebDriver getdriver(bool headless)
        {
            //关闭cmd窗口
            ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;


            ChromeOptions options = new ChromeOptions();
            options.BinaryLocation = "Chrome/Application/chrome.exe";
            //禁用图片
           // options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
            if (headless)
            {
                options.AddArgument("--headless");
            }
            options.AddArgument("--disable-gpu");
            //driver.Manage().Window.Maximize();
            return new ChromeDriver(driverService, options);
        }

        IWebDriver driver;

        private void 学科网_Load(object sender, EventArgs e)
        {
            driver = getdriver(false);

            driver.Navigate().GoToUrl("http://oa.xkw.cn/Function/Index.html");
            Cookie cookie = new Cookie("ASP.NET_SessionId", "wredy3wrdlu11vmemw1dneoy", "", DateTime.Now.AddDays(9999));
            driver.Manage().Cookies.AddCookie(cookie);
            Thread.Sleep(200);
            driver.Navigate().GoToUrl("http://oa.xkw.cn/Function/Index.html");
        }

        bool status = false;
        public void run()
        {
           

            try
            {
                if (status == false)
                {
                    driver.SwitchTo().Frame(0);
                    status = true;
                }
               
               
                //driver.FindElement(By.Id("print_span")).Click();
                driver.FindElement(By.XPath("//*[text()=\"提交\"]")).Click();
                Thread.Sleep(1000);
                driver.FindElement(By.XPath("//*[text()=\"确定\"]")).Click();
            }
            catch (Exception ex)
            {

             ex.ToString();
            }

        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
            timer1.Interval = Convert.ToInt32(textBox1.Text)*1000;
            if (radioButton1.Checked==true)
            {
                if (thread == null || !thread.IsAlive)
                {

                    thread = new Thread(run);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
            if (radioButton2.Checked == true)
            {
                if(DateTime.Now.ToString("HH:mm:ss") == dateTimePicker1.Value.ToString("HH:mm:ss"))
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = Convert.ToInt32(textBox1.Text) * 1000;
            if (radioButton1.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {

                    thread = new Thread(run);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
            if (radioButton2.Checked == true)
            {
                if (DateTime.Now.ToString("HH:mm:ss") == dateTimePicker1.Value.ToString("HH:mm:ss"))
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

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label3.Text = "当前时间：" + DateTime.Now.ToString("HH:mm:ss");
            if (DateTime.Now.ToString("HH:mm:ss") == dateTimePicker1.Value.ToString("HH:mm:ss"))
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
