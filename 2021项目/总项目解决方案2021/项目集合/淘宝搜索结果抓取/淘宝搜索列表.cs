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
using static myDLL.method;

namespace 淘宝搜索结果抓取
{
    public partial class 淘宝搜索列表 : Form
    {
        public 淘宝搜索列表()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void 淘宝搜索列表_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://acaiji.com/index/index/vip.html","utf-8");

            if (!html.Contains(@"ukYVo"))
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
        public void run()
        {
            button1.Enabled = false;

            try
            {
                string[] keywords = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string keyword in keywords)
                {
                    if (keyword == "")
                    {
                        continue;
                    }
                    string url = "https://s.taobao.com/search?q=" + keyword + "&filter=reserve_price%5B" + numericUpDown1.Value + "%2C" + numericUpDown2.Value + "%5D";
                    int page = Convert.ToInt32(textBox4.Text);
                    for (int i = 0; i < page; i++)
                    {
                        int p = i * 44;
                        string URL = url + "&s=" + p.ToString();
                        string html = getHtml(URL);

                        getInfos(keyword, html);
                        Thread.Sleep(2000);
                        if (status == false)
                        {
                            return;
                        }
                    }
                }
                button1.Enabled = true;
            }
            catch (Exception ex)
            {

               ex.ToString();
            }

        }

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

        public void getInfos(string key,string html)
        {

            try
            {
                MatchCollection titles = Regex.Matches(html, @"""raw_title"":""([\s\S]*?)""");
                MatchCollection nids = Regex.Matches(html, @"""nid"":""([\s\S]*?)""");
               // MatchCollection wangwangs = Regex.Matches(html, @""",""nick"":""([\s\S]*?)""");
                MatchCollection userids = Regex.Matches(html, @""",""user_id"":""([\s\S]*?)""");
                MatchCollection prices = Regex.Matches(html, @"""view_price"":""([\s\S]*?)""");
                MatchCollection sales = Regex.Matches(html, @"""view_sales"":""([\s\S]*?)""");
                //MatchCollection pic_urls = Regex.Matches(html, @"""pic_url"":""([\s\S]*?)""");
                MatchCollection isTmalls = Regex.Matches(html, @"""isTmall"":([\s\S]*?),");
                for (int i = 0; i < titles.Count; i++)
                {
                    Thread.Sleep(50);
                    try
                    {
                        string title = titles[i].Groups[1].Value;
                        string userid = userids[i].Groups[1].Value;
                        textBox5.Text += DateTime.Now.ToLongTimeString() + "正在采集" + title+"\r\n";
                        if (!lists.Contains(title + userid))
                        {
                            if (guolv(title, isTmalls[i].Groups[1].Value))
                            {
                                string salecount = sales[i].Groups[1].Value.Replace("人付款", "").Replace("+", "");
                                if (Convert.ToInt32(salecount) >= numericUpDown3.Value && Convert.ToInt32(salecount) <= numericUpDown4.Value)
                                {
                                    lists.Add(title + userid);
                                    ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                    listViewItem.SubItems.Add(title);
                                    listViewItem.SubItems.Add("https://item.taobao.com/item.htm?id=" + nids[i].Groups[1].Value);
                                    listViewItem.SubItems.Add(prices[i].Groups[1].Value);
                                   // listViewItem.SubItems.Add(wangwangs[i].Groups[1].Value);
                                    listViewItem.SubItems.Add(salecount);
                                   // listViewItem.SubItems.Add("https:" + pic_urls[i].Groups[1].Value);
                                    listViewItem.SubItems.Add(key);
                                    
                                    while (this.zanting == false)
                                    {
                                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                    }
                                    if (status == false)
                                        return;
                                    if (listView1.Items.Count > 2)
                                    {
                                        this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                                    }
                                }
                                else
                                {
                                    textBox5.Text += DateTime.Now.ToLongTimeString() + "不符合条件，跳过\r\n";
                                }
                            }
                            else
                            {
                                textBox5.Text += DateTime.Now.ToLongTimeString() + "包含设置违规词，跳过\r\n";
                            }

                        }
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                }
            }
            catch (Exception ex)
            {

                ex.ToString();
            }


        }

        Thread thread;


        public bool guolv(string title,string istmall)
        {
          
            if(checkBox1.Checked==true)
            {
                if(istmall.Trim()=="true")
                {
                    return false;
                }
            }
            if (textBox3.Text == "")
            {
                return true;
            }
            try
            {
                StreamReader streamReader = new StreamReader(textBox3.Text, EncodingType.GetTxtType(textBox3.Text));
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] != "")
                    {
                        if (title.Contains(array[i]))
                        {
                            return false;
                        }
                    }

                }

                return true;

            }
            catch (Exception)
            {
                return true;
                
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            webbrowser web = new webbrowser();
            web.Show();
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

        private void button5_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            status = false;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.ListviewToTxt(listView1,2);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            lists.Clear();
            listView1.Items.Clear();
            button1.Enabled = true;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
        private Point mPoint = new Point();
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamReader streamReader = new StreamReader(this.openFileDialog1.FileName, EncodingType.GetTxtType(openFileDialog1.FileName));
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    textBox1.Text += array[i] + "\r\n";

                }

            }
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                textBox3.Text =openFileDialog1.FileName;
               

            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            this.textBox5.SelectionStart = this.textBox5.Text.Length;
            this.textBox5.SelectionLength = 0;
            this.textBox5.ScrollToCaret();
            if(textBox5.Text.Length>5000)
            {
                textBox5.Text = "";
            }
        }
    }
}
