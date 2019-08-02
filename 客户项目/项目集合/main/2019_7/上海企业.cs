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

namespace main._2019_7
{
    public partial class 上海企业 : Form
    {
        public 上海企业()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
            lv1.SubItems.Add(textBox1.Text);
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            webBrowser web = new webBrowser("https://zwdtuser.sh.gov.cn/uc/country/loginCountry.do?code=90000&flag=false");
            web.Show();
        }

        private void SplitContainer1_Panel1_MouseEnter(object sender, EventArgs e)
        {
            textBox2.Text = webBrowser.cookie;
        }

        public void run()
        {
            string url = "http://yct.sh.gov.cn/namedeclare/check/check_biz1?self_id=&check_items=NAME&self_name=&check_nameApp=&check_nameRegOrgan=000000&check_nameApoOrgan=000000&check_nameDistrict=%E4%B8%8A%E6%B5%B7&check_nameTrad=%E9%A5%B0&check_tradPiny=&check_nameIndDesc=%E6%9C%89&check_indPhy=&check_industryCode=&check_ifBranch=";
            string COOKIE = "JSESSIONID=rBtPJQBQXUPzbBP3d4gN8UsfsZ-nUtTIPnQA; BIGipServerGSJ-YCT-pool1=625941420.20480.0000; BIGipServerGSJ-INT-YCT-WEB=273617324.20480.0000";

            string html = method.gethtml(url,COOKIE,"utf-8");
            MatchCollection  ids = Regex.Matches(html, @"""id"":""([\s\S]*?)""");
            MatchCollection nameTrads = Regex.Matches(html, @"""nameTrad"":""([\s\S]*?)""");
            MatchCollection etpsNames = Regex.Matches(html, @"""etpsName"":""([\s\S]*?)""");

            for (int i = 0; i < ids.Count; i++)
            {
                ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
                lv2.SubItems.Add(ids[i].Groups[1].Value);
                lv2.SubItems.Add(nameTrads[i].Groups[1].Value);
                lv2.SubItems.Add(etpsNames[i].Groups[1].Value);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
          

                StringBuilder sb = new StringBuilder();
                foreach (ListViewItem item in listView2.Items)
                {
                    List<string> list = new List<string>();
                    string temp = item.SubItems[1].Text;
                    string temp1 = item.SubItems[2].Text;
                    string temp2 = item.SubItems[3].Text;
                    list.Add(temp + "-----" + temp1 + "-----" + temp2);
                    foreach (string tel in list)
                    {
                        sb.AppendLine(tel);
                    }

                  string  path = AppDomain.CurrentDomain.BaseDirectory + "data" + "\\"+DateTime.Now.ToString("yyyy-MM-dd") +".txt";
                string path1 = AppDomain.CurrentDomain.BaseDirectory + "data" + "\\总数据.txt";

                System.IO.File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
                

            }
                MessageBox.Show("导出完成");

       }


    }
}
