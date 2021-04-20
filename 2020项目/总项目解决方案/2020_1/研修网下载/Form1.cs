using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;
using MySql.Data.MySqlClient;

namespace 研修网下载
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            webBrowser1.ScriptErrorsSuppressed = true;
        }









        #region 去掉路径中非法字符
        public string removeValid(string illegal)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                illegal = illegal.Replace(c.ToString(), "");
            }
            return illegal;
        }

        #endregion
        bool zanting = true;
        string cookie = "";
        string path = AppDomain.CurrentDomain.BaseDirectory;
        bool status = true;
        #region  主程序
        public void run()
        {

            try
            {
              


                for (int i = Convert.ToInt32(textBox1.Text); i <= Convert.ToInt32(textBox2.Text); i = i + 1)

                {

                    try
                    {



                        FileStream fs1 = new FileStream(path + "config.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1);
                        sw.WriteLine(i.ToString());
                        sw.Close();
                        fs1.Close();


                        string Url = "http://q.yanxiu.com/upload/viewResource.tc?resId=" + i;

                        string html = method.VerifyGet(Url,cookie, "yanxiu");  //定义的GetRul方法 返回 reader.ReadToEnd()
                        string downUrl = "http://q.yanxiu.com/uploadResource/DownloadServlet?type=res2&resId=" + i;

                        Match title = Regex.Match(html, @"<h1>([\s\S]*?)</h1>");
                        Match geshi = Regex.Match(html, @"格式：</dt>([\s\S]*?)</dd>");
                        Match daxiao = Regex.Match(html, @"<dd class=""w160"">([\s\S]*?)</dd>");

                        string gs = geshi.Groups[1].Value.Replace("<dd>", "").Trim();
                        string dx = daxiao.Groups[1].Value.Replace(".", "").Replace(" ", "").Trim().Replace("M", "00").Replace("K", "");
                        string bt = title.Groups[1].Value.Replace(".", "").Replace("doc", "").Replace("docx", "").Replace("ppt", "").Replace("pptx", "").Replace(" ", "").Trim();

                        if (dx != "" && dx != null)
                        {

                            if (Convert.ToInt32(dx) > 3)
                            {



                                //if (gs == "doc" || gs == "docx" || gs == "ppt" || gs == "pptx")
                                //{
                                if (geshiList.Contains(gs))
                                {

                                    method.downloadFile(downUrl, path + "下载文件\\", removeValid(bt) + "." + gs, cookie);

                                    textBox6.Text += DateTime.Now.ToString()+"  " + i + "成功：" + bt + "\r\n";
                                }

                                else
                                {
                                    textBox6.Text = DateTime.Now.ToString() + "格式不符合跳过下载" + "\r\n";
                                }


                            }
                            else
                            {
                                //textBox6.Text += DateTime.Now.ToString() + "格式不符合跳过下载" + "\r\n";
                            }

                        }
                        else
                        {
                            textBox6.Text += DateTime.Now.ToString() + "格式不符合跳过下载" + "\r\n";
                        }

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                        Thread.Sleep(13000);
                        if (status == false)
                        {
                            return;
                        }

                    }
                    catch
                    {

                        continue;
                    }

                }



            }
            catch (System.Exception ex)
            {
              textBox6.Text=  ex.ToString();
            }


        }




        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            
            webBrowser1.Url = new Uri("http://i.yanxiu.com/");
           

            foreach (Control ctr in groupBox1.Controls)
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
            }

            foreach (Control ctr in groupBox2.Controls)
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
            }

            if (File.Exists(path + "config.txt"))
            {

                StreamReader sr = new StreamReader(path  + "config.txt", Encoding.GetEncoding("utf-8"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                sr.Close();
                
                textBox1.Text = texts;

            }

        }
        ArrayList geshiList = new ArrayList();
        private void button2_Click(object sender, EventArgs e)
        {
          
            if (checkBox1.Checked == true)
            {
                geshiList.Add("doc");
            }
            if (checkBox2.Checked == true)
            {
                geshiList.Add("docx");
            }
            if (checkBox3.Checked == true)
            {
                geshiList.Add("ppt");
            }
            if (checkBox4.Checked == true)
            {
                geshiList.Add("pptx");
            }
            if (checkBox5.Checked == true)
            {
                geshiList.Add("pdf");
            }
            if (checkBox6.Checked == true)
            {
                geshiList.Add("txt");
            }
            if (checkBox7.Checked == true)
            {
                geshiList.Add("rar");
            }
            if (checkBox8.Checked == true)
            {
                geshiList.Add("zip");
            }

         

                 textBox6.Text += "开始下载";
                cookie = method.GetCookies("http://i.yanxiu.com/?j=true&fl=true");
          
                status = true;
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
                button2.Enabled = false;

            
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            button2.Enabled = true;
            zanting = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
          
            button2.Enabled = true;
            zanting = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出吗？", "退出询问"
           , MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;//告诉窗体关闭这个任务取消

            }
            else
            {
                foreach (Control ctr in groupBox1.Controls)
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
                }

                foreach (Control ctr in groupBox2.Controls)
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
                }

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            HtmlDocument dc = webBrowser1.Document;
            HtmlElementCollection es = dc.GetElementsByTagName("input");   //GetElementsByTagName返回集合
            foreach (HtmlElement e1 in es)
            {
                if (e1.GetAttribute("name") == "userName")
                {
                    e1.SetAttribute("value", textBox4.Text.Trim());
                }
                if (e1.GetAttribute("name") == "passWord")
                {
                    e1.SetAttribute("value", textBox5.Text.Trim());
                }
            }

            //点击登陆

            HtmlElementCollection es2 = dc.GetElementsByTagName("button");   //GetElementsByTagName返回集合
            foreach (HtmlElement e1 in es2)
            {
                if (e1.GetAttribute("id") == "submit")
                {
                    e1.InvokeMember("click");
                }

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            status = false;
            textBox6.Text += "已停止";
        }
    }
}
