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
using helper;
using Microsoft.Win32;

namespace 模拟采集
{
    public partial class 拼多多榜单 : Form
    {
        public 拼多多榜单()
        {
            InitializeComponent();
        }

        Dictionary<string, string> dic = new Dictionary<string, string>();
        public void getitems()
        {
            comboBox2.Items.Clear();
            dic.Clear();
            string url = "https://mobile.yangkeduo.com/proxy/api/api/george/tab/query_tab_list?pdduid=0&is_back=1&resource_type=410&group_id=564&list_id=main_hDET3469Bd";   //改变pagesize获取全部，目前是10不是全部题库
            string html = method.GetUrlWithCookie(url, cookie, "utf-8");
            
            MatchCollection items = Regex.Matches(html, @"\{""tab_id"":([\s\S]*?)\,""tab_name"":""([\s\S]*?)""");

       
            for (int i = 0; i < items.Count; i++)
            {

                comboBox2.Items.Add(items[i].Groups[2].Value);
                dic.Add(items[i].Groups[2].Value, items[i].Groups[1].Value);
            }

         

        }


        Dictionary<string, string> dic2 = new Dictionary<string, string>();
        public void getitems2(string ID)
        {

            comboBox3.Items.Clear();
            dic2.Clear();
            string url = "https://mobile.yangkeduo.com/proxy/api/api/george/tab/query_tab_list?pdduid=0&is_back=1&resource_type=410&tab_id=" + ID + "&list_id=secondary_39245_hDET3469Bd";
            string html = method.GetUrlWithCookie(url, cookie, "utf-8");

            MatchCollection items = Regex.Matches(html, @"\{""tab_id"":([\s\S]*?)\,""tab_name"":""([\s\S]*?)""");


            for (int i = 0; i < items.Count; i++)
            {
                if (!dic2.ContainsKey(items[i].Groups[2].Value) && !items[i].Groups[2].Value.Contains("榜"))
                {
                    comboBox3.Items.Add(items[i].Groups[2].Value);
                    dic2.Add(items[i].Groups[2].Value, items[i].Groups[1].Value);
                }
            }
        }








        private void 拼多多榜单_Load(object sender, EventArgs e)
        {
           



            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WB_DocumentCompleted);
             webBrowser1.Navigate("https://mobile.yangkeduo.com/login.html?from=https:%2F%2Fmobile.yangkeduo.com%2Fpersonal.html%3Frefer_page_name%3Dindex%26refer_page_id%3D10002_1599526204968_ikxqj9x2gx%26refer_page_sn%3D10002&refer_page_name=personal&refer_page_id=10001_1599526206955_vkwp6lokgl&refer_page_sn=10001");
           
        }
        private void WB_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                return;
            if (e.Url.ToString() != webBrowser1.Url.ToString())
                return;
            if (webBrowser1.IsBusy == true)
                return;
            if (webBrowser1.DocumentText.Contains("</html>"))
            {

                html = webBrowser1.Document.Body.OuterHtml;
                run();
                status = true;
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

                
            }

            


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



        public string cookie;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string ahtml = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!ahtml.Contains(@"pddbangdan"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion


            string aId = dic[comboBox2.Text.Trim()];
            string bId = dic2[comboBox3.Text.Trim()];
            string cId = "1";
            string tap = "0";
            if (comboBox4.Text == "好评榜")
            {
                cId = "2";
                tap = "1";
            }


            //size200改成500
            string url = "https://mobile.yangkeduo.com/proxy/api/api/george/content/query_content_list?pdduid=7312500755985&is_back=1&size=500&resource_type=1&tab_id="+bId+"&page=1&obj_count=2&type="+cId+"&list_id="+aId+"_"+bId+"_"+cId+"_hDET3469Bd&field_types=content_goods_material";
            string html = method.GetUrlWithCookie(url,cookie,"utf-8");
           
            MatchCollection listIds = Regex.Matches(html, @"""content_id"":""([\s\S]*?)""");
        
            for (int i = 0; i < listIds.Count; i++)
            {
               
                
                status = false;
                string URL = "https://mobile.yangkeduo.com/sjs_cat_rank_list.html?scene_id=list_channel_rank_category&_pdd_fs=1&_pdd_tc=ffffff&_pdd_sbs=1&refer_page_el_sn=3408901&list_id=" + listIds[i].Groups[1].Value + "&entrance_type=1&refer_origin_page_sn=17542&refer_first_tab_id=39246&refer_second_tab_id=39611&refer_page_name=list_classification&refer_page_id=67388_1599522932540_mqxy6u9gbl&refer_page_sn=67388&refer_abtest_info=%7B%7D&activeTab=" + tap;
                webBrowser1.Stop();
                webBrowser1.Navigate(URL);


                while (status == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
                Thread.Sleep(1000);
            }
            MessageBox.Show("抓取结束");


            //HtmlDocument doc = webBrowser1.Document;
            //HtmlElementCollection es = doc.GetElementsByTagName("div");
            //foreach (HtmlElement e1 in es)
            //{
            //    if (e1.GetAttribute("data-uniqid") == "49")
            //    {
            //        e1.InvokeMember("click");
            //    }

            //}

            //HtmlDocument doc = webBrowser1.Document;
            //HtmlElementCollection es = doc.GetElementsByTagName("p");
            //foreach (HtmlElement e1 in es)
            //{
            //    if (e1.GetAttribute("class")== "_2uIR36c6")
            //    {
            //        MessageBox.Show(e1.InnerText);
            //        e1.InvokeMember("click");

            //        if (status==true)
            //        {

            //            webBrowser1.GoBack();
            //        }
            //        else
            //        {
            //            Application.DoEvents();
            //        }
            //    }



            //}



        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            cookie = method.GetCookies("https://mobile.yangkeduo.com/");
            getitems();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            getitems2(dic[comboBox2.SelectedItem.ToString()]);
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
