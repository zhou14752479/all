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

namespace 天眼查企业查询
{
    public partial class 天眼查企业查询 : Form
    {
        public 天眼查企业查询()
        {
            InitializeComponent();
        }
        public void gettoken()
        {
            string ahtml = method.GetUrl("http://www.acaiji.com/shangxueba2/shangxueba.php?method=getcookie", "utf-8");

            token = ahtml.Trim().Replace("\r", "").Replace("\n", "");


        }

        string token = "";

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "";
            try
            {
                if (token.Substring(0, 1) != "e")
                {
                    token = token.Remove(0, 1);
                }
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("version:TYC-XCX-WX");
                headers.Add("X-AUTH-TOKEN: " + token);
                request.Headers.Add("Cookie", COOKIE);
                headers.Add("Authorization: 0###oo34J0YSWR0ucFrTbNJwK7MpAWiE###1639990889998###2ed6a2ec3ab12c1623ccd098ee5711fd");
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
        #region POST默认请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public string PostUrlDefault(string url, string postData, string COOKIE)
        {
            try
            {
                string TOKEN = "X-AUTH-TOKEN:" + token;
                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //   request.Proxy = null;//防止代理抓包
                if (token.Substring(0, 1) != "e")
                {
                    token = token.Remove(0, 1);
                }


                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("version:TYC-XCX-WX");
                headers.Add("X-AUTH-TOKEN: " + token);
              
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

                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_6_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/8.0.4(0x1800042c) NetType/WIFI Language/zh_CN";
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
        public void run()
        {
            try
            {
                StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
                for (int i = 0; i < text.Length; i++)
                {
                    string postdata = "{\"sortType\":0,\"pageSize\":20,\"pageNum\":1,\"word\":\""+text[i]+"\",\"allowModifyQuery\":1}";
                    string html = PostUrlDefault("https://capi.tianyancha.com/cloud-tempest/app/searchCompany",postdata,"");

                    string title = Regex.Match(html, @"companyName"":""([\s\S]*?)""").Groups[1].Value;
                    string legalPersonName = Regex.Match(html, @"legalPersonName"":""([\s\S]*?)""").Groups[1].Value;
                    string phone = Regex.Match(html, @"phone"":""([\s\S]*?)""").Groups[1].Value;
                    string addr = Regex.Match(html, @"regLocation"":""([\s\S]*?)""").Groups[1].Value;
                    string uid= Regex.Match(html, @"""graphId"":([\s\S]*?),").Groups[1].Value;
                   
                    string shangbiaourl = "https://api9.tianyancha.com/services/v3/expanse/allCountV3?id="+uid;
                    string sbhtml = GetUrl(shangbiaourl,"utf-8");
                   
                    string tmCount = Regex.Match(sbhtml, @"tmCount"":""([\s\S]*?)""").Groups[1].Value;

                    string tel = "";

                    try
                    {
                        tel = phone.Replace("\\", "").Replace("t", "").Split(new string[] { ";" }, StringSplitOptions.None)[0];
                    }
                    catch (Exception)
                    {

                    }
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(title.Replace("<em>", "").Replace("</em>", ""));
                    lv1.SubItems.Add(legalPersonName);
                    lv1.SubItems.Add(tel);
                    lv1.SubItems.Add(addr.Replace("<em>", "").Replace("</em>", ""));
                    lv1.SubItems.Add(tmCount);
                    if (status == false)
                        return;
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    label1.Text = DateTime.Now.ToShortTimeString() + "正在查询：" + text[i];
                    Thread.Sleep(800);
                }
              
            }
            catch (Exception ex)
            {
                label1.Text = ex.ToString();
                
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

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


        bool zanting = true;
        Thread thread;
        bool status = true;


        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {

            listView1.Items.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void 天眼查企业查询_Load(object sender, EventArgs e)
        {
            gettoken();
        }
    }
}
