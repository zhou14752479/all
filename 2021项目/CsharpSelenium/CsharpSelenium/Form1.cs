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

namespace CsharpSelenium
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           

        }

        public void ceshi()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://www.baidu.com");  //driver.Url = "http://www.baidu.com"是一样的
            Thread.Sleep(1000);
            // var source = driver.PageSource;
            //driver.FindElement(By.Id("copyright")).Click();
            //driver.FindElement(By.Name("div")).SendKeys("");
            driver.FindElement(By.XPath("//*[@id=\"kw\"]")).SendKeys("测试");
            driver.FindElement(By.XPath("//*[@id=\"su\"]")).Click();
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
           
        }
    }
}
