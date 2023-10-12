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
    public partial class titan007 : Form
    {
        public titan007()
        {
            InitializeComponent();
        }

        Thread thread;

        bool zanting = true;
        bool status = true;

        #region 让球/进球数
        public void run()
        {





            string html = method.GetUrl("http://live.titan007.com/vbsxml/bfdata_ut.js?r=0071657803475000", "utf-8");




            MatchCollection ids = Regex.Matches(html, @"A\[([\s\S]*?)\]=""([\s\S]*?)\^");

            foreach (Match id in ids)
            {
                string data365 = "";
                string databetathome = "";
                string dataWilliamHill = "";
                string dataLadbrokes = "";


                try
                {
                    string aurl = "https://vip.titan007.com/AsianOdds_n.aspx?id=" + id.Groups[2].Value;
                    string ahtml = method.GetUrl(aurl, "utf-8");
                    MatchCollection teams = Regex.Matches(ahtml, @"alt=""([\s\S]*?)""");
                    Match match = Regex.Match(ahtml, @"class=""LName"">([\s\S]*?)</a>([\s\S]*?)&nbsp");

                    MatchCollection dataa = Regex.Matches(ahtml, @"<tr bgcolor=""#FFFFFF""  >([\s\S]*?)</tr>");
                    MatchCollection datab = Regex.Matches(ahtml, @"<tr bgcolor=""#E8F2FF""  >([\s\S]*?)</tr>");


                    //数据部分1 匹配
                    for (int i = 0; i < dataa.Count; i++)
                    {

                        string gongsi = Regex.Match(dataa[i].Groups[1].Value, @"<td height=""25"">([\s\S]*?)<").Groups[1].Value;
                        MatchCollection a = Regex.Matches(dataa[i].Groups[1].Value, @"<td title=""([\s\S]*?)>([\s\S]*?)</td>");
                        ////随机顺序
                        //if (gongsi.Contains("澳") || gongsi.Contains("Crow") || gongsi.Contains("易") || gongsi.Contains("10") || gongsi.Contains("盈"))
                        //{

                        //    if (a.Count > 2)
                        //    {
                        //        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        //        lv1.SubItems.Add(match.Groups[1].Value.Trim());
                        //        lv1.SubItems.Add(teams[0].Groups[1].Value.Trim().Replace(" ", ""));
                        //        lv1.SubItems.Add(teams[1].Groups[1].Value.Trim());
                        //        lv1.SubItems.Add(match.Groups[2].Value.Trim());
                        //        lv1.SubItems.Add(gongsi.Replace("18*", "18bet").Replace("10*", "10bet").Replace("澳*", "澳门").Replace("Crow*", "Crown").Replace("易*", "易胜博").Replace("盈*", "盈和"));
                        //        lv1.SubItems.Add(a[0].Groups[2].Value);
                        //        lv1.SubItems.Add(a[1].Groups[2].Value);
                        //        lv1.SubItems.Add(a[2].Groups[2].Value);
                        //    }

                        //}

                        if (gongsi.Contains("36*"))
                        {
                            data365 = match.Groups[1].Value.Trim() + "#" + teams[0].Groups[1].Value.Trim().Replace(" ", "") + "#" + teams[1].Groups[1].Value.Trim() + "#" + match.Groups[2].Value.Trim() + "#" + gongsi.Replace("18*", "18bet") + "#" + a[0].Groups[2].Value + "#" + a[1].Groups[2].Value + "#" + a[2].Groups[2].Value;

                        }
                        if (gongsi.Contains("bet-at"))
                        {
                            databetathome = match.Groups[1].Value.Trim() + "#" + teams[0].Groups[1].Value.Trim().Replace(" ", "") + "#" + teams[1].Groups[1].Value.Trim() + "#" + match.Groups[2].Value.Trim() + "#" + gongsi.Replace("18*", "18bet") + "#" + a[0].Groups[2].Value + "#" + a[1].Groups[2].Value + "#" + a[2].Groups[2].Value;

                        }
                        if (gongsi.Contains("威廉希"))
                        {
                            dataWilliamHill = match.Groups[1].Value.Trim() + "#" + teams[0].Groups[1].Value.Trim().Replace(" ", "") + "#" + teams[1].Groups[1].Value.Trim() + "#" + match.Groups[2].Value.Trim() + "#" + gongsi.Replace("18*", "18bet") + "#" + a[0].Groups[2].Value + "#" + a[1].Groups[2].Value + "#" + a[2].Groups[2].Value;

                        }
                        if (gongsi.Contains("立*"))
                        {
                            dataLadbrokes = match.Groups[1].Value.Trim() + "#" + teams[0].Groups[1].Value.Trim().Replace(" ", "") + "#" + teams[1].Groups[1].Value.Trim() + "#" + match.Groups[2].Value.Trim() + "#" + gongsi.Replace("18*", "18bet") + "#" + a[0].Groups[2].Value + "#" + a[1].Groups[2].Value + "#" + a[2].Groups[2].Value;

                        }
                    }


                    //数据部分2 匹配
                    for (int i = 0; i < datab.Count; i++)
                    {

                        string gongsi = Regex.Match(datab[i].Groups[1].Value, @"<td height=""25"">([\s\S]*?)<").Groups[1].Value;
                        MatchCollection a = Regex.Matches(datab[i].Groups[1].Value, @"<td title=""([\s\S]*?)>([\s\S]*?)</td>");


                        ////随机顺序
                        //if (gongsi.Contains("澳") || gongsi.Contains("Crow") || gongsi.Contains("易") || gongsi.Contains("10") || gongsi.Contains("盈") )
                        //{
                        //    if (a.Count > 2)
                        //    {
                        //        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        //        lv1.SubItems.Add(match.Groups[1].Value.Trim());
                        //        lv1.SubItems.Add(teams[0].Groups[1].Value.Trim().Replace(" ", ""));
                        //        lv1.SubItems.Add(teams[1].Groups[1].Value.Trim());
                        //        lv1.SubItems.Add(match.Groups[2].Value.Trim());
                        //        lv1.SubItems.Add(gongsi.Replace("18*", "18bet").Replace("10*", "10bet").Replace("澳*", "澳门").Replace("Crow*", "Crown").Replace("易*", "易胜博").Replace("盈*", "盈和"));
                        //        lv1.SubItems.Add(a[0].Groups[2].Value);
                        //        lv1.SubItems.Add(a[1].Groups[2].Value);
                        //        lv1.SubItems.Add(a[2].Groups[2].Value);
                        //    }

                        //}

                        if (gongsi.Contains("36*"))
                        {
                            data365 = match.Groups[1].Value.Trim() + "#" + teams[0].Groups[1].Value.Trim().Replace(" ", "") + "#" + teams[1].Groups[1].Value.Trim() + "#" + match.Groups[2].Value.Trim() + "#" + gongsi.Replace("18*", "18bet") + "#" + a[0].Groups[2].Value + "#" + a[1].Groups[2].Value + "#" + a[2].Groups[2].Value;

                        }
                        if (gongsi.Contains("bet-at"))
                        {
                            databetathome = match.Groups[1].Value.Trim() + "#" + teams[0].Groups[1].Value.Trim().Replace(" ", "") + "#" + teams[1].Groups[1].Value.Trim() + "#" + match.Groups[2].Value.Trim() + "#" + gongsi.Replace("18*", "18bet") + "#" + a[0].Groups[2].Value + "#" + a[1].Groups[2].Value + "#" + a[2].Groups[2].Value;

                        }
                        if (gongsi.Contains("威廉希"))
                        {
                            dataWilliamHill = match.Groups[1].Value.Trim() + "#" + teams[0].Groups[1].Value.Trim().Replace(" ", "") + "#" + teams[1].Groups[1].Value.Trim() + "#" + match.Groups[2].Value.Trim() + "#" + gongsi.Replace("18*", "18bet") + "#" + a[0].Groups[2].Value + "#" + a[1].Groups[2].Value + "#" + a[2].Groups[2].Value;

                        }
                        if (gongsi.Contains("立*"))
                        {
                            dataLadbrokes = match.Groups[1].Value.Trim() + "#" + teams[0].Groups[1].Value.Trim().Replace(" ", "") + "#" + teams[1].Groups[1].Value.Trim() + "#" + match.Groups[2].Value.Trim() + "#" + gongsi.Replace("18*", "18bet") + "#" + a[0].Groups[2].Value + "#" + a[1].Groups[2].Value + "#" + a[2].Groups[2].Value;

                        }

                    }






                    if (data365.Length > 20)
                    {
                        ListViewItem lv = listView1.Items.Add(listView1.Items.Count.ToString());
                        string[] text = data365.Split(new string[] { "#" }, StringSplitOptions.None);

                        for (int i = 0; i < text.Length; i++)
                        {
                            lv.SubItems.Add(text[i]);
                        }
                    }


                    if (databetathome.Length > 20)
                    {
                        ListViewItem lv = listView1.Items.Add(listView1.Items.Count.ToString());
                        string[] text = databetathome.Split(new string[] { "#" }, StringSplitOptions.None);

                        for (int i = 0; i < text.Length; i++)
                        {
                            lv.SubItems.Add(text[i]);
                        }
                    }

                    if (dataWilliamHill.Length > 20)
                    {
                        ListViewItem lv = listView1.Items.Add(listView1.Items.Count.ToString());
                        string[] text = dataWilliamHill.Split(new string[] { "#" }, StringSplitOptions.None);

                        for (int i = 0; i < text.Length; i++)
                        {
                            lv.SubItems.Add(text[i]);
                        }
                    }

                    if (dataLadbrokes.Length > 20)
                    {
                        ListViewItem lv = listView1.Items.Add(listView1.Items.Count.ToString());
                        string[] text = dataLadbrokes.Split(new string[] { "#" }, StringSplitOptions.None);

                        for (int i = 0; i < text.Length; i++)
                        {
                            lv.SubItems.Add(text[i]);
                        }
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


        #region 胜平负即时盘
        public void run2()
        {


            try
            {
                string html = method.GetUrl("http://live.titan007.com/vbsxml/bfdata_ut.js?r=0071661759322000", "utf-8");



                MatchCollection ids = Regex.Matches(html, @"\]=""2([\s\S]*?)\^");

                for (int x = 1; x < ids.Count; x++)
                {
                    string data365 = "";
                    string databetathome = "";
                    string dataWilliamHill = "";
                    string dataLadbrokes = "";

                    string aid = "2" + ids[x].Groups[1].Value;
                    string datajsurl = "http://1x2d.titan007.com/" + aid + ".js?r=007133062314590247588";
                    string datajs = method.GetUrl(datajsurl, "gb2312");


                    string matchname_cn = Regex.Match(datajs, @"matchname_cn=""([\s\S]*?)""").Groups[1].Value;
                    string hometeam_cn = Regex.Match(datajs, @"hometeam_cn=""([\s\S]*?)""").Groups[1].Value;
                    string guestteam_cn = Regex.Match(datajs, @"guestteam_cn=""([\s\S]*?)""").Groups[1].Value;
                    string MatchTime = Regex.Match(datajs, @"MatchTime=""([\s\S]*?)""").Groups[1].Value;

                    if (MatchTime != "")
                    {
                        //MatchTime = MatchTime.Replace("-1,","-").Replace(",", "-").Replace(" ","");
                        MatchTime = MatchTime.Substring(0, 4) + "-" + MatchTime.Substring(5, 2) + "-" + MatchTime.Substring(10, 2)+" "+ MatchTime.Substring(13, 2)+":"+ MatchTime.Substring(16, 2)+":"+ MatchTime.Substring(19, 2);
                         MatchTime =Convert.ToDateTime(MatchTime).AddHours(8).ToString();    
                    }


                    //添加动态公司对应ID
                    MatchCollection gongsis = Regex.Matches(datajs, @"\d{1,5}\|\d{8,10}\|([\s\S]*?)\|");
                    for (int a = 0; a < gongsis.Count; a++)
                    {
                        string cid = Regex.Match(gongsis[a].ToString(), @"\d{8,10}").Groups[0].Value;
                        string cname = gongsis[a].Groups[1].ToString();

                        if (!gongsi_dics.ContainsKey(cid))
                        {
                            switch (cname)
                            {

                                //case "Bet 365":

                                //    gongsi_dics.Add(cid, "Bet 365");
                                //    break;
                                //case "Vcbet":
                                //    gongsi_dics.Add(cid, "Vcbet");
                                //    break;
                                case "William Hill":
                                    gongsi_dics.Add(cid, "William Hill");
                                    break;
                                //case "Interwetten":
                                //    gongsi_dics.Add(cid, "Interwetten");
                                //    break;
                                case "18Bet":
                                    gongsi_dics.Add(cid, "18Bet");
                                    break;

                            }
                        }

                    }
                    //添加动态公司对应ID结束




                    string datas = Regex.Match(datajs, @"gameDetail=Array\(([\s\S]*?)\)").Groups[1].Value;

                    string[] datastext = datas.Split(new string[] { "\",\"" }, StringSplitOptions.None);



                    for (int j = 0; j < datastext.Length; j++)
                    {

                        string cid = Regex.Match(datastext[j], @"\d{8,10}").Groups[0].Value.Trim();

                        //MessageBox.Show(cid);
                        if (gongsi_dics.ContainsKey(cid))
                        {
                            string gongsi_name = gongsi_dics[cid];

                            try
                            {

                                //MessageBox.Show(datastext[j]);

                                string[] datasresult = datastext[j].Split(new string[] { ";" }, StringSplitOptions.None);

                                string data1 = "";
                                string data2 = "";
                                string data3 = "";
                                //string data4 = "";
                                //string data5 = "";
                                //string data6 = "";
                                //string data7 = "";
                                //string data8 = "";
                                //string data9 = "";
                                try
                                {
                                    string[] data_a = datasresult[0].Split(new string[] { "|" }, StringSplitOptions.None);
                                    data1 = data_a[0].Replace(cid, "").Replace("^", "");
                                    data2 = data_a[1];
                                    data3 = data_a[2];


                                    //string[] data_b = datasresult[1].Split(new string[] { "|" }, StringSplitOptions.None);
                                    //data4 = data_b[0];
                                    //data5 = data_b[1];
                                    //data6 = data_b[2];


                                    //string[] data_c = datasresult[2].Split(new string[] { "|" }, StringSplitOptions.None);
                                    //data7 = data_c[0];
                                    //data8 = data_c[1];
                                    //data9 = data_c[2];
                                }
                                catch (Exception ex)
                                {
                                    continue;
                                   

                                }

                                if (status == false)
                                    return;











                                //if (gongsi_name== "Bet 365")
                                //{
                                //    data365 = matchname_cn + "#" +hometeam_cn + "#" + guestteam_cn+ "#" + MatchTime + "#" + gongsi_name+ "#" + data1 + "#" + data2 + "#" + data3;

                                //}
                                //if (gongsi_name == "Vcbet")
                                //{
                                //    databetathome = matchname_cn + "#" + hometeam_cn + "#" + guestteam_cn + "#" + MatchTime + "#" + "伟德" + "#" + data1 + "#" + data2 + "#" + data3;

                                //}
                                if (gongsi_name == "William Hill")
                                {
                                    dataWilliamHill = matchname_cn + "#" + hometeam_cn + "#" + guestteam_cn + "#" + MatchTime + "#" + "威廉希尔" + "#" + data1 + "#" + data2 + "#" + data3;

                                }
                                //if (gongsi_name == "Interwetten")
                                //{
                                //    dataLadbrokes = matchname_cn + "#" + hometeam_cn + "#" + guestteam_cn + "#" + MatchTime + "#" + gongsi_name + "#" + data1 + "#" + data2 + "#" + data3;

                                //}

                                if (gongsi_name == "18Bet")
                                {
                                    dataLadbrokes = matchname_cn + "#" + hometeam_cn + "#" + guestteam_cn + "#" + MatchTime + "#" + gongsi_name + "#" + data1 + "#" + data2 + "#" + data3;

                                }








                            }
                            catch (Exception ex)
                            {
                                //  MessageBox.Show(ex.ToString());
                                continue;
                            }
                        }
                    }






                    //if (data365.Length > 20)
                    //{
                    //    ListViewItem lv = listView1.Items.Add(listView1.Items.Count.ToString());
                    //    string[] text = data365.Split(new string[] { "#" }, StringSplitOptions.None);

                    //    for (int i = 0; i < text.Length; i++)
                    //    {
                    //        lv.SubItems.Add(text[i]);
                    //    }
                    //}


                    //if (databetathome.Length > 20)
                    //{
                    //    ListViewItem lv = listView1.Items.Add(listView1.Items.Count.ToString());
                    //    string[] text = databetathome.Split(new string[] { "#" }, StringSplitOptions.None);

                    //    for (int i = 0; i < text.Length; i++)
                    //    {
                    //        lv.SubItems.Add(text[i]);
                    //    }
                    //}

                    if (dataWilliamHill.Length > 20)
                    {
                        ListViewItem lv = listView1.Items.Add(listView1.Items.Count.ToString());
                        string[] text = dataWilliamHill.Split(new string[] { "#" }, StringSplitOptions.None);

                        for (int i = 0; i < text.Length; i++)
                        {
                            lv.SubItems.Add(text[i]);
                        }
                    }

                    if (dataLadbrokes.Length > 20)
                    {
                        ListViewItem lv = listView1.Items.Add(listView1.Items.Count.ToString());
                        string[] text = dataLadbrokes.Split(new string[] { "#" }, StringSplitOptions.None);

                        for (int i = 0; i < text.Length; i++)
                        {
                            lv.SubItems.Add(text[i]);
                        }
                    }



                    if (data365.Length > 20)
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

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


            MessageBox.Show("完成");
        }

        #endregion



        #region 胜平负初盘
        public void run3()
        {


            try
            {
                string html = method.GetUrl("http://live.titan007.com/vbsxml/bfdata_ut.js?r=0071661759322000", "utf-8");



                MatchCollection ids = Regex.Matches(html, @"\]=""2([\s\S]*?)\^");

                for (int x = 1; x < ids.Count; x++)
                {
                    string data18= "";
                    string dataysb = "";
                    string datacrown = "";
                    string data12 = "";
                    string datalibo = "";

                    string aid = "2" + ids[x].Groups[1].Value;
                    string datajsurl = "http://1x2d.titan007.com/" + aid + ".js?r=007133062314590247588";
                    string datajs = method.GetUrl(datajsurl, "gb2312");


                    string matchname_cn = Regex.Match(datajs, @"matchname_cn=""([\s\S]*?)""").Groups[1].Value;
                    string hometeam_cn = Regex.Match(datajs, @"hometeam_cn=""([\s\S]*?)""").Groups[1].Value;
                    string guestteam_cn = Regex.Match(datajs, @"guestteam_cn=""([\s\S]*?)""").Groups[1].Value;
                    string MatchTime = Regex.Match(datajs, @"MatchTime=""([\s\S]*?)""").Groups[1].Value;

                    if (MatchTime != "")
                    {
                        //MatchTime = MatchTime.Replace("-1,","-").Replace(",", "-").Replace(" ","");
                        MatchTime = MatchTime.Substring(0, 4) + "-" + MatchTime.Substring(5, 2) + "-" + MatchTime.Substring(10, 2) + " " + MatchTime.Substring(13, 2) + ":" + MatchTime.Substring(16, 2) + ":" + MatchTime.Substring(19, 2);
                        MatchTime = Convert.ToDateTime(MatchTime).AddHours(8).ToString();
                    }


                    //添加动态公司对应ID
                    MatchCollection gongsis = Regex.Matches(datajs, @"\d{1,5}\|\d{8,10}\|([\s\S]*?)\|");
                    for (int a = 0; a < gongsis.Count; a++)
                    {
                        string cid = Regex.Match(gongsis[a].ToString(), @"\d{8,10}").Groups[0].Value;
                        string cname = gongsis[a].Groups[1].ToString();

                        if (!gongsi_dics.ContainsKey(cid))
                        {
                            switch (cname)
                            {

                                case "18Bet":

                                    gongsi_dics.Add(cid, "18Bet");
                                    break;
                                case "12bet":
                                    gongsi_dics.Add(cid, "12bet");
                                    break;
                                case "Easybets":
                                    gongsi_dics.Add(cid, "Easybets");
                                    break;
                                case "Crown":
                                    gongsi_dics.Add(cid, "Crown");
                                    break;
                                case "Ladbrokes":
                                    gongsi_dics.Add(cid, "Ladbrokes");
                                    break;

                            }
                        }

                    }
                    //添加动态公司对应ID结束




                    string datas = Regex.Match(datajs, @"game=Array([\s\S]*?)gameDetail=Array").Groups[1].Value;

                    string[] datastext = datas.Split(new string[] { "\",\"" }, StringSplitOptions.None);



                    for (int j = 0; j < datastext.Length; j++)
                    {

                        string cid = Regex.Match(datastext[j], @"\d{8,10}").Groups[0].Value.Trim();

                        //MessageBox.Show(cid);
                        if (gongsi_dics.ContainsKey(cid))
                        {
                            string gongsi_name = gongsi_dics[cid];

                            try
                            {

                                //MessageBox.Show(datastext[j]);

                                string[] datasresult = datastext[j].Split(new string[] { ";" }, StringSplitOptions.None);

                                string data1 = "";
                                string data2 = "";
                                string data3 = "";
                                //string data4 = "";
                                //string data5 = "";
                                //string data6 = "";
                                //string data7 = "";
                                //string data8 = "";
                                //string data9 = "";
                                try
                                {
                                    string[] data_a = datasresult[0].Split(new string[] { "|" }, StringSplitOptions.None);
                                    data1 = data_a[3].Replace(cid, "").Replace("^", "");
                                    data2 = data_a[4];
                                    data3 = data_a[5];


                                    //string[] data_b = datasresult[1].Split(new string[] { "|" }, StringSplitOptions.None);
                                    //data4 = data_b[0];
                                    //data5 = data_b[1];
                                    //data6 = data_b[2];


                                    //string[] data_c = datasresult[2].Split(new string[] { "|" }, StringSplitOptions.None);
                                    //data7 = data_c[0];
                                    //data8 = data_c[1];
                                    //data9 = data_c[2];
                                }
                                catch (Exception ex)
                                {
                                    continue;


                                }

                                if (status == false)
                                    return;











                                if (gongsi_name == "18Bet")
                                {
                                    data18= matchname_cn + "#" + hometeam_cn + "#" + guestteam_cn + "#" + MatchTime + "#" + gongsi_name + "#" + data1 + "#" + data2 + "#" + data3;

                                }
                                if (gongsi_name == "Easybets")
                                {
                                    dataysb = matchname_cn + "#" + hometeam_cn + "#" + guestteam_cn + "#" + MatchTime + "#" + "易胜博"+ "#" + data1 + "#" + data2 + "#" + data3;

                                }
                                if (gongsi_name == "Crown")
                                {
                                    datacrown = matchname_cn + "#" + hometeam_cn + "#" + guestteam_cn + "#" + MatchTime + "#" + gongsi_name + "#" + data1 + "#" + data2 + "#" + data3;

                                }
                                if (gongsi_name == "12Bet")
                                {
                                    data12 = matchname_cn + "#" + hometeam_cn + "#" + guestteam_cn + "#" + MatchTime + "#" + gongsi_name + "#" + data1 + "#" + data2 + "#" + data3;

                                }

                                if (gongsi_name == "Ladbrokes")
                                {
                                    datalibo = matchname_cn + "#" + hometeam_cn + "#" + guestteam_cn + "#" + MatchTime + "#" + "立博"+ "#" + data1 + "#" + data2 + "#" + data3;

                                }








                            }
                            catch (Exception ex)
                            {
                                //  MessageBox.Show(ex.ToString());
                                continue;
                            }
                        }
                    }






                    if (data18.Length > 20)
                    {
                        ListViewItem lv = listView1.Items.Add(listView1.Items.Count.ToString());
                        string[] text = data18.Split(new string[] { "#" }, StringSplitOptions.None);

                        for (int i = 0; i < text.Length; i++)
                        {
                            lv.SubItems.Add(text[i]);
                        }
                    }


                    if (dataysb.Length > 20)
                    {
                        ListViewItem lv = listView1.Items.Add(listView1.Items.Count.ToString());
                        string[] text = dataysb.Split(new string[] { "#" }, StringSplitOptions.None);

                        for (int i = 0; i < text.Length; i++)
                        {
                            lv.SubItems.Add(text[i]);
                        }
                    }

                    if (datacrown.Length > 20)
                    {
                        ListViewItem lv = listView1.Items.Add(listView1.Items.Count.ToString());
                        string[] text = datacrown.Split(new string[] { "#" }, StringSplitOptions.None);

                        for (int i = 0; i < text.Length; i++)
                        {
                            lv.SubItems.Add(text[i]);
                        }
                    }

                    if (data12.Length > 20)
                    {
                        ListViewItem lv = listView1.Items.Add(listView1.Items.Count.ToString());
                        string[] text = data12.Split(new string[] { "#" }, StringSplitOptions.None);

                        for (int i = 0; i < text.Length; i++)
                        {
                            lv.SubItems.Add(text[i]);
                        }
                    }
                    if (datalibo.Length > 20)
                    {
                        ListViewItem lv = listView1.Items.Add(listView1.Items.Count.ToString());
                        string[] text = datalibo.Split(new string[] { "#" }, StringSplitOptions.None);

                        for (int i = 0; i < text.Length; i++)
                        {
                            lv.SubItems.Add(text[i]);
                        }
                    }


                    if (data18.Length > 20)
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

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


            MessageBox.Show("完成");
        }

        #endregion

        Dictionary<string, string> gongsi_dics = new Dictionary<string, string>();

        private void titan007_Load(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run3);
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

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void titan007_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
