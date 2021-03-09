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
    public partial class 蔬菜网 : Form
    {
        public 蔬菜网()
        {
            InitializeComponent();
        }
        Thread thread;
        bool zanting = true;
        bool status = true;
        List<string> lists = new List<string>();
        Dictionary<string, string> dics = new Dictionary<string, string>();
        public void getcates()
        {
         

           
           string html = Regex.Match(method.GetUrl("http://www.shucai123.com/baicai/", "utf-8"), @"<div id=""me"">([\s\S]*?)</div>").Groups[1].Value;

            MatchCollection cates = Regex.Matches(html, @"<a href=""http://www.shucai123.com/([\s\S]*?)/"">([\s\S]*?)</a>");
            for (int i = 0; i < cates.Count; i++)
            {
                dics.Add(cates[i].Groups[2].Value, cates[i].Groups[1].Value);
            }
        }

         public void run()
        {

            string cate = dics[comboBox1.Text];


           
            for (int i = 1; i < 9999; i++)
            {
                
                    string url = "http://www.shucai123.com/"+cate+"/p"+i;

                    string html = method.GetUrl(url, "utf-8");

                    MatchCollection urls = Regex.Matches(html, @"<dd><p><a href=""([\s\S]*?)""");
                if (urls.Count == 0)
                    break;
                    for (int j = 0; j < urls.Count; j++)
                    {
                    string strhtml = method.GetUrl("http://www.shucai123.com" + urls[j].Groups[1].Value,"utf-8");

                   
                    Match title = Regex.Match(strhtml, @"<h1 class=""title"">([\s\S]*?)</h1>");
                    Match lxr = Regex.Match(strhtml, @"联系人：([\s\S]*?)</p>");
                    Match phone = Regex.Match(strhtml, @"<a href=""tel:([\s\S]*?)""");
                    Match addr = Regex.Match(strhtml, @"</b></p>([\s\S]*?)</p>");
                    if (!lists.Contains(phone.Groups[1].Value))
                    {
                        lists.Add(phone.Groups[1].Value);
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(Regex.Replace(title.Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(lxr.Groups[1].Value);
                        lv1.SubItems.Add(phone.Groups[1].Value);
                        label3.Text = Regex.Replace(title.Groups[1].Value, "<[^>]+>", "");
                        lv1.SubItems.Add(addr.Groups[1].Value.Replace("<p>","").Trim());
                    }
                    else
                    {
                        label3.Text ="号码重复正在跳过......";
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



        
        private void 蔬菜网_Load(object sender, EventArgs e)
        {
            getcates();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = true;
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com:8080/api/vip.html", "utf-8");

            if (!html.Contains(@"shucaiwang"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion
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
