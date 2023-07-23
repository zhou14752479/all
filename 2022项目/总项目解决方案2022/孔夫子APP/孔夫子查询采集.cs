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

namespace 孔夫子APP
{
    public partial class 孔夫子查询采集 : Form
    {
        public 孔夫子查询采集()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
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
        public static string PostUrlDefault(string url, string postData)
        {
            try
            {
                string COOKIE = "PHPSESSID=746thl2h6l9gbse9ioehqrp0b6h3hap0;";
                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //request.Proxy = null;//防止代理抓包
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("X-Tingyun-Id:lLmhN035-8Y;c=2;r=1510130765;u=b62bf55b6da98676c7af69e7063790e6::85C1D76CAE16FFDC");
                headers.Add("ssid:1681723846000257004");
                headers.Add("refUrl:KFZDynamicHomePageViewController");


                headers.Add("uuid:51AF83E2DF004296B46750EE4142DE68");
                headers.Add("accessToken:976e5c7a-ba8b-4554-b8fb-d96e662c835a");
                headers.Add("access-token:976e5c7a-ba8b-4554-b8fb-d96e662c835a");
                headers.Add("token:976e5c7a-ba8b-4554-b8fb-d96e662c835a");
                headers.Add("ssid: 1689944873000198921");



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
        public void run()
        {

            try
            {




                StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {

                    try
                    {
                        string q1 = "";
                        string isbn = text[i].ToString();

                        if (isbn.Trim() == "")
                            break;
                        string url = "https://app.kongfz.com/invokeSearch/app/product/productSearchV2";

                        // string jiagepaixu = "100"; //总价从低达高
                        string jiagepaixu = "1";  //价格从低到高
                       string postdata = "_stpmt=ewoKfQ%3D%3D&params=%7B%22key%22%3A%22" + isbn + "%22%2C%22pagesize%22%3A%2220%22%2C%22status%22%3A%220%22%2C%22pagenum%22%3A%221%22%2C%22order%22%3A%22"+jiagepaixu+"%22%2C%22area%22%3A%221001000000%22%2C%22select%22%3A%220%22%2C%22quality%22%3A%22" + q1 + "%22%2C%22isFuzzy%22%3A%220%22%7D&type=2";
                        //string postdata = "page=1&params=%7B%22key%22%3A%229787122115300%22%2C%22pagesize%22%3A%2220%22%2C%22status%22%3A%220%22%2C%22order%22%3A%220%22%2C%22area%22%3A%22%22%2C%22select%22%3A%220%22%2C%22quality%22%3A%22%22%2C%22isFuzzy%22%3A%220%22%7D&type=2";
                      
                        string html = PostUrlDefault(url, postdata);
                        html = method.Unicode2String(html);
                        MatchCollection itemIds = Regex.Matches(html, @"""itemId"":([\s\S]*?),");
                       // MessageBox.Show(html);

                        MatchCollection itemNames = Regex.Matches(html, @"""itemName"":""([\s\S]*?)""");
                        MatchCollection userIds = Regex.Matches(html, @"""userId"":([\s\S]*?),");
                        MatchCollection shopnames = Regex.Matches(html, @"""shopName"":""([\s\S]*?)""");
                        MatchCollection prices = Regex.Matches(html, @"""price"":""([\s\S]*?)""");

                        MatchCollection qualitys = Regex.Matches(html, @"""quality"":""([\s\S]*?)""");
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   







                        lv1.SubItems.Add(isbn);
                        lv1.SubItems.Add(itemNames[0].Groups[1].Value);
                        lv1.SubItems.Add(prices[0].Groups[1].Value);
                        lv1.SubItems.Add(qualitys[0].Groups[1].Value);

                        lv1.SubItems.Add(method.Unicode2String(shopnames[0].Groups[1].Value));





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
                        label2.Text = "正在查询：" + isbn;
                        Random rand = new Random();
                        Thread.Sleep(rand.Next(1000,3000));

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        continue;

                    }
                }
                label2.Text = ("查询结束");
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.ToString());
            }
        }

        public string getShippingFee2(string sb)
        {
            string url = "https://shop.kongfz.com/book/shopsearch/getShippingFee?callback=jQuery111202310343450011496_1652665962548&params=%7B%22params%22%3A%5B" + sb + "%5D%2C%22area%22%3A%221006000000%22%7D&_=1652665962549";
            string html = method.GetUrl(url, "utf-8");

            return html;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            //if (!html.Contains(@"r0LSU"))
            //{
            //    System.Diagnostics.Process.GetCurrentProcess().Kill();
            //    return;
            //}

            if (!html.Contains(@"WPFko"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion

            //cookie = method.GetCookies("https://search.kongfz.com/product_result/?key=9787101151824&status=0&_stpmt=eyJzZWFyY2hfdHlwZSI6ImFjdGl2ZSJ9&order=100&ajaxdata=4");
            //textBox1.Text = cookie;

            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入ISBN");
                return;
            }
            status = true;

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
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

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 孔夫子查询采集_FormClosing(object sender, FormClosingEventArgs e)
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

        public string cookie = "";
      

        private void 孔夫子查询采集_Load(object sender, EventArgs e)
        {
            //method.SetFeatures(11000);
            //webBrowser1.ScriptErrorsSuppressed = true;
            //webBrowser1.Navigate("https://search.kongfz.com/product_result/?key=9787101151824&status=0&_stpmt=eyJzZWFyY2hfdHlwZSI6ImFjdGl2ZSJ9&order=100&ajaxdata=4");
            if(DateTime.Now> Convert.ToDateTime("2024-07-10"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }
            #region 通用检测


            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"WPFko"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion
        }
    }
}
