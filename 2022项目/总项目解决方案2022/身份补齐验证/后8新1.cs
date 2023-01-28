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
using myDLL;

namespace 身份补齐验证
{
    public partial class 后8新1 : Form
    {
        public 后8新1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;

            }
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
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 10000;
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
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入文本");
                return;
            }


            status = true;

            cookie = method.getSetCookie("http://www.iy6.cn/?c=Public&action=verify");

            string acw_tc = Regex.Match(cookie, @"acw_tc=([\s\S]*?);").Groups[1].Value;
            string PHPSESSID = Regex.Match(cookie, @"PHPSESSID=([\s\S]*?);").Groups[1].Value;
            cookie = "acw_tc=" + acw_tc + ";PHPSESSID=" + PHPSESSID;

            yzm_ttshitu.cookie = cookie;
            yzm = (yzm_ttshitu.shibie("zhou14752479", "zhoukaige00", "http://www.iy6.cn/?c=Public&action=verify"));


          


            run();
        }



     Thread thread; 

        DateTime starttime;
        int successcount = 0;
        int threadcount = 0;
        int allcount = 0;

        int runcount = 0;
        
        List<string> lists = new List<string>();
        public void run()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            if (textBox1.Text == "")
            {

                MessageBox.Show("请导入数据");
                return;

            }

            StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存


            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != "")
                {
                    while(threadcount>5)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    Thread thread = new Thread(new ParameterizedThreadStart(download));
                    string o = text[i];
                    thread.Start((object)o);
               
                    threadcount++;
                }
            }
           
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
        public string PostUrlDefault(string url, string postData, string COOKIE)
        {
            try
            {

                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //WebProxy proxy = new WebProxy(ip);
                //request.Proxy = proxy;
                request.Method = "Post";

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
                //request.KeepAlive = true;

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


        string cookie = "";
        string yzm = "";

        private void download(object uid)
        {


            string html = method.GetUrl("http://www.iy6.cn/?c=Public&action=register&url=http%3A%2F%2Fwww.iy6.cn%2F", "utf-8");

            string title = Regex.Match(html, @"<title>([\s\S]*?)</title>").Groups[1].Value;
            if(title=="")
            {
                MessageBox.Show(html);
            }
            textBox3.Text += DateTime.Now.ToString("HH:mm:ss") + " " + title+ "----"+ "----成功" + "\r\n";
            threadcount--;
            //try
            //{


            //    try
            //    {


            //        bool jixu = false;
            //        string[] value = uid.ToString().Split(new string[] { "----" }, StringSplitOptions.None);

            //        if (value.Length < 2)
            //        {
            //           textBox3.Text= "格式错误";

            //            return;
            //        }

            //        for (int month = 1; month <= 12; month++)
            //        {
            //            if (jixu == true)
            //                break;
            //            for (int day = 1; day <= 31; day++)
            //            {
            //                if (jixu == true)
            //                    break;
            //                for (int x = 0; x < 100; x++)
            //                {
            //                    if (jixu == true)
            //                        break;
            //                    if (textBox3.Text.Length > 1000)
            //                        textBox3.Text = "";


            //                    string month2 = month.ToString();
            //                    if (month < 10)
            //                    {
            //                        month2 = "0" + month;
            //                    }
            //                    string day2 = day.ToString();
            //                    if (day < 10)
            //                    {
            //                        day2 = "0" + day;
            //                    }

            //                    string a2 = x.ToString();
            //                    if (x < 10)
            //                    {
            //                        a2 = "0" + x;
            //                    }


            //                    string name = System.Web.HttpUtility.UrlEncode(value[0].Trim());
            //                    string card = value[1].Trim().Replace("******", "abcdef");
            //                    card = card.Replace("abcd", month2 + day2);
            //                    card = card.Replace("ef", a2);

            //                    if (身份补齐验证.CheckIDCard18(card) == false)
            //                    {

            //                        continue;
            //                    }



            //                    string zimu = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456789";
            //                    Random rd = new Random(Guid.NewGuid().GetHashCode());
            //                    string suiji = "";
            //                    for (int a = 0; a < 10; a++)
            //                    {
            //                        int suijizimu = rd.Next(0, 60);
            //                        suiji = suiji + zimu[suijizimu];
            //                    }



            //                    string url = "http://www.iy6.cn/?c=Public&action=toregister";
            //                    string postdata = "username=" + suiji + "&password=" + suiji + "&pwdconfirm=" + suiji + "&realname=" + name + "&email=943510630%40qq.com&identity_card=" + card + "&verify=" + yzm + "&url=http%3A%2F%2Fwww.iy6.cn%2F";
            //                    string html = PostUrlDefault(url, postdata, cookie);


            //                    if (html.Contains("验证码错误"))
            //                    {
            //                        cookie = method.getSetCookie("http://www.iy6.cn/?c=Public&action=verify");

            //                      string   acw_tc = Regex.Match(cookie, @"acw_tc=([\s\S]*?);").Groups[1].Value;
            //                        string  PHPSESSID = Regex.Match(cookie, @"PHPSESSID=([\s\S]*?);").Groups[1].Value;
            //                        cookie = "acw_tc=" + acw_tc + ";PHPSESSID=" + PHPSESSID;

            //                        yzm_ttshitu.cookie = cookie;
            //                        yzm = (yzm_ttshitu.shibie("zhou14752479", "zhoukaige00", "http://www.iy6.cn/?c=Public&action=verify"));
            //                    }



            //                    string amsg = Regex.Match(html, @"""msg"":""([\s\S]*?)""").Groups[1].Value;
            //                    string astatus = Regex.Match(html, @"""status"":([\s\S]*?),").Groups[1].Value;

            //                    if (astatus == "0")
            //                    {
            //                        amsg = method.Unicode2String(amsg);
            //                        if (amsg == "今日注册已达上限")
            //                        {
            //                            successcount++;
            //                            threadcount--;
            //                            lists.Add(value[0].Trim());
            //                            System.TimeSpan t = DateTime.Now - starttime;
            //                            label4.Text = "运行用时：" + t.Hours + "时" + t.Minutes + "分" + t.Seconds + "秒，导入数据数量：" + allcount + " 正在验证第" + runcount + "个，成功数量：" + successcount;
            //                            textBox3.Text = DateTime.Now.ToString("HH:mm:ss") + " " + value[0] + "----" + card + "----成功" + "\r\n";


            //                            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\成功.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
            //                            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
            //                            sw.WriteLine(value[0] + "----" + card + "----成功");
            //                            sw.Close();
            //                            fs1.Close();
            //                            sw.Dispose();
            //                            jixu = true;
            //                            break;
            //                        }

            //                        else
            //                        {

            //                            System.TimeSpan t = DateTime.Now - starttime;
            //                            label4.Text = "运行用时：" + t.Hours + "时" + t.Minutes + "分" + t.Seconds + "秒，导入数据数量：" + allcount + " 正在验证第" + runcount + "个，成功数量：" + successcount;

            //                            string[] arr = { "身份证姓名匹配失败", "身份证姓名不符合", "身份信息有误" };
            //                            Random r = new Random();
            //                            textBox3.Text += DateTime.Now.ToString("HH:mm:ss") + "  " + value[0] + "----" + card + "----" + amsg.Replace("身份证与姓名不符", arr[rd.Next(3)]).Replace("身份证与姓名不符", "身份证与姓名未匹配") + "\r\n";
            //                        }



            //                    }
            //                    if (astatus == "1")
            //                    {
            //                        successcount++;
            //                        threadcount--;
            //                        lists.Add(value[0].Trim());
            //                        System.TimeSpan t = DateTime.Now - starttime;
            //                        label4.Text = "运行用时：" + t.Hours + "时" + t.Minutes + "分" + t.Seconds + "秒，导入数据数量：" + allcount + " 正在验证第" + runcount + "个，成功数量：" + successcount;



            //                        textBox3.Text = DateTime.Now.ToString("HH:mm:ss") + " " + value[0] + "----" + card + "----成功" + "\r\n";

            //                        FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\成功.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
            //                        StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
            //                        sw.WriteLine(value[0] + "----" + card + "----成功");
            //                        sw.Close();
            //                        fs1.Close();
            //                        sw.Dispose();

            //                        jixu = true;
            //                        break;

            //                    }

            //                    if (html.Contains("编码"))
            //                    {
            //                        successcount++;
            //                        threadcount--;
            //                        lists.Add(value[0].Trim());
            //                        System.TimeSpan t = DateTime.Now - starttime;
            //                        label4.Text = "运行用时：" + t.Hours + "时" + t.Minutes + "分" + t.Seconds + "秒，导入数据数量：" + allcount + " 正在验证第" + runcount + "个，成功数量：" + successcount;



            //                        textBox3.Text = DateTime.Now.ToString("HH:mm:ss") + " " + value[0] + "----" + card + "----成功" + "\r\n";

            //                        FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\编码占用.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
            //                        StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
            //                        sw.WriteLine(value[0] + "----" + card + "----成功");
            //                        sw.Close();
            //                        fs1.Close();
            //                        sw.Dispose();

            //                        jixu = true;
            //                        break;

            //                    }







            //                }

            //            }
            //        }






            //        threadcount--;

            //    }
            //    catch (Exception ex)
            //    {

            //        threadcount--;
            //        //textBox3.Text = ex.ToString();
            //    }


            //}
            //catch (Exception ex)
            //{

            //    threadcount--;
            //    //textBox3.Text = ex.Message;
            //}


        }



    


      

        bool status = true;









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


        #region  程序验证开启个数关闭
        public void getprocesscount()
        {
            int count = 0;
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                if (p.ProcessName == "身份补齐验证")
                {
                    count++;

                }
            }


            this.Text = "身份补齐验证中间6位(月份+后2)多开版本--------" + count;
            if (count > 4)
            {
                MessageBox.Show("超出多开限制");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

        }
        #endregion

        private void 后8新1_Load(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"rdj5c"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            getprocesscount();
            #endregion

            Control.CheckForIllegalCrossThreadCalls = false;

          
        }

        private void 后8新1_FormClosing(object sender, FormClosingEventArgs e)
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
