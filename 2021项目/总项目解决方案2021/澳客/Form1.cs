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
using System.Windows.Forms;

namespace 澳客
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }

        #region GET使用代理IP请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url,string charset)
        {
            string html = "";

            try
            {
               // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36";
               
                request.Referer = Url;

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


        #region GET使用代理IP请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlwithIP(string Url, string ip, string COOKIE, string charset)
        {
            string html = "";

            try
            {
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36";
                WebProxy proxy = new WebProxy(ip);
                request.Proxy = proxy;
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

        Thread thread;
        bool zanting = true;
        bool status = true;
        string cookie = "LastUrl=; FirstURL=www.okooo.com/livecenter/jingcai/%3Fdate%3D2018-01-01; FirstOKURL=http%3A//www.okooo.com/soccer/match/1002341/history/; First_Source=www.okooo.com; _ga=GA1.2.15240016.1615507805; OkAutoUuid=43649ef140f2f5abf22eaa12cb9514bd; OkMsIndex=1; M_UserName=%22ok_242228786787%22; M_UserID=30498306; M_Ukey=067e94b82ba40266c3a93fde0c9d9c01; OkTouchAutoUuid=43649ef140f2f5abf22eaa12cb9514bd; OkTouchMsIndex=1; _gid=GA1.2.157513035.1615768129; _gat=1; PHPSESSID=j3hrdp9i7fvgcm2a07nb4q72d5; IMUserID=30498306; IMUserName=%E6%9E%97%E5%AD%94988610; OKSID=j3hrdp9i7fvgcm2a07nb4q72d5; DRUPAL_LOGGED_IN=Y; isInvitePurview=0; acw_tc=2f624a2716157681302546806e0b46fe167d958b55fe25d49f21441c8c4cc4; __utma=56961525.15240016.1615507805.1615600887.1615768130.4; __utmc=56961525; __utmz=56961525.1615768130.4.2.utmcsr=account.okooo.com|utmccn=(referral)|utmcmd=referral|utmcct=/; Hm_lvt_5ffc07c2ca2eda4cc1c4d8e50804c94b=1615507806,1615596120,1615768158; Hm_lpvt_5ffc07c2ca2eda4cc1c4d8e50804c94b=1615768167; __utmb=56961525.30.2.1615768168397";
        public string getip()
        {
            // string html = method.GetUrl("http://47.106.170.4:8081/Index-generate_api_url.html?packid=7&fa=5&groupid=0&fetch_key=&qty=1&port=1&format=txt&ss=1&css=&pro=&city=&usertype=7", "utf-8");
            string html = GetUrl("http://47.106.170.4:8081/Index-generate_api_url.html?packid=1&fa=0&groupid=0&fetch_key=&qty=1&port=1&format=txt&ss=1&css=&pro=&city=&usertype=7", "utf-8");
            label1.Text = html;
            return html;
        }


        #region 主程序
        public void run()
        {


            for (DateTime dt = dateTimePicker1.Value; dt < DateTime.Now; dt = dt.AddDays(1))
            {
                string ip = getip();

                string url = "http://www.okooo.com/livecenter/jingcai/?date=" + dt.ToString("yyyy-MM-dd");

                string html = GetUrlwithIP(url, ip, cookie, "utf-8");
                if (html.Contains("安全威胁") || html.Trim()=="")
                {
                    Thread.Sleep(6000);
                    ip = getip();
                    dt = dt.AddDays(-1);
                    continue;
                }

                MatchCollection ids = Regex.Matches(html, @"match_detail_([\s\S]*?)""");
                MatchCollection xuhaos = Regex.Matches(html, @"</label><span>([\s\S]*?)</span>");

                for (int j = 0; j < ids.Count; j++)
                {
                    textBox3.Text = "正在查询：" + dt.ToString("yyyy-MM-dd") + "   " + xuhaos[j].Groups[1].Value;
                    string aurl = "http://www.okooo.com/soccer/match/" + ids[j].Groups[1].Value + "/goals/change/2/";
                    string ahtml = GetUrlwithIP(aurl, ip, cookie, "gb2312");
                   // MessageBox.Show(ahtml);
                    if (ahtml.Contains("安全威胁"))
                    {
                        Thread.Sleep(6000);
                        ip = getip();
                        ahtml = GetUrlwithIP(aurl, ip, cookie, "gb2312");
                    }
                    try
                    {
                        MatchCollection divs = Regex.Matches(ahtml, @"<div class=""N-datatxt"">([\s\S]*?)</div>");
                        for (int i = 0; i < divs.Count / 8; i++)
                        {
                            try
                            {
                                string qiu5 = Regex.Replace(divs[(i * 8) + 5].Groups[1].Value, "<[^>]+>", "").Replace("→", "").Replace("&darr;", "").Replace("&uarr;", "").Trim();
                                string qiu6 = Regex.Replace(divs[(i * 8) + 6].Groups[1].Value, "<[^>]+>", "").Replace("→", "").Replace("&darr;", "").Replace("&uarr;", "").Trim();
                                string qiu7 = Regex.Replace(divs[(i * 8) + 7].Groups[1].Value, "<[^>]+>", "").Replace("→", "").Replace("&darr;", "").Replace("&uarr;", "").Trim();

                                double ina = Convert.ToDouble(textBox1.Text);
                                double inb = Convert.ToDouble(textBox2.Text);
                              
                                if (Convert.ToDouble(qiu5) >= ina && Convert.ToDouble(qiu5) <= inb || Convert.ToDouble(qiu6) >= ina && Convert.ToDouble(qiu6) <= inb || Convert.ToDouble(qiu7) >= ina && Convert.ToDouble(qiu7) <= inb)
                                {
                                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                    lv1.SubItems.Add(dt.ToString("yyyyMMdd") + xuhaos[j].Groups[1].Value);
                                    break;
                                }


                            }
                            catch (Exception ex)
                            {

                                //MessageBox.Show(ex.ToString());
                                continue;
                            }


                        }




                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;

                    }
                    catch (Exception ex)
                    {

                      //  MessageBox.Show(ex.ToString());
                        continue;
                    }


                }
                Thread.Sleep(1000);
            }

        }

        #endregion
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {  // 读取config.ini
           //if (ExistINIFile())
           //{
           //    cookie = IniReadValue("values", "cookie");

            //}

           
            cookie = textBox4.Text.Trim();

            
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
        
            status = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否退出？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }
        #region  listView导出CSV
        /// <summary>
        /// 导出CSV
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="includeHidden"></param>
        /// 
        public static void ListViewToCSV(ListView listView, bool includeHidden)
        {
            //make header string
            SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "xlsx|*.xls|xlsx|*.xlsx";

            //sfd.Title = "Excel文件导出";
            string filePath = "";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filePath = sfd.FileName + ".csv";
            }
            StringBuilder result = new StringBuilder();
            WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listView.Columns[i].Text);

            //export data rows
            foreach (ListViewItem listItem in listView.Items)
                WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listItem.SubItems[i].Text);

            File.WriteAllText(filePath, result.ToString(), Encoding.Default);
            MessageBox.Show("导出成功");
        }


        private static void WriteCSVRow(StringBuilder result, int itemsCount, Func<int, bool> isColumnNeeded, Func<int, string> columnValue)
        {
            bool isFirstTime = true;
            for (int i = 0; i < itemsCount; i++)
            {
                try
                {

                    if (!isColumnNeeded(i))
                        continue;

                    if (!isFirstTime)
                        result.Append(",");
                    isFirstTime = false;

                    result.Append(String.Format("\"{0}\"", columnValue(i)));
                }
                catch
                {
                    continue;
                }
            }

            result.AppendLine();
        }

        #endregion
        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //ListViewToCSV(listView1,true);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                sb.Append(listView1.Items[i].SubItems[1].Text + "\r\n");
            }

            try
            {
                Clipboard.SetDataObject(sb.ToString());

                MessageBox.Show("复制成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

       
    }
}
