using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 通用文件下载
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string COOKIE = "";
        public string referUrl = "";
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public string GetUrl(string Url)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
               
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = referUrl;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
               
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
    
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(textBox8.Text.Trim())); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();


                reader.Close();
                response.Close();
                return content;



            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion


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
        #region 下载文件  【好用】
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="URLAddress">图片地址</param>
        /// <param name="subPath">图片所在文件夹</param>
        /// <param name="name">图片名称</param>
        public void downloadFile(string URLAddress, string name)
        {
            try
            {
                string path = System.IO.Directory.GetCurrentDirectory();
                string subPath = path + "\\下载文件\\";
                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
                client.Headers.Add("Cookie", COOKIE);
                client.Headers.Add("Referer", referUrl);
              
                if (false == System.IO.Directory.Exists(subPath))
                {
                    //创建pic文件夹
                    System.IO.Directory.CreateDirectory(subPath);
                }

                client.DownloadFile(URLAddress, subPath + name);
            }
            catch (WebException ex)
            {

                ex.ToString();
            }
        }



        #endregion

        bool zanting = true;
        bool status = true;

        public void run()
        {

            try
            {


                for (int i = Convert.ToInt32(textBox2.Text); i < Convert.ToInt32(textBox3.Text); i=i+Convert.ToInt32(textBox14.Text))
                {

                  string url=  textBox1.Text.Replace("[地址参数]",i.ToString());
                    
                    string html = GetUrl(url);
                    
                    MatchCollection aids = Regex.Matches(html, @textBox4.Text);
                   

                    for (int j = 0; j < aids.Count; j++)
                    {
                        string URL = textBox5.Text + aids[j].Groups[1].Value;

                        string ahtml = GetUrl(URL);

                        Match title = Regex.Match(ahtml, @textBox7.Text.Replace(" ", "").Trim());
                        Match down = Regex.Match(ahtml, @textBox6.Text.Replace(" ", "").Trim());
                        Match geshi = Regex.Match(ahtml, @textBox12.Text.Replace(" ", "").Trim());


                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(title.Groups[1].Value);
                        lv1.SubItems.Add(down.Groups[1].Value);

                        string durl = textBox13.Text + down.Groups[1].Value;

                        downloadFile(durl,title.Groups[1].Value+"."+geshi.Groups[1].Value);
                        lv1.SubItems.Add("true");
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                        if (status == false)
                            return;

                    }

                    Thread.Sleep(Convert.ToInt32(textBox11.Text));
                }



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }



        /// <summary>
        /// 不需要进入内容页
        /// </summary>

        public void run2()
        {

            try
            {


                for (int i = Convert.ToInt32(textBox2.Text); i < Convert.ToInt32(textBox3.Text); i = i + Convert.ToInt32(textBox14.Text))
                {

                    string url = textBox1.Text.Replace("[地址参数]", i.ToString());

                    string html = GetUrl(url);
                    
                    MatchCollection aids = Regex.Matches(html, @textBox6.Text.Replace(" ","").Trim());
                    MatchCollection titles = Regex.Matches(html, @textBox7.Text.Replace(" ", "").Trim());
                    MatchCollection sizes = Regex.Matches(html, @"""res_size"":""([\s\S]*?)""");



                    for (int j = 0; j < aids.Count; j++)
                    {

                        if (Convert.ToInt32(sizes[j].Groups[1].Value) > 20000000)
                        {
                            break;
                        }

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(titles[j].Groups[1].Value);

                        string durl = textBox13.Text.Replace("[地址参数]", aids[j].Groups[1].Value);
                        
                        lv1.SubItems.Add(durl);
                        if (titles[j].Groups[1].Value.Contains("doc") || titles[j].Groups[1].Value.Contains("docx") || titles[j].Groups[1].Value.Contains("ppt") || titles[j].Groups[1].Value.Contains("pptx"))
                        {
                            if (checkBox2.Checked == true)
                            {

                                downloadFile(durl, removeValid(titles[j].Groups[1].Value));
                            }
                            else
                            {
                                MatchCollection geshis = Regex.Matches(html, @textBox12.Text.Replace(" ", "").Trim());
                                downloadFile(durl, titles[j].Groups[1].Value + "." + geshis[j].Groups[1].Value);
                            }
                            lv1.SubItems.Add("true");
                            if (listView1.Items.Count > 2)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);
                            }
                        }
                        else
                        {
                            lv1.SubItems.Add("格式不符合");
                        }
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                        Thread.Sleep(Convert.ToInt32(textBox11.Text.Replace("\r\n","").Trim()));

                     
                       
                    }

                    
                }



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox1.SelectionLength = 0;
            textBox1.SelectedText = "[地址参数]";
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox5.SelectionLength = 0;
            textBox5.SelectedText = "[地址参数]";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (Control ctr in this.Controls)
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
          referUrl = textBox9.Text.Trim();
            COOKIE = textBox10.Text.Trim();
            if (checkBox1.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(run2));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
            }
            else
            {
                Thread thread = new Thread(new ThreadStart(run));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
            }
          
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox4.SelectionLength = 0;
            textBox4.SelectedText = @"([\s\S]*?)";
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox7.SelectionLength = 0;
            textBox7.SelectedText = @"([\s\S]*?)";
        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox6.SelectionLength = 0;
            textBox6.SelectedText = @"([\s\S]*?)";
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox13.SelectionLength = 0;
            textBox13.SelectedText = @"[地址参数]";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                foreach (Control ctr in groupBox1.Controls)
                {
                    if (ctr is TextBox)
                    {


                        string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                        FileStream fs1 = new FileStream(path + ctr.Name + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1);
                        sw.WriteLine(ctr.Text.Trim());
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
                        sw.WriteLine(ctr.Text.Trim());
                        sw.Close();
                        fs1.Close();

                    }
                }


                foreach (Control ctr in this.Controls)
                {
                    if (ctr is TextBox)
                    {


                        string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                        FileStream fs1 = new FileStream(path + ctr.Name + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1);
                        sw.WriteLine(ctr.Text.Trim());
                        sw.Close();
                        fs1.Close();

                    }
                }

                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            downloadFile("http://upload.ugc.yanxiu.com/doc/b4753c00d89e5f77585d924f11c7c5b5.pptx?from=106&rid=42322669&filename=%25E3%2580%258A%25E7%2599%25BD%25E6%25A1%25A6%25E3%2580%258B%25E8%25AF%25BE%25E6%2597%25B61.pptx&attachment=true&uploadto=0&convert_result=%7B%22self%22%3A%22b4753c00d89e5f77585d924f11c7c5b5%22%7D&res_source=6", "《白桦》课时1.pptx");
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }
    }
}
