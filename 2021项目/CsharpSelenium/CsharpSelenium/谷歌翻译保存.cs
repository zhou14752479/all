using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;

namespace CsharpSelenium
{
    public partial class 谷歌翻译保存 : Form
    {

        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(System.Windows.Forms.Keys bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
        public const int KEYEVENTF_KEYUP = 2;
       

        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        //移动鼠标 
        const int MOUSEEVENTF_MOVE = 0x0001;
        //模拟鼠标左键按下 
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        //模拟鼠标左键抬起 
        const int MOUSEEVENTF_LEFTUP = 0x0004;
        //模拟鼠标右键按下 
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        //模拟鼠标右键抬起 
        const int MOUSEEVENTF_RIGHTUP = 0x0010;
        //模拟鼠标中键按下 
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        //模拟鼠标中键抬起 
        const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        //标示是否采用绝对坐标 
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        const int MOUSEEVENTF_WHEEL = 0x800;
        public 谷歌翻译保存()
        {
            InitializeComponent();
        }
        /*
       driver.Navigate().Back();
       options.AddArgument("--user-agent=" + "some safari agent");
       options.AddArgument("--lang=en");
       options.AddArgument("--save-page-as-mhtml");
            driver.Navigate().Forward();
            driver.Navigate().Refresh();
            driver.Title;
            driver.CurrentWindowHandle;
            driver.Quit();
            driver.FindElement(By.TagName("button")).Click();
            driver.SwitchTo().Frame("myframe");
            driver.Manage().Window.Size = new Size(1024, 768);
            driver.Manage().Window.Minimize();
            IWebElement searchbox = driver.FindElement(By.Name("q"));
            searchbox.SendKeys("webdriver");
             driver.FindElement(By.XPath("//*[@id=\"kw\"]")).SendKeys("测试");
            driver.FindElement(By.XPath("//*[@id=\"su\"]")).Click();
           // Enter "webdriver" text and perform "ENTER" keyboard action
            driver.FindElement(By.Name("q")).SendKeys("webdriver" + Keys.Enter);
            options.AddUserProfilePreference("prefs", prefs);
               ReadOnlyCollection<string> windows = driver.WindowHandles;
                driver.SwitchTo().Window(windows[0]);//有新窗口弹出时切换
         */

        public class ChromeOptionsWithPrefs : ChromeOptions
        {
            public Dictionary<string, object> prefs { get; set; }
        }

        public void runwithTranslate()
        {
                    
            ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--lang=en"); 
            options.AddArgument("--save-page-as-mhtml");
            options.AddUserProfilePreference("download.default_directory", @"C:\Users\zhou\Desktop\");
            options.AddUserProfilePreference("intl.accept_languages", "nl");
            options.AddUserProfilePreference("disable-popup-blocking", "true");
           
            IWebDriver driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
           
            for (int i = 0; i < 1; i++)
            {
                string url = listView1.Items[i].SubItems[1].Text;
                string rename = listView1.Items[i].SubItems[2].Text;
                driver.Navigate().GoToUrl(url);

                //driver.Url = "http://www.baidu.com"是一样的


                //mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);

                //Thread.Sleep(1000);
                //keybd_event(System.Windows.Forms.Keys.T, 0, 0, 0);
                //Thread.Sleep(1000);
                //int count = 10;
                //while (count < 3999)
                //{
                //    mouse_event(MOUSEEVENTF_WHEEL, 0, 0, -100, 0);
                //    Thread.Sleep(100);
                //    count = count + 10;
                //}

                //Thread.Sleep(10000);
                Actions action = new Actions(driver);
              
                action.KeyDown(OpenQA.Selenium.Keys.Control).SendKeys("s").KeyUp(OpenQA.Selenium.Keys.Control).Build().Perform();
                action.SendKeys(OpenQA.Selenium.Keys.Control + "s").Build().Perform();
                //keybd_event(System.Windows.Forms.Keys.ControlKey, 0, 0, 0);
                //keybd_event(System.Windows.Forms.Keys.S, 0, 0, 0);
                //keybd_event(System.Windows.Forms.Keys.S, 0, KEYEVENTF_KEYUP, 0);
                //keybd_event(System.Windows.Forms.Keys.ControlKey, 0, KEYEVENTF_KEYUP, 0);



                Thread.Sleep(1000);
                
            }

            //driver.Quit();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }

           
        }

        private void 谷歌翻译保存_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.ReadFromExcelFile(@"C:\Users\zhou\Desktop\TEST2 不翻译.xlsx", listView1);
            runwithTranslate();
        }
    }
}
