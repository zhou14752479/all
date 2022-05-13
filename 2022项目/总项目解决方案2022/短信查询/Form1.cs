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

namespace 短信查询
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
    
        public string GetUrlWithCookie(string Url)
        {
            string html = "";

            try
            {
                string COOKIE = textBox6.Text.Trim();
                string charset = "utf-8";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                // request.Proxy = null;//防止代理抓包
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
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

        

        Thread thread;

        public void getdata()
        {
            try
            {
                for (int i = Convert.ToInt32(textBox2.Text); i < Convert.ToInt32(textBox5.Text); i++)
                {
                    string userid = i.ToString();
                    string url = textBox1.Text + "/usershowse.aspx?userid=" + userid;
                    textBox4.Text += "正在请求网址：" + url + "\r\n";

                    //StreamReader sr = new StreamReader(@"C:\Users\Administrator\Desktop\2.txt", Encoding.GetEncoding("utf-8")) ;
                    //string html = sr.ReadToEnd();
                    string html = GetUrlWithCookie(url);
                    Thread.Sleep(100);

                    string username = Regex.Match(html, @"<input id=""account""([\s\S]*?)value=""([\s\S]*?)""").Groups[2].Value;
                    string total = Regex.Match(html, @"短信总数([\s\S]*?)条").Groups[1].Value;
                    string last = Regex.Match(html, @"剩余短信([\s\S]*?)条").Groups[1].Value;

                    username = Regex.Replace(username, "<[^>]+>", "").Replace("：", "").Replace(":", "").Replace(" ", "").Trim();
                    total = Regex.Replace(total, "<[^>]+>", "").Replace("：", "").Replace(":", "").Replace(" ", "").Trim();
                    last = Regex.Replace(last, "<[^>]+>", "").Replace("：", "").Replace(":", "").Replace(" ", "").Trim();
                    if (total == "")
                    {
                        textBox4.Text += "获取数据为空" + "\r\n";

                    }
                    else
                    {
                        FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\data.csv", FileMode.Append, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                        sw.WriteLine(userid + ","+username+"," + total + "," + last);
                        sw.Close();
                        fs1.Close();
                        sw.Dispose();
                        textBox4.Text += userid + "：写入表格成功" + "\r\n";
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {



            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getdata);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            timer1.Start();
            timer1.Interval = Convert.ToInt32(textBox3.Text) * 1000;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getdata);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
         

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getdata);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
