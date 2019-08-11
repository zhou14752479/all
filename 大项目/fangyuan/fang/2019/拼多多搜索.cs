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
using System.Xml;

namespace fang._2019
{
    public partial class 拼多多搜索 : Form
    {
        public 拼多多搜索()
        {
            InitializeComponent();
        }
        public static string COOKIE = "";

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, String COOKIE, string charset)
        {
            try
            {


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.81 Safari/537.36";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("AccessToken", "NJOZ36OLGEFYLG5MNP7MNPIIIAHVT6KMCGCX7I6PAUCGXBE4HYZA102118b");
                request.KeepAlive = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion
        private void timer1_Tick(object sender, EventArgs e)
        {

            //textBox3.Text = webBrowser.cookie;
            //COOKIE = textBox3.Text;
        }
        private void splitContainer1_Panel1_MouseEnter(object sender, EventArgs e)
        {

        }
        bool zanting = true;

        public string[] ReadText()
        {
            string currentDirectory = Environment.CurrentDirectory;
            StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
            string text = streamReader.ReadToEnd();
            return text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        }
        public void run()
        {
            COOKIE = textBox3.Text;
            try
            {
                string[] array = this.ReadText();
                foreach (string values  in array)
                {
                    string[] keyword = values.Split(new string[] { "&" }, StringSplitOptions.None);

                    string url = "http://mobile.yangkeduo.com/search_result.html?search_key="+keyword[1].ToString()+"&sort_type=_sales";
                    string html = GetUrl(url,COOKIE, "utf-8");
                    textBox3.Text = html;

                    if (html.Contains("暂无相关商品"))
                    {

                        Match sousuo = Regex.Match(html, @"“<!-- -->([\s\S]*?)<!-- -->");


                        ListViewItem listViewItem = this.listView2.Items.Add((listView2.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(keyword[1].ToString());
                        listViewItem.SubItems.Add(sousuo.Groups[1].Value.ToString());
                        if (this.listView2.Items.Count > 2)
                        {
                            this.listView2.EnsureVisible(this.listView2.Items.Count - 1);
                        }
                    }

                   else if(html.Contains("暂无搜索结果"))
                    { 
                        

                        ListViewItem listViewItem = this.listView2.Items.Add((listView2.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(keyword[1].ToString());
                        listViewItem.SubItems.Add("无搜索结果");
                        if (this.listView2.Items.Count > 2)
                        {
                            this.listView2.EnsureVisible(this.listView2.Items.Count - 1);
                        }
                    }

                    else
                    {
                        Match match = Regex.Match(html, @"""goodsID"":([\s\S]*?),");


                        string URL = "http://mobile.yangkeduo.com/goods.html?goods_id=" + match.Groups[1].Value.Trim();

                        string strhtml = GetUrl(URL, COOKIE, "utf-8");

                        Match counts = Regex.Match(strhtml, @"<span class=""g-sales regular-text"">([\s\S]*?)</span>");
                        Match price = Regex.Match(strhtml, @"minOnSaleGroupPrice"":""([\s\S]*?)""");
                        Match commentCount = Regex.Match(strhtml, @"commentNumText"":""([\s\S]*?)""");


                        Match name = Regex.Match(strhtml, @"mallName"":""([\s\S]*?)""");
                        Match shopcounts = Regex.Match(strhtml, @"已拼([\s\S]*?)<");

                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(keyword[1].ToString());
                        listViewItem.SubItems.Add(counts.Groups[1].Value.ToString().Replace("已拼", "").Trim());
                        listViewItem.SubItems.Add(price.Groups[1].Value.ToString());
                        listViewItem.SubItems.Add(commentCount.Groups[1].Value.ToString().Replace("商品评价","").Replace(")","").Replace("(",""));
                        listViewItem.SubItems.Add(URL);
                        listViewItem.SubItems.Add(name.Groups[1].Value.ToString());
                        listViewItem.SubItems.Add(shopcounts.Groups[1].Value.ToString().Replace(":", "").Trim());

                        if (this.listView1.Items.Count > 2)
                        {
                            this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
                        }

                        Application.DoEvents();
                        Thread.Sleep(Convert.ToInt32(100));

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                    }





                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(this.run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.zanting = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.zanting = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            //if (flag)
            //{
            //    this.textBox1.Text = this.openFileDialog1.FileName;
            //}
            //string url = "http://app.58.com/api/detail/zpfangchan/36149962795705?v=1&platform=android&version=7.0.1.0&sidDict=%7B%22PGTID%22%3A%22%22%2C%22GTID%22%3A%22101551665202271302713434986%22%7D&format=xml&localname=linyi&commondata=%7B%22cateid%22%3A%2238673%22%2C%22catename%22%3A%22%25e6%2588%25bf%25e5%259c%25b0%25e4%25ba%25a7%22%2C%22filterparams%22%3A%7B%7D%7D";
            //WebClient webClient = new WebClient();
            //webClient.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36";
            //webClient.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            //byte[] myDataBuffer = webClient.DownloadData(url);
            //string data = Encoding.UTF8.GetString(myDataBuffer);
            //XmlDocument xml = new XmlDocument();
            //xml.LoadXml(data);
            //XmlNode xn_tel_info = xml.SelectSingleNode("/root/result/userinfo_area/tel_info");
            //if (xn_tel_info != null)
            //{
            //string phone = xn_tel_info.Attributes["content"].Value;
            // MessageBox.Show(phone);
            // if (phone.Length > 0)
            //  phone = AES.Decrypt(phone, "crazycrazycrazy1");
            string phone = AES.Decrypt("63761C9101B9AC46FAC0A4BF75D29D09", "crazycrazycrazy1");
                MessageBox.Show(phone);
          //  }
            


        }

        private void 拼多多搜索_Load(object sender, EventArgs e)
        {
           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            webBrowser web = new webBrowser("http://mobile.yangkeduo.com/");
            web.Show();

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox4.Text = this.openFileDialog1.FileName;
            }
        }
    }
}
