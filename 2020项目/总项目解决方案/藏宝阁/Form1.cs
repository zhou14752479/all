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
using helper;

namespace 藏宝阁
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }


       public static  string COOKIE = "fingerprint=6ke7wjxyis0k4fqq; msg_box_flag=1; _ntes_nnid=49bc8878754882fe696204132932ebe7,1563757620289; _ntes_nuid=49bc8878754882fe696204132932ebe7; usertrack=ezq0J11HilIZSVxcILa3Ag==; UM_distinctid=16d48b104fb648-0b695a783e0c83-f353163-1fa400-16d48b104fcaf9; vinfo_n_f_l_n3=80b4785de51d55b3.1.0.1568882622014.0.1568882666358; __f_=1570753726419; sid=rcAEI_LGMVF91iIikfOM4X7SKisvxtbPKH6LZZtD; last_login_serverid=249; wallet_data=%7B%22is_locked%22%3A%20false%2C%20%22checking_balance%22%3A%200%2C%20%22balance%22%3A%200%2C%20%22free_balance%22%3A%200%7D";
        #region GET请求
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.108 Safari/537.36";
                request.Referer = "https://xy2.cbg.163.com/cgi-bin/equipquery.py?act=query&server_id=115&areaid=3&page=1&kind_id=45&query_order=selling_time+DESC";
                request.Headers.Add("Cookie", COOKIE);
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
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        ArrayList glists = new ArrayList();
        ArrayList wlists = new ArrayList();

        /// <summary>
        /// 公示期第一次获取
        /// </summary>
        public void gongshiFirst()
        {
            try
            {
                string url = "https://xy2.cbg.163.com/cgi-bin/equipquery.py?act=fair_show_list&server_id=249&areaid=4&page=1&kind_id=45&query_order=create_time+DESC";
                string html = GetUrl(url, "gb2312");
                MatchCollection links = Regex.Matches(html, @"<a class=""soldImg"" style=""text-decoration:none;"" href=""([\s\S]*?)""");
              
                for (int i = 0; i < links.Count; i++)
                {
                    glists.Add(links[i].Groups[1].Value);
                }
            }
            catch (Exception ex)
            {

                textBox1.Text = ex.ToString();
            }
        }




        /// <summary>
        /// 我要买第一次获取
        /// </summary>
        public void woyaomaiFirst()
        {
            try
            {
                string url = "https://xy2.cbg.163.com/cgi-bin/equipquery.py?act=query&server_id=249&areaid=4&page=1&kind_id=45&query_order=selling_time+DESC";
                string html = GetUrl(url, "gb2312");
                MatchCollection links = Regex.Matches(html, @"<a class=""soldImg"" style=""text-decoration:none;"" href=""([\s\S]*?)""");

                for (int i = 0; i < links.Count; i++)
                {
                    wlists.Add(links[i].Groups[1].Value);
                }
            }
            catch (Exception ex)
            {

                textBox1.Text = ex.ToString();
            }
        }






        /// <summary>
        /// 公示期
        /// </summary>
        public void gongshi()
        {
            try
            {
                string url = "https://xy2.cbg.163.com/cgi-bin/equipquery.py?act=fair_show_list&server_id=249&areaid=4&page=1&kind_id=45&query_order=create_time+DESC";
                string html = GetUrl(url, "gb2312");

                Match serverName = Regex.Match(html, @"""server_name"" : ""([\s\S]*?)""");
                MatchCollection links = Regex.Matches(html, @"<a class=""soldImg"" style=""text-decoration:none;"" href=""([\s\S]*?)""");
                MatchCollection prices = Regex.Matches(html, @"data_price=""([\s\S]*?)""");



                for (int i = 0; i < links.Count; i++)
                {
                    if (!glists.Contains(links[i].Groups[1].Value))
                    {
                        if (Convert.ToDecimal(textBox2.Text) < Convert.ToDecimal(prices[i + 1].Groups[1].Value) && Convert.ToDecimal(prices[i + 1].Groups[1].Value) < Convert.ToDecimal(textBox3.Text))
                        {
                            System.Media.SystemSounds.Asterisk.Play();
                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            listViewItem.SubItems.Add(serverName.Groups[1].Value);
                            listViewItem.SubItems.Add(prices[i + 1].Groups[1].Value);
                            listViewItem.SubItems.Add(links[i].Groups[1].Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                textBox1.Text = ex.ToString();
            }
        }

        /// <summary>
        /// 我要买
        /// </summary>
        public void woyaomai()
        {
            try
            {
                string url = "https://xy2.cbg.163.com/cgi-bin/equipquery.py?act=query&server_id=249&areaid=4&page=1&kind_id=45&query_order=create_time+DESC";
                string html = GetUrl(url, "gb2312");

                Match serverName = Regex.Match(html, @"""server_name"" : ""([\s\S]*?)""");
                MatchCollection links = Regex.Matches(html, @"<a class=""soldImg"" style=""text-decoration:none;"" href=""([\s\S]*?)""");
                MatchCollection prices = Regex.Matches(html, @"data_price=""([\s\S]*?)""");



                for (int i = 0; i < links.Count; i++)
                {
                    if (!wlists.Contains(links[i].Groups[1].Value))
                    {
                        if (Convert.ToDecimal(textBox2.Text) < Convert.ToDecimal(prices[i + 1].Groups[1].Value) && Convert.ToDecimal(prices[i + 1].Groups[1].Value) < Convert.ToDecimal(textBox3.Text))
                        {
                            System.Media.SystemSounds.Asterisk.Play();
                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            listViewItem.SubItems.Add(serverName.Groups[1].Value);
                            listViewItem.SubItems.Add(prices[i + 1].Groups[1].Value);
                            listViewItem.SubItems.Add(links[i].Groups[1].Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                textBox1.Text = ex.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                button1.Enabled = false;
                Thread thread = new Thread(new ThreadStart(gongshiFirst));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

                timer1.Start();
            }

            else if (radioButton2.Checked == true)
            {
                button1.Enabled = false;
                Thread thread = new Thread(new ThreadStart(woyaomaiFirst));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

                timer1.Start();

            }
        }

        /// <summary>
        /// 双击打开链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;

            System.Diagnostics.Process.Start(listView1.SelectedItems[0].SubItems[3].Text);
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(gongshi));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            else if (radioButton2.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(gongshi));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }

        private void Button2_Click(object sender, EventArgs e)
        {
         
            button1.Enabled = true;
            timer1.Stop();
        }
    }
}
