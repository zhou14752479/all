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
using myDLL;

namespace 微博评论
{
    public partial class 微博评论 : Form
    {
        public 微博评论()
        {
            InitializeComponent();
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
        public static string PostUrl(string url, string postData, string COOKIE,string token)
        {
            try
            {
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("mweibo-pwa:1");
                headers.Add("x-xsrf-token:"+token);
              
                //添加头部
                request.ContentType = "application/x-www-form-urlencoded";
               // request.ContentType = contentType;
                //request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "https://m.weibo.cn/detail/4618933598946851";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding("utf-8"));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
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

        
        public void comment(string weiboId, string body,string token,string cookie)
        {
            string url = "https://m.weibo.cn/api/comments/create";

            //string body = "ceshi123456";
            // string weiboId = "4618933598946851";
            string st = token;
            string postdata = "content="+ System.Web.HttpUtility.UrlEncode(body) + "&mid="+weiboId+"&st="+st+"&_spr=screen%3A1920x1080";
            string html = PostUrl(url,postdata,cookie,token);
          
            if (html.Contains("created_at"))
            {
                label1.Text = "评论微博成功";
            }

        }

        public void huifu(string weiboId, string replyId,string body,string token, string cookie)
        {
            string url = "https://m.weibo.cn/api/comments/reply";

            //string body = "ceshi123456";
           // string weiboId = "4618933598946851";
            //string replyId = "4648012401872872";
            string st = token;
            string postdata = "id="+weiboId+ "&reply="+replyId+ "&content=" + body + "&withReply=1&mid=" + weiboId + "&cid=" + replyId + "&st=" + st + "&_spr=screen%3A1920x1080";
            string html = PostUrl(url, postdata, cookie,token);
           
            if (html.Contains("created_at"))
            {
                label1.Text = "评论回复成功";
            }

        }
        string str = "qwertyuiopasdfghjklmnbvcxz";
        public void run()
        {
            Random random = new Random();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string nownickname = listView1.Items[i].SubItems[1].Text;
                string nowcookie = listView1.Items[i].SubItems[4].Text;
                string[] urls = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string url in urls)
                {
                    string zimu= str[random.Next(0, 25)].ToString()+ str[random.Next(0, 25)].ToString()+ str[random.Next(0, 25)].ToString();
                    string body = textBox2.Text.Trim();
                    if (checkBox1.Checked == true)
                    {
                        body =zimu+ body;
                    }

                    if (checkBox2.Checked == true)
                    {
                        body = body+ zimu;
                    }

                    string weiboid = Regex.Match(url, @"id=([\s\S]*?)&").Groups[1].Value;
                    string replyid = Regex.Match(url, @"reply=([\s\S]*?)&").Groups[1].Value;
                    string token = Regex.Match(nowcookie, @"XSRF-TOKEN=([\s\S]*?);").Groups[1].Value;
                    if (token == "")
                    {
                        token = Regex.Match(nowcookie, @"XSRF-TOKEN=.*").Groups[0].Value.Replace("XSRF-TOKEN=","");
                    }
                    if (comboBox1.Text == "评论微博")
                    {
                        comment(weiboid, body,token,nowcookie);
                    }

                    else if (comboBox1.Text == "评论回复")
                    {
                        huifu(weiboid, replyid, body,token,nowcookie);

                    }
                    Thread.Sleep(1000);
                   // Thread.Sleep(Convert.ToInt32(textBox3.Text) * 1000);


                    textBox4.Text += DateTime.Now.ToShortTimeString()+"   " +nownickname + "：评论" + url + "成功！"+"\r\n";
                }
            }
           

        }
        private void 微博评论_Load(object sender, EventArgs e)
        {
           

        }



        List<string> userlist = new List<string>();
        public void addcookie()
        {

            //textBox1.Text = 微博登录.linshiCookie;
            string html = method.GetUrlWithCookie("https://m.weibo.cn/api/config", 微博登录.linshiCookie, "utf-8");

            string uid = Regex.Match(html, @"""uid"":""([\s\S]*?)""").Groups[1].Value;
            string ahtml = method.GetUrlWithCookie("https://m.weibo.cn/profile/info?uid="+uid, 微博登录.linshiCookie, "utf-8");
            string nickname= Regex.Match(ahtml, @"""screen_name"":""([\s\S]*?)""").Groups[1].Value;
            
            if (!userlist.Contains(uid))
            {
                userlist.Add(uid);
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                lv1.SubItems.Add(nickname);
                lv1.SubItems.Add("成功");
                lv1.SubItems.Add(uid);
                lv1.SubItems.Add(微博登录.linshiCookie);
            }
        }
       
        private void button4_Click(object sender, EventArgs e)
        {

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(addcookie);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            微博登录 login = new 微博登录();
            login.Show();
        }
    }
}
