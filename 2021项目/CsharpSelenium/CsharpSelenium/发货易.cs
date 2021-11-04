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
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

namespace CsharpSelenium
{
    public partial class 发货易 : Form
    {
        public 发货易()
        {
            InitializeComponent();
        }
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }
        IWebDriver driver;

        public void run()
        {
            //IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            //string title = (string)js.ExecuteScript("return document.title");
            //MessageBox.Show(title);
            ChromeOptions options = new ChromeOptions();
        

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("Object.defineProperties(navigator, {webdriver:{get:()=>undefined}});");



            driver.Navigate().GoToUrl("https://a44.fahuoyi.com/order/index?autoSyncOrder=false");
           
            //Cookie cookie = new Cookie("SESSION", "56a252b8-0d51-49af-bd1e-c2b78e9b6f09", "", DateTime.Now.AddDays(9999));
            //Cookie cookie1 = new Cookie("acw_tc", "2760774d16191645264915734e2bfd56caa64b1b8ac3fa7bf386776286373d", "", DateTime.Now.AddDays(9999));
            //driver.Manage().Cookies.AddCookie(cookie);
            //driver.Manage().Cookies.AddCookie(cookie1);
          //  driver.Navigate().GoToUrl("https://login.taobao.com/member/login.jhtml?f=top&sub=true&redirectURL=http%3A%2F%2Ffuwu.taobao.com%2Fusing%2Fserv_use_soon.htm%3Fservice_code%3Dts-13000");
         
            js.ExecuteScript("Object.defineProperties(navigator, {webdriver:{get:()=>undefined}});");
        }

        public void getdata()
        {
            try
            {
                
                string html= Regex.Match(driver.PageSource, @"<table id=""orderTable""([\s\S]*?)pagination-bar").Groups[1].Value;
          
                MatchCollection ahtmls = Regex.Matches(html, @"<tbody>([\s\S]*?)</tbody>");

                //MatchCollection titles = Regex.Matches(driver.PageSource, @"<div class=""order-item-title""><p>([\s\S]*?)</span>");
                //MatchCollection shuliangs = Regex.Matches(driver.PageSource, @"title=""旺旺联系""([\s\S]*?)<span>([\s\S]*?)</span>");
                //MatchCollection names = Regex.Matches(driver.PageSource, @"<span id=""address([\s\S]*?)>([\s\S]*?)<");

                //MatchCollection danhaos = Regex.Matches(driver.PageSource, @"<div class=""waybill-no"">([\s\S]*?)</div>");
                //MatchCollection beizhus = Regex.Matches(driver.PageSource, @"<div class=""seller-remarks"">([\s\S]*?)</div>");
               


                for (int j = 0; j < ahtmls.Count; j++)
                {
                    MatchCollection titles = Regex.Matches(ahtmls[j].Groups[1].Value, @"<div class=""order-item-title""><p>([\s\S]*?)</span>");
                    MatchCollection shuliangs = Regex.Matches(ahtmls[j].Groups[1].Value, @"title=""旺旺联系""([\s\S]*?)<span>([\s\S]*?)</span>");
                    MatchCollection names = Regex.Matches(ahtmls[j].Groups[1].Value, @"<span id=""address([\s\S]*?)>([\s\S]*?)<");

                    MatchCollection danhaos = Regex.Matches(ahtmls[j].Groups[1].Value, @"<div class=""waybill-no"">([\s\S]*?)</div>");
                    MatchCollection beizhus = Regex.Matches(ahtmls[j].Groups[1].Value, @"<div class=""seller-remarks"">([\s\S]*?)</div>");



                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    string[] text = names[0].Groups[2].Value.Split(new string[] { "，" }, StringSplitOptions.None);

                    Match danhao = Regex.Match(danhaos[0].Groups[1].Value, @"title="""">([\s\S]*?)<");
                    Match beizhu= Regex.Match(beizhus[0].Groups[1].Value, @"<span>([\s\S]*?)<");
              
                    lv1.SubItems.Add(Regex.Replace(titles[0].Groups[1].Value, "<[^>]+>", ""));
                    lv1.SubItems.Add(shuliangs[0].Groups[2].Value);
                    lv1.SubItems.Add(text[0]);
                    lv1.SubItems.Add(text[1]);
                    lv1.SubItems.Add(text[2]);
                    lv1.SubItems.Add(danhao.Groups[1].Value);
                    lv1.SubItems.Add(beizhu.Groups[1].Value);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }



        private void 发货易_Load(object sender, EventArgs e)
        {
         
            driver = new ChromeDriver();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            run();
        }
        Thread thread;
        private void button6_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = myDLL.method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"Dwzy2DZ"))
            {

                return;
            }



            #endregion




            if (thread == null || !thread.IsAlive)
                {

                    thread = new Thread(getdata);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
