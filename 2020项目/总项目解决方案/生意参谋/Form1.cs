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

namespace 生意参谋
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
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

        private void Form1_Load(object sender, EventArgs e)
        {
            time = GetTimeStamp();
            webBrowser1.Navigate("https://login.taobao.com/member/login.jhtml");
            method.SetWebBrowserFeatures(method.IeVersion.IE10);
            webBrowser1.ScriptErrorsSuppressed = true;
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
        public static string cookie;
        public string time;

        public string getToken()
        {
            string url = "https://sycm.taobao.com/ipoll/visitor.htm";
            string html = GetUrlWithCookie(url,cookie,"utf-8");
            Match token= Regex.Match(html, @"legalityToken=([\s\S]*?);");
            return token.Groups[1].Value;

        }
        ArrayList oids = new ArrayList();
        string token = "";
        public void run()
        {
            string  src= System.Web.HttpUtility.UrlEncode(comboBox1.Text);
            if (comboBox1.Text == "全部")
            {
                src = "";
            }

            for (int i = 1; i < 99; i++)
            {
                
                string url = "https://sycm.taobao.com/ipoll/live/visitor/getRtVisitor.json?_=" + time + "&device=2&limit=20&page="+i+"&srcgrpname=" + src + "&token=" + token + "&type=Y";
               

                string html = GetUrlWithCookie(url, cookie, "utf-8");
                

            string[] ahtml = html.Split(new string[] { "\"},{\"" }, StringSplitOptions.None);


                if (ahtml.Length < 2)
                
                    return;
                
               

                for (int j = 0; j < ahtml.Length; j++)
                {


                    try
                    {

   
                    Match laiyuan = Regex.Match(ahtml[j]+"\"", @"srcGrpName"":""([\s\S]*?)""");
                    Match time = Regex.Match(ahtml[j] + "\"", @"visitTime"":""([\s\S]*?)""");
                    Match key = Regex.Match(ahtml[j] + "\"", @"preSeKeyword"":""([\s\S]*?)""");
                    Match oid = Regex.Match(ahtml[j] + "\"", @"oid"":""([\s\S]*?)""");

                    



                    DateTime nowtime = DateTime.Now;
                    DateTime time1 = Convert.ToDateTime(time.Groups[1].Value);
                    TimeSpan timeSpan = nowtime - time1;
                    if (timeSpan.TotalMinutes <= minutes)
                    {

                        if (!oids.Contains(oid.Groups[1].Value))
                        {
                            if (checkBox1.Checked == true)
                            {
                                oids.Add(oid.Groups[1].Value);
                            }



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
                    }
                    }
                    catch 
                    {

                        continue;
                    }

                }

                Thread.Sleep(1000);
            }
            

        }


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
                textBox1.Text += item.Key + " " + item.Value + "\r\n";

            }
        }
        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        int minutes = 9999999;

        private void Button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            if (radioButton1.Checked == true)
            {
                timer1.Interval = 600000;
                minutes = 10;
            }

            if (radioButton2.Checked == true)
            {
                timer1.Interval = 1800000;
                minutes = 30;
            }

            if (radioButton3.Checked == true)
            {
                timer1.Interval = 3600000;
                minutes = 60;
            }
            if (radioButton4.Checked == true)
            {
                timer1.Interval = 600000;
                minutes = 99999999;
            }


            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"shengyicanmou"))
            {
                
            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion

            cookie = method.GetCookies("https://sycm.taobao.com/ipoll/visitor.htm?spm=a21ag.7622617.LeftMenu.d181.1fde1be9h1T7Pt#/");
            listView1.Items.Clear();
            textBox1.Text = "";
            token = getToken();
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            timer1.Start();



        }

        private void Button2_Click(object sender, EventArgs e)
        {
           
            
        }


     
        private void Timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = "";
            listView1.Items.Clear();
            cookie = method.GetCookies("https://sycm.taobao.com/ipoll/live/visitor/getRtVisitor.json?_=1585793637702&device=2&limit=20&page=1&token=");
            token = getToken();
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            button1.Enabled = true;
           

            timer1.Stop();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            tongji();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
