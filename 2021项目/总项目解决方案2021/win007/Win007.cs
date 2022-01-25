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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using myDLL;

namespace win007
{
    public partial class Win007 : Form
    {
        public Win007()
        {
            InitializeComponent();
        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        private string GetUrl(string URL)
        {
           HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = URL,
                Method = "GET",
                Accept = "*/*",
                UserAgent = "Mozilla/5.0 (Windows NT 6.2; Win64; x64; Trident/7.0; rv:11.0) like Gecko",
                Referer = "http://op1.win007.com/OddsHistory.aspx?id=100163401&sid=1962474&cid=167&l=0",
                Host = "op1.win007.com",
            };
            item.Header.Add("Accept-Encoding", "gzip");
            item.Header.Add("Accept-Language", "zh-cn,zh,en");
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            return html;
        }
        #endregion
        function fc = new function();
        public void chaxun()
        {
            try
            {
                if(textBox1.Text=="" &&textBox2.Text=="" && textBox3.Text=="")
                {
                    MessageBox.Show("请输入数值");
                    return;
                }



                string sql = "select * from datas where";

                
                if(textBox1.Text!="")
                {
                    sql = sql + (" data1 like '" + textBox1.Text.Trim() + "' and");
                }
                if (textBox2.Text != "")
                {
                    sql = sql + (" data2 like '" + textBox2.Text.Trim() + "' and");
                }
                if (textBox3.Text != "")
                {
                    sql = sql + (" data3 like '" + textBox3.Text.Trim() + "' and");
                }

                if (comboBox1.Text != "")
                {
                    sql = sql + (" gongsi like '" + comboBox1.Text.Trim() + "' and");
                }

                if (sql.Substring(sql.Length - 3, 3) == "and")
                {
                    sql = sql.Substring(0, sql.Length - 3);
                }

                DataTable dt = fc.chaxundata(sql);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns["matchname"].HeaderText = "比赛名";
                dataGridView1.Columns["zhu"].HeaderText = "主队";
                dataGridView1.Columns["ke"].HeaderText = "客队";
                dataGridView1.Columns["time"].HeaderText = "比赛时间";
                dataGridView1.Columns["gongsi"].HeaderText = "公司名";
               
                dataGridView1.Columns["bifen"].HeaderText = "比分";
          
                dataGridView1.Columns["data1"].HeaderText = "数据1";
                dataGridView1.Columns["data2"].HeaderText = "数据2";
                dataGridView1.Columns["data3"].HeaderText = "数据3";
                dataGridView1.Columns["data4"].HeaderText = "数据4";
                dataGridView1.Columns["data5"].HeaderText = "数据5";
                dataGridView1.Columns["data6"].HeaderText = "数据6";
                dataGridView1.Columns["data7"].HeaderText = "升降平";
                dataGridView1.Columns["data8"].HeaderText = "数据8";
                dataGridView1.Columns["data9"].HeaderText = "数据9";




                dataGridView1.Columns[7].HeaderCell.Style.BackColor = Color.Red;
                dataGridView1.Columns[8].HeaderCell.Style.BackColor = Color.CornflowerBlue;
                dataGridView1.Columns[9].HeaderCell.Style.BackColor = Color.Green;

                dataGridView1.Columns[10].HeaderCell.Style.BackColor = Color.Red;
                dataGridView1.Columns[11].HeaderCell.Style.BackColor = Color.CornflowerBlue;
                dataGridView1.Columns[12].HeaderCell.Style.BackColor = Color.Green;

                //this.dataGridView1.Columns[7].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                //this.dataGridView1.Columns[8].DefaultCellStyle.BackColor = System.Drawing.Color.CornflowerBlue;
                //this.dataGridView1.Columns[9].DefaultCellStyle.BackColor = System.Drawing.Color.Green;

                //this.dataGridView1.Columns[10].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                //this.dataGridView1.Columns[11].DefaultCellStyle.BackColor = System.Drawing.Color.CornflowerBlue;
                //this.dataGridView1.Columns[12].DefaultCellStyle.BackColor = System.Drawing.Color.Green;

                //this.dataGridView1.Columns[13].Width = 0;
                this.dataGridView1.Columns[14].Width = 0;
                this.dataGridView1.Columns[15].Width = 0;


                //计算

                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)//如果DataGridView中有空的数据，则提示数据输入不完整并退出添加，不包括标题行
                {
                    try
                    {
                        string value1 = dataGridView1.Rows[i].Cells[7].Value.ToString();
                        string value2 = dataGridView1.Rows[i].Cells[8].Value.ToString();
                        string value3 = dataGridView1.Rows[i].Cells[9].Value.ToString();

                        string value4 = dataGridView1.Rows[i].Cells[10].Value.ToString();
                        string value5 = dataGridView1.Rows[i].Cells[11].Value.ToString();
                        string value6 = dataGridView1.Rows[i].Cells[12].Value.ToString();

                        double cha1 = Convert.ToDouble(value1) - Convert.ToDouble(value4);
                        double cha2 = Convert.ToDouble(value2) - Convert.ToDouble(value5);
                        double cha3 = Convert.ToDouble(value3) - Convert.ToDouble(value6);

                        string sjp1 = "平";
                        string sjp2 = "平";
                        string sjp3 = "平";

                      if(cha1>0)
                        {
                            sjp1 = "升";
                        }
                        if (cha1 <0)
                        {
                            sjp1 = "降";
                        }

                        if (cha2 > 0)
                        {
                            sjp2 = "升";
                        }
                        if (cha2 < 0)
                        {
                            sjp2 = "降";
                        }
                        if (cha3> 0)
                        {
                            sjp3 = "升";
                        }
                        if (cha3 < 0)
                        {
                            sjp3 = "降";
                        }

                        dataGridView1.Rows[i].Cells[13].Value = sjp1 + sjp2 + sjp3;

                      

                        if (cha1>-0.15&&cha1<-0.1)
                        {
                            dataGridView1.Rows[i].Cells[13].Style.BackColor = Color.Yellow;
                        }
                        else if (cha1 > -2 && cha1 < -0.15)
                        {
                            dataGridView1.Rows[i].Cells[13].Style.BackColor = Color.Orange;
                        }
                       else if (cha1 < -2 )
                        {
                            dataGridView1.Rows[i].Cells[13].Style.BackColor = Color.Red;
                        }



                        if (cha2 > -0.15 && cha2 < -0.1)
                        {
                            dataGridView1.Rows[i].Cells[13].Style.BackColor = Color.Yellow;
                        }
                        else if (cha2 > -2 && cha2 < -0.15)
                        {
                            dataGridView1.Rows[i].Cells[13].Style.BackColor = Color.Orange;
                        }
                        else if (cha2 < -2)
                        {
                            dataGridView1.Rows[i].Cells[13].Style.BackColor = Color.Red;
                        }

                        if (cha3 > -0.15 && cha3 < -0.1)
                        {
                            dataGridView1.Rows[i].Cells[13].Style.BackColor = Color.Yellow;
                        }
                        else if (cha3 > -2 && cha3 < -0.15)
                        {
                            dataGridView1.Rows[i].Cells[13].Style.BackColor = Color.Orange;
                        }
                        else if (cha3 < -2)
                        {
                            dataGridView1.Rows[i].Cells[13].Style.BackColor = Color.Red;
                        }
                    }
                    catch (Exception)
                    {

                        continue;
                    }

                }

                if (textBox4.Text != "")
                {
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)//如果DataGridView中有空的数据，则提示数据输入不完整并退出添加，不包括标题行
                    {
                        try
                        {
                            string value = dataGridView1.Rows[i].Cells[13].Value.ToString();
                           
                            if (value != textBox4.Text.Trim())
                            {
                                DataGridViewRow row = dataGridView1.Rows[i];
                                 dataGridView1.Rows.Remove(row);
                                  i--; //这句是关键。。
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }


                //fc.ShowDataInListView(dt, listView1);
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.ToString());


            }
        }


        private void Win007_Load(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"hGRLg"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion
        }


        Dictionary<string, string> gongsi_dics = new Dictionary<string, string>();
        public void run()
        {
           

            for (int day = 20210101; day < 20211225; day++)
            {
                label3.Text = day.ToString();
                fc.getdata(day.ToString());
            }

        }

        string startdate = "2021-01-01";
        string enddate = "2022-01-06";
        public void getdata()
        {




            for (DateTime dt = Convert.ToDateTime(startdate); dt < Convert.ToDateTime(enddate);dt=dt.AddDays(1))
            {
                try
                {
                    string url = "http://bf.win007.com/football/Over_" + dt.ToString("yyyyMMdd") + ".htm";
                    label7.Text = dt.ToString("yyyyMMdd");

                    string html = method.GetUrl(url, "gb2312");
                    MatchCollection trs = Regex.Matches(html, @"<tr height=18([\s\S]*?)</tr>");
                
                   

                    for (int i = 0; i < trs.Count; i++)
                    {
                        if (trs[i].Groups[1].Value.Contains("display: none"))
                        {
                           // label7.Text = "不显示，跳过..";
                            continue;
                        }
                      string id = Regex.Match(trs[i].Groups[1].Value, @"showgoallist\(([\s\S]*?)\)").Groups[1].Value;

                        string bifen_zhu= Regex.Match(trs[i].Groups[1].Value, @"showgoallist([\s\S]*?)<font color=([\s\S]*?)>([\s\S]*?)</font>").Groups[3].Value;
                        string bifen_ke = Regex.Match(trs[i].Groups[1].Value, @"showgoallist([\s\S]*?)-<font color=([\s\S]*?)>([\s\S]*?)</font>").Groups[3].Value;
                        string datajsurl = "http://1x2d.win007.com/" + id+ ".js?r=007132848760362108507";
                        string datajs = method.GetUrl(datajsurl, "gb2312");
                        string datajsjs = Regex.Match(datajs, @"game=([\s\S]*?);").Groups[1].Value;

                        string matchname_cn = Regex.Match(datajs, @"matchname_cn=""([\s\S]*?)""").Groups[1].Value;
                        string hometeam_cn = Regex.Match(datajs, @"hometeam_cn=""([\s\S]*?)""").Groups[1].Value;
                        string guestteam_cn = Regex.Match(datajs, @"guestteam_cn=""([\s\S]*?)""").Groups[1].Value;
                        string MatchTime = Regex.Match(datajs, @"MatchTime=""([\s\S]*?)""").Groups[1].Value;


                        MatchCollection gongsis = Regex.Matches(datajs, @"\d{1,5}\|\d{8,10}\|([\s\S]*?)\|");
                        for (int a = 0; a < gongsis.Count; a++)
                        {
                            string cid= Regex.Match(gongsis[a].ToString(), @"\d{8,10}").Groups[0].Value;
                            string cname =gongsis[a].Groups[1].ToString();
                          
                            if (!gongsi_dics.ContainsKey(cid))
                            {
                                switch (cname)
                                {
                                    case "SNAI":
                                        gongsi_dics.Add(cid, "SNAI");
                                        break;
                                    case "Titanbet":
                                        gongsi_dics.Add(cid, "Titanbet");
                                        break;
                                    case "Bethard":
                                        gongsi_dics.Add(cid, "Bethard");
                                        break;
                                    case "ComeOn":
                                        gongsi_dics.Add(cid, "ComeOn");
                                        break;
                                    case "Intertops":
                                        gongsi_dics.Add(cid, "Intertops");
                                        break;
                                    case "Singbet":
                                        gongsi_dics.Add(cid, "Singbet");
                                        break;

                                }
                            }
                          
                        }

                     

                        string datas = Regex.Match(datajs, @"gameDetail=Array\(([\s\S]*?)\)").Groups[1].Value;
                       
                        string[] datastext = datas.Split(new string[] { "\",\"" }, StringSplitOptions.None);


                        
                        for (int j = 0; j < datastext.Length; j++)
                        {

                            string cid= Regex.Match(datastext[j], @"\d{8,10}").Groups[0].Value.Trim();
                           
                            if (gongsi_dics.ContainsKey(cid))
                            {
                                string gongsi_name = gongsi_dics[cid];
                              
                                try
                                {

                                    //ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                    //lv1.SubItems.Add(matchname_cn);
                                    //lv1.SubItems.Add(hometeam_cn);
                                    //lv1.SubItems.Add(guestteam_cn);
                                    //lv1.SubItems.Add(MatchTime);
                                    //lv1.SubItems.Add(gongsi_dics[cid]);
                                 


                                    string bifen = bifen_zhu+ "-"+ bifen_ke;
                                  
                                    //lv1.SubItems.Add(bifen);

                                    string[] datasresult = datastext[j].Split(new string[] { ";" }, StringSplitOptions.None);

                                    string data1 = "";
                                    string data2 = "";
                                    string data3 = "";
                                    string data4 = "";
                                    string data5 = "";
                                    string data6 = "";
                                    string data7 = "";
                                    string data8 = "";
                                    string data9 = "";
                                    try
                                    {
                                        string[] data_a = datasresult[0].Split(new string[] { "|" }, StringSplitOptions.None);
                                        data1 = data_a[0].Replace(cid,"").Replace("^", "");
                                        data2 = data_a[1];
                                        data3 = data_a[2];


                                        string[] data_b = datasresult[1].Split(new string[] { "|" }, StringSplitOptions.None);
                                        data4 = data_b[0];
                                        data5 = data_b[1];
                                        data6 = data_b[2];


                                        string[] data_c = datasresult[2].Split(new string[] { "|" }, StringSplitOptions.None);
                                        data7 = data_c[0];
                                        data8 = data_c[1];
                                        data9 = data_c[2];
                                    }
                                    catch (Exception)
                                    {

                                        
                                    }
                                   



                                    //lv1.SubItems.Add(data1);                             
                                    //    lv1.SubItems.Add(data2);                                 
                                    //    lv1.SubItems.Add(data3);                                
                                    //    lv1.SubItems.Add(data4);
                                    //    lv1.SubItems.Add(data5);                                     
                                    //    lv1.SubItems.Add(data6);
                                    //    lv1.SubItems.Add(data7);
                                    //    lv1.SubItems.Add(data8);
                                    //lv1.SubItems.Add(data9);

                                    fc.insertdata(matchname_cn, hometeam_cn, guestteam_cn, MatchTime, gongsi_name, bifen, data1, data2, data3, data4, data5, data6,data7,data8,data9);



                                    if (status == false)
                                        return;
                                   // Thread.Sleep(100);


                                }
                                catch (Exception ex)
                                {
                                    //  MessageBox.Show(ex.ToString());
                                    continue;
                                }
                            }
                        }


                    }
                    MessageBox.Show("完成");
                }
                catch (Exception ex)
                {

                    //MessageBox.Show(ex.ToString());
                }
            }


        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {

            chaxun();


        }

        private void 清空查询数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                listView1.Items.Clear();
                DataTable dt = (DataTable)dataGridView1.DataSource;
                dt.Rows.Clear();

                dataGridView1.DataSource = dt;
            }
            catch (Exception)
            {

               
            }
        }

        private void 导出查询数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
        bool status = true;

        private void button2_Click(object sender, EventArgs e)
        {
            startdate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            enddate = DateTime.Now.ToString("yyyy-MM-dd");
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getdata);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //startdate = DateTime.Now.AddDays(-365).ToString("yyyy-MM-dd");
            //enddate = DateTime.Now.ToString("yyyy-MM-dd");
            //status = true;
            //if (thread == null || !thread.IsAlive)
            //{
            //    thread = new Thread(getdata);
            //    thread.Start();
            //    Control.CheckForIllegalCrossThreadCalls = false;
            //}
            MessageBox.Show("全年数据已抓取");
        }

        //private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    comboBox1.Items.Clear();
        //    ArrayList lists = fc.getsupplyers();
        //    comboBox1.Items.Add("");
        //    foreach (var item in lists)
        //    {
        //        comboBox1.Items.Add(item);
        //    }
        //}

        private void button5_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void 清空数据库全部数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要清空吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                string sql = "delete from datas";
                bool status = fc.SQL(sql);
                fc.SQL("VACUUM");
                fc.SQL("DELETE FROM sqlite_sequence WHERE name = 'datas';");
                MessageBox.Show("清空成功");
            }
            else
            {
               
            }
           
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox4.Text += "升";
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox4.Text += "平";
        }

        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox4.Text += "降";
        }

        private void LinkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox4.Text = "";
        }
    }
}
