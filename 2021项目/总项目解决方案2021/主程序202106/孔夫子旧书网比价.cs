using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 主程序202106
{
    public partial class 孔夫子旧书网比价 : Form
    {
        [DllImport("user32.dll")]
        public static extern int MessageBoxTimeoutA(IntPtr hWnd, string msg, string Caps, int type, int Id, int time);//引用DLL
        public 孔夫子旧书网比价()
        {
            InitializeComponent();
        }
        Thread thread;
        bool zanting = true;
        bool status = true;
      


        //导入ISBN、品相查询低价
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
                    string quality = dr[1].ToString();


                    string q = "95h95";
                    switch (quality)
                    {
                        case "九五品":
                            q = "95h95";
                            break;
                        case "九品":
                            q = "90h90";
                            break;
                        case "四品":
                            q = "40h40";
                            break;
                        case "八品":
                            q = "80h80";
                            break;
                        case "七品":
                            q = "70h70";
                            break;
                        case "六品":
                            q = "60h60";
                            break;
                        case "五品":
                            q = "50h50";
                            break;

                    }

                    //  string url = string.Format("http://search.kongfz.com/product_result/?key={0}&status=0&_stpmt=eyJzZWFyY2hfdHlwZSI6ImFjdGl2ZSJ9&quality={1}&quaselect=3&order=100&ajaxdata=4&type=1&_=1622545561251", isbn, q);


                    //string html = method.GetUrlWithCookie(url, cookie, "utf-8");
                    string url = "https://app.kongfz.com/invokeSearch/app/product/productSearchV2";
                    string postdata = "_stpmt=ewoKfQ%3D%3D&params=%7B%22key%22%3A%22" + isbn + "%22%2C%22pagesize%22%3A%2220%22%2C%22status%22%3A%220%22%2C%22pagenum%22%3A%221%22%2C%22order%22%3A%22100%22%2C%22area%22%3A%221001000000%22%2C%22select%22%3A%220%22%2C%22quality%22%3A%22" + q + "%22%2C%22isFuzzy%22%3A%220%22%7D&type=2";
                    string html = method.PostUrl(url, postdata, "", "utf-8", "application/x-www-form-urlencoded", "");


                    //if (html.Contains("invalid request"))
                    //{
                    //    getcookies();
                    //    j = j - 1;

                    //    MessageBoxTimeoutA((IntPtr )0,"2秒后自动关闭,自动获取cookie","消息框",0,0,2000);// 直接调用  3秒后自动关闭 父窗口句柄没有直接用0代替
                    //    continue;
                    //}

                    MatchCollection prices = Regex.Matches(html, @"""price"":([\s\S]*?),");

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;
                    label1.Text = "正在查询：" + isbn;
                    string price = "无";
                    if (prices.Count > 0)
                    {
                        price = prices[0].Groups[1].Value;
                    }
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(isbn);
                    lv1.SubItems.Add(quality);
                    lv1.SubItems.Add(price);

                    Thread.Sleep(1000);
                }
                label1.Text = ("查询结束");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }




        //获取运费
        public string getfee(string itemid,string userid)
        {
            string url = "https://app.kongfz.com/invokeShop/app/shop/batchGetShippingFees";
            string postdata = "area=18016000000&params=%5B%7B%22userId%22%20%3A%20%22"+userid+"%22%2C%22itemId%22%20%3A%20%22"+itemid+"%22%7D%5D&withCoupon=1";
          
            string html = method.PostUrl(url, postdata, "", "utf-8", "application/x-www-form-urlencoded", "");
            string totalFee = Regex.Match(html, @"""totalFee"":""([\s\S]*?)""").Groups[1].Value;
            if(totalFee=="")
            {
                totalFee = "0";
            }
            return totalFee;

        }


        //APP端
        //导入ISBN查询低价\邮费、已售数量
        public void run1()
        {

            try
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("请导入账号");
                    return;
                }
                //  DataTable dt = method.ReadExcelToTable(textBox2.Text);
                // DataTable dt = method.ExcelToDataTable(textBox2.Text,true);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{

                StreamReader sr = new StreamReader(textBox2.Text, method.EncodingType.GetTxtType(textBox2.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {

                    //DataRow dr = dt.Rows[i];
                    // string isbn = dr[0].ToString();
                    string isbn = text[i];
                    if (isbn == "")
                        continue;
                    string url = "https://app.kongfz.com/invokeSearch/app/product/productSearchV2";
                    string postdata = "_stpmt=ewoKfQ%3D%3D&params=%7B%22key%22%3A%22" + isbn + "%22%2C%22pagesize%22%3A%2220%22%2C%22status%22%3A%220%22%2C%22pagenum%22%3A%221%22%2C%22order%22%3A%22100%22%2C%22area%22%3A%221001000000%22%2C%22select%22%3A%220%22%2C%22isFuzzy%22%3A%220%22%7D&type=2";
                    string html = PostUrl(url, postdata);
                   
                    MatchCollection prices = Regex.Matches(html, @"""price"":([\s\S]*?),");
                    string itemid = Regex.Match(html, @"""itemId"":([\s\S]*?),").Groups[1].Value;
                    string userid = Regex.Match(html, @"""userId"":([\s\S]*?),").Groups[1].Value;
                 
                    string fee = getfee(itemid,userid);


                    Thread.Sleep(2000);
                    //获取已售搜索个数
                   
                    string postdata2 = "_stpmt=ewoKfQ%3D%3D&params=%7B%22key%22%3A%22" + isbn + "%22%2C%22pagesize%22%3A%2220%22%2C%22status%22%3A%221%22%2C%22pagenum%22%3A%221%22%2C%22order%22%3A%22100%22%2C%22area%22%3A%221001000000%22%2C%22select%22%3A%220%22%2C%22isFuzzy%22%3A%220%22%7D&type=2";

                    string html2 =PostUrl(url, postdata2);
                    string count=  Regex.Match(html2, @"""recordCount"":([\s\S]*?),").Groups[1].Value;
                    string isFuzzy = Regex.Match(html2, @"""isFuzzy"":([\s\S]*?),").Groups[1].Value;
                    Thread.Sleep(2000);

                    if(isFuzzy=="1")
                    {
                        count = "无";
                    }

                    label1.Text = "正在查询：" + isbn;
                    string price = "无";
                    if (prices.Count > 0)
                    {
                        price = prices[0].Groups[1].Value;
                    }
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(isbn);
                    lv1.SubItems.Add(price.Replace("\"",""));
                    lv1.SubItems.Add(fee);
                    lv1.SubItems.Add(count);

                   

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;
                }
                label1.Text = ("查询结束");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        #region POST默认请求
        public static string PostUrlDefault(string url, string postData, string COOKIE,string ip)
        {
            string result;
            try
            {
                string charset = "utf-8";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                WebProxy proxy = new WebProxy(ip);
                request.Proxy = proxy;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("version:TYC-XCX-WX");
                request.ContentType = "application/x-www-form-urlencoded";
                // request.ContentType = "application/json";
                request.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
                request.Referer = url;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                response.GetResponseHeader("Set-Cookie");
                bool flag = response.Headers["Content-Encoding"] == "gzip";
                string html;
                if (flag)
                {
                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader2 = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
                    html = reader2.ReadToEnd();
                    reader2.Close();
                }
                response.Close();
                result = html;
            }
            catch (WebException ex)
            {
                //result = ex.ToString();
                //400错误也返回内容
                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }
        #endregion


        //小程序手机端
        //导入ISBN查询低价\邮费、已售数量
        public void run_xcx()
        {
            string ip = textBox1.Text;
            try
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("请导入账号");
                    return;
                }
                //  DataTable dt = method.ReadExcelToTable(textBox2.Text);
                // DataTable dt = method.ExcelToDataTable(textBox2.Text,true);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{

                StreamReader sr = new StreamReader(textBox2.Text, method.EncodingType.GetTxtType(textBox2.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {

                    //DataRow dr = dt.Rows[i];
                    // string isbn = dr[0].ToString();
                    string isbn = text[i];
                    if (isbn == "")
                        continue;
                    string url = "https://app.kongfz.com/invokeSearch/app/product/productSearchV2";
                    string postdata = "bizType=wechat&host=msearch&type=1&params=%7B%22key%22%3A%22"+isbn+"%22%2C%22status%22%3A0%2C%22pagenum%22%3A1%2C%22pagesize%22%3A10%2C%22order%22%3A100%2C%22select%22%3A0%2C%22isFuzzy%22%3Afalse%2C%22area%22%3A1001000000%2C%22quaselect%22%3A1%7D";
                    string html = PostUrlDefault(url, postdata,"utf-8",ip);
                    html = method.Unicode2String(html);
                 
                    MatchCollection prices = Regex.Matches(html, @"""price"":([\s\S]*?),");
                    string itemid = Regex.Match(html, @"""itemId"":([\s\S]*?),").Groups[1].Value;
                    string userid = Regex.Match(html, @"""userId"":([\s\S]*?),").Groups[1].Value;

                    string fee = getfee(itemid, userid);


                    Thread.Sleep(1000);
                    //获取已售搜索个数

                    //string postdata2 = "_stpmt=ewoKfQ%3D%3D&params=%7B%22key%22%3A%22" + isbn + "%22%2C%22pagesize%22%3A%2220%22%2C%22status%22%3A%221%22%2C%22pagenum%22%3A%221%22%2C%22order%22%3A%22100%22%2C%22area%22%3A%221001000000%22%2C%22select%22%3A%220%22%2C%22isFuzzy%22%3A%220%22%7D&type=2";

                    //string html2 = PostUrl(url, postdata2);
                    //string count = Regex.Match(html2, @"""recordCount"":([\s\S]*?),").Groups[1].Value;
                    //string isFuzzy = Regex.Match(html2, @"""isFuzzy"":([\s\S]*?),").Groups[1].Value;
                    //Thread.Sleep(1000);

                    //if (isFuzzy == "1")
                    //{
                    //    count = "无";
                    //}

                    string count = "0";

                    label1.Text = "正在查询：" + isbn;
                    string price = "无";
                    if (prices.Count > 0)
                    {
                        price = prices[0].Groups[1].Value;
                    }
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(isbn);
                    lv1.SubItems.Add(price.Replace("\"", ""));
                    lv1.SubItems.Add(fee);
                    lv1.SubItems.Add(count);



                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;
                }
                label1.Text = ("查询结束");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 获取时间戳毫秒
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalMilliseconds);
            return a.ToString();
        }


        public void getcookies()
        {
            string cookies = method.getSetCookie("http://search.kongfz.com/product_result/?key=9787802501614&status=0&_stpmt=eyJzZWFyY2hfdHlwZSI6ImFjdGl2ZSJ9");
            string randomcodekey = Regex.Match(cookies, @"randomcodekey=([\s\S]*?);").Groups[1].Value;
            string randomcode = Regex.Match(cookies, @"randomcode=([\s\S]*?);").Groups[1].Value;
         
       

            cookie =  "randomcodekey=" +randomcodekey+ ";randomcode="+ randomcode+ ";randomcodesign=fJVv1m762rVRYl5awRz2U%2Fs7bUDFoabKHHZIo0uTiIgqnX5H3CuSGVadurKvAhwNWES5%2BJ3lN5K%2BRgl7itwtDw%3D%3D; shoppingCartSessionId=3fe92fe84a861dcf66c0b202bdb6a127; reciever_area=1006000000; __utma=82106124.573931208.1617870285.1617870285.1617870285.1; __utmz=82106124.1617870285.1.1.utmcsr=item.kongfz.com|utmccn=(referral)|utmcmd=referral|utmcct=/; kfz_uuid=730dfe90-8b2c-4912-96e4-07ef46916144; PHPSESSID=3jeoib1pm5ek922ct7oggcbr16; acw_tc=276077cc16234095012965126e357c05cd481f38eed8e36489f8f6f4a87691; kfz_trace=730dfe90-8b2c-4912-96e4-07ef46916144|0|49218c7e6eb9255e|; Hm_lvt_bca7840de7b518b3c5e6c6d73ca2662c=1622700438,1622771855,1623032930,1623409512; Hm_lvt_33be6c04e0febc7531a1315c9594b136=1622700438,1622771855,1623032930,1623409512; TY_SESSION_ID=5db9fd1b-ce6f-4778-b8b1-9305dd087d70; kfz-tid=12635a593fdb5cf6dfe8ad947736512b; Hm_lpvt_33be6c04e0febc7531a1315c9594b136=1623410175; Hm_lpvt_bca7840de7b518b3c5e6c6d73ca2662c=1623410175";
           // MessageBox.Show(cookie);
        }


        public static string cookie = "";



        private void button6_Click(object sender, EventArgs e)
        {
            //getcookies();
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run_xcx);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
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
        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData)
        {
            try
            {
                string COOKIE = "Hm_lvt_33be6c04e0febc7531a1315c9594b136=1681722675; Hm_lvt_bca7840de7b518b3c5e6c6d73ca2662c=1681722675; kfz_trace=51AF83E2DF004296B46750EE4142DE68|16134930|25d779d6950d2456|102002001000; kfz_uuid=51AF83E2DF004296B46750EE4142DE68; shoppingCartSessionId=d59bcc36a03c4e4e6fd4d7eb8eed4fa4; utm_source=102002001000; mpDialog=true; PHPSESSID=40rov8apgcfrnbitaodb8lg56jf42s01; acw_tc=2760828d16817226717025190ed5e3f83b2de9217167a48faff1b35cc8d629";
                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //request.Proxy = null;//防止代理抓包
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("X-Tingyun-Id:58rG1RHpflE;c=2;r=1635436235;u=bc79049057b4b68538bbcf9565bb2afb::BD4E4C616020FB61");
                headers.Add("ssid:1689757110000231373");
                headers.Add("refUrl:KFZDynamicHomePageViewController");


                headers.Add("uuid:51AF83E2DF004296B46750EE4142DE68");
                headers.Add("accessToken:6aa5f840-c7ed-44b1-bed1-a920a90c0f3e");
                headers.Add("access-token:6aa5f840-c7ed-44b1-bed1-a920a90c0f3e");
                headers.Add("token:6aa5f840-c7ed-44b1-bed1-a920a90c0f3e");
           



                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                request.ContentType = "application/x-www-form-urlencoded";
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                //request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "IOS_KFZ_COM_3.9.2_iPhone 7 Plus_13.6.1 #App Store,8DF24B435A97462BBF3BC977E86CE5FB";
                request.Headers.Add("Cookie", COOKIE);

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

        private void button2_Click(object sender, EventArgs e)
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

        private void 孔夫子旧书网比价_Load(object sender, EventArgs e)
        {
          
        }

        private void 孔夫子旧书网比价_FormClosing(object sender, FormClosingEventArgs e)
        {

            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        
    }
}
