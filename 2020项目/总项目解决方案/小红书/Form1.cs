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

namespace 小红书
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            try
            {
                string COOKIE = "Hm_lvt_c58e42b54acb40ab70d48af7b1ce0d6a=1563157838,1563157854; ASPSESSIONIDSCRDCDQB=NBHMLEHCKHKNFDMPGPGKFPNP; fikker-vMnk-0qnk=nyMU6OJy8iTIpYhmd5bST9RwBSD9TGV1; fikker-vMnk-0qnk=nyMU6OJy8iTIpYhmd5bST9RwBSD9TGV1; fikker-0epN-KaRa=dGd9KSVkZ7VSnSZSrIPidYtMDe0UVLOA; Hm_lvt_a2f6ee5c5c2efc17b10dc0659462df30=1563161043,1563259279,1563259736,1563259814; Hm_lpvt_a2f6ee5c5c2efc17b10dc0659462df30=1563262152";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";
                request.Timeout = 10000;
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = true;
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
        bool zanting = true;
        #region 小红书
        public void run()
        {
            try

            {

                string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                    string url = text[i];
                   
                    string html = method.GetUrl(url, "utf-8");
                    if (html.Contains("小红书登录"))
                    {
                        MessageBox.Show("请登录");
                        html = GetUrl(url, "utf-8");
                    }
                       

                    Match a1 = Regex.Match(html, @"{""nickname"":""([\s\S]*?)""");
                    Match a2 = Regex.Match(html, @"发布于([\s\S]*?)<");
                    
                    Match a3 = Regex.Match(html, @"},""likes"":([\s\S]*?),");
                    Match a4 = Regex.Match(html, @",""comments"":([\s\S]*?),");
                    Match a5 = Regex.Match(html, @"""collects"":([\s\S]*?),");
                  

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(url);
                    lv1.SubItems.Add(a1.Groups[1].Value);
                    lv1.SubItems.Add(a2.Groups[1].Value);
                    lv1.SubItems.Add(a3.Groups[1].Value);
                    lv1.SubItems.Add(a4.Groups[1].Value);
                    lv1.SubItems.Add(a5.Groups[1].Value);
                  


                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
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
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
