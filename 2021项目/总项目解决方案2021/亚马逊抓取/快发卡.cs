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
using myDLL;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;

namespace 亚马逊抓取
{
    public partial class 快发卡 : Form
    {
        public 快发卡()
        {
            InitializeComponent();
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


        IWebDriver driver;
        bool status = true;
        bool zanting = true;
        Thread thread;
        private void 快发卡_Load(object sender, EventArgs e)
        {
            driver = getdriver(false);
            driver.Navigate().GoToUrl("https://www.kuaifaka.net/order_sel");
        }


        string dingdanhao = "";
        public void run()
        {
            label1.Text = "正在查询";
            try
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, method.EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
                for (int i = 0; i < text.Length; i++)
                {
                    if (status == false)
                        return;
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    driver.FindElement(By.ClassName("input_text")).Clear();
                    driver.FindElement(By.ClassName("input_text")).SendKeys(text[i].ToString());
                    Thread.Sleep(500);
                    driver.FindElement(By.ClassName("sele_btn")).Click();
                    Thread.Sleep(2000);
                    string dingdan = Regex.Match(driver.PageSource, @"订单号：([\s\S]*?)<").Groups[1].Value;
                  
                    if (dingdan!="")
                    {
                        if (dingdan != dingdanhao)
                        {

                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(text[i]);
                            label1.Text = text[i] + "：查询到订单，已记录";
                            lv1.SubItems.Add(dingdan);
                            dingdanhao = dingdan;
                        }

                    }
                    else
                    {

                        label1.Text = text[i]+"：未查询到订单，跳过";
                    }

                   
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString()) ;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            #region 通用检测

            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"CUoFi"))            {                return;            }            #endregion

            status = true;

            if (thread == null || !thread.IsAlive)
            {
               
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                textBox1.Text = openFileDialog1.FileName;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }
    }
}
