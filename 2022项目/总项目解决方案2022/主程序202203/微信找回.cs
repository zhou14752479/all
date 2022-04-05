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
    public partial class 微信找回 : Form
    {
        public 微信找回()
        {
            InitializeComponent();
        }
        
        public void run()
        {
            listView1.Items.Clear();
           

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
        List<string> uids = new List<string>();
        private void getid()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "//微信账号.txt";
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
           
        }

        List<string> iplist = new List<string>();
        private void getip()
        {
            string url = textBox1.Text;
            string html = method.GetUrl(url, "utf-8");
            string[] ips = html.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < ips.Length; i++)
            {
                iplist.Add(ips[i].Trim());
              
            }

        }


        private void button2_Click(object sender, EventArgs e)
        {
            //if(textBox1.Text=="")
            //{
            //    MessageBox.Show("请输入代理IP链接");
            //    return;
            //}
            //getip();
            timer1.Start();
            run();
        }

        private void 微信找回_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            #region 通用检测


            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"借款应用"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion
            getid();
            doSendMsg += Change;
            doSendMsg += SendMsgHander;//下载过程处理事件
        }
        List<Thread> list = new List<Thread>();
        public void AddDown(int id, string uid)
        {
            Thread tsk = new Thread(() =>
            {
                download(id, uid);
            });
            list.Add(tsk);
        }
        private void Change(DownMsg msg)
        {
            if (msg.Tag == "end")
            {
                StartDown(1);
            }
        }

        public void StartDown(int StartNum = 20)
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

        #region GET使用代理IP请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public  string GetUrlwithIP(string Url, string ip,  string charset)
        {
            string html = "";

            try
            {
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36";
                WebProxy proxy = new WebProxy(ip);
                request.Proxy = proxy;
               
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
        private void download(int id, string uid)
        {
            DownMsg msg = new DownMsg();
            try
            {

                msg.Id = id;
                try
                {

                     string url = "https://weixin110.qq.com/security/frozen?action=1&lang=zh_CN&step=1&callback=jQuery11130782855431167033_1648609656614&deviceid=&operator=&accttype=wxid&acctcc=86&acct=" + uid.ToString() + "&imgcode=gffg&&_=1648609656617";

                    //Random rd = new Random(Guid.NewGuid().GetHashCode()); //生成不重复的随机数，默认的话根据时间戳如果太快会相同
                    //int suiji = rd.Next(1, iplist.Count);
                    //string ip = iplist[suiji];

                    // string html = GetUrlwithIP(url, ip,  "utf-8");
                    string html =method.GetUrl(url,"utf-8");
                    while (true)
                    {
                        html = method.GetUrl(url, "utf-8");
                        if (html.Contains("频繁"))
                        {
                            msg.status = "频繁";
                            msg.Tag = "end";
                            break;
                        }
                        else
                        {
                            msg.status = html;
                        }

                    }

                    //    if (html.Contains("频繁")) { 
                    //        msg.status = "频繁";
                    //}
                    //else{
                    //    msg.status = "1";
                    //}

                    msg.Tag = "end";

                    doSendMsg(msg);

                }
                catch (Exception ex)
                {
                    //textBox1.Text = ex.ToString();
                    msg.Tag = "end";
                    msg.status = ex.ToString();
                    textBox1.Text = ex.ToString();
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

        private void 微信找回_FormClosing(object sender, FormClosingEventArgs e)
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

        int count = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            count = count + 1;
            if(count>100)
            {
                getip();
                count = 0;
            }
            run();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
    }
}
