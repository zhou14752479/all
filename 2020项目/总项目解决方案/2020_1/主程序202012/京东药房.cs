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

namespace 主程序202012
{
    public partial class 京东药房 : Form
    {
        public 京东药房()
        {
            InitializeComponent();
        }

        private void 京东药房_Load(object sender, EventArgs e)
        {

        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "sid=64a4a095f95b472f56b387f81def9f98;USER_FLAG_CHECK=9416a352d1e261a30bc190a337c2a96f;appkey=0a0834aee67bcee63b8a39435300636d;appid=wx862d8e26109609cb;mpChannelId=388635;openid=odI0R5S8UPqCxO5UtR6u6V76SfKQ;wxclient=gxhwx;ie_ai=1;";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://servicewechat.com/wx91d27dbf599dff74/534/page-frame.html";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                // request.KeepAlive = true;
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
                ex.ToString();

            }
            return "";
        }
        #endregion


        string[] cates = {

            "21909",
"21923",
"21924",
"21920",
"21919",
"21925",
"21921",
"21922",
"21918",
"21915",
"21916",
"21914",
"21910",
"21913",
"21912",
"21911",
"21917",
"13430",
"13456",
"13468",
"13388",
"13380",
"13424",
"13446",
"13488",
"13349",
"13326",
"13315",
"13494",
"13527",
"13500",
"13518",
"13504",
"13354",
"13693",
"13362",
"13335",
"13314",
        };

        public void jd()
        {

            foreach (var cate in cates)
            {

                for (int page = 1; page < 201; page++)
                {


                    string url = "https://list.jd.com/list.html?cat="+cate+"&page="+page+"&s=61&click=0";
                    string html = method.GetUrl(url, "utf-8");

                    MatchCollection ids = Regex.Matches(html, @"<li data-sku=""([\s\S]*?)""");
                    MatchCollection prices = Regex.Matches(html, @"data-price=""([\s\S]*?)"">([\s\S]*?)<");
                    if (ids.Count == 0)
                        continue;


                    for (int i = 0; i < ids.Count; i++)
                    {
                        // string aurl = "https://wxa.jd.com/wqitem.jd.com/itemv3/wxadraw?sku=" + ids[i].Groups[1].Value;

                        //string aurl = "https://item-soa.jd.com/getWareBusiness?callback=jQuery8633016&skuId="+ ids[i].Groups[1].Value + "&cat=13314%2C21909%2C21922&area=1_72_55653_0&shopId=1000015441";
                        //string ahtml = GetUrl(aurl);
                      


                        string burl = "https://wxa.jd.com/wq.jd.com/graphext/draw?sku=" + ids[i].Groups[1].Value;
                        string bhtml = GetUrl(burl);

                        //Match title = Regex.Match(ahtml, @"""skuName"":""([\s\S]*?)""");
                        // Match title = Regex.Match(bhtml, @"""wareQD"":""([\s\S]*?)""");
                       string title = Regex.Match(bhtml, @"""attName"":""药品商品名([\s\S]*?)\[([\s\S]*?)\]").Groups[2].Value;

                        //Match price = Regex.Match(ahtml, @"""jdPrice"":""([\s\S]*?)""");
                        //Match price = Regex.Match(ahtml, @"""op"":""([\s\S]*?)""");

                        Match guige = Regex.Match(bhtml, @"""attName"":""产品规格([\s\S]*?)\[([\s\S]*?)\]");
                        Match company = Regex.Match(bhtml, @"""attName"":""生产企业([\s\S]*?)\[([\s\S]*?)\]");
                        Match zhunhao = Regex.Match(bhtml, @"""attName"":""批准文号([\s\S]*?)\[([\s\S]*?)\]");

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                        //textBox3.Text += "正在抓取：" + title.Groups[1].Value + "\r\n";
                        if (textBox3.Text.Length > 10000)
                        {
                            textBox3.Text = "";
                        }

                        if (title== "")
                        {
                            title = Regex.Match(bhtml, @"""attName"":""药品通用名名([\s\S]*?)\[([\s\S]*?)\]").Groups[2].Value;

                        }

                        if (title == "")
                        {
                            title = Regex.Match(bhtml, @"""wareQD"":""([\s\S]*?)""").Groups[1].Value;

                        }
                        lv1.SubItems.Add(title.Trim().Replace("\"", ""));

                            lv1.SubItems.Add(prices[i].Groups[2].Value);
                            lv1.SubItems.Add(guige.Groups[2].Value.Replace("\"", ""));
                            lv1.SubItems.Add(company.Groups[2].Value.Replace("\"", ""));
                            lv1.SubItems.Add(zhunhao.Groups[2].Value.Replace("\"", ""));
                            if (status == false)
                                return;
                        Thread.Sleep(100);
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                        
                    }
                }
               
                }
            


        }


        public void jdyaofang()
        {

            for (int page = 1; page < 101; page++)
            {


                string url = "https://mall.jd.com/advance_search-612358-1000015441-1000015441-0-0-0-1-" + page + "-60.html?other=";
                string html = method.GetUrl(url, "utf-8");

                MatchCollection ids = Regex.Matches(html, @"jdprice='([\s\S]*?)'");

                if (ids.Count == 0)
                    break;


                for (int i = 0; i < ids.Count; i++)
                {
                    string aurl = "https://item-soa.jd.com/getWareBusiness?callback=jQuery8715480&skuId=" + ids[i].Groups[1].Value;
                    string ahtml = method.GetUrl(aurl, "utf-8");
                    string burl = "https://item.yiyaojd.com/" + ids[i].Groups[1].Value + ".html";
                    string bhtml = method.GetUrl(burl, "utf-8");
                    Match title = Regex.Match(bhtml, @" name: '([\s\S]*?)'");

                    Match price = Regex.Match(ahtml, @"""p"":""([\s\S]*?)""");
                    Match guige = Regex.Match(bhtml, @"产品规格</dt><dd>([\s\S]*?)</dd>");
                    Match company = Regex.Match(bhtml, @"生产企业</dt><dd>([\s\S]*?)</dd>");
                    Match zhunhao = Regex.Match(bhtml, @"批准文号</dt><dd>([\s\S]*?)</dd>");

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                    textBox3.Text += "正在抓取：" + title.Groups[1].Value + "\r\n";
                    if (textBox3.Text.Length > 10000)
                    {
                        textBox3.Text = "";
                    }
                    lv1.SubItems.Add(title.Groups[1].Value);

                    lv1.SubItems.Add(price.Groups[1].Value);
                    lv1.SubItems.Add(guige.Groups[1].Value);
                    lv1.SubItems.Add(company.Groups[1].Value);
                    lv1.SubItems.Add(zhunhao.Groups[1].Value);
                    if (status == false)
                        return;
                    if (this.listView1.Items.Count > 2)
                    {
                        this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                    }
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                }

            }



        }


        public void yaofang315()
        {
            string cookie = "__yjs_duid=1_4fdb16923a213106a7193c694ba0f2161627777139914; iwmsGid=0G0ZYIHYXLOAZVXUOQNQ; Hm_lvt_4cce664ec5d8326cc457ab09053c15b2=1627777139,1627868117; rtv=5616BB,32155431; Hm_lpvt_4cce664ec5d8326cc457ab09053c15b2=1627868994";
            for (int i = 1; i < 5000; i++)
            {
                string url = "https://www.315jiage.cn/mc119p" + i + ".aspx";
                string url2 = method.GetUrl(url, "utf-8");
                MatchCollection matchCollection = Regex.Matches(url2, "<div class=\"col-2\"> <a href=\"([\\s\\S]*?)\"");
                bool flag = matchCollection.Count == 0;
                if (flag)
                {
                    break;
                }
                for (int j = 0; j < matchCollection.Count; j++)
                {
                    string url3 = "https://www.315jiage.cn/" + matchCollection[j].Groups[1].Value;
                    string urlWithCookie = method.GetUrlWithCookie(url3, cookie, "utf-8");
                    Match match = Regex.Match(urlWithCookie, "<title>([\\s\\S]*?)_");
                    Match match2 = Regex.Match(urlWithCookie, "建议零售价格：￥([\\s\\S]*?)<");
                    Match match3 = Regex.Match(urlWithCookie, "规格：</span>([\\s\\S]*?)<");
                    Match match4 = Regex.Match(urlWithCookie, "生产厂家：</span>([\\s\\S]*?)<");
                    Match match5 = Regex.Match(urlWithCookie, "批准文号：</span>([\\s\\S]*?)</a>");
                    ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
                 
                    textBox3.Text = textBox3.Text + "正在抓取：" + match.Groups[1].Value + "\r\n";
                  
                    if (textBox3.Text.Length > 10000)
                    {
                        this.textBox3.Text = "";
                    }
                    listViewItem.SubItems.Add(match.Groups[1].Value);
                    listViewItem.SubItems.Add(match2.Groups[1].Value.Trim());
                    listViewItem.SubItems.Add(match3.Groups[1].Value.Replace("&ensp;", "").Trim());
                    listViewItem.SubItems.Add(match4.Groups[1].Value.Trim());
                    listViewItem.SubItems.Add(Regex.Replace(match5.Groups[1].Value, "<[^>]+>", "").Trim());
                    bool flag3 = match.Groups[1].Value == "";
                    if (match.Groups[1].Value == "")
                    {
                        this.textBox3.Text = urlWithCookie;
                    }
                  
                    if (this.status==false)
                    {
                        return;
                    }
                    while (!this.zanting)
                    {
                        this.textBox3.Text = "已暂停";
                        Application.DoEvents();
                    }
                   
                    if (this.listView1.Items.Count > 2)
                    {
                        this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                    }
                    Thread.Sleep(1000);
                }
            }
        }

        Thread thread;
        bool zanting = true;
        bool status = true;
        private void button1_Click(object sender, EventArgs e)
        {
            //method.ReadFromExcelFile(@"C:\Users\zhou\Desktop\TEST2 不翻译.xlsx",listView1);
            this.status = true;

            if (radioButton1.Checked)
            {

                if (thread == null || !thread.IsAlive)
                {
                    this.thread = new Thread(new ThreadStart(this.jd));
                    this.thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }

            if (radioButton3.Checked)
            {
                if (thread == null || !thread.IsAlive)
                {
                    this.thread = new Thread(new ThreadStart(this.yaofang315));
                    this.thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {
                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void 京东药房_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
