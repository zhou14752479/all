using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace titan007
{
    public partial class 进球数 : Form
    {
        public 进球数()
        {
            InitializeComponent();
        }

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
        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }


        #endregion


        Thread thread;

        bool zanting = true;
        bool status = true;

        #region 订单信息
        public void run()
        {





            string html = method.GetUrl("http://live.titan007.com/vbsxml/bfdata_ut.js?r=0071657803475000", "utf-8");




            MatchCollection ids = Regex.Matches(html, @"A\[([\s\S]*?)\]=""([\s\S]*?)\^");

            foreach (Match id in ids)
            {
                string a18_1 = "";
                string a18_2 = "";
                string a18_3 = "";
                string a18_4 = "";
                string a18_5 = "";
                string a18_6 = "";
                string a18_7 = "";
                string a18_8 = "";


                try
                {
                    string aurl = "https://vip.titan007.com/OverDown_n.aspx?id=" + id.Groups[2].Value+ "&l=0";
                    string ahtml = method.GetUrl(aurl, "utf-8");
                    MatchCollection teams = Regex.Matches(ahtml, @"alt=""([\s\S]*?)""");
                    Match match = Regex.Match(ahtml, @"class=""LName"">([\s\S]*?)</a>([\s\S]*?)&nbsp");

                    MatchCollection dataa = Regex.Matches(ahtml, @"<tr bgcolor=""#FFFFFF""  >([\s\S]*?)</tr>");
                    MatchCollection datab = Regex.Matches(ahtml, @"<tr bgcolor=""#E8F2FF""  >([\s\S]*?)</tr>");

                    for (int i = 0; i < dataa.Count; i++)
                    {

                        string gongsi = Regex.Match(dataa[i].Groups[1].Value, @"<td height=""25"">([\s\S]*?)<").Groups[1].Value;
                        MatchCollection a = Regex.Matches(dataa[i].Groups[1].Value, @"<td title=""([\s\S]*?)>([\s\S]*?)</td>");

                        if (gongsi.Contains("12") || gongsi.Contains("Crow") || gongsi.Contains("易") || gongsi.Contains("10") || gongsi.Contains("盈"))
                        {

                            if (a.Count > 2)
                            {
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(match.Groups[1].Value.Trim());
                                lv1.SubItems.Add(teams[0].Groups[1].Value.Trim().Replace(" ", ""));
                                lv1.SubItems.Add(teams[1].Groups[1].Value.Trim());
                                lv1.SubItems.Add(match.Groups[2].Value.Trim());
                                lv1.SubItems.Add(gongsi.Replace("18*", "18bet").Replace("10*", "10bet").Replace("12*", "12bet").Replace("澳*", "澳门").Replace("Crow*", "Crown").Replace("易*", "易胜博").Replace("盈*", "盈和"));
                                lv1.SubItems.Add(a[0].Groups[2].Value);
                                lv1.SubItems.Add(a[1].Groups[2].Value);
                                lv1.SubItems.Add(a[2].Groups[2].Value);
                            }

                        }

                        if (gongsi.Contains("18*"))
                        {
                            a18_1 = match.Groups[1].Value.Trim();
                            a18_2 = teams[0].Groups[1].Value.Trim().Replace(" ", "");
                            a18_3 = teams[1].Groups[1].Value.Trim();
                            a18_4 = match.Groups[2].Value.Trim();
                            a18_5 = gongsi.Replace("18*", "18bet");
                            a18_6 = a[0].Groups[2].Value;
                            a18_7 = a[1].Groups[2].Value;
                            a18_8 = a[2].Groups[2].Value;
                        }
                    }

                    for (int i = 0; i < datab.Count; i++)
                    {

                        string gongsi = Regex.Match(datab[i].Groups[1].Value, @"<td height=""25"">([\s\S]*?)<").Groups[1].Value;
                        MatchCollection a = Regex.Matches(datab[i].Groups[1].Value, @"<td title=""([\s\S]*?)>([\s\S]*?)</td>");

                        if (gongsi.Contains("澳") || gongsi.Contains("Crow") || gongsi.Contains("易") || gongsi.Contains("10") || gongsi.Contains("盈"))
                        {
                            if (a.Count > 2)
                            {
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(match.Groups[1].Value.Trim());
                                lv1.SubItems.Add(teams[0].Groups[1].Value.Trim().Replace(" ", ""));
                                lv1.SubItems.Add(teams[1].Groups[1].Value.Trim());
                                lv1.SubItems.Add(match.Groups[2].Value.Trim());
                                lv1.SubItems.Add(gongsi.Replace("18*", "18bet").Replace("10*", "10bet").Replace("12*", "12bet").Replace("澳*", "澳门").Replace("Crow*", "Crown").Replace("易*", "易胜博").Replace("盈*", "盈和"));
                                lv1.SubItems.Add(a[0].Groups[2].Value);
                                lv1.SubItems.Add(a[1].Groups[2].Value);
                                lv1.SubItems.Add(a[2].Groups[2].Value);
                            }

                        }

                        if (gongsi.Contains("18*"))
                        {
                            a18_1 = match.Groups[1].Value.Trim();
                            a18_2 = teams[0].Groups[1].Value.Trim().Replace(" ", "");
                            a18_3 = teams[1].Groups[1].Value.Trim();
                            a18_4 = match.Groups[2].Value.Trim();
                            a18_5 = gongsi.Replace("18*", "18bet");
                            a18_6 = a[0].Groups[2].Value;
                            a18_7 = a[1].Groups[2].Value;
                            a18_8 = a[2].Groups[2].Value;
                        }

                    }






                    if (a18_1 != "")
                    {
                        ListViewItem lv = listView1.Items.Add(listView1.Items.Count.ToString());
                        lv.SubItems.Add(a18_1);
                        lv.SubItems.Add(a18_2);
                        lv.SubItems.Add(a18_3);
                        lv.SubItems.Add(a18_4);
                        lv.SubItems.Add(a18_5);
                        lv.SubItems.Add(a18_6);
                        lv.SubItems.Add(a18_7);
                        lv.SubItems.Add(a18_8);
                    }




                    if (datab.Count > 0 || dataa.Count > 0)
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
                    Thread.Sleep(1000);

                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);


                }

                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
                if (status == false)
                    return;
            }




        }
        #endregion


        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 进球数_Load(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"dtwi"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion
        }



    }
}
