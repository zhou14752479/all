using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace 百度贴吧ID采集
{
    public partial class 登录 : Form
    {
        public 登录()
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
               // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("appid:orders");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈


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

        #region 获取时间戳  秒
        /// <summary>
        /// 获取时间戳  秒
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalSeconds);
            return a.ToString();
        }
        #endregion

        #region base64解密
        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encodeType">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(Encoding encodeType, string result)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encodeType.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }
        #endregion

        #region  获取32位MD5加密
        public string GetMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        #endregion
        #region 获取Mac地址
        /// <summary>
        /// 获取Mac地址
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            try
            {
                string strMac = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        strMac = mo["MacAddress"].ToString();
                    }
                }
                moc = null;
                mc = null;
                return strMac;
            }
            catch
            {
                return "unknown";
            }
        }

        #endregion
        public void zhuce()
        {
            try
            {
                string jihuoma = textBox5.Text.Trim().Remove(textBox5.Text.Trim().Length-10,10).Remove(0, 10);

                string macmd5 = GetMD5(GetMacAddress());

                jihuoma = Base64Decode(Encoding.GetEncoding("utf-8"),jihuoma + "==");
               string time=jihuoma.Substring(6,10);  
                if(Convert.ToInt32(time)>Convert.ToInt32(GetTimeStamp()))
                {
                    string url = "http://www.lizhihui.love/xx/do.php?method=register&username=" + textBox3.Text.Trim()+"&password="+ textBox4.Text.Trim()+"&mac="+ macmd5;
                    string html = GetUrl(url,"utf-8");
                    string msg = Regex.Match(html,@"""msg"":""([\s\S]*?)""").Groups[1].Value;
                   if(msg=="")
                    {
                        MessageBox.Show(html.Trim());
                    }
                   else
                    {
                        MessageBox.Show(msg);
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("注册失败,激活码错误");
            }
        }







        public void login()
        {
            try
            {
                //www.lizhihui.love
                string macmd5 = GetMD5(GetMacAddress());
                string url = "http://www.lizhihui.love/xx/do.php?method=login&username=" + textBox1.Text.Trim() + "&password=" + textBox2.Text.Trim()+"&mac="+macmd5;
                string html = GetUrl(url, "utf-8");
                string msg = Regex.Match(html, @"""msg"":""([\s\S]*?)""").Groups[1].Value;
                if (msg == "登录成功")
                {
                    MessageBox.Show(msg.Trim());
                    百度贴吧ID采集 baidu=new 百度贴吧ID采集();
                    baidu.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show(msg.Trim());
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("登录失败");
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox3.Text=="")
            {
                MessageBox.Show("请输入账号");
                return;
            }
            if (textBox4.Text == "")
            {
                MessageBox.Show("请输入密码");
                return;
            }
            if (textBox5.Text == "")
            {
                MessageBox.Show("请输入激活码");
                return;
            }

            if (textBox3.Text.Length < 6)
            {
                MessageBox.Show("账号至少6位");
                return;
            }
            if (textBox4.Text.Length < 6)
            {
                MessageBox.Show("密码至少6位");
                return;
            }
            button2.Enabled = false;
            zhuce();
            button2.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入账号");
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("请输入密码");
                return;
            }
          

            if (textBox1.Text.Length < 6 )
            {
                MessageBox.Show("账号至少6位");
                return;
            }
            if (textBox2.Text.Length < 6)
            {
                MessageBox.Show("密码至少6位");
                return;
            }
            button1.Enabled = false;
            login();
            button1.Enabled = true;
        }

        private void 登录_Load(object sender, EventArgs e)
        {

        }
    }
}
