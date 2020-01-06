using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main
{
    public partial class 贝壳网 : Form
    {
        public 贝壳网()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        [DllImport("Kernel32.dll")] //引入命名空间 using System.Runtime.InteropServices;  
        public static extern bool Beep(int frequency, int duration);// 第一个参数是指频率的高低，越大越高，第二个参数是指响的时间多
        public bool zanting = true;
        public void run()
        {
            try
            {


                for (int i = 1; i < 100; i++)
                {
                    string url = textBox1.Text + "pg"+i+"/";
                    Match city = Regex.Match(url, @"//([\s\S]*?)\.");

                    string html = method.GetHtmlSource(url);

                    MatchCollection matches = Regex.Matches(html, @"&resblock_id=([\s\S]*?)&");
                    MatchCollection junjias = Regex.Matches(html, @"<div class=""totalPrice""><span>([\s\S]*?)</span>");

                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in matches)
                    {
                        lists.Add("https://"+city.Groups[1].Value+".ke.com/ershoufang/co41c"+ NextMatch .Groups[1].Value+ "/");

                    }

                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;

                    for (int j = 0; j < lists.Count; j++)
                    {

                        string strhtml = method.GetHtmlSource(lists[j].ToString());
                        Match xiaoqu = Regex.Match(strhtml, @"<span class=""name"">([\s\S]*?)</span>");
                        MatchCollection zuidi = Regex.Matches(strhtml, @"data-price=""([\s\S]*?)""");  //最低价次低价

                        Match zaishou = Regex.Match(strhtml, @"count: ([\s\S]*?),");

                        if (zuidi.Count > 2)
                        {

                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(xiaoqu.Groups[1].Value);
                            lv1.SubItems.Add(zuidi[0].Groups[1].Value);
                            lv1.SubItems.Add(zuidi[1].Groups[1].Value);
                            lv1.SubItems.Add(zuidi[2].Groups[1].Value);
                            lv1.SubItems.Add(junjias[j].Groups[1].Value);
                            lv1.SubItems.Add(zaishou.Groups[1].Value);





                  

                            Double a = Convert.ToDouble(zuidi[0].Groups[1].Value);
                            Double b = Convert.ToDouble(junjias[j].Groups[1].Value);
                           


                            if (a < b * Convert.ToDouble(textBox2.Text))
                            {
                                textBox3.Text += "ID" + listView1.Items.Count.ToString() + xiaoqu.Groups[1].Value + "出现报警信息" + "\r\n";
                                Beep(500, 700);
                                listView1.Items[listView1.Items.Count - 1].BackColor = Color.Red;
                            }


                         

                            if (listView1.Items.Count - 1 > 1)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);
                            }
                        }
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        Thread.Sleep(500);
                    }



                }


            }
            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void 贝壳网_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
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

        private void Button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
