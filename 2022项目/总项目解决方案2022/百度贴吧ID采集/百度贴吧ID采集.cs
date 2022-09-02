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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 百度贴吧ID采集
{
    public partial class 百度贴吧ID采集 : Form
    {
        public 百度贴吧ID采集()
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

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
                //request.Referer = Url;

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



        // http://tieba.baidu.com/mo//m?&kz=7979266751&pn=1
        //http://wapp.baidu.com/f?kw=LOL&pn=1
        public void run()
        {
            string txtname = DateTime.Now.ToString("yyyyMMddHHmmss");


            try
            {
                string[] text = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                foreach (var item in text)
                {

                    List<string> list = new List<string>();
                    int count = 0;
                    for (int pn = 1; pn < 1000; pn++)
                    {
                        if (count > Convert.ToInt32(textBox5.Text))
                            break;
                        string url = "http://wapp.baidu.com/f?kw=" + item.ToString().Trim() + "&pn=" + pn;
                        string html = GetUrl(url, "utf-8");

                        MatchCollection ids = Regex.Matches(html, @"kz=([\s\S]*?)&");
                        MatchCollection huis = Regex.Matches(html, @"<p>点([\s\S]*?)回([\s\S]*?)&");
                        for (int i = 0; i < ids.Count; i++)
                        {
                            count++;
                            if (count > Convert.ToInt32(textBox5.Text))
                                break;

                          

                           int hui=Convert.ToInt32(huis[i].Groups[2].Value);
                            textBox3.Text += DateTime.Now.ToString("HH:mm:ss") + " 在【" + item.Trim() + "】贴吧 第" + count + "贴里共回复" + hui + "\r\n";


                            for (int page = 0; page <= hui; page=page+30)
                            {
                                string aurl = "http://tieba.baidu.com/mo//m?&kz=" + ids[i].Groups[1].Value + "&pn="+page;
                                string ahtml = GetUrl(aurl, "utf-8");
                                MatchCollection aids = Regex.Matches(ahtml, @"<span class=""g"">([\s\S]*?)</span>");

                               
                                for (int a = 0; a < aids.Count; a++)
                                {
                                    string uid = Regex.Replace(aids[a].Groups[1].Value, "<[^>]+>", "");
                                    if (!list.Contains(uid))
                                    {
                                        list.Add(uid);
                                        textBox1.Text += uid + "\r\n";

                                       if(checkBox1.Checked==true)
                                        {
                                            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\" + txtname + ".txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                                            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                                            sw.WriteLine(uid);
                                            sw.Close();
                                            fs1.Close();
                                            sw.Dispose();
                                        }




                                    }

                                    if (status == false)
                                        return;
                                }

                                if (count > Convert.ToInt32(textBox5.Text))
                                    break;
                            }

                        }

                       
                    }



                }
            }
            catch (Exception ex)
            {

                textBox3.Text = ex.ToString();
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
        private void 百度贴吧ID采集_Load(object sender, EventArgs e)
        {
            #region 通用检测


            if (!GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"9KtQz"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion
        }

        Thread thread;
        bool status = true;
        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox2.Text=="")
            {
                MessageBox.Show("请输入贴吧");
                return;
            }
            
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            this.textBox3.SelectionStart = this.textBox3.Text.Length;
            this.textBox3.SelectionLength = 0;
            this.textBox3.ScrollToCaret();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.textBox1.SelectionStart = this.textBox1.Text.Length;
            this.textBox1.SelectionLength = 0;
            this.textBox1.ScrollToCaret();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox3.Text = "";
        }
    }
}
