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

namespace 主程序202105
{
    public partial class 无忧考网 : Form
    {
        public 无忧考网()
        {
            InitializeComponent();
        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
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
        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void button2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }

                textBox2.Text = dialog.SelectedPath;
            }
        }

        public void run()
        {
            try
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("请输入关键词");
                    return;
                }

                if (textBox2.Text != "")
                {
                    path = textBox2.Text+"\\";
                }

                for (int i = 1; i < 20; i++)
                {
                    string url = "http://zhannei.baidu.com/cse/site?q=" + System.Web.HttpUtility.UrlEncode(textBox1.Text) + "&p="+i+"&stp=1&cc=51test.net";

                    string html = GetUrl(url, "utf-8");
                    MatchCollection aids = Regex.Matches(html, @"<a rpos="""" cpos=""title"" href=""([\s\S]*?)""");

                    for (int j= 0; j <aids.Count; j++)
                    {
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;

                        if (aids[j].Groups[1].Value.Contains("html"))
                        {
                            string ahtml = GetUrl(aids[j].Groups[1].Value, "gb2312");
                            string title = Regex.Match(ahtml, @"<h1>([\s\S]*?)</h1>").Groups[1].Value.Trim();
                            string body = Regex.Match(ahtml, @"<div class=""content-txt"" id=""content-txt"">([\s\S]*?)<div class=""word_download_tip_msg"">").Groups[1].Value.Replace("</p>","\r\n").Trim();
                            string value = title + "\r\n" + Regex.Replace(body, "<[^>]+>", "");
                          
                            FileStream fs1 = new FileStream(path + title + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("utf-8"));
                            sw.WriteLine(value);
                            sw.Close();
                            fs1.Close();
                            sw.Dispose();
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(title);
                            lv1.SubItems.Add("完成");
                            Thread.Sleep(3000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        Thread thread;
        bool zanting = true;
        bool status = true;
        private void button6_Click(object sender, EventArgs e)
        {
            #region 通用检测

           
            string html = GetUrl("http://www.acaiji.com:8080/api/vip.html", "utf-8");

            if (!html.Contains(@"dukYVo"))
            {

                return;
            }

            #endregion

            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void 无忧考网_Load(object sender, EventArgs e)
        {

        }
    }
}
