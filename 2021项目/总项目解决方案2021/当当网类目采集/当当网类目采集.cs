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

        string cookie = "from=460-5-biaoti; __permanent_id=20210706110133441348145396004241224; __ddc_15d_f=1625540493%7C!%7C_utm_brand_id%3D11106; secret_key=0cc5a2a9a7ad3f000bc0ed76ef14a76d; order_follow_source=P-460-5-bi%7C%231%7C%23www.baidu.com%252Fother.php%253Fsc.Ks00000J_ZMyY4TLeJKVCi01ayQKHVaVYu9YHcST1X-TghuTUdG4Iftfnb--AEu_hJBY_q6OQ%7C%230-%7C-; __ddc_15d=1625636369%7C!%7C_utm_brand_id%3D11106; cart_id=3000502029245000048; ddscreen=2; __visit_id=20210709074613764170340241950465811; __out_refer=; dest_area=country_id%3D9000%26province_id%3D111%26city_id%3D0%26district_id%3D0%26town_id%3D0; permanent_key=20210709074953617181631368c2048c; alipay_request_from=https://login.dangdang.com/signin.aspx?returnurl=http%253A%252F%252Fproduct.dangdang.com%252F28548194.html; login_dang_code=20210709075019209511430343b3fa74; smidV2=202107090750469aee3c357e9bf07d89babed9068e4caa00b8119bbc42e98f0; __dd_token_id=20210709075114113456387708e833fc; USERNUM=MYzrLF7p3u5S9dBBtpO+aA==; login.dangdang.com=.AYH=20210709075047074766028&.ASPXAUTH=0HaueTlk+dZsdAmHviWD5QNYiX4KsgZb2j0LieFvUI4L3Z9wpmTEVg==; dangdang.com=email=MTc2MDYxMTc2MDYyNTk3MUBkZG1vYmlscGhvbmVfX3VzZXIuY29t&nickname=&display_id=1403940504405&customerid=qpxEEXuFAwxpZeZ7h4EIrQ==&viptype=blsR3X9DmaE=&show_name=176%2A%2A%2A%2A7606; ddoy=email=1760611760625971%40ddmobilphone__user.com&nickname=&agree_date=1&validatedflag=0&uname=17606117606&utype=&.ALFG=off&.ALTM=1625788274; sessionID=pc_52690c195902f208ddf08a555bb6cdd369ee498f5e624cb1970dc24bf73d4add; LOGIN_TIME=1625788296928; pos_9_end=1625788297361; pos_6_start=1625788297450; pos_6_end=1625788297457; __rpm=%7Cs_605253.451680112839%2C451680112840.1.1625788299483; __trace_id=20210709075140352156889427584759546";
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
                string hdhtml = method.GetUrl("http://promo.dangdang.com/6473096", "gb2312");
                string huodong = Regex.Match(hdhtml, @"<h1>([\s\S]*?)</h1>").Groups[1].Value.Trim();
                string huodongtime = Regex.Match(hdhtml, @"活动时间：([\s\S]*?)<").Groups[1].Value.Trim();
                

                for (int i = 1; i < 101; i++)
                {
                    string url = textBox1.Text.Replace("cp", "pg"+i+ "-cp");
                    string html = method.GetUrl(url,"gb2312");
                   
                    MatchCollection uids = Regex.Matches(html, @"id=""lcase([\s\S]*?)""");
                    if (uids.Count == 0)
                    {
                        MessageBox.Show("完成");
                        return;
                    }
                    foreach (Match uid in uids)
                    {
                   

                        string ahtml = geturl("http://product.dangdang.com/"+uid.Groups[1].Value+".html");
                        string isbn = Regex.Match(ahtml, @"<li>国际标准书号ISBN：([\s\S]*?)</li>").Groups[1].Value;
                        string title = Regex.Match(ahtml, @"<h1 title=""([\s\S]*?)""").Groups[1].Value;
                        string cate = Regex.Match(ahtml, @"所属分类：</label>([\s\S]*?)</li>").Groups[1].Value;
                        string cbs = Regex.Match(ahtml, @"dd_name=""出版社"">([\s\S]*?)<").Groups[1].Value;
                        string cbstime = Regex.Match(ahtml, @"出版时间:([\s\S]*?)<").Groups[1].Value;
                        string auther = Regex.Match(ahtml, @"dd_name=""作者"">([\s\S]*?)</").Groups[1].Value;



                        label1.Text = "开始采集" + isbn;

                        string quanhtml = geturl("http://product.dangdang.com/index.php?r=callback%2Fproduct-info&productId=" + uid.Groups[1].Value + "&isCatalog=0&shopId=0&productType=0");
                   
                        MatchCollection activityUrl = Regex.Matches(quanhtml, @"""activityUrl"":""([\s\S]*?)""");

                        MatchCollection man = Regex.Matches(quanhtml, @"""couponMinUseValue"":""([\s\S]*?)""");
                        MatchCollection jian= Regex.Matches(quanhtml, @"""couponValue"":""([\s\S]*?)""");

                        StringBuilder sb = new StringBuilder();


                        for (int a = 0; a < man.Count; a++)
                        {
                            sb.Append("满"+man[a].Groups[1].Value+"减"+ jian[a].Groups[1].Value+"元");

                        }
                        double shiprice = 0;
                        string shizhekou = "";
                        string huodongtime1 = "";
                        string huodong1 = "";
                        string quan = sb.ToString();
                        if (activityUrl.Count > 0)
                        {
                            huodongtime1 = huodongtime;
                            huodong1 = huodong;
                        }
                        
                       



                        string originalPrice = Regex.Match(quanhtml, @"""originalPrice"":""([\s\S]*?)""").Groups[1].Value;
                        string salePrice = Regex.Match(quanhtml, @"""salePrice"":""([\s\S]*?)""").Groups[1].Value;
                        string fee ="6";  //大于49包邮 小于49 收6元
                       shiprice = jisuan(salePrice,huodong1,quan);
                        shizhekou = (shiprice/Convert.ToDouble(originalPrice)).ToString("0.00"); ;
                       
                        if (Convert.ToDouble(shiprice) - 6 >= 49)
                        {
                            fee = "0";
                        }



                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                        Thread.Sleep(100);
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(isbn);
                        lv1.SubItems.Add(title);
                        lv1.SubItems.Add(Regex.Replace(cate.Replace("&gt;", "-"), "<[^>]+>", ""));
                        lv1.SubItems.Add(cbs);
                        lv1.SubItems.Add(cbstime.Replace("&nbsp;",""));
                        lv1.SubItems.Add(auther);
                        lv1.SubItems.Add(huodongtime1);
                        lv1.SubItems.Add(huodong1);
                        lv1.SubItems.Add(quan);
                        lv1.SubItems.Add(originalPrice);
                        lv1.SubItems.Add(salePrice);
                        lv1.SubItems.Add(fee);
                        lv1.SubItems.Add(shiprice.ToString());
                        lv1.SubItems.Add(shizhekou);

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
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
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
    }
}
