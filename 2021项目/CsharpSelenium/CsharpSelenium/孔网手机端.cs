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
    public partial class 孔网手机端 : Form
    {
        public 孔网手机端()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
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
               

                StreamReader keysr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
                string ReadTxt = keysr.ReadToEnd();
                string[] text = ReadTxt.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                driver.Navigate().GoToUrl("https://login.kongfz.com/Mobile/Login/index?mustLogin=1&returnUrl=https%3A%2F%2Fm.kongfz.com%2Fnewsearch%2Fresult%3Fkeyword%3D9787219116654%26type%3Dgoods%26choosetext%3D%25E6%258B%258D%25E5%2593%2581%25E6%25A0%2587%25E9%25A2%2598");
                //Thread.Sleep(1000);
                //driver.FindElement(By.Name("user")).SendKeys("17606117606");
                //driver.FindElement(By.Name("password")).SendKeys("zhoukaige00");
                //Thread.Sleep(1000);
                //driver.FindElement(By.XPath("//span[contains(text(), '登录')]")).Click();
                Thread.Sleep(30000);
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] != "")
                    {

                        try
                        {

                            driver.Navigate().GoToUrl("https://m.kongfz.com/newsearch/result?keyword="+text[i]+"&type=goods&choosetext=%E6%8B%8D%E5%93%81%E6%A0%87%E9%A2%98");

                            Thread.Sleep(1000);

                            // 定位到包含文本的元素，例如通过xpath或其他选择器
                            IWebElement element = driver.FindElement(By.XPath("//span[contains(text(), '价格')]"));
                            element.Click();    
                          
                            Thread.Sleep(1000);

                            IWebElement element2 = driver.FindElement(By.XPath("//span[contains(text(), '总价从低到高（含运费）')]"));
                            element2.Click();
                            Thread.Sleep(1000);
                            label1.Text = text[i];

                            MatchCollection prices = Regex.Matches(driver.PageSource, @"<div class=""price"">([\s\S]*?)</div>");
                            MatchCollection fees = Regex.Matches(driver.PageSource, @"<div class=""price_phase price_phase2"">([\s\S]*?)</div>");


                            string p1 = "无";
                            string f1 = "无";
                            string p3 = "无";
                            string f3 = "无";
                            if (prices.Count==0)
                            {

                            }
                            else if (prices.Count==1)
                            {
                                p1 = prices[0].Groups[1].Value.Replace("<span>", "").Replace("</span>", "").Replace("￥", "").Trim();
                                f1 = fees[0].Groups[1].Value.Replace("<span>", "").Replace("挂号印刷品", "").Replace("快递", "").Trim();
                              
                            }
                            else
                            {
                                p1 = prices[0].Groups[1].Value.Replace("<span>", "").Replace("</span>", "").Replace("￥", "").Trim();
                                f1 = fees[0].Groups[1].Value.Replace("<span>", "").Replace("挂号印刷品", "").Replace("快递", "").Trim();
                                p3 = prices[2].Groups[1].Value.Replace("<span>", "").Replace("</span>", "").Replace("￥", "").Trim();
                                f3 = fees[2].Groups[1].Value.Replace("<span>", "").Replace("挂号印刷品", "").Replace("快递", "").Trim();
                            }


                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(text[i]);

                            lv1.SubItems.Add(p1);
                            lv1.SubItems.Add(f1);
                            lv1.SubItems.Add(p3);
                            lv1.SubItems.Add(f3);
                           
                           
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
       
        bool status = true;
    }
}
