using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 淘宝商品抓取
{
    public partial class Form1 : Form
    {
        class ListViewNF : System.Windows.Forms.ListView
        {
            public ListViewNF()
            {
                // 开启双缓冲
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

                // Enable the OnNotifyMessage event so we get a chance to filter out 
                // Windows messages before they get to the form's WndProc
                this.SetStyle(ControlStyles.EnableNotifyMessage, true);
            }

            protected override void OnNotifyMessage(Message m)
            {
                //Filter out the WM_ERASEBKGND message
                if (m.Msg != 0x14)
                {
                    base.OnNotifyMessage(m);
                }
            }
        }
        ArrayList list = new ArrayList();
        public Form1()
        {
            InitializeComponent();
        }
        bool status = true;
        bool zanting = true;
        // public string cookie = "t=c9a6e5e62ca2c120600d8dcfaad4b45e; thw=cn; enc=ute7RgjnKNuWOLGvghOzHfWqQ%2Bda%2B8ztZ5sUbEMRoIt6Zyu4hnlnOniJR%2BCmKTF%2BRoN9coNBszSwMaBD4u3evQ%3D%3D; cna=nkhGF1vUIysCASrth2w3e468; v=0; cookie2=706a594478cd359f69d1e12e33b67581; _tb_token_=098750736364; _samesite_flag_=true; mt=ci=7_1; alitrackid=www.taobao.com; lastalitrackid=www.taobao.com; _m_h5_tk=7ea85b73cb3231eac90e9dc8508b4bf8_1594222212753; _m_h5_tk_enc=aebae86eef347f8828d6a6b7b45177a4; sgcookie=EFxUDEjvQ0waWYEYZfNho; uc3=nk2=qAa8HFm95q%2Fd0A%3D%3D&id2=UNX%2FQTb0NzMHoQ%3D%3D&lg2=W5iHLLyFOGW7aA%3D%3D&vt3=F8dBxGJklM302EReif8%3D; csg=57492921; lgc=%5Cu6C34%5Cu4E2D%5Cu7684%5Cu60A0%5Cu9C7C; dnk=%5Cu6C34%5Cu4E2D%5Cu7684%5Cu60A0%5Cu9C7C; skt=ca4d4ea3053d3044; existShop=MTU5NDMwMjM0Mw%3D%3D; uc4=nk4=0%40qj2lm0SW741TWX3CoxPeQKWyNIjT&id4=0%40UgJ6wqPI2HNj3h91yworhlt2facd; tracknick=%5Cu6C34%5Cu4E2D%5Cu7684%5Cu60A0%5Cu9C7C; _cc_=UIHiLt3xSw%3D%3D; tfstk=cIBdIVgqeP4hWvxO06FMPFM2v79bs_lovUmByYEljcLWtPlnYDfDWtXv9CVB43U3X; uc1=cookie14=UoTV6OIqJvBSmA%3D%3D&pas=0&existShop=false&cookie21=VT5L2FSpccLuJBreKQgf&cookie16=V32FPkk%2FxXMk5UvIbNtImtMfJQ%3D%3D; JSESSIONID=A31B50CF82FC524ABF1565ABB22B21E0; l=eB_6_1EeQ9YKC2pbBOfwourza77O7IRAguPzaNbMiOCPO9fHWCXhWZlugQLMCnGVh6UMR3PVQHf9BeYBqIv4n5U62j-lasMmn; isg=BHp6kyqV8fLlVnx4OlzzWSmXy6CcK_4FT3e-zoRzNo3SdxqxbLmSFbaFwwOrZ3ad";
        public string cookie = "";
        public  string getHtml(string url)
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
        
        public  void getInfos(string html)
        {

            MatchCollection titles = Regex.Matches(html, @"""raw_title"":""([\s\S]*?)""");
            MatchCollection links = Regex.Matches(html, @"""nid"":""([\s\S]*?)""");
            MatchCollection prices = Regex.Matches(html, @"""view_price"":""([\s\S]*?)""");
            MatchCollection sales = Regex.Matches(html, @"""view_sales"":""([\s\S]*?)""");


            for (int i = 0; i < titles.Count; i++)
            {
                Match s = Regex.Match(sales[i].Groups[1].Value,@"\d{1,}");
                if (s.Groups[0].Value!= "")
                {
                    if (Convert.ToInt32(s.Groups[0].Value) >= Convert.ToInt32(textBox5.Text))
                    {
                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(titles[i].Groups[1].Value);
                        listViewItem.SubItems.Add("https://item.taobao.com/item.htm?id=" + links[i].Groups[1].Value);
                        listViewItem.SubItems.Add(prices[i].Groups[1].Value);
                        listViewItem.SubItems.Add(sales[i].Groups[1].Value);

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        
                    }
                }
                if (status == false)
                {
                    return;
                }
            }
           

        }
 
  

        private void Form1_Load(object sender, EventArgs e)
        {
            
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

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {

            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized; //最小化
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox1.Text = "";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamReader streamReader = new StreamReader(this.openFileDialog1.FileName, Encoding.Default);
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    textBox1.Text += array[i] + "\r\n";

                }

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            webbrowser web = new webbrowser();
            web.Show();
        }

        public void run()
        {
            StringBuilder sb = new StringBuilder();
            int aprice = Convert.ToInt32(textBox3.Text);
            int bprice = Convert.ToInt32(textBox4.Text);
            sb.Append("&filter=reserve_price%5B"+aprice+"%2C"+bprice+"%5D");
            if (checkBox1.Checked == true)
            {
                sb.Append("&auction_tag%5B%5D=4806");
            }
            if (checkBox2.Checked == true)
            {
                sb.Append("&data-key=baoyou");
            }
            if (checkBox3.Checked == true)
            {
                sb.Append("&filter_tianmao=tmall");
            }
            switch (comboBox1.Text.Trim())
            {
   
                case "默认排序":
                    break;
                case "价格从高到低":
                    sb.Append("&sort=price-desc");
                    break;
                case "价格从低到高":
                    sb.Append("&sort=price-asc");
                    break;
                case "总价从低到高":
                    sb.Append("&sort=total-asc");
                    break;
                case "总价从高到低":
                    sb.Append("&sort=total-desc");
                    break;
                case "销量从高到低":
                    sb.Append("&sort=sale-desc");
                    break;

            }




            string keyword =textBox1.Text.Trim();
            string url = "https://s.taobao.com/search?q=" + keyword;
            int page = Convert.ToInt32(textBox2.Text);
            for (int i = 0; i <= page; i++)
            {
                label9.Text = "正在抓取第" + (i+1) + "页";
                int p = i * 44;
                string URL = url + "&s=" + p.ToString()+sb.ToString();
                string html = getHtml(URL);
                getInfos(html);
                Thread.Sleep(2000);
                if (status == false)
                {
                    return;
                }
            }
           
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            cookie = webbrowser.COOKIE;

            if (cookie == "")
            {
                MessageBox.Show("请先登陆");
                MessageBox.Show("请先登陆");
                return;
            }
            status = true;
            Thread search_thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            search_thread.Start();
        }


        public static void expotTxt(ListView lv1)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string dt = DateTime.Now.GetHashCode().ToString();
                StringBuilder sb = new StringBuilder();
                foreach (ListViewItem item in lv1.Items)
                {
                    try
                    {
                        List<string> list = new List<string>();
                        string temp = item.SubItems[2].Text;
                       
              
                        list.Add(temp );
                        foreach (string tel in list)
                        {
                            sb.AppendLine(tel);
                        }

                        string path = "";

                        path = dialog.SelectedPath + "\\导出结果"+dt + ".txt";

                        System.IO.File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
                    }

                    catch
                    {
                        continue;
                    }
                }
                MessageBox.Show("导出完成");
            }

        }


        private void button4_Click(object sender, EventArgs e)
        {
            expotTxt(listView1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
           
        }

        private void listView1_MouseHover(object sender, EventArgs e)
        {
           
            label7.Text = "    " + webbrowser.tbName;
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
