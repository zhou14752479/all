using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;
using MySql.Data.MySqlClient;

namespace 通用项目
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            method.SetWebBrowserFeatures(method.IeVersion.IE10);
            webBrowser1.ScriptErrorsSuppressed = true;
        }

        public static string COOKIE="";

        private void button1_Click(object sender, EventArgs e)
        {
            string constr = "Host =47.99.68.92;Database=vip_database;Username=root;Password=zhoukaige00.@*.";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();

            MySqlCommand cmd = new MySqlCommand("select * from vip where username='QQ空间'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

            MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

            if (reader.Read())
            {

                string password = reader["password"].ToString().Trim();

                if (password != "QQ空间")

                {
                    MessageBox.Show("验证失败");

                    Environment.Exit(0);
                }


                run();
               

            }
        }

       
       
        private void Button6_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        
        bool loading = true;   //该变量表示网页是否正在加载.

        void browser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
           

                loading = false;//在加载完成后,将该变量置为false,下一次循环随即开始执行.
           
        }
        public void run()
        {
            try
            {
               
                StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                
                for (int i = 0; i < array.Length; i++)
                {

                    WebBrowser browser = new WebBrowser();
                    browser.ScriptErrorsSuppressed = true;
                    browser.Navigated += new WebBrowserNavigatedEventHandler(browser_Navigated);
                    loading = true;  //表示正在加载
                    browser.Navigate("https://user.qzone.qq.com/"+array[i]);

                        textBox3.Text += "正在访问" + array[i] + "\r\n";

                        while (loading)
                        {
                            Application.DoEvents();//等待本次加载完毕才执行下次循环.
                        }

                    while (zanting == false)
                    {
                        Application.DoEvents();//等待本次加载完毕才执行下次循环.
                    }
                    if (status == false)
                    {
                        return;
                    }
                    Thread.Sleep(1000);

                }

                
                

            }
            catch (Exception)
            {

                throw;
            }
        }
        bool zanting = true;

        bool status = true;
        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri("https://i.qq.com/");
        }

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
           
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
