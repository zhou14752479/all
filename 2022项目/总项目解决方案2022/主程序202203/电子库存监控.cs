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
namespace 主程序202203
{
    public partial class 电子库存监控 : Form
    {
        public 电子库存监控()
        {
            InitializeComponent();
        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset,string ip)
        {
            string html = "";
            string COOKIE = "";
            try
            {
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                //request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                WebProxy proxy = new WebProxy(ip);
                request.Proxy = proxy;
                request.KeepAlive = false;
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.Headers.Add("Accept-Language", "zh-cn,zh,en");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
               
                request.Accept = "*/*";
                request.Timeout = 5000;
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


        List<string> uids = new List<string>();
        private void getid()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "//id.txt";
            StreamReader sr = new StreamReader(path, method.EncodingType.GetTxtType(path));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != "")
                {
                    uids.Add(text[i]);
                }
            }
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
            label1.Text = "共获取到："+uids.Count.ToString();
        }
        List<string> iplist = new List<string>();
        private void getip()
        {
            string url = textBox2.Text;
            string html =method.GetUrl(url, "utf-8");
            string[] ips = html.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < ips.Length; i++)
            {
                iplist.Add(ips[i].Trim());
               
            }

        }


        string[] wuhuo = { "0现货", "最后售卖", "按订单供货", "0InStock" };
       
        private void 电子库存监控_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            #region 通用检测


            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"WxcM"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion
            getid();
            doSendMsg += Change;
            doSendMsg += SendMsgHander;//下载过程处理事件
        }

        #region 发送wxpusher消息
        public string getuids()
        {
            StringBuilder sb = new StringBuilder();
            string url = "http://wxpusher.zjiecode.com/api/fun/wxuser/v2?appToken=AT_Zwbx5uVZTIpxJ2OPaCGXXOZNuiWHmTKQ&page=1";

            string html = GetUrl(url, "utf-8","");

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
                string postdata = "{\"appToken\":\"AT_Zwbx5uVZTIpxJ2OPaCGXXOZNuiWHmTKQ\",\"content\":\"" + neirong + "\",\"contentType\":2,\"uids\":[\""+textBox3.Text.Trim()+"\"]}";
                string html = PostUrlDefault(url, postdata, "");

                // MessageBox.Show(html);

            }
        }

        #endregion

        
        public void run()
        {
            listView1.Items.Clear();
            getip();
            if (textBox2.Text == "")
            {

                MessageBox.Show("请导入代理IP");
                return;

            }

            for (int i = 0; i < uids.Count; i++)
            {
                if (uids[i] != "")
                {

                    ListViewItem item = listView1.Items.Add(new ListViewItem(new string[] { (listView1.Items.Count + 1).ToString(), uids[i], DateTime.Now.ToString(), "准备中" }));
                    int id = item.Index;
                    AddDown(id, uids[i]);
                }
            }
            StartDown();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Start();
            run();
        }

        List<Thread> list = new List<Thread>();
        public void AddDown(int id, string uid)
        {
            Thread tsk = new Thread(() =>
            {
                download(id,uid);
            });
            list.Add(tsk);
        }
        private void Change(DownMsg msg)
        {
            if (msg.Tag =="end")
            {
                StartDown(1);
            }
        }

        public void StartDown(int StartNum =20)
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

        public delegate void dlgSendMsg(DownMsg msg);
        public event dlgSendMsg doSendMsg;


        public class DownMsg
        {
            public int Id;
            public string Tag;
            public string status;

        }

        Dictionary<string, string> dics = new Dictionary<string, string>();
        private void download(int id,string uid)
        {
            DownMsg msg = new DownMsg();
            try
            {
               
                msg.Id = id;
                try
                {
                    bool wuhuomsg = false;
                    string url = "https://www.digikey.cn/zh/products/detail/analog-devices-inc/" + uid.ToString().Replace("#", "-");
                   // string ip = iplist[id];
                   
                    string html = GetUrl(url, "utf-8", textBox2.Text.Trim());
                    MatchCollection msgs = Regex.Matches(html, @"messages"":\[{""message"":""([\s\S]*?)""");
                    for (int i = 0; i < msgs.Count; i++)
                    {
                        int aid = Array.IndexOf(wuhuo, msgs[i].Groups[1].Value.Replace(" ","")); // 这里的1就是你要查找的值
                        if (aid != -1)//包含
                        {
                            wuhuomsg = true;
                            msg.status = msgs[i].Groups[1].Value;
                        }
                    }

                   

                    if(msgs.Count==0)
                    {
                        //msg.status = html;
                        msg.status = "0 现货";
                        msg.Tag = "end";
                    }
                    else if (wuhuomsg == false)
                    {
                        textBox1.Text += uid + "\r\n";
                        msg.status = "有货！";
                        string surl = "https://www.digikey.cn/zh/products/detail/analog-devices-inc/" + uid;

                       string youhuo = Regex.Match(html, @"messages"":\[{""message"":""([\s\S]*?)""").Groups[1].Value.Trim();
                        if (dics.ContainsKey(uid))
                        {
                            
                            if (dics[uid]!=youhuo)
                            {
                                dics[uid] = youhuo;
                                if (youhuo != "" && youhuo.Contains("货"))
                                {
                                    sendmsg(uid + "库存提醒", "<a href='" + surl + "'>" + surl + "</a>");
                                }
                            }
                            else
                            {
                                textBox1.Text += uid +"库存未变不提醒"+ "\r\n";
                            }
                        }
                        else
                        {
                            
                            if (youhuo != ""&&youhuo.Contains("货"))
                            {
                                dics.Add(uid, youhuo);
                                sendmsg(uid + "库存提醒", "<a href='" + surl + "'>" + surl + "</a>");
                            }
                        }

                        
                    }
                    msg.Tag = "end";
                    doSendMsg(msg);
                    
                }
                catch (Exception ex)
                {
                   // textBox1.Text = ex.ToString();
                    msg.Tag = "end";
                    msg.status = ex.ToString();
                }


            }
            catch (Exception ex)
            {
                //textBox1.Text = ex.ToString();
                msg.Tag = "end";
                msg.status = ex.Message;
            }
           

        }
        private void SendMsgHander(DownMsg msg)
        {

            this.Invoke((MethodInvoker)delegate ()
            {
                listView1.Items[msg.Id].SubItems[2].Text = DateTime.Now.ToString();
                listView1.Items[msg.Id].SubItems[3].Text = msg.status;
                Application.DoEvents();
            });



        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            run();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void 电子库存监控_FormClosing(object sender, FormClosingEventArgs e)
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
