using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_4
{
    public partial class 搜索引擎 : Form
    {
        public 搜索引擎()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }


        #region  获取跳转后的URL

        static string getTurl(string cahxunurl)
        {
            string url = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(cahxunurl);
            
            req.Method = "get";
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.81 Safari/537.36";
            req.AllowAutoRedirect = false;
            HttpWebResponse myResp = (HttpWebResponse)req.GetResponse();
            if (myResp.StatusCode == HttpStatusCode.Redirect)
            { url = myResp.GetResponseHeader("Location"); }
            return url;
        }
        #endregion

        private void 搜索引擎_Load(object sender, EventArgs e)
        {

        }

        bool status = true;

        ArrayList finishes = new ArrayList();

        #region 百度竞价排名查询
        public void baidu()
        {

            try
            {
                string keyword = "美容院加盟";
               string keyutf8= System.Web.HttpUtility.UrlEncode(keyword);
                for (int i = 0; i < Convert.ToInt32(textBox1.Text); i++)
                {

                    string Url = "https://m.baidu.com/from=1000539d/s?pn=" + i + "0&word=" + keyutf8;

                    string html = method.GetHtmlSource(Url);

                    MatchCollection urls = Regex.Matches(html, @"data-lp=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection bs = Regex.Matches(html, @"mu':'([\s\S]*?)'", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    
                    if (comboBox1.Text == "自然排名网址")
                    {
                        urls = bs;
                    }

                    foreach (Match url in urls)
                    {

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(System.Web.HttpUtility.UrlDecode(url.Groups[1].Value.Trim()));
                        lv1.SubItems.Add(keyword);

                        if (this.status == false)
                            return;

                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }

                    }
                    Thread.Sleep(Convert.ToInt32(textBox3.Text));
                }
            }




            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        #region 360竞价排名查询
        public void a360()
        {

            try
            {
                string keyword = "美容院加盟";
                string keyutf8 = System.Web.HttpUtility.UrlEncode(keyword);

                for (int i = 1; i < Convert.ToInt32(textBox1.Text); i++)
                {

                    string Url = "https://m.so.com/nextpage?q="+ keyutf8 + "&src=suggest_m1.0_a&sug_pos=2&srcg=home_next&pn="+i+"&ajax=1&psid=c7412fca1c4a62b43444f9d081cded08&es=0%7C0%7C0%7C0%7C0%7C0%7C-1";

                    string html = method.GetHtmlSource(Url);

                    MatchCollection ass = Regex.Matches(html, @"<a class=""e-more-see-detail"" href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection urls = Regex.Matches(html, @"<span class='res-site-url'>([\s\S]*?)</span>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    if (comboBox1.Text == "自然排名网址")
                    {
                        foreach (Match url in urls)
                        {

                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                            lv1.SubItems.Add(System.Web.HttpUtility.UrlDecode(url.Groups[1].Value.Trim()));
                            lv1.SubItems.Add(keyword);
                            if (this.status == false)
                                return;

                            if (listView1.Items.Count > 2)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                            }

                        }
                    }
                    else if(comboBox1.Text == "竞价排名网址")
                    {

                        foreach (Match url in ass)
                        {

                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                            lv1.SubItems.Add(getTurl(url.Groups[1].Value.Trim()));
                            lv1.SubItems.Add(keyword);
                            if (this.status == false)
                                return;

                            if (listView1.Items.Count > 2)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                            }

                        }
                    }














                  
                    Thread.Sleep(Convert.ToInt32(textBox3.Text));
                }
            }




            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        #region 搜狗竞价排名查询
        public void sougou()
        {

            try
            {
                string keyword = "美容院加盟";
                string keyutf8 = System.Web.HttpUtility.UrlEncode(keyword);
                for (int i = 0; i < Convert.ToInt32(textBox1.Text); i++)
                {

                    string Url = "https://m.sogou.com/web/search/ajax_query.jsp?type=1&uID=AAGFTAEkJwAAAAqMGE/jJgEAkwA=&v=5&dp=1&pid=sogou-waps-7880d7226e872b77&keyword="+keyutf8+"&suuid=65173a96-4d07-406e-b5a6-6bb5e967b8f7&p="+i+"&s_from=pagenext&showextquery=1";

                    string html = method.GetHtmlSource(Url);

                    MatchCollection urls = Regex.Matches(html, @"data-lp=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection bs = Regex.Matches(html, @"<div class=""citeurl"">([\s\S]*?)</div>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    if (comboBox1.Text == "自然排名网址")
                    {
                        urls = bs;
                    }

                    foreach (Match url in urls)
                    {

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(Regex.Replace(url.Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(keyword);

                        if (this.status == false)
                            return;

                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }

                    }
                    Thread.Sleep(Convert.ToInt32(textBox3.Text));
                }
            }




            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        #region 神马竞价排名查询
        public void shenma()
        {

            try
            {
                string keyword = "美容院加盟";
                string keyutf8 = System.Web.HttpUtility.UrlEncode(keyword);
                for (int i = 0; i < Convert.ToInt32(textBox1.Text); i++)
                {

                    string Url = "https://m.sm.cn/s?q="+keyutf8+"&page="+i+"&by=next&from=smor&safe=1";

                    string html = method.GetUrl(Url,"utf-8");

                    MatchCollection urls = Regex.Matches(html, @"<div class=""other"">([\s\S]*?)</div>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    foreach (Match url in urls)
                    {
                        if (comboBox1.Text == "自然排名网址"&& !url.Groups[1].Value.Contains("广告"))
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                            lv1.SubItems.Add(url.Groups[1].Value.Trim());
                            lv1.SubItems.Add(keyword);

                            if (listView1.Items.Count > 2)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                            }
                        }

                        else if(comboBox1.Text == "竞价排名网址" && url.Groups[1].Value.Contains("广告"))
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                            lv1.SubItems.Add(Regex.Replace(url.Groups[1].Value, "<[^>]+>", "").Replace(("广告"),"").Trim());
                            lv1.SubItems.Add(keyword);

                            if (listView1.Items.Count > 2)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                            }
                        }




                        if (this.status == false)
                            return;


                    }

                    Thread.Sleep(Convert.ToInt32(textBox3.Text));

                }
            }




            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        private void Button1_Click(object sender, EventArgs e)
        {

            status = true;
            if (radioButton1.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(baidu));
                thread.Start();
            }
            else if (radioButton2.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(a360));
                thread.Start();
            }
            else if (radioButton3.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(sougou));
                thread.Start();
            }
            else if (radioButton4.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(shenma));
                thread.Start();
            }



        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
