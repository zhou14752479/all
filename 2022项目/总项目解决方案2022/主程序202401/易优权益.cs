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

namespace 主程序202401
{
    public partial class 易优权益 : Form
    {
        public 易优权益()
        {
            InitializeComponent();
        }

        private void 易优权益_Load(object sender, EventArgs e)
        {
          
          
            
            pictureBox1.Image=  GetImage("https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=gQE68DwAAAAAAAAAAS5odHRwOi8vd2VpeGluLnFxLmNvbS9xLzAycUw2T1VmSVFjWWoxS1ByQTFDY0QAAgSzTn1mAwQAjScA");
        }
        #region POST默认请求
        public static string PostUrlDefault(string url, string postData, string COOKIE)
        {
            string result;
            try
            {
                string charset = "utf-8";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("version:TYC-XCX-WX");
                //request.ContentType = "application/x-www-form-urlencoded";
                request.ContentType = "application/json";
                request.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
                request.Referer = url;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                response.GetResponseHeader("Set-Cookie");
                bool flag = response.Headers["Content-Encoding"] == "gzip";
                string html;
                if (flag)
                {
                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader2 = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
                    html = reader2.ReadToEnd();
                    reader2.Close();
                }
                response.Close();
                result = html;
            }
            catch (WebException ex)
            {
                //result = ex.ToString();
                //400错误也返回内容
                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }
        #endregion

        string canshu;

        #region GET请求
        public static string GetUrl(string Url, string charset)
        {
            string COOKIE = "";
            string result;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Proxy = null;
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("version:TYC-XCX-WX");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                bool flag = response.Headers["Content-Encoding"] == "gzip";
                string html;
                if (flag)
                {
                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader2 = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
                    html = reader2.ReadToEnd();
                    reader2.Close();
                }
                response.Close();
                result = html;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        #endregion

        #region 网址转图片
        public static Image GetImage(string url)
        {
            Image result;
            try
            {
                WebRequest request = WebRequest.Create(url);
                Image img;
                using (WebResponse response = request.GetResponse())
                {
                    bool flag = response.ContentType.IndexOf("text") != -1;
                    if (flag)
                    {
                        Stream responseStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        string srcString = reader.ReadToEnd();
                        return null;
                    }
                    img = Image.FromStream(response.GetResponseStream());
                }
                result = img;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                result = null;
            }
            return result;
        }
        #endregion


        #region  时间戳转时间
        public static DateTime ConvertStringToDateTime(string timeStamp)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(Convert.ToDouble(timeStamp));
        }
        #endregion
        string cookie = "sessionID=cf6266ee373834897eb625c32578b5e4; auth_token=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE3MjAwOTIzOTEsImp0aSI6IjEiLCJUeXBlIjoiY29uc29sZSIsIlN1YnN0YXRpb25JRCI6MCwiSXNLaW5nIjp0cnVlLCJWZXJpZnkiOiIxMmZkM2JhZDlkZTdhMTkzNjIyMzZkMDcwZGJlNDhkNyJ9.cyg3zoH65iRZRflfBAk9btjN8eoZXihVwXz4FytUiig";
        public void run()
        {
            try
            {
                listView1.Items.Clear();
              
                   
                    string url = "http://gl.yy112.top/api/console/OrderManage/Paging";

                    string html = PostUrlDefault(url, "{\"type\":0,\"page\":1,\"list_rows\":10,\"status\":0,\"goods_name\":\"\",\"docking_status\":0,\"id\":null,\"goods_id\":null,\"customer_id\":null,\"buy_params\":null,\"docking_site_id\":0,\"supplier_id\":null}",cookie);


                    MatchCollection buy_numbers = Regex.Matches(html, @"""buy_number"":([\s\S]*?),");
                    MatchCollection amounts = Regex.Matches(html, @"""amount"":([\s\S]*?),");
                    MatchCollection goods_names = Regex.Matches(html, @"""goods_name"": ""([\s\S]*?)""");
                    MatchCollection create_times = Regex.Matches(html, @"""create_time"":([\s\S]*?),");

                    MatchCollection parameters = Regex.Matches(html, @"""parameter"": ""([\s\S]*?)""");


               
                label1.Text = DateTime.Now.ToString()+"："+ "下单参数：" + parameters[0].Groups[1].Value + " " + "下单数量：" + buy_numbers[0].Groups[1].Value + "实付金额：" + amounts[0].Groups[1].Value + "下单时间：" + ConvertStringToDateTime(create_times[0].Groups[1].Value).ToString();
                    for (int i = 0; i < 10; i++)
                    {
                        try
                        {
                           
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(goods_names[i].Groups[1].Value);
                            lv1.SubItems.Add(parameters[i].Groups[1].Value);
                            lv1.SubItems.Add(buy_numbers[i].Groups[1].Value);
                            lv1.SubItems.Add(amounts[2*i].Groups[1].Value);
                            lv1.SubItems.Add(ConvertStringToDateTime(create_times[2*i].Groups[1].Value).ToString());


                        }
                        catch (Exception)
                        {

                            continue;
                        }
                    }


                if (parameters.Count > 0)
                {
                    if (canshu != parameters[0].Groups[1].Value)
                    {
                        sendmsg(goods_names[0].Groups[1].Value, "下单参数：" + parameters[0].Groups[1].Value + " " + "下单数量：" + buy_numbers[0].Groups[1].Value + "实付金额：" + amounts[0].Groups[1].Value + "下单时间：" + ConvertStringToDateTime(create_times[0].Groups[1].Value).ToString());
                        canshu = parameters[0].Groups[1].Value;
                    }

                }

            }
            catch (Exception ex)
            {
               
            }


        }

        Thread thread;

        #region wxpush消息UID
        public string getuids()
        {
            StringBuilder sb = new StringBuilder();
            string url = "http://wxpusher.zjiecode.com/api/fun/wxuser/v2?appToken=AT_Zwbx5uVZTIpxJ2OPaCGXXOZNuiWHmTKQ&page=1";
            string html =GetUrl(url, "utf-8");
            MatchCollection uids = Regex.Matches(html, "\"uid\":\"([\\s\\S]*?)\"");
            foreach (object obj in uids)
            {
                Match item = (Match)obj;
                sb.Append("\"" + item.Groups[1].Value + "\",");
            }
            return sb.ToString().Remove(sb.ToString().Length - 1, 1);
        }
        #endregion

        #region 发送wxpush消息
        public void sendmsg(string title, string neirong)
        {
            bool flag = title.Trim() != "";
            if (flag)
            {
               
                string url = "https://wxpusher.zjiecode.com/api/send/message";
                string postdata = string.Concat(new string[]
                {
                    "{\"appToken\":\"AT_LtFEsRJbxhHfYSd5RVWGMXsaGYHPRm8c\",\"content\":\"",
                    title,
                    neirong,
                    "\",\"contentType\":2,\"uids\":[\""+textBox1.Text+"\"],  \"verifyPay\":false, \"verifyPayType\":0}" });
                string html = PostUrlDefault(url, postdata, "");
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now > Convert.ToDateTime("2024-08-01"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
    }
}
