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

namespace 淘宝搜索
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool loaded = false;   //该变量表示网页是否加载完成.默认未加载完成
        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://www.taobao.com");

            this.webBrowser1.ScriptErrorsSuppressed = true;  //屏蔽IE脚本弹出错误
            
        }

        public static string HTML;
     

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.loaded = true; //加载完成传值

            this.webBrowser1.Document.Window.Error += OnWebBrowserDocumentWindowError;
            //textBox1.Text = e.Url.ToString();
            HTML = webBrowser1.DocumentText;
        }
        private void OnWebBrowserDocumentWindowError(object sender, HtmlElementErrorEventArgs e)
        {
            e.Handled = true;
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                if (loaded == true)
                {
                    string html = HTML;
                    MatchCollection titles = Regex.Matches(html, @"""raw_title"":""(\s\S]*?)""");
                    MessageBox.Show(titles.Count.ToString());
                    for (int j = 0; j < titles.Count; j++)
                    {
                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(titles[j].Groups[1].Value);
                    }


                    HTML = "";
                   

                    HtmlDocument dc = webBrowser1.Document;
                    HtmlElementCollection es = dc.GetElementsByTagName("a");   //GetElementsByTagName返回集合
                    foreach (HtmlElement e1 in es)
                    {
                        if (e1.GetAttribute("trace") == "srp_bottom_pagedown")
                        {
                            //  e1.SetAttribute("value", textBox1.Text.Trim());
                            e1.InvokeMember("click");
                        }

                    }
                    
                }
                loaded = false;
                this.webBrowser1.DocumentCompleted += WebBrowser1_DocumentCompleted;

            }

           
            
            

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(loaded.ToString());
        }

      
    }
}
