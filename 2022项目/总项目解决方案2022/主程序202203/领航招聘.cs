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

namespace 主程序202203
{
    public partial class 领航招聘 : Form
    {
        public 领航招聘()
        {
            InitializeComponent();
        }
        Thread thread;
        bool zanting = true;
        bool status = true;
        #region 主程序
        public void run()
        {

          

            for (int page = 0; page < 9999; page = page + 15)
            {

                string url = "http://www.leadzp.com/main/newRecruit?pager.offset=" + page;

                string html = method.GetUrl(url, "utf-8");

                MatchCollection ids = Regex.Matches(html, @"showComInfo/([\s\S]*?)""");

              
                if (ids.Count == 0)
                {
                    break;
                }
                for (int a = 0; a < ids.Count; a++)
                {
                    try
                    {
                        string aurl = "http://www.leadzp.com/hryun/showComInfo/" + ids[a].Groups[1].Value;
                        string ahtml = method.GetUrl(aurl, "utf-8");
                      
                        string companyname = Regex.Match(ahtml, @"<div class=""com_name"">([\s\S]*?)</div>").Groups[1].Value;
                        string tel = Regex.Match(ahtml, @"联系电话：<label>([\s\S]*?)</label>").Groups[1].Value;
                        string name = Regex.Match(ahtml, @"联系人：<label>([\s\S]*?)</label>").Groups[1].Value;
                        string addr = Regex.Match(ahtml, @"联系地址：([\s\S]*?)<").Groups[1].Value;
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                        lv1.SubItems.Add(companyname);
                        lv1.SubItems.Add(tel.Trim());
                        lv1.SubItems.Add(name.Trim());
                        lv1.SubItems.Add(addr.Trim());


                        Thread.Sleep(100);
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                    }
                    catch (Exception)
                    {

                        continue;
                    }

                }




            }


        }

        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            if (DateTime.Now > Convert.ToDateTime("2022-09-08"))
            {
                return;
            }

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

        private void button6_Click(object sender, EventArgs e)
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

        private void 领航招聘_Load(object sender, EventArgs e)
        {

        }
    }
}
