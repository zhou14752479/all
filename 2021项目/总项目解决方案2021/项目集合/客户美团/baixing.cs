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
        public void run()
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

       



        bool status = true;
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html","utf-8");

            if (!html.Contains(@"YVoWQ"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion
            #region 通用检测

            //string ahtml = method.GetUrl("http://139.129.92.113/","utf-8");

            //if (!ahtml.Contains(@"siyisoft"))
            //{

            //    return;
            //}



            #endregion
            status = true;
           
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
            textBox1.Text += comboBox2.Text + "\r\n";
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
            ProvinceCity.ProvinceCity.BindCity(comboBox1, comboBox2);
        }
    }
}
