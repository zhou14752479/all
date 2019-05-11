using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zhaopin_58
{
    public partial class 验证码登陆 : Form
    {
        public 验证码登陆()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false; // 设置线程之间可以操作
        }

        public void run()
        {
            //string url = "http://222.187.200.202:8001/Account/Login?examid=00181228141531";
            //string postData="PINType=1&PIN=321321199303012258&Password=zhoukaige00&ValidateCode="+textBox1.Text.Trim();
            //string COOKIE = "UM_distinctid=1687983485214-0b85b9ef36aeea-5d1f3b1c-100200-1687983485718e; ASP.NET_SessionId=hmis5x2bzdkc3ibcjjauft5e; CNZZDATA5018238=cnzz_eid%3D2032971112-1548224314-http%253A%252F%252Fsqhrss.suqian.gov.cn%252F%26ntime%3D1548912663; __RequestVerificationToken_Lw__=wpXnc9DQkmF7pL6D5GxENojtj/Jgjal3iyhFlPW4WqhYE6G84wTMpNIt7gvJW891CR9iCHwHqLhz1TVfwJcRzz7uVpaX1JR2usFnXg49+cM=";
            //string charset = "utf-8";

            //textBox2.Text = PostUrl( url, postData, COOKIE, charset);

            textBox2.Text = getUrl("https://mall.jd.com/showLicence-1000074101.html?verifyCode="+textBox1.Text.Trim());
        }



        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;

            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.112 Safari/537.36";
            request.Headers.Add("Cookie", COOKIE);

            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(postData);
            sw.Flush();


            WebResponse response = request.GetResponse();
            Stream s = response.GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.GetEncoding(charset));
            string html = sr.ReadToEnd();

            sw.Dispose();
            sw.Close();
            sr.Dispose();
            sr.Close();
            s.Dispose();
            s.Close();
            return html;
        }

        #endregion

        #region GET请求
        public static string getUrl(string url)
        {

            StreamReader reader = new StreamReader(getStream(url), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

            string content = reader.ReadToEnd();
            return content;

        }
        #endregion

        #region 获取数据流
        public static Stream getStream(string Url)
        {

            string COOKIE = "shshshfpa=1325c19d-7317-26c8-e006-872a4999a27c-1526910507; shshshfpb=04f1e443ad4a14669b330d37d1758accda7f9d43dafa36bed5b0a7393d; __jdu=1746186544; user-key=ac09ee9c-3a85-4f62-b422-bd2f5436c54c; cn=2; PCSYCityID=933; __jda=122270672.1746186544.1539856564.1557561034.1557561069.9; __jdv=122270672|baidu|-|organic|not set|1557561069043; areaId=12; __jdc=122270672; ipLoc-djd=12-933-3407-40385; mt_xid=V2_52007VwMUV1pYW10bTBxsDGFRRlQPXgdGT0ERDxliUxtUQVBUXR9VEFhSb1QaAFsNUw4deRpdBW8fE1ZBWFJLH04SWA1sARBiXWhSahpNGlUCYQMWVm1aVlsb; _gcl_au=1.1.2120468405.1557565190; shshshfp=3d9d5b5ee4df1d426f7790085e66ff99; 3AB9D23F7A4B3C9B=W6QBEF5Y5EEHHOXWYSZC2W573XDNDCKCNKNFR2WTIFWNLSYRADQCTJY3QJNDM2XXCICM7SW4MLBVF7SE2WASP7S7CU; shshshsID=71cf11e2b286360f93326c3a6f5654ec_69_1557566178401; _jshop_vd_=NTZWCV6WKKGBA; __jdb=122270672.97.1746186544|9.1557561069; JSESSIONID=011256AB80718214DBBA8CC218158A96.s1";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
            request.Timeout = 10000;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.81 Safari/537.36";
            request.AllowAutoRedirect = true;
            request.Headers.Add("Cookie", COOKIE);
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
            return response.GetResponseStream();

        }
        #endregion

        private void 验证码登陆_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromStream(getStream("https://mall.jd.com/sys/vc/createVerifyCode.html?random=0.7105247244422384"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Ocr ocr = new Ocr();
            textBox1.Text = ocr.OCR_sougou(this.pictureBox1.Image).Replace(".",""); 
        }

        private void button2_Click(object sender, EventArgs e)
        {

            pictureBox1.Image = Image.FromStream(getStream("https://mall.jd.com/sys/vc/createVerifyCode.html?random=0.7105247244422384"));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromStream(getStream("https://mall.jd.com/sys/vc/createVerifyCode.html?random=0.7105247244422384"));
            Ocr ocr = new Ocr();
            textBox1.Text = ocr.OCR_sougou(this.pictureBox1.Image).Replace(".", "");
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();

        }







    }
}
