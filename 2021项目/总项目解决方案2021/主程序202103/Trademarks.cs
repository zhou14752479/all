using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using myDLL;

namespace 主程序202103
{
    public partial class Trademarks : Form
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


        public Trademarks()
        {
            InitializeComponent();
        }
        Thread thread;
        bool zanting = true;
        bool status = true;


        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlWithCookie(string Url, string COOKIE, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                request.Accept = "*/*";
                request.Timeout = 100000;
                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                return (ex.ToString());



            }

        }
        #endregion
        public void run()
        {


            for (int page = 1; page < 99999; page=page+500)
            {
                label1.Text = "正在抓取第："+page+"条数据";
                string url = "http://tess2.uspto.gov/bin/showfield?f=toc&state="+state+page;
               
                string html = GetUrlWithCookie(url, cookie, "utf-8");
               
                string ahtml = Regex.Match(html, @"<TABLE BORDER=2>([\s\S]*?)</TABLE>").Groups[1].Value;

                MatchCollection tds = Regex.Matches(ahtml, @"<TD>([\s\S]*?)</TD>");
                if (tds.Count == 0)
                    return;

                for (int i = 0; i < tds.Count / 6; i++)
                {
                    try
                    {
                        ListViewItem lv1 = listView1.Items.Add(Regex.Replace(tds[(6 * i)].Groups[1].Value, "<[^>]+>", "")); //使用Listview展示数据

                        lv1.SubItems.Add(Regex.Replace(tds[(6 * i) + 1].Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(tds[(6 * i) + 2].Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(tds[(6 * i) + 3].Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(tds[(6 * i) + 4].Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(tds[(6 * i) + 5].Groups[1].Value, "<[^>]+>", ""));
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }


                }
            }


        }

        string cookie = "";
        string state = "";
        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void button1_Click(object sender, EventArgs e)
        {
            // 读取config.ini
            if (ExistINIFile())
            {
                try
                {
                    //cookie = IniReadValue("values", "cookie");
                    //state = IniReadValue("values", "state");

                    StreamReader sr = new StreamReader(path + "cookie.txt", method.EncodingType.GetTxtType(path + "cookie.txt"));
                    //一次性读取完 
                    string texts = sr.ReadToEnd();
                    state  = Regex.Match(texts, @"state=([\s\S]*?)&").Groups[1].Value;
                    cookie = Regex.Match(texts, @"cookie=([\s\S]*?)&").Groups[1].Value;
                    sr.Close();  //只关闭流
                    sr.Dispose();   //销毁流内存
                    state = state.Remove(state.Length - 1, 1);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString()) ;
                }
            }

            
        
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
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

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void Trademarks_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            Process.Start(path + "helper.exe");
        }

        private void Trademarks_Load(object sender, EventArgs e)
        {

        }
    }
}
