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

namespace CsharpSelenium
{
    public partial class 琴房预约查找 : Form
    {
        public 琴房预约查找()
        {
            InitializeComponent();
          
        }
        Thread thread;
      
        bool status = true;

        #region 琴房预约
        public void run()
        {

            ChromeOptions options = new ChromeOptions();
            IWebDriver driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();

            

            for (int i = 0; i <urls.Count; i++)
            {
                string url = urls[i];
                if (url != "")
                {
                    driver.Navigate().GoToUrl(url);
                    driver.FindElement(By.Id("menu-btn")).Click();
                    Thread.Sleep(2000);
                   
                    if (driver.PageSource.Contains(textBox2.Text.Trim()) && textBox2.Text.Trim()!="")
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(url);
                    }

                    if (status == false)
                        return;
                }
            }

            MessageBox.Show("查询完成");


        }
        #endregion

        List<string> urls = new List<string>();
        private void button6_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"3Q2juu"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion
            status = true;
            string path = AppDomain.CurrentDomain.BaseDirectory;
            if (!File.Exists(path + "网址.txt"))
            {
                MessageBox.Show("文件夹内不存在网址.txt");
                return;
            }
         
            StreamReader sr = new StreamReader(path+"网址.txt", Encoding.Default);
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                //richTextBox1.AppendText(text[i]+"\r\n");
                urls.Add(text[i]);
            }
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存

            if (thread == null || !thread.IsAlive)
            {
                timer1.Start();
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;

          
        }
       
        private void 琴房预约查找_Load(object sender, EventArgs e)
        {
          
            //options.AddArgument("--lang=en"); 


          
           
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView listview = (ListView)sender;
            ListViewItem lstrow = listview.GetItemAt(e.X, e.Y);
            System.Windows.Forms.ListViewItem.ListViewSubItem lstcol = lstrow.GetSubItemAt(e.X, e.Y);
            string strText = lstcol.Text;
            try
            {
                Clipboard.SetDataObject(strText);
                string info = string.Format("内容【{0}】已经复制到剪贴板", strText);
                MessageBox.Show("复制成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                sb.Append(listView1.Items[i].SubItems[1].Text+"\r\n");
            }

            try
            {
                Clipboard.SetDataObject(sb.ToString());
               
                MessageBox.Show("复制成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
