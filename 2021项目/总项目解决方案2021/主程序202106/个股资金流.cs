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

namespace 主程序202106
{
    public partial class 个股资金流 : Form
    {
        public 个股资金流()
        {
            InitializeComponent();
        }
        Thread thread;
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

        #region  txtbox导出文本TXT
        public static void ListviewToTxt(string txt)
        {

                
            string path = AppDomain.CurrentDomain.BaseDirectory+DateTime.Now.ToString("yyyyMMdd") + ".txt";
          
            System.IO.File.WriteAllText(path,txt, Encoding.UTF8);
            MessageBox.Show("文件导出成功!文件地址:" + path);


        }






        #endregion

        public void run()
        {
            try
            {


                string url = "http://push2.eastmoney.com/api/qt/clist/get?cb=jQuery11230018960774597807317_1622339669735&fid=f62&po=1&pz=5000&pn=1&np=1&fltt=2&invt=2&ut=b2884a393a59ad64002292a3e90d46a5&fs=m%3A0%2Bt%3A6%2Bf%3A!2%2Cm%3A0%2Bt%3A13%2Bf%3A!2%2Cm%3A0%2Bt%3A80%2Bf%3A!2%2Cm%3A1%2Bt%3A2%2Bf%3A!2%2Cm%3A1%2Bt%3A23%2Bf%3A!2%2Cm%3A0%2Bt%3A7%2Bf%3A!2%2Cm%3A1%2Bt%3A3%2Bf%3A!2&fields=f12%2Cf62";

                string html = GetUrl(url,"utf-8");

                MatchCollection codes= Regex.Matches(html, @"""f12"":""([\s\S]*?)""");
                MatchCollection values = Regex.Matches(html, @"""f62"":([\s\S]*?)}");
               
                for (int i = 0; i < codes.Count; i++)
                {

                    string shsz = "0";

                    if (codes[i].Groups[1].Value.Trim().Substring(0,1) == "6")
                    {
                        shsz = "1";
                    }
                    string value = values[i].Groups[1].Value.Trim().Replace("\"","").Replace(".0", "");
                    textBox1.Text += ""+shsz+"|" + codes[i].Groups[1].Value.Trim() + "|" + DateTime.Now.ToString("yyyyMMdd") + "|" +value + "\r\n";
                    label1.Text = "已获取："+i.ToString();
                }
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ListviewToTxt(textBox1.Text);
        }

        private void 个股资金流_Load(object sender, EventArgs e)
        {

        }
    }
}
