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


namespace 好好住
{
    public partial class 好好住 : Form
    {
        public 好好住()
        {
            InitializeComponent();
        }


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
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;
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

        public static void downloadFile(string URLAddress, string subPath, string name, string COOKIE)
        {
            try
            {
                string path = Directory.GetCurrentDirectory();
                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
                client.Headers.Add("Cookie", COOKIE);
                client.Headers.Add("Referer", "https://m.mm131.net/chemo/89_5.html");
                bool flag = !Directory.Exists(subPath);
                if (flag)
                {
                    Directory.CreateDirectory(subPath);
                }
                client.DownloadFile(URLAddress, subPath + "\\" + name);
            }
            catch (WebException ex)
            {
                ex.ToString();
            }
        }


        public void chuli(string aid)
        {
  
                string url = "https://m.haohaozhu.cn/bangyang/" + aid + ".html?no-pc-redirect=true";
                string html =GetUrl(url, "utf-8");
                string title = Regex.Match(html, "\"title\":\"([\\s\\S]*?)\"").Groups[1].Value.Replace("-好好住", "").Trim();
                string description = Regex.Match(html, "\"description\":\"([\\s\\S]*?)\"").Groups[1].Value;
                MatchCollection remarks = Regex.Matches(html, "\"remark\":\"([\\s\\S]*?)\"");
                MatchCollection picurls = Regex.Matches(html, "\"water_pic_url\":\"([\\s\\S]*?)\"");
                StringBuilder sb = new StringBuilder();
                sb.Append(title + "\n");
                sb.Append(description + "\n");
                foreach (object obj in remarks)
                {
                    Match item = (Match)obj;
                    sb.Append(item.Groups[1].Value + "\n");
                }
                for (int i = 0; i < picurls.Count; i++)
                {
                    TextBox textBox = this.textBox3;
                textBox3.Text = string.Concat(new object[]
                    {
                            textBox.Text,
                            "正在下载图片：",
                            title,
                            i,
                            "\r\n"
                    });
                    好好住.downloadFile(picurls[i].Groups[1].Value, this.path + "//images//" + title + "//", i + ".jpg", "");
                }
                File.WriteAllText(this.path + "//" + title + ".txt", sb.ToString(), Encoding.UTF8);
            textBox3.Text=(title + "-保存完成");
            
        }
        public void run()
        {
            try
            {
              
                if (this.textBox2.Text !="")
                {
                    this.path = this.textBox2.Text;
                }

                if (textBox1.Text.Contains("user"))
                {
                    string url = textBox1.Text;
                    string html = GetUrl(url, "utf-8");
                    MatchCollection aids = Regex.Matches(html, "\"article_id\":\"([\\s\\S]*?)\"");
                    foreach (Match aid in aids)
                    {
                     
                        chuli(aid.Groups[1].Value);

                    }

                }
                else
                {
                    string aid = Regex.Match(textBox1.Text.Trim(), "bangyang([\\s\\S]*?)html").Groups[1].Value;

                    chuli(aid);
                }
                MessageBox.Show("完成");
            }
            catch (Exception)
            {
                throw;
            }
        }

        Thread thread;
        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void button1_Click(object sender, EventArgs e)
        {
          
            if (this.thread == null || !this.thread.IsAlive)
            {
                this.thread = new Thread(new ThreadStart(this.run));
                this.thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择所在文件夹";
            bool flag = dialog.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                bool flag2 = string.IsNullOrEmpty(dialog.SelectedPath);
                if (flag2)
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                }
                else
                {
                    this.textBox2.Text = dialog.SelectedPath;
                }
            }
        }
    }
}
