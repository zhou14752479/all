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
    public partial class 中海化工项目 : Form
    {
        public 中海化工项目()
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
            options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
            if (headless)
            {
                options.AddArgument("--headless");
            }
            options.AddArgument("--disable-gpu");
            //driver.Manage().Window.Maximize();
            return new ChromeDriver(driverService, options);
        }


        IWebDriver driver;
        private void 中海化工项目_Load(object sender, EventArgs e)
        {
            driver = getdriver(false);
         
            driver.Navigate().GoToUrl("http://60.214.69.236:8888/seeyon/main.do?method=main");
        }


        public void run()
        {
            try
            {
                driver.SwitchTo().Frame(1);
                driver.FindElement(By.Id("print_span")).Click();
            }
            catch (Exception ex)
            {
                try
                {
                    driver.FindElement(By.Id("print_span")).Click();
                }
                catch (Exception)
                {

                    MessageBox.Show(ex.ToString());
                }
               
            }

            //try
            //{
            //    for (int i = 0; i < Convert.ToInt32(textBox1.Text); i++)
            //    {
            //        driver.FindElement(By.Id("print_span")).Click();
            //        try
            //        {
            //            driver.FindElement(By.ClassName("v-easy-table-checkbox-input")).Click();
            //        }
            //        catch (Exception ex)
            //        {

            //            MessageBox.Show("1111"+ex.ToString()); ;
            //        }

            //        Thread.Sleep(10);
            //    }
            //}
            //catch (Exception ex)
            //{

            //   MessageBox.Show(ex.ToString());
            //}
        }

        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
              
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    driver.SwitchTo().Frame(i);
                    MessageBox.Show(i.ToString());
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}
