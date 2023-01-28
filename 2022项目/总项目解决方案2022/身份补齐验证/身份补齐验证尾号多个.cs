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
using myDLL;

namespace 身份补齐验证
{
    public partial class 身份补齐验证尾号多个 : Form
    {
        public 身份补齐验证尾号多个()
        {
            InitializeComponent();
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
        public  string PostUrlDefault(string url, string postData, string COOKIE)
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

        public string getip()
        {
            string ip = method.GetUrl(textBox2.Text.Trim(),"utf-8");
            return ip;
        }
        string ip = "";
        
        private void button2_Click(object sender, EventArgs e)
        {

            ip= getip();    
            Control.CheckForIllegalCrossThreadCalls = false;
            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入数据");
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

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        public void run()
        {
          
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

                    AddDown(text[i]);
                }
            }
            StartDown(Convert.ToInt32(numericUpDown1.Value));//开始线程
        }

        public void AddDown(string uid)
        {
            Thread tsk = new Thread(() =>
            {
                download( uid);
            });
            list.Add(tsk);
        }

        public void StartDown(int StartNum)
        {

            for (int i2 = 0; i2 < StartNum; i2++)
            {
                lock (list)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].ThreadState == System.Threading.ThreadState.Unstarted || list[i].ThreadState == ThreadState.Suspended)
                        {
                            list[i].Start();
                            break;
                        }
                    }
                }
            }

        }

        string cookie = "";
        string yzm = "";



        DateTime starttime;
        int successcount = 0;
        int threadcount = 0;
        int allcount = 0;

        int runcount = 0;

        List<string> lists = new List<string>();
        private void download( string uid)
        {
            DownMsg msg = new DownMsg();
            try
            {

               
                try
                {


                    bool jixu = false;
                    string[] value = uid.Split(new string[] { "----" }, StringSplitOptions.None);

                    if (value.Length < 2)
                    {
                        textBox3.Text = "格式错误";
                        msg.Tag = "end";
                        doSendMsg(msg);
                        return;
                    }

                    string[] arrs = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "X" };

                    for (int month = 0; month <= 999; month++)
                    {
                        if (jixu == true)
                            break;
                        foreach (string item in arrs)
                        {

             

                            if (textBox3.Text.Length > 1000)
                                textBox3.Text = "";


                            string month2 = month.ToString();
                            //if (month < 10)
                            //{
                            //    month2 = "0" + month;
                            //}

                            //string day2 = day.ToString();
                            //if (day < 10)
                            //{
                            //    day2 = "0" + day;
                            //}

                            if (month < 10)
                            {
                                month2 = "00" + month;
                            }

                           else if (month < 100 && month>9)
                            {
                                month2 = "0" + month;
                            }
                            string day2 = item.ToString();
                            

                            string name = System.Web.HttpUtility.UrlEncode(value[0].Trim());
                            string card = value[1].Trim().Replace("****", month2 + day2);

                            if (身份补齐验证.CheckIDCard18(card) == false)
                            {

                                continue;
                            }

                            //身份证号码上的第17位数字表示性别：奇数表示男性，偶数表示女性
                            if (checkBox3.Checked == true)
                            {
                                //MessageBox.Show(card);
                                //MessageBox.Show(card.Substring(16, 1));
                                string sex = value[2].Trim();
                                if (sex == "男")
                                {
                                    if (Convert.ToInt32(card.Substring(16, 1)) % 2 == 0)
                                    {
                                        textBox3.Text += card + "性别不符合" + "\r\n";
                                        continue;
                                    }
                                }
                                if (sex == "女")
                                {
                                    if (Convert.ToInt32(card.Substring(16, 1)) % 2 == 1)
                                    {
                                        textBox3.Text += card + "性别不符合" + "\r\n";
                                        continue;
                                    }

                                }
                            }

                            string zimu = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456789";
                            Random rd = new Random(Guid.NewGuid().GetHashCode());
                            string suiji = "";
                            for (int a = 0; a < 10; a++)
                            {
                                int suijizimu = rd.Next(0, 60);
                                suiji = suiji + zimu[suijizimu];
                            }



                            string url = "http://www.iy6.cn/?c=Public&action=toregister";
                            string postdata = "username=" + suiji + "&password=" + suiji + "&pwdconfirm=" + suiji + "&realname=" + name + "&email=943510630%40qq.com&identity_card=" + card + "&verify=" + yzm + "&url=http%3A%2F%2Fwww.iy6.cn%2F";
                            string html = PostUrlDefault(url, postdata, cookie);
                          
                            
                            //if(html.Contains("验证码错误"))
                            //{
                            //    yzm = (yzm_ttshitu.shibie("2030017712", "123q123q", "http://www.iy6.cn/?c=Public&action=verify"));
                            //    Thread.Sleep(5000);
                            //}
                            string amsg = Regex.Match(html, @"""msg"":""([\s\S]*?)""").Groups[1].Value;
                            string astatus = Regex.Match(html, @"""status"":([\s\S]*?),").Groups[1].Value;

                            if (astatus == "0")
                            {
                                amsg = method.Unicode2String(amsg);
                                if (amsg == "今日注册已达上限")
                                {
                                    successcount++;
                                    threadcount--;
                                    lists.Add(value[0].Trim());
                                    System.TimeSpan t = DateTime.Now - starttime;
                                    label4.Text = "运行用时：" + t.Hours + "时" + t.Minutes + "分" + t.Seconds + "秒，导入数据数量：" + allcount + " 正在验证第" + runcount + "个，成功数量：" + successcount;
                                    textBox3.Text += value[0] + "----" + card + "----成功" + "\r\n";


                                    FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\成功.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                                    StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                                    sw.WriteLine(value[0] + "----" + card + "----成功");
                                    sw.Close();
                                    fs1.Close();
                                    sw.Dispose();
                                    jixu = true;
                                    break;
                                }

                                else
                                {

                                    System.TimeSpan t = DateTime.Now - starttime;
                                    label4.Text = "运行用时：" + t.Hours + "时" + t.Minutes + "分" + t.Seconds + "秒，导入数据数量：" + allcount + " 正在验证第" + runcount + "个，成功数量：" + successcount;

                                    string[] arr = { "身份证姓名匹配失败", "身份证姓名不符合", "身份信息有误" };
                                    Random r = new Random();
                                    textBox3.Text += value[0] + "----" + card + "----" + amsg.Replace("身份证与姓名不符", arr[rd.Next(3)]).Replace("身份证与姓名不符", "身份证与姓名未匹配") + "\r\n";
                                }



                            }
                            if (astatus == "1")
                            {
                                successcount++;
                                threadcount--;
                                lists.Add(value[0].Trim());
                                System.TimeSpan t = DateTime.Now - starttime;
                                label4.Text = "运行用时：" + t.Hours + "时" + t.Minutes + "分" + t.Seconds + "秒，导入数据数量：" + allcount + " 正在验证第" + runcount + "个，成功数量：" + successcount;



                                textBox3.Text += value[0] + "----" + card + "----成功" + "\r\n";

                                FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\成功.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                                StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                                sw.WriteLine(value[0] + "----" + card + "----成功");
                                sw.Close();
                                fs1.Close();
                                sw.Dispose();

                                jixu = true;
                                break;

                            }

                            if (html.Contains("编码"))
                            {
                                successcount++;
                                threadcount--;
                                lists.Add(value[0].Trim());
                                System.TimeSpan t = DateTime.Now - starttime;
                                label4.Text = "运行用时：" + t.Hours + "时" + t.Minutes + "分" + t.Seconds + "秒，导入数据数量：" + allcount + " 正在验证第" + runcount + "个，成功数量：" + successcount;



                                textBox3.Text += value[0] + "----" + card + "----成功" + "\r\n";

                                FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\编码占用.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                                StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                                sw.WriteLine(value[0] + "----" + card + "----成功");
                                sw.Close();
                                fs1.Close();
                                sw.Dispose();

                                jixu = true;
                                break;

                            }









                        }
                    }





                 
                    msg.Tag = "end";
                    doSendMsg(msg);

                }
                catch (Exception ex)
                {
                    
                    msg.Tag = "end";
                    //textBox3.Text = ex.ToString();
                }


            }
            catch (Exception ex)
            {

                msg.Tag = "end";
                //textBox3.Text = ex.Message;
            }


        }







        

        public delegate void dlgSendMsg(DownMsg msg);
        public event dlgSendMsg doSendMsg;


        public class DownMsg
        {
           
            public string Tag;
            public string status;

        }

        private void Change(DownMsg msg)
        {
            //按下停止键
            if (status == false)
                return;

            if (msg.Tag == "end")
            {
                StartDown(1);
            }
        }


        List<Thread> list = new List<Thread>();

        bool status = true;
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


            this.Text = "身份补齐验证后四位--------" + count;
            if (count > 2)
            {
                MessageBox.Show("超出多开限制");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

        }
        #endregion
        private void 身份补齐验证尾号多个_Load(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"rdj5c"))
            {
                //TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            getprocesscount();
            #endregion
            doSendMsg += Change;


            method.SetFeatures(11000);
           

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            // openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "TXT文本文件|*.txt";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;



            }
        }

        private void 身份补齐验证尾号多个_FormClosing(object sender, FormClosingEventArgs e)
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

        private void timer1_Tick(object sender, EventArgs e)
        {
           ip= getip();
        }
    }
}
