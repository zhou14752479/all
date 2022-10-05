using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 宿网办公助手
{
    public partial class 宿城地址查询街道 : Form
    {
        public 宿城地址查询街道()
        {
            InitializeComponent();
        }

        string reviewcookie = "_m_h5_tk=92db0fa413f671f2890f3f01433abdb1_1650879394197;_m_h5_tk_enc=d85cbacbe17135f56c54809f7f00c70f; ";
        string token = "";

        public static string Md5_utf8(string str)
        {
            //将输入字符串转换成字节数组
            var buffer = Encoding.GetEncoding("utf-8").GetBytes(str);
            //接着，创建Md5对象进行散列计算
            var data = MD5.Create().ComputeHash(buffer);

            //创建一个新的Stringbuilder收集字节
            var sb = new StringBuilder();

            //遍历每个字节的散列数据 
            foreach (var t in data)
            {
                //格式每一个十六进制字符串
                sb.Append(t.ToString("X2"));
            }

            //返回十六进制字符串
            return sb.ToString().ToLower();
        }

       
        /// <summary>
        /// 获取时间戳毫秒
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalMilliseconds);
            return a.ToString();
        }

        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlWithCookie(string Url, string COOKIE, string charset)
        {
            string html = "";

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                // request.Proxy = null;//防止代理抓包
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                //request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;

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

        #region GET请求获取Set-cookie
        public static string getSetCookie(string url)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);  //创建一个链接
            request.Timeout = 10000;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
            request.AllowAutoRedirect = false;

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

            string content = response.GetResponseHeader("Set-Cookie"); ;
            return content;


        }
        #endregion
        public void getarea()
        {
            try
            {
                textBox2.Text = "";
                string addr = textBox1.Text.Trim();
                token = Regex.Match(reviewcookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
                string time = GetTimeStamp();
                string str = token + "&" + time + "&12574478&{\"sn\":\"suibianchuan\",\"keywords\":\"" + addr + "\",\"provId\":\"320000\",\"cityId\":\"321300\",\"districtId\":\"321302\"}";
                string sign = Md5_utf8(str);

                string aurl = "https://h5api.m.taobao.com/h5/mtop.cainiao.address.ua.china.address.suggest/1.0/?jsv=2.5.1&appKey=12574478&t=" + time + "&sign=" + sign + "&api=mtop.cainiao.address.ua.china.address.suggest&v=1.0&dataType=jsonp&type=jsonp&callback=mtopjsonp26&data=%7B%22sn%22%3A%22suibianchuan%22%2C%22keywords%22%3A%22" + addr + "%22%2C%22provId%22%3A%22320000%22%2C%22cityId%22%3A%22321300%22%2C%22districtId%22%3A%22321302%22%7D";

                string html = GetUrlWithCookie(aurl, reviewcookie, "utf-8");


                if (html.Contains("令牌过期"))
                {
                    string cookiestr = getSetCookie(aurl);
                    string _m_h5_tk = "_m_h5_tk=" + Regex.Match(cookiestr, @"_m_h5_tk=([\s\S]*?);").Groups[1].Value;
                    string _m_h5_tk_enc = "_m_h5_tk_enc=" + Regex.Match(cookiestr, @"_m_h5_tk_enc=([\s\S]*?);").Groups[1].Value;
                    reviewcookie = _m_h5_tk + ";" + _m_h5_tk_enc + ";";

                    token = Regex.Match(reviewcookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
                    time = GetTimeStamp();
                    str = str = token + "&" + time + "&12574478&{\"sn\":\"suibianchuan\",\"keywords\":\"" + addr + "\",\"provId\":\"320000\",\"cityId\":\"321300\",\"districtId\":\"321302\"}";

                    sign = Md5_utf8(str);

                    aurl = "https://h5api.m.taobao.com/h5/mtop.cainiao.address.ua.china.address.suggest/1.0/?jsv=2.5.1&appKey=12574478&t=" + time + "&sign=" + sign + "&api=mtop.cainiao.address.ua.china.address.suggest&v=1.0&dataType=jsonp&type=jsonp&callback=mtopjsonp26&data=%7B%22sn%22%3A%22suibianchuan%22%2C%22keywords%22%3A%22" + addr + "%22%2C%22provId%22%3A%22320000%22%2C%22cityId%22%3A%22321300%22%2C%22districtId%22%3A%22321302%22%7D";

                    html = GetUrlWithCookie(aurl, reviewcookie, "utf-8");
                    //MessageBox.Show(html);
                }

                MatchCollection name = Regex.Matches(html, @"""name"":""([\s\S]*?)""");
                MatchCollection town = Regex.Matches(html, @"""town"":""([\s\S]*?)""");
                for (int i = 0; i < name.Count; i++)
                {
                    textBox2.Text += name[i].Groups[1].Value + "-----" + town[i].Groups[1].Value + "\r\n";
                }
            }
            catch (Exception)
            {

                textBox1.Text="阿哦好像出错了...";
            }

        }





            private void 宿城地址查询街道_Load(object sender, EventArgs e)
        {

        }

        Thread thread;

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="")
            {
                textBox1.Text = "输入为空...";
                return;
            }
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getarea);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 宿城地址查询街道_FormClosing(object sender, FormClosingEventArgs e)
        {
          
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
               button1.Focus();
                button1_Click(this, new EventArgs());
                return;
            }
        }
    }
}
