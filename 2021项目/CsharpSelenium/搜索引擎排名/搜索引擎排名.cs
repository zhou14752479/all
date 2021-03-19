using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 搜索引擎排名
{
    public partial class 搜索引擎排名 : Form
    {
        public 搜索引擎排名()
        {
            InitializeComponent();
        }
        Thread thread;
        ChromeOptions options = new ChromeOptions();
        #region  获取32位MD5加密
        public string GetMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        #endregion

        #region HmacMD5
        string HmacMD5(string source, string key)
        {
            HMACMD5 hmacmd = new HMACMD5(Encoding.Default.GetBytes(key));
            byte[] inArray = hmacmd.ComputeHash(Encoding.Default.GetBytes(source));
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < inArray.Length; i++)
            {
                sb.Append(inArray[i].ToString("X2"));
            }

            hmacmd.Clear();

            return sb.ToString().ToLower();
        }

        #endregion


        private Object hash_hmac(string signatureString, string secretKey, bool raw_output = false)
        {
            var enc = Encoding.UTF8;
            HMACSHA1 hmac = new HMACSHA1(enc.GetBytes(secretKey));
            hmac.Initialize();

            byte[] buffer = enc.GetBytes(signatureString);
            if (raw_output)
            {
                return hmac.ComputeHash(buffer);
            }
            else
            {
                return BitConverter.ToString(hmac.ComputeHash(buffer)).Replace(" - ", "").ToLower();
            }
        }



        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                // request.ContentType = "application/x-www-form-urlencoded";

                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("appid:orders");
                //headers.Add("x-nike-visitid:5");
                //headers.Add("x-nike-visitorid:d03393ee-e42c-463e-9235-3ca0491475b4");
                //添加头部
                request.ContentType = "application/json";
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.Headers.Add("Cookie", COOKIE);

                request.Referer = "https://web.duanmatong.cn/";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion

        /// <summary>
        /// 获取时间戳  秒
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalSeconds);
            return a.ToString();
        }
        public void getkeys()
        {
            string timestamp = GetTimeStamp();
            string token = GetMD5(timestamp);
            textBox1.Text = "limit=10timestamp=" + timestamp + "token=" + token;
            string sign = hash_hmac("limit=10timestamp=" + timestamp + "token=" + token, "Z7InH1Zd").ToString();
          
            string url = "http://vip.17s.cn/api/rank/get.html";
            string postdata = "limit=10&timestamp="+timestamp+"&token="+token+"&sign="+sign;
            MessageBox.Show(postdata);
            string html = PostUrl(url,postdata);
            MessageBox.Show(html);
           }

        public void run()
        {
            
            string type = "1";
            string keyword = "%E6%80%9D%E5%BF%86%E8%BD%AF%E4%BB%B6";
            string yuming = "www.qqtn.com";
            //options.AddArgument("--headless");//设置无界面
            IWebDriver driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            switch (type)
            {
                case "1":
                    baidupc(driver,keyword,yuming);
                    break;

            }
           
        }

        public void baidupc(IWebDriver driver,string keyword,string yuming)
        {
           string  url = "https://www.baidu.com/s?wd=" + keyword + "&pn=0&rn=50";
            driver.Navigate().GoToUrl(url);
            MatchCollection urls = Regex.Matches(driver.PageSource, @"position:relative;"">([\s\S]*?)/");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < urls.Count; i++)
            {
                if (urls[i].Groups[1].Value == yuming)
                {
                    sb.Append((i+1) + ",");
                }
            }

            
        }

        public void san60pc(IWebDriver driver, string keyword, string yuming)
        {
            string url = "https://www.so.com/s?ie=utf-8&fr=none&src=360sou_newhome&q="+keyword;
            driver.Navigate().GoToUrl(url);
            MatchCollection urls = Regex.Matches(driver.PageSource, @"<p class=""g-linkinfo""><cite>([\s\S]*?)</cite>");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < urls.Count; i++)
            {
                if (urls[i].Groups[1].Value == yuming)
                {
                    sb.Append((i + 1) + ",");
                }
            }

           
        }

        public void sougoupc(IWebDriver driver, string keyword, string yuming)
        {
            string url = "https://www.so.com/s?ie=utf-8&fr=none&src=360sou_newhome&q=" + keyword;
            driver.Navigate().GoToUrl(url);
            MatchCollection urls = Regex.Matches(driver.PageSource, @"<p class=""g-linkinfo""><cite>([\s\S]*?)</cite>");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < urls.Count; i++)
            {
                if (urls[i].Groups[1].Value == yuming)
                {
                    sb.Append((i + 1) + ",");
                }
            }


        }

        private void 搜索引擎排名_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
              
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
