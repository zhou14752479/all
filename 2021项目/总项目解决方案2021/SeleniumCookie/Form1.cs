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

namespace SeleniumCookie
{
    public partial class Form1 : Form
    {
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



        public Form1()
        {
            InitializeComponent();
        }
       

        public void getCookies()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://wangdian.sto.cn");
            //options.AddArgument("--lang=en"); 


            StringBuilder sb;

            while (true)
            {
                sb = new StringBuilder();
                var _cookies = driver.Manage().Cookies.AllCookies;

                foreach (Cookie cookie in _cookies)
                {

                    sb.Append(cookie.Name + "=" + cookie.Value + ";");
                    // driver.Manage().Cookies.AddCookie(cookie);
                }

                if (sb.ToString().Contains("WD_SESSION"))
                {

                    break;
                }
            }
            driver.Quit();
            this.Hide();
            IniWriteValue("COOKIES","cookie", sb.ToString());
            textBox1.Text = sb.ToString();
           

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        Thread thread;

        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getCookies);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
