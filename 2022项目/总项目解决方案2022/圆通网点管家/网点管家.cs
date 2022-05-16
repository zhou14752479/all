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

namespace 圆通网点管家
{
    public partial class 网点管家 : Form
    {
        public 网点管家()
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("jwt-token:eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiIsInppcCI6IkdaSVAifQ.H4sIAAAAAAAAAI2SwU7bMBzG38VXCrKTJk1z2zo0QBNDUGmHeQe3-ZN6OHZmO6yo6oG9wS4TEkW7TjtM48akTXuZtrwGadKmIvTALf_f9332Z8cjBMMUhcT3nBZpethtIJP1UIhGFKVanfMI9CFLgKKQondR_LGrzkAeMW3gaClT1KDIqEz3l65Xrw8KlhnQcpXFpO21sduulI6KNihKx5XQ8lqE4AJbkEza_ajgBRFMxutJKwEmH99_yIco08xyJfO5GeAcsMwOlOaWrz2QpEJdACyXfNRhpVXnnt_8mE9upn-_ryp2L9JSeXn84rCzt8JVYPbn__Rucj-5nP67nt3ezu6-zL_9nv26uv_5tfD2QCgZv9181krctEslPnsvwYx9o2Iuu3yZcLDjbGNvm_hd4oUOCZvujovdLRyEuKzQByHSgZKlnwSe7wdtP2gG5f0MbdV7RClFXB4Dj8ziMzxlwkCjpCcDpqFGF_9iX56qApfpJK55zsFYHteTfSZEDSWqx0Xdl4Ax7En6U7ZYNH8Uj3H--pW2T5pbyF9zWudSdUQGu8N0Y42TgfpcEyyP1mQ8pmiMxg-g8P-3cgMAAA.tbNGASL3pjO2U1Xnqqur3TvTlDjdYL8freDADIKEaFBeYXNprnnGnevQEesfjeV6Zd38IGjU-XSCk6I9xKiGZg");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
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


        #region POST默认请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrlDefault(string url, string postData, string COOKIE)
        {
            try
            {

                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;//防止代理抓包
                                     //添加头部
                                     //WebHeaderCollection headers = request.Headers;
                                     //headers.Add("sec-fetch-mode:navigate");
                                     //headers.Add("sec-fetch-site:same-origin");
                                     //headers.Add("sec-fetch-user:?1");
                                     //headers.Add("upgrade-insecure-requests: 1");
                                     //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("jwt-token:eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiIsInppcCI6IkdaSVAifQ.H4sIAAAAAAAAAI2SwU7bMBzG38VXCrKTJk1z2zo0QBNDUGmHeQe3-ZN6OHZmO6yo6oG9wS4TEkW7TjtM48akTXuZtrwGadKmIvTALf_f9332Z8cjBMMUhcT3nBZpethtIJP1UIhGFKVanfMI9CFLgKKQondR_LGrzkAeMW3gaClT1KDIqEz3l65Xrw8KlhnQcpXFpO21sduulI6KNihKx5XQ8lqE4AJbkEza_ajgBRFMxutJKwEmH99_yIco08xyJfO5GeAcsMwOlOaWrz2QpEJdACyXfNRhpVXnnt_8mE9upn-_ryp2L9JSeXn84rCzt8JVYPbn__Rucj-5nP67nt3ezu6-zL_9nv26uv_5tfD2QCgZv9181krctEslPnsvwYx9o2Iuu3yZcLDjbGNvm_hd4oUOCZvujovdLRyEuKzQByHSgZKlnwSe7wdtP2gG5f0MbdV7RClFXB4Dj8ziMzxlwkCjpCcDpqFGF_9iX56qApfpJK55zsFYHteTfSZEDSWqx0Xdl4Ax7En6U7ZYNH8Uj3H--pW2T5pbyF9zWudSdUQGu8N0Y42TgfpcEyyP1mQ8pmiMxg-g8P-3cgMAAA.tbNGASL3pjO2U1Xnqqur3TvTlDjdYL8freDADIKEaFBeYXNprnnGnevQEesfjeV6Zd38IGjU-XSCk6I9xKiGZg");
                request.Headers.Add("Cookie", COOKIE);
                //request.ContentType = "application/x-www-form-urlencoded";
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
              
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
        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                textBox1.Text = openFileDialog1.FileName;

            }
        }

        bool status = true;
        bool zanting = true;
        Thread thread;


        public void run()
        {
            label1.Text = "正在查询";
            try
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, method.EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] == "")
                        continue;
                    if (status == false)
                        return;
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    label1.Text = "正在查询："+text[i];
                    string url = "http://track.yto.net.cn/webapi/compre/trackV2?token=4fa7acc7-28a0-402b-956d-5fb75e53d719";
                    string postdata = "{\"display\":0,\"weightFlag\":1,\"waybillNos\":[\""+text[i]+"\"],\"exceptionTrace\":0,\"issueOrg\":0}";
                    
                    string html = PostUrlDefault(url,postdata,"");
                    //MatchCollection opOrgName = Regex.Matches(html, @"""opOrgName"":""([\s\S]*?)""");
                    //MessageBox.Show(html);
                    MatchCollection opTime = Regex.Matches(html, @"""opTime"":""([\s\S]*?)""");
                    MatchCollection opEmpName = Regex.Matches(html, @"""opEmpName"":""([\s\S]*?)""");

                    string receiverAdrress = Regex.Match(html, @"""receiverAdrress"":""([\s\S]*?)""").Groups[1].Value;

                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                    lv1.SubItems.Add(text[i]);
                    lv1.SubItems.Add(opTime[opTime.Count-1].Groups[1].Value.Replace(".000+08:00", "").Replace("T", " "));
                    lv1.SubItems.Add(opEmpName[opEmpName.Count - 1].Groups[1].Value);
                    lv1.SubItems.Add(receiverAdrress);


                    string html3 = GetUrl("http://track.yto.net.cn/webapi/compre/threeSegment?waybillNo="+text[i]+"&token=4fa7acc7-28a0-402b-956d-5fb75e53d719", "utf-8");
                    string searchThreeCode = Regex.Match(html3, @"searchThreeCode"":""([\s\S]*?)""").Groups[1].Value;

                    if (listView1.Items.Count > 2)
                    {
                        this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                    }

                    lv1.SubItems.Add(searchThreeCode);
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }



        }
        private void button1_Click(object sender, EventArgs e)
        {
            status = true;

            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
