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
    public partial class 孔网导入isbn : Form
    {
        public 孔网导入isbn()
        {
            InitializeComponent();
        }


        #region GET请求带COOKIE
        public static string GetUrlWithCookie(string Url, string COOKIE, string charset)
        {
            string result;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36";
                request.Referer = Url;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("version:TYC-XCX-WX");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
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
                result = ex.ToString();

            }
            return result;
        }
        #endregion

        Thread thread;
        bool zanting = true;
        bool status = true;
        private void button6_Click(object sender, EventArgs e)
        {


            if (DateTime.Now > Convert.ToDateTime("2025-05-20"))
            {
                return;
            }
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入店铺网址");
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

        static string GetRandomUserAgent(List<string> userAgents)
        {
            Random random = new Random();
            int index = random.Next(0, userAgents.Count);
            return userAgents[index];
        }

        List<string> userAgents = new List<string>
        {
           "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/116.0",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/16.5 Safari/605.1.15",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36 Edg/115.0.1901.203",
            "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36",
            "Mozilla/5.0 (X11; Linux x86_64; rv:109.0) Gecko/20100101 Firefox/116.0",
            "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:108.0) Gecko/20100101 Firefox/115.0",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_16) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:109.0) Gecko/20100101 Firefox/116.0",
            "Mozilla/5.0 (Windows NT 6.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_10_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Windows NT 5.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36",
            "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:109.0) Gecko/20100101 Firefox/116.0",
            "Mozilla/5.0 (X11; Fedora; Linux x86_64; rv:109.0) Gecko/20100101 Firefox/116.0",
            "Mozilla/5.0 (X11; Debian; Linux x86_64; rv:109.0) Gecko/20100101 Firefox/116.0",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 16_5 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/16.5 Mobile/15E148 Safari/604.1",
            "Mozilla/5.0 (iPad; CPU OS 16_5 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/16.5 Mobile/15E148 Safari/604.1",
            "Mozilla/5.0 (iPod touch; CPU iPhone OS 16_5 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/16.5 Mobile/15E148 Safari/604.1",
            "Mozilla/5.0 (Android 13; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 12; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 11; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 10; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 9; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 8.1.0; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 8.0.0; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 7.1.2; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 7.1.1; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 7.0; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 6.0.1; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 6.0; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 5.1.1; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 5.0.2; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 5.0; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 4.4.4; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 4.4.2; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 4.4; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 4.3; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 4.2.2; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 4.2.1; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 4.1.2; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 4.1.1; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 4.0.4; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 4.0.3; Mobile; rv:109.0) Gecko/109.0 Firefox/116.0",
            "Mozilla/5.0 (Android 13; Mobile) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (Android 12; Mobile) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (Android 11; Mobile) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (Android 10; Mobile) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (Android 9; Mobile) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (Android 8.1.0; Mobile) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (Android 8.0.0; Mobile) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (Android 7.1.2; Mobile) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (Android 7.1.1; Mobile) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (Android 7.0; Mobile) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (Android 6.0.1; Mobile) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (Android 6.0; Mobile) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (Android 5.1.1; Mobile) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (Android 5.0.2; Mobile) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (Android 5.0; Mobile) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (Android 4.4.4; Mobile) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (Android 4.4.2; Mobile) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Mobile Safari/537.36",
            "Mozilla/5.0 (Android 4.4; Mobile) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Mobile Safari/537.36"
        };

        #region GET请求
       string GetUrl(string Url, string charset)
        {
            string COOKIE = "";
            string result;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Proxy = null;
                request.AllowAutoRedirect = true;
                string randomUserAgent = GetRandomUserAgent(userAgents);
                request.UserAgent = randomUserAgent;
                request.Referer = Url;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("version:TYC-XCX-WX");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
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
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        #endregion

        
        public void run()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入店铺网址txt");
                return;
            }
            string cookie = textBox2.Text.Trim();

            StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == "")
                {
                    continue;
                }


                try
                {


                    

                    string searchUrl = "https://item.kongfz.com/ugc/book/checkIsbnIsExist";

                    string ahtml = method.PostUrlDefault(searchUrl, "isbn=" + text[i].Trim(), cookie);

                    string mid = Regex.Match(ahtml, @"""data"":""([\s\S]*?)""").Groups[1].Value;

                  

                    if (mid=="")
                    {
                       Thread.Sleep(1000);
                        label2.Text = "未查询到相关图书" + text[i];
                        continue;
                    }

                    label2.Text = "正在抓取第"+i+"个：" + text[i];
                    string url = "https://item.kongfz.com/book/"+ mid + ".html";
                    
                    string html = GetUrlWithCookie(url, cookie, "utf-8");
                   
                    string coverImage = Regex.Match(html, @"<div class=""detail-img"">([\s\S]*?)<a href=""([\s\S]*?)""").Groups[2].Value;
                 
                    string price = Regex.Match(html, @"""price"":""([\s\S]*?)""").Groups[1].Value;
                    string author = Regex.Match(html, @"""author"":""([\s\S]*?)""").Groups[1].Value;
                    string bookName = Regex.Match(html, @"""bookName"":""([\s\S]*?)""").Groups[1].Value;
                    string pubDate = Regex.Match(html, @"""pubDate"":""([\s\S]*?)""").Groups[1].Value;
                    string press = Regex.Match(html, @"""press"":""([\s\S]*?)""").Groups[1].Value;
                   
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(coverImage.Replace("_water", ""));
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add(text[i]);
                    lv1.SubItems.Add(method.Unicode2String(bookName));
                    lv1.SubItems.Add(price);
                    lv1.SubItems.Add(method.Unicode2String(author));
                    lv1.SubItems.Add(method.Unicode2String(press));
                    lv1.SubItems.Add(pubDate);



                    if (listView1.Items.Count > 10)
                    {
                        this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                    }
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;

                    Thread.Sleep(1000);

                }


                catch (Exception ex)
                {

                   continue;
                }




            }


        }

     

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void 孔网导入isbn_Load(object sender, EventArgs e)
        {

        }

        private void 孔网导入isbn_FormClosing(object sender, FormClosingEventArgs e)
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
