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
using helper;

namespace 主程序202011
{
    public partial class win007 : Form
    {
        public win007()
        {
            InitializeComponent();
        }


        string type = "AsianOdds";

        public void run()
        {


            string url = "http://live.win007.com/vbsxml/bfdata_ut.js?r=0071603592223000";
            string html =method.GetUrl(url,"utf-8");
            MatchCollection aids = Regex.Matches(html, @"\]=""([\s\S]*?)\^");
            
            foreach (Match aid in aids)
            {
               
                string URL = "http://vip.win007.com/"+type+"_n.aspx?id="+aid.Groups[1].Value;
                string strhtml = method.GetUrl(URL,"gb2312");
              
                Match name = Regex.Match(strhtml, @"&t1=([\s\S]*?)&t2=([\s\S]*?)""");
                Match time = Regex.Match(strhtml, @"class=""LName"">([\s\S]*?)</a>([\s\S]*?)&");

                MatchCollection ahtmls = Regex.Matches(strhtml, @"""  >([\s\S]*?)</font></a>");

                label1.Text = name.Groups[1].Value.Trim() + "VS" + name.Groups[2].Value.Trim();

                string html1 = "";
                string html2= "";
                string html3 = "";
                foreach (Match ahtml in ahtmls)
                {
                    if (ahtml.Groups[1].Value.Contains("Bet365"))
                    {
                         html1 = ahtml.Groups[1].Value;

                    }
                    if (ahtml.Groups[1].Value.Contains("澳门"))
                    {
                         html2 = ahtml.Groups[1].Value;

                    }
                    if (ahtml.Groups[1].Value.Contains("易胜博"))
                    {
                         html3 = ahtml.Groups[1].Value;

                    }
                }



                if (html1 != "")
                {
                    MatchCollection a11 = Regex.Matches(html1, @"<td title=""([\s\S]*?)>([\s\S]*?)</td>");

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(a11[0].Groups[2].Value);
                    lv1.SubItems.Add(a11[1].Groups[2].Value);
                    lv1.SubItems.Add(a11[2].Groups[2].Value);
                    lv1.SubItems.Add(name.Groups[1].Value.Trim() + "VS" + name.Groups[2].Value.Trim());
                    lv1.SubItems.Add("Bet365");
                    lv1.SubItems.Add(time.Groups[2].Value);

                }
                if (html2 != "")
                {
                    MatchCollection a12 = Regex.Matches(html2, @"<td title=""([\s\S]*?)>([\s\S]*?)</td>");

                        ListViewItem lv2 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv2.SubItems.Add(a12[0].Groups[2].Value);
                        lv2.SubItems.Add(a12[1].Groups[2].Value);
                        lv2.SubItems.Add(a12[2].Groups[2].Value);
                        lv2.SubItems.Add(name.Groups[1].Value.Trim() + "VS" + name.Groups[2].Value.Trim());
                        lv2.SubItems.Add("澳门");
                        lv2.SubItems.Add(time.Groups[2].Value);
                }
                if (html3 != "")
                {

                    MatchCollection a13 = Regex.Matches(html3, @"<td title=""([\s\S]*?)>([\s\S]*?)</td>");

                    ListViewItem lv3 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv3.SubItems.Add(a13[0].Groups[2].Value);
                    lv3.SubItems.Add(a13[1].Groups[2].Value);
                    lv3.SubItems.Add(a13[2].Groups[2].Value);
                    lv3.SubItems.Add(name.Groups[1].Value.Trim() + "VS" + name.Groups[2].Value.Trim());
                    lv3.SubItems.Add("易胜博");
                    lv3.SubItems.Add(time.Groups[2].Value);

                }

                  
                 

                
                ListViewItem lv = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                lv.SubItems.Add("-----------------------------");
                lv.SubItems.Add("-----------------------------");
                lv.SubItems.Add("-----------------------------");
                lv.SubItems.Add("-----------------------------");
                lv.SubItems.Add("-----------------------------");
                lv.SubItems.Add("-----------------------------");
                lv.SubItems.Add("-----------------------------");



                Thread.Sleep(2000);
                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
            }

            label1.Text = "结束";
        }
        private void win007_Load(object sender, EventArgs e)
        {

        }
        bool zanting = true;
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                type = "AsianOdds";
            }

            if (radioButton2.Checked == true)
            {
                type = "OverDown";
            }



            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
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
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
