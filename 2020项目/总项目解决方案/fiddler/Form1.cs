using Fiddler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fiddler
{
    public partial class Form1 : Form
    {
        private const string Separator = "------------------------------------------------------------------";
        private UrlCaptureConfiguration CaptureConfiguration { get; set; }
        public Form1()
        {
            InitializeComponent();
            CaptureConfiguration = new UrlCaptureConfiguration();  // 配置设置
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "获取")
            {
                Start();//开始抓包
                button1.Text = "停止";
            }
            else
            {
                Stop();
                button1.Text = "获取";
            }
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
            CaptureConfiguration.CaptureDomain = "xcx.qcc.com";
            FiddlerApplication.AfterSessionComplete += FiddlerApplication_AfterSessionComplete;//加入会话事件
            FiddlerApplication.Startup(8888, true, true, true);//0:端口号1:是否注册系统代理2：是否应解密安全通信。如果为true，则需要应用程序文件夹中的MakeCert.exe 3：是否接受来自远程计算机的连接
                                                               // FiddlerApplication.Startup(8888,null);
        }
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

            }), output);

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
        }
    }
}
