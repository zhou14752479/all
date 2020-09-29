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
using System.Windows.Forms;

namespace 数据获取
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
      
        public static string GetUrl(string Url, string charset)
        {

            string outStr = "";
            string tmpStr = "";

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; 
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  
                request.Referer = "https://p.likesc.net/";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36";
                request.AllowAutoRedirect = true;
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); 

                try
                {//循环获取
                    while ((tmpStr = reader.ReadLine()) != null)
                    {
                        outStr += tmpStr;
                    }
                }
                catch
                {

                }
                reader.Close();
                response.Close();

                return outStr;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return ex.ToString();

            }
        }


        public bool panduan(string valuea, string valueb)
        {
            bool a = true;
            Invoke(new Action(() =>
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (listView1.Items[i].SubItems[1].Text == valuea && listView1.Items[i].SubItems[2].Text == valueb)
                    {
                        a=false;
                    }

                }
            }));
            return a;
        }
        
        public void run()
        {
            string html = GetUrl(textBox1.Text.Trim(), "utf-8");
          
            string lotteryid = textBox2.Text.Trim();
            string pattern = @"lottery_"+ lotteryid + @"([\s\S]*?)},\\""lottery_";
            Match ahtml = Regex.Match(html,pattern);
            Invoke(new Action(() =>
            {
                textBox3.Text = html;
            }));
          
           
            MatchCollection last_codes= Regex.Matches(ahtml.Groups[1].Value, @"""last_code\\"":\\""([\s\S]*?)\\""");
            MatchCollection last_issues = Regex.Matches(ahtml.Groups[1].Value, @"""last_issue\\"":\\""([\s\S]*?)\\""");

            for (int i = 0; i < last_codes.Count; i++)
            {
                if (panduan(last_codes[i].Groups[1].Value, last_issues[i].Groups[1].Value))
                {
                    Invoke(new Action(() =>
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                        lv1.SubItems.Add(last_codes[i].Groups[1].Value);
                        lv1.SubItems.Add(last_issues[i].Groups[1].Value);
                        lv1.SubItems.Add(lotteryid);
                    }));
                }
            }



        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (RefreshTimer.Enabled == false)
            {
                RefreshTimer.Interval = 1;
                RefreshTimer.Enabled = true;
                button1.Text = "自动获取中";
               
            }
            else
            {
                RefreshTimer.Enabled = false;
                button1.Text = "开启获取";
               ;
            }
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        Thread Timerthread;
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            RefreshTimer.Interval = 8000;
            if (Timerthread == null || !Timerthread.IsAlive)
            {
                Timerthread = new Thread(run);
                Timerthread.Start();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RefreshTimer.Stop();
        }
    }
}
