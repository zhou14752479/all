using System;
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
using CsharpHttpHelper;
using myDLL;

namespace 百姓二手车
{
    public partial class _86货车网 : Form
    {
        public _86货车网()
        {
            InitializeComponent();
        }
        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlWithCookie(string Url)
        {

            try
            {
                HttpHelper http = new HttpHelper();
                HttpItem item = new HttpItem()
                {
                    URL = Url,
                    Method = "GET",
                    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36",
                    Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9",
                    Cookie = "__yjs_duid=1_fdfa677e2fbb98bb942b9ba073a91a621629883285824; UM_distinctid=17b7c9d43bb585-08637ea2b95e6d-4343363-1fa400-17b7c9d43bc2c0; __gads=ID=76592425d54e9c9d-2268c18c1bcb0099:T=1629883287:RT=1629883287:S=ALNI_MYmi41S79K7r9abEfQR8HPgZiVOFg; Hm_lvt_f842c043ced087368ebcd293e788bad4=1629883287,1629940250,1630292867; CNZZDATA2228403=cnzz_eid%3D435990255-1629881780-https%253A%252F%252Fwww.86huoche.com%252F%26ntime%3D1630290788",
                    Host = "m.86huoche.com",
                    Timeout=5000,
                };
                item.Header.Add("sec-ch-ua", "\"Chromium\";v=\"92\", \" Not A; Brand\";v=\"99\", \"Google Chrome\";v=\"92\"");
                item.Header.Add("sec-ch-ua-mobile", "?0");
                item.Header.Add("Sec-Fetch-Site", "none");
                item.Header.Add("Sec-Fetch-Mode", "navigate");
                item.Header.Add("Sec-Fetch-User", "?1");
                item.Header.Add("Sec-Fetch-Dest", "document");
                item.Header.Add("Accept-Encoding", "gzip, deflate, br");
                item.Header.Add("Accept-Language", "zh,sq;q=0.9,zh-CN;q=0.8,oc;q=0.7,de;q=0.6,en;q=0.5");
                HttpResult result = http.GetHtml(item);
                string html = result.Html;
                return html;
            }
            catch (Exception)
            {

                return "";
            }
       

    }
        #endregion

        #region  获取跳转网址
        public static string GetRedirectUrl(string url)
        {

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "HEAD";
                req.Timeout = 5000;
                req.AllowAutoRedirect = false;
                HttpWebResponse myResp = (HttpWebResponse)req.GetResponse();
                if (myResp.StatusCode == HttpStatusCode.Redirect)
                { url = myResp.GetResponseHeader("Location"); }
                return url;
            }
            catch (Exception)
            {

                return "";
            }
        }
        #endregion

        #region 86货车
        public void _86huoche()
        {



            try
            {
                for (int page = 1; page < 3000; page++)
                {

                    string url = "https://www.86huoche.com/china/list/?page=" + page;

                    string html = method.GetUrl(url,"utf-8");

                    MatchCollection ids = Regex.Matches(html, @"id='a([\s\S]*?)'");
                    MatchCollection titles = Regex.Matches(html, @"<h3>([\s\S]*?)</h3>");

                   
                    label2.Text = "正在获取第"+page+"页,共"+ ids.Count.ToString();
                    if (ids.Count == 0)
                    {
                        continue;
                    }
                    for (int a = 0; a < ids.Count; a++)
                    {
                        try
                        {

                            string aurl = "https://m.86huoche.com/ershouche/member.ashx?id=" + ids[a].Groups[1].Value + "&mid=13965";

                            string ahtml = GetUrlWithCookie(aurl);
                           
                            string city = Regex.Match(ahtml, @"html\('([\s\S]*?)'").Groups[1].Value;
                            string tel = Regex.Match(ahtml, @"tel:([\s\S]*?)'").Groups[1].Value;

                            if (tel == "")
                            {
                                string location = "https:" + Regex.Match(ahtml, @"location='([\s\S]*?)'").Groups[1].Value;
                                tel = GetRedirectUrl(location).Replace("tel:","");
                            }

                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                            lv1.SubItems.Add(ids[a].Groups[1].Value);
                            lv1.SubItems.Add(Regex.Replace(titles[a].Groups[1].Value, "<[^>]+>", ""));
                            lv1.SubItems.Add(tel);
                            lv1.SubItems.Add(city);
                            Thread.Sleep(100);
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;
                        }
                        catch (Exception ex)
                        {
                           // MessageBox.Show(ex.ToString());
                            Thread.Sleep(2000);
                            continue;
                        }

                    }
                }

            }
            catch (Exception ex)
            {

              ex.ToString();
            }





        }

        #endregion
        bool zanting = true;
        bool status = true;
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(_86huoche);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void _86货车网_Load(object sender, EventArgs e)
        {

        }
    }
}
