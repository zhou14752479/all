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
using System.Threading;
using System.Net;
using System.IO.Compression;

namespace fiddler
{
    public partial class fiddlerUse : Form
    {
        public fiddlerUse()
        {
            InitializeComponent();
            CaptureConfiguration = new UrlCaptureConfiguration();  // 配置设置
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


        #region 获取txt编码
        //调用：EncodingType.GetTxtType(textBox1.Text)
        public class EncodingType
        {
            /// <summary> 
            /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型 
            /// </summary> 
            /// <param name=“FILE_NAME“>文件路径</param> 
            /// <returns>文件的编码类型</returns> 
            public static System.Text.Encoding GetTxtType(string FILE_NAME)
            {
                FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
                Encoding r = GetType(fs);
                fs.Close();
                return r;
            }

            /// <summary> 
            /// 通过给定的文件流，判断文件的编码类型 
            /// </summary> 
            /// <param name=“fs“>文件流</param> 
            /// <returns>文件的编码类型</returns> 
            public static System.Text.Encoding GetType(FileStream fs)
            {
                byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
                byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
                byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM 
                Encoding reVal = Encoding.Default;

                BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
                int i;
                int.TryParse(fs.Length.ToString(), out i);
                byte[] ss = r.ReadBytes(i);
                if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
                {
                    reVal = Encoding.UTF8;
                }
                else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
                {
                    reVal = Encoding.BigEndianUnicode;
                }
                else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
                {
                    reVal = Encoding.Unicode;
                }
                r.Close();
                return reVal;

            }

            /// <summary> 
            /// 判断是否是不带 BOM 的 UTF8 格式 
            /// </summary> 
            /// <param name=“data“></param> 
            /// <returns></returns> 
            private static bool IsUTF8Bytes(byte[] data)
            {
                int charByteCounter = 1; //计算当前正分析的字符应还有的字节数 
                byte curByte; //当前分析的字节. 
                for (int i = 0; i < data.Length; i++)
                {
                    curByte = data[i];
                    if (charByteCounter == 1)
                    {
                        if (curByte >= 0x80)
                        {
                            //判断当前 
                            while (((curByte <<= 1) & 0x80) != 0)
                            {
                                charByteCounter++;
                            }
                            //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
                            if (charByteCounter == 1 || charByteCounter > 6)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        //若是UTF-8 此时第一位必须为1 
                        if ((curByte & 0xC0) != 0x80)
                        {
                            return false;
                        }
                        charByteCounter--;
                    }
                }
                if (charByteCounter > 1)
                {
                    throw new Exception("非预期的byte格式");
                }
                return true;
            }
        }

        #endregion







        private const string Separator = "------------------------------------------------------------------";
        private UrlCaptureConfiguration CaptureConfiguration { get; set; }

        public static string datas = "";
        private void button1_Click(object sender, EventArgs e)
        {
            

            Start();//开始抓包
            timer1.Start();
            button1.Text = "请打开程序";
            button1.Enabled = false;
           
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



            if (datas.Contains("code="))
            {
                //Stop();
                //timer1.Stop();

                string code = Regex.Match(datas, @"code=([\s\S]*?)&").Groups[1].Value.Trim();
                if (!codelist.Contains(code) && code.Trim() != "")
                {
                    codelist.Add(code);
                    textBox3.Text += code + "\r\n";
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
                StreamReader sr = new StreamReader(path + "code.txt",EncodingType.GetTxtType(path + "code.txt"));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
                File.Delete(path + "code.txt");
                // System.Diagnostics.Process.Start(@"‪C:\Users\Administrator\Desktop\农行微缴费.lnk");

                System.Diagnostics.Process.Start(path + "新疆专技继续教育.lnk");


            }
        }
        private void fiddlerUse_Load(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"Duq4"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();

            }



            #endregion


            tabControl1.SelectedIndex = 1;
            datas = "";
        }

        private void fiddlerUse_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
            timer1.Stop();
        }

       
    }
}
