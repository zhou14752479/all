using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeleniumCookie
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        IWebDriver driver = new ChromeDriver();

        public string getCookies()
        {
            driver.Navigate().GoToUrl("https://qzone.qq.com/");
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

                if (sb.ToString().Contains("skey"))
                {

                    break;
                }
            }
            driver.Quit();
            textBox1.Text = sb.ToString();
            return sb.ToString();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            getCookies();
        }
    }
}
