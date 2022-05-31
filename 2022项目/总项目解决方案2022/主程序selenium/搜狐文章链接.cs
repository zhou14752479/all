using System;
using System.Collections.Generic;
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
using myDLL;
using OpenQA.Selenium;

namespace 主程序selenium
{
    public partial class 搜狐文章链接 : Form
    {
        public 搜狐文章链接()
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

        List<string> list=new List<string>();   
        IWebDriver driver;
        public void run()
        {
            driver = function.getdriver(false, false);
            StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != "")
                {
                   

                    for (int a = 0;a < 100; a++)
                    {
                        driver.Navigate().GoToUrl(text[i]);
                        Thread.Sleep(1000);
                        driver.Navigate().Refresh();
                        string html = driver.PageSource;
                        //textBox2.Text = html;
                        MatchCollection links = Regex.Matches(html, @"<h4 class=""feed-title"">([\s\S]*?)<a href=""([\s\S]*?)\?");
                        MatchCollection times = Regex.Matches(html, @"<span class=""time"">([\s\S]*?)<");
                        for (int j = 0; j < links.Count; j++)
                        {


                            if (!list.Contains(links[j].Groups[2].Value))
                            {
                                list.Add(links[j].Groups[2].Value);
                                try
                                {
                                   
                                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                    lv1.SubItems.Add(text[i]);
                                    lv1.SubItems.Add(links[j].Groups[2].Value);
                                    lv1.SubItems.Add(times[j].Groups[1].Value);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.ToString());

                                }
                            }
                            
                        }
                        
                        Thread.Sleep(2000);
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                    }
                    
                }
                Thread.Sleep(2000);
            }
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
        }


        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {
            
            if(textBox1.Text=="")
            {
                MessageBox.Show("请导入链接");
                return;
            }
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }


        #endregion
        private void 搜狐文章链接_Load(object sender, EventArgs e)
        {
            #region 通用检测


            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"jDB8R"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion
        }
    }
}
