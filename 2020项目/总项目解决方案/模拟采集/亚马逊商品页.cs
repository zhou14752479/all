using System;
using System.Collections;
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
using helper;

namespace 模拟采集
{
    public partial class 亚马逊商品页 : Form
    {
        public 亚马逊商品页()
        {
            InitializeComponent();
        }



        public void getdetail()
        {

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string asin = listView1.Items[i].SubItems[1].Text;
                string aurl = "https://www.amazon.com/dp/" + asin + "/";

                string strhtml = method.gethtml(aurl, "");
                string available = "0";
                if(strhtml.Contains("Currently unavailable"))
                {
                    available = "1";
                }

                Match title = Regex.Match(strhtml, @"<span id=""productTitle"" class=""a-size-large product-title-word-break"">([\s\S]*?)</span>");
                Match review = Regex.Match(strhtml, @"acrCustomerReviewText"" class=""a-size-base"">([\s\S]*?) ");
                Match star = Regex.Match(strhtml, @"class=""a-size-medium a-color-base"">([\s\S]*?)</span>");
                Match avalable = Regex.Match(strhtml, @"Amazon.com:([\s\S]*?)""");
                listView1.Items[i].SubItems[2].Text = title.Groups[1].Value.Trim();
                listView1.Items[i].SubItems[3].Text = review.Groups[1].Value.Trim();
                listView1.Items[i].SubItems[4].Text = star.Groups[1].Value.Trim();
                listView1.Items[i].SubItems[5].Text = aurl;
                listView1.Items[i].SubItems[7].Text = available;
            }
        }




        public void run()
        {



            MatchCollection asins = Regex.Matches(html, @"<div data-asin=""([\s\S]*?)""");

            if (asins.Count == 0)
            {
                shuju = false;
                return;
            }

            for (int j = 0; j < asins.Count; j++)
            {
                if (asins[j].Groups[1].Value != "")
                {
                    
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(asins[j].Groups[1].Value);
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add(key);
                    lv1.SubItems.Add("");




                }


            }




        }

        bool shuju = true;  //判断页码是否有数据
        bool status = false;
        public static string html; //网页源码传值
       
        ArrayList pageKeyList = new ArrayList();

        string key = "";

        public void qishi()
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"amazon"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion
            string domain = "com";
            switch (comboBox1.Text.Trim())
            {
                case "美国":
                    domain = "com";
                    break;

                case "加拿大":
                    domain = "ca";
                    break;
                case "英国":
                    domain = "co.uk";
                    break;
                case "法国":
                    domain = "fr";
                    break;
                case "西班牙":
                    domain = "es";
                    break;
                case "德国":
                    domain = "de";
                    break;
                case "意大利":
                    domain = "it";
                    break;
                case "日本":
                    domain = "ip";
                    break;


            }


            label1.Text = "正在查询......";

            string[] keywords = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string keyword in keywords)
            {


                key = keyword;
                for (int i = 1; i < 2; i++)
                {
                    if (shuju == false)
                    {
                        return;
                    }


                    status = false;
                    webBrowser1.Navigate("https://www.amazon." + domain + "/s?k=" + System.Web.HttpUtility.UrlEncode(keyword) + "&page=" + i);



                    while (this.status == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    // Thread.Sleep(1000);
                }


            }

            label1.Text = "查询结束";
            Thread thread = new Thread(new ThreadStart(getdetail));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"amazondetail"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion
            Thread thread = new Thread(new ThreadStart(qishi));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void 亚马逊商品页_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WB_DocumentCompleted);
        }
        private void WB_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                return;
            if (e.Url.ToString() != webBrowser1.Url.ToString())
                return;

            if (webBrowser1.DocumentText.Contains("</html>"))
            {

                html = webBrowser1.DocumentText;

                run();
                status = true;
            }
            else
            {
                Application.DoEvents();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要删除吗？", "清空", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                listView1.Items.Clear();
            }
            else
            {

            }
        }
    }
}
