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
            cookie = "nr-browser=1; _st=1582967184971; JSESSIONID=38C1965DD401AC6B18E4FBD52B020054.blade05_cn_a_bnet_shop; web.id=CN-6bf7c1ca-592f-4823-a5c0-f1e36dceb124; _ga=GA1.3.2100474302.1582966974; _gid=GA1.3.339242375.1582966974; LPVID=VlMDVkZjk4MDUwOGY4NjUw; loc=zh-cn; optimizelyEndUserId=oeu1582967176045r0.9065126639076413; mprtcl-v4_CCA8AE13={'gs':{'ie':1|'dt':'719f10ea1d9f664eab8238c61651c212'|'cgid':'8c6a111b-7f3a-447a-95d9-bc05a0d09d1a'|'das':'765acb3b-19ec-42e9-a50c-e015a21810f6'|'csm':'WyI1MzkzODk1MDM0NDEzNDU3OTg5IiwiNjc3MjI0NTc3MDE0ODYzNDUzMiJd'}|'l':1|'1032215442762915510':{'fst':1582967000201|'csd':'eyI0MSI6MTU4Mjk2NzAwMDIwOCwiMTAzIjoxNTgyOTY3MDAwMjEwfQ=='|'lst':1582967101394}|'cu':'6772245770148634532'|'5393895034413457989':{'fst':1582967101395|'csd':'eyI0MSI6MTU4Mjk2NzEwMTQwMCwiMTAzIjoxNTgyOTY3MTAxNDAyfQ=='|'ui':'eyIxIjoiMTg2NjA2OTE2In0='|'lst':1582969677831}|'6772245770148634532':{'fst':1582969677834|'csd':'eyI0MSI6MTU4Mjk2OTY3NzgzOSwiMTAzIjoxNTgyOTY5Njc3ODQyfQ=='|'ui':'eyIxIjoiNTIyMjgwODE1In0='}}; BA-tassadar-login.key=efd17e3dd069ab24cee25cf8452c46f5; login.key=efd17e3dd069ab24cee25cf8452c46f5; opt=1; ticket-id=eyJzZXJ2aW5nSWQiOjE1ODI4NzE1MDYsImxhc3ROb3dTZXJ2aW5nU2VlbiI6MTU4Mjg3NDUwMiwicmVkaXJlY3RVcmwiOiJodHRwczovL3d3dy5iYXR0bGVuZXQuY29tLmNuL3Nob3AvemgvY2hlY2tvdXQvZWJhbGFuY2UtY2xhaW0/bnVsbCJ9|d68ad0c761a642267b5a3c41230cb6aa7737aef957cd46e5e88db961c120792f; loginChecked=1; _gat_UA-50249600-1=1; _gat_UA-50249600-81=1";
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
