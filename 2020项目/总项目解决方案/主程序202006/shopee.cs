using System;
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
using helper;


namespace 主程序202006
{
    public partial class shopee : Form
    {
        public shopee()
        {
            InitializeComponent();
        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                request.Headers.Add("Cookie", COOKIE);

                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();


                reader.Close();
                response.Close();
                return content;



            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion

        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(Convert.ToDouble(timeStamp));

        }
        /// <summary>
        /// 规格时间
        /// </summary>
        public void run()
        {


            for (int i = 0; i < Convert.ToInt32(textBox2.Text); i++)
            {
                Match itemId = Regex.Match(textBox1.Text + ",", @"i\.([\s\S]*?)\.([\s\S]*?),");



                string url = "https://shopee.com.my/api/v2/item/get_ratings?filter=0&flag=1&itemid=" + itemId.Groups[2].Value.Trim() + "&limit=6&offset=" + (i * 6) + "&shopid=" + itemId.Groups[1].Value.Trim() + "&type=0";
                string html = GetUrl(url, "utf-8");

                MatchCollection ahtmls = Regex.Matches(html, @"show_reply([\s\S]*?)delete_reason");
                foreach (Match ahtml in ahtmls)
                {
                    Match guige = Regex.Match(ahtml.Groups[1].Value, @"""model_name"":""([\s\S]*?)""");
                    Match time = Regex.Match(ahtml.Groups[1].Value, @"anonymous([\s\S]*?)""ctime"":([\s\S]*?),");
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(guige.Groups[1].Value);
                    lv1.SubItems.Add(ConvertStringToDateTime(time.Groups[2].Value).ToString("yyyy-MM-dd- HH:mm"));
                }

                Thread.Sleep(500);





            }




        }
        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //request.ContentType = "application/x-www-form-urlencoded";

                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("appid:orders");
                //headers.Add("x-nike-visitid:5");
                //headers.Add("x-nike-visitorid:d03393ee-e42c-463e-9235-3ca0491475b4");
                //添加头部
                request.ContentType = "application/json";
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
               
                request.Referer = "https://seller.shopee.com.my/portal/sale?type=toship";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion
        public static string cookie ="";

        /// <summary>
        /// 订单编码
        /// </summary>
        public void dingdan()
        {


            for (int i = 1; i <=Convert.ToInt32(textBox3.Text); i++)
            {


                string url = "https://seller.shopee.com.my/api/v3/order/get_simple_order_ids/?SPC_CDS=810a7ed9-85c1-482b-aa70-e718234409e1&SPC_CDS_VER=2&page_size=40&page_number="+i+"&source=toship&total=747&flip_direction=ahead&sort_by=confirmed_date_desc&backend_offset=";
                string html = method.GetUrlWithCookie(url, cookie,"utf-8");

                MatchCollection uids = Regex.Matches(html, @"order_id"":([\s\S]*?),");
                StringBuilder sb = new StringBuilder();
              
                if (uids.Count > 0)
                {
                    foreach (Match uid in uids)
                    {

                        sb.Append("{\"order_id\":" + uid.Groups[1].Value.Trim() + ",\"shop_id\":172422083,\"region_id\":\"MY\"},");
                    }

                    string postdata = "{\"orders\":[" + sb.ToString().Remove(sb.ToString().Length - 1, 1) + "],\"from_seller_data\":true,\"source\":\"toship\"}";
                    string aurl = "https://seller.shopee.com.my/api/v3/order/get_order_list_by_order_ids_multi_shop/?SPC_CDS=810a7ed9-85c1-482b-aa70-e718234409e1&SPC_CDS_VER=2";
                    string ahtml = PostUrl(aurl, postdata, cookie, "utf-8");

                    MatchCollection sns = Regex.Matches(ahtml, @"""order_sn"": ""([\s\S]*?)""");

                    foreach (Match sn in sns)
                    {
                        ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString());
                        lv2.SubItems.Add(sn.Groups[1].Value.Replace("\"", ""));

                    }



                }

            }




        }
        private void shopee_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://seller.shopee.com.my/account/signin");
            webBrowser1.ScriptErrorsSuppressed = true;
            method.SetWebBrowserFeatures(method.IeVersion.IE11);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cookie = textBox4.Text;
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"shopee"))
            {
               
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
               method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button4.Enabled = true;
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            cookie = textBox4.Text;
            button4.Enabled = false;
            Thread thread = new Thread(new ThreadStart(dingdan));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
