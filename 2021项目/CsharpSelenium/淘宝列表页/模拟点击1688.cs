using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotRas;
using myDLL;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace 淘宝列表页
{
    public partial class 模拟点击1688 : Form
    {
        public 模拟点击1688()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

   
      
    
        public void run()
        {
            try
            {
                ChromeOptions options = new ChromeOptions();
                IWebDriver driver = new ChromeDriver(options);
                //driver.Manage().Window.Maximize();
                string loginurl = "https://m.1688.com/search.html";

                driver.Navigate().GoToUrl(loginurl);

                StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Replace("\"","").Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                    string[] value = text[i].Split(new string[] { "," }, StringSplitOptions.None);
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(value[0]);
                    lv1.SubItems.Add(value[1]);
                    lv1.SubItems.Add(value[2]);

                    string url = "https://m.1688.com/offer_search/-6D7033.html?keywords="+value[1];
                    driver.Navigate().GoToUrl(url);
                    Thread.Sleep(1000);
                    driver.Navigate().GoToUrl(value[2]);
                    Thread.Sleep(1000);
                    lv1.SubItems.Add("成功");
                    if (checkBox1.Checked == true)
                    {
                        Method.Unlink();
                        Thread.Sleep(1000);
                        Method.boolLink();
                    }
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
               
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }


        public static class Method
        {
            /// <summary>
            /// 断开连接
            /// </summary>
            public static void Unlink()
            {
                ReadOnlyCollection<RasConnection> conList = RasConnection.GetActiveConnections();
                foreach (RasConnection con in conList)
                {
                    con.HangUp();
                }
            }

            /// <summary>
            /// 宽带连接
            /// </summary>
            /// <returns></returns>
            public static bool boolLink()
            {
                try
                {
                    RasDialer dialer = new RasDialer();
                    dialer.EntryName = "宽带连接";
                    dialer.PhoneNumber = " ";
                    dialer.AllowUseStoredCredentials = true;
                    dialer.PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.AllUsers);
                    dialer.Timeout = 2000;
                    dialer.Dial();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return false;
                }
                return true;
            }
        }







        private void 模拟点击1688_Load(object sender, EventArgs e)
        {

        }
        Thread thread;
        bool zanting = true;
        bool status = true;
      
        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("未导入文件");
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

        private void button2_Click(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Method.Unlink();
            Method.boolLink();
        }

       
    }
}
