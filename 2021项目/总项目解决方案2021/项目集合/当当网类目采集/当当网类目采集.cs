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
using CsharpHttpHelper;
using myDLL;

namespace 当当网类目采集
{
    public partial class 当当网类目采集 : Form
    {
        public 当当网类目采集()
        {
            InitializeComponent();
        }

        string cookie = "from=460-5-biaoti; __permanent_id=20210706110133441348145396004241224; __ddc_15d_f=1625540493%7C!%7C_utm_brand_id%3D11106; secret_key=0cc5a2a9a7ad3f000bc0ed76ef14a76d; order_follow_source=P-460-5-bi%7C%231%7C%23www.baidu.com%252Fother.php%253Fsc.Ks00000J_ZMyY4TLeJKVCi01ayQKHVaVYu9YHcST1X-TghuTUdG4Iftfnb--AEu_hJBY_q6OQ%7C%230-%7C-; __ddc_15d=1625636369%7C!%7C_utm_brand_id%3D11106; permanent_key=20210709074953617181631368c2048c; smidV2=202107090750469aee3c357e9bf07d89babed9068e4caa00b8119bbc42e98f0; dangdang.com=email=MTc2MDYxMTc2MDYyNTk3MUBkZG1vYmlscGhvbmVfX3VzZXIuY29t&nickname=&display_id=1403940504405&customerid=qpxEEXuFAwxpZeZ7h4EIrQ==&viptype=blsR3X9DmaE=&show_name=176%2A%2A%2A%2A7606; cart_id=4000000007290579698; ddscreen=2; __visit_id=20210711095622954999122274681348204; __out_refer=; dest_area=country_id%3D9000%26province_id%3D111%26city_id%3D0%26district_id%3D0%26town_id%3D0; ad_ids=5381354%2C2777517%2C2777520%7C%232%2C2%2C2; pos_9_end=1625968626754; alipay_request_from=https://login.dangdang.com/signin.aspx?returnurl=http%253A%252F%252Fproduct.dangdang.com%252F29233640.html; __rpm=s_605253.451680112839%2C451680112840.18.1625968672623%7Clogin_page.login_nolocal_mobile_div..1625968701448; USERNUM=MYzrLF7p3u5S9dBBtpO+aA==; login.dangdang.com=.AYH=2021071109575407543822272&.ASPXAUTH=0HaueTlk+dZsdAmHviWD5QNYiX4KsgZb2j0LieFvUI4L3Z9wpmTEVg==; ddoy=email=1760611760625971%40ddmobilphone__user.com&nickname=&agree_date=1&validatedflag=0&uname=17606117606&utype=1&.ALFG=on&.ALTM=1625968702; sessionID=pc_18aa86eb434b313b5555448f4859319ac681d4e29e71bb958480cda03143ec41; __dd_token_id=20210711095822719736406671887fca; LOGIN_TIME=1625968721681; __trace_id=20210711095841694390552110197499481; pos_6_start=1625968721787; pos_6_end=1625968722537";
        private string geturl(string url)
        {
           HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = url,
                Method = "GET",
                Host = "product.dangdang.com",
                Accept = "application/json, text/javascript, */*; q=0.01",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
                Referer = "http://product.dangdang.com/20981913.html",
                Cookie = cookie,
                
            };
            item.Header.Add("X-Requested-With", "XMLHttpRequest");
            item.Header.Add("Accept-Encoding", "gzip, deflate");
            item.Header.Add("Accept-Language", "zh,sq;q=0.9,zh-CN;q=0.8,oc;q=0.7,de;q=0.6,en;q=0.5");
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            return html;
        }


        public double jisuan(string saleprice,string huodong,string quan)
        {
            
            double price = Convert.ToDouble(saleprice);
           
            if (huodong != "")
            {
                string man = Regex.Match(huodong, @"满([\s\S]*?)元").Groups[1].Value;
                string jian = Regex.Match(huodong, @"减([\s\S]*?)元").Groups[1].Value;
                if (Convert.ToDouble(saleprice) > Convert.ToDouble(man))
                {
                    double count = Math.Floor(Convert.ToDouble(saleprice) / Convert.ToDouble(man));
                    if (Convert.ToDouble(saleprice) % 100 >= 50)
                    {
                        price = Math.Ceiling(price);
                         count = count + 1;
                    }

                    price = price - Convert.ToDouble(jian) * count;
                }
            }
            if (quan != "")
            {
                MatchCollection man = Regex.Matches(quan, @"满([\s\S]*?)减");
                MatchCollection jian= Regex.Matches(quan, @"减([\s\S]*?)元");
                if (man.Count == 2)  //两张优惠券
                {
                    if (price >= Convert.ToDouble(man[0].Groups[1].Value) && price >= Convert.ToDouble(man[1].Groups[1].Value))
                    {
                        price = price - Convert.ToDouble(jian[0].Groups[1].Value);
                    }
                    if (price <Convert.ToDouble(man[0].Groups[1].Value) && price >= Convert.ToDouble(man[1].Groups[1].Value))
                    {
                        price = price - Convert.ToDouble(jian[1].Groups[1].Value);
                    }
                    if (price < Convert.ToDouble(man[0].Groups[1].Value) && price < Convert.ToDouble(man[1].Groups[1].Value))
                    {
                        
                    }
                }
            }

            if (price < 49)
            {
                price =price + 6;
            }

            return price;

           }

        public void run()
        {
            try
            {
                string hdhtml = method.GetUrl("http://promo.dangdang.com/6476132", "gb2312");
                string huodong = Regex.Match(hdhtml, "<h1>([\\s\\S]*?)</h1>").Groups[1].Value.Trim();
                string huodongtime = Regex.Match(hdhtml, "活动时间：([\\s\\S]*?)<").Groups[1].Value.Trim();
                for (int i = 1; i < 101; i++)
                {
                    string url = this.textBox1.Text.Replace("cp", "pg" + i + "-cp");
                    string html = method.GetUrl(url, "gb2312");
                    MatchCollection uids = Regex.Matches(html, "id=\"lcase([\\s\\S]*?)\"");
                    bool flag = uids.Count == 0;
                    if (flag)
                    {
                        MessageBox.Show("完成");
                        break;
                    }
                    int j = 0;
                    while (j < uids.Count)
                    {
                        try
                        {
                            string uid = uids[j].Groups[1].Value;
                            string ahtml = method.GetUrl("https://mapi.dangdang.com/index.php?action=get_product&user_client=iphone&client_version=11.7.3&union_id=537-50&permanent_id=20210730095015646129326676251121154&udid=CDD236322B97916364381C974957D10C&time_code=55F960825DDED6D818694D99AAC0C61A&timestamp=1627609894&lunbo_img_size=h&abtest=1&page_action=1&pid=" + uid + "&img_size=h&global_province_id=111&person_on=1", "utf-8");
                            StringBuilder cate = new StringBuilder();
                            string isbn = Regex.Match(ahtml, "\"standard_id\":\"([\\s\\S]*?)\"").Groups[1].Value;
                            string title = Regex.Match(ahtml, "\"product_name\":\"([\\s\\S]*?)\"").Groups[1].Value;
                            MatchCollection cates = Regex.Matches(ahtml, "\"path_name\":\"([\\s\\S]*?)\"");
                            string cbs = Regex.Match(ahtml, "\"publisher\":\"([\\s\\S]*?)\"").Groups[1].Value;
                            string cbstime = Regex.Match(ahtml, "\"publish_date\":\"([\\s\\S]*?)\"").Groups[1].Value;
                            string auther = Regex.Match(ahtml, "\"author_name\":\"([\\s\\S]*?)\"").Groups[1].Value;
                            string originalPrice = Regex.Match(ahtml, "\"original_price\":\"([\\s\\S]*?)\"").Groups[1].Value;
                            string salePrice = Regex.Match(ahtml, "\"sale_price\":\"([\\s\\S]*?)\"").Groups[1].Value;
                            for (int x = 1; x < cates.Count; x++)
                            {
                                cate.Append(cates[x].Groups[1].Value + " ");
                            }
                            bool flag2 = title == "";
                            if (flag2)
                            {
                                MessageBox.Show("");
                            }
                            this.label1.Text = "开始采集" + isbn;
                            Thread.Sleep(500);
                            string quanhtml = this.geturl("http://product.dangdang.com/index.php?r=callback%2Fproduct-info&productId=" + uid + "&isCatalog=0&shopId=0&productType=0");
                            MatchCollection activityUrl = Regex.Matches(quanhtml, "\"activityUrl\":\"([\\s\\S]*?)\"");
                            MatchCollection man = Regex.Matches(quanhtml, "\"couponMinUseValue\":\"([\\s\\S]*?)\"");
                            MatchCollection jian = Regex.Matches(quanhtml, "\"couponValue\":\"([\\s\\S]*?)\"");
                            StringBuilder sb = new StringBuilder();
                            for (int a = 0; a < man.Count; a++)
                            {
                                sb.Append(string.Concat(new string[]
                                {
                                    "满",
                                    man[a].Groups[1].Value,
                                    "减",
                                    jian[a].Groups[1].Value,
                                    "元"
                                }));
                            }
                            string huodongtime2 = "";
                            string huodong2 = "";
                            string quan = sb.ToString();
                            bool flag3 = activityUrl.Count > 0;
                            if (flag3)
                            {
                                huodongtime2 = huodongtime;
                                huodong2 = huodong;
                            }
                            string fee = "6";
                            double shiprice = this.jisuan(salePrice, huodong2, quan);
                            string shizhekou = (shiprice / Convert.ToDouble(originalPrice)).ToString("0.00");
                            bool flag4 = Convert.ToDouble(shiprice) - 6.0 >= 49.0;
                            if (flag4)
                            {
                                fee = "0";
                            }
                            while (!this.zanting)
                            {
                                Application.DoEvents();
                            }
                            bool flag5 = !this.status;
                            if (flag5)
                            {
                                return;
                            }
                            Thread.Sleep(100);
                            ListViewItem lv = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
                            lv.SubItems.Add(isbn);
                            lv.SubItems.Add(method.Unicode2String(title));
                            lv.SubItems.Add(method.Unicode2String(cate.ToString()));
                            lv.SubItems.Add(method.Unicode2String(cbs));
                            lv.SubItems.Add(method.Unicode2String(cbstime));
                            lv.SubItems.Add(method.Unicode2String(auther));
                            lv.SubItems.Add(huodongtime2);
                            lv.SubItems.Add(huodong2);
                            lv.SubItems.Add(quan);
                            lv.SubItems.Add(originalPrice);
                            lv.SubItems.Add(salePrice);
                            lv.SubItems.Add(fee);
                            lv.SubItems.Add(shiprice.ToString());
                            lv.SubItems.Add(shizhekou);
                            bool flag6 = this.listView1.Items.Count > 2;
                            if (flag6)
                            {
                                this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                            }
                            Thread.Sleep(1500);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                       
                      
                       
                       
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }



        public void run2()
        {
            try
            {
                string hdhtml = method.GetUrl("http://promo.dangdang.com/6473096", "gb2312");
                string huodong = Regex.Match(hdhtml, "<h1>([\\s\\S]*?)</h1>").Groups[1].Value.Trim();
                string huodongtime = Regex.Match(hdhtml, "活动时间：([\\s\\S]*?)<").Groups[1].Value.Trim();
                for (int i = 1; i < 101; i++)
                {
                    string url = this.textBox1.Text.Replace("cp", "pg" + i + "-cp");
                    string html = method.GetUrl(url, "gb2312");
                    MatchCollection uids = Regex.Matches(html, "id=\"lcase([\\s\\S]*?)\"");
                    bool flag = uids.Count == 0;
                    if (flag)
                    {
                        MessageBox.Show("完成");
                        break;
                    }
                    for (int j = 0; j < uids.Count; j++)
                    {
                        while (!this.zanting)
                        {
                            Application.DoEvents();
                        }
                        bool flag2 = !this.status;
                        if (flag2)
                        {
                            return;
                        }
                        bool flag3 = webbrowser.cookie != "";
                        if (flag3)
                        {
                            this.cookie = webbrowser.cookie;
                        }
                        string uid = uids[j].Groups[1].Value;
                        string ahtml = this.geturl("http://product.dangdang.com/" + uid + ".html");
                        string isbn = Regex.Match(ahtml, "<li>国际标准书号ISBN：([\\s\\S]*?)</li>").Groups[1].Value;
                        string title = Regex.Match(ahtml, "<h1 title=\"([\\s\\S]*?)\"").Groups[1].Value;
                        string cate = Regex.Match(ahtml, "所属分类：</label>([\\s\\S]*?)</li>").Groups[1].Value;
                        string cbs = Regex.Match(ahtml, "dd_name=\"出版社\">([\\s\\S]*?)<").Groups[1].Value;
                        string cbstime = Regex.Match(ahtml, "出版时间:([\\s\\S]*?)<").Groups[1].Value;
                        string auther = Regex.Match(ahtml, "dd_name=\"作者\">([\\s\\S]*?)</").Groups[1].Value;
                        bool flag4 = title == "";
                        if (flag4)
                        {
                            MessageBox.Show("登录失效");
                            j--;
                            this.zanting = false;
                        }
                        else
                        {
                            this.label1.Text = "开始采集" + isbn;
                            Thread.Sleep(500);
                            string quanhtml = this.geturl("http://product.dangdang.com/index.php?r=callback%2Fproduct-info&productId=" + uid + "&isCatalog=0&shopId=0&productType=0");
                            MatchCollection activityUrl = Regex.Matches(quanhtml, "\"activityUrl\":\"([\\s\\S]*?)\"");
                            MatchCollection man = Regex.Matches(quanhtml, "\"couponMinUseValue\":\"([\\s\\S]*?)\"");
                            MatchCollection jian = Regex.Matches(quanhtml, "\"couponValue\":\"([\\s\\S]*?)\"");
                            StringBuilder sb = new StringBuilder();
                            for (int a = 0; a < man.Count; a++)
                            {
                                sb.Append(string.Concat(new string[]
                                {
                                    "满",
                                    man[a].Groups[1].Value,
                                    "减",
                                    jian[a].Groups[1].Value,
                                    "元"
                                }));
                            }
                            string huodongtime2 = "";
                            string huodong2 = "";
                            string quan = sb.ToString();
                            bool flag5 = activityUrl.Count > 0;
                            if (flag5)
                            {
                                huodongtime2 = huodongtime;
                                huodong2 = huodong;
                            }
                            string originalPrice = Regex.Match(quanhtml, "\"originalPrice\":\"([\\s\\S]*?)\"").Groups[1].Value;
                            string salePrice = Regex.Match(quanhtml, "\"salePrice\":\"([\\s\\S]*?)\"").Groups[1].Value;
                            string fee = "6";
                            double shiprice = this.jisuan(salePrice, huodong2, quan);
                            string shizhekou = (shiprice / Convert.ToDouble(originalPrice)).ToString("0.00");
                            bool flag6 = Convert.ToDouble(shiprice) - 6.0 >= 49.0;
                            if (flag6)
                            {
                                fee = "0";
                            }
                            while (!this.zanting)
                            {
                                Application.DoEvents();
                            }
                            bool flag7 = !this.status;
                            if (flag7)
                            {
                                return;
                            }
                            Thread.Sleep(100);
                            ListViewItem lv = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
                            lv.SubItems.Add(isbn);
                            lv.SubItems.Add(title);
                            lv.SubItems.Add(Regex.Replace(cate.Replace("&gt;", "-"), "<[^>]+>", ""));
                            lv.SubItems.Add(cbs);
                            lv.SubItems.Add(cbstime.Replace("&nbsp;", ""));
                            lv.SubItems.Add(auther);
                            lv.SubItems.Add(huodongtime2);
                            lv.SubItems.Add(huodong2);
                            lv.SubItems.Add(quan);
                            lv.SubItems.Add(originalPrice);
                            lv.SubItems.Add(salePrice);
                            lv.SubItems.Add(fee);
                            lv.SubItems.Add(shiprice.ToString());
                            lv.SubItems.Add(shizhekou);
                            bool flag8 = this.listView1.Items.Count > 2;
                            if (flag8)
                            {
                                this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                            }
                            Thread.Sleep(1500);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        Thread thread;
        bool zanting = true;
        bool status = true;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 当当网类目采集_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            webbrowser web = new webbrowser();
            web.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void 当当网类目采集_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
