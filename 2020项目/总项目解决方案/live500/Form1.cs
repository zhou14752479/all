using System;
using System.Collections;
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

namespace live500
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        
        
        
        bool zanting = true;
        string cookie = "_jzqx=1.1574666905.1574666905.1.jzqsr=live%2E500%2Ecom|jzqct=/wanchang%2Ephp.-; sdc_session=1574750368429; _jzqc=1; _jzqckmp=1; Hm_lvt_4f816d475bb0b9ed640ae412d6b42cab=1574662394,1574670892,1574750371; _qzjc=1; __utmc=63332592; _jzqa=1.3024433889462328000.1574662394.1574750369.1574757944.5; motion_id=1574757963912_0.20063945858556798; WT_FPC=id=undefined:lv=1574757965514:ss=1574757965514; sdc_userflag=1574757944139::1574757965519::3; Hm_lpvt_4f816d475bb0b9ed640ae412d6b42cab=1574757966; _qzja=1.892491754.1574662759148.1574750380463.1574757965624.1574750380463.1574757965624.0.0.0.17.5; _qzjto=2.2.0; _jzqb=1.3.10.1574757944.1; _qzjb=1.1574757965624.1.0.0.0; __utma=63332592.832186015.1574662397.1574750392.1574757967.4; __utmz=63332592.1574757967.4.3.utmcsr=live.500.com|utmccn=(referral)|utmcmd=referral|utmcct=/wanchang.php; __utmt=1; __utmb=63332592.2.10.1574757967; CLICKSTRN_ID=49.89.124.214-1574662395.188996::BBBDC09E0995FD2E900BCCA43F96ED30";

        public void run()
        {
            string date = textBox1.Text.Trim()+"-"+ textBox2.Text.Trim() + "-"+textBox3.Text;

            try
            {


                string html = method.gethtml("https://live.500.com/wanchang.php/?e="+date, cookie);
              
                MatchCollection matches = Regex.Matches(html, @"<tr id=""a([\s\S]*?)""");
                MatchCollection times = Regex.Matches(html, @"<td align=""center"">([\s\S]*?)</td>");

                
                ArrayList lists = new ArrayList();
                foreach (Match NextMatch in matches)
                {

                    lists.Add(NextMatch.Groups[1].Value);

                }
                
                for (int j = 0; j < lists.Count; j++)
                {

                    string ahtml = method.gethtml("https://odds.500.com/fenxi/yazhi-" + lists[j]+ ".shtml", cookie);


                  
                    Match aomen = Regex.Match(ahtml, @"title=""澳门"">([\s\S]*?)<td class=""td_one"">");
                    Match yishengbo = Regex.Match(ahtml, @"title=""易胜博"">([\s\S]*?)<td class=""td_one"">");
                  
                    
                    Match bisai = Regex.Match(ahtml, @"<title>([\s\S]*?)\(");
                    Match bifen = Regex.Match(ahtml, @"<strong>([\s\S]*?)</strong>");
                    MatchCollection a1s = Regex.Matches(aomen.Groups[1].Value, @"<td width=""58""([\s\S]*?)>([\s\S]*?)</td>");
                    MatchCollection a2s = Regex.Matches(aomen.Groups[1].Value, @""" ref=""([\s\S]*?)>([\s\S]*?)<");
                    MatchCollection a3s = Regex.Matches(aomen.Groups[1].Value, @"<td row=""1"" width=""58""([\s\S]*?)>([\s\S]*?)</td>");

                    MatchCollection b1s = Regex.Matches(yishengbo.Groups[1].Value, @"<td width=""58""([\s\S]*?)>([\s\S]*?)</td>");
                    MatchCollection b2s = Regex.Matches(yishengbo.Groups[1].Value, @""" ref=""([\s\S]*?)>([\s\S]*?)<");
                    MatchCollection b3s = Regex.Matches(yishengbo.Groups[1].Value, @"<td row=""1"" width=""58""([\s\S]*?)>([\s\S]*?)</td>");

                    if (a1s.Count > 1 && b1s.Count > 1)
                    {
                        ListViewItem listViewItem = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(bisai.Groups[1].Value);
                        listViewItem.SubItems.Add(bifen.Groups[1].Value);
                        listViewItem.SubItems.Add(a1s[0].Groups[2].Value);
                        listViewItem.SubItems.Add(a2s[0].Groups[2].Value);
                        listViewItem.SubItems.Add(a3s[0].Groups[2].Value);
                        listViewItem.SubItems.Add(a1s[1].Groups[2].Value);
                        listViewItem.SubItems.Add(a2s[1].Groups[2].Value);
                        listViewItem.SubItems.Add(a3s[1].Groups[2].Value);

                        listViewItem.SubItems.Add(b1s[0].Groups[2].Value);
                        listViewItem.SubItems.Add(b2s[0].Groups[2].Value);
                        listViewItem.SubItems.Add(b3s[0].Groups[2].Value);
                        listViewItem.SubItems.Add(b1s[1].Groups[2].Value);
                        listViewItem.SubItems.Add(b2s[1].Groups[2].Value);
                        listViewItem.SubItems.Add(b3s[1].Groups[2].Value);
                        listViewItem.SubItems.Add(times[(5*j)+1].Groups[1].Value);


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void run1()
        {

            try
            {
                string date1 = textBox4.Text.Trim() + "-" + textBox5.Text.Trim() + "-" + textBox6.Text;

                string html = method.gethtml("https://live.500.com/?e="+date1, cookie);

                MatchCollection matches = Regex.Matches(html, @"<tr id=""a([\s\S]*?)""");
                MatchCollection times = Regex.Matches(html, @"<td align=""center"">([\s\S]*?)</td>");
                ArrayList lists = new ArrayList();
                foreach (Match NextMatch in matches)
                {

                    lists.Add(NextMatch.Groups[1].Value);

                }

                for (int j = 0; j < lists.Count; j++)
                {

                    string ahtml = method.gethtml("https://odds.500.com/fenxi/ouzhi-" + lists[j] + ".shtml", cookie);
                   

                    Match bisai = Regex.Match(ahtml, @"<title>([\s\S]*?)\(");
                    Match bifen = Regex.Match(ahtml, @"<strong>([\s\S]*?)</strong>");

                    textBox7.Text = "https://odds.500.com/fenxi/ouzhi-" + lists[j] + ".shtml";
                    Match a1 = Regex.Match(ahtml, @"avwinj2"">([\s\S]*?)</td>");
                    Match a2 = Regex.Match(ahtml, @"avdrawj2"">([\s\S]*?)</td>");
                    Match a3 = Regex.Match(ahtml, @"avlostj2"">([\s\S]*?)</td>");

                    ListViewItem listViewItem = listView2.Items.Add((listView2.Items.Count + 1).ToString());
                    listViewItem.SubItems.Add(bisai.Groups[1].Value);
                    listViewItem.SubItems.Add(bifen.Groups[1].Value);
                    listViewItem.SubItems.Add(a1.Groups[1].Value);
                    listViewItem.SubItems.Add(a2.Groups[1].Value);
                    listViewItem.SubItems.Add(a3.Groups[1].Value);
                   
                    listViewItem.SubItems.Add(times[(4* j) + 1].Groups[1].Value);


                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            listView1.Visible = true;
            listView2.Visible = false;
            MessageBox.Show("已开始");
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            listView2.Visible = true;
            listView1.Visible = false;
            MessageBox.Show("已开始");
            Thread thread = new Thread(new ThreadStart(run1));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
        }
    }
}
