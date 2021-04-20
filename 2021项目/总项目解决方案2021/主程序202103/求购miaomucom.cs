using System;
using System.Collections;
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

namespace 主程序202103
{
    public partial class 求购miaomucom : Form
    {
        public 求购miaomucom()
        {
            InitializeComponent();
        }
        Thread thread;
        string cookie = "bdshare_firstime=1613808835295; Hm_lvt_e6885059788fc2617b304f7df85469c8=1613808888,1614411582,1614740603; destoon_auth=UjQAPVNnUz9TMARpWwUCYAYoUy8COwBlAzYENQFYDDdTDg0wU2IBYlZnBDcAYAMyBmJVZFEwAW4AZwA2UDYNMlJnAG5TM1NmUzYENVttAmEGZVMwAmIAYwNhBDgBYgwyUzY%3D; Hm_lvt_7adeae9b43e0fbccb9b7537726ae8fb1=1615688276,1616582127,1617168224,1617948489; Hm_lpvt_7adeae9b43e0fbccb9b7537726ae8fb1=1617948918";
        ArrayList finishes = new ArrayList();

        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlWithCookie(string Url, string COOKIE, string charset)
        {
            string html = "";

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36";

                request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {

                return ex.ToString();

            }

        }
        #endregion
        #region 主程序
        public void run()
        {
            textBox1.Text = DateTime.Now.ToString() + "：开始本次抓取...";
            string firstdate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            for (int i = 0; i < 100; i++)
            {


                string url = "https://api.miaomu.com/mysj/info/listqg03?country=&pdType=&page="+i+"0&showname=";

                string html = GetUrlWithCookie(url, cookie, "utf-8");

                MatchCollection uids = Regex.Matches(html, @"""infoId"":([\s\S]*?),");
                MatchCollection title = Regex.Matches(html, @"""showname"":""([\s\S]*?)""");
                MatchCollection tel = Regex.Matches(html, @"""phone"":""([\s\S]*?)""");
                MatchCollection province = Regex.Matches(html, @"""country"":""([\s\S]*?)""");
                MatchCollection address = Regex.Matches(html, @"""address"":""([\s\S]*?)""");
                MatchCollection date = Regex.Matches(html, @"""dateandtime"":""([\s\S]*?)""");
                MatchCollection content = Regex.Matches(html, @"""chanpinguige"":""([\s\S]*?)""");
              
                for (int j = 0; j < title.Count; j++)
                {
                    if (!finishes.Contains(uids[j].Groups[1].Value))
                    {
                        finishes.Add(uids[j].Groups[1].Value);
 
                        try
                        {
                           
                            if (Convert.ToDateTime(date[j].Groups[1].Value).ToString("yyyy-MM-dd")!=DateTime.Now.ToString("yyyy-MM-dd"))
                            {
                                textBox1.Text = DateTime.Now.ToString() + "：本次抓取结束,等待下次执行...";
                                return;
                            }
                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(title[j].Groups[1].Value);

                            lv1.SubItems.Add(Regex.Replace(tel[j].Groups[1].Value, "<[^>]+>", "").Replace(" ", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(province[j].Groups[1].Value, "&nbsp;.*", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(address[j].Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(date[j].Groups[1].Value, "<[^>]+>", ""));
                            lv1.SubItems.Add(Regex.Replace(content[j].Groups[1].Value, "<[^>]+>", "").Replace("&nbsp;", ""));

                            Thread.Sleep(100);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }

                }

            }

        }

        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"ZkGmf7"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            timer1.Start();
            timer1.Interval = Convert.ToInt32(textBox2.Text) * 60 * 1000;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = DateTime.Now.ToString() + "：结束抓取...";
            timer1.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                timer1.Start();
            }
            else
            {
                textBox1.Text = DateTime.Now.ToString() + "：结束抓取...";
                timer1.Stop();
            }
        }
    }
}
