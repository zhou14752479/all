using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

namespace 主程序202206
{
    public partial class 代理IP提取 : Form
    {
        public 代理IP提取()
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
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
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

        public void run()
        {
            try
            {
                textBox2.Text = "";


                List<string> list = new List<string>();

                //string url = textBox1.Text.Trim();
                
                string user = "|"+textBox3.Text.Trim()+","+textBox4.Text.Trim();
               
                if(checkBox1.Checked==true)
                {
                    //url = textBox1.Text.Trim();
                    list.Add(textBox1.Text.Trim());
                    user = "|" + textBox3.Text.Trim() + "," + textBox4.Text.Trim();
                    if (textBox3.Text.Trim() == "" || textBox4.Text.Trim() == "")
                    {
                        user = "";

                    }
                }

                if (checkBox2.Checked == true)
                {
                    //url = textBox7.Text.Trim();
                    list.Add(textBox7.Text.Trim());
                    user = "|" + textBox6.Text.Trim() + "," + textBox5.Text.Trim();
                    if (textBox6.Text.Trim() == "" || textBox5.Text.Trim() == "")
                    {
                        user = "";

                    }
                }

                if (checkBox3.Checked == true)
                {
                    //url = textBox10.Text.Trim();
                    list.Add(textBox10.Text.Trim());
                    user = "|" + textBox9.Text.Trim() + "," + textBox8.Text.Trim();
                    if (textBox9.Text.Trim() == "" || textBox8.Text.Trim() == "")
                    {
                        user = "";

                    }
                }



                StringBuilder sb = new StringBuilder();

                for (int a = 0; a < list.Count; a++)
                {
                    string url=list[a].ToString();
                    for (int i = 0; i < numericUpDown1.Value; i++)
                    {

                        string html = GetUrl(url, "utf-8");
                        string[] text = html.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        foreach (var item in text)
                        {
                            if (item != "")
                            {

                                sb.Append(item + user + "\r\n");


                            }

                        }

                        Thread.Sleep(1000);
                    }
                }
                textBox2.Text = sb.ToString();
                MessageBox.Show("完成");
                //System.Windows.Forms.Clipboard.SetText(textBox2.Text); //复制
                //textBox2.Text = html;

            }
            catch (Exception ex)
            {

               textBox2.Text=ex.ToString();
            }
        }

        Thread thread;

        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }
        #endregion


        private void 代理IP提取_Load(object sender, EventArgs e)
        {
            #region 通用检测


            if (!GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"VuVb"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion

            Control.CheckForIllegalCrossThreadCalls = false;
            foreach (Control ctr in panel1.Controls)
            {

                if (ctr is TextBox)
                {

                    string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                    if (File.Exists(path + ctr.Name + ".txt"))
                    {

                        StreamReader sr = new StreamReader(path + ctr.Name + ".txt", Encoding.GetEncoding("utf-8"));
                        //一次性读取完 
                        string texts = sr.ReadToEnd();
                        ctr.Text = texts;
                        sr.Close();
                    }
                }

                if (ctr is CheckBox)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                    if (File.Exists(path + ctr.Name + ".txt"))
                    {

                        StreamReader sr = new StreamReader(path + ctr.Name + ".txt", Encoding.GetEncoding("utf-8"));
                        //一次性读取完 
                        string texts = sr.ReadToEnd();
                        if(texts.Trim()== "True")
                        {
                            ((CheckBox)ctr).Checked = true;
                        }
                        
                        sr.Close();
                    }
                   
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(checkBox1.Checked==false &&checkBox2.Checked==false &&checkBox3.Checked==false)
            {
                MessageBox.Show("请选择一个链接");
                return;
            }


            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            //run();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

       
        private void 代理IP提取_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {

                foreach (Control ctr in panel1.Controls)
                {
                    if (ctr is TextBox)
                    {


                        string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                        FileStream fs1 = new FileStream(path + ctr.Name + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1);
                        sw.WriteLine(ctr.Text);
                        sw.Close();
                        fs1.Close();

                    }
                    if(ctr is CheckBox)
                    {
                        
                        string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                        FileStream fs1 = new FileStream(path + ctr.Name + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1);
                        sw.WriteLine(((CheckBox)ctr).Checked.ToString());
                        sw.Close();
                        fs1.Close();
                    }
                }




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
            //System.Windows.Forms.Clipboard.SetText(textBox2.Text); //复制

           

        }

    }
}
