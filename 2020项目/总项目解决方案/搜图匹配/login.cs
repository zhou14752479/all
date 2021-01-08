using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 搜图匹配
{
    public partial class login : Form
    {
        public login()
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


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                // request.KeepAlive = true;
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

        public string logins(string username, string password)
        {
            try
            {
                string url = "http://47.242.69.104/api/do.php?method=login&username=" + username + "&password=" + password;
                string html = GetUrl(url, "utf-8");
                return html.Trim();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static string user;
        public static string xiugai(string username, string islogin)
        {
            try
            {
                string url = "http://47.242.69.104/api/do.php?method=xiugai&username=" + username + "&islogin=" + islogin;
                string html = GetUrl(url, "utf-8");
                return html.Trim();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void run()
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("请输入账号密码");
                return;
            }
            string status = logins(textBox1.Text.Trim(), textBox2.Text.Trim());

            if (status.Contains("true"))
            {
                user = textBox1.Text.Trim();
                Form1 ma = new Form1();
                ma.Show();
               
                //xiugai(user, "1");
                this.Hide();
            }
            else
            {
                MessageBox.Show(status);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            run();
        }

        private void login_Load(object sender, EventArgs e)
        {

        }
    }
}
