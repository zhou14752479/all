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

namespace 萌推发货
{
    public partial class 萌推发货 : Form
    {
        public 萌推发货()
        {
            InitializeComponent();
        }

        public static string COOKIE = "UM_distinctid=17d5bd3547f602-030ffbe1902c83-4343363-1fa400-17d5bd3548060e; CNZZDATA1278658731=281648808-1637921871-https%253A%252F%252Fwww.baidu.com%252F%7C1638049274; session=55736-1638231941.836a2fad729b1b9d88d92a21678b42af.1638059141.55736-.1638231941";

        public static string UA = "";
        
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public string GetUrl(string Url, string charset)
        {
            string html = "";
          
            try
            {
               System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = UA;
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


        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public  string PostUrl(string url, string postData, string charset)
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

                request.UserAgent = UA;
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "https://web.duanmatong.cn/";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

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

        public string fahuo(string orderid)
        {
            string url = "https://mms.mengtuiapp.com/api/deliver";
            string postdata = "{\"logistic_id\":0,\"order_id\":\""+orderid+"\",\"tracking_num\":null,\"deliver_method\":2,\"recharge_link\":\"\",\"voucher\":\"\"}";
            string html = PostUrl(url,postdata,"utf-8");
            return html;
        }
        public void run()
        {
            try
            {
                for (int page = 1; page <999; page++)
                {


               string url = "https://mms.mengtuiapp.com/api/order/list?page="+page+"&size=20&label_buyer_time=success_time&success_time[]="+ DateTime.Now.AddDays(-89).ToString("yyyy-MM-dd") + "+00:00:00&success_time[]="+DateTime.Now.ToString("yyyy-MM-dd")+"+23:59:59&process_status=4&n_c=1";
                  
                    string html = GetUrl(url,"utf-8");
                    MessageBox.Show(html);
                    MatchCollection uids = Regex.Matches(html, @"{""id"":""([\s\S]*?)""");
                    MatchCollection names = Regex.Matches(html, @"""goods_name"":""([\s\S]*?)""");
                    if (uids.Count==0)
                    {
                        MessageBox.Show("完成");
                        return;
                    }
                    for (int i = 0; i < uids.Count; i++)
                    {
                        string orderid = uids[i].Groups[1].Value;
                        string fahuohtml = fahuo(orderid);

                        if(fahuohtml.Contains("success\":true"))
                        {
                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            listViewItem.SubItems.Add(orderid);
                            listViewItem.SubItems.Add(names[i].Groups[1].Value);
                            listViewItem.SubItems.Add("发货成功");
                            label1.Text = orderid + "发货成功";
                        }
                        else
                        {
                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            listViewItem.SubItems.Add(orderid);
                            listViewItem.SubItems.Add(names[i].Groups[1].Value);
                            listViewItem.SubItems.Add("发货失败");
                            label1.Text = orderid + "发货失败,等待5秒";
                            Thread.Sleep(5000);
                        }


                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void 萌推发货_Load(object sender, EventArgs e)
        {
           // pictureBox1.Image = UrlToBitmap("data:image/jpeg;base64,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");
        }

        Thread thread;
        bool status = true;
        private void button1_Click(object sender, EventArgs e)
        {
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
            status = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            登录 login = new 登录();
            login.Show();
        }
    }
}
