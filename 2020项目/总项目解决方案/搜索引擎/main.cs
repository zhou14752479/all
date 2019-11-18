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
using MySql.Data.MySqlClient;

namespace 搜索引擎
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        bool zanting = true;
       
     

        bool status1 = false;
        bool status2 = false;
        bool status3 = false;
        bool status4 = false;

        bool tingzhi = true;

        ArrayList baidus = new ArrayList();
        ArrayList sougous = new ArrayList();
        ArrayList so360s = new ArrayList();
        ArrayList biyings = new ArrayList();
        #region 去掉路径中非法字符
        public string removeValid(string illegal)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                illegal = illegal.Replace(c.ToString(), "");
            }
            return illegal;
        }

        #endregion


        public void exportTXT(string key,string title,string body)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory+"txt\\" + key.Trim() + "\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path); //创建文件夹
            }
            FileStream fs1 = new FileStream(path + removeValid(title) + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1);
            sw.WriteLine(body);
            sw.Close();
            fs1.Close();

        }

        public string IP = "";
        public static string COOKIE = "usid=07WwkCu3b_78aUPT; IPLOC=CN3213; SUV=00BA2DBC3159B8CD5D2585534E6EA580; CXID=5EA7E0DBFC0F423A95BC1EB511A405C7; SUID=CDB859313118960A000000005D25B077; ssuid=7291915575; pgv_pvi=5970681856; start_time=1562896518693; front_screen_resolution=1920*1080; wuid=AAElSJCaKAAAAAqMCGWoVQEAkwA=; FREQUENCY=1562896843272_13; sg_uuid=6358936283; newsCity=%u5BBF%u8FC1; sortcookie=1; sw_uuid=3118318168; ld=Hyllllllll2NmXZSlllllVLfKx9llllltm@ySyllll9lllll4Zlll5@@@@@@@@@@; sct=2; SNUID=D88B74083A3FAF46DECF61003A5B6CA1";
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
                // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
               
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://news.sogou.com/news?query=site%3Asohu.com+%B4%F3%CA%FD%BE%DD&_ast=1571813760&_asf=news.sogou.com&time=0&w=03009900&sort=1&mode=1&manual=&dp=1";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";

                request.AllowAutoRedirect = true;
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

        #region GET使用代理IP请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlwithIP(string Url, string ip,string charset)
        {
            try
            {
          
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";
                WebProxy proxy = new WebProxy(ip);
                request.Proxy = proxy;
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 8000;
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

        #region  读取数据插入数据库

        public void insertData(string a, string b, string c, string d)
        {

            try
            {

                string constr = "Host =" + textBox1.Text.Trim() + ";Database=" + textBox2.Text.Trim() + ";Username=" + textBox3.Text.Trim() + ";Password=" + textBox4.Text.Trim();
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO '" + textBox6.Text.Trim() + " ' ('" + textBox7.Text.Trim() + " ','" + textBox8.Text.Trim() + " ','" + textBox9.Text.Trim() + " ','" + textBox10.Text.Trim() + " ')VALUES('" + a + " ', '" + b + " ','" + c + " ', '" + d + "')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {


                }

                mycon.Close();



            }

            catch (System.Exception ex)
            {
                ex.ToString();
            }


        }

        #endregion
       
        #region  百度获取
        public void baidu()
        {
            try
            {
                status1 = false;
                string[] keywords = textBox5.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string key in keywords)

                {
                    string[] keyword1 = key.Split(new string[] { "," }, StringSplitOptions.None);

                    foreach (string keyword in keyword1)
                    {


                        textBox11.Text += "正在抓取百度搜索" + keyword+"\r\n";
                        for (int i = 0; i < 990; i = i + 10)
                        {


                            string url = "https://www.baidu.com/s?ie=utf-8&cl=2&medium=2&rtt=1&bsst=1&rsv_dl=news_b_pn&tn=news&wd=" + keyword + "&tfflag=0&x_bfe_rqs=03E80&x_bfe_tjscore=0.002154&tngroupname=organic_news&pn=" + i;
                            string html = method.GetUrl(url, "utf-8");

                            MatchCollection ids = Regex.Matches(html, @"baijiahao\.baidu\.com([\s\S]*?)""");


                            if (ids.Count == 0)
                                break;

                            for (int j = 0; j < ids.Count; j++)
                            {

                                if (!baidus.Contains(ids[j].Groups[1].Value))
                                {
                                    baidus.Add(ids[j].Groups[1].Value);
                                    string URL = "http://baijiahao.baidu.com" + ids[j].Groups[1].Value;

                                    string strhtml = method.GetUrl(URL, "utf-8");



                                    Match a1 = Regex.Match(strhtml, @"<h2>([\s\S]*?)</h2>");
                                    Match a2 = Regex.Match(strhtml, @"dateUpdate"" content=""([\s\S]*?)""");
                                    Match a3 = Regex.Match(strhtml, @"uthor-name"">([\s\S]*?)<");
                                    Match a4 = Regex.Match(strhtml, @"<div class=""article-content"">([\s\S]*?)</source>");

                                    //DateTime dt = Convert.ToDateTime(a2.Groups[1].Value);
                                    //if (dateTimePicker1.Value < dt && dt < dateTimePicker2.Value)
                                    //{
                                    if (checkBox1.Checked == true)
                                    {
                                        insertData(a1.Groups[1].Value, a2.Groups[1].Value, a3.Groups[1].Value, a4.Groups[1].Value);
                                    }
                                    if (a4.Groups[1].Value.Length > 500)
                                    {
                                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                        listViewItem.SubItems.Add(a1.Groups[1].Value);
                                        listViewItem.SubItems.Add(a2.Groups[1].Value);
                                        listViewItem.SubItems.Add(a3.Groups[1].Value);
                                        listViewItem.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<(?!img|p|/p)[^>]*>", "").Trim());
                                        listViewItem.SubItems.Add(keyword);
                                        listViewItem.SubItems.Add(URL);

                                        exportTXT(keyword,a1.Groups[1].Value, Regex.Replace(a4.Groups[1].Value, "<(?!img|p|/p)[^>]*>", "").Trim());
                                        while (this.zanting == false)
                                        {
                                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                        }
                                        if (tingzhi == false)
                                        {
                                            return;
                                        }
                                        Thread.Sleep(100);
                                        //    }
                                    }

                                }

                                else

                                {
                                    break;
                                }
                            }

                        }
                    }
                }
                status1 = true;
            }




            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #endregion

        #region  搜狗获取
        public void sougou()
        {
            try
            {

                status2 = false;
                string[] keywords = textBox5.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string key in keywords)

                {
                    string[] keyword1 = key.Split(new string[] { "," }, StringSplitOptions.None);

                    foreach (string keyword in keyword1)
                    {
                        textBox11.Text += "正在抓取搜狗搜索" + keyword + "\r\n"; ;
                        for (int i = 0; i < 99; i = i + 1)
                        {

                            string url = "https://news.sogou.com/news?mode=1&media=&query=site%3Asohu.com " + keyword + "&time=0&clusterId=&sort=1&page=" + i + "&p=42230305&dp=1";
                            string html = GetUrlwithIP(url, IP,"gbk");

                            MatchCollection urls = Regex.Matches(html, @"<h3 class=""vrTitle"">([\s\S]*?)<a href=""([\s\S]*?)""");


                            if (urls.Count == 0)
                                break;

                            for (int j = 0; j < urls.Count; j++)
                            {

                                if (!sougous.Contains(urls[j].Groups[2].Value))
                                {
                                    sougous.Add(urls[j].Groups[2].Value);

                                    string strhtml = GetUrlwithIP(urls[j].Groups[2].Value,IP, "utf-8");


                                    Match a1 = Regex.Match(strhtml, @"<title>([\s\S]*?)</title>");
                                    Match a2 = Regex.Match(strhtml, @"dateUpdate"" content=""([\s\S]*?)""");
                                    Match a3 = Regex.Match(strhtml, @"mediaid"" content=""([\s\S]*?)""");
                                    Match a4 = Regex.Match(strhtml, @"<p data-role=""original-title"" style=""display:none"">([\s\S]*?)</article>");

                                    //DateTime dt = Convert.ToDateTime(a2.Groups[1].Value);
                                    //if (dateTimePicker1.Value < dt && dt < dateTimePicker2.Value)
                                    //{

                                    if (a1.Groups[1].Value=="")
                                    {
                                        getIp();                         
                                    }
                                    if (checkBox1.Checked == true)
                                    {
                                        insertData(a1.Groups[1].Value, a2.Groups[1].Value, a3.Groups[1].Value, a4.Groups[1].Value);
                                    }

                                    if (a4.Groups[1].Value.Length > 500)
                                    {
                                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                        listViewItem.SubItems.Add(a1.Groups[1].Value);
                                        listViewItem.SubItems.Add(a2.Groups[1].Value);
                                        listViewItem.SubItems.Add(a3.Groups[1].Value);
                                        listViewItem.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<(?!img|p|/p)[^>]*>", "").Replace("<!-- 政务处理 -->", "").Trim());
                                        listViewItem.SubItems.Add(keyword);
                                        listViewItem.SubItems.Add(urls[j].Groups[2].Value);

                                        exportTXT(keyword, a1.Groups[1].Value, Regex.Replace(a4.Groups[1].Value, "<(?!img|p|/p)[^>]*>", "").Replace("<!-- 政务处理 -->", "").Trim());
                                        while (this.zanting == false)
                                        {
                                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                        }
                                        if (tingzhi == false)
                                        {
                                            return;
                                        }
                                        Thread.Sleep(Convert.ToInt32(textBox15.Text));

                                        //    }
                                    }

                                }

                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                status2 = true;
            }




            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #endregion

        
        #region  360获取
        public void so360()
        {
            try
            {

                status3 = false;
                string[] keywords = textBox5.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string key in keywords)

                {
                    string[] keyword1 = key.Split(new string[] { "," }, StringSplitOptions.None);

                    foreach (string keyword in keyword1)
                    {
                        textBox11.Text += "正在抓取360搜索" + keyword + "\r\n"; 
                        for (int i = 0; i < 99; i = i + 1)
                        {

                            string url = "https://news.sogou.com/news?mode=1&media=&query=site:qq.com " + keyword + "&time=0&clusterId=&sort=1&page=" + i + "&p=42230305&dp=1";
                            string html = GetUrlwithIP(url,IP, "gbk");

                            MatchCollection urls = Regex.Matches(html, @"<h3 class=""vrTitle"">([\s\S]*?)<a href=""([\s\S]*?)""");


                            if (urls.Count == 0)
                                break;

                            for (int j = 0; j < urls.Count; j++)
                            {
                                if (!so360s.Contains(urls[j].Groups[2].Value))
                                {
                                    so360s.Add(urls[j].Groups[2].Value);

                                    string strhtml = GetUrlwithIP(urls[j].Groups[2].Value,IP, "gb2312");



                                    Match a1 = Regex.Match(strhtml, @"<title>([\s\S]*?)_");
                                    Match a2 = Regex.Match(strhtml, @"pubtime:'([\s\S]*?)'");
                                    Match a3 = Regex.Match(strhtml, @"jgname"">([\s\S]*?)</span>");
                                    Match a4 = Regex.Match(strhtml, @"<div class=""content-article"">([\s\S]*?)<div id=""Status""></div>");

                                    //DateTime dt = Convert.ToDateTime(a2.Groups[1].Value);
                                    //if (dateTimePicker1.Value < dt && dt < dateTimePicker2.Value)
                                    //{
                                    if (checkBox1.Checked == true)
                                    {
                                        insertData(a1.Groups[1].Value, a2.Groups[1].Value, a3.Groups[1].Value, a4.Groups[1].Value);
                                    }

                                    if (a4.Groups[1].Value.Length > 500)
                                    {
                                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                        listViewItem.SubItems.Add(a1.Groups[1].Value);
                                        listViewItem.SubItems.Add(a2.Groups[1].Value);
                                        listViewItem.SubItems.Add(a3.Groups[1].Value);
                                        listViewItem.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<(?!img|p|/p)[^>]*>", "").Trim());
                                        listViewItem.SubItems.Add(keyword);
                                        listViewItem.SubItems.Add(urls[j].Groups[2].Value);
                                        exportTXT(keyword, a1.Groups[1].Value, Regex.Replace(a4.Groups[1].Value, "<(?!img|p|/p)[^>]*>", "").Trim());
                                        while (this.zanting == false)
                                        {
                                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                        }
                                        if (tingzhi == false)
                                        {
                                            return;
                                        }
                                        Thread.Sleep(Convert.ToInt32(textBox15.Text));
                                        //    }
                                    }
                                }
                                else
                                {
                                    break;
                                }

                            }
                        }
                    }
                }
                status3 = true;
            }




            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #endregion

        #region  必应获取
        public void biying()
        {
            try
            {


                status4 = false;
                string[] keywords = textBox5.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string key in keywords)

                {
                    string[] keyword1 = key.Split(new string[] { "," }, StringSplitOptions.None);

                    foreach (string keyword in keyword1)
                    {
                        textBox11.Text += "正在抓取必应搜索" + keyword + "\r\n"; ;
                        for (int i = 0; i < 99; i = i + 1)
                        {

                            string url = "https://news.sogou.com/news?mode=1&media=&query=site:sina.com.cn " + keyword + "&time=0&clusterId=&sort=1&page=" + i + "&p=42230305&dp=1";
                            string html = GetUrlwithIP(url,IP, "gbk");

                            MatchCollection urls = Regex.Matches(html, @"<h3 class=""vrTitle"">([\s\S]*?)<a href=""([\s\S]*?)""");


                            if (urls.Count == 0)
                                break;

                            for (int j = 0; j < urls.Count; j++)
                            {
                                if (!biyings.Contains(urls[j].Groups[2].Value))
                                {
                                    biyings.Add(urls[j].Groups[2].Value);

                                    string strhtml = GetUrlwithIP(urls[j].Groups[2].Value,IP, "utf-8");



                                    Match a1 = Regex.Match(strhtml, @"<title>([\s\S]*?)_");
                                    Match a2 = Regex.Match(strhtml, @"<span class=""date"">([\s\S]*?)</span>");
                                    Match a3 = Regex.Match(strhtml, @"rel=""nofollow"">([\s\S]*?)</a>");
                                    Match a4 = Regex.Match(strhtml, @"<!-- 正文 start -->([\s\S]*?)<!-- 正文 end -->");


                                    if (checkBox1.Checked == true)
                                    {
                                        insertData(a1.Groups[1].Value, a2.Groups[1].Value, a3.Groups[1].Value, a4.Groups[1].Value);
                                    }

                                    if (a4.Groups[1].Value.Length > 500)
                                    {
                                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                        listViewItem.SubItems.Add(a1.Groups[1].Value);
                                        listViewItem.SubItems.Add(a2.Groups[1].Value);
                                        listViewItem.SubItems.Add(a3.Groups[1].Value);
                                        listViewItem.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<(?!img|p|/p)[^>]*>", "").Trim());
                                        listViewItem.SubItems.Add(keyword);
                                        listViewItem.SubItems.Add(urls[j].Groups[2].Value);

                                        exportTXT(keyword, a1.Groups[1].Value, Regex.Replace(a4.Groups[1].Value, "<(?!img|p|/p)[^>]*>", "").Trim());

                                        while (this.zanting == false)
                                        {
                                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                        }
                                        if (tingzhi == false)
                                        {
                                            return;
                                        }

                                        Thread.Sleep(Convert.ToInt32(textBox15.Text));
                                        //    }
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }


                    }
                }
                status4 = true;
            }




            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #endregion

        #region  代理iP

        public void getIp()
        {
            string ahtml = method.GetUrl(textBox14.Text, "utf-8");
            this.IP = ahtml.Trim();

        }
        #endregion

        /// <summary>
        /// 判断相似度
        /// </summary>
        /// <returns></returns>
        public bool panduan()
        {
            return false;
        }
        private void Main_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox14.Text == "")
            {
                MessageBox.Show("请输入代理IP地址");
                return;
            }

            tingzhi = true;
            button1.Enabled = false;
            timer1.Start();
            getIp();


            if (checkedListBox1.GetItemChecked(0) == true)
                {
                    Thread thread = new Thread(new ThreadStart(baidu));
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
                if (checkedListBox1.GetItemChecked(1) == true)
                {
                    Thread thread = new Thread(new ThreadStart(sougou));
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
                if (checkedListBox1.GetItemChecked(2) == true)
                {
                    Thread thread = new Thread(new ThreadStart(so360));
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
                if (checkedListBox1.GetItemChecked(3) == true)
                {
                    Thread thread = new Thread(new ThreadStart(biying));
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
              
            

            
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
             
        }


        public void export()
        {
            if (status1 == true && status2 == true && status3 == true && status4 == true)
            {

                timer1.Stop();
                string[] keywords = textBox5.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string key in keywords)

                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + key + "\\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path); //创建文件夹
                    }
                    string filename = path + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Trim() + ".xlsx";

                    method.DataTableToExcelTime(method.listViewToDataTableSx(this.listView1, key), "Sheet1", true, filename);
                }

                textBox11.Text += "数据导出完成";
            }

       }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            export();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            zanting = true;
            button1.Enabled = true;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Process.Start(this.listView1.SelectedItems[0].SubItems[6].Text);
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tingzhi = false;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
