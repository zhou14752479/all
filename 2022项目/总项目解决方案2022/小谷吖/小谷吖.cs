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
    public partial class 小谷吖 : Form
    {
        public 小谷吖()
        {
            InitializeComponent();
        }

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

        Thread thread;

        bool zanting = true;
        bool status = true;



        #region POST默认请求
        public  string PostUrlDefault(string url, string postData, string COOKIE)
        {
            string result;
            try
            {
                string charset = "utf-8";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";

                request.ContentType = "application/x-www-form-urlencoded";
                // request.ContentType = "application/json";
                WebHeaderCollection headers = request.Headers;
                headers.Add("Authorization:" + textBox2.Text.Trim());
               // headers.Add("Authorization: bearer 65aae660-f70e-4a19-b4c8-0e611bb57c19");
               // headers.Add("Authorization: bearer f164ff17-5fb2-4664-9c9a-2afadd22ac3b");
                request.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36 MicroMessenger/7.0.20.1781(0x6700143B) NetType/WIFI MiniProgramEnv/Windows WindowsWechat/WMPF XWEB/6939";
                request.Headers.Add("Cookie", COOKIE);
                request.Referer = "https://servicewechat.com/wx3d65f6c97795bc65/208/page-frame.html";
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
        #region 主程序
        public void run()
        {



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

                    label1.Text = "正在查询："+text[a];
                    if (text[a].Trim() == "")
                        continue;

                    string url = "https://api.xiaoguya.com:9898/recycle/api/recycle/order/scan/"+text[a].Trim();

                    string html = PostUrlDefault(url, "{}", "");

                    string price = Regex.Match(html, @"""amount"":([\s\S]*?),").Groups[1].Value;
                    string id = Regex.Match(html, @"""detailVos"":([\s\S]*?)""id"":([\s\S]*?),").Groups[2].Value;
                    string msg = Regex.Match(html, @"""msg"":""([\s\S]*?)""").Groups[1].Value;
                   // MessageBox.Show(html);
                    delete(id);
                    if (price=="")
                    {
                        price = msg;
                    }

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                    lv1.SubItems.Add(text[a]);
                    lv1.SubItems.Add(price);


                    if (listView1.Items.Count > 2)
                    {
                        this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                    }
                    Thread.Sleep(1000); 
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
        public void delete(string id)
        {

                try
                {
                    string url = "https://api.xiaoguya.com:9898/recycle/api/recycle/order/removeBookByDetailId/" + id;

                    string html = PostUrlDefault(url, "{}", "");

                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);


                }

        }
        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void 小谷吖_FormClosing(object sender, FormClosingEventArgs e)
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

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
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
            try
            {
                StreamReader sr = new StreamReader("D:/token.txt", method.EncodingType.GetTxtType("D:/token.txt"));
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
