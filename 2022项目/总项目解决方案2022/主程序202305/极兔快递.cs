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
using myDLL;

namespace 主程序202305
{
    public partial class 极兔快递 : Form
    {
        public 极兔快递()
        {
            InitializeComponent();
        }
        Thread thread;

        bool zanting = true;
        bool status = true;
        string cookie = "";
        private void button3_Click(object sender, EventArgs e)
        {
            //string path = AppDomain.CurrentDomain.BaseDirectory;

            //try
            //{
            //    StreamReader sr = new StreamReader(path + "cookie.txt", method.EncodingType.GetTxtType(path + "cookie.txt"));
            //    //一次性读取完 
            //    string texts = sr.ReadToEnd();

            //    cookie = Regex.Match(texts, @"cookie=([\s\S]*?)&").Groups[1].Value;
            //    textBox2.Text = cookie;
            //    sr.Close();  //只关闭流
            //    sr.Dispose();   //销毁流内存

            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.ToString());
            //}




            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
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
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36";
                request.Referer = "https://jms.jtexpress.com.cn/";
                WebHeaderCollection headers = request.Headers;
                headers.Add("Authtoken:f801017874ae403c888fd8983b462a6d");
                headers.Add("Routename:trackingExpress");
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

        public  string PostUrlDefault(string url, string postData, string COOKIE)
        {
            string result;
            try
            {
                string charset = "utf-8";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;
                WebHeaderCollection headers = request.Headers;
                headers.Add("Authtoken:"+textBox2.Text.Trim());
                headers.Add("Routename:trackingExpress");
                //request.ContentType = "application/x-www-form-urlencoded";
                request.ContentType = "application/json";
                request.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
                request.Referer = "https://jms.jtexpress.com.cn/";
               
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                response.GetResponseHeader("Set-Cookie");
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
            catch (WebException ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        #region 订单信息
        public void run()
        {


            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string item in text)
            {

                string url = "https://jmsgw.jtexpress.com.cn/operatingplatform/order/getOrderDetail";
                string postdata = "{\"waybillNo\":\""+ item + "\",\"countryId\":\"1\"}";
                string html = PostUrlDefault(url,postdata,"");

               
                string orderSourceName = Regex.Match(html, @"""orderSourceName"":([\s\S]*?),").Groups[1].Value.Replace("\"","");

              
                string receiverName = Regex.Match(html, @"""receiverName"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                string receiverFullAddress = Regex.Match(html, @"""receiverFullAddress"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                string receiverMobilePhone = Regex.Match(html, @"""receiverMobilePhone"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                string packageChargeWeight = Regex.Match(html, @"""packageChargeWeight"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
               
                string packageLength = Regex.Match(html, @"""packageLength"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                
                string packageVolume = Regex.Match(html, @"""packageVolume"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                string paymentModeName = Regex.Match(html, @"""paymentModeName"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                string terminalDispatchCode = Regex.Match(html, @"""terminalDispatchCode"":""([\s\S]*?)""").Groups[1].Value.Replace("\"", "");


                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                try
                {

                    lv1.SubItems.Add(item);
                    lv1.SubItems.Add(orderSourceName);
                    lv1.SubItems.Add(receiverName);
                    lv1.SubItems.Add(receiverMobilePhone);
                    lv1.SubItems.Add(receiverFullAddress);
                    lv1.SubItems.Add(packageChargeWeight);

                    lv1.SubItems.Add("0*0*0cm³");

                    lv1.SubItems.Add("--");
                    lv1.SubItems.Add(paymentModeName);
                    lv1.SubItems.Add(terminalDispatchCode);

                }
                catch (Exception)
                {

                    lv1.SubItems.Add("");

                }
                Thread.Sleep(1000);
                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
                if (status == false)
                    return;

            }

        }

        #endregion
        private void button2_Click(object sender, EventArgs e)
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

        private void button6_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
           listView1.Items.Clear(); 
        }

        private void 极兔快递_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void 极兔快递_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                StreamReader sr = new StreamReader( "D:/token.txt", method.EncodingType.GetTxtType("D:/token.txt"));
                //一次性读取完 
                string texts = sr.ReadToEnd();

            
                textBox2.Text = texts.Trim();
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
}
