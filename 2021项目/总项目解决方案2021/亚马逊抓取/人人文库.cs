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

namespace 亚马逊抓取
{
    public partial class 人人文库 : Form
    {
        public 人人文库()
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
        private delegate void InvokeHandler();
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

      

        IWebDriver driver;
        #region 模拟登录
        public void run_moni()
        {

            //driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);
          
            Thread.Sleep(1000);

            for (int i= 0; i< listView1.Items.Count; i++)
            {
               
                try
                {
                    string user = listView1.Items[i].SubItems[1].Text.Trim();
                    string pass = listView1.Items[i].SubItems[2].Text.Trim();
                    driver.Navigate().GoToUrl("https://www.renrendoc.com/login.aspx?returl=%3freturl%3dhttps%253a%252f%252fm.renrendoc.com%252f");
                    Thread.Sleep(1000);
                    driver.FindElement(By.Id("nav1")).Click();
                    Thread.Sleep(500);
                    driver.FindElement(By.Id("Content_txtUserName")).SendKeys(user);
                    driver.FindElement(By.Id("Content_txtPassword")).SendKeys(pass);
                    driver.FindElement(By.Id("Content_btnLogin")).Click();
                    label1.Text = "正在查询："+user;
                    StringBuilder sb;

                    while (true)
                    {
                        sb = new StringBuilder();
                        var _cookies = driver.Manage().Cookies.AllCookies;

                        foreach (OpenQA.Selenium.Cookie cookie in _cookies)
                        {
                            sb.Append(cookie.Name + "=" + cookie.Value + ";");
                           
                        }
                        if (sb.ToString().Contains("SessionId"))
                        {
                            break;
                        }
                    }
                    
                    string html = method.GetUrlWithCookie("https://www.renrendoc.com/UserManage/UserScore.aspx", sb.ToString(), "utf-8");
                    string jifen = Regex.Match(html, @"账户积分余额：([\s\S]*?)</span>").Groups[1].Value;
                    jifen = Regex.Replace(jifen, "<[^>]+>", "").Trim();

                    string ahtml = method.GetUrlWithCookie("https://www.renrendoc.com/FlexPaper/BookList.aspx", sb.ToString(), "utf-8");
                    string doc = Regex.Match(ahtml, @"共<span style='color: #FF7713;'>([\s\S]*?)</span>").Groups[1].Value;
                   

                    string lastjifen = "0";
                    string lastdoc = "0";
                    
                    if (ExistINIFile())
                    {
                         lastdoc= IniReadValue("doc", user);
                        lastjifen = IniReadValue("jifen", user);
                    }

                    if(lastdoc=="")
                    {
                        lastdoc = "0";
                    }
                    if (doc == "")
                    {
                       doc = "0";
                    }
                    if (lastjifen == "")
                    {
                        lastjifen = "0";
                    }
                    if (jifen == "")
                    {
                        jifen = "0";
                    }
                    IniWriteValue("doc", user, doc);
                    IniWriteValue("jifen", user, jifen);
                    listView1.Items[i].SubItems[3].Text = lastdoc;
                    listView1.Items[i].SubItems[4].Text = doc;
                    listView1.Items[i].SubItems[5].Text = (Convert.ToInt32(doc)-Convert.ToInt32(lastdoc)).ToString();
                    listView1.Items[i].SubItems[6].Text = lastjifen;
                    listView1.Items[i].SubItems[7].Text = jifen;
                    listView1.Items[i].SubItems[8].Text = (Convert.ToDouble(jifen)-Convert.ToDouble(lastjifen)).ToString();
                    listView1.Items[i].SubItems[9].Text = sb.ToString();
                    driver.Manage().Cookies.DeleteAllCookies();
                }
                catch (Exception ex)
                {

                    
                   MessageBox.Show(ex.ToString());
                }

            }
          
            MessageBox.Show("查询完成");
            double a = 0;
            double b = 0;
            double c = 0;
            double d = 0;
            double e = 0;
            double f = 0;


            for (int i = 0; i <listView1.Items.Count; i++)
            {
                a =a+ Convert.ToDouble(listView1.Items[i].SubItems[3].Text);
                b =  b+ Convert.ToDouble(listView1.Items[i].SubItems[4].Text);
                c = c + Convert.ToDouble(listView1.Items[i].SubItems[5].Text);
                d =d + Convert.ToDouble(listView1.Items[i].SubItems[6].Text);
                e= e + Convert.ToDouble(listView1.Items[i].SubItems[7].Text);
                f = f + Convert.ToDouble(listView1.Items[i].SubItems[8].Text);

            }

            ListViewItem lv= listView1.Items.Add((listView1.Items.Count + 1).ToString());
            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString());
            lv1.SubItems.Add("");
            lv1.SubItems.Add("合计");

            lv1.SubItems.Add(a.ToString());
            lv1.SubItems.Add(b.ToString());
            lv1.SubItems.Add(c.ToString());
            lv1.SubItems.Add(d.ToString());
            lv1.SubItems.Add(e.ToString());
            lv1.SubItems.Add(f.ToString());

         
        }
        #endregion

        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {

            #region 通用检测

            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"iSyck"))            {                return;            }            #endregion

          

            if (thread == null || !thread.IsAlive)
            {
               
                thread = new Thread(run_moni);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        public void getdata()
        {
            listView1.Items.Clear();
            StreamReader sr = new StreamReader(path + "data.txt", method.EncodingType.GetTxtType(path + "data.txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {

                string[] value = text[i].Split(new string[] { "----" }, StringSplitOptions.None);

                if (value.Length > 1)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据                                                     
                    lv1.SubItems.Add(value[0].Trim());
                    lv1.SubItems.Add(value[1].Trim());
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                }
            }
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
        }
        private void 人人文库_Load(object sender, EventArgs e)
        {
            getdata();
            driver = getdriver(false);
         
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            IWebDriver driver = getdriver(false);
            ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            ChromeOptions options = new ChromeOptions();
            options.BinaryLocation = "Chrome/Application/chrome.exe";
            options.AddArgument("--disable-gpu");
            driver.Navigate().GoToUrl("https://www.renrendoc.com/UserManage/UserDefault.aspx");
            string cookie= listView1.SelectedItems[0].SubItems[9].Text;
            string[] text = cookie.Split(new string[] { ";" }, StringSplitOptions.None);
            foreach (var item in text)
            {
                if (item.Contains("+"))
                {
                    string[] value = item.Split(new string[] { "=" }, StringSplitOptions.None);
                    MessageBox.Show(value[0]);
                    MessageBox.Show(item.Replace(value[0] + "=", ""));
                    Cookie cookie2 = new Cookie(value[0], item.Replace(value[0] + "=", ""), "", DateTime.Now.AddDays(9999));
                    driver.Manage().Cookies.AddCookie(cookie2);
                }
            }
            Thread.Sleep(200);
            driver.Navigate().GoToUrl("https://www.renrendoc.com/UserManage/UserDefault.aspx");

        }
    }
}
