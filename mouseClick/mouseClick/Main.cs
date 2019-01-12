using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mouseClick
{
    public partial class Main : Form
    {



        [DllImport("user32.dll", EntryPoint = "MessageBoxA")]  //若要使用其它函数名，可以使用EntryPoint属性设置。
        static extern int MsgBox(int hWnd, string msg, string caption, int type);
        public Main()
        {
            InitializeComponent();
            webBrowser1.ScriptErrorsSuppressed = true;
            Control.CheckForIllegalCrossThreadCalls = false;

        }
        bool loaded = false;   //该变量表示网页是否正在加载.

        bool status = true;


        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //webBrowser1.Url = new Uri("https://www.linkedin.com/search/results/companies/?keywords=fashion%20accessory&origin=SWITCH_SEARCH_VERTICAL&page=2");


        }

        private void Document_Click(Object sender, HtmlElementEventArgs e)
        {
            if (webBrowser1.Document != null)
            {
                HtmlElement elem = webBrowser1.Document.GetElementFromPoint(e.ClientMousePosition);
                elem.ScrollIntoView(true);
                MessageBox.Show(elem.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {


            HtmlDocument doc = this.webBrowser1.Document;
            HtmlElementCollection nexts = doc.GetElementsByTagName("a");

            foreach (HtmlElement next in nexts)
            {
                if (next.InnerText == "下一页")   //如果没有ID或者特异性的标志，通过标签的值去找标签<a>下一页</a>
                {

                    next.InvokeMember("click");
                }


            }
            //  MsgBox(0, " 这就是用 DllImport 调用 DLL 弹出的提示框哦！ ", " 挑战杯 ", 0x30);


        }



        #region 领英网
        public void run()

        {


            try
            {
                //string[] lists = textBox1.Text.Split(',');
                //foreach (string list in lists)
                //{

                for (int i = 0; i < 2; i++)
                {


                    String url = "https://www.linkedin.com/search/results/companies/?keywords=fashion%20accessory&origin=SWITCH_SEARCH_VERTICAL&page=" + i;

                    webBrowser1.Navigate(url);

                    while (this.loaded == false)
                    {
                        Application.DoEvents();//等待本次加载完毕才执行下次循环.
                    }

                    MatchCollection matches = Regex.Matches(textBox2.Text, @"urn:li:company:([\s\S]*?)&");

                    ArrayList lists = new ArrayList();

                    foreach (Match match in matches)
                    {
                        lists.Add("https://www.linkedin.com/company/" + match.Groups[1].Value);
                    }

                    if (lists.Count > 0)

                    { 

                    for (int j = 0; j < lists.Count; j++)
                        {

                           // MessageBox.Show((2*j).ToString());
                            //webBrowser1.Url = new Uri(lists[2*j].ToString());

                            webBrowser1.Navigate(lists[2 * j].ToString());
                            while (this.loaded== false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }

                            string html = textBox2.Text;
                            if (this.status == false)
                                return;

                            Match codes = Regex.Match(html, @"localizedName&quot;:&quot;([\s\S]*?)&");


                            ListViewItem lv1 = listView1.Items.Add(codes.Groups[1].Value.Trim()); //使用Listview展示数据
                                                                                                  // lv1.SubItems.Add("");


                            Thread.Sleep(1000);

                        }
                }
                }

            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }





        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            //Thread thread = new Thread(new ThreadStart(run));
            //thread.Start();
            run();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            StreamReader reader = new StreamReader(webBrowser1.DocumentStream, Encoding.GetEncoding(webBrowser1.Document.Encoding));
            string html = reader.ReadToEnd();
            textBox2.Text = html;
            this.loaded = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.status = false;
        }





        //www.baidu.com来做测试
        //HtmlDocument doc = this.webBrowser1.Document;
        //HtmlElement keyword = doc.GetElementById("kw");
        //keyword.InnerText = "冰川时代";

        //HtmlElement ele = doc.GetElementById("su");
        //ele.InvokeMember("click");


        //doc.GetElementById("su").InvokeMember("click");

        //HtmlElementCollection es = doc.GetElementsByTagName("a"); //GetElementsByTagName返回集合

        //foreach (HtmlElement e1 in es)
        //{
        //    if (e1.GetAttribute("name").ToLower() == "j_username")
        //    {
        //        //e1.SetAttribute("value", textBox3.Text.Trim());
        //        e1.InvokeMember("click");
        //    }

        //}


        //doc.GetElementFromPoint();
    }
}
