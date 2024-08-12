using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
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

namespace CsharpSelenium
{
    public partial class 孔网已售 : Form
    {
        public 孔网已售()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入isbn");
                return;
            }

            status = true;
            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }


        #region 主程序
        public void run()
        {

            try
            {
                ChromeOptions options = new ChromeOptions();
                options.BinaryLocation = "Chrome/Application/chrome.exe";
                options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
                options.AddArgument("--disable-gpu");

                IWebDriver driver = new ChromeDriver(options);
                //driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("https://www.kongfz.com/");

                Thread.Sleep(1000);


                //登录
                // 定位到包含文本的元素，例如通过xpath或其他选择器
                IWebElement element = driver.FindElement(By.XPath("//span[contains(text(), '注册')]"));

                // 创建一个操作Builders来模拟鼠标行为
                Actions builder = new Actions(driver);

                builder.MoveToElement(element).Build().Perform();
                Thread.Sleep(1000);
                driver.FindElement(By.ClassName("login-btn")).Click();
                Thread.Sleep(1000);

                driver.SwitchTo().Frame("iframe_login");

                driver.FindElement(By.Name("username")).SendKeys(textBox2.Text);
                driver.FindElement(By.Name("password")).SendKeys(textBox3.Text);
                Thread.Sleep(1000);
                driver.FindElement(By.XPath("//*[@id=\"login\"]/div[2]/input")).Click();

                Thread.Sleep(2000);
                //登录结束

                StreamReader keysr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
                string ReadTxt = keysr.ReadToEnd();
                string[] text = ReadTxt.Split(new string[] { "\r\n" }, StringSplitOptions.None);


                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] != "")
                    {

                        try
                        {
                            label1.Text = text[i];
                          

                          
                            driver.Navigate().GoToUrl("https://search.kongfz.com/product/?dataType=1&keyword=" + Text[i] + "&sortType=10&page=1&actionPath=sortType");
                            Thread.Sleep(1000);
                            Match sale = Regex.Match(driver.PageSource, @"result-count__number"">([\s\S]*?)</span>");

                            string sales = sale.Groups[1].Value;


                            if (sales.Trim() == "")
                            {
                                sales = "无";
                            }

                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(text[i]);
                   
                            lv1.SubItems.Add("无");
                            lv1.SubItems.Add("无");
                            lv1.SubItems.Add(sales);
                        }
                        catch (Exception)
                        {

                            continue;
                        }

                    }

                    Thread.Sleep(2000);
                }

                MessageBox.Show("完成");

                // Thread.Sleep(1000);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }


        #endregion


        Thread thread;
        bool zanting = true;
        bool status = true;

        private void 孔网已售_Load(object sender, EventArgs e)
        {

        }
    }
}
