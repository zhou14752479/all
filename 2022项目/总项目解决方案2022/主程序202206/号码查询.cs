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

namespace 主程序202206
{
    public partial class 号码查询 : Form
    {
        public 号码查询()
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
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

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

        #region unicode转中文
        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        #endregion;

        Dictionary<string, string> dics = new Dictionary<string, string>(); 
        public void chaxun()
        {

            try
            {

                Random random = new Random();   
                string url = "https://www.ip138.com/mobile.asp?mobile="+textBox1.Text.Trim()+"&action=mobile";
                string html = GetUrl(url,"utf-8");
                string addr = Regex.Match(html, @"卡号归属地</td>([\s\S]*?)</span>").Groups[1].Value;
               addr= Regex.Replace(addr, "<[^>]+>", "").Replace("&nbsp;","").Trim();

                string aurl = "https://map.baidu.com/?newmap=1&reqflag=pcmap&biz=1&from=webmap&da_par=direct&pcevaname=pc4.1&qt=s&da_src=searchBox.button&wd="+ System.Web.HttpUtility.UrlEncode(addr+"饭店") + "&c=277&src=0&wd2=&pn=0&sug=0&l=11&b=(13034941,3951320.8599999994;13362621,4037976.8599999994)&from=webmap&biz_forward={%22scaler%22:1,%22styles%22:%22pl%22}&sug_forward=&auth=7LJFLASzvaBREPCYRTLzWK9HBSDEz0v8uxLHHEHEHNLtBHWKOKEDHByHxzBU%40yYxvkGcuVtvvhguVtvyheuVtvCMGuVtvCQMuVtvIPcuxtw8wkv7uvYgPk1dK84yDFFCFfqXzajPyYxv%40vcuVtvYgPIiu9z%40jSJjv%40DenSC%40BzczgyYxvZgMuVtv%40vcuVtvc3CuVtcvY1SGpuxNt3s%3D%3DJ0IveGvuxTtLwAYKO3xjhNwBWWvvjkGcuVtvrMhuxBt5ooioGIFf0wd0vyMIIFIFIOy&seckey=TeTpTkK8YvalTLLiE0SVUx0Of4%2Fb8B%2FHAKAhys0oaqw%3D%2CTeTpTkK8YvalTLLiE0SVUyyWM9U-EFs-uuSO0SBozFoij9sk_fmf3zL11fAXBYWu9DvBd41AMEcczFCyRBBG-0-_oIJgBi4ujMhbwXSpOj-s8eNFiFp9wWG3V2AO9Cm-PamiZwAWk8yvBvytQ79quKCGm_80WcBY1ta6rTaB_VrsW8ppYmKuO4mpmca1vj-D&device_ratio=1&tn=B_NORMAL_MAP&nn=0&u_loc=13163913,3997918&ie=utf-8&t=1655454578501&newfrom=zhuzhan_webmap";
                string ahtml = GetUrl(aurl, "utf-8");
                MatchCollection address = Regex.Matches(ahtml, @"""addr"":""([\s\S]*?)""");
                addr = address[random.Next(address.Count)].Groups[1].Value;
                addr = Unicode2String(addr);
                if (dics.ContainsKey(textBox1.Text.Trim()))
                {
                    addr = dics[textBox1.Text.Trim()];
                }
                label2.Text = addr;
                if(!dics.ContainsKey(textBox1.Text.Trim()))
                {
                    dics.Add(textBox1.Text.Trim(),addr);
                }
            }
            catch (Exception)
            {

               
            }
        }

        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(chaxun);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 号码查询_Load(object sender, EventArgs e)
        {
            #region 通用检测


            if (!GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"GfOh"))
            {
                
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion
        }
    }
}
