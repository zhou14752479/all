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
using myDLL;

namespace 淘宝搜索结果抓取
{
    public partial class 直通车抓取 : Form
    {
        public 直通车抓取()
        {
            InitializeComponent();
        }
        Thread thread;


        List<string> lists = new List<string>();



        bool status = true;
        bool zanting = true;
        // public string cookie = "t=c9a6e5e62ca2c120600d8dcfaad4b45e; thw=cn; enc=ute7RgjnKNuWOLGvghOzHfWqQ%2Bda%2B8ztZ5sUbEMRoIt6Zyu4hnlnOniJR%2BCmKTF%2BRoN9coNBszSwMaBD4u3evQ%3D%3D; cna=nkhGF1vUIysCASrth2w3e468; v=0; cookie2=706a594478cd359f69d1e12e33b67581; _tb_token_=098750736364; _samesite_flag_=true; mt=ci=7_1; alitrackid=www.taobao.com; lastalitrackid=www.taobao.com; _m_h5_tk=7ea85b73cb3231eac90e9dc8508b4bf8_1594222212753; _m_h5_tk_enc=aebae86eef347f8828d6a6b7b45177a4; sgcookie=EFxUDEjvQ0waWYEYZfNho; uc3=nk2=qAa8HFm95q%2Fd0A%3D%3D&id2=UNX%2FQTb0NzMHoQ%3D%3D&lg2=W5iHLLyFOGW7aA%3D%3D&vt3=F8dBxGJklM302EReif8%3D; csg=57492921; lgc=%5Cu6C34%5Cu4E2D%5Cu7684%5Cu60A0%5Cu9C7C; dnk=%5Cu6C34%5Cu4E2D%5Cu7684%5Cu60A0%5Cu9C7C; skt=ca4d4ea3053d3044; existShop=MTU5NDMwMjM0Mw%3D%3D; uc4=nk4=0%40qj2lm0SW741TWX3CoxPeQKWyNIjT&id4=0%40UgJ6wqPI2HNj3h91yworhlt2facd; tracknick=%5Cu6C34%5Cu4E2D%5Cu7684%5Cu60A0%5Cu9C7C; _cc_=UIHiLt3xSw%3D%3D; tfstk=cIBdIVgqeP4hWvxO06FMPFM2v79bs_lovUmByYEljcLWtPlnYDfDWtXv9CVB43U3X; uc1=cookie14=UoTV6OIqJvBSmA%3D%3D&pas=0&existShop=false&cookie21=VT5L2FSpccLuJBreKQgf&cookie16=V32FPkk%2FxXMk5UvIbNtImtMfJQ%3D%3D; JSESSIONID=A31B50CF82FC524ABF1565ABB22B21E0; l=eB_6_1EeQ9YKC2pbBOfwourza77O7IRAguPzaNbMiOCPO9fHWCXhWZlugQLMCnGVh6UMR3PVQHf9BeYBqIv4n5U62j-lasMmn; isg=BHp6kyqV8fLlVnx4OlzzWSmXy6CcK_4FT3e-zoRzNo3SdxqxbLmSFbaFwwOrZ3ad";
        public string cookie = "";
        public string getHtml(string url)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.186 Safari/537.36");
            headers.Add("Cookie", cookie);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.UserAgent = headers["user-agent"];
            request.Timeout = 5000;
            request.KeepAlive = true;
            request.Headers["Cookie"] = headers["Cookie"];
            HttpWebResponse Response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(Response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
            string content = reader.ReadToEnd();
            return content;

        }

        public void getInfos(string html,string key)
        {

            MatchCollection titles = Regex.Matches(html, @"\\""ADGTITLE\\"":\\""([\s\S]*?)\\""");
            MatchCollection grades = Regex.Matches(html, @"\\""GRADE\\"":\\""([\s\S]*?)\\""");
            MatchCollection wangwangs = Regex.Matches(html, @"\\""WANGWANGID\\"":\\""([\s\S]*?)\\""");
            MatchCollection ISMALL = Regex.Matches(html, @"\\""ISMALL\\"":\\""([\s\S]*?)\\""");


            for (int i = 0; i < titles.Count; i++)
            {
                Thread.Sleep(500);
                try
                {
                    string title = method.Unicode2String(titles[i].Groups[1].Value.Replace("\\\\u", "\\u"));
                    string grade = method.Unicode2String(grades[i].Groups[1].Value.Replace("\\\\u", "\\u"));
                    string wangwang = method.Unicode2String(wangwangs[i].Groups[1].Value.Replace("\\\\u", "\\u"));
                    if (!lists.Contains(title))
                    {
                       
                        lists.Add(title);
                        if (ISMALL[i].Groups[1].Value == "0" && Convert.ToInt32(grade)<10001)
                        {
                            label2.Text = wangwang + "：符合抓取";
                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            //listViewItem.SubItems.Add(title);
                            listViewItem.SubItems.Add(wangwang);
                            listViewItem.SubItems.Add(key);
                            // listViewItem.SubItems.Add(grade);
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;
                        }
                        else
                        {
                            label2.Text = wangwang + "：不符合跳过";
                        }

                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                    continue;
                }
            }


        }
        public void run()
        {
            string[] keywords = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (string keyword in keywords)
            {
                if (keyword == "")
                {
                    continue;
                }
                string url = "https://s.taobao.com/search?q=" + keyword;
                int page = Convert.ToInt32(textBox2.Text);
                for (int i = 0; i < page; i++)
                {
                    int p = i * 44;
                    string URL = url + "&s=" + p.ToString();
                    string html = getHtml(URL);

                    getInfos(html,keyword);
                    Thread.Sleep(2000);
                    if (status == false)
                    {
                        return;
                    }
                }
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测


            string html = getHtml("http://acaiji.com/index/index/vip.html");

            if (!html.Contains(@"UDqVT"))
            {

                return;
            }

            #endregion

            cookie = webbrowser.COOKIE;

            if (cookie == "")
            {
                MessageBox.Show("请先登录");

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

        private void button5_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            webbrowser web = new webbrowser();
            web.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamReader streamReader = new StreamReader(this.openFileDialog1.FileName, Encoding.GetEncoding("utf-8"));
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    textBox1.Text += array[i] + "\r\n";

                }

            }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void 直通车抓取_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
               
            }
        }

        private void 直通车抓取_Load(object sender, EventArgs e)
        {

        }
    }
}
