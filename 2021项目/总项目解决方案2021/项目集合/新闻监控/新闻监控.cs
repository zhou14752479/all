using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 新闻监控
{
    public partial class 新闻监控 : Form
    {

        [DllImport("user32.dll")]
        public static extern int MessageBoxTimeoutA(IntPtr hWnd, string msg, string Caps, int type, int Id, int time);//引用DLL

        public 新闻监控()
        {
            InitializeComponent();
        }
        Thread thread;
        public void run()
        {

            try
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("请输入关键字");
                    return;
                }

                string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {
                    string keyword = text[i].Trim();
                    if(keyword!="")
                    {
                        string url = "https://www.baidu.com/s?ie=utf-8&medium=1&rtt=4&bsst=1&rsv_dl=news_t_sk&cl=2&wd=" + System.Web.HttpUtility.UrlEncode(keyword) + "&tn=news&rsv_bp=1&tfflag=0&rn=10";

                        string html = method.GetUrl(url, "utf-8");
                        MatchCollection items = Regex.Matches(html, @"<!--s-text-->([\s\S]*?)<!--/s-text-->");
                        MatchCollection times = Regex.Matches(html, @"c-font-normal"">([\s\S]*?)</span>");
                        MatchCollection resources = Regex.Matches(html, @"c-gap-right"">([\s\S]*?)</span>");
                        MatchCollection urls = Regex.Matches(html, @"<h3 class=""news-title_1YtI1""><a href=""([\s\S]*?)""");
                        for (int j = 0; j < items.Count/2; j++)
                        {
                           

                            string title = items[2*j].Groups[1].Value;
                            string body = items[(2*j)+1].Groups[1].Value;
                            string time = times[j].Groups[1].Value;
                            string resource = resources[j].Groups[1].Value;

                            title = title.Replace(keyword, "【" + keyword + "】").Replace("<em>","").Replace("</em>", "");
                            body = body.Replace(keyword, "【" + keyword + "】").Replace("<em>", "").Replace("</em>", "");

                            //MessageBoxTimeoutA((IntPtr)0, title, "消息框", 0, 0, 500);// 直接调用  3秒后自动关闭 父窗口句柄没有直接用0代替
                            ListViewItem lv1 = listView1.Items.Add(keyword); //使用Listview展示数据
                            lv1.SubItems.Add(title);
                            lv1.SubItems.Add(time);
                            lv1.SubItems.Add(resource);
                            lv1.SubItems.Add(body);
                            lv1.SubItems.Add(urls[j].Groups[1].Value);

                        }
                    }

                }

            }
            catch (Exception ex)
            {

                ex.ToString();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            if (checkBox2.Checked == true)
            {
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //listView1.Items.Clear();
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            timer1.Stop();
        }

        private void 新闻监控_Load(object sender, EventArgs e)
        {
            foreach (Control ctr in Controls)
            {

                if (ctr is TextBox)
                {

                    string path = AppDomain.CurrentDomain.BaseDirectory;
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

        private void 新闻监控_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出吗？", "退出询问"
             , MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;//告诉窗体关闭这个任务取消

            }
            else
            {
                foreach (Control ctr in Controls)
                {
                    if (ctr is TextBox)
                    {


                        string path = AppDomain.CurrentDomain.BaseDirectory;
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

        private void button3_Click(object sender, EventArgs e)
        {
            method.ListviewToTxt(listView1,4);
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;

            System.Diagnostics.Process.Start(listView1.SelectedItems[0].SubItems[5].Text);
           
        }
    }
}
