using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using myDLL;
using System.Threading;
using System.Text.RegularExpressions;
using System.Net;
using System.IO.Compression;

namespace 主程序1225
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region POST请求全参
        public  string PostUrl(string url, string postData, string ip)
        {
            string result;
            try
            {
                ServicePointManager.Expect100Continue = false;
                string charset = "utf-8";
               // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
              
            

                request.Method = "Post";
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("Proxy-Authorization:Basic MTA2MDg0NzU4MDM2MTQxMjYwOCUzQXBnOXZwUXBL");
                WebProxy proxy = new WebProxy(ip);
                request.Proxy = proxy;
                //添加头部
                request.ContentType = "application/json";
                request.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
                //request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = true;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36";
                request.Headers.Add("Cookie", "");
                request.Referer = "";
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
                //result = ex.ToString();
                //400错误也返回内容
                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }
        #endregion

        //webBrowser1.Document.InvokeScript("dodo", new object[] { "91340104MA8PDWDU0U" }).ToString();

        private void Form1_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            // webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate(AppDomain.CurrentDomain.BaseDirectory + "index.html");
        }

       
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if(DateTime.Now>Convert.ToDateTime("2024-05-01"))
            {
                return;
            }

            if(textBox2.Text=="")
            {
                MessageBox.Show("请导入表格");
                return;
            }
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            //run();
        }
        bool zanting = true;
        bool status = true;
        public void run()
        {
            try
            {
                string code = Regex.Match(comboBox1.Text, @"\d{2}").Groups[0].Value.Trim()+"0000";

               
                DataTable dt = method.ExcelToDataTable(textBox2.Text, false);

                //MessageBox.Show(dt.Rows.Count.ToString());
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    DataRow dr = dt.Rows[i];
                    string company = dr[0].ToString();
                    string ens = "";
                    this.webBrowser1.Invoke((MethodInvoker)delegate ()
                    {
                        ens = webBrowser1.Document.InvokeScript("dodo", new object[] { company }).ToString();
                    });

                    string ip = "http://http-dynamic-S04.xiaoxiangdaili.com:10030";

                    string aurl = "https://etax.gansu.chinatax.gov.cn/login-web/api/auth/kqsyh/employees/get";
                    string postdata = "{\"xzqh\":\""+code+"\",\"nsrsbh\":\"" + ens + "\"}";
                    string ahtml = PostUrl(aurl, postdata,ip);
                    label3.Text = "";
                    label1.Text = "正在读取：" + company;
                    if (ahtml.Contains("快"))
                    {
                        
                        label3.Text = ahtml;
                    }
                    MatchCollection xms = Regex.Matches(ahtml, @"""xm"":""([\s\S]*?)""");
                    MatchCollection sfzjhms = Regex.Matches(ahtml, @"""sfzjhm"":""([\s\S]*?)""");
                    MatchCollection mobiles = Regex.Matches(ahtml, @"""mobile"":""([\s\S]*?)""");


                    for (int a = 0; a < xms.Count; a++)
                    {
                        try
                        {
                           
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(xms[a].Groups[1].Value);
                            lv1.SubItems.Add(sfzjhms[a].Groups[1].Value);
                            lv1.SubItems.Add(mobiles[a].Groups[1].Value);
                            lv1.SubItems.Add(company);

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
                        catch (Exception)
                        {

                            continue;
                        }
                    }



                    Thread.Sleep(Convert.ToInt32(textBox1.Text)*1000);

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            //textBox2.Text= webBrowser1.Document.InvokeScript("dodo", new object[] { "91340104MA8PDWDU0U" }).ToString();
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

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            //  openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";

            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox2.Text = openFileDialog1.FileName;

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
