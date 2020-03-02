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
using MySql.Data.MySqlClient;

namespace 乐天网店
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        public static string cookie = "";
        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Post";
           // request.ContentType = "application/x-www-form-urlencoded";
            //添加头部
            //WebHeaderCollection headers = request.Headers;
            //headers.Add("appid:orders");
            //headers.Add("x-nike-visitid:5");
            //headers.Add("x-nike-visitorid:d03393ee-e42c-463e-9235-3ca0491475b4");
            //添加头部
            request.ContentType = "application/json";
            request.ContentLength = postData.Length;
            request.AllowAutoRedirect = false;
            request.KeepAlive = true;

            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
            request.Headers.Add("Cookie", COOKIE);
            //request.Headers.Add("origin","https://www.nike.com");
            request.Referer = "https://www.nike.com/orders/gift-card-lookup";
            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(postData);
            sw.Flush();

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
            response.GetResponseHeader("Set-Cookie");
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

            string html = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return html;

        }

        #endregion

        public void run()

        {
            string[] array = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < array.Length; i++)
            {
                try
                {

                    string html = method.GetUrlWithCookie(array[i], cookie, "utf-8");
                        Match post = Regex.Match(html, @"var prd =([\s\S]*?)};");
                        Match title = Regex.Match(html, @"productName"" content=""([\s\S]*?)""");
                        string postdata = post.Groups[1].Value.Trim() + "}";

                    string strhtml = PostUrl("http://chn.lottedfs.cn/kr/product/getPrdDtlPromInfoAjax?returnUrl=productNew/common/fragments/prdPriceBenefit", postdata, cookie, "utf-8");
                    Match zhekou = Regex.Match(strhtml, @"<td class=""fcf1"">([\s\S]*?)&");
                    Match jifen = Regex.Match(strhtml, @"<div class=""priceArea""><em>最多</em> <span>([\s\S]*?)</span>");
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString());
                    lv1.SubItems.Add(array[i]);
                    lv1.SubItems.Add(zhekou.Groups[1].Value);
                    lv1.SubItems.Add(jifen.Groups[1].Value);
                    lv1.SubItems.Add(title.Groups[1].Value);



                    if (radioButton1.Checked == true)
                    {
                        if (Convert.ToDecimal(zhekou.Groups[1].Value) <= Convert.ToDecimal(textBox5.Text))

                        {
                            MessageBox.Show("网页值出现变化！");
                            textBox4.Text += "出现变化的网址：" + title.Groups[1].Value + "\r\n";

                            FileStream fs1 = new FileStream(path + "config.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                            StreamWriter sw = new StreamWriter(fs1);
                            sw.WriteLine(listView1.Items[i].SubItems[1].Text + title.Groups[1].Value);
                            sw.Close();
                            fs1.Close();

                        }
                    }
                    else if (radioButton2.Checked == true)
                    {

                        if (Convert.ToDecimal(jifen.Groups[1].Value.Replace("%", "")) <= Convert.ToDecimal(textBox6.Text))

                        {
                            MessageBox.Show("网页值出现变化！");
                            textBox4.Text += "出现变化的网址：" + title.Groups[1].Value + "\r\n";
                            FileStream fs1 = new FileStream(path + "config.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                            StreamWriter sw = new StreamWriter(fs1);
                            sw.WriteLine(listView1.Items[i].SubItems[1].Text + title.Groups[1].Value);
                            sw.Close();
                            fs1.Close();

                        }

                    }


                    Thread.Sleep(1000);
                }

                catch
                {
                   
                   continue;
                }


            }

           

          

        }
        private void button1_Click(object sender, EventArgs e)
        {

            //string[] array = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            //for (int i = 0; i < array.Length; i++)
            //{

            //    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString());
            //    lv1.SubItems.Add(array[i]);
            //    lv1.SubItems.Add("0");
            //    lv1.SubItems.Add("0");
            //}


            textBox4.Text = "监控已开启";
            timer1.Start();

            
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //webBrowser1.ScriptErrorsSuppressed = true;
            method.SetWebBrowserFeatures(method.IeVersion.IE8);
            webBrowser1.Url = new Uri("https://chn.lps.lottedfs.cn/kr/member/login");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            cookie = method.GetCookies("http://chn.lottedfs.cn/kr/product/productDetail?prdNo=10001805265&prdOptNo=10001805265");
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            webBrowser1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamReader streamReader = new StreamReader(this.openFileDialog1.FileName, Encoding.Default);
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    textBox1.Text += array[i]+"\r\n";
                  
                }
                   
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";

        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
    }
}
