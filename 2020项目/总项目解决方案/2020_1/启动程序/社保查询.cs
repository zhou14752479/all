using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
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
    public partial class 社保查询 : Form
    {
        public 社保查询()
        {
            InitializeComponent();
        }

        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

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
        public  string GetMD5(string txt)
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
        public string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "HWWAFSESID=cc9147f4aa41fc86ee; HWWAFSESTIME=1618565738420; route=0f1040e0778720d344b64fd91ee406cf; _monitor_sessionid=tCy7Ys6iRe1626459960928; _monitor_idx=5; JMOPENSESSIONID=1b9e1ff0-e9f4-4dc0-9a49-3720b58f83d9";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                                                                                  // request.Proxy = null;//防止代理抓包
                                                                                  //WebProxy proxy = new WebProxy(ip);
                                                                                  //request.Proxy = proxy;
                string ua1 = "Mozilla/5.0 (iPhone; CPU iPhone OS 14_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.17(0x17001126) NetType/WIFI Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.UserAgent = ua1;
                request.Referer = "http://app.gjzwfw.gov.cn/jmopen/webapp/html5/rkkpcxxcxjtapp/index.html";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }
        #endregion

     
        bool zanting = true;
        bool status = true;
        public void run()
        {
            string[] array = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < array.Length; i++)
            {
                Match value1 = Regex.Match(array[i], @"[\u4e00-\u9fa5]*");
                string value2 = Regex.Replace(array[i], @"[\u4e00-\u9fa5]*", "");

                string time = GetTimeStamp();

                // sign = GetMD5("qyylbacbrypc" + time);
                string ggsjpt_sign = GetMD5("ada72850-2b2e-11e7-985b-008cfaeb3d74995e00df72f14bbcb7833a9ca063adef" + time);


                string sign = GetMD5("qyylbacbrypc" + time);


                string name = System.Web.HttpUtility.UrlEncode(value1.Groups[0].Value.Trim());

                string card = value2.Trim();
                //string url = "http://app.gjzwfw.gov.cn/jmopen/interfaces/wxTransferPort.do?callback=jQuery18309492701749972507_" + time + "&requestUrl=http%3A%2F%2Fapp.gjzwfw.gov.cn%2Fjimps%2Flink.do&datas=dhzkh%22param%22%3A%22dhzkh%5C%22from%5C%22%3A%5C%221%5C%22%2C%5C%22key%5C%22%3A%5C%2291da7d51a42542219852bb3df4399d03%5C%22%2C%5C%22requestTime%5C%22%3A%5C%22" + time + "%5C%22%2C%5C%22sign%5C%22%3A%5C%22" + sign + "%5C%22%2C%5C%22zj_ggsjpt_app_key%5C%22%3A%5C%22ada72850-2b2e-11e7-985b-008cfaeb3d74%5C%22%2C%5C%22zj_ggsjpt_sign%5C%22%3A%5C%22" + ggsjpt_sign + "%5C%22%2C%5C%22zj_ggsjpt_time%5C%22%3A%5C%22" + time + "%5C%22%2C%5C%22name%5C%22%3A%5C%22"+name+"%5C%22%2C%5C%22cardId%5C%22%3A%5C%22"+card+"%5C%22%2C%5C%22additional%5C%22%3A%5C%22%5C%22dhykh%22dhykh&heads=&_=" + time;


                string url = "http://app.gjzwfw.gov.cn/jimps/link.do?param=%7B%22from%22%3A%221%22%2C%22key%22%3A%2291da7d51a42542219852bb3df4399d03%22%2C%22requestTime%22%3A%22"+time+"%22%2C%22sign%22%3A%22"+sign+"%22%2C%22zj_ggsjpt_app_key%22%3A%22ada72850-2b2e-11e7-985b-008cfaeb3d74%22%2C%22zj_ggsjpt_sign%22%3A%22"+ggsjpt_sign+"%22%2C%22zj_ggsjpt_time%22%3A%22"+time+"%22%2C%22cardId%22%3A%22"+card+"%22%2C%22name%22%3A%22"+ name+ "%22%2C%22additional%22%3A%22%22%7D";


                //url = url.Replace("%22:", "%22%3A");

                string html = GetUrl(url, "utf-8");
                textBox2.Text = url;
                Match com = Regex.Match(html, @"""companyName"":""([\s\S]*?)""");
                MessageBox.Show(html);
                Match aa = Regex.Match(html, @"""personelNo"":""([\s\S]*?)""");
                Match bb= Regex.Match(html, @"""insuranceType"":""([\s\S]*?)""");
                Match cc = Regex.Match(html, @"""addr"":""([\s\S]*?)""");
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                lv1.SubItems.Add(value1.Groups[0].Value);
                lv1.SubItems.Add(value2);
                lv1.SubItems.Add(com.Groups[1].Value);
                lv1.SubItems.Add(aa.Groups[1].Value);
                lv1.SubItems.Add(bb.Groups[1].Value);
                lv1.SubItems.Add(cc.Groups[1].Value);

                Thread.Sleep(1000);
                while (zanting == false)
                {
                    Application.DoEvents();//等待本次加载完毕才执行下次循环.
                }
                if (status == false)
                    return;
            }


          
        }

        private void 社保查询_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

          

            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            status = false;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
