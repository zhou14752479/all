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
    public partial class 阿里采购 : Form
    {
        public 阿里采购()
        {
            InitializeComponent();
        }


        public void run()
        {
            string[] text = {"660101","1513","3308","660103","660104","3306","100000615","660201", "660102", "660302", "127720001", "660105", "201195004", "100000618", "3305", "100000616", "100000617", "201268399", "201337614", "201330820" };

            foreach (string aid in text)
            {
                for (int page = 1; page < 101; page++)
                {


                    string url = "https://sourcing.alibaba.com/rfq_search_list.htm?categoryIds="+aid+"&page=" + page;
                    string strhtml = method.GetUrl(url, "utf-8");
                  
                    MatchCollection titles = Regex.Matches(strhtml, @"subject: ""([\s\S]*?)""");

                    MatchCollection quantitys = Regex.Matches(strhtml, @"quantity:([\s\S]*?),");
                    MatchCollection countrys = Regex.Matches(strhtml, @"country:""([\s\S]*?)""");
                    MatchCollection times = Regex.Matches(strhtml, @"openTimeStr:""([\s\S]*?)""");
                    MatchCollection buyerNames = Regex.Matches(strhtml, @"buyerName: '([\s\S]*?)'");
                    MatchCollection counts = Regex.Matches(strhtml, @"rfqLeftCount:parseInt\(""([\s\S]*?)""");
                    MatchCollection imageurls = Regex.Matches(strhtml, @"imageUrl:""([\s\S]*?)""");

                    if (titles.Count == 0)
                        break;


                    for (int i = 0; i < titles.Count; i++)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(method.Unicode2String(titles[i].Groups[1].Value).Replace("\\x20"," "));

                        lv1.SubItems.Add(method.Unicode2String(quantitys[i].Groups[1].Value));
                        lv1.SubItems.Add(method.Unicode2String(countrys[i].Groups[1].Value));
                        lv1.SubItems.Add(method.Unicode2String(times[i].Groups[1].Value));
                        lv1.SubItems.Add(method.Unicode2String(buyerNames[i].Groups[1].Value).Replace("\\x20", " "));
                        lv1.SubItems.Add(method.Unicode2String(counts[i].Groups[1].Value));
                        lv1.SubItems.Add("https:"+method.Unicode2String(imageurls[i].Groups[1].Value));
                        lv1.SubItems.Add(aid);

                    }

                    Thread.Sleep(1000);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                }
            }

            
        }

        Thread thread;
        bool zanting = true;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
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

        private void 阿里采购_Load(object sender, EventArgs e)
        {

        }
    }
}
