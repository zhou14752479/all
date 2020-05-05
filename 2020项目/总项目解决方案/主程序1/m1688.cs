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
namespace 主程序1
{
    public partial class m1688 : Form
    {
        public m1688()
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


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "session-id=144-7450304-7580635; session-id-time=2082787201l; ubid-main=133-9411273-8184314; x-wl-uid=1AR+eCj1iY57TRhM7A2m5KF9SEb1ho13Om87l60jAFJAp10qHX8GNgnZcOFTknCbmNkftPnMho/k=; aws-priv=eyJ2IjoxLCJldSI6MCwic3QiOjB9; aws-target-static-id=1536650638823-915613; s_fid=16BD3861C3483809-386224FB67B4E94E; regStatus=pre-register; s_dslv=1536656308918; i18n-prefs=USD; lc-main=zh_CN; sp-cdn=\"L5Z9: CN\"; session-token=/8/yst6nJSzUghSOya1omO6MEhQ/Moyyq2FsFStf5zcm4cZPhl38RIpfC+UZyiw//J9HubG+McoZMSB4hRyykQZ0SH1X07eSi5nxcOjmHQshqSmCJD6tL8cgFOFCByRnF1EJMjmxRfVwTkZZ/4yLqjzBQ2Ik6WclU4tG1u7+4UCFeGDYa//WLb3fCGfB6RuU; csm-hit=tb:DT2JH7KAE9BTWY50PJA8+s-DT2JH7KAE9BTWY50PJA8|1585472314824&t:1585472314824&adb:adblk_no";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://www.amazon.com/s?k=6Q0+959+856&__mk_zh_CN=%E4%BA%9A%E9%A9%AC%E9%80%8A%E7%BD%91%E7%AB%99&ref=nb_sb_noss";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("sec-fetch-mode:navigate");
                headers.Add("sec-fetch-site:same-origin");
                headers.Add("sec-fetch-user:?1");
                headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                // request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
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
        #region   主程序
        public void run()
        {
           

            try
            {
                ArrayList lists = new ArrayList();

                foreach (string keyword in lists)
                {


                    for (int i = 1; i <= 70; i++)
                    {
                        String Url = "https://m.1688.com/offer_search/-6D7033.html?keywords=" + System.Web.HttpUtility.UrlEncode(keyword);

                        string html = GetUrl(Url);

                        MatchCollection TitleMatchs = Regex.Matches(html, @"https://[a-z]+.58.com/[a-z]+/[0-9]+x.shtml", RegexOptions.IgnoreCase | RegexOptions.Multiline);




                        foreach (Match NextMatch in TitleMatchs)
                        {
                            if (!lists.Contains(NextMatch.Groups[0].Value))
                            {
                                lists.Add(NextMatch.Groups[0].Value);
                            }
                        }


                        foreach (string list in lists)
                        {

                            Match uid = Regex.Match(list, @"\d{10,}");

                            string strhtml = GetUrl("https://miniappfang.58.com/shop/plugin/v1/shopdetail?infoId=" + uid.Groups[0].Value + "&openId=77AA769A2A2C8740ECF1EDB47CD855A04C573D57DAF470CD8AD018A504661F6A");  //定义的GetRul方法 返回 reader.ReadToEnd()

                            Match title = Regex.Match(strhtml, @"""title"":""([\s\S]*?)""");
                            Match contacts = Regex.Match(strhtml, @"""brokerName"":""([\s\S]*?)""");
                            Match tel = Regex.Match(strhtml, @"""phone"":""([\s\S]*?)""");
                            Match region = Regex.Match(strhtml, @"""quyu"":""([\s\S]*?)""");
                            Match dizhi = Regex.Match(strhtml, @"""dizhi"":""([\s\S]*?)""");

                            Match date = Regex.Match(strhtml, @"""postDate"":""([\s\S]*?)""");
                            Match description = Regex.Match(strhtml, @"""description"":""([\s\S]*?)""");

                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            listViewItem.SubItems.Add(title.Groups[1].Value);
                            listViewItem.SubItems.Add(contacts.Groups[1].Value);
                            listViewItem.SubItems.Add(tel.Groups[1].Value);
                            listViewItem.SubItems.Add(region.Groups[1].Value);
                            listViewItem.SubItems.Add(dizhi.Groups[1].Value);
                            listViewItem.SubItems.Add(date.Groups[1].Value);
                            listViewItem.SubItems.Add(description.Groups[1].Value);



                            Application.DoEvents();
                            Thread.Sleep(1000);   //内容获取间隔，可变量

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }

                        }

                    }
                    }
                


            }

            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }

        bool zanting = true;

        #endregion

        private void m1688_Load(object sender, EventArgs e)
        {

        }
    }
}
