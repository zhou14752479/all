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

namespace 主程序202202
{
    public partial class 计算 : Form
    {
        public 计算()
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                //request.Proxy = null;//防止代理抓包
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
        string path = AppDomain.CurrentDomain.BaseDirectory;
        public void jisuan()
        {

            try
            {
                string now = DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒");
                double total = 0;
                StreamReader sr = new StreamReader(textBox1.Text, EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存

                for (int i = 0; i < text.Length; i++)
                {
                    //https://ugxlyxnlrg9udfdyzwnrvghlu2vydmvycg.blockjoy.com/v1/accounts/13aBmrJfLNPUP8W1grDu2RibjQ3xHmfnwNPHi3z5VRbwFNvSXk8
                    label2.Text = "正在计算第：" + (i + 1);
                    string url = "https://ugxlyxnlrg9udfdyzwnrvghlu2vydmvycg.blockjoy.com/v1/accounts/" + text[i];
                    string html = GetUrl(url, "utf-8");
                    string value = Regex.Match(html, @"""balance"":([\s\S]*?),").Groups[1].Value;
                    total = total + Convert.ToDouble(value) / 100000000;
                    textBox2.Text = total.ToString();
                    Thread.Sleep(100);

                    FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\" + now + ".txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                    sw.WriteLine(Convert.ToDouble(value) / 100000000);
                    sw.Close();
                    fs1.Close();
                    sw.Dispose();
                }
                MessageBox.Show("计算完成");
            }
            catch (Exception ex)
            {

               MessageBox.Show(ex.ToString());
            }
        }
        public void jisuan2()
        {
            double total = 0;
            StreamReader sr = new StreamReader(textBox1.Text, EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
            for (int i = 0; i <text.Length; i++)
            {
              
                label2.Text = "正在计算第：" + (i + 1);
                //https://ugxlyxnlrg9udfdyzwnrvghlu2vydmvycg.blockjoy.com/v1/accounts/13aBmrJfLNPUP8W1grDu2RibjQ3xHmfnwNPHi3z5VRbwFNvSXk8/rewards/sum?min_time=-48%20hour&max_time=2022-03-29T06%3A00%3A00.000Z&bucket=hour
                string url = "https://helium-api.stakejoy.com/v1/accounts/"+text[i]+ "/rewards/sum?min_time=-24%20hour&max_time="+DateTime.Now.ToString("yyyy-MM-dd")+"T10%3A00%3A00.000Z&bucket=hour";
                string html = GetUrl(url, "utf-8");
                MatchCollection values = Regex.Matches(html, @"""total"":([\s\S]*?),");
                for (int j = 0; j < values.Count; j++)
                {
                    total = total + Convert.ToDouble(values[j].Groups[1].Value);
                }
                
                textBox2.Text = total.ToString();
                Thread.Sleep(100);
                if (status == false)
                    return;
            }
            MessageBox.Show("计算完成");
        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            status = true;
            if(textBox1.Text=="")
            {
                MessageBox.Show("请导入文本");
                return;
            }


            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(jisuan);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 计算_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(jisuan2);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void 计算_FormClosing(object sender, FormClosingEventArgs e)
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

        bool status = true;
        private void button4_Click(object sender, EventArgs e)
        {
            status = false; 
        }
    }
}
