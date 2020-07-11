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

namespace 主程序202006
{
    public partial class heepsy : Form
    {
        public heepsy()
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
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "_gcl_au=1.1.1037081120.1591082515; _ga=GA1.2.1860429647.1591082516; intercom-id-t9epzkvq=dc62d19c-ec40-4a1d-b2af-5c91967d4f6c; _fbp=fb.1.1591092390530.420941159; _reg_id_73966a73f6=Y2hyaXMuY2hlbkB3ZWJyaWRnZXVzLmNvbQ%3D%3D%0A; _gid=GA1.2.1428889311.1591599014; fs_uid=rs.fullstory.com#84XC9#5116204840271872:4944138676355072#c9b3df1f#/1622628442; fs_intercom=5116204840271872:4944138676355072; _hjid=61107388-6af2-404a-a669-45b2c2368e6a; _hjIncludedInSample=1; _hjAbsoluteSessionInProgress=1; intercom-session-t9epzkvq=dVhlSFI3M0VEVUVhb3I0ZzZYL1FmUGtpWTJYQmlHUCtqOXBRb2FnK09uVGw3ZWlPem54all1Q0sxa2ZRNEdjZS0tM2NMN09adThGUDhvcXprTXRBaWt2Zz09--9865d4c41ad207a2b6fea57756c2f53a156ad4cc; session_cookie=361568bb92627968dce60f243ad2ece6; _uetsid=9814986a-e714-f52f-bd90-911a2631a646";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                request.Headers.Add("Cookie", COOKIE);

                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
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
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            
          
            for (int i = Convert.ToInt32(textBox1.Text); i < 999; i++)
            {
                label1.Text = i.ToString();

                string url= "https://www.heepsy.com/influencers?page="+i;

                string html = GetUrl(url, "utf-8");

                MatchCollection ids = Regex.Matches(html, @""" data-id=""([\s\S]*?)""");

                MatchCollection posts = Regex.Matches(html, @"""post_count"":""([\s\S]*?)""");
                MatchCollection followers = Regex.Matches(html, @"""follower_count"":""([\s\S]*?)""");
                MatchCollection following = Regex.Matches(html, @"""following_count"":""([\s\S]*?)""");
                MatchCollection engagement = Regex.Matches(html, @"""engagement"":""([\s\S]*?)""");
                MatchCollection tags = Regex.Matches(html, @"<p class=""text-muted block-with-text"">([\s\S]*?)</p>");
                MatchCollection address = Regex.Matches(html, @"locationsArray"":([\s\S]*?),");

                MatchCollection emails = Regex.Matches(html, @"user_email = '([\s\S]*?)'");

                if (ids.Count == 0)
                {
                    MessageBox.Show(" ");
                    return;
                }

                for (int j = 0; j < ids.Count; j++)
                {
                    string aurl = "https://www.heepsy.com/get_influencer_contact.json?id="+ids[j].Groups[1].Value+"&m_source=instagram";
                    string ahtml = GetUrl(aurl, "utf-8");

                    Match name = Regex.Match(ahtml, @"""username"":""([\s\S]*?)""");
                    Match fullname = Regex.Match(ahtml, @"""full_name"":""([\s\S]*?)""");

                    Match ins = Regex.Match(ahtml, @"""link_to_profile"":""([\s\S]*?)""");

                    Match links = Regex.Match(ahtml, @"""links"":\[""([\s\S]*?)""");


                    Match percent = Regex.Match(ahtml, @"""percent"":""([\s\S]*?)""");
                    Match EngagementRate = Regex.Match(ahtml, @"""percent"":""([\s\S]*?)""");


                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(name.Groups[1].Value);
                    lv1.SubItems.Add(fullname.Groups[1].Value);
                    lv1.SubItems.Add(posts[j].Groups[1].Value);
                    lv1.SubItems.Add(followers[j].Groups[1].Value);
                    lv1.SubItems.Add(following[j].Groups[1].Value);
                    lv1.SubItems.Add(engagement[j].Groups[1].Value);
                    lv1.SubItems.Add(address[j].Groups[1].Value);
                    lv1.SubItems.Add(emails[j].Groups[1].Value);
                    lv1.SubItems.Add(ins.Groups[1].Value);
                    lv1.SubItems.Add(links.Groups[1].Value);
                    lv1.SubItems.Add(tags[j].Groups[1].Value);
                    lv1.SubItems.Add(EngagementRate.Groups[1].Value);

                    lv1.SubItems.Add(ahtml);

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                }
             

             




            }




        }
        private void heepsy_Load(object sender, EventArgs e)
        {

        }
        bool zanting = true;
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

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }
    }
}
