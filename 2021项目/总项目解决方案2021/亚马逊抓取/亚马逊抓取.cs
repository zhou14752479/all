using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using myDLL;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace 亚马逊抓取
{
    public partial class 亚马逊抓取 : Form
    {

        /*
 配置部分:

ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();
driverService.HideCommandPromptWindow = true;//关闭黑色cmd窗口

ChromeOptions options = new ChromeOptions();
// 不显示浏览器
//options.AddArgument("--headless");
// GPU加速可能会导致Chrome出现黑屏及CPU占用率过高,所以禁用
options.AddArgument("--disable-gpu");
// 伪装user-agent
options.AddArgument("user-agent=Mozilla/5.0 (iPhone; CPU iPhone OS 10_3 like Mac OS X) AppleWebKit/602.1.50 (KHTML, like Gecko) CriOS/56.0.2924.75 Mobile/14E5239e Safari/602.1");
// 设置chrome启动时size大小
options.AddArgument("--window-size=414,736");
// 禁用图片
options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);

IWebDriver webDriver = new ChromeDriver(driverService,options);
//如果查找元素在5S内还没有找到
webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5); 
string url = "https://www.baidu.com";
webDriver.Navigate().GoToUrl(url);
 执行JS(将滚动条拉到底部):
((IJavaScriptExecutor)webDriver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
 获取标签(以多Class为例):
ReadOnlyCollection<IWebElement> elements = webDriver.FindElements(By.CssSelector("[class='item goWork']"));
--------------------- 

            //点击下一页  元素不可见，使之可见
              ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].checked = true;", driver.FindElement(By.XPath("//*[text()=\"下一页\"]")));
                Thread.Sleep(2000);
                driver.FindElement(By.XPath("//*[text()=\"下一页\"]")).Click();

    */

        public 亚马逊抓取()
        {
            InitializeComponent();
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //textBox1.Text = "";
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
        #region 模拟采集列表
        public void run_moni()
        {

            //driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);
            driver.Navigate().GoToUrl(textBox1.Text);
            Thread.Sleep(1000);

            for (int page = 0; page < 600; page++)
            {
                if (status == false)
                    return;
                try
                {
                    progressBar1.Value++;
                    Thread.Sleep(2000);
                    string html = driver.PageSource;
                    MatchCollection htmls = Regex.Matches(html, @"cel_widget_id=""MAIN-SEARCH_RESULTS([\s\S]*?)</div></div></div></div></div>");

                    for (int i = 0; i < htmls.Count; i++)
                    {
                        string title = Regex.Match(htmls[i].Groups[1].Value, @"a-color-base a-text-normal"">([\s\S]*?)</span>").Groups[1].Value;
                        string price = Regex.Match(htmls[i].Groups[1].Value, @"data-a-color=""base""><span class=""a-offscreen"">([\s\S]*?)</span>").Groups[1].Value;
                        string asin = Regex.Match(htmls[i].Groups[1].Value, @";asin=([\s\S]*?)&").Groups[1].Value;
                        string star = Regex.Match(htmls[i].Groups[1].Value, @"<span aria-label=""([\s\S]*?) ").Groups[1].Value;
                        string comment = Regex.Match(htmls[i].Groups[1].Value, @"<span class=""a-size-base"">([\s\S]*?)</span>").Groups[1].Value;
                        string picurl = Regex.Match(htmls[i].Groups[1].Value, @"<img class=""s-image"" src=""([\s\S]*?)""").Groups[1].Value;

                        if (asin == "")
                        {
                            continue;
                        }
                            if (price == "")
                            {
                                price = "目前无货";
                            }

                        //子线程中
                        this.Invoke(new InvokeHandler(delegate ()
                        {
                            string aurl = "https://www.amazon.com/dp/" + asin + "/";
                            this.index = this.dataGridView1.Rows.Add();
                          
                            dataGridView1.Rows[index].Cells[0].Value = index.ToString();
                            dataGridView1.Rows[index].Cells[1].Value = asin;
                            dataGridView1.Rows[index].Cells[2].Value = "1";
                            dataGridView1.Rows[index].Cells[3].Value = aurl;
                            dataGridView1.Rows[index].Cells[4].Value = "COM";
                            dataGridView1.Rows[index].Cells[5].Value = DateTime.Now.ToLongTimeString();
                            dataGridView1.Rows[index].Cells[6].Value = title;
                            dataGridView1.Rows[index].Cells[7].Value = price;
                            dataGridView1.Rows[index].Cells[8].Value = star;
                            dataGridView1.Rows[index].Cells[9].Value = comment.Replace(",","");
                            dataGridView1.Rows[index].Cells[10].Value = picurl;
                           
                        }));

                        if (status == false)
                            return;
                    }

                    try
                    {
                        Thread.Sleep(1000);
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].checked = true;", driver.FindElement(By.XPath("//*[text()=\"下一页\"]")));
                        Thread.Sleep(1000);
                        driver.FindElement(By.XPath("//*[text()=\"下一页\"]")).Click();
                    }
                    catch (Exception)
                    {
                       
                        continue;
                    }
                }
                catch (Exception)
                {

                    Thread.Sleep(10000);
                    continue;
                }

            }
            progressBar1.Value = progressBar1.Maximum;
            MessageBox.Show("列表抓取完成");

        }
        #endregion


        #region 模拟采集细节
        public void run_xijie_moni()
        {
            progressBar1.Value = 0;
            progressBar1.Maximum= dataGridView1.Rows.Count;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                try
                {
                    if (status == false)
                        return;
                    string asin = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    if (asin == "")
                        continue;
                    string url = "https://www.amazon.com/dp/" + asin + "/";
                    driver.Navigate().GoToUrl(url);

                    
                    Thread.Sleep(1000);
                    string html = driver.PageSource;

                    MatchCollection leimus = Regex.Matches(html, @"商品里排([\s\S]*?)</a>");
                    string sellerid = Regex.Match(html, @"seller=([\s\S]*?)&").Groups[1].Value;
                    string sellerLink = "https://www.amazon.com/sp?_encoding=UTF8&asin=&isAmazonFulfilled=1&isCBA=&marketplaceID=&orderID=&protocol=current&seller=" + sellerid + "&sshmPath=";
                    string pinpai = Regex.Match(html, @"<a id=""bylineInfo""([\s\S]*?)>([\s\S]*?)</a>").Groups[2].Value;
                    if(pinpai!="")
                    {
                        pinpai = pinpai.Replace("Visit the", "").Trim();
                    }
                    string addtocart = "无";
                    string youhuo = "无";
                    string wuliu = "FBM";
                    if (html.Contains("有货") || html.Contains("In Stock"))
                    {
                        youhuo = "有";
                    }

                    if (html.Contains("add-to-cart"))
                    {
                        addtocart = "有";
                        youhuo = "有";
                    }

                    if (html.Contains("<span class=\"a-size-small\">Amazon"))
                    {
                        wuliu = "FBA";
                    }
                    string bigleimu = "";
                    string smallleimu = "";
                    if (leimus.Count > 1)
                    {
                        bigleimu = Regex.Replace(leimus[0].Groups[1].Value, "<[^>]+>", "").ToString();
                        smallleimu = Regex.Replace(leimus[1].Groups[1].Value, "<[^>]+>", "").ToString();


                        bigleimu = Regex.Match(bigleimu, @"第([\s\S]*?)名").Groups[1].Value;
                        smallleimu = Regex.Match(smallleimu, @"第([\s\S]*?)名").Groups[1].Value;
                        if(bigleimu!="")
                        {
                            bigleimu = bigleimu.Replace(",","");
                        }
                        if (smallleimu != "")
                        {
                            smallleimu = smallleimu.Replace(",", "");
                        }
                    }


                    //子线程中
                    this.Invoke(new InvokeHandler(delegate ()
                    {
                        progressBar1.Value++;
                        dataGridView1.Rows[i].Cells[11].Value = bigleimu;
                        dataGridView1.Rows[i].Cells[12].Value = smallleimu;
                        dataGridView1.Rows[i].Cells[13].Value = sellerLink;

                        dataGridView1.Rows[i].Cells[14].Value = youhuo;
                        dataGridView1.Rows[i].Cells[15].Value = addtocart;
                        dataGridView1.Rows[i].Cells[16].Value = wuliu;
                        dataGridView1.Rows[i].Cells[17].Value = pinpai;
                    }));

                    if (status == false)
                        return;

                }
                catch (Exception)
                {

                    continue;
                }

            }
            progressBar1.Value = progressBar1.Maximum;
            MessageBox.Show("列表抓取完成");
        }
        #endregion

        int index { get; set; }
        string cookie = "session-id=147-3224729-6027332; session-id-time=2082787201l; i18n-prefs=USD; lc-main=en_US; ubid-main=133-2258364-8692801; session-token=KDqk+y3UXy8qOdfLB8DonvGIx8mkT9sSyJnGoDYeqSm+nwCHqhiN1NsgBHJs07GDw8DX8TnizQGWRi17H0YagbvFngjxcBGu8KUZ8Uwz1MI7RaLcR7IT8np9j5uJRk6Ib9PsfqwXNybR9i239XjecLTLLW0vWmwLexhXrkqpa2ci7PW4HJa+HClbrF+I1UEG; csm-hit=tb:06REF2EJ5YA9F0RJPDFJ+s-06REF2EJ5YA9F0RJPDFJ|1634004953556&t:1634004953556&adb:adblk_no";
        private delegate void InvokeHandler();

        #region 协议版
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {

            int pageall = 12;

            for (int page = 1; page < pageall; page++)
            {
                try
                {
                    cookie = "session-id=147-3224729-6027332; session-id-time=2082787201l; i18n-prefs=USD; lc-main=en_US; ubid-main=133-2258364-8692801; AMCV_7742037254C95E840A4C98A6%40AdobeOrg=1585540135%7CMCIDTS%7C18916%7CMCMID%7C17365472170500899173413100713897947316%7CMCAAMLH-1634903551%7C11%7CMCAAMB-1634903551%7CRKhpRz8krg2tLO6pguXWp5olkAcUniQYPHaMWWgdJ3xzPWQmdj0y%7CMCOPTOUT-1634305952s%7CNONE%7CMCAID%7CNONE%7CvVersion%7C4.4.0; aws-target-data=%7B%22support%22%3A%221%22%7D; aws-target-visitor-id=1634298757202-19151.32_0; aws-ubid-main=677-7251680-8776118; remember-account=false; aws-userInfo-signed=eyJ0eXAiOiJKV1MiLCJrZXlSZWdpb24iOiJ1cy1lYXN0LTEiLCJhbGciOiJFUzM4NCIsImtpZCI6ImQ4NWNkZjU1LTcxNDEtNDE0NS04YTY3LTZjYTQyZTNiZTJjYyJ9.eyJzdWIiOiIiLCJzaWduaW5UeXBlIjoiUFVCTElDIiwiaXNzIjoiaHR0cDpcL1wvc2lnbmluLmF3cy5hbWF6b24uY29tXC9zaWduaW4iLCJrZXliYXNlIjoia3ZiSlNqVk0ySnpZM2dNMGZONWJnOFN6dkRSU1VRMHVFQnVZYml4bUhGaz0iLCJhcm4iOiJhcm46YXdzOmlhbTo6NDQyNDA3MzU2MjUxOnJvb3QiLCJ1c2VybmFtZSI6Inpob3UxNDc1MjQ3OSJ9.FajZrI81UC65jgqcvWxY9aEgelZE_EcxPrJvpanM4rkODcWOa19O3q3mfyqinCkpo_k9ZCNy6CK9G-Hz9qPydlJdcP5cU4fJT4UHWR4kPecbKHWPOpSyNPIOmspxBWNr; aws-userInfo=%7B%22arn%22%3A%22arn%3Aaws%3Aiam%3A%3A442407356251%3Aroot%22%2C%22alias%22%3A%22%22%2C%22username%22%3A%22zhou14752479%22%2C%22keybase%22%3A%22kvbJSjVM2JzY3gM0fN5bg8SzvDRSUQ0uEBuYbixmHFk%5Cu003d%22%2C%22issuer%22%3A%22http%3A%2F%2Fsignin.aws.amazon.com%2Fsignin%22%2C%22signinType%22%3A%22PUBLIC%22%7D; regStatus=pre-register; session-token=90xdjkeOQYPjycBUhbtY+ruDUZv7JCdMG+APBxRJHDw6V3BMtFnQBWc4E7sgFmAb4Iye6wUkqUI/9vgh59ZaxHn9eoBIJx0UInEiM8JDSOjrCBV4FrpsPlkPoBBjFfw50dewL/4crzLJ7uw7gEU+rLwKhsJA11aw6e1cZOeRzKskvd40oF/4zS4Ah/kVxw/+; sp-cdn=\"L5Z9:CN\"";

                    if (status == false)
                    {
                        return;
                    }

                    string url = "https://www.amazon.com/s?k=Home+Office+Desk+Chairs&i=garden&rh=n%3A3733721&page=" + page + "&_encoding=UTF8&c=ts&qid=1634002990&ts_id=3733721&ref=sr_pg_2";
                    string html = method.GetUrlWithCookie(url, cookie, "utf-8");
                    // textBox2.Text = html;
                    MatchCollection htmls = Regex.Matches(html, @"<span cel_widget_id=""MAIN-SEARCH_RESULTS([\s\S]*?)</div></div></div></div></div>");
                    // MessageBox.Show(htmls.Count.ToString());
                    //MatchCollection pagealls= Regex.Matches(html, @"aria-disabled=""true"">([\s\S]*?)</li>");

                    // if(pagealls.Count>0)
                    // {
                    //     pageall = Convert.ToInt32(pagealls[pagealls.Count - 1].Groups[1].Value);
                    // }

                    for (int i = 0; i < htmls.Count; i++)
                    {
                        string title = Regex.Match(htmls[i].Groups[1].Value, @"<span class=""a-size-base-plus a-color-base a-text-normal"">([\s\S]*?)</span>").Groups[1].Value;
                        string price = Regex.Match(htmls[i].Groups[1].Value, @"data-a-color=""base""><span class=""a-offscreen"">([\s\S]*?)</span>").Groups[1].Value;
                        string asin = Regex.Match(htmls[i].Groups[1].Value, @";asin=([\s\S]*?)&").Groups[1].Value;
                        string star = Regex.Match(htmls[i].Groups[1].Value, @"<div class=""a-row a-size-small"">([\s\S]*?)label=""([\s\S]*?)out").Groups[2].Value;
                        string comment = Regex.Match(htmls[i].Groups[1].Value, @"<span class=""a-size-base"">([\s\S]*?)</span>").Groups[1].Value;


                        //子线程中
                        this.Invoke(new InvokeHandler(delegate ()
                        {
                            this.index = this.dataGridView1.Rows.Add();
                            dataGridView1.Rows[index].Cells[0].Value = index.ToString();
                            dataGridView1.Rows[index].Cells[1].Value = asin;
                            dataGridView1.Rows[index].Cells[2].Value = asin;
                            dataGridView1.Rows[index].Cells[3].Value = asin;
                            dataGridView1.Rows[index].Cells[4].Value = asin;

                            dataGridView1.Rows[index].Cells[5].Value = title;
                            dataGridView1.Rows[index].Cells[6].Value = price;
                            dataGridView1.Rows[index].Cells[7].Value = star;
                            dataGridView1.Rows[index].Cells[8].Value = comment;
                        }));




                    }
                    Thread.Sleep(1000);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    continue;
                }
            }




        }

        #endregion
        bool status = true;
        Thread thread;


        public static void KillProcess(string processName)
        {
            foreach (Process p in Process.GetProcesses())
            {
                bool flag = p.ProcessName.Contains(processName);
                if (flag)
                {
                    try
                    {
                        p.Kill();
                        p.WaitForExit();
                        Console.WriteLine("已杀掉" + processName + "进程！！！");
                    }
                    catch (Win32Exception e)
                    {
                        Console.WriteLine(e.Message.ToString());
                    }
                    catch (InvalidOperationException e2)
                    {
                        Console.WriteLine(e2.Message.ToString());
                    }
                }
            }
        }
        private void 亚马逊抓取_Load(object sender, EventArgs e)
        {
            driver = getdriver(false);
            dataGridView1.DefaultCellStyle.ForeColor = Color.White;
            dataGridView1.DefaultCellStyle.BackColor = Color.DimGray;
            driver.Navigate().GoToUrl("https://www.amazon.com/s?dc&k=pop%20it&qid=1635408390&ref=glow_cls&refresh=1&rh=p_n_availability%3A2661601011&rnid=2661599011");
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.DarkSlateGray;

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            progressBar1.Value = 0;
            dataGridView1.Rows.Clear();
            textBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            start_btn.Enabled = true;
            status = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.DgvToTable(dataGridView1), "Sheet1", true);
        }

        private void start_btn_Click(object sender, EventArgs e)
        {
         

            #region 通用检测

            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"ZbH3Q"))            {                return;            }            #endregion

            status = true;

            if (thread == null || !thread.IsAlive)
            {
                start_btn.Enabled = false;
                thread = new Thread(run_moni);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                start_btn.Enabled = false;
                thread = new Thread(run_xijie_moni);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 亚马逊抓取_FormClosing(object sender, FormClosingEventArgs e)
        {
            KillProcess("chromedriver");
            KillProcess("Google Chrome");
        }

        private void 亚马逊抓取_FormClosed(object sender, FormClosedEventArgs e)
        {
          
        }

        private void 清除进程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KillProcess("chromedriver");
            KillProcess("Google Chrome");
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                string url = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                System.Diagnostics.Process.Start(url);
            }
            catch (Exception)
            {

                MessageBox.Show("请先选择数据");
            }

        }

        private void 清空数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void 导出数据ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.DgvToTable(dataGridView1), "Sheet1", true);
        }

        private void 打开选择链接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string url = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                System.Diagnostics.Process.Start(url);
            }
            catch (Exception)
            {

                MessageBox.Show("请先选择数据");
            }
           
        }

        private void 筛选价格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            string str = Interaction.InputBox("请输入一个数值，筛选大于此数值的产品数据", "输入价格", "30", -1, -1);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                try
                {
                    double price = Convert.ToDouble(dataGridView1.Rows[i].Cells[7].Value.ToString().Replace("US$",""));
                    if (price <= Convert.ToDouble(str))
                    {
                        dataGridView1.Rows.RemoveAt(i);
                    }
                }
                catch (Exception)
                {

                    continue;
                }

            }
        }
    }
}
