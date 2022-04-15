using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace 亚马逊抓取
{
    public partial class 天眼查发票 : Form
    {
        public 天眼查发票()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

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
                //options.AddArgument("window-size=1920,1080");
                options.AddArgument("--headless");

            }
            options.AddArgument("--disable-gpu");

            return new ChromeDriver(driverService, options);
        }
        private void 天眼查发票_Load(object sender, EventArgs e)
        {
            #region 通用检测

            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"mpxO"))            {                System.Diagnostics.Process.GetCurrentProcess().Kill();                return;            }            #endregion
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {
            status = true;
            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
        IWebDriver driver;
        bool zanting = true;
        bool status = true;
        private void button1_Click(object sender, EventArgs e)
        {
            driver = getdriver(false);
            driver.Navigate().GoToUrl("https://www.tianyancha.com/search/ocA-r200500?key=%E8%BD%AF%E4%BB%B6%E5%BC%80%E5%8F%91");
        }
        #region 模拟页面
        public void run()
        {

            //driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);
            try
            {
                StringBuilder sb = new StringBuilder();
                var _cookies = driver.Manage().Cookies.AllCookies;

                foreach (OpenQA.Selenium.Cookie cookie in _cookies)
                {
                    sb.Append(cookie.Name + "=" + cookie.Value + ";");

                }
                String currentURL = driver.Url;
              

                for (int i = 2; i < 5001; i++)
                {
                    Thread.Sleep(1000);
                  string URL = currentURL.Replace("?", "/p" + i + "?");

                    string html = driver.PageSource;
                    //textBox1.Text = html;
                    MatchCollection ids = Regex.Matches(html, @"search-result-single([\s\S]*?)data-id=""([\s\S]*?)""");
                    if (ids.Count == 0)
                        break;
                    for (int j = 0; j < ids.Count; j++)
                    {
                        string ahtml = method.GetUrlWithCookie("https://www.tianyancha.com/cloud-wechat/qrcode.json?gid=" + ids[j].Groups[2].Value + "&_=1649821424041", sb.ToString(), "utf-8");
                        string name = Regex.Match(ahtml, @"""name"":""([\s\S]*?)""").Groups[1].Value;
                        string taxnum = Regex.Match(ahtml, @"""taxnum"":""([\s\S]*?)""").Groups[1].Value;
                        string address = Regex.Match(ahtml, @"""address"":""([\s\S]*?)""").Groups[1].Value;
                        string phone = Regex.Match(ahtml, @"""phone"":""([\s\S]*?)""").Groups[1].Value;
                        string bank = Regex.Match(ahtml, @"""bank"":""([\s\S]*?)""").Groups[1].Value;
                        string bankAccount = Regex.Match(ahtml, @"""bankAccount"":""([\s\S]*?)""").Groups[1].Value;

                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                        lv1.SubItems.Add(name);
                        lv1.SubItems.Add(taxnum);
                        lv1.SubItems.Add(address);
                        lv1.SubItems.Add(phone);
                        lv1.SubItems.Add(bank);
                        lv1.SubItems.Add(bankAccount);

                        FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\data.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                        sw.WriteLine(name+"-"+taxnum+"-"+address + "-"+phone + "-"+bank + "-"+bankAccount);
                        sw.Close();
                        fs1.Close();
                        sw.Dispose();

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if(status==false)
                        {
                            return;
                        }
                        Thread.Sleep(Convert.ToInt32(textBox2.Text));


                    }
                    //driver.FindElement(By.XPath("//*[text()=\"立即升级\"]")).Click();
                    Thread.Sleep(Convert.ToInt32(textBox2.Text));
                    driver.Navigate().GoToUrl(URL);
                }
            }


            catch (Exception ex)
            {


                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void 天眼查发票_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

      



    }
    }
