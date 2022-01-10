using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 孔网店铺图书
{
    public partial class 孔网店铺图书采集 : Form
    {
        public 孔网店铺图书采集()
        {
            InitializeComponent();
        }
        Thread thread;
        bool zanting = true;
        bool status = true;

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData, string charset)
        {
            try
            {
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                // request.Proxy = null;//防止代理抓包
               
                request.ContentType = "application/x-www-form-urlencoded";
               
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "IOS_KFZ_COM_3.10.0_iPhone14,5_15.1.1 #App Store,6FB137584B08435A8842DEF8F4E8D38E";
                request.Headers.Add("Cookie", "acw_tc=276077d616418046192976172ebd1e7fae6ea4270c4763e88726b0f3a719f5");

                request.Referer = url;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }


                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion
        public void run()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入店铺网址txt");
                return;
            }



            for (int page = 1; page < 9999; page++)
            {
                try
                {
                    label2.Text = "正在抓取第：" + page + "页";

                    string shopid = Regex.Match(textBox1.Text, @"\d{4,}").Groups[0].Value;

                    string url = "http://shop.kongfz.com/" + shopid + "/all/0_50_0_0_" + page + "_sort_desc_0_0/";

                    string html = method.GetUrl(url, "utf-8");
                    string shopname = Regex.Match(html, @"<div class=""shop_top_text"">([\s\S]*?)</div>").Groups[1].Value;
                    MatchCollection itemids = Regex.Matches(html, @"<div class=""item-row clearfix""([\s\S]*?)itemid=""([\s\S]*?)""");
                    MatchCollection userids = Regex.Matches(html, @"<div class=""item-row clearfix""([\s\S]*?)userid=""([\s\S]*?)""");
                    MatchCollection isbns = Regex.Matches(html, @"<div class=""item-row clearfix""([\s\S]*?)isbn=""([\s\S]*?)""");
                    MatchCollection names = Regex.Matches(html, @"class=""row-name"">([\s\S]*?)</a>");
                    MatchCollection prices = Regex.Matches(html, @"<div class=""row-price"">([\s\S]*?)</div>");
                    MatchCollection pinxiangs = Regex.Matches(html, @"<div class=""row-quality"">([\s\S]*?)</div>");
                  
                    if (names.Count == 0)
                    {
                        label2.Text = "采集结束";
                        break;
                    }



                    StringBuilder sb = new StringBuilder();
                    for (int a = 0; a < itemids.Count; a++)
                    {
                        sb.Append("%7B%22userId%22%3A%22" + userids[a].Groups[2].Value + "%22%2C%22itemId%22%3A%22" + itemids[a].Groups[2].Value + "%22%7D%2C");
                    }

                    string aurl = "https://shop.kongfz.com/book/shopsearch/getShippingFee?callback=jQuery111208550195114156878_1633577971044&params=%7B%22params%22%3A%5B" + sb.ToString().Remove(sb.ToString().Length - 3, 3) + "%5D%2C%22area%22%3A%221006000000%22%7D&_=1633577971050";
                    //textBox1.Text = aurl;
                    string htmls = method.GetUrl(aurl, "utf-8");

                    MatchCollection fees = Regex.Matches(htmls, @"""totalFee"":""([\s\S]*?)""");


                   




                    for (int j = 0; j < isbns.Count; j++)
                    {
                        try
                        {
                            string itemid = itemids[j].Groups[2].Value;
                            string ahtml = PostUrl("https://app.kongfz.com/invokeBook/app/api/getItemInfo", "itemId=" + itemid + "&shopId=" + shopid, "utf-8");
                            ahtml = method.Unicode2String(ahtml);
                            string tuijianyu = Regex.Match(ahtml, @"""shareTitle"":""([\s\S]*?)""").Groups[1].Value.Trim();
                            string shoujia = Regex.Match(ahtml, @"""price"":""([\s\S]*?)""").Groups[1].Value.Trim();
                            string dingjia = Regex.Match(ahtml, @"定价""([\s\S]*?)c"":""([\s\S]*?)""").Groups[2].Value.Trim().Replace("元", "");
                            string fee = Regex.Replace(fees[j].Groups[1].Value.Trim(), "<[^>]+>", "");
                            double dingjiazhekou = (Convert.ToDouble(shoujia) + Convert.ToDouble(fee)) / Convert.ToDouble(dingjia);


                            string kucun = Regex.Match(ahtml, @"""stock"":""([\s\S]*?)""").Groups[1].Value.Trim();
                            string huohao = Regex.Match(ahtml, @"货号""([\s\S]*?)c"":""([\s\S]*?)""").Groups[2].Value.Trim();
                            string zuozhe = Regex.Match(ahtml, @"作者""([\s\S]*?)c"":""([\s\S]*?)""").Groups[2].Value.Trim();
                            string cbs = Regex.Match(ahtml, @"出版社""([\s\S]*?)c"":""([\s\S]*?)""").Groups[2].Value.Trim();
                            string cbtime = Regex.Match(ahtml, @"出版时间""([\s\S]*?)c"":""([\s\S]*?)""").Groups[2].Value.Trim();
                            string sjtime = Regex.Match(ahtml, @"上书时间""([\s\S]*?)c"":""([\s\S]*?)""").Groups[2].Value.Trim();

                            string cate = Regex.Match(ahtml, @"categoryList([\s\S]*?)name"":""([\s\S]*?)""").Groups[2].Value.Trim();
                            string neirong = Regex.Match(ahtml, @"内容简介""([\s\S]*?)c"":""([\s\S]*?)""").Groups[2].Value.Trim();
                            string zuozheinfo = Regex.Match(ahtml, @"作者简介""([\s\S]*?)c"":""([\s\S]*?)""").Groups[2].Value.Trim();

                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            lv1.SubItems.Add(Regex.Replace(names[j].Groups[1].Value.Trim(), " <[^>] +> ", ""));
                            lv1.SubItems.Add(Regex.Replace(isbns[j].Groups[2].Value.Trim(), "<[^>]+>", ""));
                            lv1.SubItems.Add(tuijianyu);
                            lv1.SubItems.Add(Regex.Replace(pinxiangs[j].Groups[1].Value.Trim(), "<[^>]+>", ""));
                            lv1.SubItems.Add(dingjia);
                            lv1.SubItems.Add(shoujia);//售价
                            lv1.SubItems.Add(fee);
                            lv1.SubItems.Add(dingjiazhekou.ToString("F2"));
                            lv1.SubItems.Add(kucun);
                            lv1.SubItems.Add(huohao);
                            lv1.SubItems.Add(zuozhe);
                            lv1.SubItems.Add(cbs);
                            lv1.SubItems.Add(cbtime);
                            lv1.SubItems.Add(sjtime);
                            lv1.SubItems.Add(cate);
                            lv1.SubItems.Add(neirong);
                            lv1.SubItems.Add(zuozheinfo);
                        }
                        catch (Exception ex)
                        {

                           continue;
                        }






                        if (listView1.Items.Count > 2)
                        {
                            this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                        }
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;

                        Thread.Sleep(500);
                    }
                    

                }


                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }


            label2.Text = "采集结束";



        }
        private void Button6_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入店铺网址");
                return;
            }

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
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

        private void Button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
