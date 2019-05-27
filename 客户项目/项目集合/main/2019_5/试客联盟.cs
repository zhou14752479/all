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

namespace main._2019_5
{
    public partial class 试客联盟 : Form
    {
        public 试客联盟()
        {
            InitializeComponent();
        }

        private void 试客联盟_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        public static string COOKIE = "";

        bool status = true;
        bool zanting = true;
        #region 
        public void run()
        {
            COOKIE = webBrowser.cookie;
            try
            {

                for (int i = Convert.ToInt32(textBox1.Text)*6; i <= Convert.ToInt32(textBox2.Text) * 6; i = i + 6)
                {

                    string Url = "http://user.shikee.com/seller/report/rlist/" + i + "?title=&uname=&orderno=&state=0";

                    string html = method.GetUrlWithCookie(Url, COOKIE,"utf-8");

                    MatchCollection names = Regex.Matches(html, @"data-name=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection dingdans = Regex.Matches(html, @"uid([\s\S]*?)</a></td>([\s\S]*?)<td>([\s\S]*?)</td>([\s\S]*?)<td>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    
                    for (int j = 0; j < names.Count; j++)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(names[j].Groups[1].Value.Trim());
                        lv1.SubItems.Add(dingdans[j].Groups[3].Value.Trim());
                        lv1.SubItems.Add(dingdans[j].Groups[5].Value.Trim());

                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        label1.Text = "当前正在抓取第"+i/6+"页";

                        if (this.status == false)
                            return;
                    }

                }

                Thread.Sleep(500);



            }
           



            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }


        #endregion

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            webBrowser web = new webBrowser("http://login.shikee.com/");
            web.Show();
        }
    }
}
