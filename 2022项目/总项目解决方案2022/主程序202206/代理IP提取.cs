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

        int allcount = 0;
        public void run()
        {
            allcount = 0;


            bool json1 = false;
            bool json2 = false;
            bool json3= false;
            bool json4 = false;
            bool json5 = false;
           
            try
            {
                textBox2.Text = "";


                List<string> list = new List<string>();

                //string url = textBox1.Text.Trim();
                
                string user = "|"+textBox3.Text.Trim()+","+textBox4.Text.Trim();
               
                if(radioButton1.Checked==true)
                {
                  
                    list.Add(textBox1.Text.Trim());
                    user = "|" + textBox3.Text.Trim() + "," + textBox4.Text.Trim();
                    if (textBox3.Text.Trim() == "" || textBox4.Text.Trim() == "")
                    {
                        user = "";

                    }
                }

                if (radioButton2.Checked == true)
                {
                  
                    list.Add(textBox7.Text.Trim());
                    user = "|" + textBox6.Text.Trim() + "," + textBox5.Text.Trim();
                    if (textBox6.Text.Trim() == "" || textBox5.Text.Trim() == "")
                    {
                        user = "";

                    }
                }

                if (radioButton3.Checked == true)
                {
                   
                    list.Add(textBox10.Text.Trim());
                    user = "|" + textBox9.Text.Trim() + "," + textBox8.Text.Trim();
                    if (textBox9.Text.Trim() == "" || textBox8.Text.Trim() == "")
                    {
                        user = "";

                    }
                }
                if (radioButton4.Checked == true)
                {

                    list.Add(textBox26.Text.Trim());
                    user = "|" + textBox25.Text.Trim() + "," + textBox24.Text.Trim();
                    if (textBox25.Text.Trim() == "" || textBox24.Text.Trim() == "")
                    {
                        user = "";

                    }
                }
                if (radioButton5.Checked == true)
                {

                    list.Add(textBox29.Text.Trim());
                    user = "|" + textBox28.Text.Trim() + "," + textBox27.Text.Trim();
                    if (textBox28.Text.Trim() == "" || textBox27.Text.Trim() == "")
                    {
                        user = "";

                    }
                }

                if (radioButton6.Checked == true)
                {

                    list.Add(textBox61.Text.Trim());
                    user = "|" + textBox60.Text.Trim() + "," + textBox59.Text.Trim();
                    if (textBox60.Text.Trim() == "" || textBox59.Text.Trim() == "")
                    {
                        user = "";

                    }
                }

                if (radioButton7.Checked == true)
                {

                    list.Add(textBox65.Text.Trim());
                    user = "|" + textBox64.Text.Trim() + "," + textBox63.Text.Trim();
                    if (textBox64.Text.Trim() == "" || textBox63.Text.Trim() == "")
                    {
                        user = "";

                    }
                }











                if (radioButton8.Checked == true)
                {
                    list.Add(textBox11.Text.Trim());
                    json1 = true;
                }

                if (radioButton9.Checked == true)
                {
                    list.Add(textBox12.Text.Trim());
                    json2 = true;
                }

                if (radioButton10.Checked == true)
                {
                    list.Add(textBox41.Text.Trim());
                    json3 = true;
                }

                if (radioButton11.Checked == true)
                {
                    list.Add(textBox40.Text.Trim());
                    json4 = true;
                }

                if (radioButton12.Checked == true)
                {
                    list.Add(textBox47.Text.Trim());
                    json5 = true;
                }


                StringBuilder sb = new StringBuilder();

           


                for (int a = 0; a < list.Count; a++)
                {
                    string url=list[a].ToString();
                    for (int i = 0; i < numericUpDown1.Value; i++)
                    {
                        int count = 0;
                        string html = GetUrl(url, "utf-8");



                        if(json1==true)
                        {
                           
                            MatchCollection servers = Regex.Matches(html, "\""+textBox14.Text.Trim()+"\":\"([\\s\\S]*?)\"");
                            MatchCollection ports = Regex.Matches(html, "\"" + textBox15.Text.Trim() + "\":([\\s\\S]*?),");
                            MatchCollection users = Regex.Matches(html, "\"" + textBox16.Text.Trim() + "\":\"([\\s\\S]*?)\"");
                            MatchCollection pws = Regex.Matches(html, "\"" + textBox17.Text.Trim() + "\":\"([\\s\\S]*?)\"");

                            for (int j= 0;j< servers.Count;j++)
                            {
                                
                                    count++;
                                    allcount++;
                                    sb.Append(servers[j].Groups[1].Value+":"+ports[j].Groups[1].Value+ "|" + users[j].Groups[1].Value+","+pws[j].Groups[1].Value + "\r\n");
                            }
                        }
                       else if (json2 == true)
                        {

                            MatchCollection servers = Regex.Matches(html, "\"" + textBox19.Text.Trim() + "\":\"([\\s\\S]*?)\"");
                            MatchCollection ports = Regex.Matches(html, "\"" + textBox20.Text.Trim() + "\":([\\s\\S]*?),");
                            MatchCollection users = Regex.Matches(html, "\"" + textBox21.Text.Trim() + "\":\"([\\s\\S]*?)\"");
                            MatchCollection pws = Regex.Matches(html, "\"" + textBox22.Text.Trim() + "\":\"([\\s\\S]*?)\"");
                            for (int j = 0; j < servers.Count; j++)
                            {

                                count++;
                                allcount++;
                                sb.Append(servers[j].Groups[1].Value + ":" + ports[j].Groups[1].Value + "|" + users[j].Groups[1].Value + "," + pws[j].Groups[1].Value + "\r\n");
                            }
                        }


                        else if (json3== true)
                        {

                            MatchCollection servers = Regex.Matches(html, "\"" + textBox38.Text.Trim() + "\":\"([\\s\\S]*?)\"");
                            MatchCollection ports = Regex.Matches(html, "\"" + textBox37.Text.Trim() + "\":([\\s\\S]*?),");
                            MatchCollection users = Regex.Matches(html, "\"" + textBox36.Text.Trim() + "\":\"([\\s\\S]*?)\"");
                            MatchCollection pws = Regex.Matches(html, "\"" + textBox35.Text.Trim() + "\":\"([\\s\\S]*?)\"");
                            for (int j = 0; j < servers.Count; j++)
                            {

                                count++;
                                allcount++;
                                sb.Append(servers[j].Groups[1].Value + ":" + ports[j].Groups[1].Value + "|" + users[j].Groups[1].Value + "," + pws[j].Groups[1].Value + "\r\n");
                            }
                        }
                        else if (json4 == true)
                        {

                            MatchCollection servers = Regex.Matches(html, "\"" + textBox31.Text.Trim() + "\":\"([\\s\\S]*?)\"");
                            MatchCollection ports = Regex.Matches(html, "\"" + textBox32.Text.Trim() + "\":([\\s\\S]*?),");
                            MatchCollection users = Regex.Matches(html, "\"" + textBox33.Text.Trim() + "\":\"([\\s\\S]*?)\"");
                            MatchCollection pws = Regex.Matches(html, "\"" + textBox34.Text.Trim() + "\":\"([\\s\\S]*?)\"");
                            for (int j = 0; j < servers.Count; j++)
                            {

                                count++;
                                allcount++;
                                sb.Append(servers[j].Groups[1].Value + ":" + ports[j].Groups[1].Value + "|" + users[j].Groups[1].Value + "," + pws[j].Groups[1].Value + "\r\n");
                            }
                        }

                        else if (json5 == true)
                        {

                            MatchCollection servers = Regex.Matches(html, "\"" + textBox43.Text.Trim() + "\":\"([\\s\\S]*?)\"");
                            MatchCollection ports = Regex.Matches(html, "\"" + textBox44.Text.Trim() + "\":([\\s\\S]*?),");
                            MatchCollection users = Regex.Matches(html, "\"" + textBox45.Text.Trim() + "\":\"([\\s\\S]*?)\"");
                            MatchCollection pws = Regex.Matches(html, "\"" + textBox46.Text.Trim() + "\":\"([\\s\\S]*?)\"");
                            for (int j = 0; j < servers.Count; j++)
                            {

                                count++;
                                allcount++;
                                sb.Append(servers[j].Groups[1].Value + ":" + ports[j].Groups[1].Value + "|" + users[j].Groups[1].Value + "," + pws[j].Groups[1].Value + "\r\n");
                            }
                        }

                        else
                        {
                            string[] text = html.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                            foreach (var item in text)
                            {
                                if (item != "")
                                {

                                    count++;
                                    allcount++;

                                    sb.Append(item + user + "\r\n");



                                }

                            }
                        }
                        





                        label19.Text = "本次提取："+count+"共提取："+allcount;
                        Thread.Sleep(Convert.ToInt32(textBox23.Text));
                    }
                }

                textBox2.Text = sb.ToString();

                MessageBox.Show("完成");
                System.Windows.Forms.Clipboard.SetText(textBox2.Text); //复制
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
                        ctr.Text = texts.Trim();
                        sr.Close();
                    }
                }

                if (ctr is RadioButton)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                    if (File.Exists(path + ctr.Name + ".txt"))
                    {

                        StreamReader sr = new StreamReader(path + ctr.Name + ".txt", Encoding.GetEncoding("utf-8"));
                        //一次性读取完 
                        string texts = sr.ReadToEnd();
                        if(texts.Trim()== "True")
                        {
                            ((RadioButton)ctr).Checked = true;
                        }
                        
                        sr.Close();
                    }
                   
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {


            //if (thread == null || !thread.IsAlive)
            //{
            //    thread = new Thread(run);
            //    thread.Start();
            //    Control.CheckForIllegalCrossThreadCalls = false;
            //}
            run();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            allcount = 0;
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
                    if(ctr is RadioButton)
                    {
                        
                        string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                        FileStream fs1 = new FileStream(path + ctr.Name + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1);
                        sw.WriteLine(((RadioButton)ctr).Checked.ToString());
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
            System.Windows.Forms.Clipboard.SetText(textBox2.Text); //复制

           

        }

    }
}
