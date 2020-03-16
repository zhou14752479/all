using System;
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


namespace 启动程序
{
    public partial class 奇趣分分彩 : Form
    {
        public 奇趣分分彩()
        {
            InitializeComponent();
        }

        


        

        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlWithCookie(string Url, string COOKIE)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                
                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
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

        public static string cookie="";


        #region  分分彩
        public void ffc(string cai)

        {
           
            string url = "http://wawa.xinjy01.com/ct-data/openCodeList?shortName="+cai+"&num=50";

            string html = GetUrlWithCookie(url, cookie);
            
            MatchCollection qishus = Regex.Matches(html, @"""expect"":""([\s\S]*?)""");
           
            MatchCollection times = Regex.Matches(html, @"""openTime"":""([\s\S]*?)""");
            MatchCollection results = Regex.Matches(html, @"""openCode"":""([\s\S]*?)""");

            for (int j = 0; j < qishus.Count; j++)
            {
                ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count).ToString()); //使用Listview展示数据   
                lv2.SubItems.Add(qishus[j].Groups[1].Value);
               
                lv2.SubItems.Add(times[j].Groups[1].Value);
                lv2.SubItems.Add(results[j].Groups[1].Value);


            }

          
        }

        #endregion

        private void 奇趣分分彩_Load(object sender, EventArgs e)
        {
           
        }

      
       
        private void Button1_Click(object sender, EventArgs e)
        {

            helper.Form1 fm1 = new helper.Form1();
            fm1.Show();
        }

        public void run()

        {
            string url1 = "http://wawa.xinjy01.com/ct-data/openCodeList?shortName=qqtxffc&num=1";
            string url2 = "http://wawa.xinjy01.com/ct-data/openCodeList?shortName=qqtxsfc&num=1";
            string url3 = "http://wawa.xinjy01.com/ct-data/openCodeList?shortName=qqtxwfc&num=1";
            string url4 = "http://wawa.xinjy01.com/ct-data/openCodeList?shortName=qqtxysfc&num=1";

            string html1= GetUrlWithCookie(url1, cookie);
            string html2 = GetUrlWithCookie(url2, cookie);
            string html3 = GetUrlWithCookie(url3, cookie);
            string html4 = GetUrlWithCookie(url4, cookie);

            Match result1 = Regex.Match(html1, @"""openCode"":""([\s\S]*?)""");
            Match result2 = Regex.Match(html2, @"""openCode"":""([\s\S]*?)""");
            Match result3 = Regex.Match(html3, @"""openCode"":""([\s\S]*?)""");
            Match result4 = Regex.Match(html4, @"""openCode"":""([\s\S]*?)""");


            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   

            string r1 = result1.Groups[1].Value.Trim();
            string r2 = result2.Groups[1].Value.Trim();
            string r3= result3.Groups[1].Value.Trim();
            string r4 = result4.Groups[1].Value.Trim();
            lv1.SubItems.Add(r1);
            lv1.SubItems.Add(r2);
            lv1.SubItems.Add(r3);
            lv1.SubItems.Add(r4);
            textBox1.Text = r1;
            textBox2.Text = r2;
            textBox3.Text = r3;
            textBox4.Text = r4;

            textBox1.ForeColor = Color.Black;
            textBox2.ForeColor = Color.Black;
            textBox3.ForeColor = Color.Black;
            textBox4.ForeColor = Color.Black;


            if (r1 == r2)
            {
                textBox1.ForeColor= Color.Red;
                textBox2.ForeColor = Color.Red;

            }
            if (r1 == r3)
            {

                textBox1.ForeColor = Color.Red;
                textBox3.ForeColor = Color.Red;
            }
            if (r1 == r4)
            {

                textBox1.ForeColor = Color.Red;
                textBox4.ForeColor = Color.Red;
            }
            if (r2== r3)
            {

                textBox2.ForeColor = Color.Red;
                textBox3.ForeColor = Color.Red;
            }
            if (r2 == r4)
            {

                textBox2.ForeColor = Color.Red;
                textBox4.ForeColor = Color.Red;
            }
            if (r3 == r4)
            {

                textBox3.ForeColor = Color.Red;
                textBox4.ForeColor = Color.Red;
            }



        }

        private void Button6_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"qqtxffc"))
            {
                MessageBox.Show("监控已开启");
                cookie = helper.Form1.cookie;


                timer1.Start();



            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion

           
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            listView1.Items[1].SubItems[1].BackColor = Color.Red;
            listView2.Items.Clear();
            cookie = helper.Form1.cookie;

            ffc("qqtxffc");
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            cookie = helper.Form1.cookie;
            ffc("qqtxsfc");
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            cookie = helper.Form1.cookie;
            ffc("qqtxwfc");
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            cookie = helper.Form1.cookie;
            ffc("qqtxysfc");
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            cookie = helper.Form1.cookie;
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("监控已关闭");
            timer1.Stop();
        }
    }
}
