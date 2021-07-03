using CefSharp.WinForms;
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
using MSHTML;

namespace 模拟采集谷歌版
{
    public partial class autozone : Form
    {
        public autozone()
        {
            InitializeComponent();
        }
        string html = "";
        string n = "";
        private void autozone_Load(object sender, EventArgs e)
        {
            //webBrowser1.Navigate("https://contentinfo.autozone.com/znetcs/product-info/en/US/ksm/HB200201/image/10/");
            Control.CheckForIllegalCrossThreadCalls = false;
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WB_DocumentCompleted);
            webBrowser1.ScriptErrorsSuppressed = true;
        }
        private void WB_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //if (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
            //    return;
            //if (e.Url.ToString() != webBrowser1.Url.ToString())
            //    return;
            //if (webBrowser1.IsBusy == true)
            //    return;
            //if (webBrowser1.DocumentText.Contains("</html>"))
            //{

            //    html = webBrowser1.DocumentText;
            //    run();

            //    status = true;
            //}
            //else
            //{
            //    Application.DoEvents();
            //}

            WebBrowser WebCtl = webBrowser1;
            HTMLDocument doc = (HTMLDocument)WebCtl.Document.DomDocument;
            HTMLBody body = (HTMLBody)doc.body;
            IHTMLControlRange rang = (IHTMLControlRange)body.createControlRange();
            IHTMLControlElement Img = (IHTMLControlElement)webBrowser1.Document.Images[0].DomElement; //图片地址
            Image oldImage = Clipboard.GetImage();
            rang.add(Img);
            rang.execCommand("Copy", false, null);  //拷贝到内存
            Image numImage = Clipboard.GetImage();
            numImage.Save(string.Format("D:\\images1\\{0}.jpg", n));
            status = true;
        }
        bool status = false;
        public void run()
        {
           // textBox1.Text = html;

            Match title = Regex.Match(html, @"<title>([\s\S]*?)</title>");
            Match price = Regex.Match(html, @"""price"":""([\s\S]*?)""");
            Match part = Regex.Match(html, @"Part #</span>([\s\S]*?)</span>");
            Match sku = Regex.Match(html, @"""sku"":""([\s\S]*?)""");
            Match weight = Regex.Match(html, @"""WEIGHT"":""([\s\S]*?)""");
            Match img = Regex.Match(html, @"<img src=""([\s\S]*?)""");


            if (title.Groups[1].Value != "")
            {
                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add(n);
                lv1.SubItems.Add(title.Groups[1].Value.Trim());
                lv1.SubItems.Add(price.Groups[1].Value.Trim());
                lv1.SubItems.Add(Regex.Replace(part.Groups[1].Value.Trim(), "<[^>]+>", ""));
                lv1.SubItems.Add(sku.Groups[1].Value.Trim());
                lv1.SubItems.Add(weight.Groups[1].Value.Trim());
                lv1.SubItems.Add(img.Groups[1].Value.Trim());
            }
            else
            {
                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add(n);
                lv1.SubItems.Add("空");
                lv1.SubItems.Add("空");
                lv1.SubItems.Add("空");
                lv1.SubItems.Add("空");
                lv1.SubItems.Add("空");
                lv1.SubItems.Add("空");




            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                status = false;
                n = richTextBox1.Lines[i].Trim();

                string url = "https://www.autozone.com/searchresult?searchText=" + n;

                webBrowser1.Navigate(url);

                while (status == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
                Thread.Sleep(1000);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }


        
        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                status = false;
               string url = richTextBox1.Lines[i].Trim();
                n= richTextBox2.Lines[i].Trim();
                webBrowser1.Navigate(url);

                while (status == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
                Thread.Sleep(1000);
            }
        }
    }
}
