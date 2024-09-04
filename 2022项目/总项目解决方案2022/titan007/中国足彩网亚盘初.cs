using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using myDLL;
using System.Text.RegularExpressions;
using System.Collections;

namespace titan007
{
    public partial class 中国足彩网亚盘初 : Form
    {
        public 中国足彩网亚盘初()
        {
            InitializeComponent();
        }
        Thread thread;

        bool zanting = true;
        bool status = true;


        private void button1_Click(object sender, EventArgs e)
        {
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
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

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 中国足彩网亚盘初_Load(object sender, EventArgs e)
        {

        }

        List<string> list1 = new List<string>();
        List<string> list2 = new List<string>();
        #region  亚盘初值
        public void run()
        {


            string html = method.GetUrl("https://live.zgzcw.com/qb/", "utf-8");
            label1.Text = "正在提取....";

            MatchCollection ids = Regex.Matches(html, @"matchid=""([\s\S]*?)""");
           
            for (int i = 0; i < ids.Count; i++)
    
                try
                {

                    label1.Text = "正在提取...."+ ids[i].Groups[1].Value;
                    string url = "https://fenxi1.zgzcw.com/" + ids[i].Groups[1].Value +"/ypdb";
                    string ahtml= method.GetUrl(url, "utf-8");

                    string matchname = Regex.Match(ahtml, @"<h2 pid=([\s\S]*?)<p>([\s\S]*?)<").Groups[2].Value.Trim();
                    string teama = Regex.Match(ahtml, @"<title>([\s\S]*?)VS").Groups[1].Value.Trim();
                    string teamb= Regex.Match(ahtml, @"VS([\s\S]*?)亚盘").Groups[1].Value.Trim();
                    string time = Regex.Match(ahtml, @"比赛时间：([\s\S]*?)<").Groups[1].Value.Trim();

                    MatchCollection aahtml = Regex.Matches(ahtml, @"<label class=""sxinput"">([\s\S]*?)同</a>");
                    
                    for (int a = 0; a < aahtml.Count; a++)
                    {
                        //MessageBox.Show(aahtml[a].Groups[1].Value);
                        string company= Regex.Match(aahtml[a].Groups[1].Value, @"class=""border-r border-l""([\s\S]*?)<").Groups[1].Value.Replace(">","").Trim();


                        string chu_a = Regex.Match(aahtml[a].Groups[1].Value, @"chupan-w-([\s\S]*?)""	data=""([\s\S]*?)""").Groups[2].Value;
                        string chu_b= Regex.Match(aahtml[a].Groups[1].Value, @"chupan-s-([\s\S]*?)""	data=""([\s\S]*?)""").Groups[2].Value;
                        string chu_c = Regex.Match(aahtml[a].Groups[1].Value, @"chupan-l-([\s\S]*?)""	data=""([\s\S]*?)""").Groups[2].Value;




                        if (company == "威廉希尔")
                        {
                            ListViewItem lv1 = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
                            lv1.SubItems.Add(matchname);
                            lv1.SubItems.Add(teama);
                            lv1.SubItems.Add(teamb);
                            lv1.SubItems.Add(time);
                            lv1.SubItems.Add(company);
                            lv1.SubItems.Add(chu_a);
                            lv1.SubItems.Add(chu_b);
                            lv1.SubItems.Add(chu_c);
                        }
                        if ( company == "Bet365" )
                        {
                            list1.Add(matchname);
                            list1.Add(teama);
                            list1.Add(teamb);
                            list1.Add(time);
                            list1.Add(company);
                            list1.Add(chu_a);
                            list1.Add(chu_b);
                            list1.Add(chu_c);
                        }
                        if (company == "金宝博")
                        {
                            list2.Add(matchname);
                            list2.Add(teama);
                            list2.Add(teamb);
                            list2.Add(time);
                            list2.Add(company);
                            list2.Add(chu_a);
                            list2.Add(chu_b);
                            list2.Add(chu_c);
                        }
                        if (listView1.Items.Count > 2)
                        {

                            listView1.EnsureVisible(listView1.Items.Count - 1);
                        }
                    }
                    if (list1.Count != 0)
                    {
                        ListViewItem lv2 = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
                        foreach (string item in list1)
                        {
                            lv2.SubItems.Add(item);
                        }
                    }
                    if (list2.Count != 0)
                    {
                        ListViewItem lv3 = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
                        foreach (string item in list2)
                        {
                            lv3.SubItems.Add(item);
                        }
                    }

                  
                   if(list1.Count>0 || list2.Count>0)
                    {
                        ListViewItem lv = listView1.Items.Add(listView1.Items.Count.ToString());
                        lv.SubItems.Add("------------");
                        lv.SubItems.Add("------------------------");
                        lv.SubItems.Add("------------------------");
                        lv.SubItems.Add("------------------------");
                        lv.SubItems.Add("------------");
                        lv.SubItems.Add("------------");
                        lv.SubItems.Add("------------");
                        lv.SubItems.Add("------------");
                    }
                    list1.Clear();
                    list2.Clear();
                    Thread.Sleep(1000);
                    while (!this.zanting)
                    {
                        Application.DoEvents();
                    }
                    if (status == false)
                        return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
               

               
            }
        


        #endregion
    }
}
