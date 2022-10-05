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


namespace 足球数据比较
{
    
    public partial class 足球数据比较 : Form
    {
        [DllImport("user32.dll")]
        public static extern int MessageBoxTimeoutA(IntPtr hWnd, string msg, string Caps, int type, int Id, int time);//引用DLL
        public 足球数据比较()
        {
            InitializeComponent();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label3.Text = DateTime.Now.ToString("HH:mm:ss");
        }

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
                //request.ContentType = "application/x-www-form-urlencoded";
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                request.ContentType = "application/json";
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
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("appid:orders");
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

        #region 图片URL转image
        /// <summary>
        /// 通过NET获取网络图片
        /// </summary>
        /// <param name="url">要访问的图片所在网址</param>
        /// <param name="requestAction">对于WebRequest需要进行的一些处理，比如代理、密码之类</param>
        /// <param name="responseFunc">如何从WebResponse中获取到图片</param>
        /// <returns></returns>
        public static Image GetImage(string url)
        {
            Image img;
            try
            {
                WebRequest request = WebRequest.Create(url);

                using (WebResponse response = request.GetResponse())
                {
                    if (response.ContentType.IndexOf("text") != -1)
                    {
                        System.IO.Stream responseStream = response.GetResponseStream();
                        System.IO.StreamReader reader = new System.IO.StreamReader(responseStream, Encoding.UTF8);
                        string srcString = reader.ReadToEnd();
                        return null;
                    }
                    else
                    {
                        img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    }
                }
                return img;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        #endregion

        private void 足球数据比较_Load(object sender, EventArgs e)
        {
            #region 通用检测


            if (!GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"6xlD2"))
            {
               
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion

            pictureBox1.Image= GetImage("http://wxpusher.zjiecode.com/api/qrcode/VlbeD3y4xWB2obTY7xtDariOYONX3NUjXrmlyGTSj9mlucUF9686RrnJVYN7CJZc.jpg");
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("http://live.nowscore.com/odds/letGoal.aspx");
        }


        List<string> gongsilist=new List<string>();

        List<string> tishi = new List<string>();

        #region 发送wxpusher消息
        public string getuids()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                string url = "http://wxpusher.zjiecode.com/api/fun/wxuser/v2?appToken=AT_dRxL3cOuSXIEiCzjUCPsWAcH5RDfpITK&page=1";

                string html = GetUrl(url, "utf-8");

                MatchCollection uids = Regex.Matches(html, @"""uid"":""([\s\S]*?)""");
                foreach (Match item in uids)
                {
                    sb.Append("\"" + item.Groups[1].Value + "\"" + ",");

                }

            }
            catch (Exception)
            {

               
            }

            return sb.ToString().Remove(sb.ToString().Length - 1, 1);
        }

        public void sendmsg(string title, string neirong,string uid)
        {

            if (title.Trim() != "")
            {
                //"application/json"
               
                string url = "http://wxpusher.zjiecode.com/api/send/message";
                string postdata = "{\"appToken\":\"AT_dRxL3cOuSXIEiCzjUCPsWAcH5RDfpITK\",\"content\":\"" + neirong + "\",\"contentType\":2,\"uids\":[" + uid + "]}";
                string html = PostUrlDefault(url, postdata, "");

                 //MessageBox.Show(html);

            }
        }

        #endregion


        #region 主程序
        public void run()
        {
         string uids= getuids();
            string html = webBrowser1.Document.Body.OuterHtml;

          
            MatchCollection ahtml = Regex.Matches(html, @"<TR id=tr_([\s\S]*?)</TD></TR>");
            MatchCollection gongsis= Regex.Matches(html, @"<TD width=""12%"">([\s\S]*?)</TD>");

            for (int i = 0; i < gongsis.Count; i++)
            {
                if(!gongsilist.Contains(gongsis[i].Groups[1].Value))
                {
                    gongsilist.Add(gongsis[i].Groups[1].Value);
                }
            }

           
            for (int a = 0; a < ahtml.Count; a++)
            {
                try
                {

                    MatchCollection team = Regex.Matches(ahtml[a].Groups[1].Value, @"TeamPanlu_10\(([\s\S]*?)\)"">([\s\S]*?)</A>");
                    MatchCollection shang = Regex.Matches(ahtml[a].Groups[1].Value, @"class=sb href=""javascript:"">([\s\S]*?)<BR>");
                    MatchCollection xia = Regex.Matches(ahtml[a].Groups[1].Value, @"<BR><A>([\s\S]*?)</A>");
                    MatchCollection pankou = Regex.Matches(ahtml[a].Groups[1].Value, @"; class=pk href=""javascript:"">([\s\S]*?)</A>");
                    string time= Regex.Match(ahtml[a].Groups[1].Value+ "</SPAN>", @"<SPAN id=t_([\s\S]*?)>([\s\S]*?)</SPAN>").Groups[2].Value ;
                    string liansai= Regex.Match(ahtml[a].Groups[1].Value + "</SPAN>", @"<FONT color=#ffffff>([\s\S]*?)<").Groups[1].Value;
                    //MessageBox.Show(shang.Count.ToString());
                    //MessageBox.Show(xia.Count.ToString());
                    //MessageBox.Show(pankou.Count.ToString());

                    if (shang.Count > gongsilist.Count-1)
                    {

                        int xieru = 0;
                        for (int i = 0; i < shang.Count; i++)
                        {
                           
                            string pk0 = Regex.Replace(pankou[0].Groups[1].Value, "<[^>]+>", "");
                            string pk1 = Regex.Replace(pankou[1].Groups[1].Value, "<[^>]+>", "");
                            string pk2 ="";

                            string shang0 = Regex.Replace(shang[0].Groups[1].Value, "<[^>]+>", "");
                            string shang1 = Regex.Replace(shang[1].Groups[1].Value, "<[^>]+>", "");
                            string shang2 = "";

                            string xia0 = Regex.Replace(xia[0].Groups[1].Value, "<[^>]+>", "");
                            string xia1 = Regex.Replace(xia[1].Groups[1].Value, "<[^>]+>", "");
                            string xia2 = "";

                            if (gongsilist.Count>2)
                            {
                                pk2 = Regex.Replace(pankou[2].Groups[1].Value, "<[^>]+>", "");
                                shang2 = Regex.Replace(shang[2].Groups[1].Value, "<[^>]+>", "");
                                xia2 = Regex.Replace(xia[2].Groups[1].Value, "<[^>]+>", "");
                            }





                            ListViewItem lv1 = listView1.Items.Add(liansai); //使用Listview展示数据
                            lv1.SubItems.Add(Regex.Replace(time, "<[^>]+>", ""));
                            lv1.SubItems.Add(team[0].Groups[2].Value);
                            lv1.SubItems.Add(team[1].Groups[2].Value); ;
                            if (i==0)
                            {
                                lv1.SubItems.Add(gongsilist[0]);
                            }
                            if (i == 1)
                            {
                                lv1.SubItems.Add(gongsilist[1]);
                            }
                            if (i == 2)
                            {
                                lv1.SubItems.Add(gongsilist[2]);
                            }
                            lv1.SubItems.Add(Regex.Replace(pankou[i].Groups[1].Value, "<[^>]+>", ""));
                            lv1.SubItems.Add(Regex.Replace(shang[i].Groups[1].Value, "<[^>]+>", ""));
                            lv1.SubItems.Add(Regex.Replace(xia[i].Groups[1].Value, "<[^>]+>", ""));
                           

                            StringBuilder sb = new StringBuilder();
                            sb.Append(liansai+",");
                            sb.Append(team[0].Groups[2].Value + ",");
                            sb.Append(team[1].Groups[2].Value + ",");
                            sb.Append(time + ",   ");
                            sb.Append(gongsilist[0]+"-"+shang0 + "-"+pk0+"-"+xia0+",  ");
                            sb.Append(gongsilist[1]+"-"+shang1 + "-" + pk1 + "-" + xia1+ ",  ");
                            if (pk2 != "")
                            {
                                sb.Append(gongsilist[2]+"-"+shang2 + "-" + pk2 + "-" + xia2 + ",   ");
                            }

                            if (pk0 == pk1)
                            {


                                ////新添加功能大球加小球大于值
                                //if (Convert.ToDouble(xia0) + Convert.ToDouble(shang1) > Convert.ToDouble(textBox2.Text) || Convert.ToDouble(xia1) + Convert.ToDouble(shang0) > Convert.ToDouble(textBox2.Text))
                                //{
                                //    lv1.BackColor = Color.Red;
                                //    if (xieru == 0)
                                //    {

                                //        if (!tishi.Contains(team[0].Groups[2].Value + team[1].Groups[2].Value + pk0))
                                //        {
                                //            MessageBoxTimeoutA((IntPtr)0, sb.ToString(), "消息框", 0, 0, 3000);
                                //            tishi.Add(team[0].Groups[2].Value + team[1].Groups[2].Value + pk0);
                                //            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\data.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                                //            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                                //            sw.WriteLine(sb.ToString());
                                //            sw.Close();
                                //            fs1.Close();
                                //            sw.Dispose();


                                //            if (checkBox2.Checked == true)
                                //            {

                                //                sendmsg("足球数据提醒" + DateTime.Now.ToString(), sb.ToString(), uids);
                                //            }

                                //            if (checkBox1.Checked == true)
                                //            {
                                //                send(textBox1.Text, "足球数据对比提醒：" + liansai + "-" + team[0].Groups[2].Value + "-" + team[1].Groups[2].Value, sb.ToString());
                                //            }

                                //            xieru = 1;
                                //        }
                                //    }

                                //}
                                ////新添加功能大球加小球大于值























                                double v1 = Convert.ToDouble(shang0) - Convert.ToDouble(shang1);
                                double v2 = Convert.ToDouble(xia0) - Convert.ToDouble(xia1);
                                if (v1 > 0.1 || v1 < -0.1)
                                {
                                    if (v2 > 0.1 || v2 < -0.1)
                                    {

                                        if (gongsilist.Count > 2)
                                        {

                                            if (Convert.ToDouble(xia0) - Convert.ToDouble(shang0) > 0 && Convert.ToDouble(xia1) - Convert.ToDouble(shang1) > 0 && Convert.ToDouble(xia2) - Convert.ToDouble(shang2) > 0)
                                            {
                                                lv1.BackColor = Color.Red;
                                                if (xieru == 0)
                                                {

                                                    if (!tishi.Contains(team[0].Groups[2].Value + team[1].Groups[2].Value + pk0))
                                                    {
                                                        MessageBoxTimeoutA((IntPtr)0, sb.ToString(), "消息框", 0, 0, 3000);
                                                        tishi.Add(team[0].Groups[2].Value + team[1].Groups[2].Value + pk0);
                                                        FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\data.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                                                        StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                                                        sw.WriteLine(sb.ToString());
                                                        sw.Close();
                                                        fs1.Close();
                                                        sw.Dispose();


                                                        if (checkBox2.Checked == true)
                                                        {

                                                            sendmsg("足球数据提醒" + DateTime.Now.ToString(), sb.ToString(), uids);
                                                        }

                                                        if (checkBox1.Checked == true)
                                                        {
                                                            send(textBox1.Text, "足球数据对比提醒：" + liansai + "-" + team[0].Groups[2].Value + "-" + team[1].Groups[2].Value, sb.ToString());
                                                        }

                                                        xieru = 1;
                                                    }
                                                }

                                            }
























                                        }










                                        }

                                    }

                            }

                            else if(pk1==pk2)
                            {
                                double v1 = Convert.ToDouble(shang1) - Convert.ToDouble(shang2);
                                double v2 = Convert.ToDouble(xia1) - Convert.ToDouble(xia2);
                                if (v1 > 0.1 || v1 < -0.1)
                                {
                                    if (v2 > 0.1 || v2 < -0.1)
                                    {


                                        if (gongsilist.Count > 2)
                                        {

                                            if (Convert.ToDouble(xia0) - Convert.ToDouble(shang0) > 0 && Convert.ToDouble(xia1) - Convert.ToDouble(shang1) > 0 && Convert.ToDouble(xia2) - Convert.ToDouble(shang2) > 0)
                                            {





                                                lv1.BackColor = Color.Red;
                                                if (xieru == 0)
                                                {

                                                    if (!tishi.Contains(team[0].Groups[2].Value + team[1].Groups[2].Value + pk1))
                                                    {
                                                        MessageBoxTimeoutA((IntPtr)0, sb.ToString(), "消息框", 0, 0, 3000);
                                                        tishi.Add(team[0].Groups[2].Value + team[1].Groups[2].Value + pk1);
                                                        FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\data.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                                                        StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                                                        sw.WriteLine(sb.ToString());
                                                        sw.Close();
                                                        fs1.Close();
                                                        sw.Dispose();


                                                        if (checkBox2.Checked == true)
                                                        {

                                                            sendmsg("足球数据提醒" + DateTime.Now.ToString(), sb.ToString(), uids);
                                                        }
                                                        if (checkBox1.Checked == true)
                                                        {

                                                            send(textBox1.Text, "足球数据对比提醒：" + liansai + "-" + team[0].Groups[2].Value + "-" + team[1].Groups[2].Value, sb.ToString());
                                                        }
                                                        xieru = 1;
                                                    }
                                                }






                                            }
                                        }









                                    }

                                }
                            }


















                        }
                    }




                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                    continue;
                }

            }


        }

        #endregion
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
            timer1.Interval = Convert.ToInt32(numericUpDown1.Value * 1000);
            run();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = Convert.ToInt32(numericUpDown1.Value*1000);
            listView1.Items.Clear();
            run();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            send("852266010@qq.com","测试","测试");
            // textBox2.Text= webBrowser1.Document.Body.OuterHtml;
            timer1.Stop();
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
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            System.Diagnostics.Process.Start(listView1.SelectedItems[0].SubItems[5].Text);
            MessageBox.Show(listView1.SelectedItems[0].SubItems[5].Text);
        }

        private void 足球数据比较_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
