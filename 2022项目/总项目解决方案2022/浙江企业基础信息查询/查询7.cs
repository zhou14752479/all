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

namespace 浙江企业基础信息查询
{
    public partial class 查询7 : Form
    {
        public 查询7()
        {
            InitializeComponent();
        }

        private void 查询7_Load(object sender, EventArgs e)
        {

        }


        static string ua1 = "Mozilla/5.0 (iPhone; CPU iPhone OS 14_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.17(0x17001126) NetType/WIFI Language/zh_CN";

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "HWWAFSESID=cc9147f4aa41fc86ee; HWWAFSESTIME=1618565738420; route=0f1040e0778720d344b64fd91ee406cf; _monitor_sessionid=tCy7Ys6iRe1626459960928; _monitor_idx=5; JMOPENSESSIONID=1b9e1ff0-e9f4-4dc0-9a49-3720b58f83d9";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                                                                                  // request.Proxy = null;//防止代理抓包
                                                                                  //WebProxy proxy = new WebProxy(ip);
                                                                                  //request.Proxy = proxy;

                request.AllowAutoRedirect = true;
                request.UserAgent = ua1;
                request.Referer = "http://app.gjzwfw.gov.cn/jmopen/webapp/html5/rkkpcxxcxjtapp/index.html";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
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
                request.ContentType = "application/x-www-form-urlencoded";
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                //request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
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
        public static string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalMilliseconds);
            return a.ToString();
        }
        int total;
        public void run()
        {
            //string gregegedrgerheh = gdsgdgdgdgdstgfeewrwerw3r23r32rvxsvdsv.rgebgdgdvsdfsdvsdfsdvdsbgdsrt435b515sdfsdf("shebaocbd", "timestr");
            //string expiretime = gregegedrgerheh.Split(new string[] { "," }, StringSplitOptions.None)[2];

            //if (DateTime.Now > Convert.ToDateTime(expiretime))
            //{
            //    MessageBox.Show("{\"msg\":\"非法请求\"}");
            //    return;
            //}
            if (DateTime.Now > Convert.ToDateTime("2022-07-05"))
            {
                MessageBox.Show("{\"msg\":\"非法请求\"}");
                return;
            }
            try
            {
                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    total = total + 1;


                    DataRow dr = dt.Rows[a];
                    string uid = dr[0].ToString();
                    string timestr = GetTimeStamp();

                    string sign = method.GetMD5("shbxcbdcxapp"+timestr) ;
                    string zj_ggsjpt_sign = method.GetMD5("ada72850-2b2e-11e7-985b-008cfaeb3d74"+ "995e00df72f14bbcb7833a9ca063adef" + timestr);


                    string url = "http://app.gjzwfw.gov.cn/jimps/link.do?param=%7B%22from%22%3A%222%22%2C%22key%22%3A%22d38d675571cc4bcfbf7abd91391d78f5%22%2C%22requestTime%22%3A%22" + timestr + "%22%2C%22sign%22%3A%22" + sign + "%22%2C%22zj_ggsjpt_app_key%22%3A%22ada72850-2b2e-11e7-985b-008cfaeb3d74%22%2C%22zj_ggsjpt_sign%22%3A%22" + zj_ggsjpt_sign + "%22%2C%22zj_ggsjpt_time%22%3A%22" + timestr + "%22%2C%22aac147%22%3A%22" + uid + "%22%7D";
                
                    label2.Text = "正在查询：" + uid;
                
                    string html = GetUrl(url, "utf-8");
                  
                   string aab301 = Regex.Match(html, @"""aab301"":""([\s\S]*?)""").Groups[1].Value;
                    string name = Regex.Match(html, @"""name"":""([\s\S]*?)""").Groups[1].Value;
                    if (jiami == true)
                    {
                        uid = method.Base64Encode(Encoding.GetEncoding("utf-8"), uid);
                        aab301 = method.Base64Encode(Encoding.GetEncoding("utf-8"), aab301);
                        name = method.Base64Encode(Encoding.GetEncoding("utf-8"), name);
                       

                    }

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(uid);
                    lv1.SubItems.Add(aab301);
                    lv1.SubItems.Add(name);
                    if (radioButton1.Checked == true)
                    {
                        Thread.Sleep(500);
                    }
                    if (radioButton2.Checked == true)
                    {
                        Thread.Sleep(500);
                    }
                    Thread.Sleep(2000);
                    if (listView1.Items.Count > 2)
                    {
                        this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                    }


                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;

                }
                MessageBox.Show("完成");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        bool jiami = true;
        bool zanting = true;
        bool status = false;
        Thread thread;
        DataTable dt;

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先导入表格");
                return;
            }

            if (status == true)
            {
                status = false;
                label2.Text = "已停止";
            }
            else
            {
                status = true;
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            // openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;
                dt = method.ExcelToDataTable(textBox1.Text, true);

            }
        }

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

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "14752479")
            {
                MessageBox.Show("密码错误");
                return;
            }


            zanting = false;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                for (int j = 1; j < listView1.Columns.Count; j++)
                {
                    try
                    {

                        if (jiami == false)
                        {
                            if (j != 0)
                            {
                                listView1.Items[i].SubItems[j].Text = method.Base64Encode(Encoding.GetEncoding("utf-8"), listView1.Items[i].SubItems[j].Text);
                            }

                        }
                        else
                        {
                            if (j != 0)
                            {
                                listView1.Items[i].SubItems[j].Text = method.Base64Decode(Encoding.GetEncoding("utf-8"), listView1.Items[i].SubItems[j].Text);
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                        continue;
                    }
                }
            }


            zanting = true;

            if (jiami == false)
            {
                jiami = true;
            }
            else
            {
                jiami = false;
            }
        }
    }
}
