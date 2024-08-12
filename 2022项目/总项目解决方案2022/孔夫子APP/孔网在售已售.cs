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
    public partial class 孔网在售已售 : Form
    {
        public 孔网在售已售()
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

        private void button6_Click(object sender, EventArgs e)
        {
           

           
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


        string COOKIE = "Hm_lvt_33be6c04e0febc7531a1315c9594b136=1713963270; shoppingCartSessionId=bf96c8533a0bdd06e8e47556884cf211; Hm_lvt_bca7840de7b518b3c5e6c6d73ca2662c=1713963270,1714048900; reciever_area=1001000000; kfz_uuid=0845d93e-fe4a-4883-bf9c-3e923afb5dd3; _c_WBKFRo=U3qserpq0pualfM8Oe81fuT0H32buIxGel5Yg9Vm; acw_tc=276077bf17229386742377400e8f89a6a03f816c83d681ba3faed6b62e6d59; PHPSESSID=fea439d47d58fd39dc367088c475125bb2212b1a; kfz_trace=0845d93e-fe4a-4883-bf9c-3e923afb5dd3|16134930|971e72b21a34635c|-";
        
        public string getsaled(string isbn)
        {
            try
            {
                string url = "https://search.kongfz.com/pc-gw/search-web/client/pc/product/keyword/list?dataType=1&keyword=" + isbn + "&userArea=1001000000";


                string html = method.GetUrlWithCookie(url, COOKIE, "utf-8");

                string saled = Regex.Match(html, @"""totalFoundText"":""([\s\S]*?)""").Groups[1].Value;
                return saled;
            }
            catch (Exception)
            {

                return "";
            }
        }
       

        // 用户名+密码授权
        string username = "17606117606";
        string password = "0JuYrGtx";

        #region GET请求带COOKIE
        public  string GetUrlWithCookie(string Url, string COOKIE, string charset )
        {
            string result;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36";
                request.Referer = Url;
               
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
               
                //WebProxy proxy = new WebProxy();
                //proxy.Address = new Uri(String.Format("http://{0}:{1}", tunnelhost, tunnelport));
                //proxy.Credentials = new NetworkCredential(username, password);
                //// 代理服务器

                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
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

        string tunnelhost;

        int tunnelport;
        public void getip()
        {

            string url = "http://api.91http.com/v1/get-ip?trade_no=A971570130670&secret=wkAm2ySvZg6lssly&num=1&protocol=1&format=text&sep=1";
            string html = method.GetUrl(url,"utf-8");
           
            string[] text = html.Split(new string[] { ":" }, StringSplitOptions.None);
           if(text.Length>1)
            {
                tunnelhost = text[0];
                tunnelport = Convert.ToInt32(text[1]);
            }
        }
        #endregion
        public void run()
        {
            getip();
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

                        string isbn = text[i].ToString();

                        if (isbn.Trim() == "")
                            break;
                        string url = "https://search.kongfz.com/pc-gw/search-web/client/pc/product/keyword/list?keyword="+isbn+"&dataType=0&sortType=7&page=1&actionPath=sortType&userArea=1001000000";

                      
                        string html = GetUrlWithCookie(url, COOKIE, "utf-8");

                        
                         MatchCollection prices = Regex.Matches(html, @"""priceText"":""([\s\S]*?)""");

                        MatchCollection shippingFee = Regex.Matches(html, @"""shippingFeeText"":""([\s\S]*?)""");

                        string price1 = prices.Count > 0 ? prices[0].Groups[1].Value.ToString() : "无";
                        string shippingFee1 = prices.Count > 0 ? shippingFee[0].Groups[1].Value.ToString() : "无";
                        string price3 = prices.Count > 0 ? prices[2].Groups[1].Value.ToString() : "无";
                        string shippingFee3 = prices.Count > 0 ? shippingFee[2].Groups[1].Value.ToString() : "无";
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(isbn);
                        lv1.SubItems.Add(price1);
                        lv1.SubItems.Add(shippingFee1);
                        lv1.SubItems.Add(price3);
                        lv1.SubItems.Add(shippingFee3);


                      // string saled= getsaled(isbn);
                       // lv1.SubItems.Add(saled);



                        if (prices.Count == 0)
                        {
                            Thread.Sleep(5000);
                           
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
                        Random rand = new Random();
                        Thread.Sleep(rand.Next(800, 1200));

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        continue;

                    }
                }
                label1.Text = ("查询结束");
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.ToString());
            }
        }
        Thread thread;
        bool zanting = true;
        bool status = true;

        private void 孔网在售已售_FormClosing(object sender, FormClosingEventArgs e)
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


      

        private void 孔网在售已售_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            // webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("https://m.kongfz.com/dist/search/result.86ffe85cb9660dddaf49.bundle.js");
        }
    }
}
