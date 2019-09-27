using System;
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

namespace 商标查询
{
    public partial class shangBiao : Form
    {
        public shangBiao()
        {
            InitializeComponent();
        }

        bool zanting = true;
        #region  主程序
        public void run()
        {
            try
            
{
                StreamReader sr = new StreamReader(textBox1.Text, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int a = 0; a < text.Length - 1; a++)
                {
                    label2.Text = "正在抓取"+text[a]+"........";
                    string url = "https://api.ipr.kuaifawu.com/xcx/tmsearch/index";

             

                string postdata=textBox2.Text;
                  
              postdata=  Regex.Replace(postdata, @"\d{6,}",text[a].Trim());
                    
                    string html =method.PostUrl(url,postdata,"","utf-8");
                    string aurl = "https://www.qccip.com/trademark/search/index.html?searchType=MARKNAME&keyword="+text[a];
                    string acookie = "Hm_lvt_bba8c1510f76443f6de83bcb863f8e4a=1569483410; NTKF_T2D_CLIENTID=guestA9DE2DBC-E274-ED6C-D5BA-6C804E772568; nTalk_CACHE_DATA={uid:kf_10532_ISME9754_guestA9DE2DBC-E274-ED,tid:1569483411062766}; tokenOverTime=1570088295476; qcc_token=eyJhbGciOiJIUzUxMiJ9.eyJpc3MiOiJhcGkubGFuZ2RvbmcucWNjLmNuIiwiY2xpZW50IjpudWxsLCJ1c2VySWQiOiIzNzMxIiwiaWF0IjoxNTY5NDgzNDk1fQ.t-EwmOIh7WoQCfSA5XS7654Zf8HIA-ObfoiKxJLUoYBLs2KJu50peTBOt6R9c9g0A0wQL3Lc4cNuJ3AeBb8oBw; phoneNo=17606117606; userName=17606117606; Hm_lpvt_bba8c1510f76443f6de83bcb863f8e4a=1569484374";
                    string ahtml = method.GetUrlWithCookie(aurl, acookie, "utf-8");
                    Match aid = Regex.Match(ahtml, @"""encryptid"":""([\s\S]*?)""");
                    string burl = "https://www.qccip.com/trademark/detail/"+aid.Groups[1].Value+".html";
                    string bhtml = method.GetUrlWithCookie(burl, acookie, "utf-8");
                    Match adate = Regex.Match(bhtml, @"""flowDate"":""([\s\S]*?)""");
                    Match aname = Regex.Match(bhtml, @"""flowItem"":""([\s\S]*?)""");


                    Match a1 = Regex.Match(html, @"""MarkName"":""([\s\S]*?)""");
                        Match a2 = Regex.Match(html, @"UnionTypeCode"":([\s\S]*?),");
                        Match a3 = Regex.Match(html, @"""StateDate2017"":""([\s\S]*?)""");
                        Match a4 = Regex.Match(html, @"""AppPerson"":""([\s\S]*?)""");
                        Match a5 = Regex.Match(html, @"""Addr"":""([\s\S]*?)""");  // 地址


                    Match a51 = Regex.Match(html, @"""Addr"":""([\s\S]*?)省");  // 省
                    Match a52 = Regex.Match(html, @"省([\s\S]*?)市");  // 市

                        Match a6 = Regex.Match(html, @"""AppDate"":""([\s\S]*?)""");
                        Match a7 = Regex.Match(html, @"""AgentName"":""([\s\S]*?)""");


                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      
                        lv1.SubItems.Add(text[a]);
                        lv1.SubItems.Add(a1.Groups[1].Value);
                        lv1.SubItems.Add(a2.Groups[1].Value);
                        lv1.SubItems.Add(aname.Groups[1].Value+adate.Groups[1].Value);//收发文日期
                        lv1.SubItems.Add(a4.Groups[1].Value);
                        lv1.SubItems.Add("中国");
                        lv1.SubItems.Add(a51.Groups[1].Value);
                        lv1.SubItems.Add(a52.Groups[1].Value);
                        lv1.SubItems.Add(a5.Groups[1].Value);
                        lv1.SubItems.Add(a6.Groups[1].Value);
                        lv1.SubItems.Add(a7.Groups[1].Value);
                        

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        Thread.Sleep(1000);


                    }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
        private void Button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }



        private void Button1_Click(object sender, EventArgs e)
        {

            button1.Enabled = false;
            #region 通用验证

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "16.16.16.16")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {

                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;


            }
            else
            {
                MessageBox.Show("IP不符");

            }
            #endregion
        }

        private void shangBiao_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            zanting = true;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void ShangBiao_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0); //点确定的代码
            }
            else
            { //点取消的代码 
            }
        }
    }
}
