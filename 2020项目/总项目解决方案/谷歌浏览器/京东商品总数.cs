using CefSharp;
using CefSharp.WinForms;
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

namespace 谷歌浏览器
{
    public partial class 京东商品总数 : Form
    {
        public ChromiumWebBrowser browser;

        public 京东商品总数()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {
            try
            {
                string COOKIE = "shshshfpa=1325c19d-7317-26c8-e006-872a4999a27c-1526910507; shshshfpb=04f1e443ad4a14669b330d37d1758accda7f9d43dafa36bed5b0a7393d; __jdu=1746186544; TrackID=1_1VrjhgDxypbnkupRVC8-fhEamTN9-EMNVnsqnMbXDDX1FE2GnewJPML6ewDSBSwC8ZM1e9D7kR0U2vB2MXeFb93l0AjKKwjWHmR03BRKug; pinId=wBSTpSnGmOea2wq-tGfAabV9-x-f3wj7; pin=jd_7a668be69efce; unick=%E5%91%A8%E5%87%AF%E6%AD%8C926; _tp=Di7eJDNFrmd1yz1kW%2F%2BsJm9I0i6a1fTtvQSGK2Uil64%3D; _pst=jd_7a668be69efce; cn=8; __jdv=76161171|direct|-|none|-|1564369950283; areaId=12; ipLoc-djd=12-933-3407-0; PCSYCityID=CN_320000_321300_321302; __jdc=122270672; 3AB9D23F7A4B3C9B=W6QBEF5Y5EEHHOXWYSZC2W573XDNDCKCNKNFR2WTIFWNLSYRADQCTJY3QJNDM2XXCICM7SW4MLBVF7SE2WASP7S7CU; JSESSIONID=07CC37B65B62A8E8048975D148F396A6.s1; __jda=122270672.1746186544.1539856564.1564369950.1564372074.18; shshshfp=8ff0bf5b7f1c46bcb48220bd8d1534ad; shshshsID=f11b05338261d7a6e634c05990ad3a41_3_1564372120748; __jdb=122270672.3.1746186544|18.1564372074";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = true;
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
        private void 京东商品总数_Load(object sender, EventArgs e)
        {
            Cef.Initialize(new CefSettings());
          //  browser = new ChromiumWebBrowser("https://mall.jd.com/index-10191.html");
            //browser.Parent = this.splitContainer1.Panel2;
            //browser.Dock = DockStyle.Fill;
           // browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(FrameEndFunc);

            try
            {

                string[] texts = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (var url in texts)
                {
                  
                        string html = GetUrl(url);

                        Match match = Regex.Match(html, @"search-"" \+ ([\s\S]*?) ");
                       
                        string URL = "https://mall.jd.com/view_search-" + match.Groups[1].Value + "-0-99-1-24-1.html";
                        browser = new ChromiumWebBrowser(URL);
                        browser.Parent = this.splitContainer1.Panel2;
                        browser.Dock = DockStyle.Fill;
                    if (status == true)
                    {
                        browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(FrameEndFunc);
                        status = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        bool status = false;
        private void FrameEndFunc(object sender, FrameLoadEndEventArgs e)
        {

            this.BeginInvoke(new Action(() => {
                String html = browser.GetSourceAsync().Result;
                textBox2.Text = html;
                status = true;

            }));
        }

        public void run()
        {
            try
            {

            string[] texts = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (var url in texts)
            {
                string html = GetUrl(url);

                Match match = Regex.Match(html, @"search-"" \+ ([\s\S]*?) ");

                string URL = "https://mall.jd.com/view_search-" + match.Groups[1].Value + "-0-99-1-24-1.html";
                browser = new ChromiumWebBrowser(URL);
                    browser.Parent = this.splitContainer1.Panel2;
                    browser.Dock = DockStyle.Fill;
                    browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(FrameEndFunc);

                }
        }
             catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Thread thread = new Thread(new ThreadStart(run));
            //Control.CheckForIllegalCrossThreadCalls = false;
            //thread.Start();
           
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
