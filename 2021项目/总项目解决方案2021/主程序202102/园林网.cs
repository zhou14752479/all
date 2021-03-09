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

namespace 主程序202102
{
    public partial class 园林网 : Form
    {
        public 园林网()
        {
            InitializeComponent();
        }
        Thread thread;
        bool zanting = true;
        bool status = true;
        List<string> lists = new List<string>();
        Dictionary<string, string> dics = new Dictionary<string, string>();
      

        public void run()
        {

            for (int i = 1; i < 30000; i++)
            {

                string url = "https://www.yuanlin.com/b2b/sell_"+i+".html";

                string html = method.GetUrl(url, "utf-8");

                MatchCollection urls = Regex.Matches(html, @"<div class=""item_title td"">([\s\S]*?)<h2><a href=""([\s\S]*?)""");
                if (urls.Count == 0)
                    break;
                for (int j = 0; j < urls.Count; j++)
                {
                    
                    string strhtml = method.GetUrl("https://www.yuanlin.com" + urls[j].Groups[2].Value, "utf-8");


                    Match title = Regex.Match(strhtml, @"<h1>([\s\S]*?)</h1>");
                    Match lxr = Regex.Match(strhtml, @"联系人：</span>([\s\S]*?)<");
                    Match phone = Regex.Match(strhtml, @"机：</span>([\s\S]*?)</li>");
                    Match addr = Regex.Match(strhtml, @"址：</span>([\s\S]*?)</li>");
                    if (!lists.Contains(phone.Groups[1].Value))
                    {
                        if (checkBox1.Checked == true)
                        {
                            lists.Add(phone.Groups[1].Value);

                        }
                        
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(Regex.Replace(title.Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(lxr.Groups[1].Value.Trim());
                        lv1.SubItems.Add(phone.Groups[1].Value);
                        label3.Text = Regex.Replace(title.Groups[1].Value, "<[^>]+>", "");
                        lv1.SubItems.Add(addr.Groups[1].Value.Replace("<p>", "").Trim());
                    }
                    else
                    {
                        label3.Text = "号码重复正在跳过......";
                    }
                    Thread.Sleep(100);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;
                }
                Thread.Sleep(1000);

            }
        }



        public void run1()
        {

            for (int i = 1; i < 5165; i++)
            {

                string url = "https://www.yuanlin.com/114/SearchView.html?ShowMode=0&Province=&City=&ClassName=&KeyWord=&PageIndex="+i;

                string html = method.GetUrl(url, "utf-8");

              

                MatchCollection titles = Regex.Matches(html, @"<td width=""80%"">([\s\S]*?)_blank"">([\s\S]*?) </a>");
                MatchCollection lxrs = Regex.Matches(html, @"联系人：([\s\S]*?)<");
                MatchCollection phones = Regex.Matches(html, @"手 机：([\s\S]*?)<");
                MatchCollection addrs = Regex.Matches(html, @"地 址：([\s\S]*?)<");

                if (titles.Count == 0)
                    break;
                for (int j = 0; j < titles.Count; j++)
                {


               
                    if (!lists.Contains(phones[j].Groups[1].Value))
                    {
                        if (checkBox1.Checked == true)
                        {
                            lists.Add(phones[j].Groups[1].Value);

                        }

                        try
                        {
                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(Regex.Replace(titles[j].Groups[2].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(lxrs[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(phones[j].Groups[1].Value.Trim());
                            label3.Text = Regex.Replace(titles[j].Groups[2].Value, "<[^>]+>", "").Trim();
                            lv1.SubItems.Add(addrs[j].Groups[1].Value.Replace("<p>", "").Trim());
                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                           continue;
                        }
                    }
                    else
                    {
                        label3.Text = "号码重复正在跳过......";
                    }
                   
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;
                }
                Thread.Sleep(1000);

            }
        }
        private void 园林网_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = true;
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"shucaiwang"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run1);
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

        private void button6_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
