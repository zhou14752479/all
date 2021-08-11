using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace coinmarketcap邮件提醒监控
{
    public partial class Form1 : Form
    {
        public Form1()
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
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
                return html;



            }
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }
        #endregion

        #region 发邮件
        public  void send(string address, string subject, string body)
        {
            //实例化一个发送邮件类。
            MailMessage mailMessage = new MailMessage();
            //发件人邮箱地址，方法重载不同，可以根据需求自行选择。
            mailMessage.From = new MailAddress("1073689549@qq.com");
            //收件人邮箱地址。
            mailMessage.To.Add(new MailAddress(address));
            //邮件标题。
            mailMessage.Subject = subject;
            //邮件内容。
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            //实例化一个SmtpClient类。
            SmtpClient client = new SmtpClient();
            //在这里我使用的是qq邮箱，所以是smtp.qq.com，如果你使用的是126邮箱，那么就是smtp.126.com。
            client.Host = "smtp.qq.com";
            //使用安全加密连接。
            client.EnableSsl = true;
            //不和请求一块发送。
            client.UseDefaultCredentials = false;
            //验证发件人身份(发件人的邮箱，邮箱里的生成授权码);
            client.Credentials = new NetworkCredential("1073689549@qq.com", "gusmfehdkmqtbbbh");   //这里的密码用授权码
            //发送
            client.Send(mailMessage);
            // MessageBox.Show("发送成功");

        }
        #endregion

        List<string> lists = new List<string>();
        public void run()
        {
            try
            {
                string url = "https://coinmarketcap.com/zh/new/";
                string html = GetUrl(url,"utf-8");
                MatchCollection ids = Regex.Matches(html, @"font-size=""1"" class=""sc-1eb5slv-0 iJjGCS"">([\s\S]*?)</p>");
                if (lists.Count == 0)
                {
                    for (int i = 0; i < ids.Count; i++)
                    {
                        lists.Add(ids[i].Groups[1].Value);
                    }
                }
                else
                {
                    for (int i = 0; i < ids.Count; i++)
                    {
                        if (!lists.Contains(ids[i].Groups[1].Value))
                        {
                            string subject = DateTime.Now.ToString() + "：出现新币种--" + ids[i].Groups[1].Value;
                            send(textBox2.Text.Trim(), "出现新币种提醒", subject);
                            label3.Text = subject;
                            textBox3.Text += subject + "\r\n";
                            lists.Clear();
                            break;


                        }
                        else
                        {
                            textBox3.Text += DateTime.Now.ToString()+"：正在监控.."+ ids[i].Groups[1].Value + "\r\n";
                        }
                       
                    }

                }
            }
            catch (Exception ex)
            {

              textBox3.Text=ex.ToString();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        Thread thread;

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("请输入邮件地址和监控间隔");
                return;
            }
            timer1.Interval = Convert.ToInt32(textBox1.Text) * 1000;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (textBox3.TextLength > 1000)
            {
                textBox3.Text = "";
            }
            timer1.Interval = (Convert.ToInt32(textBox1.Text.Trim()))*1000;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            send(textBox2.Text.Trim(), "出现新币种提醒", "123");
            timer1.Stop();
        }
    }
}
