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

namespace TRC监控
{
    public partial class ERC监控 : Form
    {
        [DllImport("kernel32.dll")]
        public static extern bool Beep(int freq, int duration);
        [DllImport("user32.dll")]
        public static extern int MessageBoxTimeoutA(IntPtr hWnd, string msg, string Caps, int type, int Id, int time);//引用DLL

        public ERC监控()
        {
            InitializeComponent();
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

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {
            string charset = "utf-8";
            string html = "";
            string COOKIE = "cf_clearance=8IzimqOjoHRbgEoOa7TGYXS26YFNbAmu6cACWrHZUkk-1640060480-0-250; ASP.NET_SessionId=olomf2bfyhrled5h3kmn4ang; _pk_id.10.1f5c=026743d19da0e740.1640060484.; __cf_bm=YjMr2SAiQcRsl5S2Zad9LMJsWxluJwPVcjM13AGezWI-1640064010-0-AULnS+WyC3Ua1KHPcSELpg0aP2UnvjQ2ghNxpQ0wlEGVodytXdfhLcVgz6vkDXENHLA6PhSBZGyNsyeNPOKXAOK9IzDPMfxZ/UDOOg8CD1dpnwUE9V0kWinIOt6viMxJKg==";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                //request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.93 Safari/537.36";
                request.Referer = Url;
                //添加头部
                WebHeaderCollection headers = request.Headers;
                //headers.Add("x-apiKey:LWIzMWUtNDU0Ny05Mjk5LWI2ZDA3Yjc2MzFhYmEyYzkwM2NjfDI3NTExNzEyNTUyNzAxMTA=");
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

        private DateTime ConvertStringToDateTime(string timeStamp)
        {

            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long mTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(mTime);
            return startTime.Add(toNow);
        }


        public void run()
        {


            for (int i = 0; i < listView1.Items.Count; i++)
            {
                try
                {
                    label4.Text = DateTime.Now.ToLongTimeString() + "：正在监控......";
                    if (listView1.Items[i].SubItems[1].Text == "")
                        continue;




                    //慢
                    // string url = "https://info.chaindigg.com/api/api/addressToken?hash="+  listView1.Items[i].SubItems[1].Text + "&tokenHash=0xdac17f958d2ee523a2206206994597c13d831ec7&pageNumber=0&pageSize=30&coinType=eth";

                    //快 但是加密
                    //string url = "https://www.oklink.com/api/explorer/v1/eth/addresses/"+ listView1.Items[i].SubItems[1].Text + "/transfers?t=1640060144159&offset=0&limit=20";

                    string url = "https://cn.etherscan.com/address-tokenpage?m=normal&a=" + listView1.Items[i].SubItems[1].Text;
                    string html = GetUrl(url);
                  
                    //MatchCollection times = Regex.Matches(html, @"""blocktime"":([\s\S]*?),");
                    //MatchCollection values = Regex.Matches(html, @"""realValue"":([\s\S]*?),");
                    //MatchCollection tokenSymbols = Regex.Matches(html, @"""symbol"":""([\s\S]*?)""");
                    //MatchCollection txids = Regex.Matches(html, @"""txHash"":""([\s\S]*?)""");
                    //MatchCollection from = Regex.Matches(html, @"""from"":""([\s\S]*?)""");
                    //MatchCollection to = Regex.Matches(html, @"""to"":""([\s\S]*?)""");

                    MatchCollection times = Regex.Matches(html, @"<span rel='tooltip' data-toggle='tooltip' data-placement='bottom' title='([\s\S]*?)'");
                    MatchCollection values = Regex.Matches(html, @"</td><td>([\s\S]*?)<");
                    MatchCollection tokenSymbols = Regex.Matches(html, @"class='mr-1'>([\s\S]*?)<");
                    MatchCollection txids = Regex.Matches(html, @"<span class='hash-tag text-truncate myFnExpandBox_searchVal'><a href='/tx/([\s\S]*?)title='([\s\S]*?)'");
                    MatchCollection from = Regex.Matches(html, @"data-boundary='viewport' data-html='true' data-toggle='tooltip' data-placement='bottom' title='([\s\S]*?)'");
                    //MatchCollection to = Regex.Matches(html, @"target='_parent' data-boundary='viewport' data-html='true' data-toggle='tooltip' data-placement='bottom' title='([\s\S]*?)'");

                    List<string> timelist = new List<string>();
                    List<string> fromlist = new List<string>();
                    List<string> tolist = new List<string>();
                    List<string> valuelist = new List<string>();
                  
                    for (int j = 0; j < from.Count; j++)
                    {
                        if(j%2==0)
                        {
                            fromlist.Add(from[j].Groups[1].Value);
                        }
                        else
                        {
                         
                            tolist.Add(from[j].Groups[1].Value);
                           
                            timelist.Add(times[j].Groups[1].Value);
                        }

                       
                    }
                    for (int z = 0; z < values.Count; z++)
                    {
                        if (values[z].Groups[1].Value != "")
                        {
                            valuelist.Add(values[z].Groups[1].Value);
                        }
                    }
                   
                    int index = 0;
                    //for (int a = 0; a < from.Count; a++)
                    //{
                    //    if (from[a].Groups[1].Value != listView1.Items[i].SubItems[1].Text && to[a].Groups[1].Value == listView1.Items[i].SubItems[1].Text)
                    //    {
                    //        index = a;
                    //        break;

                    //    }

                    //}

                    for (int a = 0; a <fromlist.Count; a++)
                    {
                      
                        if (fromlist[a] != listView1.Items[i].SubItems[1].Text.ToLower() && tolist[a]== listView1.Items[i].SubItems[1].Text.ToLower())
                        {
                         
                            index = a;
                            break;

                        }

                    }
                
                    if (listView1.Items[i].SubItems[2].Text == "")
                    {
                       
                        listView1.Items[i].SubItems[2].Text = timelist[index];
                        listView1.Items[i].SubItems[3].Text = "USDT";
                        listView1.Items[i].SubItems[4].Text = valuelist[index];
                        listView1.Items[i].SubItems[6].Text = txids[index].Groups[1].Value;
                        listView1.Items[i].SubItems[7].Text = fromlist[index];
                    }
                    else
                    {
                        if (listView1.Items[i].SubItems[6].Text != txids[index].Groups[1].Value)
                        {


                            listView1.Items[i].SubItems[2].Text = timelist[index];
                            listView1.Items[i].SubItems[3].Text = tokenSymbols[index].Groups[1].Value;
                            listView1.Items[i].SubItems[4].Text =valuelist[index];
                            listView1.Items[i].SubItems[6].Text = txids[index].Groups[1].Value;
                            listView1.Items[i].SubItems[7].Text = fromlist[index];



                            Beep(800, 1000);

                            string msg = listView1.Items[i].SubItems[5].Text + "  " + listView1.Items[i].SubItems[4].Text + listView1.Items[i].SubItems[3].Text;
                            MessageBox.Show(msg);


                        }
                    }
                }
                catch (Exception ex)
                {
                     MessageBox.Show(ex.ToString());
                    continue;
                }
            }

        }





        private void ERC监控_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListViewItem lv1 = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
            lv1.SubItems.Add(textBox1.Text.Trim());
            lv1.SubItems.Add("");
            lv1.SubItems.Add("");
            lv1.SubItems.Add("");
            lv1.SubItems.Add(textBox2.Text.Trim());
            lv1.SubItems.Add("");
            lv1.SubItems.Add("");
        }


        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html");

            if (!html.Contains(@"6WxcM"))
            {

                return;
            }



            #endregion

            timer1.Start();
            timer1.Interval = Convert.ToInt32(textBox3.Text) * 1000;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            timer1.Start();
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, EncodingType.GetTxtType(openFileDialog1.FileName));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i].Trim() == "")
                        continue;

                    string[] value = text[i].Split(new string[] { "----" }, StringSplitOptions.None);
                    ListViewItem lv1 = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                    lv1.SubItems.Add(value[0].Trim());
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add(value[1].Trim());
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
        }




        private void button3_Click(object sender, EventArgs e)
        {


            timer1.Stop();
        }

        private void 复制付款地址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                System.Windows.Forms.Clipboard.SetText(listView1.SelectedItems[0].SubItems[7].Text); //复制

            }
        }

        private void 复制收款地址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                System.Windows.Forms.Clipboard.SetText(listView1.SelectedItems[0].SubItems[1].Text); //复制

            }
        }

        private void 删除此行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                listView1.Items.Remove(listView1.SelectedItems[0]);
            }
        }

        private void 导出文本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListviewToTxt(listView1);
        }

        #region  listview导出文本TXT
        public static void ListviewToTxt(ListView listview)
        {

            //if (listview.Items.Count == 0)
            //{
            //    MessageBox.Show("列表为空!");
            //}

            List<string> list = new List<string>();
            foreach (ListViewItem item in listview.Items)
            {

                list.Add(item.SubItems[1].Text + " " + item.SubItems[2].Text + " " + item.SubItems[3].Text + " " + item.SubItems[4].Text + " " + item.SubItems[5].Text + " " + item.SubItems[6].Text + " " + item.SubItems[7].Text);


            }
            SaveFileDialog sfd = new SaveFileDialog();

            // string path = AppDomain.CurrentDomain.BaseDirectory + "导出_" + Guid.NewGuid().ToString() + ".txt";
            string path = "";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                path = sfd.FileName + ".txt";
            }
            StringBuilder sb = new StringBuilder();
            foreach (string tel in list)
            {
                sb.AppendLine(tel);
            }
            System.IO.File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("文件导出成功!文件地址:" + path);


        }






        #endregion

        private void 复制交易idToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                System.Windows.Forms.Clipboard.SetText(listView1.SelectedItems[0].SubItems[6].Text); //复制

            }
        }
    }
}
