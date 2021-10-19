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
using myDLL;

namespace 当当网类目采集
{
    public partial class 当当商家图书采集 : Form
    {
        public 当当商家图书采集()
        {
            InitializeComponent();
        }

        Thread thread;
        bool zanting = true;
        bool status = true;


        public double jisuan(string saleprice, string huodong, string quan)
        {
            double price = Convert.ToDouble(saleprice);
            try
            {
               

                if (huodong != "" && !huodong.Contains("\""))
                {
                    string man = Regex.Match(huodong, @"满([\s\S]*?)减").Groups[1].Value;
                    string jian = Regex.Match(huodong, @"减([\s\S]*?)元").Groups[1].Value;

                    if (Convert.ToDouble(saleprice) > Convert.ToDouble(man) || Convert.ToDouble(saleprice) >= 50)
                    {
                        double count = Math.Floor(Convert.ToDouble(saleprice) / Convert.ToDouble(man));
                        double yushu = Convert.ToDouble(saleprice) % 100;
                        if (yushu >= 50)
                        {


                            price = Math.Ceiling(price) + (100 - Math.Ceiling(yushu));
                            count = count + 1;
                        }

                        price = price - Convert.ToDouble(jian) * count;
                    }
                }
                if (quan != "")
                {
                    MatchCollection man = Regex.Matches(quan, @"满([\s\S]*?)减");
                    MatchCollection jian = Regex.Matches(quan, @"减([\s\S]*?)元");


                    if (man.Count == 1)  //一张优惠券
                    {
                        if (price >= Convert.ToDouble(man[0].Groups[1].Value))
                        {
                            price = price - Convert.ToDouble(jian[0].Groups[1].Value);
                        }
                        
                    }

                    if (man.Count == 2)  //两张优惠券
                    {
                        if (price >= Convert.ToDouble(man[0].Groups[1].Value) && price >= Convert.ToDouble(man[1].Groups[1].Value))
                        {
                            price = price - Convert.ToDouble(jian[0].Groups[1].Value);
                        }
                        if (price < Convert.ToDouble(man[0].Groups[1].Value) && price >= Convert.ToDouble(man[1].Groups[1].Value))
                        {
                            price = price - Convert.ToDouble(jian[1].Groups[1].Value);
                        }
                        if (price < Convert.ToDouble(man[0].Groups[1].Value) && price < Convert.ToDouble(man[1].Groups[1].Value))
                        {

                        }
                    }
                }



                return price;
            }
            catch (Exception)
            {

                return price;

            }
        }


        string fee = "0";

        /// <summary>
        /// APP端
        /// </summary>
        public void run()
        {
            try
            {
                fee = "0";
                int start_p = Convert.ToInt32(textBox2.Text);
                int end_p = Convert.ToInt32(textBox3.Text);
                int offset_p = Convert.ToInt32(textBox4.Text);

                for (int i = start_p; i <=end_p; i=i+offset_p)
                {
                    for (int page = 1; page < 101; page++)
                    {
                        string id = Regex.Match(textBox1.Text, @"http://shop.dangdang.com/([\s\S]*?)/").Groups[1].Value;
                       
                        string url = "http://shop.dangdang.com/"+id+"/list.html?inner_cat=all&lowp="+i+"&highp="+(i+offset_p)+"&page_index="+page+"#pos";
                      
                        string html = method.GetUrl(url, "gb2312");
                        MatchCollection uids = Regex.Matches(html, @"class=""pic""  href=""//product.dangdang.com/([\s\S]*?)\.");
                       
                        if (uids.Count == 0)
                        {
                            //MessageBox.Show("完成");
                            break;
                        }

                        for (int j = 0; j < uids.Count; j++)
                        {

                            try
                            {
                                string uid = uids[j].Groups[1].Value;
                                string aurl = "https://mapi.dangdang.com/index.php?action=get_product&user_client=iphone&client_version=11.7.3&union_id=537-50&permanent_id=20210730095015646129326676251121154&udid=CDD236322B97916364381C974957D10C&time_code=55F960825DDED6D818694D99AAC0C61A&timestamp=1627609894&lunbo_img_size=h&abtest=1&page_action=1&pid=" + uid + "&img_size=h&global_province_id=111&person_on=1";
                                string ahtml = method.GetUrl(aurl, "utf-8");


                                //获取运费
                                if(fee=="0")
                                {
                                   
                                    string template_id = Regex.Match(ahtml, @"""template_id"":""([\s\S]*?)""").Groups[1].Value;
                                    string feeurl = "http://product.dangdang.com/index.php?r=%2Fcallback%2Fshipping&shopId="+id+"&areaId=165220373&templateId="+ template_id + "&type=1";
                                    string feehtml = method.GetUrl(feeurl,"utf-8");
                                   fee= Regex.Match(method.Unicode2String(feehtml), @"运费([\s\S]*?)元").Groups[1].Value;
                                  
                                }

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
                                this.label1.Text = "开始采集" + isbn;



                                //活动开始
                                MatchCollection man = Regex.Matches(method.Unicode2String(ahtml), @"""promotion_name"":""([\s\S]*?)满([\s\S]*?)减([\s\S]*?)""");
                                MatchCollection huodong = Regex.Matches(method.Unicode2String(ahtml), @"""promotion_name"":""([\s\S]*?)""");
                                MatchCollection huodong_start = Regex.Matches(method.Unicode2String(ahtml), @"""start_date"":""([\s\S]*?)""");
                                MatchCollection huodong_end = Regex.Matches(method.Unicode2String(ahtml), @"""end_date"":""([\s\S]*?)""");


                                StringBuilder sb = new StringBuilder();
                                if (huodong_start.Count > 0)
                                {
                                   
                                    for (int a = 0; a < 1; a++) //一个活动
                                    {
                                        if (!huodong[a].Groups[1].Value.Contains("加价购"))
                                        {
                                            sb.Append(huodong[a].Groups[1].Value + "\r\n" +"开始时间"+ huodong_start[a].Groups[1].Value + "--结束时间" + huodong_end[a].Groups[1].Value);
                                        }
                                    }
                                }

                                string huodongall = sb.ToString().Replace("设置失败","限时抢");









                                string huodongyige = "";
                                if (man.Count > 0)
                                {
                                    huodongyige = "满" + man[0].Groups[2].Value + "减" + man[0].Groups[3].Value + "元";

                                }
                                //活动结束



                                //券开始
                                MatchCollection quanman = Regex.Matches(method.Unicode2String(ahtml), @"""coupon_label"":""([\s\S]*?)减([\s\S]*?)""");

                                StringBuilder quansb = new StringBuilder();
                                for (int a = 0; a < quanman.Count; a++)
                                {
                                    quansb.Append("满" + quanman[a].Groups[1].Value + "减" + quanman[a].Groups[2].Value + "元" + "\r\n");
                                }

                                string quanall = quansb.ToString().Trim();
                                //券结束



                               
                                string tag = "";






                                double shiprice = this.jisuan(salePrice.Trim(), huodongyige.Trim(), quanall.Trim());
                                string shizhekou = (shiprice / Convert.ToDouble(originalPrice)).ToString("0.00");

                                //if (Convert.ToDouble(shiprice) >= 49.0)
                                //{
                                //    fee = "0";
                                //}

                                //if (Convert.ToDouble(shiprice) < 49.0)
                                //{
                                    
                                //    fee = "0";

                                //}

                                //包邮新品标签开始
                                MatchCollection tags = Regex.Matches(method.Unicode2String(ahtml), @"item_trace([\s\S]*?)\]");
                                for (int a = 0; a < tags.Count; a++)
                                {
                                    if (tags[a].Groups[1].Value.Contains(uid))
                                    {
                                        if (tags[a].Groups[1].Value.Contains("包邮"))
                                        {
                                            fee = "0";
                                        }

                                        if (tags[a].Groups[1].Value.Contains("新品"))
                                        {
                                            tag = "新品";
                                        }
                                        if (tags[a].Groups[1].Value.Contains("特例品"))
                                        {
                                            tag = tag + "特例品";
                                        }
                                    }

                                }

                                //包邮新品标签结束

                                shiprice = shiprice + Convert.ToDouble(fee);

                                while (!this.zanting)
                                {
                                    Application.DoEvents();
                                }

                                if (this.status == false)
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
                                lv.SubItems.Add(huodongall);
                                lv.SubItems.Add(quanall);
                                lv.SubItems.Add(originalPrice);
                                lv.SubItems.Add(salePrice);
                                lv.SubItems.Add(fee);
                                lv.SubItems.Add(shiprice.ToString());
                                lv.SubItems.Add(shizhekou);
                                lv.SubItems.Add(tag);

                                if (this.listView1.Items.Count > 2)
                                {
                                    this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                                }
                                Thread.Sleep(1500);
                            }
                            catch (Exception e)
                            {
                               // MessageBox.Show(e.ToString());
                                continue;
                            }



                        }
                    }
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.ToString());
                ex.ToString();
            }

            label1.Text = "完成";
        }




        private void button1_Click(object sender, EventArgs e)
        {
            #region tongyongjiance

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"oFikH"))
            {

                return;
            }
            #endregion
            status = true;

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

        private void button7_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void 当当商家图书采集_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 当当商家图书采集_Load(object sender, EventArgs e)
        {

        }
    }
}
