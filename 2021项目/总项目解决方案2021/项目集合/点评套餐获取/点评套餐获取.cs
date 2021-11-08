using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 点评套餐获取
{
    public partial class 点评套餐获取 : Form
    {/// <summary>
     /// This class is an implementation of the 'IComparer' interface.
     /// </summary>
        public class ListViewColumnSorter : IComparer
        {
            /// <summary>
            /// Specifies the column to be sorted
            /// </summary>
            private int ColumnToSort;
            /// <summary>
            /// Specifies the order in which to sort (i.e. 'Ascending').
            /// </summary>
            private SortOrder OrderOfSort;
            /// <summary>
            /// Case insensitive comparer object
            /// </summary>
            private CaseInsensitiveComparer ObjectCompare;

            /// <summary>
            /// Class constructor.  Initializes various elements
            /// </summary>
            public ListViewColumnSorter()
            {
                // Initialize the column to '0'
                ColumnToSort = 0;

                // Initialize the sort order to 'none'
                OrderOfSort = SortOrder.None;

                // Initialize the CaseInsensitiveComparer object
                ObjectCompare = new CaseInsensitiveComparer();
            }

            private int comaretInt = 0;
            /// <summary>
            /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
            /// </summary>
            /// <param name="x">First object to be compared</param>
            /// <param name="y">Second object to be compared</param>
            /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
            public int Compare(object x, object y)
            {
                int compareResult;
                ListViewItem listviewX, listviewY;

                // Cast the objects to be compared to ListViewItem objects
                listviewX = (ListViewItem)x;
                listviewY = (ListViewItem)y;

                if (int.TryParse(listviewX.SubItems[ColumnToSort].Text, out comaretInt) &&
                   int.TryParse(listviewY.SubItems[ColumnToSort].Text, out comaretInt))
                {
                    compareResult = int.Parse(listviewX.SubItems[ColumnToSort].Text).CompareTo(int.Parse(listviewY.SubItems[ColumnToSort].Text));
                }
                else
                {
                    // Compare the two items
                    compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
                }
                // Calculate correct return value based on object comparison
                if (OrderOfSort == SortOrder.Ascending)
                {
                    // Ascending sort is selected, return normal result of compare operation
                    return compareResult;
                }
                else if (OrderOfSort == SortOrder.Descending)
                {
                    // Descending sort is selected, return negative result of compare operation
                    return (-compareResult);
                }
                else
                {
                    // Return '0' to indicate they are equal
                    return 0;
                }
            }

            /// <summary>
            /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
            /// </summary>
            public int SortColumn
            {
                set
                {
                    ColumnToSort = value;
                }
                get
                {
                    return ColumnToSort;
                }
            }

            /// <summary>
            /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
            /// </summary>
            public SortOrder Order
            {
                set
                {
                    OrderOfSort = value;
                }
                get
                {
                    return OrderOfSort;
                }
            }

        }


        public 点评套餐获取()
        {
            InitializeComponent();
        }
        Thread t;
        string cookie = " _gw_ab_15963_null=382; _gw_ab_call_15963_null=TRUE; cityid=100; dpid=-7383434693177396553; latlng=33.94110%2C118.24797%2C1623993972244; network=wifi; latlon=33.94111%2C118.24798%2C1623993970777; logan_custom_report=; logan_session_token=51dny2cm948rwubjipmw; _hc.v=2d57e295-2064-adff-c59e-47cdcbc51c2e.1623993875; _lxsdk=f94d10e562a34551879fc534e09f19570000000000005494584; _lxsdk_dpid=-7383434693177396553; _lxsdk_unoinid=f94d10e562a34551879fc534e09f19570000000000005494584; _lxsdk_cuid=17a1d93f84ec8-065820f02d8461-1c27065b-4a640-17a1d93f84fc8; _lxsdk_s=17a1d93f850-f5a-9f1-3c9%7C%7CNaN; mtuuid=0000000000000F94D10E562A34551879FC534E09F19570000000000005494584; token=";
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public string GetUrl(string Url)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_6_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 TitansNoX/11.35.11.4 KNB/1.0 iOS/13.6.1 dp/com.dianping.dpscope/10.46.11 dp/10.46.11 App/10210/10.46.11 iPhone/iPhone7Plus WKWebView";
                request.KeepAlive = true;
                request.Headers.Add("Cookie", cookie);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            return "";
        }
        #endregion
        //public void run()
        //{

        //    try
        //    {
        //        string ahtml= Regex.Match(html, @"精选商品</a>([\s\S]*?)<a class=""unfold").Groups[1].Value.Trim();

        //        string shopname = Regex.Match(html, @"<h1 class=""shop-name"">([\s\S]*?)<").Groups[1].Value.Trim();
        //        string comment = Regex.Match(html, @"<span class=""sub-title"">([\s\S]*?)<").Groups[1].Value.Replace("(", "").Replace(")", "").Trim();
        //        string star1 = Regex.Match(html, @"效果：([\s\S]*?)<").Groups[1].Value;
        //        string star2 = Regex.Match(html, @"环境：([\s\S]*?)<").Groups[1].Value;
        //        string star3 = Regex.Match(html, @"服务：([\s\S]*?)<").Groups[1].Value;



        //        ListViewItem lv1 = listView1.Items.Add(shopname); //使用Listview展示数据
        //        lv1.SubItems.Add(comment);
        //        lv1.SubItems.Add(star1);
        //        lv1.SubItems.Add(star2);
        //        lv1.SubItems.Add(star3);
        //        MatchCollection items = Regex.Matches(ahtml, @"<a class=""item small([\s\S]*?)</a>");
        //        ListViewItem lv2= listView1.Items.Add("分类"); //使用Listview展示数据
        //        lv2.SubItems.Add("标题");
        //        lv2.SubItems.Add("原价");
        //        lv2.SubItems.Add("折扣价");
        //        lv2.SubItems.Add("销量");

        //        for (int j = 0; j < items.Count; j++)
        //        {

        //            string item = items[j].Groups[1].Value+"</a>";

        //            string name= Regex.Match(item, @"</del>([\s\S]*?)</a>").Groups[1].Value.Trim();
        //            string price = Regex.Match(item, @"del-price"">([\s\S]*?)</del>").Groups[1].Value.Trim();
        //            string dealprice = Regex.Match(item, @"<span class=""price"">([\s\S]*?)</span>").Groups[1].Value.Trim();

        //            string cate = Regex.Match(name, @"\[([\s\S]*?)\]").Groups[1].Value;
        //            ListViewItem lv = listView1.Items.Add(cate); //使用Listview展示数据
        //            lv.SubItems.Add(name);
        //            lv.SubItems.Add(price);
        //            lv.SubItems.Add(dealprice);
        //            lv.SubItems.Add("0");


        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show(ex.ToString()) ;
        //    }



        //}

        public void run()
        {

            try
            {
                string ahtml = Regex.Match(html, @"精选商品</a>([\s\S]*?)<a class=""unfold").Groups[1].Value.Trim();
                string shopid = Regex.Match(html, @"shopid=([\s\S]*?)""").Groups[1].Value.Trim();
                string shopname = Regex.Match(html, @"<h1 class=""shop-name"">([\s\S]*?)<").Groups[1].Value.Trim();
                string comment = Regex.Match(html, @"<span class=""sub-title"">([\s\S]*?)<").Groups[1].Value.Replace("(", "").Replace(")", "").Trim();
                string star1 = Regex.Match(html, @"效果：([\s\S]*?)<").Groups[1].Value;
                string star2 = Regex.Match(html, @"环境：([\s\S]*?)<").Groups[1].Value;
                string star3 = Regex.Match(html, @"服务：([\s\S]*?)<").Groups[1].Value;



                ListViewItem lv1 = listView1.Items.Add("a"+shopname); //使用Listview展示数据
                lv1.SubItems.Add(comment);
                lv1.SubItems.Add(star1);
                lv1.SubItems.Add(star2);
                lv1.SubItems.Add(star3);

                ListViewItem lv2 = listView1.Items.Add("b分类"); //使用Listview展示数据
                lv2.SubItems.Add("标题");
                lv2.SubItems.Add("原价");
                lv2.SubItems.Add("折扣价");
                lv2.SubItems.Add("销量");
                //MessageBox.Show(items.Count.ToString());


                MatchCollection items = Regex.Matches(ahtml, @"productid=([\s\S]*?)&");
                MatchCollection itemNames = Regex.Matches(ahtml, @"</del>([\s\S]*?)</a>");
                for (int j = 0; j < items.Count; j++)
                {
                    string aurl = "https://m.dianping.com/arche-product-universal-web-detail/dzpssr/beauty-product.html?latitude=33.94110&longitude=118.24797&productid=" + items[j].Groups[1].Value + "&channelofurl=1&sputypeofurl=850&shopuuid=" + shopid + "&product=dpapp&pushEnabled=0";

                    string item = GetUrl(aurl);
                    // string cate = Regex.Match(itemNames[j].Groups[1].Value, @"\[([\s\S]*?)\]").Groups[1].Value;
                   
                    string name = Regex.Match(item, @"{""title"":""([\s\S]*?)""").Groups[1].Value.Trim().Replace("u002F", "");
                    string price = Regex.Match(item, @"marketPrice"":([\s\S]*?),").Groups[1].Value.Trim();
                    string dealprice = Regex.Match(item, @"""price"":([\s\S]*?),").Groups[1].Value.Trim();
                    string salecount = Regex.Match(item, @"""saleCount"":([\s\S]*?),").Groups[1].Value.Trim();

                    string cate= Regex.Match(item, @"""goodsName"":""([\s\S]*?)""").Groups[1].Value.Trim().Replace("u002F", "");
                    if (cate.Trim() == "")
                    {
                        if (name.Contains("毛"))
                        {
                            cate = "脱毛";
                        }
                        else
                        {
                            cate = "皮肤美容";
                        }
                    }
                    ListViewItem lv = listView1.Items.Add(cate); //使用Listview展示数据

                    lv.SubItems.Add(name);
                    lv.SubItems.Add(price);
                    lv.SubItems.Add(dealprice);
                    lv.SubItems.Add(salecount);
                    Thread.Sleep(500);

                }

                MessageBox.Show("完成");
               


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }



        }
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html");

            if (!html.Contains(@"OaO6W"))
            {
                MessageBox.Show("");
                return;
            }

            #endregion
            if (t == null || !t.IsAlive)
            {
                t = new Thread(run);
                t.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
        public string html = "";
        private void 点评套餐获取_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WB_DocumentCompleted);
            //webBrowser1.Url = new Uri(webUrl);

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
            }
            else
            {
                Application.DoEvents();
            }
        }




        private void button4_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }


        private ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();


        public void paixu(ColumnClickEventArgs e)
        {
           
        }
             
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            foreach (ColumnHeader ch in listView1.Columns)
            {
                ch.ImageIndex = -1;
            }

            listView1.ListViewItemSorter = lvwColumnSorter;

            //if (listTask.SmallImageList == null)
            //{
            //    listTask.SmallImageList = imageList1;
            //}

            //listTask.Columns[e.Column].ImageIndex = 0;

            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                    //listTask.Columns[e.Column].ImageIndex = 1;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.

            this.listView1.Sort();
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);   //根据列的文本内容，自动设置list列的宽度
            listView1.Update();
        }



    }
}
