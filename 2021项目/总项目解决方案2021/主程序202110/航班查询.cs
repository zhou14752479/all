using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202110
{
    public partial class 航班查询 : Form
    {
        public 航班查询()
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
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
               
             
                request.ContentType = "application/json";
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "https://web.duanmatong.cn/";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

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
        /// <summary>
        /// 获取时间戳毫秒
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalMilliseconds);
            return a.ToString();
        }

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

        public void getdata()
        {
            try
            {
                string timestamp = GetTimeStamp();
                string sign = GetMD5(timestamp+textBox1.Text.Trim());
                string url = "http://14.152.95.93:8888/open-api/seat-ratio";
                string postdata = "{\"partnerCode\":\""+ textBox1.Text.Trim() + "\",\"timestamp\":\""+timestamp+"\",\"sign\":\""+sign+"\",\"param\":{\"fromCity\":\""+ textBox2.Text.Trim() + "\",\"toCity\":\""+ textBox3.Text.Trim() + "\",\"flightDate\":\""+dateTimePicker1.Value.ToString("yyyy-MM-dd")+"\",\"flightNo\":\""+ textBox4.Text.Trim() + "\"}}";
                string html = PostUrl(url,postdata,"","utf-8");
                string shangzuolv = Regex.Match(html,@"""data"":([\s\S]*?),").Groups[1].Value;
                shangzuolvtxt.Text = shangzuolv;


                string aurl = "http://14.152.95.93:8888/open-api/query-balance";
                string apostdata = "{\"partnerCode\":\"" + textBox1.Text.Trim() + "\",\"timestamp\":\"" + timestamp + "\",\"sign\":\"" + sign + "\"}";
                string ahtml = PostUrl(aurl, apostdata, "", "utf-8");
                string yue = Regex.Match(ahtml, @"""data"":([\s\S]*?),").Groups[1].Value;
                yuetxt.Text = (Convert.ToDouble(yue) * 10).ToString();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        public void getdata2()
        {
            try
            {
                string timestamp = GetTimeStamp();
                string sign = GetMD5(timestamp + textBox1.Text.Trim());
                string url = "http://14.152.95.93:8888/open-api/seat-ratio";
                string postdata = "{\"partnerCode\":\"" + textBox1.Text.Trim() + "\",\"timestamp\":\"" + timestamp + "\",\"sign\":\"" + sign + "\",\"param\":{\"fromCity\":\"" + textBox5.Text.Trim() + "\",\"toCity\":\"" + textBox3.Text.Trim() + "\",\"flightDate\":\"" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "\",\"flightNo\":\"" + textBox4.Text.Trim() + "\"}}";
                string html = PostUrl(url, postdata, "", "utf-8");
                string shangzuolv = Regex.Match(html, @"""data"":([\s\S]*?),").Groups[1].Value;
                shangzuolvtxt.Text = shangzuolv;


                string aurl = "http://14.152.95.93:8888/open-api/query-balance";
                string apostdata = "{\"partnerCode\":\"" + textBox1.Text.Trim() + "\",\"timestamp\":\"" + timestamp + "\",\"sign\":\"" + sign + "\"}";
                string ahtml = PostUrl(aurl, apostdata, "", "utf-8");
                string yue = Regex.Match(ahtml, @"""data"":([\s\S]*?),").Groups[1].Value;
                yuetxt.Text = (Convert.ToDouble(yue) * 10).ToString();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void 航班查询_Load(object sender, EventArgs e)
        {

        }


        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getdata);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
           
        }

        private void 航班查询_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getdata2);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
