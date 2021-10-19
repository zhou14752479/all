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
using myDLL;

namespace 推特项目
{
    public partial class 推特ID转换 : Form
    {
        public 推特ID转换()
        {
            InitializeComponent();
        }



        protected void getMentionV3()
        {
            string resource_url = "https://api.twitter.com/1.1/friendships/lookup.json?";
            string user_id = "01082389502";

     var oauth_token = "1299586017112932352-jJP5I6tvPywqeeflsXkmooEc4Kp0fz";
            var oauth_token_secret = "Az1OajR40krXsLg4tvoRWUvxya4Qn77jJifvUnKLOBxAm";
            var oauth_consumer_key = "xevP9aVHj4obnCQLjujWUGrvR";
            var oauth_consumer_secret = "FRaS8ImHhleJIjS8aFpS2JL2uewXLGzIhOHGZeJQq0ebXWm5Bq";

            var oauth_version = "1.0";
            var oauth_signature_method = "HMAC-SHA1";

            var oauth_nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            var timeSpan = DateTime.UtcNow
             - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&q={6}";

            var baseString = string.Format(baseFormat,
                   oauth_consumer_key,
                   oauth_nonce,
                   oauth_signature_method,
                   oauth_timestamp,
                   oauth_token,
                   oauth_version,
                   Uri.EscapeDataString(user_id)
                   );

            baseString = string.Concat("GET&", Uri.EscapeDataString(resource_url), "&", Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                  "&", Uri.EscapeDataString(oauth_token_secret));

            string oauth_signature;
            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                 hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            // create the request header 
            var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                 "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                 "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                 "oauth_version=\"{6}\"";

            var authHeader = string.Format(headerFormat,
                  Uri.EscapeDataString(oauth_nonce),
                  Uri.EscapeDataString(oauth_signature_method),
                  Uri.EscapeDataString(oauth_timestamp),
                  Uri.EscapeDataString(oauth_consumer_key),
                  Uri.EscapeDataString(oauth_token),
                  Uri.EscapeDataString(oauth_signature),
                  Uri.EscapeDataString(oauth_version)
                );



            ServicePointManager.Expect100Continue = false;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
            request.Headers.Add("Authorization", authHeader);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            var response = (HttpWebResponse)request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            var objText = reader.ReadToEnd();
            MessageBox.Show(objText);
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
            string COOKIE = "personalization_id=\"v1_mtJIfeIJiIusltSwFf9vcA == \"; guest_id=v1%3A163425675469341284; ct0=51538752fa1a54052fa807eecae38df3; gt=1448803970248372227; _gid=GA1.2.700969426.1634256759; _sl=1; _twitter_sess=BAh7CSIKZmxhc2hJQzonQWN0aW9uQ29udHJvbGxlcjo6Rmxhc2g6OkZsYXNo%250ASGFzaHsABjoKQHVzZWR7ADoPY3JlYXRlZF9hdGwrCJhVS4F8AToMY3NyZl9p%250AZCIlOWVkYzI0ODEzNDFhOTMzMTljNDk2NTUxYmM3ZDRlYWI6B2lkIiVjMjcw%250AYTVkOThjYzE0MzRmNjQxN2Y1N2E4OWMyYjAzMw%253D%253D--e238c6f2642915620ea4ca8e1e59f1b403dd8163; att=1-r943mPAr596EzvWFrPrWtrFHspHt6tWcrIn9uvdI; at_check=true; mbox=session#424ca849dd9e432ba360dfc37e31d0b6#1634261941|PC#424ca849dd9e432ba360dfc37e31d0b6.35_0#1697506499; _ga_34PHSZMC42=GS1.1.1634260084.1.1.1634261739.0; external_referer=padhuUp37zixoA2Yz6IlsoQTSjz5FgRcKMoWWYN3PEQ%3D|0|8e8t2xd8A2w%3D; _ga=GA1.2.1484680940.1634256759";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
              
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = "https://twitter.com/ankladium1";
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("authorization:Bearer AAAAAAAAAAAAAAAAAAAAANRILgAAAAAAnNwIzUejRCOuH5E6I8xnZz4puTs%3D1Zv7ttfk8LF81IUq16cHjhLTvJu4FA33AGWWjCpTnA");
                headers.Add("x-csrf-token:51538752fa1a54052fa807eecae38df3");
                headers.Add("x-guest-token:1448803970248372227");
               
                //添加头部
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


        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrlDefault(string url, string postData, string COOKIE)
        {
            try
            {

                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;//防止代理抓包
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                //request.ContentType = "application/x-www-form-urlencoded";
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = url;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

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
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion
        private void 推特ID转换_Load(object sender, EventArgs e)
        {

        }


        public string getname(string id)
        {
            string url = "https://tweeterid.com/ajax.php";
            string html = method.PostUrlDefault(url, "input="+id, "");
            return html.Trim();
        }
        public string getuid(string id)
        {
            string url = "https://twitter.com/i/api/1.1/onboarding/task.json";
            string html = method.PostUrlDefault(url, "input=" + id, "");
            return html.Trim();
        }

        public  void run()
        {
            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                try
                {
                    string[] text = richTextBox1.Lines[i].Trim().Split(new string[] { " " }, StringSplitOptions.None);
                    string id = text[0].Trim();
                    string pass = text[1].Trim();
                    string name = getname(id);
                   
                    if (name.Contains("@"))
                    {
                        name = name.Replace("@","");
                       
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        lv1.SubItems.Add(id);
                        lv1.SubItems.Add(pass);
                        lv1.SubItems.Add(name);
                       

                    }
                    Thread.Sleep(1000);

                }
                catch (Exception)
                {

                    continue;
                }

            }
        }

        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            //getMentionV3();
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
