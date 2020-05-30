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

namespace 耐克查询
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
          

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
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                // request.ContentType = "application/x-www-form-urlencoded";
                request.ContentType = "application/json";
                request.ContentLength = postData.Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("appid:orders");
                headers.Add("x-nike-visitid:5");
                headers.Add("x-nike-visitorid:d03393ee-e42c-463e-9235-3ca0491475b4");
                //添加头部
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("origin", "https://www.nike.com");
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
            catch (Exception ex)
            {

              return  ex.ToString();

            }

        }

        #endregion
        bool zanting = true;
        public void run()
        {
            try
            {
                // string[] array = textBox1.Text.Split(new string[] { " \r\n " }, StringSplitOptions.None);

                MatchCollection array = Regex.Matches(textBox1.Text, @"60\d.*");
               
                for (int i = 0; i < array.Count; i++)
                {
                   
                    string[] value = array[i].Groups[0].Value.Replace("  "," ").Split(new string[] { " " }, StringSplitOptions.None);
                    if (value.Length !=0)
                    {
                        string url = "https://api.nike.com/payment/giftcard_balance/v1/";
                        string postdata = "{\"accountNumber\":\"" + value[0].Trim() + "\",\"currency\":\"USD\",\"pin\":\"" + value[1].Trim() + "\"}";
                        string cookie = "";
                        
                        string html = PostUrl(url, postdata, cookie, "utf-8");
                        Match key = Regex.Match(html, @"""balance"":([\s\S]*?)\}");
                        if (key.Groups[1].Value=="")
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(value[0]);
                            lv1.SubItems.Add(value[1]);
                            lv1.SubItems.Add("inv");
                        }
                        else
                        {
                            
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(value[0]);
                            lv1.SubItems.Add(value[1]);
                            lv1.SubItems.Add(key.Groups[1].Value.Replace(",\"resourceType\":\"payment/giftcard_balance\"", ""));
                        }
                      

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;


            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
               zanting = false;
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            zanting = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamReader streamReader = new StreamReader(this.openFileDialog1.FileName, Encoding.Default);
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    textBox1.Text += array[i] + "\r\n";

                }

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            textBox1.Text = "";
            listView1.Items.Clear();

        }
    }
}
