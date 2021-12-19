using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202110
{
    public partial class 互动易网站监控 : Form
    {
        [DllImport("kernel32.dll")]
        public static extern bool Beep(int freq, int duration);

        [DllImport("winmm.dll")]
        public static extern uint mciSendString(string lpstrCommand, string lpstrReturnString, uint uReturnLength, uint hWndCallback);
        public 互动易网站监控()
        {
            InitializeComponent();
        }
        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";

                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("appid:orders");
                //headers.Add("x-nike-visitid:5");
                //headers.Add("x-nike-visitorid:d03393ee-e42c-463e-9235-3ca0491475b4");
                //添加头部
            
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
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
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            bool flag = openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamReader streamReader = new StreamReader(openFileDialog1.FileName, Encoding.GetEncoding("utf-8"));
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    textBox1.Text += array[i] + "\r\n";

                }

            }
        }

        List<string> lists = new List<string>();
        public string guolv(string title)
        {

            if (textBox1.Text == "")
            {
                return "1";
            }
            try
            {

                string[] array = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].Trim() != "")
                    {
                        if (title.Contains(array[i]))
                        {
                            return array[i];
                        }
                    }
                }
                return "";
            }
            catch (Exception)
            {
                return "";

            }
        }



        public static string zhi = "";


        public string key;
        弹出页面 t;
        public void run()
        {
            try
            {
               
                string url = "http://irm.cninfo.com.cn/ircs/index/search";
                string postdata = "pageNo=1&pageSize=20&searchTypes=1%2C11%2C&market=&industry=&stockCode=";
                string html = PostUrl(url, postdata, "", "utf-8");
                MatchCollection ahtmls = Regex.Matches(html, @"""esId""([\s\S]*?)}");
             
                for (int i = 0; i < ahtmls.Count; i++)
                {
                    label1.Text= DateTime.Now.ToLongTimeString() +"：正在监控...";
                    string title = Regex.Match(ahtmls[i].Groups[1].Value, @"""mainContent"":""([\s\S]*?)""").Groups[1].Value;
                    string esId = Regex.Match(ahtmls[i].Groups[1].Value, @":""([\s\S]*?)""").Groups[1].Value;
                    string date = Regex.Match(ahtmls[i].Groups[1].Value, @"packageDate"":""([\s\S]*?)""").Groups[1].Value;
                    string companyShortName = Regex.Match(ahtmls[i].Groups[1].Value, @"companyShortName"":""([\s\S]*?)""").Groups[1].Value;
                    string stockCode = Regex.Match(ahtmls[i].Groups[1].Value, @"stockCode"":""([\s\S]*?)""").Groups[1].Value;
                    string answer = Regex.Match(ahtmls[i].Groups[1].Value, @"""attachedContent"":""([\s\S]*?)""").Groups[1].Value;

                    string keyword = guolv(title);
                  
                    if (keyword != "")
                    {

                        if (answer != "")
                        {
                            if (!lists.Contains(title))
                            {
                                //触发提醒
                                lists.Add(title);

                              
                                string value = companyShortName + "(" + stockCode + ")---" + esId + "----" + date + "\r\n" + "问：" + title + "\r\n" + "答：" + answer + "\r\n";
                                key = keyword;
                                richTextBox1.Text =value + "\r\n\r\n" + richTextBox1.Text; 
                                zhi = value;
                              
                                // Beep(800, 800);
                                // MessageBox.Show(companyShortName+"("+ stockCode + ")---"+esId+"----"+date+"\r\n"+ "问：" +title+"\r\n"+"答："+answer+"\r\n");
                                //t = new 弹出页面();
                                // t.Show() ;

                                new System.Threading.Thread((System.Threading.ThreadStart)delegate { Application.Run(new 弹出页面(keyword)); }).Start();
                                mciSendString("play" + " " + @"1.wav", null, 0, 0);

                            }
                        }


                    }

                }
              

            }
            catch (Exception ex)
            {

               
            }
        }


        Thread thread;
        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void button1_Click(object sender, EventArgs e)
        {
            FileStream fs1 = new FileStream(path + "keyword.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
            sw.WriteLine(textBox1.Text);
            sw.Close();
            fs1.Close();
            sw.Dispose();
            timer1.Start();

            Control.CheckForIllegalCrossThreadCalls = false;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
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
        private void 互动易网站监控_Load(object sender, EventArgs e)
        {
            try
            {
                StreamReader sr = new StreamReader(path + "keyword.txt", EncodingType.GetTxtType(path + "keyword.txt"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                    textBox1.Text += text[i] + "\r\n";
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
            catch (Exception)
            {

               
            }

            #region 通用检测


            string html = GetUrl("http://acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"DZkGm"))
            {

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            #endregion
        }
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
        private void 互动易网站监控_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            char[] str = richTextBox1.Text.ToCharArray();

            for (int i = 0; i < str.Length; i++)
            {

                int p1 = richTextBox1.Text.IndexOf(key, i);
                if (p1 != -1)
                {
                    richTextBox1.Select(p1, key.Length);
                    richTextBox1.SelectionColor = Color.Red;
                    FontStyle style = richTextBox1.SelectionFont.Style;
                    style = FontStyle.Bold;
                    richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, style);

                }
            }
            richTextBox1.Refresh();
        }
    }
}
