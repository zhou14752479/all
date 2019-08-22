using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
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

        #region GET使用代理IP请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url,string COOKIE, string ip,int port)
        {
            try
            {
               
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                WebProxy proxy = new WebProxy(ip,port);
                request.Proxy = proxy;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

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

        string IP = "";
        int PORT = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
          string html=  method.GetUrl(textBox2.Text,"utf-8");
            string[] text = html.Split(new string[] { ":" }, StringSplitOptions.None);
            IP = text[0];
            PORT = Convert.ToInt32(text[1]);
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
            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入文本");
                return;
            }
            COOKIE = textBox3.Text;
            try
            {
                string[] array = this.ReadText();
                foreach (string values in array)
                {
                    string[] keyword = values.Split(new string[] { "&" }, StringSplitOptions.None);

                    string url = "http://mobile.yangkeduo.com/search_result.html?search_key="+keyword[1].ToString()+"&sort_type=_sales";
                  
                    string html = GetUrl(url, COOKIE, this.IP, this.PORT);
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

                        string strhtml = GetUrl(URL, COOKIE, this.IP,this.PORT);

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
            string html = method.GetUrl(textBox2.Text, "utf-8");
            string[] text = html.Split(new string[] { ":" }, StringSplitOptions.None);
            if (text.Length > 1)
            {
                IP = text[0];
                PORT = Convert.ToInt32(text[1]);
                timer1.Start();
                timer1.Interval = (Convert.ToInt32(textBox5.Text)) * 1000;
                Thread thread = new Thread(new ThreadStart(this.run));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
            }
            else
            {
                MessageBox.Show("请输入正确的代理IP地址");
                return;
            }

            

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
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
            

        }

        private void 拼多多搜索_Load(object sender, EventArgs e)
        {
            
        }

        private void button7_Click(object sender, EventArgs e)
        {

            //bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            //if (flag)
            //{
            //    this.textBox4.Text = this.openFileDialog1.FileName;
            //}

            textBox3.Text = method.getsufeiUrl("http://tool.sufeinet.com/HttpHelper.aspx?type=url&url=http://mobile.yangkeduo.com/goods.html?goods_id=735873502");
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            webBrowser web = new webBrowser("http://mobile.yangkeduo.com/");
            web.Show();
        }

        private void SplitContainer1_Panel1_MouseEnter(object sender, EventArgs e)
        {
            //textBox3.Text = webBrowser.cookie;
        }
    }
}
