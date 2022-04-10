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

namespace 台湾彩券
{
    public partial class Form1 : Form
    {
        public Form1()
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; 
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
               
                request.ContentType = "application/x-www-form-urlencoded";
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
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

        public string cookie = "ASPSESSIONIDSGQDSDBR=EKBGDPBABGPIOBCKECEHDGDD; ASPSESSIONIDSGSBQBCR=DCJMDFIAPDOLEKLPGLIAADPD; _ga_QH8PJX46RG=GS1.1.1649578268.1.0.1649578268.60; _ga=GA1.3.140840180.1649578269; _gid=GA1.3.614156529.1649578269";
        public string VIEWSTATE = "";
        public string VIEWSTATEGENERATOR = "";
        public string EVENTVALIDATION = "";
        public string DropDownList1 = "2022%2F4";
        public string DropDownList2 = "1";

        public void run()
        {
            try
            {
                string url = "https://www.taiwanlottery.com.tw/lotto/BINGOBINGO/drawing.aspx";
                string html = PostUrlDefault(url,"",cookie);
                //textBox1.Text = html;
                VIEWSTATE = Regex.Match(html, @"VIEWSTATE"" value=""([\s\S]*?)").Groups[1].Value;
                VIEWSTATEGENERATOR = Regex.Match(html, @"VIEWSTATEGENERATOR"" value=""([\s\S]*?)").Groups[1].Value;
                EVENTVALIDATION = Regex.Match(html, @"EVENTVALIDATION"" value=""([\s\S]*?)").Groups[1].Value;


                VIEWSTATE = System.Web.HttpUtility.UrlEncode(VIEWSTATE);
                VIEWSTATEGENERATOR = System.Web.HttpUtility.UrlEncode(VIEWSTATEGENERATOR);
                EVENTVALIDATION = System.Web.HttpUtility.UrlEncode(EVENTVALIDATION);

                string postdata = string.Format("__EVENTTARGET=DropDownList2&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE={0}&__VIEWSTATEGENERATOR={1}&__EVENTVALIDATION={2}&DropDownList1={3}&DropDownList2={4}", VIEWSTATE, VIEWSTATEGENERATOR, EVENTVALIDATION, DropDownList1, DropDownList2);

                string ahtml = PostUrlDefault(url, postdata, cookie);
                string content = Regex.Match(ahtml, @"<td class=tdA_3></td></tr><tr>([\s\S]*?)</table>").Groups[1].Value;
                MatchCollection values = Regex.Matches(content, @"<td class=tdA_([\s\S]*?)>([\s\S]*?)</td>");

              
                MessageBox.Show(values.Count.ToString());
                //int count = values.Count / 5;
                //for (int i = 0; i < count; i++)
                //{
                //    string o1 = values[5*i].Groups[2].Value;
                //    string o2 = values[(5*i)+1].Groups[2].Value;
                //    string o3 = values[(5 * i) + 2].Groups[2].Value;
                //    string o4 = values[(5 * i) + 3].Groups[2].Value;
                //    string o5 = values[(5 * i) + 4].Groups[2].Value;

                //    FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\data.json", FileMode.Append, FileAccess.Write);//创建写入文件 
                //    StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                //    sw.WriteLine(o1+"  "+o2);
                //    sw.Close();
                //    fs1.Close();
                //    sw.Dispose();
                //}
               

            }
            catch (Exception ex)
            {

                textBox1.Text=ex.ToString();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

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
    }
}
