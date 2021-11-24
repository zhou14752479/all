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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 快递超市
{
    public partial class 改密码 : Form
    {
        public 改密码()
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
        string domail = "www.acaiji.com/shangxueba2";
        public void delete()
        {

            string url = "http://" + domail + "/shangxueba.php?method=del&username=kdzs";
            string html = GetUrl(url, "utf-8");

            //MessageBox.Show(html.Trim());


        }

        public void register()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入密码");
                return;
            }


            string html = GetUrl("http://" + domail + "/shangxueba.php?method=register&username=kdzs&password=" + textBox1.Text.Trim() + "&days=999&type=kdzs", "utf-8");
            if (html.Contains("成功"))
            {
                MessageBox.Show("密码已修改为：" + textBox1.Text.Trim());
                textBox1.Text = "";
            }
            else
            {
                MessageBox.Show(html.Trim());
            }



        }
        private void button1_Click(object sender, EventArgs e)
        {
            delete();
            register();
        }
    }
}
