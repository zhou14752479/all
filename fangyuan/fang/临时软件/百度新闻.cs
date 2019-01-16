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

namespace fang.临时软件
{
    public partial class 百度新闻 : Form
    {
        public 百度新闻()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        ArrayList titles = new ArrayList();

        bool zanting = true;
        #region  主函数
        public void baidu()

        {


            try

            {

                for (int i = 1230739200; i < 1547654399; i = i + 86400)
                {
                    int atime = i;
                    int btime = i + 86399;

                    for (int j = 1; j < 50; j++)
                    {
                        string url = "http://news.baidu.com/ns?tn=newstitle&word=%E6%88%BF%E4%BB%B7&pn=" + j + "0&ct=1&tn=news&rn=20&ie=utf-8&bt=" + atime + "&et=" + btime;
                        textBox1.Text = url;
                        string html = method.GetUrl( url,"utf-8");


                        MatchCollection biaotis = Regex.Matches(html, @"<h3 class=""c-title"">([\s\S]*?)</h3>");

                        if (biaotis.Count == 0)
                            break;

                        MatchCollection times = Regex.Matches(html, @"<div class=""c-title-author"">([\s\S]*?)20([\s\S]*?)<");


                        for (int z = 0; z < biaotis.Count; z++)
                        {
                            string value = Regex.Replace(biaotis[z].Groups[1].Value, "<[^>]*>", "");
                            if (!titles.Contains(value))
                            {
                                
                                titles.Add(value);
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                                lv1.SubItems.Add(value);
                                lv1.SubItems.Add("20"+times[z].Groups[2].Value.ToString());
                                lv1.SubItems.Add(url);
                                if (listView1.Items.Count - 1 > 0)
                                {
                                    listView1.EnsureVisible(listView1.Items.Count - 1);
                                }
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }

                            }

                            
                        }

                    }

                }
            }

            catch (Exception ex)
            {
              MessageBox.Show(  ex.ToString());
            }
        }

        #endregion

        private void skinButton4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                Thread thread = new Thread(new ThreadStart(baidu));
                thread.Start();
            }
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            this.zanting = false;
        }

        private void skinButton5_Click(object sender, EventArgs e)
        {
            this.zanting = true;
        }
    }
}
