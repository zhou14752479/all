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
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace 宿网办公助手
{
    public partial class 网站监控 : Form
    {
        public 网站监控()
        {
            InitializeComponent();
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
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
                request.Referer = "";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

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
        #region POST默认请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrlDefault(string url, string postData, string COOKIE)
        {
            try
            {

                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;//防止代理抓包
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
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
                request.Headers.Add("Cookie", COOKIE);

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
        List<string> sendlist = new List<string>();
        List<string> list = new List<string>();
        public void run_sq360()
        {
            try
            {
                //label2.Text = DateTime.Now.ToString("HH:mm:ss") + "：监控了宿迁零距离。";
                this.Text = "上次监控：" + DateTime.Now.ToString("HH:mm:ss");
                string url = "https://www.suqian360.com/forum.php?mod=forumdisplay&fid=96&filter=author&orderby=dateline";
                string html = GetUrl(url, "gb2312");

                MatchCollection titles = Regex.Matches(html, @"class=""s xst"">([\s\S]*?)</a>");
                MatchCollection times = Regex.Matches(html, @"<em><span([\s\S]*?)title=""([\s\S]*?)</span>");
                MatchCollection urls = Regex.Matches(html, @"<td class=""num""><a href=""([\s\S]*?)""");

                for (int a = 0; a < tiaoshu; a++)
                {
                    try
                    {
                        string aurl = urls[a].Groups[1].Value.Replace("viewthread&amp;", "viewthread&");
                        string time = times[a].Groups[2].Value.Replace("\">", " ").Replace("&nbsp;", "");

                        list.Add(urls[a].Groups[1].Value);
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add("宿迁零距离");
                            lv1.SubItems.Add(titles[a].Groups[1].Value);
                            lv1.SubItems.Add(time) ;
                            lv1.SubItems.Add(aurl);
                   
                        if (a == 0)
                        {
                           
                            lv1.ForeColor = Color.Red;
                        }
                     
                        //if (checkBox3.Checked == true)
                        //{
                            if (!sendlist.Contains(urls[a].Groups[1].Value))
                            {
                                sendlist.Add(urls[a].Groups[1].Value);
                                if (sendlist.Count > 10)
                                {
                                    //send(textBox1.Text, titles[a].Groups[1].Value, titles[a].Groups[1].Value+ "-宿迁零距离" + urls[a].Groups[1].Value);

                                    FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\宿迁零距离.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                                    StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                                    sw.WriteLine(titles[a].Groups[1].Value, titles[a].Groups[1].Value + "-宿迁零距离" + urls[a].Groups[1].Value);
                                    sw.Close();
                                    fs1.Close();
                                    sw.Dispose();
                                }
                            }
                       

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        continue;
                    }
                }



            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }


        public void run_sqluntan()
        {
            try
            {

                //label2.Text = DateTime.Now.ToString("HH:mm:ss") + "：监控了宿迁论坛。";
                this.Text = "上次监控：" + DateTime.Now.ToString("HH:mm:ss"); ;

                string url = "http://www.sqee.cn/forum.php?mod=forumdisplay&fid=24&filter=author&orderby=dateline";
                string html = GetUrl(url, "gb2312");

                MatchCollection titles = Regex.Matches(html, @"class=""s xst"">([\s\S]*?)</a>");
                MatchCollection times = Regex.Matches(html, @"<em><span([\s\S]*?)title=""([\s\S]*?)</span>");
                MatchCollection urls = Regex.Matches(html, @"<td class=""num""><a href=""([\s\S]*?)""");

                for (int a = 0; a < tiaoshu; a++)
                {
                    try
                    {
                        string aurl = urls[a].Groups[1].Value.Replace("viewthread&amp;", "viewthread&");
                        string time = times[a].Groups[2].Value.Replace("\">", " ").Replace("&nbsp;", "");
                            list.Add(urls[a].Groups[1].Value);
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add("宿迁论坛");
                            lv1.SubItems.Add(titles[a].Groups[1].Value);
                            lv1.SubItems.Add(time);
                            lv1.SubItems.Add(aurl);
                     
                        if (a == 0)
                        {
                            lv1.ForeColor = Color.Red;
                        }

                        //if (checkBox3.Checked == true)
                        //{
                            if (!sendlist.Contains(urls[a].Groups[1].Value))
                            {
                                sendlist.Add(urls[a].Groups[1].Value);
                                if (sendlist.Count > 10)
                                {
                                   // send(textBox1.Text, titles[a].Groups[1].Value, titles[a].Groups[1].Value +"-宿迁论坛"+ urls[a].Groups[1].Value);

                                    FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\宿迁论坛.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                                    StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                                    sw.WriteLine(titles[a].Groups[1].Value, titles[a].Groups[1].Value + "-宿迁论坛" + urls[a].Groups[1].Value);
                                    sw.Close();
                                    fs1.Close();
                                    sw.Dispose();
                                }
                            }
                        
                       

                    }
                    catch (Exception)
                    {

                        continue;
                    }
                }



            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        Thread thread;
        Thread thread2;
       
        private void timer1_Tick(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            timer1.Interval = Convert.ToInt32(time_refresh) * 1000 * 60;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run_sq360);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            if (thread2 == null || !thread2.IsAlive)
            {
                thread2 = new Thread(run_sqluntan);
                thread2.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }


            IniWriteValue("values", "mail", time_refresh.ToString());
        }

      

        private void 网站监控_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
        #region 发邮件
        public static void send(string address, string subject, string body)
        {
            try
            {
                //实例化一个发送邮件类。
                MailMessage mailMessage = new MailMessage();
                //发件人邮箱地址，方法重载不同，可以根据需求自行选择。
                mailMessage.From = new MailAddress("a852266010@126.com");
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
                client.Host = "smtp.126.com";
                //使用安全加密连接。
                client.EnableSsl = true;
                //不和请求一块发送。
                client.UseDefaultCredentials = false;
                //验证发件人身份(发件人的邮箱，邮箱里的生成授权码);
                client.Credentials = new NetworkCredential("a852266010@126.com", "SAWQWAEMPDKLHFLD");   //这里的密码用授权码
                                                                                                        //发送
                client.Send(mailMessage);
                // MessageBox.Show("发送成功");
            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.ToString());
            }

        }
        #endregion
        #region 发送wxpusher消息
        public string getuids()
        {
            StringBuilder sb = new StringBuilder();
            string url = "http://wxpusher.zjiecode.com/api/fun/wxuser/v2?appToken=AT_Zwbx5uVZTIpxJ2OPaCGXXOZNuiWHmTKQ&page=1";

            string html = GetUrl(url, "utf-8");

            MatchCollection uids = Regex.Matches(html, @"""uid"":""([\s\S]*?)""");
            foreach (Match item in uids)
            {
                sb.Append("\"" + item.Groups[1].Value + "\"" + ",");

            }

            return sb.ToString().Remove(sb.ToString().Length - 1, 1);
        }

        public void sendmsg(string title, string neirong)
        {
            if (title.Trim() != "")
            {
                //"application/json"
                string uids = getuids();
                string url = "http://wxpusher.zjiecode.com/api/send/message";
                string postdata = "{\"appToken\":\"AT_Zwbx5uVZTIpxJ2OPaCGXXOZNuiWHmTKQ\",\"content\":\"" + neirong + "\",\"contentType\":2,\"uids\":[\"" + wxid + "\"]}";
                string html = PostUrlDefault(url, postdata, "");

                // MessageBox.Show(html);

            }
        }

        #endregion
        public static string mail="852266010@qq.com";
        public static int time_refresh=1;
        public static int tiaoshu=5;
        public static string wxid = "";


        private void 网站监控_Load(object sender, EventArgs e)
        {
            try
            {
                if (ExistINIFile())
                {
                    time_refresh = Convert.ToInt32(IniReadValue("values", "time_refresh"));
                    mail = IniReadValue("values", "mail");
                    tiaoshu = Convert.ToInt32(IniReadValue("values", "tiaoshu"));
                }
                else
                {
                    IniWriteValue("values", "time_refresh", "1");
                    IniWriteValue("values", "tiaoshu", "5");
                    IniWriteValue("values", "mail", "852266010@qq.com");
                }
            }
            catch (Exception)
            {

                time_refresh = 1;
                mail = "852266010@qq.com";
                tiaoshu = 5;

            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.listView1.SelectedItems.Count == 0)
                    return;
                System.Diagnostics.Process.Start(listView1.SelectedItems[0].SubItems[4].Text);
            }
            catch (Exception)
            {
                MessageBox.Show("异常");
                
            }
           
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory+"//宿迁零距离.txt";
            if (File.Exists(path))
            {
                System.Diagnostics.Process.Start(path);
            }
        }

     

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            timer1.Start();
            //timer1.Interval = Convert.ToInt32(numericUpDown1.Value) * 1000 * 60;

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run_sq360);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            if (thread2 == null || !thread2.IsAlive)
            {
                thread2 = new Thread(run_sqluntan);
                thread2.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //send("852266010@qq.com","测试","测试");
            timer1.Stop();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run_sq360);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            if (thread2 == null || !thread2.IsAlive)
            {
                thread2 = new Thread(run_sqluntan);
                thread2.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            通知管理 tz=new 通知管理();
            tz.Show();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            设置 sz = new 设置();
            sz.Show();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            宿城地址查询街道 sc = new 宿城地址查询街道();
            sc.Show();

        }
    }
    }
