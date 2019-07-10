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

namespace main._2019_7
{
    public partial class 搜索竞价 : Form
    {
        public 搜索竞价()
        {
            InitializeComponent();
        }

        private void 搜索竞价_Load(object sender, EventArgs e)
        {

        }

        bool status = true;

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

        public string[] ReadText()
        {

            StreamReader streamReader = new StreamReader(this.textBox4.Text);
            string text = streamReader.ReadToEnd();
            return text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        }
        ArrayList finishes = new ArrayList();
        #region 百度竞价排名查询
        public void baidu()
        {

            try
            {
                string[] keywords = this.ReadText();
                foreach (string keyword in keywords)
                {
                    string keyutf8 = System.Web.HttpUtility.UrlEncode(keyword);
                        string Url = "https://m.baidu.com/from=1000539d/s?word=" + keyutf8;

                        string html = method.GetHtmlSource(Url, "utf-8");

                        MatchCollection urls = Regex.Matches(html, @"data-lp=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

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
                string[] keywords = this.ReadText();
                foreach (string keyword in keywords)
                {
                    string keyutf8 = System.Web.HttpUtility.UrlEncode(keyword);

                  

                        string Url = "https://m.so.com/nextpage?q=" + keyutf8 ;

                        string html = method.GetHtmlSource(Url, "utf-8");

                        MatchCollection urls = Regex.Matches(html, @"<a class=""e-more-see-detail"" href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline); //需要跳转
                     
                       
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
            }



            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        #region 搜狗PC端
        public void sougou()
        {

            try
            {
                string[] keywords = this.ReadText();
                foreach (string keyword in keywords)
                {
                    string keyutf8 = System.Web.HttpUtility.UrlEncode(keyword);


                    string Url = "https://www.sogou.com/web?query=" + keyutf8;

                    string html = method.GetHtmlSource(Url, "utf-8");

                    MatchCollection urls = Regex.Matches(html, @"<h3 class=""biz_title""><a href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    MessageBox.Show(urls.Count.ToString());
                    foreach (Match url in urls)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(getTurl(url.Groups[1].Value));
                        lv1.SubItems.Add(keyword);

                        if (this.status == false)
                            return;

                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }
                    }


                }

            }


            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        #region 搜狗手机端
        public void sougou1()
        {

            try
            {
                string[] keywords = this.ReadText();
                foreach (string keyword in keywords)
                {
                    string keyutf8 = System.Web.HttpUtility.UrlEncode(keyword);
                  

                        string Url = "https://m.sogou.com/web/search/ajax_query.jsp?type=1&uID=AAGFTAEkJwAAAAqMGE/jJgEAkwA=&v=5&dp=1&pid=sogou-waps-7880d7226e872b77&keyword=" + keyutf8 ;

                        string html = method.GetHtmlSource(Url, "utf-8");

                        MatchCollection urls = Regex.Matches(html, @"data-lp=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        MatchCollection bs = Regex.Matches(html, @"<div class=""citeurl"">([\s\S]*?)</div>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                     

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
            #region 通用登录
            if (textBox4.Text == "")
            {
                MessageBox.Show("请输入关键字");
                return;
            }
            status = true;
            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == localip.Trim())
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {

               
                    Thread thread = new Thread(new ThreadStart(sougou));
                    thread.Start();
                
            }
            else
            {
                MessageBox.Show("请登录您的账号！");
                System.Diagnostics.Process.Start("http://www.acaiji.com");
                return;
            }
            #endregion
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox4.Text = this.openFileDialog1.FileName;
            }
        }
    }
}
