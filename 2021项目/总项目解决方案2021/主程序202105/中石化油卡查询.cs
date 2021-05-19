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

namespace 主程序202105
{
    public partial class 中石化油卡查询 : Form
    {
        public 中石化油卡查询()
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
        public static string PostUrl(string url, string postData, string COOKIE)
        {
            try
            {
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
               // request.ContentType = "application/x-www-form-urlencoded";

                request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = true;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_6_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/8.0.4(0x1800042c) NetType/4G Language/zh_CN";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding("utf-8"));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
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

        Thread thread;
        bool zanting = true;
        bool status = true;
        string cookie = "";
        string codEpay = "JF-EPAY2017061903141";
        public void run()
        {
            try
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("请导入账号");
                    return;
                }

                StreamReader sr = new StreamReader(textBox2.Text, method.EncodingType.GetTxtType(textBox2.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    label1.Text = "正在查询："+text[i];
                    if (text[i] != "")
                    {
                       // string url = "https://enjoy.abchina.com/jf-pcweb/transPay/getPayInfo ";
                        string url = "https://enjoy.abchina.com/jf-openweb/transPay/getPayInfo";
                       // string postdata = "{\"host\":\"https://enjoy.abchina.com/\",\"codEpay\":\""+codEpay+"\",\"userInput\":{\"input1\":\"" + text[i] + "\"}}";
                        string postdata = "{\"codEpay\":\""+codEpay+"\",\"userInput\":{\"input1\":\""+text[i]+"\"}}";
                        
                        string html = PostUrl(url, postdata, cookie);
                        if (html.Contains("过期"))
                        {
                            MessageBox.Show("登录已过期，请重新点击登录");
                            i = i - 1;
                            zanting = false;
                            continue;
                        }
                        string value = Regex.Match(html, @"""sVal"":""([\s\S]*?)""").Groups[1].Value;
                     
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(text[i]);
                        lv1.SubItems.Add(value);

                        Thread.Sleep(1000);
                       
                        if (status == false)
                            return;
                    }



                }
                label1.Text=("查询结束");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "txt|*.txt";
            sfd.Title = "txt文件导出";


            if (sfd.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = sfd.FileName;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://www.acaiji.com:8080/api/vip.html", "utf-8");

            if (!html.Contains(@"Eq7ZF"))
            {

                return;
            }

            #endregion

       
            if (radioButton1.Checked==true)
            {
                codEpay = "JF-EPAY2017061903141";

            }
            if (radioButton2.Checked == true)
            {
                codEpay = "JF-EPAY2016082306659";

            }

            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }


            //System.Diagnostics.Process[] myProcesses = System.Diagnostics.Process.GetProcesses();

            //foreach (System.Diagnostics.Process myProcess in myProcesses)
            //{
            //    if ("WeChatApp" == myProcess.ProcessName)
            //        myProcess.Kill();//强制关闭该程序
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start(@"C:\Users\zhou\Desktop\农行微缴费.lnk");
           

            status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 中石化油卡查询_Load(object sender, EventArgs e)
        {

        }
        public void getcode()
        {
            method.GetUrl("http://47.102.145.207/index.php?codenum=1","utf-8");
            string codehtml = method.GetUrl("http://47.102.145.207/getcode.txt", "utf-8");
           
            while (!codehtml.Contains("mobile_id"))
            {
                codehtml = method.GetUrl("http://47.102.145.207/getcode.txt", "utf-8");
                
            }
          
            cookie=codehtml;
            if (cookie != "")
            {
                MessageBox.Show("登录成功");
                zanting = true;
            }
           
        }
        private void button7_Click(object sender, EventArgs e)
        {
           
                Thread thread = new Thread(getcode);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            
        }
    }
}
