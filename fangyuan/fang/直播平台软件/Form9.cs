using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fang
{
    public partial class Form9 : Form
    {
        bool status = true;

        public Form9()
        {
            InitializeComponent();
        }

        private void Form9_Load(object sender, EventArgs e)
        {

        }

     

        #region  斗鱼平台
        public void douyu()
        {

            try
            {



                for (int i = 1; i < 200; i++)
                {
                    String Url = "https://www.douyu.com/gapi/rkc/directory/0_0/"+i;

                    string strhtml = method.GetUrl(Url, "utf-8");


                    MatchCollection TitleMatchs = Regex.Matches(strhtml, @"rid"":([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in TitleMatchs)
                    {
                        lists.Add("https://www.douyu.com/" + NextMatch.Groups[1].Value);

                    }

                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;


                    foreach (string url in lists)
                    {

                        string html = method.GetUrl(url, "utf-8");



                        Match infos = Regex.Match(html, @"<p class=""column-cotent"">([\s\S]*?)</p>");
                        Match name = Regex.Match(html, @"<h1>([\s\S]*?)</h1>");

                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                        lv1.SubItems.Add(url);
                        lv1.SubItems.Add(infos.Groups[1].Value.Trim());
                        lv1.SubItems.Add(name.Groups[1].Value.Trim());

                        
                        if (listView1.Items.Count > 1)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(Convert.ToInt32(1000));

                        while (this.status == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                    }
                    

                }
            }


            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        #region  来疯平台
        public void laifeng()
        {

            try
            {



                for (int i = 1; i < 1000; i++)
                {
                    String Url = "http://www.laifeng.com/center?spm=a2h55.8996835.lf_header_nav.5~5~5~1~3!2~A&pageNo=" + i;

                    string strhtml = method.GetUrl(Url, "utf-8");


                    MatchCollection TitleMatchs = Regex.Matches(strhtml, @"<p class=""name"">([\s\S]*?)<a href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in TitleMatchs)
                    {
                        lists.Add(NextMatch.Groups[2].Value);

                    }

                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;


                    foreach (string url in lists)
                    {

                        string html = method.GetUrl(url, "utf-8");



                        Match name = Regex.Match(html, @"anchorName"": ""([\s\S]*?)""");
                        Match infos = Regex.Match(html, @"topNotify"": ""([\s\S]*?)""");


                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                        lv1.SubItems.Add(url);
                        lv1.SubItems.Add(infos.Groups[1].Value.Trim());
                        lv1.SubItems.Add(name.Groups[1].Value.Trim());
                        if (listView1.Items.Count > 1)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(Convert.ToInt32(1000));
                        while (this.status == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }


                    }
                    

                }
            }


            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

      

        private void button1_Click(object sender, EventArgs e)
        {

            this.status = true;
            if (radioButton1.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(douyu));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
            }

            else if (radioButton2.Checked == true)
            {
                Thread thread1 = new Thread(new ThreadStart(laifeng));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread1.Start();

            }

            else
            {

                MessageBox.Show("请选择平台！");
                return;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.status = true;
        }
    }
}
