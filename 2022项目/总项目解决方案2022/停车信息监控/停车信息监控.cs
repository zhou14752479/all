﻿using System;
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

namespace 停车信息监控
{
    public partial class 停车信息监控 : Form
    {
        public 停车信息监控()
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
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
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

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData,string contenttype)
        {
            try
            {
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                request.ContentType = contenttype;

               // request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", "");

                request.Referer = "";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding("utf-8"));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
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
        public string getuids()
        {
            StringBuilder sb = new StringBuilder();
            string url = "http://wxpusher.zjiecode.com/api/fun/wxuser/v2?appToken=AT_Zwbx5uVZTIpxJ2OPaCGXXOZNuiWHmTKQ&page=1";

            string html = GetUrl(url, "utf-8");

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
                   
                    string uids = getuids();
                    string url = "http://wxpusher.zjiecode.com/api/send/message";
                    string postdata = "{\"appToken\":\"AT_Zwbx5uVZTIpxJ2OPaCGXXOZNuiWHmTKQ\",\"content\":\"" + title + neirong + "\",\"contentType\":2,\"uids\":[" + uids + "]}";
                    string html = PostUrl(url, postdata, "application/json");

               // MessageBox.Show(html);
                
            }
        }


        public void run()
        {
            try
            {
                
                string[] text = textBox4.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                StringBuilder sb = new StringBuilder();
              
                for (int i = 0; i < text.Length; i++)
                {
                    Thread.Sleep(Convert.ToInt32(textBox1.Text) * 1000);
                    if (text[i] != "")
                    {
                        string chepai = System.Web.HttpUtility.UrlEncode(text[i]);
                        string url = "https://parking.mixcapp.com/a/park/10055/ParkFee/GetParkFeeInit?_token=";
                        string postdata = "pid=18&plate="+chepai+"&barcode=&orgin=2";
                        string html = PostUrl(url, postdata, "application/x-www-form-urlencoded");
                        string time = Regex.Match(html, @"EntryTime\\"":\\""([\s\S]*?)\\""").Groups[1].Value;

                      
                        if (time != "")
                        {
                            TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks); TimeSpan ts2 = new TimeSpan(Convert.ToDateTime(time).Ticks);
                            TimeSpan ts = ts1.Subtract(ts2).Duration();
                            string dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟";
                           

                            ListViewItem lv1 = listView1.Items.Add(text[i]); //使用Listview展示数据
                          
                            lv1.SubItems.Add("读取成功");
                            lv1.SubItems.Add(dateDiff);

                            sb.Append(text[i] + "----" + dateDiff+ "<br>");
                        }
                        else
                        {
                            ListViewItem lv1 = listView1.Items.Add(text[i]); //使用Listview展示数据

                            lv1.SubItems.Add("读取成功");
                            lv1.SubItems.Add("无停车信息");
                        }
                        //else
                        //{
                        //    continue;
                        //}
                    }
                }

                if (checkBox3.Checked == true)
                {
                    sendmsg(textBox5.Text.Trim(), "<body><br>" + sb.ToString() + "</body>");
                }

            }
            catch (Exception ex)
            {
                textBox4.Text = ex.ToString();
                
            }
        }

        private void 停车信息监控_Load(object sender, EventArgs e)
        {
            #region 通用检测


            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"2DZkG"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName,EncodingType.GetTxtType(openFileDialog1.FileName));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                    textBox4.Text += text[i] + "\r\n";
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
          
            if (checkBox1.Checked==true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }

                timer1.Start();
                timer1.Interval = Convert.ToInt32(textBox2.Text)*1000*60;
            }
        }

        Thread thread;
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
            textBox4.Text = "";
        }
    }
}
