
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using System.Collections;
using System.IO.Compression;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace 主程序202102
{
    public partial class 中通快递查询 : Form
    {
        public 中通快递查询()
        {
            InitializeComponent();
        }

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

        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlWithCookie(string Url, string COOKIE, string charset,string ticket)
        {
            string html = "";

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36";

                request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("X-Bill-Token:" + ticket);
                headers.Add("X-Bill-Type: A014");
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
       
        Thread t;
        Thread thread;
       
        bool zanting = true;
        bool status = true;
        string cookie = "";
        public string gettoken(string billid)
        {
           
            string html = GetUrlWithCookie("https://sso.zto.com/security-services/billtrack/billinfo-query-preauth?bill_id="+billid+"&type=A014", cookie,"utf-8","");
            string ticket = Regex.Match(html, @"""ticket"":""([\s\S]*?)""").Groups[1].Value;
           
            return ticket;

        }
       

        public string getinfos(string billid)
        {
            string ticket =  gettoken(billid);
          
            string html = GetUrlWithCookie("https://newbill.zt-express.com/order-query/get?billCode="+billid, cookie, "utf-8",ticket);
           
            return html;

           

        }

        public string getguiji(string billid)
        {
           
            string url = "https://newbill.zt-express.com/bill-tracing/list-rendered";
            string postdata = "{\"billCodes\":[\""+billid+"\"],\"useArchive\":false,\"hideForm\":false,\"hideCarInfo\":false,\"showData\":false,\"showDataColumn\":[],\"controlSet\":[]}";
            string html = method.PostUrl(url,postdata, cookie, "utf-8", "application/json",url);
          
            return html;

        }

        #region 订单信息
        public void dingdan()
        {
           

            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
           
            foreach (string item in text)
            {

                string html = getinfos(item);
                string ahtml = getguiji(item);
               

                Match sendInfo = Regex.Match(html, @"""sendInfo"":""([\s\S]*?)""");
                Match sendAddress = Regex.Match(html, @"""sendAddress"":""([\s\S]*?)""");
                Match receiveInfo = Regex.Match(html, @"""receiveInfo"":""([\s\S]*?)""");
                Match receiveAddress = Regex.Match(html, @"""receiveAddress"":""([\s\S]*?)""");
                Match a1 = Regex.Match(html, @"""firstCode"":""([\s\S]*?)""");
                Match a2 = Regex.Match(html, @"""secondCode"":""([\s\S]*?)""");
                Match a3 = Regex.Match(html, @"""thirdCode"":""([\s\S]*?)""");
                MatchCollection guijis = Regex.Matches(Regex.Replace(ahtml, "动态轨迹.*", ""), @"data-scantype=([\s\S]*?)<td align=\\""left\\"">([\s\S]*?)</td>");

                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    try
                    {
                    string date = Regex.Match(guijis[guijis.Count - 1].Groups[1].Value, @"202([\s\S]*?)</td>").Groups[1].Value;

                    lv1.SubItems.Add(item);
                    lv1.SubItems.Add(Regex.Replace(sendInfo.Groups[1].Value, "<[^>]+>", ""));
                    lv1.SubItems.Add(Regex.Replace(sendAddress.Groups[1].Value, "<[^>]+>", ""));
                    lv1.SubItems.Add(Regex.Replace(receiveInfo.Groups[1].Value, "<[^>]+>", ""));
                    lv1.SubItems.Add(Regex.Replace(receiveAddress.Groups[1].Value, "<[^>]+>", ""));
                    lv1.SubItems.Add(Regex.Replace(a1.Groups[1].Value, "<[^>]+>", ""));
                    lv1.SubItems.Add(Regex.Replace(a2.Groups[1].Value, "<[^>]+>", ""));
                    lv1.SubItems.Add(Regex.Replace(a3.Groups[1].Value, "<[^>]+>", ""));
                    lv1.SubItems.Add(Regex.Replace(guijis[guijis.Count-1].Groups[2].Value, "<[^>]+>", ""));
                    lv1.SubItems.Add("202"+date);
                    string sx = getsx(item);
                    lv1.SubItems.Add(sx);
                }
                    catch (Exception)
                    {

                        lv1.SubItems.Add("");

                    }

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;

            }

        }

        #endregion

        #region 包内件
        public void baoneijian()
        {


            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (string item in text)
            {
                if (item.Trim() == "")
                {

                    continue; }
               
                string url = "https://newbill.zt-express.com/bill-tracing/list-rendered";
                string postdata = "{\"billCodes\":[\"" +item+ "\"],\"useArchive\":false,\"hideForm\":false,\"hideCarInfo\":false,\"showData\":false,\"showDataColumn\":[],\"controlSet\":[]}";

                string html = method.PostUrl(url, postdata, cookie, "utf-8", "application/json", "");

                string baohao = Regex.Match(html, @"z_num\\"">【([\s\S]*?)】").Groups[1].Value;
                string zongdan = Regex.Match(html, @"\(共([\s\S]*?)单").Groups[1].Value;
                string zongzhong = Regex.Match(html, @"合计袋重([\s\S]*?)\)").Groups[1].Value;
                MatchCollection danhaos = Regex.Matches(html, @"btn_drill\\"">([\s\S]*?)</td><td>([\s\S]*?)</td>");



                for (int i = 0; i < danhaos.Count; i++)
                {
                    ListViewItem lv2 = listView2.Items.Add(listView2.Items.Count.ToString()); //使用Listview展示数据
                    try
                    {

                        lv2.SubItems.Add(baohao);
                        lv2.SubItems.Add(zongdan);
                        lv2.SubItems.Add(zongzhong);
                        lv2.SubItems.Add(danhaos[i].Groups[1].Value);
                        lv2.SubItems.Add(danhaos[i].Groups[2].Value);
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                    }
                    catch (Exception)
                    {

                        lv2.SubItems.Add("");

                    }
                }
            }



           
          




        }

        #endregion

        public string getsx(string code)
        {
            string url = "https://newbill.zt-express.com/lazy/prescription?type=vip&url=%2Flazy%2Fprescription&billCode="+code;
          string html=  GetUrlWithCookie(url, cookie, "utf-8", "");
            if(html.Contains("SX"))
            {
                return "SX";
            }
            else
            {
                return "";
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"RE72"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion
            string path = AppDomain.CurrentDomain.BaseDirectory;

            try
            {
                StreamReader sr = new StreamReader(path + "cookie.txt", method.EncodingType.GetTxtType(path + "cookie.txt"));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                cookie = Regex.Match(texts, @"cookie=([\s\S]*?)&").Groups[1].Value;
                textBox2.Text = cookie;
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
           
            status = true;
            if (radioButton1.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(dingdan);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
            if (radioButton2.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(baoneijian);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
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

        private void button6_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            listView1.Items.Clear();
            listView2.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
            }

            if (radioButton2.Checked == true)
            {
                method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
            }

        }

        private void 中通快递查询_FormClosing(object sender, FormClosingEventArgs e)
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

        private void 中通快递查询_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            Process.Start(path + "helper.exe");
        }
    }
}
