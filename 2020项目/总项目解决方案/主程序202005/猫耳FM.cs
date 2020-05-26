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
using CsharpHttpHelper;
using helper;

namespace 主程序202005
{
    public partial class 猫耳FM : Form
    {
        public 猫耳FM()
        {
            InitializeComponent();
        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {

               HttpHelper http = new HttpHelper();
               HttpItem item = new HttpItem()
                {
                    URL = Url,
                    Method = "GET",
                    Accept = "*/*",
                    UserAgent = "MissEvanApp/4.4.9 (iOS;12.3.1;iPhone9,2)",
                    Referer = "https://app.missevan.com/drama/filter?filters=1_0_0_0_0_1&page=2",
                    Host = "app.missevan.com",
                    Cookie = "equip_id=3b5c5e74-08a3-4604-a2f7-710fbc91a576",
                };
                item.Header.Add("Accept-Encoding", "");
                item.Header.Add("Accept-Language", "zh-cn,zh,en");
                item.Header.Add("Authorization", "MissEvan AKkebUxMJk7jevkGQcTzjE28T9ioJ8IR9+AJnR0q5wM=");
                item.Header.Add("X-M-Date", "2020-05-25T09:53:50Z");
                item.Header.Add("X-M-Nonce", "276aaafb-50fc-475a-8ff2-325687011b50");
                HttpResult result = http.GetHtml(item);
                string html = result.Html;
                return html;
            

        }
        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"ximalaya"))
            {

                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
        }

        public void run()
        {

            try
            {
               
                for (int i = 2; i < 9999; i++)
                {
                    string url = "https://app.missevan.com/drama/filter?filters=1_0_0_0_0_1&page="+i;

                    string html = GetUrl(url);
                    MatchCollection uids = Regex.Matches(html, @"""url"":""([\s\S]*?)""");
                    textBox1.Text = html;
                    MessageBox.Show("1");

                    for (int j = 0; j < uids.Count; j++)
                    {
                        string URL = "https://m.ximalaya.com" + uids[j].Groups[1].Value;
                        string ahtml = GetUrl(URL);

                        Match fans = Regex.Match(ahtml, @"style=""font-size:16px""></i>([\s\S]*?)<");
                        Match name = Regex.Match(ahtml, @"<span class=""name sG_"">([\s\S]*?)<");
                        Match dingyue = Regex.Match(ahtml, @"已订阅<!-- -->([\s\S]*?)<");
   
                  



                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                    //    lv1.SubItems.Add(titles[j].Groups[1].Value);
                
                       

                    }
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void 猫耳FM_Load(object sender, EventArgs e)
        {

        }
    }
}
