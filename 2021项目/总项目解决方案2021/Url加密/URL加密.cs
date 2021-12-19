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

using System.Windows.Forms;

namespace Url加密
{
    public partial class URL加密 : Form
    {

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.m";
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
        public URL加密()
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
               // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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
        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string pipeiyuming = Regex.Match(textBox1.Text, @"www\.([\s\S]*?)\.").Groups[1].Value;
            string yuming2 = Regex.Match(yuming, @"www\.([\s\S]*?)\.").Groups[1].Value;
            if (pipeiyuming==yuming2)
            {
                textBox2.Text = UrlEncode(textBox1.Text.Trim());
            }
            else
            {
                MessageBox.Show("域名不匹配");
            }

            
        }

        public static string yuming = "";
        public static string expiretime = "";
        private void URL加密_Load(object sender, EventArgs e)
        {
            if (ExistINIFile())
            {
                string key = IniReadValue("values", "key");
                string url = "http://47.99.104.164/do.php?key="+key+"&method=login";
                string html = GetUrl(url,"utf-8");
                if(html.Contains("true"))
                {
                    yuming = Regex.Match(html, @"""url"":""([\s\S]*?)""").Groups[1].Value;
                    expiretime = Regex.Match(html, @"""expiretime"":""([\s\S]*?)""").Groups[1].Value;
                    if(yuming=="")
                    {
                        MessageBox.Show("key错误，请联系QQ71751777");
                        System.Diagnostics.Process.GetCurrentProcess().Kill();

                    }

                    this.Text = "防劫持链接加密 QQ71751777          绑定域名：" + yuming+"  过期时间："+expiretime;
                    label1.Text = "绑定域名：" + yuming + " 过期时间：" + expiretime;


                }

                else
                {
                    MessageBox.Show("key错误，请联系QQ71751777");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }

            }
            else
            {
                MessageBox.Show("key文件不存在，请联系QQ71751777");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(textBox2.Text.Trim()); //复制
        }

      
    }
}
