using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace titan007
{
    public partial class 欧赔初陪初盘平均值 : Form
    {
        public 欧赔初陪初盘平均值()
        {
            InitializeComponent();
        }


        Thread thread;

        bool zanting = true;
        bool status = true;


        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();    
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

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 欧赔初陪初盘平均值_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WB_DocumentCompleted);
            webBrowser1.Navigate("https://op1.titan007.com/oddslist/2379903.htm");
        }


        private void WB_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                return;
            if (e.Url.ToString() != webBrowser1.Url.ToString())
                return;
            if (webBrowser1.IsBusy == true)
                return;
            if (webBrowser1.DocumentText.Contains("</html>"))
            {

                html = webBrowser1.Document.Body.OuterHtml;
                run22();
                status = true;
            }
            else
            {
                Application.DoEvents();
            }
        }

        public string html = "";

        #region 主程序
        public void run()
        {





            string html = method.GetUrl("http://live.titan007.com/vbsxml/bfdata_ut.js?r=0071657803475000", "utf-8");




            MatchCollection ids = Regex.Matches(html, @"A\[([\s\S]*?)\]=""([\s\S]*?)\^");

            foreach (Match id in ids)
            {


                try
                {

                    string URL = "https://op1.titan007.com/oddslist/" + id.Groups[2].Value + ".htm";

                    webBrowser1.Stop();
                    webBrowser1.Navigate(URL);






                    Thread.Sleep(2000);

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

        #region 即时值和初盘平均值
        /// <summary>
        ///  即时值和初盘平均值  //浏览器默认加载 手动选择初盘，下次就会获取初盘值
        /// </summary>
        public void run22()
        {

            try
            {
                string home = Regex.Match(html, @"<div class=""home"">([\s\S]*?)title=""([\s\S]*?)""").Groups[2].Value.Replace(" ", "");
                string guest = Regex.Match(html, @"<div class=""guest"">([\s\S]*?)title=""([\s\S]*?)""").Groups[2].Value.Replace(" ", "");
                string liansai = Regex.Match(html, @"LName([\s\S]*?)>([\s\S]*?)<").Groups[2].Value.Replace(" ", "");
                string time = Regex.Match(html, @"2023-([\s\S]*?)<").Groups[1].Value.Replace(" ", "").Trim().Replace("&nbsp;", "");
                string chu = Regex.Match(html, @"初盘平均值([\s\S]*?)即时平均值").Groups[1].Value;

                string[] text = chu.Replace("<td>", "").Replace("<td class=\"rb\">", "").Replace("<td>", "").Split(new string[] { "</td>" }, StringSplitOptions.None);

                //string jishi = Regex.Match(html, @"即时平均值([\s\S]*?)</tr>").Groups[1].Value;

                //string[] text2 = jishi.Replace("<td>", "").Replace("<td class=\"rb\">", "").Replace("<span class=\"o_green\">", "").Replace("<span class=\"o_red\">", "").Replace("</span>", "").Split(new string[] { "</td>" }, StringSplitOptions.None);

                //lv1.SubItems.Add(text2[1].Trim());
                //lv1.SubItems.Add(text2[2].Trim());
                //lv1.SubItems.Add(text2[3].Trim());






                //获取公司的即时值
                // textBox1.Text = html;

                MatchCollection trs = Regex.Matches(html, @"id=""oddstr_([\s\S]*?)</tr>");


                string sheng_18 = "";
                string he_18 = "";
                string fu_18 = "";

                string sheng_libo = "";
                string he_libo = "";
                string fu_libo = "";



                string sheng_weilian = "";
                string he_weilian = "";
                string fu_weilian = "";

                for (int i = 0; i < trs.Count; i++)
                {
                    try
                    {
                        if (trs[i].Groups[1].Value.Contains("18*"))
                        {
                            MatchCollection value_18s = Regex.Matches(trs[i].Groups[1].Value, @"l=0'\)"">([\s\S]*?)</td>");
                          
                            sheng_18 = Regex.Replace(value_18s[0].Groups[1].Value, "<[^>]+>", "").Trim();
                            he_18 = Regex.Replace(value_18s[1].Groups[1].Value, "<[^>]+>", "").Trim();
                            fu_18 = Regex.Replace(value_18s[2].Groups[1].Value, "<[^>]+>", "").Trim();

                        }

                        if (trs[i].Groups[1].Value.Contains("立*(英国)"))
                        {
                            MatchCollection value_libos = Regex.Matches(trs[i].Groups[1].Value, @"l=0'\)"">([\s\S]*?)</td>");

                           // MessageBox.Show(value_libos.Count.ToString());
                            sheng_libo = Regex.Replace(value_libos[0].Groups[1].Value, "<[^>]+>", "").Trim();
                            he_libo = Regex.Replace(value_libos[1].Groups[1].Value, "<[^>]+>", "").Trim();
                            fu_libo = Regex.Replace(value_libos[2].Groups[1].Value, "<[^>]+>", "").Trim();

                        }
                       
                        if (trs[i].Groups[1].Value.Contains("威廉"))
                        {
                         
                              MatchCollection value_weilians = Regex.Matches(trs[i].Groups[1].Value, @"l=0'\)"">([\s\S]*?)</td>");
                           
                            sheng_weilian = Regex.Replace(value_weilians[0].Groups[1].Value, "<[^>]+>", "").Trim();
                            he_weilian = Regex.Replace(value_weilians[1].Groups[1].Value, "<[^>]+>", "").Trim();
                            fu_weilian = Regex.Replace(value_weilians[2].Groups[1].Value, "<[^>]+>", "").Trim();

                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.ToString());
                    }
                }

             
                //结束








                if (home != "")
                {
                    //第一行
                   if(sheng_18!="")
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(liansai);
                        lv1.SubItems.Add(home);
                        lv1.SubItems.Add(guest);
                        lv1.SubItems.Add("2023-" + time);

                        lv1.SubItems.Add("18bet");
                        lv1.SubItems.Add(sheng_18);
                        lv1.SubItems.Add(he_18);
                        lv1.SubItems.Add(fu_18);
                    }


                    //第二行
                    ListViewItem lv = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                    lv.SubItems.Add(liansai);
                    lv.SubItems.Add(home);
                    lv.SubItems.Add(guest);
                    lv.SubItems.Add("2023-" + time);
                    lv.SubItems.Add("初盘平均值");
                    lv.SubItems.Add(text[1].Trim());
                    lv.SubItems.Add(text[2].Trim());
                    lv.SubItems.Add(text[3].Trim());


                    //第三行
                    if (sheng_libo!="")
                    {
                        ListViewItem lv2 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                        lv2.SubItems.Add(liansai);
                        lv2.SubItems.Add(home);
                        lv2.SubItems.Add(guest);
                        lv2.SubItems.Add("2023-" + time);
                        lv2.SubItems.Add("立博");
                        lv2.SubItems.Add(sheng_libo);
                        lv2.SubItems.Add(he_libo);
                        lv2.SubItems.Add(fu_libo);
                    }


                    //第四行
                    if (sheng_weilian != "")
                    {
                        ListViewItem lv2 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                        lv2.SubItems.Add(liansai);
                        lv2.SubItems.Add(home);
                        lv2.SubItems.Add(guest);
                        lv2.SubItems.Add("2023-" + time);
                        lv2.SubItems.Add("威廉希尔");
                        lv2.SubItems.Add(sheng_weilian);
                        lv2.SubItems.Add(he_weilian);
                        lv2.SubItems.Add(fu_weilian);
                    }



                    ListViewItem lv3 = listView1.Items.Add(listView1.Items.Count.ToString());
                    lv3.SubItems.Add("------------");
                    lv3.SubItems.Add("------------------------");
                    lv3.SubItems.Add("------------------------");
                    lv3.SubItems.Add("------------------------");
                    lv3.SubItems.Add("------------");
                    lv3.SubItems.Add("------------");
                    lv3.SubItems.Add("------------");
                    lv3.SubItems.Add("------------");
                }


            }
            catch (Exception)
            {
                ;
            }

        }

        #endregion





        #region 初盘值和初盘平均值


        public string getchu_18(string id)
        {
            string aurl = "http://1x2d.titan007.com/" + id + ".js?r=007133062314590247588";
            string ahtml = method.GetUrl(aurl, "utf-8");
            string s = ",,";
            string data = Regex.Match(ahtml, @"18Bet([\s\S]*?)18").Groups[1].Value;


            string[] dataa = data.Split(new string[] { "|" }, StringSplitOptions.None);

            if (dataa.Length > 2)
            {
                s = dataa[1] + "," + dataa[2] + "," + dataa[3];

            }

            return s;
        }

        public string getchu_yibo(string id)
        {
            string aurl = "http://1x2d.titan007.com/" + id + ".js?r=007133062314590247588";
            string ahtml = method.GetUrl(aurl, "utf-8");
            string s = ",,";
            string data = Regex.Match(ahtml, @"Easybets([\s\S]*?)易").Groups[1].Value;


            string[] dataa = data.Split(new string[] { "|" }, StringSplitOptions.None);

            if (dataa.Length > 2)
            {
                s = dataa[1] + "," + dataa[2] + "," + dataa[3];

            }

            return s;
        }
        public string getchu_weilian(string id)
        {
            string aurl = "http://1x2d.titan007.com/" + id + ".js?r=007133062314590247588";
            string ahtml = method.GetUrl(aurl, "utf-8");
            string s = ",,";
            string data = Regex.Match(ahtml, @"William Hill([\s\S]*?)威廉").Groups[1].Value;


            string[] dataa = data.Split(new string[] { "|" }, StringSplitOptions.None);

            if (dataa.Length > 2)
            {
                s = dataa[1] + "," + dataa[2] + "," + dataa[3];

            }

            return s;
        }
        /// <summary>
        ///  初盘值和初盘平均值
        /// </summary>
        public void run33()
        {

            try
            {
                string uid = Regex.Match(html, @"ScheduleID=([\s\S]*?)&").Groups[1].Value;
                string home = Regex.Match(html, @"<div class=""home"">([\s\S]*?)title=""([\s\S]*?)""").Groups[2].Value.Replace(" ", "");
                string guest = Regex.Match(html, @"<div class=""guest"">([\s\S]*?)title=""([\s\S]*?)""").Groups[2].Value.Replace(" ", "");
                string liansai = Regex.Match(html, @"LName([\s\S]*?)>([\s\S]*?)<").Groups[2].Value.Replace(" ", "");
                string time = Regex.Match(html, @"2023-([\s\S]*?)<").Groups[1].Value.Replace(" ", "").Trim().Replace("&nbsp;", "");
                string chu = Regex.Match(html, @"初盘平均值([\s\S]*?)即时平均值").Groups[1].Value;

                string[] text = chu.Replace("<td>", "").Replace("<td class=\"rb\">", "").Replace("<td>", "").Split(new string[] { "</td>" }, StringSplitOptions.None);

                //string jishi = Regex.Match(html, @"即时平均值([\s\S]*?)</tr>").Groups[1].Value;

                //string[] text2 = jishi.Replace("<td>", "").Replace("<td class=\"rb\">", "").Replace("<span class=\"o_green\">", "").Replace("<span class=\"o_red\">", "").Replace("</span>", "").Split(new string[] { "</td>" }, StringSplitOptions.None);

                //lv1.SubItems.Add(text2[1].Trim());
                //lv1.SubItems.Add(text2[2].Trim());
                //lv1.SubItems.Add(text2[3].Trim());






                //获取公司的初盘值
                // textBox1.Text = html;

                MatchCollection trs = Regex.Matches(html, @"<tr id=""oddstr_([\s\S]*?)</tr>");


             



                string datas = getchu_18(uid);

                string[] dataa = datas.Split(new string[] { "," }, StringSplitOptions.None); 

                string sheng_18 = dataa[0];
                string he_18 = dataa[0];
                string fu_18 = dataa[0];


                string datas2 = getchu_yibo(uid);

                string[] dataa2 = datas2.Split(new string[] { "," }, StringSplitOptions.None); 
                string sheng_libo = dataa2[0];
                string he_libo = dataa2[0];
                string fu_libo = dataa2[0];

                string datas3 = getchu_weilian(uid);

                string[] dataa3 = datas3.Split(new string[] { "," }, StringSplitOptions.None); 

                string sheng_weilian = dataa3[0];
                string he_weilian = dataa3[0];
                string fu_weilian = dataa3[0];


                if (home != "")
                {
                    //第一行
                    if (sheng_18 != "")
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(liansai);
                        lv1.SubItems.Add(home);
                        lv1.SubItems.Add(guest);
                        lv1.SubItems.Add("2023-" + time);

                        lv1.SubItems.Add("18bet");
                        lv1.SubItems.Add(sheng_18);
                        lv1.SubItems.Add(he_18);
                        lv1.SubItems.Add(fu_18);
                    }


                    //第二行
                    ListViewItem lv = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                    lv.SubItems.Add(liansai);
                    lv.SubItems.Add(home);
                    lv.SubItems.Add(guest);
                    lv.SubItems.Add("2023-" + time);
                    lv.SubItems.Add("初盘平均值");
                    lv.SubItems.Add(text[1].Trim());
                    lv.SubItems.Add(text[2].Trim());
                    lv.SubItems.Add(text[3].Trim());


                    //第三行
                    if (sheng_libo != "")
                    {
                        ListViewItem lv2 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                        lv2.SubItems.Add(liansai);
                        lv2.SubItems.Add(home);
                        lv2.SubItems.Add(guest);
                        lv2.SubItems.Add("2023-" + time);
                        lv2.SubItems.Add("立博");
                        lv2.SubItems.Add(sheng_libo);
                        lv2.SubItems.Add(he_libo);
                        lv2.SubItems.Add(fu_libo);
                    }


                    //第四行
                    if (sheng_weilian != "")
                    {
                        ListViewItem lv2 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                        lv2.SubItems.Add(liansai);
                        lv2.SubItems.Add(home);
                        lv2.SubItems.Add(guest);
                        lv2.SubItems.Add("2023-" + time);
                        lv2.SubItems.Add("威廉希尔");
                        lv2.SubItems.Add(sheng_weilian);
                        lv2.SubItems.Add(he_weilian);
                        lv2.SubItems.Add(fu_weilian);
                    }



                    ListViewItem lv3 = listView1.Items.Add(listView1.Items.Count.ToString());
                    lv3.SubItems.Add("------------");
                    lv3.SubItems.Add("------------------------");
                    lv3.SubItems.Add("------------------------");
                    lv3.SubItems.Add("------------------------");
                    lv3.SubItems.Add("------------");
                    lv3.SubItems.Add("------------");
                    lv3.SubItems.Add("------------");
                    lv3.SubItems.Add("------------");
                }


            }
            catch (Exception)
            {
                ;
            }

        }

        #endregion

        private void 欧赔初陪初盘平均值_FormClosing(object sender, FormClosingEventArgs e)
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
