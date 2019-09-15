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

namespace main._2019_9
{
    public partial class 企查查 : Form
    {
        public 企查查()
        {
            InitializeComponent();
        }
        public string GetUrl(string url)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);  //创建一个链接

            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.132 Safari/537.36";

            request.Headers.Add("Cookie", "");
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

            string content = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return content;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        #region  中间量获取
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
                    string url = "https://www.qichacha.com/search?key=" + text[a];
                    string html = GetUrl(url);
                    Match key = Regex.Match(html, @"data-keyno=""([\s\S]*?)""");
                    if (html.Contains("我不甘心"))
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      
                        lv1.SubItems.Add(text[a]);
                        lv1.SubItems.Add("未查询到企业名");
                    }

                    else if(key.Groups[1].Value=="")
                    {
                        MessageBox.Show("需要验证");
                    }
                    else
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      
                        lv1.SubItems.Add(text[a]);
                        lv1.SubItems.Add(key.Groups[1].Value);

                    }

                    if (listView1.Items.Count > 2)
                    {
                        listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                    }
                    Thread.Sleep(300);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion


        #region  基本信息获取
        public void run1()

        {


            try
            {
                StreamReader sr = new StreamReader(textBox1.Text, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int a = 0; a < text.Length - 1; a++)
                {
                    string url = "https://www.qichacha.com/firm_" + text[a] + ".html";
                    string html = GetUrl(url);
                    Match a1 = Regex.Match(html, @"<h1>([\s\S]*?)</h1>");
                    Match a2 = Regex.Match(html, @"组织机构代码</td>([\s\S]*?)</td>");
                    Match a3 = Regex.Match(html, @"成立日期</td>([\s\S]*?)</td>");
                    Match a4 = Regex.Match(html, @"<h2 class=""seo font-20"">([\s\S]*?)</h2>");
                    Match a5 = Regex.Match(html, @"企业类型</td>([\s\S]*?)</td>");
                    Match a6 = Regex.Match(html, @"所属行业</td>([\s\S]*?)</td>");
                    Match a7 = Regex.Match(html, @"注册资本 </td>([\s\S]*?)</td>");
                    Match a8 = Regex.Match(html, @"登记机关</td>([\s\S]*?)</td>");
                    if (a1.Groups[1].Value=="")
                    {
                        MessageBox.Show("需要验证");
                    }

                 
                    else
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      
                        lv1.SubItems.Add(Regex.Replace(a1.Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(a2.Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(a3.Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(a5.Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(a6.Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(a7.Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(a8.Groups[1].Value, "<[^>]+>", ""));

                    }

                    if (listView1.Items.Count > 2)
                    {
                        listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                    }
                    Thread.Sleep(300);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run1));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void 企查查_Load(object sender, EventArgs e)
        {

        }
    }
}
