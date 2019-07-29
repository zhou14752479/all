using System;
using System.Collections;
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

namespace main._2019_6
{
    public partial class jd1 : Form
    {
        public jd1()
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
        public string GetUrl(string url)
        {
            Uri uri = new Uri(url);
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);
            myReq.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
            myReq.Accept = "*/*";
            myReq.KeepAlive = true;
            myReq.Referer = url;
            myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
            HttpWebResponse result = (HttpWebResponse)myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8"));
            string strHTML = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            return strHTML;
        }
        ArrayList finishes = new ArrayList();

        #region  手机端
        public void run()
        {
            try
            {
                StreamReader sr = new StreamReader(textBox1.Text, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int a = 0; a < text.Length - 1; a++)
                {
                    if (!finishes.Contains(text[a]))
                    {
                        finishes.Add(text[a]);
                        string url = text[a];

                        
                        Match shopID = Regex.Match(url, @"index-([\s\S]*?)\.");
                        string html = GetUrl("https://shop.m.jd.com/?shopId="+ shopID.Groups[1].Value);
                        string ZURL = "https://wqsou.jd.com/search/searchjson?datatype=1&page=1&pagesize=100&merge_sku=yes&qp_disable=yes&key=ids%2C%2C"+shopID.Groups[1].Value+"&source=omz&_=1564383664458&sceneval=2&g_login_type=1&callback=jsonpCBKV&g_ty=ls";
                        string strhtml = GetUrl(ZURL);

                        Match name = Regex.Match(html, @"<title>([\s\S]*?)</title>");
                        Match count = Regex.Match(strhtml, @"OrgSkuCount"": ""([\s\S]*?)""");

                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      

                            lv1.SubItems.Add(name.Groups[1].Value.Trim());
                            lv1.SubItems.Add(count.Groups[1].Value);
                            lv1.SubItems.Add(text[a]);
                        

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {if (textBox1.Text == "")
            {
                MessageBox.Show("请导入店铺链接");
                return;
            }
            Thread thread = new Thread(new ThreadStart(run));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void jd1_Load(object sender, EventArgs e)
        {
            
        }

      
    }
}
