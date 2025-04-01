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
using CsharpHttpHelper;
using myDLL;

namespace 客户美团
{
    public partial class baixing : Form
    {
        public baixing()
        {
            InitializeComponent();
        }

       


        #region GET请求
        public string GetUrl(string Url,string cookie,string city)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = Url,
                Method = "GET",
                Accept = "*/*",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.85 Safari/537.36",
                Referer = "https://"+city+".baixing.com/ershoufang/?grfy=1&page=1",
                Host = city+".baixing.com",
                Cookie = cookie,
            };
            item.Header.Add("Accept-Encoding", "gzip");
            item.Header.Add("Accept-Language", "zh-cn,zh,en");
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            return html;



        }
        #endregion

        public string getpinyin(string city)
        {
            string url = "https://www.baixing.com/autocomplete/city";

            string html = method.PostUrl(url, "q="+ System.Web.HttpUtility.UrlEncode(city.Replace("市","")) + "&fuzzy=1", "","utf-8", "application/x-www-form-urlencoded", "");
            string englishName = Regex.Match(html, @"""englishName"":""([\s\S]*?)""").Groups[1].Value;
            return englishName;
        }

        /// <summary>
        /// 主程序
        /// </summary>
        public void run2()
        {
          

            int count = 0;
            try
            {

                string[] citys= textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                foreach (string city in citys)
                {
                    string citypinyin = getpinyin(city);
                    string cookie = "suid=1197640040; __trackId=4526158735018318; Hm_lvt_5a727f1b4acc5725516637e03b07d3d2=1647486825; __city=shanghai; __s=9em7cn2b2r9t2f8qhe2cajntg2; bxf=4b26fc601742a78be16c4ed646177b93054; __sense_session_pv=11; __admx_track_id=styJ6bnbafGdtnpcESo38Q; __admx_track_id.sig=wku-c3nXgAjjaRgOJemQNFsdSnU; Hm_lpvt_5a727f1b4acc5725516637e03b07d3d2=1647486840";
                    for (int page = 1; page < 100; page++)
                    {
                       
                        string url = "https://" + citypinyin + ".baixing.com/ershoufang/?grfy=1&page=" + page;
                      
                        string html = GetUrl(url,cookie,citypinyin);

                        MatchCollection aids = Regex.Matches(html, @"aid='([\s\S]*?)'");

                       // MessageBox.Show(aids.Count.ToString());
                        if (aids.Count == 0)
                            break;

                        for (int i = 0; i < aids.Count; i++)
                        {
                            string aurl = "https://"+ citypinyin + ".baixing.com/ershoufang/a" + aids[i].Groups[1].Value + ".html?from=regular";
                            string ahtml = GetUrl(aurl,cookie, citypinyin);
                            string title = Regex.Match(ahtml, @"<h1>([\s\S]*?)</h1>").Groups[1].Value;
                         
                            string lxr = Regex.Match(ahtml, @"class='poster-name'>([\s\S]*?)<").Groups[1].Value;
                            string tel = Regex.Match(ahtml, @"<strong>([\s\S]*?)</strong>").Groups[1].Value;

                            if (title != "")
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(title);
                                lv1.SubItems.Add(city);
                                lv1.SubItems.Add(lxr);
                                lv1.SubItems.Add(tel);
                                lv1.SubItems.Add(aurl);
                            }
                            if (status == false)
                                return;
                            Thread.Sleep(500);
                            count = count + 1;
                            label4.Text = count.ToString();
                        }

                        Thread.Sleep(1000);
                    }
                }
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }




        #region 获取token
        public static string gettoken()
        {
            string url = "http://api.yunmai.vip:686/?id=pc_user&action=api_login&username=18627986383&password=18627986383&sn=EE3116635693C0C4&version=4.1.8&shiyong=1 ";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 10000;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
            request.AllowAutoRedirect = false;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            return response.GetResponseHeader("token");
        }
        #endregion
        bool status = true;
        string token = "";
        Thread thread;

        private void Form1_Load(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindProvince(comboBox1);
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {

            }

        }
        string cookie = "";
        #region GET请求
        public string GetUrl(string Url)
        {
            try
            {


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                string COOKIE = cookie;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.124 Safari/537.36 Edg/102.0.1245.41";
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("token", token);
                //request.Referer = "https://servicewechat.com/wxde8ac0a21135c07d/350/page-frame.html";
                request.Referer = "https://map.tianditu.gov.cn/";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

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


        #region  美团
        public void run()
        {


            string[] keywords = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None); ;
            if (keywords.Length == 0)
            {
                MessageBox.Show("请添加关键字");
                return;
            }

            int count = 0;
            try
            {
                string pro =  Regex.Replace(comboBox1.Text, @"\([^)]*\)", "").Replace("所有", ""); 
                string city= Regex.Replace(comboBox2.Text, @"\([^)]*\)", "").Replace("所有", "");

                foreach (string keyword in keywords)
                {


                    for (int page = 1; page < 1000; page++)
                    {
                        


                        string url = "http://139.129.17.82:6806/?action=api_mtdp&keyword="+keyword+"&page="+page+"&province="+pro.Trim()+"&city="+city.Trim();
                        string html = GetUrl(url);

                        MatchCollection coname = Regex.Matches(html, @"""coname"": ""([\s\S]*?)""");

                        MatchCollection address = Regex.Matches(html, @"""address"": ""([\s\S]*?)""");
                        MatchCollection tel = Regex.Matches(html, @"""tel"": ""([\s\S]*?)""");
                        MatchCollection socpes = Regex.Matches(html, @"""scope"": ""([\s\S]*?)""");
                        MatchCollection phone = Regex.Matches(html, @"""phone"": ""([\s\S]*?)""");
                        MatchCollection score = Regex.Matches(html, @"""score"": ""([\s\S]*?)""");
                        MatchCollection scorechanpin = Regex.Matches(html, @"""scorechanpin"": ""([\s\S]*?)""");
                        MatchCollection scorehuanjing = Regex.Matches(html, @"""scorehuanjing"": ""([\s\S]*?)""");
                        MatchCollection catbig = Regex.Matches(html, @"""catbig"": ""([\s\S]*?)""");

                        if (coname.Count == 0)
                            break;

                        for (int i = 0; i < coname.Count; i++)
                        {




                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                            lv1.SubItems.Add(coname[i].Groups[1].Value);
                            lv1.SubItems.Add(tel[i].Groups[1].Value);
                            lv1.SubItems.Add(phone[i].Groups[1].Value);
                         
                            lv1.SubItems.Add(score[i].Groups[1].Value);
                            lv1.SubItems.Add(scorechanpin[i].Groups[1].Value);
                            lv1.SubItems.Add(scorehuanjing[i].Groups[1].Value);
                            lv1.SubItems.Add(catbig[i].Groups[1].Value);
                            lv1.SubItems.Add(socpes[i].Groups[1].Value);
                            lv1.SubItems.Add(address[i].Groups[1].Value);


                            lv1.SubItems.Add(keyword);
                            if (status == false)
                                return;
                            if (listView1.Items.Count > 2)
                            {
                                this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                            }
                            Thread.Sleep(100);
                            count = count + 1;
                            label4.Text = count.ToString();

                        }

                        Thread.Sleep(1000);
                    }
                }


            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }

        #endregion
        private void button1_Click(object sender, EventArgs e)
        {

          
            
            if (token == "")
            {
                token = gettoken();
            }




            status = true;
            if (textBox1.Text == "")
            {
                MessageBox.Show("行业为空");
                return;
            }

            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            else
            {
                status = false;
            }


        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {



        }







        private void button2_Click(object sender, EventArgs e)
        {
            method.ListviewToTxt(listView1, 4);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Contains(comboBox2.Text))
            {
                MessageBox.Show(comboBox2.Text + "：请勿重复添加", "重复添加错误");
                return;
            }
        
        }


        private Point mPoint = new Point();
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Add("所有");
            ProvinceCity.ProvinceCity.BindCity(comboBox1, comboBox2);
            comboBox2.Items.Add("所有");
            if (comboBox1.Text.Contains("北京"))
            {
                comboBox2.Items.Clear();
                comboBox2.Items.Add("北京市");
                comboBox2.Text = "北京市";
            }
            if (comboBox1.Text.Contains("上海"))
            {
                comboBox2.Items.Clear();
                comboBox2.Items.Add("上海市");
                comboBox2.Text = "上海";
            }
            if (comboBox1.Text.Contains("天津"))
            {
                comboBox2.Items.Clear();
                comboBox2.Items.Add("天津市");
                comboBox2.Text = "天津";
            }
            if (comboBox1.Text.Contains("重庆"))
            {
                comboBox2.Items.Clear();
                comboBox2.Items.Add("重庆市");
                comboBox2.Text = "重庆市";
            }
        }
    }
}
