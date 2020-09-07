using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 模拟采集
{
    public partial class 拼多多榜单 : Form
    {
        public 拼多多榜单()
        {
            InitializeComponent();
        }

        private void 拼多多榜单_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WB_DocumentCompleted);
            webBrowser1.Navigate("https://mobile.yangkeduo.com/");
        }
        private void WB_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                return;
            if (e.Url.ToString() != webBrowser1.Url.ToString())
                return;

            if (webBrowser1.DocumentText.Contains("</html>"))
            {

                html = webBrowser1.Document.Body.OuterHtml;
                run();
            }
            else
            {
                Application.DoEvents();
            }
        }

        public string html = "";


        bool status = false;
        public void run()
        {
           
            MatchCollection ids = Regex.Matches(html, @"data-ranking-goods-id=""([\s\S]*?)""");
           
            if (ids.Count != 0)
            {
                for (int j = 0; j < ids.Count; j++)
                {

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(ids[j].Groups[1].Value);

                }

                status = true;
            }

            


        }
        private void button3_Click(object sender, EventArgs e)
        {
           
            //string url = comboBox1.Text.Trim();

            //webBrowser1.Navigate(url);
            webBrowser1.Navigate("https://mobile.yangkeduo.com/pincard_share_card_popup.html?categoryId=39245&refer_origin_page_sn=17542&refer_page_name=list_channel&refer_page_id=67387_1598790596801_yaudndkbba&refer_page_sn=67387&refer_abtest_info=%7B%7D&page_id=67388_1599359105078_h03t9y94mj&list_id=hDET3469Bd&bc_cached_html=1&build_version=1599203566&sp=0&is_back=1");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            comboBox1.Text = webBrowser1.Url.ToString();

           
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            if (webBrowser1.Document.ActiveElement != null)
            {
                webBrowser1.Navigate(webBrowser1.Document.ActiveElement.GetAttribute("href"));
                comboBox1.Text = webBrowser1.Document.ActiveElement.GetAttribute("href");
            }
        }




        private void button1_Click(object sender, EventArgs e)
        {
            //HtmlDocument doc = webBrowser1.Document;
            //HtmlElementCollection es = doc.GetElementsByTagName("div");
            //foreach (HtmlElement e1 in es)
            //{
            //    if (e1.GetAttribute("data-uniqid") == "49")
            //    {
            //        e1.InvokeMember("click");
            //    }

            //}

            HtmlDocument doc = webBrowser1.Document;
            HtmlElementCollection es = doc.GetElementsByTagName("p");
            foreach (HtmlElement e1 in es)
            {
                if (e1.GetAttribute("class")== "_2uIR36c6")
                {
                    MessageBox.Show(e1.InnerText);
                    e1.InvokeMember("click");

                    if (status==true)
                    {

                        webBrowser1.GoBack();
                    }
                    else
                    {
                        Application.DoEvents();
                    }
                }



            }

        }

       
    }
}
