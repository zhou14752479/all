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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202006
{
    public partial class 百家号发布 : Form
    {
        public 百家号发布()
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
        public static string PostUrl(string url, string postData)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
               // request.ContentType = "application/x-www-form-urlencoded";

            
               request.ContentType = "application/json";
               // request.ContentLength = postData.Length;
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie","");

                request.Referer = "";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

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
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://www.amazon.com/s?k=6Q0+959+856&__mk_zh_CN=%E4%BA%9A%E9%A9%AC%E9%80%8A%E7%BD%91%E7%AB%99&ref=nb_sb_noss";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
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
                ex.ToString();

            }
            return "";
        }
        #endregion

        public void caiji()
        {
            int start = Convert.ToInt32(textBox1.Text);
            int end = Convert.ToInt32(textBox1.Text) + Convert.ToInt32(textBox2.Text);
            for (int i = start; i < end; i++)
            {
                string url = "http://www.mitan.com.cn/question/"+i+".html";
                string html = GetUrl(url,"utf-8");
                Match biaoti = Regex.Match(html, @"<title>([\s\S]*?)-");
                Match body = Regex.Match(html, @"<div class=""detail-content"">([\s\S]*?)<script>");
                Match image = Regex.Match(html, @"center-block"" src=""([\s\S]*?)""");

                string img = "<img src=\""+image.Groups[1].Value+"\"/>";
                string title = "<h3>" + biaoti.Groups[1].Value + "</h3>";
                string content ="<p>"+ Regex.Replace(body.Groups[1].Value, "<(?!/?p)[^>]*>", "").Replace("\r\n","").Replace(" ","").Trim()+"</p>";
                string origin_url = url;
                string main=img+title + content;
                textBox3.Text = main;
                textBox4.Text = fabu(title, image.Groups[1].Value,origin_url,main);
            }

        }
        public string fabu(string title, string cover_images, string origin_url, string content)
        {
            string app_id = "1670348246558490";
            string app_token = "e882f481e932be3156e3f4b36b13853b";

            string url = "https://baijiahao.baidu.com/builderinner/open/resource/article/publish";
            string postdata = "{\"app_id\" : \""+app_id+ "\",\"app_token\" : \"" + app_token + "\", \"title\" : \"" + title + "\", \"cover_images\" : \"[{\"src\":\"" + cover_images + "\"}], \"origin_url\" : \"" + origin_url + "\", \"is_original\" :1, \"content\" : \"" + content + "\" }";
           
            string status=  PostUrl(url,postdata);
            return status;
            
         }
        private void 百家号发布_Load(object sender, EventArgs e)
        {
            foreach (Control ctr in this.Controls)
            {

                if (ctr is TextBox)
                {

                    string path = AppDomain.CurrentDomain.BaseDirectory;
                    if (File.Exists(path + ctr.Name + ".txt"))
                    {

                        StreamReader sr = new StreamReader(path + ctr.Name + ".txt", Encoding.GetEncoding("utf-8"));
                        //一次性读取完 
                        string texts = sr.ReadToEnd();
                        ctr.Text = texts;
                        sr.Close();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            caiji();
        }

        private void 百家号发布_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出吗？", "退出询问"
           , MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;//告诉窗体关闭这个任务取消

            }
            else
            {
                foreach (Control ctr in this.Controls)
                {
                    if (ctr is TextBox)
                    {


                        string path = AppDomain.CurrentDomain.BaseDirectory;
                        FileStream fs1 = new FileStream(path + ctr.Name + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1);
                        sw.WriteLine(ctr.Text);
                        sw.Close();
                        fs1.Close();

                    }
                }


                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
    }
}
