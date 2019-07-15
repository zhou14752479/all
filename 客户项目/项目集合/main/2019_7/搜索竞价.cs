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
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void 搜索竞价_Load(object sender, EventArgs e)
        {

        }

        bool status = true;
       

    #region  获取跳转后的URL

    static string getTurl(string url)
        {
          
                Uri uri = new Uri(url);
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);
                myReq.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
                myReq.Accept = "*/*";
                myReq.KeepAlive = true;
                myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
                HttpWebResponse result = (HttpWebResponse)myReq.GetResponse();
                //Console.Write(result.Headers);
                Stream receviceStream = result.GetResponseStream();
                StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8"));
                string strHTML = readerOfStream.ReadToEnd();
                readerOfStream.Close();
                receviceStream.Close();
                result.Close();
               // return strHTML;
                Match url2 = Regex.Match(strHTML, @"replace\(""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                return url2.Groups[1].Value;


        }
        #endregion

        #region  获取跳转后的URL2

        static string getTTurl(string url)
        {

            
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "get";
           
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
        #region 手机端百度竞价排名查询完成
        public void baidu()
        {

            try
            {
                string[] keywords = textBox4.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string keyword in keywords)
                {
                    string keyutf8 = System.Web.HttpUtility.UrlEncode(keyword);
                        string Url = "https://m.baidu.com/from=1000539d/s?word=" + keyutf8;

                        string html = method.gethtml(Url, "","utf-8");
                  
                        MatchCollection urls = Regex.Matches(html, @"data-lp=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                  
                        foreach (Match url in urls)
                        {

                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                            lv1.SubItems.Add(System.Web.HttpUtility.UrlDecode(url.Groups[1].Value.Trim()));
                            lv1.SubItems.Add(keyword);
                        lv1.SubItems.Add("百度");

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

        #region 手机端360竞价排名查询完成
        public void a360()
        {

            try
            {
                string[] keywords = textBox4.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string keyword in keywords)
                {
                    string keyutf8 = System.Web.HttpUtility.UrlEncode(keyword);

                  

                        string Url = "https://m.so.com/nextpage?q=" + keyutf8 ;

                        string html = method.gethtml(Url,"", "utf-8");

                        MatchCollection urls = Regex.Matches(html, @"<a class=""e-more-see-detail"" href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline); //需要跳转
                     
                       
                            foreach (Match url in urls)
                            {

                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                                lv1.SubItems.Add(getTTurl(url.Groups[1].Value.Trim()));
                                lv1.SubItems.Add(keyword);
                        lv1.SubItems.Add("360移动");
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

        #region 搜狗PC端完成
        public void sougou()
        {
            string[] keywords =textBox4.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            try
            {
               
                foreach (string keyword in keywords)
                {
                    string keyutf8 = System.Web.HttpUtility.UrlEncode(keyword, Encoding.GetEncoding("utf-8")); 

                    string cookie = "usid=07WwkCu3b_78aUPT; SNUID=6316F79EAEAB22E36A0A605EAFCC12C7; IPLOC=CN3213; SUV=00BA2DBC3159B8CD5D2585534E6EA580; CXID=5EA7E0DBFC0F423A95BC1EB511A405C7; ABTEST=0|1562751095|v17; SUID=CDB859313118960A000000005D25B077; browerV=3; osV=1; sct=1; LSTMV=105%2C349; LCLKINT=8529; cd=1562894171&0d0613433f97ba7fd20b80f4a9e5403a; ld=XZllllllll2NJYqFgvKKQC1XT1SNJddbGquc8yllll9llllxRllll5@@@@@@@@@@";
                    string Url = "https://www.sogou.com/web?query="+keyutf8+"&ie=utf8&_ast=1562895232&_asf=null&w=01029901&cid=&cid=&s_from=result_up";

                    string html = method.GetUrlWithCookie(Url,cookie, "utf-8");
                  
                    MatchCollection urls = Regex.Matches(html, @"<h3 class=""biz_title""><a href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                   
                    foreach (Match url in urls)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(getTurl(url.Groups[1].Value));
                        lv1.SubItems.Add(keyword);
                        lv1.SubItems.Add("搜狗PC");

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

        #region 搜狗手机端完成
        public void sougou1()
        {

            try
            {
                string[] keywords = textBox4.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string keyword in keywords)
                {
                    string keyutf8 = System.Web.HttpUtility.UrlEncode(keyword);

                    string cookie = "usid=07WwkCu3b_78aUPT; SNUID=6316F79EAEAB22E36A0A605EAFCC12C7; IPLOC=CN3213; SUV=00BA2DBC3159B8CD5D2585534E6EA580; CXID=5EA7E0DBFC0F423A95BC1EB511A405C7; ABTEST=0|1562751095|v17; SUID=CDB859313118960A000000005D25B077; browerV=3; osV=1; sct=1; LSTMV=105%2C349; LCLKINT=8529; cd=1562894171&0d0613433f97ba7fd20b80f4a9e5403a; ld=XZllllllll2NJYqFgvKKQC1XT1SNJddbGquc8yllll9llllxRllll5@@@@@@@@@@";
                    string Url = "https://wap.sogou.com/web/searchList.jsp?uID=AAFo4YqaKAAAAAqMGDTQ4AAAZAM%3D&v=5&dp=1&pid=sogou-waps-7880d7226e872b77&w=1283&t=1562897199082&s_t=1562897383887&s_from=result_up&htprequery=" + keyutf8 + "&keyword=" + keyutf8+"&pg=webSearchList&rcer=uNz_alvVqvzeAE_5&s=%E6%90%9C%E7%B4%A2&suguuid=4454524d-4520-4e6e-b448-fb7a6c6bb5f2&sugsuv=AAFo4YqaKAAAAAqMGDTQ4AAAZAM&sugtime=1562897383887";

                    string html = method.gethtml(Url, cookie, "utf-8");

                     MatchCollection urls = Regex.Matches(html, @"cite_url:'([\s\S]*?)'", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                       
                     

                        foreach (Match url in urls)
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                            lv1.SubItems.Add(url.Groups[1].Value);
                            lv1.SubItems.Add(keyword);
                        lv1.SubItems.Add("搜狗移动");

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

        #region UC搜索
        public void Uc()
        {

            try
            {
                string[] keywords = textBox4.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string keyword in keywords)
                {
                    string keyutf8 = System.Web.HttpUtility.UrlEncode(keyword);

                    string cookie = "sm_uuid=7be889c3e1b5b527a6a1ab71bbc0f48c%7C%7C%7C1563171518; sm_diu=7be889c3e1b5b527a6a1ab71bbc0f48c%7C%7C11eef1794c3d0baf5b%7C1563171518; sm_sid=7be889c3e1b5b527a6a1ab71bbc0f48c; cna=8QJMFUu4DhACATFZv2JYDtwd; isg=BFBQDZWhOf8bd-X74K9oyJyVIZ5isTwJDjayE0ojnqt-hfUv8SgG87D3XQ3AVew7";
                    string Url = "https://so.m.sm.cn/s?q="+keyutf8+"&uc_param_str=dnntnwvepffrgibijbprsv&from=ucdh";

                    string html = method.GetUrlWithCookie(Url,cookie, "utf-8");
                   
                    MatchCollection urls = Regex.Matches(html, @"<div class=""other"">([\s\S]*?)</div>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                   

                    foreach (Match url in urls)
                    {
                        if (url.Groups[1].Value.Contains("广告"))
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                            lv1.SubItems.Add(Regex.Replace(url.Groups[1].Value, "<[^>]+>", "").Replace("广告","").Trim());
                            lv1.SubItems.Add(keyword);
                            lv1.SubItems.Add("UC移动");

                            if (this.status == false)
                                return;

                            if (listView1.Items.Count > 2)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
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
                Thread thread1 = new Thread(new ThreadStart(baidu));
                thread1.Start();
                Thread thread2 = new Thread(new ThreadStart(a360));
                thread2.Start();
                Thread thread3 = new Thread(new ThreadStart(sougou));
                thread3.Start();
                Thread thread4 = new Thread(new ThreadStart(sougou1));
                thread4.Start();
                Thread thread5 = new Thread(new ThreadStart(Uc));
                thread5.Start();

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

      
    }
}
