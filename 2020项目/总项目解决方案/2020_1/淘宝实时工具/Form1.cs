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

namespace 淘宝实时工具
{
    public partial class Form1 : Form
    {
        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlWithCookie(string Url, string COOKIE, string charset)
        {
            try
            {
                // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Referer = "https://sycm.taobao.com/ipoll/visitor.htm?spm=a21ag.7623863.LeftMenu.d181.2955d27bxx8vL5";
                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
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

        public static string cookie;
        public Form1()
        {
            InitializeComponent();
        }
        ArrayList titles = new ArrayList();
     
        private void openFm2(object sender, EventArgs e)
        {
            Form2 fm2 = new Form2();
            fm2.Show();
            Form2.itemId = textBox1.Text.Trim();
        }
        public string getToken()
        {
            string url = "https://sycm.taobao.com/ipoll/visitor.htm";
            string html = GetUrlWithCookie(url, cookie, "utf-8");
            Match token = Regex.Match(html, @"legalityToken=([\s\S]*?);");
            return token.Groups[1].Value;

        }

        #region  主程序
        public void run()
        {
            string src = System.Web.HttpUtility.UrlEncode(comboBox2.Text);
            if (comboBox2.Text == "全部")
            {
                src = "";
            }

            for (int i = 1; i < 99; i++)
            {

                string url = "https://sycm.taobao.com/ipoll/live/visitor/getRtVisitor.json?_=" + time + "&device=2&limit=20&itemid=&page=" + i + "&srcgrpname=" + src + "&token=" + token + "&type=Y";

               
                string html = GetUrlWithCookie(url, cookie, "utf-8");

                
                string[] ahtml = html.Split(new string[] { "\"},{\"" }, StringSplitOptions.None);


                if (ahtml.Length < 2)
                {
                   
                    return;

                }

                for (int j = 0; j < ahtml.Length; j++)
                {


                    try
                    {


                        Match laiyuan = Regex.Match(ahtml[j] + "\"", @"srcGrpName"":""([\s\S]*?)""");
                        Match time = Regex.Match(ahtml[j] + "\"", @"visitTime"":""([\s\S]*?)""");
                        Match key = Regex.Match(ahtml[j] + "\"", @"preSeKeyword"":""([\s\S]*?)""");
                        Match oid = Regex.Match(ahtml[j] + "\"", @"oid"":""([\s\S]*?)""");
                        Match titleUrl= Regex.Match(ahtml[j] + "\"", @"detailUrl"":""([\s\S]*?)""");

                        titles.Add("https:"+titleUrl.Groups[1].Value.Replace("\\",""));
                       

                        DateTime nowtime = DateTime.Now;
                        DateTime time1 = Convert.ToDateTime(time.Groups[1].Value);
                        TimeSpan timeSpan = nowtime - time1;
                        if (timeSpan.TotalMinutes <= minutes)
                        {

                            //if (!oids.Contains(oid.Groups[1].Value))
                            //{
                            //    if (checkBox1.Checked == true)
                            //    {
                            //        oids.Add(oid.Groups[1].Value);
                            //    }



                            if (laiyuan.Groups[1].Value == "")
                                {
                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                    lv1.SubItems.Add(time.Groups[1].Value);
                                    lv1.SubItems.Add("其他来源");
                                    lv1.SubItems.Add(oid.Groups[1].Value);
                                }
                                else
                                {
                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                    lv1.SubItems.Add(time.Groups[1].Value);
                                    lv1.SubItems.Add(laiyuan.Groups[1].Value + key.Groups[1].Value);
                                    lv1.SubItems.Add(oid.Groups[1].Value);
                                }
                            }
                      //  }
                    }
                    catch
                    {

                        continue;
                    }

                }

                Thread.Sleep(1000);
            }

           
           

        }

        #endregion
        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
        ArrayList oids = new ArrayList();
        string token = "";
        int minutes = 9999999;
        /// <summary>
        /// 获取时间戳毫秒
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalMilliseconds);
            return a.ToString();
        }
        public string time;
        private void Form1_Load(object sender, EventArgs e)
        {
          
           
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = "";
            listView1.Items.Clear();
          
            token = getToken();
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
               
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

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
            
            token = getToken();
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            timer1.Start();
            timer1.Interval = 600000;
            minutes = 99999999;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
            
            token = getToken();
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            timer1.Start();
            timer1.Interval = 600000;
            minutes = 10;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
            
            token = getToken();
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            timer1.Start();
            timer1.Interval = 1800000;
            minutes = 30;
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
            
            token = getToken();
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            timer1.Start();
            timer1.Interval = 3600000;
            minutes = 60;
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (titles.Count > 4)
            {
                textBox2.Text = titles[0].ToString();
                textBox3.Text = titles[1].ToString();
                textBox4.Text = titles[2].ToString();
                textBox5.Text = titles[3].ToString();
                textBox6.Text = titles[4].ToString();
            }
        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (titles.Count > 4)
            {
                textBox2.Text = titles[4].ToString();
                textBox3.Text = titles[3].ToString();
                textBox4.Text = titles[2].ToString();
                textBox5.Text = titles[0].ToString();
                textBox6.Text = titles[1].ToString();
            }
        }

        private void linkLabel11_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (titles.Count > 4)
            {
                textBox2.Text = titles[3].ToString();
                textBox3.Text = titles[4].ToString();
                textBox4.Text = titles[1].ToString();
                textBox5.Text = titles[2].ToString();
                textBox6.Text = titles[0].ToString();
            }
        }

        private void linkLabel12_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (titles.Count > 4)
            {
                textBox2.Text = titles[4].ToString();
                textBox3.Text = titles[3].ToString();
                textBox4.Text = titles[2].ToString();
                textBox5.Text = titles[0].ToString();
                textBox6.Text = titles[1].ToString();
            }
        }

        private void linkLabel13_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (titles.Count > 4)
            {
                textBox2.Text = titles[0].ToString();
                textBox3.Text = titles[1].ToString();
                textBox4.Text = titles[2].ToString();
                textBox5.Text = titles[3].ToString();
                textBox6.Text = titles[4].ToString();
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}
