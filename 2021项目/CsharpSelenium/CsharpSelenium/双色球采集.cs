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
using System.Runtime.InteropServices;

namespace CsharpSelenium
{
    public partial class 双色球采集 : Form
    {
        [DllImport("user32.dll")]
        static extern void mouse_event(int flags, int dX, int dY, int buttons, int extraInfo);

        const int MOUSEEVENTF_MOVE = 0x1;//模拟鼠标移动
        const int MOUSEEVENTF_LEFTDOWN = 0x2;//
        const int MOUSEEVENTF_LEFTUP = 0x4;
        const int MOUSEEVENTF_RIGHTDOWN = 0x8;
        const int MOUSEEVENTF_RIGHTUP = 0x10;
        const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        const int MOUSEEVENTF_MIDDLEUP = 0x40;
        const int MOUSEEVENTF_WHEEL = 0x800;
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;


        public 双色球采集()
        {
            InitializeComponent();
        }

        public string getqishu()
        {
            string url = "https://www.cjcp.com.cn/kaijiang/cpbqkjinfo.php?lot=ssq&ly=pc";
            string html = method.GetUrl(url,"gb2312");
           
            string qishu= Regex.Match(html, @"双色球 </td><td>([\s\S]*?)期").Groups[1].Value;
            return (Convert.ToInt32(qishu) + 1).ToString();
        }

        #region 双色球红球杀六
        public void shaungseqiu()
        {
          
            string qishu = getqishu();
            ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--lang=en"); 
        
           

            // 不显示浏览器
         
          // options.AddArgument("--headless");
            // GPU加速可能会导致Chrome出现黑屏及CPU占用率过高,所以禁用
            //options.AddArgument("--disable-gpu");
            // 伪装user-agent
           // options.AddArgument("user-agent=Mozilla/5.0 (iPhone; CPU iPhone OS 10_3 like Mac OS X) AppleWebKit/602.1.50 (KHTML, like Gecko) CriOS/56.0.2924.75 Mobile/14E5239e Safari/602.1");
            //隐身模式（无痕模式）
            //options.AddArgument("--incognito");
            // 设置chrome启动时size大小
            options.AddArgument("--window-size=400,800");
            // 禁用图片
            options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
            IWebDriver driver = new ChromeDriver(options);
           
            driver.Manage().Window.Size = new System.Drawing.Size(600, 1000);
            // driver.Manage().Window.Maximize();
            string[] uids =textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < uids.Length; i++)
            {

                string uid = uids[i];
                try
                {
                    driver.Navigate().GoToUrl("https://h5.duoduoyoucai.com/lottery/pages/expert-scheme/expert-scheme?userId=" + uid + "&lotteryId=5&tenantCode=recom&issueName=" + qishu);
                    Thread.Sleep(1000);
                    driver.FindElement(By.XPath("//*[text()=\"查看完整预测\"]")).Click();
                    Thread.Sleep(200);
                    driver.FindElement(By.XPath("//*[text()=\"取消\"]")).Click();
                    Thread.Sleep(200);

                    Match ahtml = Regex.Match(driver.PageSource, @"红球杀六</div>([\s\S]*?)</div>");
                    MatchCollection titles = Regex.Matches(ahtml.Groups[1].Value, @"<span class="""">([\s\S]*?)</span>");

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(uid);
                    for (int j = 0; j < titles.Count; j++)
                    {
                        lv1.SubItems.Add(titles[j].Groups[1].Value);

                    }
                    lv1.SubItems.Add((i+1).ToString());
                }
                catch (Exception)
                {

                    continue;
                }
            }



            // Thread.Sleep(1000);
          
        


    }
        #endregion


        #region 获取userid
        public void getuserid()
        {

            ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--lang=en"); 



            // 不显示浏览器
         
           // options.AddArgument("--headless");
            // GPU加速可能会导致Chrome出现黑屏及CPU占用率过高,所以禁用
            options.AddArgument("--disable-gpu");
            // 伪装user-agent
            options.AddArgument("user-agent=Mozilla/5.0 (iPhone; CPU iPhone OS 10_3 like Mac OS X) AppleWebKit/602.1.50 (KHTML, like Gecko) CriOS/56.0.2924.75 Mobile/14E5239e Safari/602.1");
            //隐身模式（无痕模式）
            options.AddArgument("--incognito");
            // 设置chrome启动时size大小
            options.AddArgument("--window-size=400,800");
            // 禁用图片
            options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
            IWebDriver driver = new ChromeDriver(options);
            driver.Manage().Window.Size = new System.Drawing.Size(400, 800);
            // driver.Manage().Window.Maximize();


            driver.Navigate().GoToUrl("https://h5.duoduoyoucai.com/lottery/pages/expert/expert");
            Thread.Sleep(60000);
         

            for (int i = 555; i < 1000; i++)
            {
              
                IList<IWebElement> listOption = driver.FindElements(By.ClassName("expert-name"));
                try
                {
                    listOption[i].Click();
                }
                catch (Exception)
                {
                    MessageBox.Show((i+1).ToString());
                    listOption[i].Click(); ;
                }
               
                Thread.Sleep(1000);
                string currentURL = driver.Url;

              
               string userid= Regex.Match(currentURL, @"userId=([\s\S]*?)&").Groups[1].Value;
              

                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                lv1.SubItems.Add(currentURL);
                lv1.SubItems.Add(userid);
                //driver.SwitchTo().Window(driver.WindowHandles[0]);
                    //切换至初始标签页句柄
                //后退
               driver.Navigate().Back();
                Thread.Sleep(1000);

                //控制鼠标滑轮滚动，count代表滚动的值，负数代表向下，正数代表向上，如-100代表向下滚动100的y坐标
                
                //IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                //js.ExecuteScript("window.scrollTo(500, 500);");
                //前进
                // driver.Navigate().Forward();
                //刷新
                //  driver.Navigate().Refresh();

            }

           
            




        }
        #endregion

        private void 双色球采集_Load(object sender, EventArgs e)
        {

        }
        Thread thread;
      
      
        private void button6_Click(object sender, EventArgs e)
        {
         
            if (thread == null || !thread.IsAlive)
            {
               
                thread = new Thread(shaungseqiu);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 双色球采集_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
