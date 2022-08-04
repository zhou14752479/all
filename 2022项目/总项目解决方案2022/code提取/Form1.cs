using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

using myDLL;

namespace code提取
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
            string html = "";
            string COOKIE = "";
            try
            {
              
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈


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


        #region POST默认请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrlDefault(string url, string postData, string Authorization)
        {
            try
            {

                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //request.Proxy = null;//防止代理抓包
                request.Headers.Set(HttpRequestHeader.Authorization, Authorization);
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("Authorization:"+ Authorization);
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                request.ContentType = "application/x-www-form-urlencoded";
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                //request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", "");

                request.Referer = url;
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

        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {

        }


        List<string> list1=new List<string>();
        List<string> list2= new List<string>();
        public void run1()
        {

            try
            {
                for (int i = 0; i < numericUpDown2.Value; i++)
                {
                    if (!list1.Contains(i.ToString()))
                    {
                        list1.Add(i.ToString());

                        string url = "http://39.108.184.87/api/xcx.asp?username=" + textBox1.Text.Trim() + "&password=" + GetMD5(textBox2.Text.Trim()).ToUpper() + "&add=1&xcxid=9&more=1";

                        string html = GetUrl(url, "utf-8");
                        string code = Regex.Match(html, @"""code"":""([\s\S]*?)""").Groups[1].Value;
                        string msg = Regex.Match(html, @"""msg"":""([\s\S]*?)""").Groups[1].Value;
                        ListViewItem lv = listView1.Items.Add(listView1.Items.Count.ToString());
                        lv.SubItems.Add(code);
                        lv.SubItems.Add(msg);

                        if (status == false)
                            return;
                    }
                }



                button1.Enabled = true;
            }
            catch (Exception)
            {

               ;
            }
        }



        public string login()
        {
            string url = "http://keleapi.jinwandalaohu.net:89/api/Member/login";
            string postdata = "member_user="+textBox3.Text.Trim() + "&member_pwd="+textBox4.Text.Trim();
            string html = PostUrlDefault(url,postdata, "");
           
            string token = Regex.Match(html, @"""token"":""([\s\S]*?)""").Groups[1].Value;
            return token;
        }

        string token = "";

        public void run2()
        {
            try
            {
                if (token == "")
                {
                    token = login();
                }

                for (int i = 0; i < numericUpDown2.Value; i++)
                {
                    if (!list2.Contains(i.ToString()))
                    {
                        list2.Add(i.ToString());

                        string url = "http://keleapi.jinwandalaohu.net:89/api/Order/CreateOrder";
                        string postdata = "type=11&projectid=5669&Identifier=";
                        string html = PostUrlDefault(url, postdata, token);

                        string JsCode = Regex.Match(html, @"""JsCode"":""([\s\S]*?)""").Groups[1].Value;
                        string msg = Regex.Match(html, @"""msg"":""([\s\S]*?)""").Groups[1].Value;
                        ListViewItem lv = listView1.Items.Add(listView1.Items.Count.ToString());
                        lv.SubItems.Add(JsCode);
                        lv.SubItems.Add(msg);

                        if (status == false)
                            return;
                    }
                }



                button1.Enabled = true;
            }
            catch (Exception)
            {

                
            }
        }


        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }
        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                #region 通用检测

                string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

                if (!html.Contains(@"淘宝阿里SKU"))
                {
                    TestForKillMyself();
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }


                #endregion

                list1.Clear();
                list2.Clear();
                status = true;
                button1.Enabled = false;
                MessageBox.Show("开始提取");
                for (int i = 0; i < numericUpDown1.Value; i++)
                {
                    if (checkBox1.Checked == true)
                    {
                        Thread thread = new Thread(run1);
                        thread.Start();
                        Control.CheckForIllegalCrossThreadCalls = false;
                    }
                    if (checkBox2.Checked == true)
                    {
                        Thread thread = new Thread(run2);
                        thread.Start();
                        Control.CheckForIllegalCrossThreadCalls = false;
                    }

                    Thread.Sleep(100);
                }
                button1.Enabled = true;
            }
            catch (Exception)
            {

                
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        #region  listview导出文本TXT
        public static void ListviewToTxt(ListView listview, int i)
        {

            if (listview.Items.Count == 0)
            {
                MessageBox.Show("列表为空!");
                return;
            }

            List<string> list = new List<string>();
            foreach (ListViewItem item in listview.Items)
            {
                if (item.SubItems[i].Text.Trim() != "")
                {

                    list.Add(item.SubItems[i].Text);
                }


            }
            SaveFileDialog sfd = new SaveFileDialog();

            // string path = AppDomain.CurrentDomain.BaseDirectory + "导出_" + Guid.NewGuid().ToString() + ".txt";
            string path = "";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                path = sfd.FileName + ".txt";
            }
            StringBuilder sb = new StringBuilder();
            foreach (string tel in list)
            {
                sb.AppendLine(tel);
            }
            System.IO.File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("文件导出成功!文件地址:" + path);


        }






        #endregion
        private void button3_Click(object sender, EventArgs e)
        {
            ListviewToTxt(listView1,1);
        }

        bool status = true;
        private void button2_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
