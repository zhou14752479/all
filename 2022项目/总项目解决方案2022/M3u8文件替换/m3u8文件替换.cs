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

namespace M3u8文件替换
{
    public partial class m3u8文件替换 : Form
    {
        public m3u8文件替换()
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
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("appid:orders");
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

        #region 获取页面跳转后Url
        static string fanhuiurl(string originalAddress)
        {
            string redirectUrl;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            WebRequest myRequest = WebRequest.Create(originalAddress);

            WebResponse myResponse = myRequest.GetResponse();
            redirectUrl = myResponse.ResponseUri.ToString();

            myResponse.Close();
            return redirectUrl;
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
       

        private void m3u8文件替换_Load(object sender, EventArgs e)
        {
            #region 通用检测


            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"vXgp"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion
            Control.CheckForIllegalCrossThreadCalls = false;
            textBox2.Text= AppDomain.CurrentDomain.BaseDirectory;
            doSendMsg += Change;
            doSendMsg += SendMsgHander;//下载过程处理事件
        }

        private void button2_Click(object sender, EventArgs e)
        {
            run();
        }
        public void run()
        {
            
            if (textBox2.Text == "")
            {

                MessageBox.Show("请选择保存文件夹");
                return;

            }
            if (listView1.Items.Count ==0)
            {

                MessageBox.Show("请添加文件");
                return;

            }

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].SubItems[1].Text != "")
                {

                    int id = Convert.ToInt32(listView1.Items[i].SubItems[0].Text)-1;
                    AddDown(id, listView1.Items[i].SubItems[1].Text);
                }
            }
            StartDown();
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

        public void StartDown(int StartNum = 10)
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
        private void download(int id, string uid)
        {
            DownMsg msg = new DownMsg();
            try
            {

                msg.Id = id;
                try
                {
                    StringBuilder sb = new StringBuilder();
                    StreamReader sr = new StreamReader(uid,EncodingType.GetTxtType(uid));
                    //一次性读取完 
                    string texts = sr.ReadToEnd();
                    string[] text = texts.Split(new string[] { "\n" }, StringSplitOptions.None);
                    for (int i = 0; i < text.Length; i++)
                    {
                        if(text[i].Contains("http"))
                        {
                            if (text[i].Contains(textBox1.Text.Trim()))
                            {
                                string redictUrl = fanhuiurl(text[i]);
                                sb.Append(redictUrl + "\n");
                            }
                            else
                            {
                                sb.Append(text[i] + "\n");
                            }
                        }
                        else
                        {
                            sb.Append(text[i]+"\n");
                        }
                    }
                    sr.Close();  //只关闭流
                    sr.Dispose();   //销毁流内存


                    System.IO.File.WriteAllText(textBox2.Text + "\\" + Path.GetFileName(uid), sb.ToString(), Encoding.UTF8);
                    msg.status = "完成";
                    msg.Tag = "end";
                    doSendMsg(msg);

                }
                catch (Exception ex)
                {
                    textBox1.Text = ex.ToString();
                    msg.Tag = "end";
                    msg.status = ex.ToString();
                }


            }
            catch (Exception ex)
            {
                textBox1.Text = ex.ToString();
                msg.Tag = "end";
                msg.status = ex.Message;
            }


        }
        private void SendMsgHander(DownMsg msg)
        {

            this.Invoke((MethodInvoker)delegate ()
            {
                listView1.Items[msg.Id].SubItems[2].Text = msg.status;
                Application.DoEvents();
            });



        }
      
        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            Array filePath = ((Array)e.Data.GetData(DataFormats.FileDrop));

            foreach (var item in filePath)
            {
                ListViewItem item2 = listView1.Items.Add(new ListViewItem(new string[] { (listView1.Items.Count + 1).ToString(), item.ToString(), "准备中" }));
            }

        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
            listView1.Items.Clear();
        }
    }
}
