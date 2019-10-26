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

namespace 藏宝阁
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }


       public static  string COOKIE = "return_url=; msg_box_flag=1; vjuids=-2726bafde.161942f98fe.0.02543fddefe7; _ntes_nuid=3f1bc64af8e3be5d2510190789b7c3d9; _ga=GA1.2.2029656332.1522893311; __gads=ID=ce9df49c84b50a78:T=1531915856:S=ALNI_Mb1_wWgIlwzpsg1nHswBaugPS2PMA; vjlast=1518609603.1540779331.12; _ntes_nnid=3f1bc64af8e3be5d2510190789b7c3d9,1552719963380; vinfo_n_f_l_n3=85348dd7f24f9d12.1.7.1518609602838.1540779571787.1552720007350; UM_distinctid=16c1460b43a524-0d13ac087207e7-c343162-1fa400-16c1460b43b3eb; P_INFO=xingjiyou001@163.com|1563710223|0|other|00&99|jis&1563709989&163#jis&321300#10#0#0|&0|163&ecard&mail163|xingjiyou001@163.com; mail_psc_fingerprint=137d1e4f97c7c196c6daef1e19ff9ccf; __f_=1570701703931; fingerprint=gecq3ncnemrwn1u2; area_td_id=1; area_id=3; _nietop_foot=%u5927%u8BDD2%u7ECF%u5178%u7248%7Cxy2.163.com; latest_views=115_1408813; msg_box_flag=1; cur_servername=%25E7%25A7%25A6%25E5%2585%25B3%25E6%25B1%2589%25E6%259C%2588; sid=bb7bNo2EAsr4kWJpHVD7lQJipF4N6e79Bj2U-_dx; last_login_serverid=249; wallet_data=%7B%22is_locked%22%3A%20false%2C%20%22checking_balance%22%3A%200%2C%20%22balance%22%3A%200%2C%20%22free_balance%22%3A%200%7D";
        #region GET请求带COOKIE
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
                    ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                    listViewItem.SubItems.Add(serverName.Groups[1].Value);
                    listViewItem.SubItems.Add(prices[i+1].Groups[1].Value);
                    listViewItem.SubItems.Add(links[i].Groups[1].Value);
                }
            }
            catch (Exception ex)
            {

                textBox1.Text = ex.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(gongshi));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
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
    }
}
