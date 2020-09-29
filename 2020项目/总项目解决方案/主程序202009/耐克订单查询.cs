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
using helper;

namespace 主程序202009
{
    public partial class 耐克订单查询 : Form
    {
        public 耐克订单查询()
        {
            InitializeComponent();
        }
        public string sid = "";
        bool zanting = true;
        Thread thread;

        string COOKIE = "";


        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public string GetUrl(string Url, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
               request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
               
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                WebHeaderCollection headers = request.Headers;
             
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

        #region nikeGET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public string nikeGetUrl(string Url, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://www.nike.com/";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                WebHeaderCollection headers = request.Headers;
                headers.Add("appid: orders");
                headers.Add("x-nike-visitid: 2");
                headers.Add("x-nike-visitorid: c2247fe0-c826-412e-864f-911541b0ea64");
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


        ArrayList orderList = new ArrayList();

        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            for (int page = 0; page < Convert.ToInt32(textBox1.Text); page++)
            {


                string url = "https://mail.qq.com/cgi-bin/mail_list?sid=" + sid + "&folderid=1&folderkey=1&page=" + page + "&s=inbox&topmails=0&showinboxtop=1&ver=592297.0&cachemod=maillist&cacheage=7200&r=&selectall=0#stattime=1601086114071";

                string html = GetUrl(url, "gb18030");

                MatchCollection mailids = Regex.Matches(html, @"addrvip=""false"" mailid=""([\s\S]*?)""");


                for (int j = 0; j < mailids.Count; j++)
                {
                    string aurl = "https://mail.qq.com/cgi-bin/readmail?folderid=1&folderkey=1&t=readmail&mailid=" + mailids[j].Groups[1].Value + "&mode=pre&maxage=3600&base=12.65&ver=16664&sid=" + sid + "#stattime=1601085966167";
                    string ahtml = GetUrl(aurl, "gb18030");

                    Match ema = Regex.Match(ahtml, @"class=""left""t=""1"" e=""([\s\S]*?)""");
                    Match title = Regex.Match(ahtml, @"<title>([\s\S]*?)</title>");
                    Match orderNumber = Regex.Match(title.Groups[1].Value, @"C\d{10,}");

                    label1.Text = "正在抓取：" + Regex.Replace(orderNumber.Groups[0].Value, "<[^>]+>", "").Trim();


                    string order = orderNumber.Groups[0].Value.Trim();
                    string email = ema.Groups[1].Value.Trim();

                    string nikeurl = "https://api.nike.com/order_mgmt/user_order_details/v2/" + order + "?filter=email(" + email + ")";

                    string nikehtml = nikeGetUrl(nikeurl, "utf-8");

                    Match a1 = Regex.Match(nikehtml, @"""itemDescription"":""([\s\S]*?),");  //商品名称
                    Match a2 = Regex.Match(nikehtml, @"""displaySize"":""([\s\S]*?)""");            //尺码
                    Match a3 = Regex.Match(nikehtml, @"""orderSubmitDate"":""([\s\S]*?)""");   //订单时间
                    Match a4 = Regex.Match(nikehtml, @"""orderTotal"":([\s\S]*?),");  //总价

                    Match name1 = Regex.Match(nikehtml, @"""firstName"":""([\s\S]*?)""");
                    Match name2 = Regex.Match(nikehtml, @"""lastName"":""([\s\S]*?)""");

                    Match addr1 = Regex.Match(nikehtml, @"""address1"":""([\s\S]*?)""");
                    Match addr2 = Regex.Match(nikehtml, @"""city"":""([\s\S]*?)""");
                    Match addr3 = Regex.Match(nikehtml, @"""state"":""([\s\S]*?)""");
                    Match addr4 = Regex.Match(nikehtml, @"""zipCode"":""([\s\S]*?)""");
                    Match addr5 = Regex.Match(nikehtml, @"""country"":""([\s\S]*?)""");

                    Match a5 = Regex.Match(nikehtml, @"""shipping"":([\s\S]*?),"); //运费
                    Match a6 = Regex.Match(nikehtml, @"""rolledUpStatus"":""([\s\S]*?)"""); //状态
                    Match a7 = Regex.Match(nikehtml, @"""styleNumber"":""([\s\S]*?)"""); //货号1
                    Match a8 = Regex.Match(nikehtml, @"""colorCode"":""([\s\S]*?)""");//货号2
                   MatchCollection gifts = Regex.Matches(nikehtml, @"CardNumber"":""([\s\S]*?)"""); //礼品券付款方式
                    MatchCollection gifts1 = Regex.Matches(nikehtml, @"""paymentType"":""([\s\S]*?)"""); //礼品券付款方式
                    Match a11 = Regex.Match(nikehtml, @"""itemDescription"":""([\s\S]*?)""");
                    Match a12 = Regex.Match(nikehtml, @"""itemDescription"":""([\s\S]*?)""");
                    Match a13 = Regex.Match(nikehtml, @"""itemDescription"":""([\s\S]*?)""");
                    Match a14 = Regex.Match(nikehtml, @"""itemDescription"":""([\s\S]*?)""");

                    StringBuilder gift = new StringBuilder();
                    for (int a = 0; a <gifts.Count ; a++)
                    {
                        gift.AppendLine(gifts1[a].Groups[1].Value+" "+gifts[a].Groups[1].Value);
                    }

                    if (!orderList.Contains(order) && order!="")
                    {
                        orderList.Add(order);
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                        lv1.SubItems.Add(order);
                        lv1.SubItems.Add(email);
                        lv1.SubItems.Add(a1.Groups[1].Value.Replace("\"", "").Replace("\\", ""));
                        lv1.SubItems.Add(a2.Groups[1].Value);
                        lv1.SubItems.Add(a3.Groups[1].Value);
                        lv1.SubItems.Add(a4.Groups[1].Value);
                        lv1.SubItems.Add(name1.Groups[1].Value + " " + name2.Groups[1].Value);
                        lv1.SubItems.Add(addr1.Groups[1].Value + " " + addr2.Groups[1].Value + " " + addr3.Groups[1].Value + " " + addr4.Groups[1].Value + " " + addr5.Groups[1].Value);
                        lv1.SubItems.Add(a5.Groups[1].Value);
                        lv1.SubItems.Add(a6.Groups[1].Value);
                        lv1.SubItems.Add(a7.Groups[1].Value + "-" + a8.Groups[1].Value);
                        lv1.SubItems.Add(gift.ToString());
                        lv1.SubItems.Add("1");
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                    }

                    Thread.Sleep(1000);
                }
            }
            label1.Text = "抓取结束";


        }

        private void 耐克订单查询_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form1 fm1 = new Form1();
            fm1.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sid = Form1.SID;
            COOKIE = Form1.cookie;
          
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
