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

namespace 启动程序
{
    public partial class 抢单软件 : Form
    {
        public 抢单软件()
        {
            InitializeComponent();
        }

        private void 抢单软件_Load(object sender, EventArgs e)
        {

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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://cn.bing.com/search?q=%e9%a6%99%e6%b8%af%e5%85%ad%e5%90%88%e5%bd%a9&qs=n&sp=-1&first=01&FORM=PORE";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("appid:orders");
                //headers.Add("x-nike-visitid:5");
                //headers.Add("x-nike-visitorid:d03393ee-e42c-463e-9235-3ca0491475b4");
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

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData, string COOKIE)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
              
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion
        public void run()
        {
            string url = "https://www.iciti.co/otc/push-buy";
            string cookie = textBox4.Text;
            string postdata = "id="+textBox1.Text.Trim()+ "if&type=0&password=" + textBox2.Text.Trim();
            string html = PostUrl(url,postdata,cookie);
            if (!html.Contains("不存在"))
            {
                label4.Text = "成功，已暂停";
                timer1.Stop();
                return;
               
            }
            textBox3.Text += DateTime.Now.ToString()+ html + "\r\n";
            textBox3.Text += DateTime.Now.ToString() + html + "\r\n";
            Thread.Sleep(1);
            textBox3.Text += DateTime.Now.ToString() + html + "\r\n";
            Thread.Sleep(1);
            textBox3.Text += DateTime.Now.ToString() + html + "\r\n";
            textBox3.Text += DateTime.Now.ToString() + html + "\r\n";
            Thread.Sleep(1);
            textBox3.Text += DateTime.Now.ToString() + html + "\r\n";
            textBox3.Text += DateTime.Now.ToString() + html + "\r\n";
            Thread.Sleep(1);
            textBox3.Text += DateTime.Now.ToString() + html + "\r\n";
            return;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"qiangdan"))
            {
                timer1.Start();

            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
           
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {

            Thread thread1 = new Thread(new ThreadStart(run));
            thread1.Start();
            Control.CheckForIllegalCrossThreadCalls = false;



        }

        private void Button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
    }
}
