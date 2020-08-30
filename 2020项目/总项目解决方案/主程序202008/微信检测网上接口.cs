using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 主程序202008
{
    public partial class 微信检测网上接口 : Form
    {
        public const int MB_ICONEXCLAMATION = 48;

        [DllImport("user32.dll")]
        public static extern bool MessageBeep(uint uType);

        public 微信检测网上接口()
        {
            InitializeComponent();
        }

        ArrayList lists = new ArrayList();
        ArrayList bflists = new ArrayList();

        SpeechSynthesizer ss = new SpeechSynthesizer();


        public string getIp()
        {

            string url = "http://47.106.170.4:8081/Index-generate_api_url.html?packid=3&fa=0&groupid=0&fetch_key=&qty=1&port=1&format=txt&ss=1&css=&pro=&city=&usertype=7";
            return GetUrl(url,"utf-8");
        }

        #region GET使用代理IP请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlwithIP(string Url, string ip)
        {
            try
            {

                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";
                WebProxy proxy = new WebProxy(ip);
                request.Proxy = proxy;

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 3000;
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

        string ip = "";
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "被封域名.txt";
           

                foreach (string item in lists)
                {

                    if (item != "")
                    {
                    string url = "http://mp.weixinbridge.com/mp/wapredirect?url=http://" + item + "&action=appmsg_redirect&uin=&biz=MzUxMTMxODc2MQ==&mid=100000007&idx=1&type=1&scene=0";
                     string html = GetUrlwithIP(url,ip);

                  


                    if (html.Contains("如需浏览") || html.Contains("停止访问") || html=="")
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(item);
                        lv1.SubItems.Add("域名被封");
                        textBox1.Text.Replace(item, "");

                        ss.SpeakAsync("域名被封");
                        using (StreamWriter fs = new StreamWriter(path, true))
                        {
                            fs.WriteLine(item);
                        }
                        bflists.Add(item);
                    }
                    else
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(item);
                        lv1.SubItems.Add("域名正常");

                    }

                     }
                

                   
                    Thread.Sleep(600);
                }

            



        }

        
        private void 微信检测网上接口_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {

                    textBox1.Text += text[i] + "\r\n";


                }
            }
        }

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
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

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

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"wxyumingjiance"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion
            if (textBox1.Text != "")
            {
                string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {
                    lists.Add(text[i]);
                }
            }
            button1.Enabled = false;
            ip = getIp();
            timer1.Start();
            timer2.Start();
            timer1.Interval = Convert.ToInt32(textBox2.Text)*60*1000;
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            listView1.Items.Clear();

            
        }

      

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            button1.Enabled = true;
            timer2.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (string item in bflists)
            {
                lists.Remove(item);
            }
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            ip = getIp();
        }
    }
}
