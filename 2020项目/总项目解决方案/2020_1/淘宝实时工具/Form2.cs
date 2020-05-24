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

namespace 淘宝实时工具
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        string token = "";

        public string getToken()
        {
            string url = "https://sycm.taobao.com/ipoll/visitor.htm";
            string html = GetUrlWithCookie(url, Form1.cookie, "utf-8");
            Match token = Regex.Match(html, @"legalityToken=([\s\S]*?);");
            return token.Groups[1].Value;

        }
        private Point mPoint = new Point();
        public static string itemId;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }
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
        ArrayList titles = new ArrayList();
        int minutes = 9999999;
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

                string url = "https://sycm.taobao.com/ipoll/live/visitor/getRtVisitor.json?_=" + time + "&device=2&limit=20&itemid="+itemId+"&page=" + i + "&srcgrpname=" + src + "&token=" + token + "&type=Y";


                string html = GetUrlWithCookie(url, Form1.cookie, "utf-8");

               
              
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
                        Match titleUrl = Regex.Match(ahtml[j] + "\"", @"detailUrl"":""([\s\S]*?)""");

                        titles.Add("https:" + titleUrl.Groups[1].Value.Replace("\\", ""));


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
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }

        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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

        private void Form2_Load(object sender, EventArgs e)
        {
           
        }



        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
           
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                string url = "https://item.taobao.com/item.htm?id=" + itemId;
                string html = GetUrlWithCookie(url, Form1.cookie, "gbk");
                Match title = Regex.Match(html, @"<title>([\s\S]*?)-");
                textBox1.Text = title.Groups[1].Value;
            }
        }


        ArrayList keysList = new ArrayList();
        ArrayList valuesList = new ArrayList();
        public void tongji()

        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string value = listView1.Items[i].SubItems[2].Text.Trim();
                if (!dic.ContainsKey(value))
                {
                    dic.Add(value, 1);   //1代表只有1个

                }
                else
                {
                    dic[value]++;       //包含了则增加1
                }

            }

            foreach (KeyValuePair<string, int> item in dic)
            {
                keysList.Add(item.Key);
                valuesList.Add(item.Value);
               // textBox1.Text += item.Key + " " + item.Value + "\r\n";
               
            }
        }
        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tongji();

            alabel.Text = keysList[0].ToString();
            blabel.Text = keysList[1].ToString();
            clabel.Text = keysList[2].ToString();
            dlabel.Text = keysList[3].ToString();
            elabel.Text = keysList[4].ToString();

            atextBox1.Text = valuesList[0].ToString();
            btextBox1.Text = valuesList[1].ToString();
            ctextBox1.Text = valuesList[2].ToString();
            dtextBox1.Text = valuesList[3].ToString();
            etextBox1.Text = valuesList[4].ToString();
        }
    }
}
