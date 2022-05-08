using System;
using System.Collections;
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

namespace 主程序202104
{
    public partial class 极速赛车 : Form
    {
        public 极速赛车()
        {
            InitializeComponent();
        }


      

        ArrayList lists = new ArrayList();


        #region 主程序
        public void run()
        {


            try
            {
                //http://www.daytomko.com/
                //string url = "https://backs.wlyylw.cn/lottery-client-api/races/min/10002/history?date=";
                string url = "https://api.api68.com/pks/getPksHistoryList.do?date=&lotCode=10037";
                string html = method.GetUrl(url, "utf-8");
                MessageBox.Show(html);
                string date = Regex.Match(html, @"""preDrawTime"":""([\s\S]*?)""").Groups[1].Value;
                string qishu = Regex.Match(html, @"""preDrawIssue"":""([\s\S]*?)""").Groups[1].Value;
                string haoma = Regex.Match(html, @"""preDrawCode"":""([\s\S]*?)""").Groups[1].Value;
                if (!lists.Contains(qishu))
                {
                    lists.Add(qishu);
                    string[] text = haoma.Split(new string[] { "," }, StringSplitOptions.None);
                    string v1 = text[0];
                    string v2 = text[1];
                    string v3 = text[2];
                    string v4 = text[3];
                    string v5 = text[4];
                    string v6 = text[5];
                    string v7 = text[6];
                    string v8 = text[7];
                    string v9 = text[8];
                    string v10 = text[9];

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                    lv1.SubItems.Add(qishu.Trim());
                    lv1.SubItems.Add(haoma);
                    for (int i = 0; i < 40; i++)
                    {
                        lv1.SubItems.Add(haoma_text1.Text.Trim() + haoma_text2.Text.Trim());
                    }


                    ListViewItem lv2 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv2.SubItems.Add(" ");
                    lv2.SubItems.Add(" ");


                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v1 || haoma_text2.Text.Trim() == v3) || haoma_text1.Text.Trim() == v3 || haoma_text2.Text.Trim() == v1 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v1 || haoma_text2.Text.Trim() == v6) || haoma_text1.Text.Trim() == v6 || haoma_text2.Text.Trim() == v1 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v1 || haoma_text2.Text.Trim() == v7) || haoma_text1.Text.Trim() == v7 || haoma_text2.Text.Trim() == v1 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v1 || haoma_text2.Text.Trim() == v8) || haoma_text1.Text.Trim() == v8|| haoma_text2.Text.Trim() == v1 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v1 || haoma_text2.Text.Trim() == v10) || haoma_text1.Text.Trim() == v10 || haoma_text2.Text.Trim() == v1 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v2 || haoma_text2.Text.Trim() == v5) || haoma_text1.Text.Trim() == v5 || haoma_text2.Text.Trim() == v2 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v2 || haoma_text2.Text.Trim() == v6) || haoma_text1.Text.Trim() == v6 || haoma_text2.Text.Trim() == v2 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v2 || haoma_text2.Text.Trim() == v9) || haoma_text1.Text.Trim() == v9 || haoma_text2.Text.Trim() == v2 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v3 || haoma_text2.Text.Trim() == v6) || haoma_text1.Text.Trim() == v6 || haoma_text2.Text.Trim() == v3 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v3 || haoma_text2.Text.Trim() == v7) || haoma_text1.Text.Trim() == v7 || haoma_text2.Text.Trim() == v3 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v3 || haoma_text2.Text.Trim() == v9) || haoma_text1.Text.Trim() == v9 || haoma_text2.Text.Trim() == v3 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v4 || haoma_text2.Text.Trim() == v7) || haoma_text1.Text.Trim() == v7 || haoma_text2.Text.Trim() == v4 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v4 || haoma_text2.Text.Trim() == v9) || haoma_text1.Text.Trim() == v9 || haoma_text2.Text.Trim() == v4 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v5 || haoma_text2.Text.Trim() == v8) || haoma_text1.Text.Trim() == v8 || haoma_text2.Text.Trim() == v5 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v5 || haoma_text2.Text.Trim() == v9) || haoma_text1.Text.Trim() == v9 || haoma_text2.Text.Trim() == v5 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v6 || haoma_text2.Text.Trim() == v8) || haoma_text1.Text.Trim() == v8 || haoma_text2.Text.Trim() == v6 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v6 || haoma_text2.Text.Trim() == v10) || haoma_text1.Text.Trim() == v10 || haoma_text2.Text.Trim() == v6 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v7 || haoma_text2.Text.Trim() == v9) || haoma_text1.Text.Trim() == v9 || haoma_text2.Text.Trim() == v7 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v7 || haoma_text2.Text.Trim() == v10) || haoma_text1.Text.Trim() == v10 || haoma_text2.Text.Trim() == v7 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v8 || haoma_text2.Text.Trim() == v10) || haoma_text1.Text.Trim() == v10 || haoma_text2.Text.Trim() == v8 ? "中" : "挂");


                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v1 || haoma_text2.Text.Trim() == v2) || haoma_text1.Text.Trim() == v2 || haoma_text2.Text.Trim() == v1 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v1 || haoma_text2.Text.Trim() == v4) || haoma_text1.Text.Trim() == v4 || haoma_text2.Text.Trim() == v1 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v1 || haoma_text2.Text.Trim() == v5) || haoma_text1.Text.Trim() == v5 || haoma_text2.Text.Trim() == v1 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v1 || haoma_text2.Text.Trim() == v9) || haoma_text1.Text.Trim() == v9 || haoma_text2.Text.Trim() == v1 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v2 || haoma_text2.Text.Trim() == v3) || haoma_text1.Text.Trim() == v3 || haoma_text2.Text.Trim() == v2 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v2 || haoma_text2.Text.Trim() == v4) || haoma_text1.Text.Trim() == v4 || haoma_text2.Text.Trim() == v2 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v2 || haoma_text2.Text.Trim() == v7) || haoma_text1.Text.Trim() == v7 || haoma_text2.Text.Trim() == v2 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v2 || haoma_text2.Text.Trim() == v8) || haoma_text1.Text.Trim() == v8 || haoma_text2.Text.Trim() == v2 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v2 || haoma_text2.Text.Trim() == v10) || haoma_text1.Text.Trim() == v10 || haoma_text2.Text.Trim() == v2 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v3 || haoma_text2.Text.Trim() == v4) || haoma_text1.Text.Trim() == v4 || haoma_text2.Text.Trim() == v3 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v3 || haoma_text2.Text.Trim() == v5) || haoma_text1.Text.Trim() == v5 || haoma_text2.Text.Trim() == v3 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v3 || haoma_text2.Text.Trim() == v10) || haoma_text1.Text.Trim() == v10 || haoma_text2.Text.Trim() == v3 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v4 || haoma_text2.Text.Trim() == v5) || haoma_text1.Text.Trim() == v5 || haoma_text2.Text.Trim() == v4 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v4 || haoma_text2.Text.Trim() == v6) || haoma_text1.Text.Trim() == v6 || haoma_text2.Text.Trim() == v4 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v4 || haoma_text2.Text.Trim() == v10) || haoma_text1.Text.Trim() == v10 || haoma_text2.Text.Trim() == v4 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v5 || haoma_text2.Text.Trim() == v7) || haoma_text1.Text.Trim() == v7 || haoma_text2.Text.Trim() == v5 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v5 || haoma_text2.Text.Trim() == v10) || haoma_text1.Text.Trim() == v10 || haoma_text2.Text.Trim() == v5 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v6 || haoma_text2.Text.Trim() == v9) || haoma_text1.Text.Trim() == v9 || haoma_text2.Text.Trim() == v6 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v8 || haoma_text2.Text.Trim() == v9) || haoma_text1.Text.Trim() == v9 || haoma_text2.Text.Trim() == v8 ? "中" : "挂");
                    lv2.SubItems.Add((haoma_text1.Text.Trim() == v5 || haoma_text2.Text.Trim() == v6) || haoma_text1.Text.Trim() == v6 || haoma_text2.Text.Trim() == v5 ? "中" : "挂");


               





                    haoma_text1.Text = "";
                    haoma_text2.Text = "";

                    tongji();
                }
            }
            catch (Exception ex)
            {


                //MessageBox.Show(ex.ToString());
            }

           

        }

        #endregion

        private void 极速赛车_Load(object sender, EventArgs e)
        {
            #region 通用检测


            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"k1aiu"))
            {
                
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion
            //this.Width = Screen.PrimaryScreen.Bounds.Width / 2;
            //this.Height = Screen.PrimaryScreen.Bounds.Height / 2;
            //this.Left = Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2;
            //this.Top = Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2;

            ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
            ListViewItem lv21 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
            for (int j = 0; j < 43; j++)
            {

               
                lv2.SubItems.Add(" ");
                lv21.SubItems.Add(" ");
            }


            for (int j = 3; j < 43; j++)
            {
               
                listView1.Columns[j].Width = 30;
            }

            for (int j = 3; j < 43; j++)
            {

                listView2.Columns[j].Width = 30;
            }



            listView1.Columns[7].Width = 37;
            listView1.Columns[19].Width = 37;
            listView1.Columns[21].Width = 37;
            listView1.Columns[22].Width = 37;

            listView1.Columns[31].Width = 37;
            listView1.Columns[34].Width = 37;
            listView1.Columns[37].Width = 37;
            listView1.Columns[39].Width = 37;

            listView2.Columns[7].Width = 37;
            listView2.Columns[19].Width = 37;
            listView2.Columns[21].Width = 37;
            listView2.Columns[22].Width = 37;

            listView2.Columns[31].Width = 37;
            listView2.Columns[34].Width = 37;
            listView2.Columns[37].Width = 37;
            listView2.Columns[39].Width = 37;




            listView1.Columns[3].Text =   "1-3" ;
            listView1.Columns[4].Text = "1-6";
            listView1.Columns[5].Text = "1-7";
            listView1.Columns[6].Text = "1-8";
            listView1.Columns[7].Text = "1-10";
            listView1.Columns[8].Text = "2-5";
            listView1.Columns[9].Text = "2-6";
            listView1.Columns[10].Text = "2-9";
            listView1.Columns[11].Text = "3-6";
            listView1.Columns[12].Text = "3-7";
            listView1.Columns[13].Text = "3-9";
            listView1.Columns[14].Text = "4-7";
            listView1.Columns[15].Text = "4-9";
            listView1.Columns[16].Text = "5-8";
            listView1.Columns[17].Text = "5-9";
            listView1.Columns[18].Text = "6-8";
            listView1.Columns[19].Text = "6-10";
            listView1.Columns[20].Text = "7-9";
            listView1.Columns[21].Text = "7-10";
            listView1.Columns[22].Text = "8-10";

            listView1.Columns[23].Text = "1-2";
            listView1.Columns[24].Text = "1-4";
            listView1.Columns[25].Text = "1-5";
            listView1.Columns[26].Text = "1-9";
            listView1.Columns[27].Text = "2-3";
            listView1.Columns[28].Text = "2-4";
            listView1.Columns[29].Text = "2-7";
            listView1.Columns[30].Text = "2-8";
            listView1.Columns[31].Text = "2-10";
            listView1.Columns[32].Text = "3-4";
            listView1.Columns[33].Text = "3-5";
            listView1.Columns[34].Text = "3-10";
            listView1.Columns[35].Text = "4-5";
            listView1.Columns[36].Text = "4-6";
            listView1.Columns[37].Text = "4-10";
            listView1.Columns[38].Text = "5-7";
            listView1.Columns[39].Text = "5-10";
            listView1.Columns[40].Text = "6-9";
            listView1.Columns[41].Text = "8-9";
            listView1.Columns[42].Text = "5-6";


            listView2.Columns[3].Text = "1-3";
            listView2.Columns[4].Text = "1-6";
            listView2.Columns[5].Text = "1-7";
            listView2.Columns[6].Text = "1-8";
            listView2.Columns[7].Text = "1-10";
            listView2.Columns[8].Text = "2-5";
            listView2.Columns[9].Text = "2-6";
            listView2.Columns[10].Text = "2-9";
            listView2.Columns[11].Text = "3-6";
            listView2.Columns[12].Text = "3-7";
            listView2.Columns[13].Text = "3-9";
            listView2.Columns[14].Text = "4-7";
            listView2.Columns[15].Text = "4-9";
            listView2.Columns[16].Text = "5-8";
            listView2.Columns[17].Text = "5-9";
            listView2.Columns[18].Text = "6-8";
            listView2.Columns[19].Text = "6-10";
            listView2.Columns[20].Text = "7-9";
            listView2.Columns[21].Text = "7-10";
            listView2.Columns[22].Text = "8-10";

            listView2.Columns[23].Text = "1-2";
            listView2.Columns[24].Text = "1-4";
            listView2.Columns[25].Text = "1-5";
            listView2.Columns[26].Text = "1-9";
            listView2.Columns[27].Text = "2-3";
            listView2.Columns[28].Text = "2-4";
            listView2.Columns[29].Text = "2-7";
            listView2.Columns[30].Text = "2-8";
            listView2.Columns[31].Text = "2-10";
            listView2.Columns[32].Text = "3-4";
            listView2.Columns[33].Text = "3-5";
            listView2.Columns[34].Text = "3-10";
            listView2.Columns[35].Text = "4-5";
            listView2.Columns[36].Text = "4-6";
            listView2.Columns[37].Text = "4-10";
            listView2.Columns[38].Text = "5-7";
            listView2.Columns[39].Text = "5-10";
            listView2.Columns[40].Text = "6-9";
            listView2.Columns[41].Text = "8-9";
            listView2.Columns[42].Text = "5-6";

        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            
            if ( haoma_text1.Text == "" || haoma_text2.Text == "")
            {
                MessageBox.Show("输入框为空");
                return;
            }
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            timer1.Start();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(panduan("中中挂挂中中中挂挂").ToString());
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }


        public void tongji()
        {

            try
            {
      
                for (int j = 3; j < 43; j++)
                {
                    
                    StringBuilder sb = new StringBuilder();
                    for (int i = 1; i < listView1.Items.Count; i++)
                    {
                        if (i % 2 == 1 || i==1)
                        {
                            sb.Append(listView1.Items[i].SubItems[j].Text);
                        }
                    }
                    string str = sb.ToString();
                    string[] text = panduan(str).Split(new string[] { "," }, StringSplitOptions.None);
                    listView2.Items[0].SubItems[j].Text = text[0];
                    listView2.Items[1].SubItems[j].Text = text[1];
                }
            }
            catch (Exception)
            {
                Thread.Sleep(2000);
                for (int j = 3; j < 23; j++)
                {

                    StringBuilder sb = new StringBuilder();
                    for (int i = 1; i < listView1.Items.Count; i++)
                    {
                        if (i % 2 == 1 || i == 1)
                        {
                            sb.Append(listView1.Items[i].SubItems[j].Text);
                        }
                    }
                    string str = sb.ToString();
                    string[] text = panduan(str).Split(new string[] { "," }, StringSplitOptions.None);
                    listView2.Items[0].SubItems[j].Text = text[0];
                    listView2.Items[1].SubItems[j].Text = text[1];
                }

            }
        }


        public string panduan(string str)
        {
            string value = str.Substring(str.Length-1,1);

            int cishu = 0;
            if (value == "中")
            {
                string match1 = Regex.Match(str, @"中{1}$").Groups[0].Value;
                string match2 = Regex.Match(str, @"中{2}$").Groups[0].Value;
                string match3 = Regex.Match(str, @"中{3}$").Groups[0].Value;
                string match4 = Regex.Match(str, @"中{4}$").Groups[0].Value;
                string match5 = Regex.Match(str, @"中{5}$").Groups[0].Value;
                string match6 = Regex.Match(str, @"中{6}$").Groups[0].Value;
                string match7 = Regex.Match(str, @"中{7}$").Groups[0].Value;
                string match8 = Regex.Match(str, @"中{8}$").Groups[0].Value;
                string match9 = Regex.Match(str, @"中{9}$").Groups[0].Value;
                string match10 = Regex.Match(str, @"中{10}$").Groups[0].Value;
                string match11 = Regex.Match(str, @"中{11}$").Groups[0].Value;
                string match12 = Regex.Match(str, @"中{12}$").Groups[0].Value;
                string match13 = Regex.Match(str, @"中{13}$").Groups[0].Value;
                string match14 = Regex.Match(str, @"中{14}$").Groups[0].Value;
                string match15 = Regex.Match(str, @"中{15}$").Groups[0].Value;
                string match16 = Regex.Match(str, @"中{16}$").Groups[0].Value;
                string match17 = Regex.Match(str, @"中{17}$").Groups[0].Value;
                string match18 = Regex.Match(str, @"中{18}$").Groups[0].Value;
                string match19 = Regex.Match(str, @"中{19}$").Groups[0].Value;
                string match20 = Regex.Match(str, @"中{20}$").Groups[0].Value;
                string match21 = Regex.Match(str, @"中{21}$").Groups[0].Value;
                if (match1 != "" && match2 == "")
                {
                    cishu = 1;
                }
                if (match2 != "" && match3 == "")
                {
                    cishu = 2;
                }
                if (match3 != "" && match4 == "")
                {
                    cishu = 3;
                }
                if (match4 != "" && match5 == "")
                {
                    cishu = 4;
                }
                if (match5 != "" && match6 == "")
                {
                    cishu = 5;
                }
                if (match6 != "" && match7 == "")
                {
                    cishu = 6;
                }
                if (match7 != "" && match8 == "")
                {
                    cishu = 7;
                }
                if (match8 != "" && match9 == "")
                {
                    cishu = 8;
                }
                if (match9 != "" && match10 == "")
                {
                    cishu = 9;
                }
                if (match10 != "" && match11 == "")
                {
                    cishu = 10;
                }
                if (match11 != "" && match12 == "")
                {
                    cishu = 11;
                }
                if (match12 != "" && match13 == "")
                {
                    cishu = 12;
                }
                if (match13 != "" && match14 == "")
                {
                    cishu = 13;
                }
                if (match14 != "" && match15 == "")
                {
                    cishu = 14;
                }
                if (match15 != "" && match16 == "")
                {
                    cishu = 15;
                }
                if (match16 != "" && match17 == "")
                {
                    cishu = 16;
                }
                if (match17 != "" && match18 == "")
                {
                    cishu = 17;
                }
                if (match18 != "" && match19 == "")
                {
                    cishu = 18;
                }
                if (match19 != "" && match20 == "")
                {
                    cishu = 19;
                }
                if (match20 != "" && match21 == "")
                {
                    cishu = 20;
                }
            }

            if (value == "挂")
            {
                string match1 = Regex.Match(str, @"挂{1}$").Groups[0].Value;
                string match2 = Regex.Match(str, @"挂{2}$").Groups[0].Value;
                string match3 = Regex.Match(str, @"挂{3}$").Groups[0].Value;
                string match4 = Regex.Match(str, @"挂{4}$").Groups[0].Value;
                string match5 = Regex.Match(str, @"挂{5}$").Groups[0].Value;
                string match6 = Regex.Match(str, @"挂{6}$").Groups[0].Value;
                string match7 = Regex.Match(str, @"挂{7}$").Groups[0].Value;
                string match8 = Regex.Match(str, @"挂{8}$").Groups[0].Value;
                string match9 = Regex.Match(str, @"挂{9}$").Groups[0].Value;
                string match10 = Regex.Match(str, @"挂{10}$").Groups[0].Value;
                string match11 = Regex.Match(str, @"挂{11}$").Groups[0].Value;
                string match12 = Regex.Match(str, @"挂{12}$").Groups[0].Value;
                string match13 = Regex.Match(str, @"挂{13}$").Groups[0].Value;
                string match14 = Regex.Match(str, @"挂{14}$").Groups[0].Value;
                string match15 = Regex.Match(str, @"挂{15}$").Groups[0].Value;
                string match16 = Regex.Match(str, @"挂{16}$").Groups[0].Value;
                string match17 = Regex.Match(str, @"挂{17}$").Groups[0].Value;
                string match18 = Regex.Match(str, @"挂{18}$").Groups[0].Value;
                string match19 = Regex.Match(str, @"挂{19}$").Groups[0].Value;
                string match20 = Regex.Match(str, @"挂{20}$").Groups[0].Value;
                string match21 = Regex.Match(str, @"挂{21}$").Groups[0].Value;
                if (match1 != "" && match2 == "")
                {
                    cishu = 1;
                }
                if (match2 != "" && match3 == "")
                {
                    cishu = 2;
                }
                if (match3 != "" && match4 == "")
                {
                    cishu = 3;
                }
                if (match4 != "" && match5 == "")
                {
                    cishu = 4;
                }
                if (match5 != "" && match6 == "")
                {
                    cishu = 5;
                }
                if (match6 != "" && match7 == "")
                {
                    cishu = 6;
                }
                if (match7 != "" && match8 == "")
                {
                    cishu = 7;
                }
                if (match8 != "" && match9 == "")
                {
                    cishu = 8;
                }
                if (match9 != "" && match10 == "")
                {
                    cishu = 9;
                }
                if (match10 != "" && match11 == "")
                {
                    cishu = 10;
                }
                if (match11 != "" && match12 == "")
                {
                    cishu = 11;
                }
                if (match12 != "" && match13== "")
                {
                    cishu = 12;
                }
                if (match13 != "" && match14 == "")
                {
                    cishu = 13;
                }
                if (match14 != "" && match15 == "")
                {
                    cishu = 14;
                }
                if (match15 != "" && match16 == "")
                {
                    cishu = 15;
                }
                if (match16 != "" && match17 == "")
                {
                    cishu = 16;
                }
                if (match17 != "" && match18 == "")
                {
                    cishu = 17;
                }
                if (match18 != "" && match19 == "")
                {
                    cishu = 18;
                }
                if (match19 != "" && match20 == "")
                {
                    cishu = 19;
                }
                if (match20 != "" && match21 == "")
                {
                    cishu = 20;
                }
            }
            return  value+","+cishu;
        }
        Thread thread1;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }



          
        }

        private void button4_Click(object sender, EventArgs e)
        {

          
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void 极速赛车_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
