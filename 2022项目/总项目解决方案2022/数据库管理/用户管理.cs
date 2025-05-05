using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace 数据库管理
{
    public partial class 用户管理 : Form
    {
        public 用户管理()
        {
            InitializeComponent();
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

        public void run()
        {
            try
            {

                listView1.Items.Clear();    
                string url = "http://8.155.4.205/main.aspx?method=getusers";
                string html = GetUrl(url, "utf-8");

                MatchCollection userid = Regex.Matches(html, @"""userid"":([\s\S]*?),");
                MatchCollection username = Regex.Matches(html, @"""username"":""([\s\S]*?)""");
                MatchCollection password = Regex.Matches(html, @"""password"":""([\s\S]*?)""");
                MatchCollection time = Regex.Matches(html, @"""time"":""([\s\S]*?)""");
                MatchCollection status = Regex.Matches(html, @"""status"":""([\s\S]*?)""");
                MatchCollection cishu = Regex.Matches(html, @"""cishu"":""([\s\S]*?)""");


                for (int a = 0; a < userid.Count; a++)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                  
                    lv1.SubItems.Add(username[a].Groups[1].Value);
                    lv1.SubItems.Add(password[a].Groups[1].Value);
                    lv1.SubItems.Add(time[a].Groups[1].Value);
                    lv1.SubItems.Add(status[a].Groups[1].Value);
                    lv1.SubItems.Add(cishu[a].Groups[1].Value);
                    lv1.SubItems.Add(userid[a].Groups[1].Value);
                    if (listView1.Items.Count > 2)
                    {
                        this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                    }



                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }


        }



    }
}
