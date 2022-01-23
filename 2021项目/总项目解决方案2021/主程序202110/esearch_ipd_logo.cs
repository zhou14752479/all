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

namespace 主程序202110
{
    public partial class esearch_ipd_logo : Form
    {
        public esearch_ipd_logo()
        {
            InitializeComponent();
        }

        #region POST默认请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public string PostUrlDefault(string url, string postData, string COOKIE)
        {
            try
            {

                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;//防止代理抓包
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("X-XSRF-TOKEN: "+ XSRFTOKEN);
     
                request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.93 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = url;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

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
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion
        string path = AppDomain.CurrentDomain.BaseDirectory+"//logo//";
        Thread thread;
        bool zanting = true;
        bool status = true;
        string cookie = "";
        string XSRFTOKEN="";
        method md = new method();
        public void run()
        {
           
            try
            {

                StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
                string text = sr.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i <array.Length; i++)
                {
                    try
                    {
                        if (array[i] == "")
                        {
                            continue;
                        }

                        string url = "https://esearch.ipd.gov.hk/nis-pos-view/tm/search/?page=1&rows=10";
                        string postdata = "{\"searchMethod\":\"TM_SEARCHMETHOD_WILDCARD\",\"filingDate\":{},\"documentFilingDate\":{},\"applicationNumber\":[\"" + array[i].Trim() + "\"],\"registrationDate\":{},\"isDeadRecordIndicator\":\"false\",\"expirationDate\":{},\"publicationDateOfAcceptance\":{},\"actualRegistrationDate\":{}}";

                        string html = PostUrlDefault(url, postdata, cookie);

                        string picbase64 = Regex.Match(html, @"thumbnail"":\[""([\s\S]*?)""").Groups[1].Value;
                        Thread.Sleep(500);
                        md.Base64ToImage(picbase64, path + array[i] + ".jpg");
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(array[i]);
                        if (picbase64 != "")
                        {
                            lv1.SubItems.Add("下载成功");

                        }
                        else
                        {
                            lv1.SubItems.Add("下载失败");
                        }



                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                    }
                    catch (Exception ex) 
                    {

                        MessageBox.Show("请重新验证非机器人");
                        Thread.Sleep(30000);
                        continue;
                    }




                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }
        private void esearch_ipd_logo_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
               textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="")
            {
                MessageBox.Show("请选择文件");
                return;
            }
            status = true;

            
                StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\tool\\cookie.txt", method.EncodingType.GetTxtType(AppDomain.CurrentDomain.BaseDirectory + "\\tool\\cookie.txt"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                cookie = texts.Trim();
                XSRFTOKEN = Regex.Match(cookie, @"XSRF-TOKEN=([\s\S]*?);").Groups[1].Value;

                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
           




            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            status = false;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\tool\\CEF主程序.exe");
        }
    }
}
