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

namespace 台湾彩券
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
      
        public static string PostUrlDefault(string url, string postData, string COOKIE)
        {
            try
            {
                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; 
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
               
                request.ContentType = "application/x-www-form-urlencoded";
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
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

        public string cookie = "ASPSESSIONIDSGQDSDBR=EKBGDPBABGPIOBCKECEHDGDD; ASPSESSIONIDSGSBQBCR=DCJMDFIAPDOLEKLPGLIAADPD; _ga_QH8PJX46RG=GS1.1.1649578268.1.0.1649578268.60; _ga=GA1.3.140840180.1649578269; _gid=GA1.3.614156529.1649578269";
        public string VIEWSTATE = "";
        public string VIEWSTATEGENERATOR = "";
        public string EVENTVALIDATION = "";
        public string DropDownList1 = "2022%2F4";
        public string DropDownList2 = "1";

        public void first_run()
        {
            try
            {
                string url = "https://www.taiwanlottery.com.tw/lotto/BINGOBINGO/drawing.aspx";
                string html = PostUrlDefault(url, "", cookie);

                VIEWSTATE = Regex.Match(html, @"VIEWSTATE"" value=""([\s\S]*?)""").Groups[1].Value;
                VIEWSTATEGENERATOR = Regex.Match(html, @"VIEWSTATEGENERATOR"" value=""([\s\S]*?)""").Groups[1].Value;
                EVENTVALIDATION = Regex.Match(html, @"EVENTVALIDATION"" value=""([\s\S]*?)""").Groups[1].Value;

                VIEWSTATE = System.Web.HttpUtility.UrlEncode(VIEWSTATE);
                VIEWSTATEGENERATOR = System.Web.HttpUtility.UrlEncode(VIEWSTATEGENERATOR);
                EVENTVALIDATION = System.Web.HttpUtility.UrlEncode(EVENTVALIDATION);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string latestcode = "";
        bool all = true;
        Thread thread;
        public void run()
        {
            try
            {
                //if(VIEWSTATE=="")
                //{
                //    first_run();
                //}
                first_run();
                string url = "https://www.taiwanlottery.com.tw/lotto/BINGOBINGO/drawing.aspx";
                string postdata = string.Format("__EVENTTARGET=DropDownList2&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&__EVENTVALIDATION={2}&DropDownList1={3}&DropDownList2={4}", VIEWSTATE, VIEWSTATEGENERATOR, EVENTVALIDATION, DropDownList1, DropDownList2);

                string html = PostUrlDefault(url, postdata, cookie);
                string content = Regex.Match(html, @"<td class=tdA_3></td></tr><tr>([\s\S]*?)</table>").Groups[1].Value;
                MatchCollection values = Regex.Matches(content, @"<td class=tdA_([\s\S]*?)>([\s\S]*?)</td>");
                int count = values.Count / 5;

                if (!all)
                {
                    count = 1;
                }
                for (int i = 0; i < count; i++)
                {
                    
                   if(!all)
                    {
                        if (latestcode != "")
                        {
                            label1.Text = latestcode + "-----" + values[0].Groups[2].Value;
                            if (Convert.ToInt32(latestcode) != Convert.ToInt32(values[0].Groups[2].Value) - 1)
                            {
                                // textBox1.Text += "不更新"+values[0].Groups[2].Value + "\r\n";
                                break;
                            }
                            else
                            {
                                label1.Text = "更新" + values[0].Groups[2].Value + "\r\n";
                            }
                        }
                
                    }



                    latestcode = values[0].Groups[2].Value;
                    //textBox1.Text = DateTime.Now.ToString("HH:mm:ss") + "正在抓取：" + latestcode;
                    string o1 = values[5 * i].Groups[2].Value;
                    string o2 = values[(5 * i) + 1].Groups[2].Value;
                    string o3 = values[(5 * i) + 2].Groups[2].Value;
                    string o4 = values[(5 * i) + 3].Groups[2].Value;
                    string o5 = values[(5 * i) + 4].Groups[2].Value;
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                    lv1.SubItems.Add(o1);
                    lv1.SubItems.Add(o2);
                    lv1.SubItems.Add(o3);
                    lv1.SubItems.Add(o4);
                    lv1.SubItems.Add(o5);

                    StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\data.json", EncodingType.GetTxtType(AppDomain.CurrentDomain.BaseDirectory + "\\data.json"));
                    //一次性读取完 
                    string texts = sr.ReadToEnd();
                    string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    
                    sr.Close();  //只关闭流
                    sr.Dispose();   //销毁流内存



                    FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\data.json", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                    sw.WriteLine(text[0]);
                    sw.WriteLine(" { \"期别\":\"" + o1 + "\",\"奖号\":\"" + o2+ "\",\"超级奖号\":\"" + o3+ "\",\"猜大小\":\"" + o4+ "\",\"猜单双\":\"" + o5+"\" },");
                  

                    for (int j = 1; j < text.Length; j++)
                    {
                        sw.WriteLine(text[j]);
                    }
                    sw.Close();
                    fs1.Close();
                    sw.Dispose();

                }


            }
            catch (Exception ex)
            {

                label1.Text = ex.ToString();
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            all = false;
            label1.Text = "开始监控抓取....";
            timer1.Start();
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            all = false;
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

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
