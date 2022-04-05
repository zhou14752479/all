using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202202
{
    public partial class 计算 : Form
    {
        public 计算()
        {
            InitializeComponent();
        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
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
        public void jisuan()
        {
            double total = 0;
            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                //https://ugxlyxnlrg9udfdyzwnrvghlu2vydmvycg.blockjoy.com/v1/accounts/13aBmrJfLNPUP8W1grDu2RibjQ3xHmfnwNPHi3z5VRbwFNvSXk8
                label2.Text = "正在计算第："+(i+1);
                string url = "https://ugxlyxnlrg9udfdyzwnrvghlu2vydmvycg.blockjoy.com/v1/accounts/" + text[i];
                string html = GetUrl(url,"utf-8");
                string value = Regex.Match(html, @"""balance"":([\s\S]*?),").Groups[1].Value;
                total = total+ Convert.ToDouble(value)/ 100000000;
                textBox2.Text = total.ToString();
                Thread.Sleep(100);
            }
            MessageBox.Show("计算完成");
        }
        public void jisuan2()
        {
            double total = 0;
            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i <text.Length; i++)
            {
                label2.Text = "正在计算第：" + (i + 1);
                //https://ugxlyxnlrg9udfdyzwnrvghlu2vydmvycg.blockjoy.com/v1/accounts/13aBmrJfLNPUP8W1grDu2RibjQ3xHmfnwNPHi3z5VRbwFNvSXk8/rewards/sum?min_time=-48%20hour&max_time=2022-03-29T06%3A00%3A00.000Z&bucket=hour
                string url = "https://helium-api.stakejoy.com/v1/accounts/"+text[i]+ "/rewards/sum?min_time=-24%20hour&max_time="+DateTime.Now.ToString("yyyy-MM-dd")+"T10%3A00%3A00.000Z&bucket=hour";
                string html = GetUrl(url, "utf-8");
                MatchCollection values = Regex.Matches(html, @"""total"":([\s\S]*?),");
                for (int j = 0; j < values.Count; j++)
                {
                    total = total + Convert.ToDouble(values[j].Groups[1].Value);
                }
                
                textBox2.Text = total.ToString();
                Thread.Sleep(100);
            }
            MessageBox.Show("计算完成");
        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(jisuan);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 计算_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(jisuan2);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
