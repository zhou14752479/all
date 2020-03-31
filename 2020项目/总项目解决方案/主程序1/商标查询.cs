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

namespace 主程序1
{
    public partial class 商标查询 : Form
    {
        public 商标查询()
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
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";

                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("Authorization: appId=19001&timestamp=1585623031450&sign=369d983492c3f36c0eec1e8c2925503e");
                headers.Add("X-token: null");
              
                //添加头部
                request.ContentType = "application/json";
                //request.ContentLength = postData.Length;
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
              
                request.Referer = "https://www.qccip.com/trademark/search/index.html?searchType=APPLICANT&keyword=%E5%B9%BF%E4%B8%9C%E6%80%9D%E8%AF%BA%E4%BC%9F%E6%99%BA%E8%83%BD%E6%8A%80%E6%9C%AF";
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
        bool zanting = true;
        #endregion

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "JSESSIONID=E9350A267A4F1733A3125E4F4C1DB49A";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                WebHeaderCollection headers = request.Headers;
                headers.Add("Upgrade-Insecure-Requests: 1");
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
               
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();


                reader.Close();
                response.Close();
                return content;



            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            return "";
        }
        #endregion

        public void run()
        {
            //StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
            //string text = streamReader.ReadToEnd();
            //string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

          
                try
                {
                string name = "广东思诺伟智能技术有限公司";
                    string url = "https://wx.xcwz.com.cn/applet/getTMPages.do?searchKey="+name+"&searchType=3&pageNo=3&t=1585621054635";
                    
                    string html = GetUrl(url,"utf-8");

                textBox2.Text = html;
                  
                    MatchCollection names = Regex.Matches(html, @"""stringCls"":""([\s\S]*?)""");

                for (int i = 0; i < names.Count; i++)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(names[i].Groups[1].Value);
                    

                }


                while (zanting == false)
                    {
                        Application.DoEvents();//等待本次加载完毕才执行下次循环.
                    }

                }
                catch (Exception ex)
                {
                MessageBox.Show(ex.ToString());
                    
                }


            
        }
        private void 商标查询_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
