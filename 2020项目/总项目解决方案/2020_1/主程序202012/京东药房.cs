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

namespace 主程序202012
{
    public partial class 京东药房 : Form
    {
        public 京东药房()
        {
            InitializeComponent();
        }

        private void 京东药房_Load(object sender, EventArgs e)
        {

        }

        public void jd()
        {
          
                for (int page = 1; page < 101; page++)
                {


                    string url = "https://mall.jd.com/advance_search-612358-1000015441-1000015441-0-0-0-1-"+page+"-60.html?other=";
                    string html = method.GetUrl(url, "utf-8");

                    MatchCollection ids = Regex.Matches(html, @"jdprice='([\s\S]*?)'");
               
                if (ids.Count == 0)
                        break;


                    for (int i = 0; i < ids.Count; i++)
                    {
                    string aurl = "https://item-soa.jd.com/getWareBusiness?callback=jQuery8715480&skuId="+ids[i].Groups[1].Value;
                    string ahtml = method.GetUrl(aurl, "utf-8");
                    string burl = "https://item.yiyaojd.com/" + ids[i].Groups[1].Value+".html";
                    string bhtml = method.GetUrl(burl, "utf-8");
                    Match title = Regex.Match(bhtml, @" name: '([\s\S]*?)'");
                  
                    Match price= Regex.Match(ahtml, @"""p"":""([\s\S]*?)""");
                    Match guige = Regex.Match(bhtml, @"产品规格</dt><dd>([\s\S]*?)</dd>");
                    Match company = Regex.Match(bhtml, @"生产企业</dt><dd>([\s\S]*?)</dd>");

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                    textBox3.Text += "正在抓取："+ title.Groups[1].Value + "\r\n";
                    if (textBox3.Text.Length > 10000)
                    {
                        textBox3.Text = "";
                    }
                        lv1.SubItems.Add(title.Groups[1].Value);
              
                    lv1.SubItems.Add(price.Groups[1].Value);
                    lv1.SubItems.Add(guige.Groups[1].Value);
                    lv1.SubItems.Add(company.Groups[1].Value);
                    if (status == false)
                        return;

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                }
               
                }
            


        }


        Thread thread;
        bool zanting = true;
        bool status = true;
        private void button1_Click(object sender, EventArgs e)
        {
            //method.ReadFromExcelFile(@"C:\Users\zhou\Desktop\TEST2 不翻译.xlsx",listView1);
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(jd);
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

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
