using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

        public string getShippingFee2(string sb)
        {
            string url = "https://shop.kongfz.com/book/shopsearch/getShippingFee?callback=jQuery111202310343450011496_1652665962548&params=%7B%22params%22%3A%5B"+sb+"%5D%2C%22area%22%3A%221006000000%22%7D&_=1652665962549";
            string html = method.GetUrl(url, "utf-8");
          
            return html;

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



        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public  string PostUrl(string url, string postData,string ip)
        {
            try
            {
                string html = "";
                string COOKIE = "";
                string charset = "UTF-8";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.KeepAlive = false;
                request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                if(textBox1.Text!="")
                {
                    WebProxy proxy = new WebProxy(ip);
                    request.Proxy = proxy;
                }

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "";
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

        public void run_20220516()
        {
            listView1.Columns.Clear();
            try
            {
                listView1.Columns.Add("序号", 60, HorizontalAlignment.Center);

                listView1.Columns.Add("ISBN", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("最低售价", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("运费1", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("实付款1", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("店铺名称1", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("成交率1", 80, HorizontalAlignment.Center);


                listView1.Columns.Add("次低售价", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("运费2", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("实付款2", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("店铺名称2", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("成交率2", 80, HorizontalAlignment.Center);


                listView1.Columns.Add("第三低售价", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("运费3", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("实付款3", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("店铺名称3", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("成交率3", 80, HorizontalAlignment.Center);


                listView1.Columns.Add("第四售价", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("运费4", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("实付款4", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("店铺名称4", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("成交率4", 80, HorizontalAlignment.Center);

                listView1.Columns.Add("第五低售价", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("运费5", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("实付款5", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("店铺名称5", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("成交率5", 80, HorizontalAlignment.Center);

                listView1.Columns.Add("书名", 80, HorizontalAlignment.Center);

                if (textBox2.Text == "")
                {
                    MessageBox.Show("请导入账号");
                    return;
                }
                DataTable dt = method.ExcelToDataTable(textBox2.Text, true);
                string q1 = "100h"; //全新
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    try
                    {
                        DataRow dr = dt.Rows[i];
                        string isbn = dr[0].ToString();

                        if (isbn.Trim() == "")
                            break;
                        string url = "https://app.kongfz.com/invokeSearch/app/product/productSearchV2";
                        string postdata = "_stpmt=ewoKfQ%3D%3D&params=%7B%22key%22%3A%22" + isbn + "%22%2C%22pagesize%22%3A%2220%22%2C%22status%22%3A%220%22%2C%22pagenum%22%3A%221%22%2C%22order%22%3A%22100%22%2C%22area%22%3A%221001000000%22%2C%22select%22%3A%220%22%2C%22quality%22%3A%22"+ q1 + "%22%2C%22isFuzzy%22%3A%220%22%7D&type=2";
                        string ip = textBox1.Text.Trim();
                        string html = PostUrl(url, postdata,ip);
                        html = method.Unicode2String(html);
                        MatchCollection itemIds = Regex.Matches(html, @"""itemId"":([\s\S]*?),");


                        MatchCollection itemNames = Regex.Matches(html, @"""itemName"":""([\s\S]*?)""");
                        MatchCollection userIds = Regex.Matches(html, @"""userId"":([\s\S]*?),");
                        MatchCollection shopnames = Regex.Matches(html, @"""shopName"":""([\s\S]*?)""");
                        MatchCollection prices = Regex.Matches(html, @"""price"":([\s\S]*?),");
                        MatchCollection cjls = Regex.Matches(html, @"成交率([\s\S]*?)""");

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   

                        StringBuilder sb = new StringBuilder();

                        int count = 5;
                        if (itemIds.Count < 5)
                        {
                            count = itemIds.Count;
                        }
                        
                        StringBuilder sb2 = new StringBuilder();
                        for (int j = 0; j < count; j++)
                        {
                            sb2.Append("%7B%22userId%22%3A%22" + userIds[j].Groups[1].Value + "%22%2C%22itemId%22%3A%22" + itemIds[j].Groups[1].Value + "%22%7D%2C");

                        }

                        Dictionary<string, string> feedic = new Dictionary<string, string>();
                        if (sb2.ToString().Length > 10)
                        {

                            string feehtml = getShippingFee2(sb2.ToString().Remove(sb2.ToString().Length - 3, 3));

                            MatchCollection feeitemIds = Regex.Matches(feehtml, @"""itemId"":""([\s\S]*?)""");
                            MatchCollection feeinhtml = Regex.Matches(feehtml, @"{""fee""([\s\S]*?)\]");

                      
                            for (int j = 0; j < count; j++)
                            {
                                string fee= Regex.Match(feeinhtml[j].Groups[1].Value, @"""totalFee"":([\s\S]*?),").Groups[1].Value;
                                if(fee=="")
                                {
                                    fee = "0";
                                }    
                                feedic.Add(feeitemIds[j].Groups[1].Value, fee.Replace("\"",""));
                            }
                        }

                        lv1.SubItems.Add(isbn);
                        for (int a = 0; a < count; a++)
                        {
                            double totalprice = Convert.ToDouble(prices[a].Groups[1].Value) + Convert.ToDouble(feedic[itemIds[a].Groups[1].Value]);
                            lv1.SubItems.Add(prices[a].Groups[1].Value);
                            lv1.SubItems.Add(feedic[itemIds[a].Groups[1].Value]);
                            lv1.SubItems.Add(totalprice.ToString());
                            lv1.SubItems.Add(method.Unicode2String(shopnames[a].Groups[1].Value));
                            lv1.SubItems.Add(cjls[a].Groups[1].Value);
                        }

                        if (count > 1)
                        {
                            string bookname = itemNames[count - 1].Groups[1].Value;
                            lv1.SubItems.Add(bookname);
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
                        label1.Text = "正在查询：" + isbn;
                        Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {

                        continue;
                        //MessageBox.Show(ex.ToString());
                    }
                }
                label1.Text = ("查询结束");
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.ToString());
            }
        }




        public void run_20220516_10()
        {
            listView1.Columns.Clear();
            try
            {
                listView1.Columns.Add("序号", 60, HorizontalAlignment.Center);

                listView1.Columns.Add("ISBN", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("最低售价", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("运费1", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("实付款1", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("店铺名1", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("统计值1", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("差值1", 80, HorizontalAlignment.Center);


                listView1.Columns.Add("次低售价", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("运费2", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("实付款2", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("店铺名2", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("统计值2", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("差值2", 80, HorizontalAlignment.Center);


                listView1.Columns.Add("第三售价", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("运费3", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("实付款3", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("店铺名3", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("统计值3", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("差值3", 80, HorizontalAlignment.Center);


                listView1.Columns.Add("第四售价", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("运费4", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("实付款4", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("店铺名4", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("统计值4", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("差值4", 80, HorizontalAlignment.Center);

                listView1.Columns.Add("第五售价", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("运费5", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("实付款5", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("店铺名5", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("统计值5", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("差值5", 80, HorizontalAlignment.Center);


                //后加5个
                listView1.Columns.Add("第六售价", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("运费6", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("实付款6", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("店铺名6", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("统计值6", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("差值6", 80, HorizontalAlignment.Center);

                listView1.Columns.Add("第七售价", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("运费7", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("实付款7", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("店铺名7", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("统计值7", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("差值7", 80, HorizontalAlignment.Center);

                listView1.Columns.Add("第八售价", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("运费8", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("实付款8", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("店铺名8", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("统计值8", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("差值8", 80, HorizontalAlignment.Center);

                listView1.Columns.Add("第九售价", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("运费9", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("实付款9", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("店铺名9", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("统计值9", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("差值9", 80, HorizontalAlignment.Center);

                listView1.Columns.Add("第十售价", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("运费10", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("实付款10", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("店铺名10", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("统计值10", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("差值10", 80, HorizontalAlignment.Center);





                listView1.Columns.Add("书名", 80, HorizontalAlignment.Center);

                if (textBox2.Text == "")
                {
                    MessageBox.Show("请导入账号");
                    return;
                }
                DataTable dt = method.ExcelToDataTable(textBox2.Text, true);
                string q1 = "100h"; //全新
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    try
                    {
                        DataRow dr = dt.Rows[i];
                        string isbn = dr[0].ToString();

                        if (isbn.Trim() == "")
                            break;
                        string url = "https://app.kongfz.com/invokeSearch/app/product/productSearchV2";
                        string postdata = "_stpmt=ewoKfQ%3D%3D&params=%7B%22key%22%3A%22" + isbn + "%22%2C%22pagesize%22%3A%2220%22%2C%22status%22%3A%220%22%2C%22pagenum%22%3A%221%22%2C%22order%22%3A%22100%22%2C%22area%22%3A%221001000000%22%2C%22select%22%3A%220%22%2C%22quality%22%3A%22" + q1 + "%22%2C%22isFuzzy%22%3A%220%22%7D&type=2";
                        string ip = textBox1.Text.Trim();
                        string html = PostUrl(url, postdata, ip);
                        html = method.Unicode2String(html);
                        MatchCollection itemIds = Regex.Matches(html, @"""itemId"":([\s\S]*?),");


                        MatchCollection itemNames = Regex.Matches(html, @"""itemName"":""([\s\S]*?)""");
                        MatchCollection userIds = Regex.Matches(html, @"""userId"":([\s\S]*?),");
                        MatchCollection shopnames = Regex.Matches(html, @"""shopName"":""([\s\S]*?)""");
                        MatchCollection prices = Regex.Matches(html, @"""price"":([\s\S]*?),");
                       // MatchCollection cjls = Regex.Matches(html, @"成交率([\s\S]*?)""");

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   

                        StringBuilder sb = new StringBuilder();

                        int count = 10;
                        if (itemIds.Count < 10)
                        {
                            count = itemIds.Count;
                        }

                        StringBuilder sb2 = new StringBuilder();
                        for (int j = 0; j < count; j++)
                        {
                            sb2.Append("%7B%22userId%22%3A%22" + userIds[j].Groups[1].Value + "%22%2C%22itemId%22%3A%22" + itemIds[j].Groups[1].Value + "%22%7D%2C");

                        }

                        Dictionary<string, string> feedic = new Dictionary<string, string>();
                        if (sb2.ToString().Length > 10)
                        {

                            string feehtml = getShippingFee2(sb2.ToString().Remove(sb2.ToString().Length - 3, 3));

                            MatchCollection feeitemIds = Regex.Matches(feehtml, @"""itemId"":""([\s\S]*?)""");
                            MatchCollection feeinhtml = Regex.Matches(feehtml, @"{""fee""([\s\S]*?)\]");


                            for (int j = 0; j < count; j++)
                            {
                                string fee = Regex.Match(feeinhtml[j].Groups[1].Value, @"""totalFee"":([\s\S]*?),").Groups[1].Value;
                                if (fee == "")
                                {
                                    fee = "0";
                                }
                                feedic.Add(feeitemIds[j].Groups[1].Value, fee.Replace("\"", ""));
                            }
                        }

                        lv1.SubItems.Add(isbn);

                        int tongji = 0;
                        double lastshifu = 0;
                        for (int a = 0; a < count; a++)
                        {
                            double totalprice = Convert.ToDouble(prices[a].Groups[1].Value) + Convert.ToDouble(feedic[itemIds[a].Groups[1].Value]);
                            lv1.SubItems.Add(prices[a].Groups[1].Value);
                            lv1.SubItems.Add(feedic[itemIds[a].Groups[1].Value]);
                            lv1.SubItems.Add(totalprice.ToString());
                            lv1.SubItems.Add(method.Unicode2String(shopnames[a].Groups[1].Value));

                            double chazhi = totalprice - lastshifu;
                            lv1.SubItems.Add(tongji.ToString());
                            lv1.SubItems.Add("");

                           
                            if(a!=0)
                            {
                               
                                lv1.SubItems[(6 * (a-1)) + 7].Text = chazhi.ToString("0.00");
                            }
                            // lv1.SubItems.Add(cjls[a].Groups[1].Value);

                            lastshifu = totalprice;
                        }

                        if (count > 1)
                        {
                            string bookname = itemNames[count - 1].Groups[1].Value;
                            lv1.SubItems.Add(bookname);
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
                        label1.Text = "正在查询：" + isbn;
                        Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {

                        continue;
                        //MessageBox.Show(ex.ToString());
                    }
                }
                label1.Text = ("查询结束");
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.ToString());
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
                thread = new Thread(run_20220516_10);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            webbrowser web = new webbrowser();
            web.Show();
        }
        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }
        #endregion
        private void 孔夫子淘宝低价_Load(object sender, EventArgs e)
        {
            #region 通用检测


            string html = getHtml("http://acaiji.com/index/index/vip.html");

            if (!html.Contains(@"DZkGm"))
            {

                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            #endregion

        }


        IDictionary<string,int> tongjidic =new Dictionary<string,int>();

        public void tongji()
        {
            button7.Enabled = false;
            tongjidic.Clear();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                for (int j = 0; j < listView1.Columns.Count; j++)
                {


                    if (listView1.Columns[j].Text.Contains("店铺名"))
                    {
                        try
                        {
                            if (tongjidic.ContainsKey(listView1.Items[i].SubItems[j].Text + j))
                            {
                                tongjidic[listView1.Items[i].SubItems[j].Text + j] = tongjidic[listView1.Items[i].SubItems[j].Text + j] + 1;
                            }
                            else
                            {
                                tongjidic.Add(listView1.Items[i].SubItems[j].Text + j, 1);
                            }
                        }
                        catch (Exception)
                        {

                            continue;
                        }

                    }

                }
            }


            for (int i = 0; i < listView1.Items.Count; i++)
            {
                for (int j = 0; j < listView1.Columns.Count; j++)
                {

                    if (listView1.Columns[j].Text.Contains("店铺名"))
                    {
                        try
                        {
                            listView1.Items[i].SubItems[j + 1].Text = tongjidic[listView1.Items[i].SubItems[j].Text + j].ToString();
                        }
                        catch (Exception)
                        {

                            continue;
                        }

                    }
                }
            }
            button7.Enabled = true;
        }
        private void button7_Click_1(object sender, EventArgs e)
        {
           tongji();
            //Thread thread = new Thread(tongji);
            //thread.Start();
            //Control.CheckForIllegalCrossThreadCalls = false;


        }

        private void 孔夫子淘宝低价_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
