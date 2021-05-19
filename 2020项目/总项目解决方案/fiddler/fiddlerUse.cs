using Fiddler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using System.Threading;

namespace fiddler
{
    public partial class fiddlerUse : Form
    {
        public fiddlerUse()
        {
            InitializeComponent();
            CaptureConfiguration = new UrlCaptureConfiguration();  // 配置设置
        }
        private const string Separator = "------------------------------------------------------------------";
        private UrlCaptureConfiguration CaptureConfiguration { get; set; }

        public static string datas = "";
        private void button1_Click(object sender, EventArgs e)
        {
            Start();//开始抓包
            timer1.Start();
            button1.Text = "请打开程序";
           
        }
        /// <summary>
        /// 停止抓包
        /// </summary>
        void Stop()
        {
            FiddlerApplication.AfterSessionComplete -= FiddlerApplication_AfterSessionComplete;

            if (FiddlerApplication.IsStarted())
                FiddlerApplication.Shutdown();
        }

        /// <summary>
        /// 开始抓包
        /// </summary>
        void Start()
        {

            CaptureConfiguration.IgnoreResources = true;
            int procId = 0;
            CaptureConfiguration.ProcessId = procId;//进程ID
                                                    //  CaptureConfiguration.CaptureDomain = "auction.hmqqw.com";//捕捉域
                                                    
            CaptureConfiguration.CaptureDomain =textBox2.Text.Trim();
            FiddlerApplication.AfterSessionComplete += FiddlerApplication_AfterSessionComplete;//加入会话事件
            FiddlerApplication.Startup(8888, true, true, true);//0:端口号1:是否注册系统代理2：是否应解密安全通信。如果为true，则需要应用程序文件夹中的MakeCert.exe 3：是否接受来自远程计算机的连接
                                                               // FiddlerApplication.Startup(8888,null);
        }

        string head = "";
        /// <summary>
        /// 会话事件
        /// </summary>
        /// <param name="sess"></param>
        private void FiddlerApplication_AfterSessionComplete(Session sess)
        {
            // Ignore HTTPS connect requests
            if (sess.RequestMethod == "CONNECT")
                return;

            if (CaptureConfiguration.ProcessId > 0)
            {
                if (sess.LocalProcessID != 0 && sess.LocalProcessID != CaptureConfiguration.ProcessId)
                    return;
            }

            if (!string.IsNullOrEmpty(CaptureConfiguration.CaptureDomain))
            {
                if (sess.hostname.ToLower() != CaptureConfiguration.CaptureDomain.Trim().ToLower())
                    return;
            }

            if (CaptureConfiguration.IgnoreResources)
            {
                string url = sess.fullUrl.ToLower();
                head = url;
                var extensions = CaptureConfiguration.ExtensionFilterExclusions;
                foreach (var ext in extensions)
                {
                    if (url.Contains(ext))
                        return;
                }

                var filters = CaptureConfiguration.UrlFilterExclusions;
                foreach (var urlFilter in filters)
                {
                    if (url.Contains(urlFilter))
                        return;
                }
            }

            if (sess == null || sess.oRequest == null || sess.oRequest.headers == null)
                return;

            string headers = sess.oRequest.headers.ToString();
            var reqBody = Encoding.UTF8.GetString(sess.RequestBody);
            var resPonseBody = Encoding.UTF8.GetString(sess.ResponseBody);


           
            // if you wanted to capture the response
            //string respHeaders = session.oResponse.headers.ToString();
            //var respBody = Encoding.UTF8.GetString(session.ResponseBody);

            // replace the HTTP line to inject full URL
            string firstLine = sess.RequestMethod + " " + sess.fullUrl + " " + sess.oRequest.headers.HTTPVersion;
            int at = headers.IndexOf("\r\n");
            if (at < 0)
                return;
            headers = firstLine + "\r\n" + headers.Substring(at + 1);

            string output = headers + "\r\n" +
                            (!string.IsNullOrEmpty(reqBody) ? reqBody + "\r\n" : string.Empty) +
                            Separator + "\r\n\r\n" + Separator + "\r\n" + (!string.IsNullOrEmpty(resPonseBody) ? resPonseBody + "\r\n" : string.Empty) + "\r\n\r\n";

           
            // 跨线程更新UI
            BeginInvoke(new Action<string>((text) =>
            {

                textBox1.AppendText(text);
                // datas = datas + text;  //公共值用于其他软件,累加抓包数据
                datas = text; //不累加抓包数据
            }), output);
           
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;

        ArrayList codelist = new ArrayList();
        Thread thread;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(readtxt);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }



            if (datas.Contains("mobile_id"))
            {
                //Stop();
                //timer1.Stop();

                string code = Regex.Match(datas, @"cookie:([\s\S]*?)R").Groups[1].Value.Trim();
                if (!codelist.Contains(code) && code.Trim() != "")
                {
                  
                    textBox3.Text = code + "\r\n";
                    FileStream fs1 = new FileStream(path + "getcode.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine(code);
                    sw.Close();
                    fs1.Close();
                    sw.Dispose();
                }

            }


        }
        public void readtxt()
        {
            if (File.Exists(path + "code.txt"))
            {
                StreamReader sr = new StreamReader(path + "code.txt", myDLL.method.EncodingType.GetTxtType(path + "code.txt"));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
                File.Delete(path + "code.txt");
                // System.Diagnostics.Process.Start(@"‪C:\Users\Administrator\Desktop\农行微缴费.lnk");

                System.Diagnostics.Process.Start(path + "农行微缴费.lnk");


            }
        }
        private void fiddlerUse_Load(object sender, EventArgs e)
        {
            datas = "";
        }

        private void fiddlerUse_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
            timer1.Stop();
        }

       
    }
}
