using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace douyinSelenium
{
    public partial class 卡密后台 : Form
    {
        public 卡密后台()
        {
            InitializeComponent();
        }

        public static string PostUrlDefault(string url, string postData, string COOKIE)
        {
            try
            {

                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;
                request.ContentType = "application/x-www-form-urlencoded";

                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

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
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        

        #region  获取32位MD5加密
        public static string GetMD5(string txt)
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


        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(create);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }


        public void create()
        {
           
            string time = DateTime.Now.ToString();
            if (radioButton1.Checked == true)
            {
                time = DateTime.Now.AddDays(1).ToString();
            }
            if (radioButton2.Checked == true)
            {
                time = DateTime.Now.AddDays(7).ToString();
            }
            if (radioButton3.Checked == true)
            {
                time = DateTime.Now.AddDays(30).ToString();
            }
            if (radioButton4.Checked == true)
            {
                time = DateTime.Now.AddDays(90).ToString();
            }
            if (radioButton5.Checked == true)
            {
                time = DateTime.Now.AddDays(180).ToString();
            }
            if (radioButton6.Checked == true)
            {
                time = DateTime.Now.AddDays(365).ToString();
            }
            if (radioButton7.Checked == true)
            {
                time = DateTime.Now.AddDays(730).ToString();
            }
            if (radioButton8.Checked == true)
            {
                time = DateTime.Now.AddDays(3600).ToString();
            }



            string kami = GetMD5(Guid.NewGuid().GetHashCode() + "56146svsfs"+DateTime.Now.ToString()).ToUpper();
            textBox1.Text = kami;
            string url = "http://43.154.221.28/kami.php";
            string postdata = "type=set&kami="+kami+"&time=" + time.Trim();
            string html = PostUrlDefault(url, postdata, "");
            string msg = Regex.Match(html, @"""msg"":""([\s\S]*?)""").Groups[1].Value;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(textBox1.Text); //复制
        }
    }
}
