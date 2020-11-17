using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 启动程序
{
    public partial class 企业基础信息查询 : Form
    {
        public 企业基础信息查询()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 获取时间戳毫秒
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalMilliseconds);
            return a.ToString();
        }

        #region  获取32位MD5加密
        public string GetMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        #endregion

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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "http://app.gjzwfw.gov.cn/jmopen/webapp/html5/unZip/21804951cd294d869e0f92c27ba118a6/qyylbacbrypc/index.html";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("sec-fetch-mode:navigate");
                headers.Add("sec-fetch-site:same-origin");
                headers.Add("sec-fetch-user:?1");
                headers.Add("upgrade-insecure-requests: 1");
                //添加头部
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

        public string time = "";
        public string sign = "";

        public string ggsjpt_sign = "";

        bool zanting = true;
        bool status = true;
        public void run()
        {
            StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
            string text = streamReader.ReadToEnd();
            string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
      
            for (int i = 0; i < array.Length; i++)
            {
                time = GetTimeStamp();
                sign = GetMD5("qyjcxxcxpc" + time);
                ggsjpt_sign = GetMD5("ada72850-2b2e-11e7-985b-008cfaeb3d74995e00df72f14bbcb7833a9ca063adef" + time);
                string card = array[i];
                
                string url = "http://app.gjzwfw.gov.cn/jmopen/interfaces/wxTransferPort.do?callback=jQuery18306037720505044832_"+time+"&requestUrl=http%3A%2F%2Fapp.gjzwfw.gov.cn%2Fjimps%2Flink.do&datas=dhzkh%22param%22%3A%22dhzkh%5C%22from%5C%22%3A%5C%221%5C%22%2C%5C%22key%5C%22%3A%5C%221e448cd40a784fbbaf142d9528785535%5C%22%2C%5C%22requestTime%5C%22%3A%5C%22"+time+"%5C%22%2C%5C%22sign%5C%22%3A%5C%22"+sign+"%5C%22%2C%5C%22zj_ggsjpt_app_key%5C%22%3A%5C%22ada72850-2b2e-11e7-985b-008cfaeb3d74%5C%22%2C%5C%22zj_ggsjpt_sign%5C%22%3A%5C%22"+ggsjpt_sign+"%5C%22%2C%5C%22zj_ggsjpt_time%5C%22%3A%5C%22"+time+"%5C%22%2C%5C%22NSRSBH%5C%22%3A%5C%22"+card+"%5C%22%2C%5C%22additional%5C%22%3A%5C%22%5C%22dhykh%22dhykh&heads=&_="+time;

                string html = GetUrl(url, "utf-8");

                Match a1 = Regex.Match(html, @"""nSRSBH"":""([\s\S]*?)""");
                Match a2 = Regex.Match(html, @"""sHXYDM"":""([\s\S]*?)""");
                Match a3 = Regex.Match(html, @"""sCJYDZ"":""([\s\S]*?)""");
                Match a4 = Regex.Match(html, @"""zCDZ"":""([\s\S]*?)""");
                Match a5 = Regex.Match(html, @"""fDDBRXM"":""([\s\S]*?)""");
                Match a6 = Regex.Match(html, @"""dJRQ"":""([\s\S]*?)""");
                Match a7 = Regex.Match(html, @"""zZJG_DM"":""([\s\S]*?)""");
                Match a8 = Regex.Match(html, @"""nSRZT"":""([\s\S]*?)""");
                Match a9 = Regex.Match(html, @"""nSRMC"":""([\s\S]*?)""");
                Match a10 = Regex.Match(html, @"""dJXH"":""([\s\S]*?)""");
                Match a11 = Regex.Match(html, @"""dJZCLXMC"":""([\s\S]*?)""");
                Match a12 = Regex.Match(html, @"""sWJGMC"":""([\s\S]*?)""");
                Match a13 = Regex.Match(html, @"""fDDBRSFZJHM"":""([\s\S]*?)""");


                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                
                lv1.SubItems.Add(a1.Groups[1].Value);
                lv1.SubItems.Add(a2.Groups[1].Value);
                lv1.SubItems.Add(a10.Groups[1].Value);
                lv1.SubItems.Add(a6.Groups[1].Value);
                lv1.SubItems.Add(a11.Groups[1].Value);
                lv1.SubItems.Add(a13.Groups[1].Value);
                lv1.SubItems.Add(a5.Groups[1].Value);
                lv1.SubItems.Add(a9.Groups[1].Value);
                lv1.SubItems.Add(a8.Groups[1].Value);
                lv1.SubItems.Add(a3.Groups[1].Value);
                lv1.SubItems.Add(a12.Groups[1].Value);
                lv1.SubItems.Add(a4.Groups[1].Value);
                lv1.SubItems.Add(a7.Groups[1].Value);



                while (zanting == false)
                {
                    Application.DoEvents();//等待本次加载完毕才执行下次循环.
                }
                if (status == false)
                    return;

                Thread.Sleep(1000);
            }



        }
        private void 企业基础信息查询_Load(object sender, EventArgs e)
        {

        }

        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"shebaochaxun"))
            {
                status = true;
                button1.Enabled = false;
                //time = GetTimeStamp();
                //sign = GetMD5("qyjcxxcxpc" + time);
                //ggsjpt_sign = GetMD5("ada72850-2b2e-11e7-985b-008cfaeb3d74995e00df72f14bbcb7833a9ca063adef" + time);

                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;


            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            status = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }
    }
}
