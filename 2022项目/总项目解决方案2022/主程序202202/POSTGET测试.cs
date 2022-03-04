using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202202
{
    public partial class POSTGET测试 : Form
    {
        public POSTGET测试()
        {
            InitializeComponent();
        }


        Thread thread;


        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public  void GetUrl()
        {
            string url = urltxt.Text;
            string charset = charsetcob.Text;
            string contenttype = contenttypecob.Text;
            string html = "";
            if (!url.Contains("http"))
            {
                url = "http://" + url;
            }
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);  //创建一个链接
              
                request.UserAgent = useragenttxt.Text;
                request.Referer = refertxt.Text;
                request.Headers.Add("Cookie", cookietxt.Text);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                if (redirectcheck.Checked == true)
                {
                    request.AllowAutoRedirect = true;
                }
                else
                {
                    request.AllowAutoRedirect = false;
                }

                if (KeepAlivecheck.Checked == true)
                {
                    request.KeepAlive = true;
                }
                else
                {
                    request.KeepAlive = true;
                }
                request.Accept = "*/*";
                request.Timeout = 100000;

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
                bodytxt.Text = html;



            }
            catch (System.Exception ex)
            {
               bodytxt.Text= ex.ToString();

            }



        }
        #endregion
        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
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
        public void PostUrl()
        {
            try
            {
                string url = urltxt.Text;
                if(!url.Contains("http"))
                {
                    url = "http://" + url;
                }
                string postData = postdatatxt.Text.Trim();
                string contenttype = contenttypecob.Text;
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                request.ContentType = contenttype;
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                if(redirectcheck.Checked==true)
                {
                    request.AllowAutoRedirect = true;
                }
                else
                {
                    request.AllowAutoRedirect = false;
                }

                if (KeepAlivecheck.Checked == true)
                {
                    request.KeepAlive = true;
                }
                else
                {
                    request.KeepAlive = false;
                }

               

                request.UserAgent = useragenttxt.Text;
                request.Headers.Add("Cookie", cookietxt.Text);

                request.Referer = refertxt.Text;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charsetcob.Text));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charsetcob.Text)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                bodytxt.Text = html;
                response.Close();
               
            }
            catch (WebException ex)
            {

               bodytxt.Text= ex.ToString();
            }


        }

        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            bodytxt.Text = "";
            if (radioButton1.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(GetUrl);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
            if (radioButton2.Checked==true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(PostUrl);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
           
        }

        private void POSTGET测试_Load(object sender, EventArgs e)
        {

        }
    }
}
