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

namespace 思忆美团
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        Point mPoint = new Point();
        functions fc = new functions();

        #region  主程序
        public void run()

        {

            string[] citys = city_text.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            string[] cates = cate_text.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string city in citys)
            {
                string cityId = fc.GetcityId((city));

                foreach (string cateName in cates)
                {
                    string cateid = fc.catedic[cateName];
                    try
                    {

                        for (int i = 0; i < 100001; i = i + 100)

                        {

                            string Url = "http://localhost:8080/api/mt/mt_getdata.html?userid=2&cityid="+ cityId + "&cateid="+ cateid + "&page=0&timestamp=1&sign=aaa";

                            string html = method.GetUrl(Url, "utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()

                            MatchCollection names = Regex.Matches(html, @"""name"":""([\s\S]*?)""");

                            MatchCollection address = Regex.Matches(html, @"""addr"":""([\s\S]*?)""");
                            MatchCollection phone = Regex.Matches(html, @"""phone"":""([\s\S]*?)""");
                            MatchCollection waimai = Regex.Matches(html, @"""isWaimai"":([\s\S]*?),");

                            MatchCollection cate = Regex.Matches(html, @"cateName"":""([\s\S]*?)""");
                            MatchCollection area = Regex.Matches(html, @"areaName"":""([\s\S]*?)""");
                            MatchCollection shangquan = Regex.Matches(html, @"mallName"":""([\s\S]*?)""");


                            if (names.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;

                            for (int j = 0; j < names.Count; j++)
                            {

                                ListViewItem listViewItem = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                listViewItem.SubItems.Add(names[j].Groups[1].Value);
                                listViewItem.SubItems.Add(address[j].Groups[1].Value);
                                listViewItem.SubItems.Add(phone[j].Groups[1].Value);
                                listViewItem.SubItems.Add(waimai[j].Groups[1].Value);

                                listViewItem.SubItems.Add(cate[j].Groups[1].Value);
                                listViewItem.SubItems.Add(area[j].Groups[1].Value);
                                listViewItem.SubItems.Add(shangquan[j].Groups[1].Value);
                                listViewItem.SubItems.Add(city);
                                while (zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                                if (status == false)
                                    return;
                            }



                          
                            Thread.Sleep(200);
                        }
                        Application.DoEvents();
                        Thread.Sleep(1000);




                    }
                    catch (System.Exception ex)
                    {
                        ex.ToString();
                    }
                }
            }


        }



        #endregion

        private void addtask_linklabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void top_panel_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void top_panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }

        private void small_linklabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void close_linklabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MessageBox.Show("是否退出？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
              
            }
            else
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void yincang_linklabel_MouseHover(object sender, EventArgs e)
        {
            toolTip1.IsBalloon = true;
            toolTip1.SetToolTip(this.button1, "测试");
        }

        private void yincang_linklabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            this.Hide();
            notifyIcon1.Visible = true;//该控件可见
            this.ShowInTaskbar = false;//在任务栏中显示该窗口

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;//窗口正常显示
            this.ShowInTaskbar = true;//在任务栏中显示该窗口
        }

        private void 显示软件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;//窗口正常显示
            this.ShowInTaskbar = true;//在任务栏中显示该窗口
        }

        private void 开通会员ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 退出软件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否退出？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {

            }
            else
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void 隐藏软件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            notifyIcon1.Visible = true;//该控件可见
            this.ShowInTaskbar = false;//在任务栏中显示该窗口
        }

       

        private void clear_data_btn_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void exportdata_btn_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void pauseContinue_btn_Click(object sender, EventArgs e)
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

        private void stop_btn_Click(object sender, EventArgs e)
        {
            status = false;

        }
        Thread thread;
        bool zanting = true;
        bool status = true;
        private void start_btn_Click(object sender, EventArgs e)
        {
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
           
        }

        private void main_Load(object sender, EventArgs e)
        {
            fc.Getcates(comboBox1);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cate_text.Text.Contains(comboBox1.Text))
            {
                cate_text.Text += comboBox1.Text + "\r\n";
            }
            else
            {
                MessageBox.Show("已经添加："+ comboBox1.Text+"，请勿重复","重复添加提示");
            }
        }
    }
}
