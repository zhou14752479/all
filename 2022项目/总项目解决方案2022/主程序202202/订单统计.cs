using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202202
{
    public partial class 订单统计 : Form
    {
        public 订单统计()
        {
            InitializeComponent();
        }
        #region POST默认请求
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
                //request.Proxy = null;//防止代理抓包
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                request.ContentType = "application/x-www-form-urlencoded";
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                //request.ContentType = "application/json";
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
       public static string cookie = "PHPSESSID=ndkrptqrs1gi2ru38j9tfs0b37;username=yinlong;userid=5577;";

        long dingdancount = 0;
        long xiadancount = 0;
        double zongjine = 0;

        Dictionary<string, long> dics_dingdan = new Dictionary<string, long>();
        Dictionary<string, long> dics_xiadan = new Dictionary<string, long>();
        Dictionary<string, double> dics_jine = new Dictionary<string, double>();


        public void run()
        {
            for (int i = 1; i <999999; i++)
            {
                label12.Text = "正在计算第"+i+"页";
                string url = "http://110.40.186.121/jiuwuxiaohun.php?m=Admin&c=Orders&a=adminGoodsList&goods_id=42589&_=1646289301606";
                
                string postdata = "pageSize=1000&pageCurrent="+i+"&orderField=%24%7Bparam.orderField%7D&orderDirection=%24%7Bparam.orderDirection%7D&orderstate=3&ksid=&zpid=&OrderId=&CardId=&username=&cardno=&start_time="+dateTimePicker1.Value.ToString("yyyy-MM-dd")+ "&end_time=" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "&last_start_time=&last_end_time=&total=";
                string html = PostUrlDefault(url,postdata,cookie);

                MatchCollection xiadans = Regex.Matches(html, @"</th>                <td>([\s\S]*?)</td>");
                MatchCollection jines = Regex.Matches(html, @"<span class=""label label-warning"">([\s\S]*?)</span>");
                MatchCollection userids = Regex.Matches(html, @"user_id=([\s\S]*?)""");

                dingdancount = dingdancount + xiadans.Count;
                if(xiadans.Count==0)
                {
                    label12.Text = "计算完成";
                    return;
                }
                for (int j = 0; j < xiadans.Count; j++)
                {
                    textBox4.Text = "";
                    double jine = Convert.ToDouble(jines[j].Groups[1].Value.Replace("元", ""));
                    long xiadan = Convert.ToInt32(xiadans[j].Groups[1].Value);
                    if (!dics_dingdan.ContainsKey(userids[j].Groups[1].Value))
                    {
                        dics_dingdan.Add(userids[j].Groups[1].Value, 1);
                        dics_xiadan.Add(userids[j].Groups[1].Value, xiadan);
                        dics_jine.Add(userids[j].Groups[1].Value, jine);
                    }
                    else
                    {
                        dics_dingdan[userids[j].Groups[1].Value] = dics_dingdan[userids[j].Groups[1].Value] + 1;
                        dics_xiadan[userids[j].Groups[1].Value] = dics_xiadan[userids[j].Groups[1].Value] + xiadan;
                        dics_jine[userids[j].Groups[1].Value] = dics_jine[userids[j].Groups[1].Value] + jine;

                    }
              
                    for (int a = 0; a < dics_dingdan.Count; a++)
                    {
                        textBox4.Text += dics_dingdan.ElementAt(a).Key + "订单数："+dics_dingdan.ElementAt(a).Value+" " + "下单数：" + dics_xiadan.ElementAt(a).Value + " " + "金额：" + dics_jine.ElementAt(a).Value.ToString("F2") + " \r\n";
                    }

                    xiadancount = xiadancount + xiadan;
                    zongjine = zongjine + jine;

                    label9.Text = dingdancount.ToString();
                    label10.Text = xiadancount.ToString();
                    label11.Text = zongjine.ToString("F2");
                }
            }
        }

        #region  网络图片转Bitmap
        public Bitmap UrlToBitmap(string url)
        {
            WebClient mywebclient = new WebClient();
            mywebclient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
            mywebclient.Headers.Add("Cookie", logincookie);
            byte[] Bytes = mywebclient.DownloadData(url);

            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                Image outputImg = Image.FromStream(ms);

                Bitmap map = new Bitmap(outputImg);
                return map;
            }
        }
        #endregion

        private void 订单统计_Load(object sender, EventArgs e)
        {
            

            pictureBox1.Image = UrlToBitmap("http://110.40.186.121/jiuwuxiaohun.php/Publics/verify_code.html?random=1646292948602");
            #region 通用检测


            string html = PostUrlDefault("http://www.acaiji.com/index/index/vip.html", "", "utf-8");

            if (!html.Contains(@"18059892545"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion

            dateTimePicker1.Value = DateTime.Now.AddDays(-1);
        }

        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {
            //string queryURL = "http://110.40.186.121/jiuwuxiaohun.php?m=Admin&c=Orders&a=adminGoodsList&goods_id=42589&_=1646289301606";

            //var content = "pageSize=1000&pageCurrent=1&orderField=%24%7Bparam.orderField%7D&orderDirection=%24%7Bparam.orderDirection%7D&orderstate=3&ksid=&zpid=&OrderId=&CardId=&username=&cardno=&start_time=" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "&end_time=" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "&last_start_time=&last_end_time=&total=";


            //var handler = new HttpClientHandler() { UseCookies = false };
            //var client = new HttpClient(handler);// { BaseAddress = baseAddress };

            //var httpRequestMessage = new HttpRequestMessage
            //{

            //    Method = HttpMethod.Post,
            //    RequestUri = new Uri(queryURL),
            //    Content = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded")
            //};
            //httpRequestMessage.Headers.Add("Cookie", cookie);
            //var result = client.SendAsync(httpRequestMessage);
            //textBox4.Text = result.Result.Content.ReadAsStringAsync().Result;




            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }


        #region POST默认请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrlDefault_getcookie(string url, string postData, string COOKIE)
        {
            try
            {

                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;//防止代理抓包
               
                request.ContentType = "application/x-www-form-urlencoded";
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                //request.ContentType = "application/json";
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
          
                string content = response.GetResponseHeader("Set-Cookie"); ;
              
     
                response.Close();
                return content;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion

        string logincookie = "username=yinlong; userid=5577; bjui_theme=blue; PHPSESSID=l8fqtsin9ovifsuavjv3jcqtf3";
        private void button1_Click(object sender, EventArgs e)
        {
            string url = "http://110.40.186.121/jiuwuxiaohun.php/Publics/ajax_login";
            string postdata = "username=yinlong&passwordhash=9b6e513292f758bc67e429eee4964d0413511a57e154450e5310fce52a399d9a&code=" + textBox3.Text + "&sms_code=&wx_code=";
            string  oldcookie = PostUrlDefault_getcookie(url,postdata,"");
            MessageBox.Show(oldcookie);
            string php = Regex.Match(oldcookie, @"PHPSESSID=([\s\S]*?);").Groups[1].Value;
            cookie = "PHPSESSID=" + php + ";username=yinlong;userid=5577;"; 

            if(php!="" && oldcookie.Contains("username"))
            {
                MessageBox.Show("登录成功");
            }

            if (textBox2.Text!= "wu321321")
            {
                MessageBox.Show("密码错误");

            }


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = UrlToBitmap("http://110.40.186.121/jiuwuxiaohun.php/Publics/verify_code.html?random=1646292948602");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            登录 login = new 登录();
            login.Show();
        }
    }
}
