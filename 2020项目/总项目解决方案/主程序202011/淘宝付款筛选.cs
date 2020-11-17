using System;
using System.Collections;
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
using helper;
using static helper.method;

namespace 主程序202011
{
    public partial class 淘宝付款筛选 : Form
    {
        public 淘宝付款筛选()
        {
            InitializeComponent();
        }
        ArrayList finishes = new ArrayList();
        public void run()
        {
           
            if (html.Contains("付款"))
            {

               
                MatchCollection price = Regex.Matches(html, @"fixed=""false"">([\s\S]*?)</span>");
                html = "";
                if (!finishes.Contains(uid))
                {
                    
                    finishes.Add(uid);
                    if (Convert.ToInt32(price[2].Groups[1].Value.Replace("人付款","")) >= Convert.ToInt32(textBox2.Text))
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(uid);
                    lv1.SubItems.Add(price[2].Groups[1].Value);
                    lv1.SubItems.Add(price[1].Groups[1].Value);
                     }
                    if (status == false)
                    {
                        return;
                    }
                   
                }
               
            }
           
        }

        public void main()
        {
            StreamReader keysr = new StreamReader(textBox1.Text, EncodingType.GetTxtType(textBox1.Text));

            string ReadTxt = keysr.ReadToEnd();
            string[] text = ReadTxt.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string uid in text)
            {
               
                    if (uid != "")
                {
                  
                        this.uid = uid;
                    label1.Text = "正在查询："+uid;
                   status = false;
                    webBrowser1.Navigate("https://market.m.taobao.com/app/find-like/find-like/pages/index?&appid=9709&nid="+uid);
                   

                    while (this.status == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    while (zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    Thread.Sleep(3000);
                 
                }
            }

           label1.Text= "查询结束";
            MessageBox.Show("查询结束");
           
        }
     
        bool zanting = true;
        Thread thread;
        string uid = "";
        private void button1_Click(object sender, EventArgs e)
        {
           
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"taobaoshaixuan"))
            {
                MessageBox.Show("");
                return;
            }

            #endregion

            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入商品ID");
                return;
            }
            if (thread == null || !thread.IsAlive)
            {
                timer1.Start();
                thread = new Thread(main);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        bool status = false;
        public static string html; //网页源码传值
        private void WB_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            if (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                return;
            if (e.Url.ToString() != webBrowser1.Url.ToString())
                return;
            var htmldocument = (mshtml.HTMLDocument)webBrowser1.Document.DomDocument;
            html = htmldocument.documentElement.outerHTML;
          
            status = true;
        }
        private void 淘宝付款筛选_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WB_DocumentCompleted);
        }

        private void button5_Click(object sender, EventArgs e)
        {
          
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
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

        private void button6_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            html = "";
            listView1.Items.Clear();

            finishes.Clear();

            webBrowser1.Url = null;
            webBrowser1.Document.OpenNew(false);
            //webBrowser1 = null;
        }
        Thread thread1;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (webBrowser1.Document != null)
            {
                var htmldocument = (mshtml.HTMLDocument)webBrowser1.Document.DomDocument;
                html = htmldocument.documentElement.outerHTML;
               
                if (thread1 == null || !thread1.IsAlive)
                {
                    thread1 = new Thread(run);
                    thread1.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
                
            }
        }
    }
}
