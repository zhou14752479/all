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
using helper;


namespace 主程序202005
{
    public partial class 淘豆网过滤 : Form
    {
        public 淘豆网过滤()
        {
            InitializeComponent();
        }
        static string COOKIE = "Hm_lvt_ca7e9f56b3d80d0351ff52f7b43d6132=1589796449; testid=15539291748; user_loginid=15539291748; user_id=356796; user_nk=15539291748; user_email=; user_photo=0; ASP.NET_SessionId=og53mugjcyppeuegqcekvslw; user_pass=nYtg6TWxqkMN4YQmRY/p6xeSPpliF7ImuydIW9OY7TA3SauZ1WH2CTs9REw31I3gIfEIpbnIn73RWecCC3eCJ3svaxMj9M9UsA878uO50YA=; Hm_lpvt_ca7e9f56b3d80d0351ff52f7b43d6132=1589797121";
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("sec-fetch-mode:navigate");
                headers.Add("sec-fetch-site:same-origin");
                headers.Add("sec-fetch-user:?1");
                headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                // request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
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

        public bool panduan(string title)

        {

            string[] text = textBox8.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != "")
                {
                    if (title.Contains(text[i]))
                    {
                        return true;
                    }
                }

            }
            return false;

        }


        public void getdocs()
        {

            try
            {
                for (int i = Convert.ToInt32(textBox1.Text.Trim()); i <= Convert.ToInt32(textBox2.Text.Trim()); i++)
                {
                    textBox3.Text += "正在筛选第" + i + "页" + "\r\n";
                    string url = "https://admin.taodocs.com/selfSearch.aspx?fnn=true&sort=0&ft=&fn=mybook&q=&p="+i+"&ps=25";
                   
                    string html = GetUrl(url);
                  
                    MatchCollection uids = Regex.Matches(html, @"DocID"":([\s\S]*?),");
                    MatchCollection titles = Regex.Matches(html, @"Title"":""([\s\S]*?)""");
                   
                    for (int j = 0; j < uids.Count; j++)
                    {
                        if (panduan(titles[j].Groups[1].Value))
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        
                            lv1.SubItems.Add(uids[j].Groups[1].Value);
                            lv1.SubItems.Add(titles[j].Groups[1].Value);
                            lv1.SubItems.Add("--");
                        }
                    }
                }

                MessageBox.Show("筛选结束");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        public bool deletedocs(string id)
        {

            try
            {
                string url = "https://www.taodocs.com/my/ajax.aspx?method=deleteProduct&pid="+id+"&1589797066381 ";
               
                string html = GetUrl(url);
                if (html.Contains("删除成功"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {

                return false;
            }
        }
        public void del()
        {
            textBox3.Text = "";
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                string uid = listView1.CheckedItems[i].SubItems[1].Text;

                textBox3.Text += uid + "：删除状态" + deletedocs(uid) + "\r\n";
                Thread.Sleep(Convert.ToInt32(textBox4.Text) * 1000);
            }
            MessageBox.Show("删除结束");
        }
        private void 淘豆网过滤_Load(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri("https://www.taodocs.com/login.aspx?=1.0");
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(getTitle);
        }
        private void getTitle(object sender, EventArgs e)
        {

            System.IO.StreamReader getReader = new System.IO.StreamReader(this.webBrowser1.DocumentStream, System.Text.Encoding.GetEncoding("utf-8"));

            string html = getReader.ReadToEnd();


            if (html.Contains("touxiang"))
            {
                label4.Text = "登陆成功";
                return;
            }
            else
            {
                label4.Text = "登陆失败";
            }


        }
        private void button7_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamReader streamReader = new StreamReader(this.openFileDialog1.FileName, Encoding.Default);
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    textBox8.Text += array[i] + "\r\n";

                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"doc88"))
            {
                textBox3.Text = "";
                COOKIE = method.GetCookies("https://www.taodocs.com/my/myBook.aspx");
                Thread thread = new Thread(new ThreadStart(getdocs));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(del));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = true;
            }
        }

        private void 取消全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 淘豆网过滤_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
