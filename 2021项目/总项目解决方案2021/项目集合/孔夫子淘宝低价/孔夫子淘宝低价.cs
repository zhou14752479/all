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
using myDLL;

namespace 孔夫子淘宝低价
{
    public partial class 孔夫子淘宝低价 : Form
    {
        public 孔夫子淘宝低价()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            //  openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";

            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox2.Text = openFileDialog1.FileName;

            }
        }


        Thread thread;
        bool zanting = true;
        bool status = true;
        string cookie = "";


        public string getShippingFee(string itemid,string userid)
        {
            string url = "http://shop.kongfz.com/book/shopsearch/getShippingFee?callback=jQuery111209607689280462008_1625446557129&params={%22params%22:[{%22userId%22:%22"+userid+"%22,%22itemId%22:%22"+itemid+"%22}],%22area%22:%221006000000%22}&_=1625446557140";
            string html = method.GetUrl(url,"utf-8");
            string fee = Regex.Match(html, @"""totalFee"":""([\s\S]*?)""").Groups[1].Value;
            return fee;

        }
        public void run()
        {

            try
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("请导入账号");
                    return;
                }
                DataTable dt = method.ExcelToDataTable(textBox2.Text, true);

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    DataRow dr = dt.Rows[i];
                    string isbn = dr[0].ToString();
                   

                    //string q = "95h"; //九五品以上
                    string q1 = "100h"; //全新
                  string q2 = "95h95"; //九五品
                    

                    string url = "https://app.kongfz.com/invokeSearch/app/product/productSearchV2";
                    string postdata = "_stpmt=ewoKfQ%3D%3D&params=%7B%22key%22%3A%22" + isbn + "%22%2C%22pagesize%22%3A%2220%22%2C%22status%22%3A%220%22%2C%22pagenum%22%3A%221%22%2C%22order%22%3A%22100%22%2C%22area%22%3A%221001000000%22%2C%22select%22%3A%220%22%2C%22quality%22%3A%22" + q1 + "%22%2C%22isFuzzy%22%3A%220%22%7D&type=2";
                    string html = method.PostUrl(url, postdata, "", "utf-8", "application/x-www-form-urlencoded", "");
                  
                    MatchCollection itemIds = Regex.Matches(html, @"""itemId"":([\s\S]*?),");
                    MatchCollection userIds = Regex.Matches(html, @"""userId"":([\s\S]*?),");
                    MatchCollection shopnames = Regex.Matches(html, @"""shopName"":""([\s\S]*?)""");
                    MatchCollection prices = Regex.Matches(html, @"""price"":([\s\S]*?),");
                    MatchCollection qualitys = Regex.Matches(html, @"""quality"":""([\s\S]*?)""");

                    string price100 = "无";
                    string price1002 = "无"; //全新品相次低价
                    string fee1002 = "";

                    string price95 = "无";



                    string fee100 = "";
                    string fee95 = "";

                    string shopname95 = "无";
                    string shopname100= "无";
                    string shopname1002 = "无";

                    string url2 = "https://app.kongfz.com/invokeSearch/app/product/productSearchV2";
                    string postdata2 = "_stpmt=ewoKfQ%3D%3D&params=%7B%22key%22%3A%22" + isbn + "%22%2C%22pagesize%22%3A%2220%22%2C%22status%22%3A%220%22%2C%22pagenum%22%3A%221%22%2C%22order%22%3A%22100%22%2C%22area%22%3A%221001000000%22%2C%22select%22%3A%220%22%2C%22quality%22%3A%22" + q2 + "%22%2C%22isFuzzy%22%3A%220%22%7D&type=2";
                    string html2 = method.PostUrl(url2, postdata2, "", "utf-8", "application/x-www-form-urlencoded", "");

                    MatchCollection itemIds2 = Regex.Matches(html2, @"""itemId"":([\s\S]*?),");
                    MatchCollection userIds2 = Regex.Matches(html2, @"""userId"":([\s\S]*?),");
                    MatchCollection shopnames2 = Regex.Matches(html2, @"""shopName"":""([\s\S]*?)""");
                    MatchCollection prices2 = Regex.Matches(html2, @"""price"":([\s\S]*?),");
                    MatchCollection qualitys2 = Regex.Matches(html2, @"""quality"":""([\s\S]*?)""");



                    for (int j = 0; j <prices.Count; j++)
                    {
                       
                        if (method.Unicode2String(qualitys[j].Groups[1].Value) == "全新")
                        {
                            if (price100 == "无")
                            {
                                fee100 = getShippingFee(itemIds[j].Groups[1].Value, userIds[j].Groups[1].Value);
                                price100 = prices[j].Groups[1].Value;
                                if (j + 1 < prices.Count)
                                {
                                    price1002 = prices[j + 1].Groups[1].Value;
                                   fee1002 = getShippingFee(itemIds[j+1].Groups[1].Value, userIds[j+1].Groups[1].Value);
                                    shopname1002 = shopnames[j + 1].Groups[1].Value;
                                }
                                shopname100 = shopnames[j].Groups[1].Value;
                                break;
                            }



                        }
                    }

                    for (int j = 0; j < prices2.Count; j++)
                    {
                        if (method.Unicode2String(qualitys2[j].Groups[1].Value) == "九五品")
                        {
                            if (price95 == "无")
                            {
                                fee95 = getShippingFee(itemIds2[j].Groups[1].Value, userIds2[j].Groups[1].Value);
                                price95 = prices2[j].Groups[1].Value;
                                shopname95 = shopnames2[j].Groups[1].Value;
                                break;
                            }
                        }
                    }

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(isbn);
       
                    lv1.SubItems.Add(price100);
                    lv1.SubItems.Add(fee100);
                    lv1.SubItems.Add(method.Unicode2String(shopname100));

                    lv1.SubItems.Add(price1002);
                    lv1.SubItems.Add(fee1002);
                    lv1.SubItems.Add(method.Unicode2String(shopname1002));

                    lv1.SubItems.Add(price95);
                    lv1.SubItems.Add(fee95);
                    lv1.SubItems.Add(method.Unicode2String(shopname95));
                  

                    if(listView1.Items.Count>2)
                    {
                        this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                    }
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;
                    label1.Text = "正在查询：" + isbn;
                    Thread.Sleep(1000);
                }
                label1.Text = ("查询结束");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }



        public void run1()
        {

            try
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("请导入账号");
                    return;
                }
                DataTable dt = method.ExcelToDataTable(textBox2.Text, true);

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    DataRow dr = dt.Rows[i];
                    string isbn = dr[0].ToString();


                    string q = "95h"; //九五品以上
                                      // string q1 = "100h"; //全新
                                      //  string q2 = "95h95"; //九五品


                    string url = "https://app.kongfz.com/invokeSearch/app/product/productSearchV2";
                    string postdata = "_stpmt=ewoKfQ%3D%3D&params=%7B%22key%22%3A%22" + isbn + "%22%2C%22pagesize%22%3A%2220%22%2C%22status%22%3A%220%22%2C%22pagenum%22%3A%221%22%2C%22order%22%3A%22100%22%2C%22area%22%3A%221001000000%22%2C%22select%22%3A%220%22%2C%22quality%22%3A%22" + q + "%22%2C%22isFuzzy%22%3A%220%22%7D&type=2";
                    string html = method.PostUrl(url, postdata, "", "utf-8", "application/x-www-form-urlencoded", "");

                    MatchCollection itemIds = Regex.Matches(html, @"""itemId"":([\s\S]*?),");
                    MatchCollection userIds = Regex.Matches(html, @"""userId"":([\s\S]*?),");


                    MatchCollection shopnames = Regex.Matches(html, @"""shopName"":""([\s\S]*?)""");
                    MatchCollection prices = Regex.Matches(html, @"""price"":([\s\S]*?),");
                    MatchCollection qualitys = Regex.Matches(html, @"""quality"":""([\s\S]*?)""");

                    string price100 = "无";
                    string price95 = "无";

                    string fee100 = "";
                    string fee95 = "";

                    string shopname95 = "无";
                    string shopname100 = "无";

                    for (int j = 0; j < prices.Count; j++)
                    {
                        if (method.Unicode2String(qualitys[j].Groups[1].Value) == "九五品")
                        {
                            if (price95 == "无")
                            {
                                fee95 = getShippingFee(itemIds[j].Groups[1].Value, userIds[j].Groups[1].Value);
                                price95 = prices[j].Groups[1].Value;
                                shopname95 = shopnames[j].Groups[1].Value;
                            }
                        }
                        if (method.Unicode2String(qualitys[j].Groups[1].Value) == "全新")
                        {
                            if (price100 == "无")
                            {
                                fee100 = getShippingFee(itemIds[j].Groups[1].Value, userIds[j].Groups[1].Value);
                                price100 = prices[j].Groups[1].Value;
                                shopname100 = shopnames[j].Groups[1].Value;
                            }
                        }
                    }


                    // string ahtml = getHtml("https://s.taobao.com/search?q=" + isbn+ "&sort=total-asc");
                    //string tbprice = Regex.Match(ahtml, @"""view_price"":""([\s\S]*?)""").Groups[1].Value;
                    // string shop= Regex.Match(ahtml, @"""nick"":""([\s\S]*?)""").Groups[1].Value;
                    // string istmall = Regex.Match(ahtml, @"""isTmall"":([\s\S]*?),").Groups[1].Value;

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(isbn);

                    lv1.SubItems.Add(price100);
                    lv1.SubItems.Add(fee100);
                    lv1.SubItems.Add(method.Unicode2String(shopname100));
                    lv1.SubItems.Add(price95);
                    lv1.SubItems.Add(fee95);
                    lv1.SubItems.Add(method.Unicode2String(shopname95));


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
                    label1.Text = "正在查询：" + isbn;
                    Thread.Sleep(1000);
                }
                label1.Text = ("查询结束");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        public string getHtml(string url)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.186 Safari/537.36");
            headers.Add("Cookie", cookie);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.UserAgent = headers["user-agent"];
            request.Timeout = 5000;
            request.KeepAlive = true;
            request.Headers["Cookie"] = headers["Cookie"];
            HttpWebResponse Response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(Response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
            string content = reader.ReadToEnd();
            return content;

        }

      
       
        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
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

        private void button1_Click(object sender, EventArgs e)
        {

            #region 通用检测


            string html = getHtml("http://acaiji.com/index/index/vip.html");

            if (!html.Contains(@"DZkGm"))
            {

                return;
            }

            #endregion
            //cookie = webbrowser.COOKIE;

            //if (cookie == "")
            //{
            //    MessageBox.Show("请先登录");

            //    return;
            //}
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            webbrowser web = new webbrowser();
            web.Show();
        }

        private void 孔夫子淘宝低价_Load(object sender, EventArgs e)
        {

        }
    }
}
