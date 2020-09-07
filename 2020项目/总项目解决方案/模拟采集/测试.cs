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
    public partial class 测试 : Form
    {
        public 测试()
        {
            InitializeComponent();
        }

        private void 测试_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            //webBrowser1.Navigate("https://m.500fc99.com/openCenter/pastOpen?lcode=1407");

            webBrowser1.Navigate("https://mobile.yangkeduo.com/pincard_share_card_popup.html?categoryId=39245&refer_origin_page_sn=17542&refer_page_name=list_channel&refer_page_id=67387_1598790596801_yaudndkbba&refer_page_sn=67387&refer_abtest_info=%7B%7D&page_id=67388_1599359105078_h03t9y94mj&list_id=hDET3469Bd&bc_cached_html=1&build_version=1599203566&sp=0&is_back=1");
        }

        public void run()
        {
            var htmldocument = (mshtml.HTMLDocument)webBrowser1.Document.DomDocument;

            string html = htmldocument.documentElement.outerHTML;
            MatchCollection qishus = Regex.Matches(html, @"lottery-name""data-v-3648a589 = """">([\s\S]*?)期");
            MatchCollection values = Regex.Matches(html, @"Dice Dice([\s\S]*?)""");

            for (int i = 0; i < values.Count; i=i++)
            {
                textBox1.Text += Convert.ToInt32(values[i].Groups[1].Value) + Convert.ToInt32(values[i+1].Groups[1].Value) + Convert.ToInt32(values[i+2].Groups[1].Value) + "\r\n";
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            var htmldocument = (mshtml.HTMLDocument)webBrowser1.Document.DomDocument;

            string html = htmldocument.documentElement.outerHTML;
            textBox1.Text = html;

        }
        
     

        private void button4_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string url = comboBox1.Text.Trim();

            webBrowser1.Navigate(url);
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

        private void button2_Click(object sender, EventArgs e)
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
           
           
        }
    }
}
