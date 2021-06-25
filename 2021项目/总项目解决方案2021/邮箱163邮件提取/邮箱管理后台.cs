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

namespace 邮箱163邮件提取
{
    public partial class 邮箱管理后台 : Form
    {
        public 邮箱管理后台()
        {
            InitializeComponent();
        }


        public void getdataall()
        {
            listView1.Items.Clear();
            string html = method.GetUrl("http://www.acaiji.com:8080/api/mt/mtjianliall.html", "utf-8");
            MatchCollection names = Regex.Matches(html, @"""name"": ""([\s\S]*?)""");
            MatchCollection sexs = Regex.Matches(html, @"""sex"": ""([\s\S]*?)""");
            MatchCollection ages = Regex.Matches(html, @"""age"": ""([\s\S]*?)""");
            MatchCollection birthdays = Regex.Matches(html, @"""birthday"": ""([\s\S]*?)""");
            MatchCollection phones = Regex.Matches(html, @"""phone"": ""([\s\S]*?)""");
            MatchCollection areas = Regex.Matches(html, @"""area"": ""([\s\S]*?)""");
            MatchCollection jobs = Regex.Matches(html, @"""job"": ""([\s\S]*?)""");
            MatchCollection times = Regex.Matches(html, @"""time"": ""([\s\S]*?)""");
            MatchCollection usernames = Regex.Matches(html, @"""username"": ""([\s\S]*?)""");


          
            for (int i = 0; i < names.Count; i++)
            {
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                lv1.SubItems.Add(System.Web.HttpUtility.UrlDecode(names[i].Groups[1].Value));
                lv1.SubItems.Add(System.Web.HttpUtility.UrlDecode(sexs[i].Groups[1].Value));
                lv1.SubItems.Add(System.Web.HttpUtility.UrlDecode(ages[i].Groups[1].Value));
                lv1.SubItems.Add(birthdays[i].Groups[1].Value);
                lv1.SubItems.Add(phones[i].Groups[1].Value);
                lv1.SubItems.Add(System.Web.HttpUtility.UrlDecode(areas[i].Groups[1].Value));
                lv1.SubItems.Add(System.Web.HttpUtility.UrlDecode(jobs[i].Groups[1].Value));
                lv1.SubItems.Add(times[i].Groups[1].Value);
                lv1.SubItems.Add(System.Web.HttpUtility.UrlDecode(usernames[i].Groups[1].Value));


            }
        }

        Thread thread;
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode.Text == "数据列表")
            {
                tabControl1.SelectedIndex = 0;
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(getdataall);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }

            if (treeView1.SelectedNode.Text == "用户列表")
            {
                tabControl1.SelectedIndex =1;
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(getdataall);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }

            if (treeView1.SelectedNode.Text == "导出数据")
            {
                method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
            }
        }

        private void 邮箱管理后台_Load(object sender, EventArgs e)
        {

        }
    }
}
