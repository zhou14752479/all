using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 战网充值
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string cookie = "";
        private void Form1_Load(object sender, EventArgs e)
        {
            //webBrowser1.ScriptErrorsSuppressed = true;
            method.SetWebBrowserFeatures(method.IeVersion.IE10);
            webBrowser1.Url = new Uri("https://www.battlenet.com.cn/login/zh/?ref=https://www.battlenet.com.cn/shop/zh/checkout/ebalance-claim&app=shop");
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
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = "application/x-www-form-urlencoded";
            //添加头部
            //WebHeaderCollection headers = request.Headers;
            //headers.Add("appid:orders");
            //headers.Add("x-nike-visitid:5");
            //headers.Add("x-nike-visitorid:d03393ee-e42c-463e-9235-3ca0491475b4");
            //添加头部
            // request.ContentType = "application/json";
            request.ContentLength = postData.Length;
            request.AllowAutoRedirect = true;
            request.KeepAlive = true;

            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
            request.Headers.Add("Cookie", COOKIE);
            //request.Headers.Add("origin","https://www.nike.com");
            request.Referer = "https://www.nike.com/orders/gift-card-lookup";
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

        #endregion
        bool zanting = true;
        public void run()

        {
            string ahtml = method.GetUrlWithCookie("https://www.battlenet.com.cn/shop/zh/checkout/ebalance-claim",cookie, "utf-8");
            Match token = Regex.Match(ahtml,@"csrftoken"" value=""([\s\S]*?)""");
            
            textBox2.Text = token.Groups[1].Value;
         
            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i<text.Length; i++)
            {
                string[] ka = text[i].Split(new string[] { "----" }, StringSplitOptions.None);

                string user = ka[0];
                string pass = ka[1];
                try
                {
                    string url = "https://www.battlenet.com.cn/shop/zh/checkout/ebalance-claim";
                    string postdata = "csrftoken="+token.Groups[1].Value+"&keyCode="+user+"&keyPin="+pass+"&returnUrl=https%3A%2F%2Fshop.battlenet.com.cn%2F";
                   
                    string html = PostUrl(url,postdata, cookie, "utf-8");
                    if (html.Contains("兑换成功"))
                    {
                        textBox2.Text += user + "充值成功" + "\r\n";
                    }
                    else
                    {
                        textBox2.Text += user + "充值失败" + "\r\n";
                    }

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    Thread.Sleep(1000);
                }

                catch(Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                    //continue;
                }


            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //cookie = method.getUrlCookie("https://www.battlenet.com.cn/shop/zh/checkout/ebalance-claim");
            cookie = textBox3.Text;
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }
    }
}
