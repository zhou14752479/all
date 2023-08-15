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

namespace 小谷吖
{
    public partial class 骑马收书 : Form
    {
        public 骑马收书()
        {
            InitializeComponent();
        }

        #region POST默认请求
        public string PostUrlDefault(string url, string postData, string COOKIE)
        {
            string result;
            try
            {
                string charset = "utf-8";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";

                request.ContentType = "application/json";
                // request.ContentType = "application/json";
                WebHeaderCollection headers = request.Headers;
                headers.Add("token:eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpYXQiOjE2ODY0NzM5MDgsIm5iZiI6MTY4NjQ3MzkwOCwiZXhwIjoxNjg2NzMzMTA4LCJkYXRhIjp7InVzZXJpZCI6NTcyNTcwLCJ1c2VybmFtZSI6InJ0NTcyNTcwMTY4NjQ3MzkwNyJ9fQ.PeoE-BVUysYmld8Nb_A-jEWTPlwML35JrvwYMY1bLWQ");
                headers.Add("uid: 572570");
              
                request.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 16_6 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/8.0.40(0x1800282c) NetType/WIFI Language/zh_CN";
                request.Referer = "https://servicewechat.com/wx4cb9324a47db434b/105/page-frame.html";
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
                // result = ex.ToString();


                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }
        #endregion
        Thread thread;

        bool zanting = true;
        bool status = true;



        #region 主程序
        public void run()
        {
            int count = 0;


            string filename = textBox1.Text.Trim();
            StreamReader sr = new StreamReader(filename, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
            for (int a = 0; a < text.Length; a++)
            {

                try
                {

                    label1.Text = "正在查询：" + text[a];
                    if (text[a].Trim() == "")
                        continue;

                    string url = "https://www.qimar.cn/one_scan";

                 PostUrlDefault(url, "{\"isbn\":\""+ text[a].Trim() + "\"}", "");

                    Thread.Sleep(1000);
                    count++;
                    if (count > 10 || a == text.Length - 1)
                    {

                        string html = PostUrlDefault("https://www.qimar.cn/find_carts", "{\"page\":1,\"limit\":10}", "");
                        MatchCollection prices = Regex.Matches(html, @"""recycle"":""([\s\S]*?)""");
                        MatchCollection bar_codes = Regex.Matches(html, @"""bar_code"":""([\s\S]*?)""");
                        for (int i = 0; i < prices.Count; i++)
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                            lv1.SubItems.Add(bar_codes[i].Groups[1].Value);
                            lv1.SubItems.Add(prices[i].Groups[1].Value);


                            if (listView1.Items.Count > 2)
                            {
                                this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                            }
                        }
                        count = 0;
                        delete();
                    }


                }

                     
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);


                }

                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
                if (status == false)
                    return;
            }




        }
        #endregion


        #region 删除
        public void delete()
        {

            try
            {
                string url = "https://www.qimar.cn/del_carts";

                string html = PostUrlDefault(url, "{}", "");

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);


            }

        }
        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测


            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"bQmj"))
            {

                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion
            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入ISBN文本");
                return;
            }
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
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

        private void 骑马收书_FormClosing(object sender, FormClosingEventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}
