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

namespace fang._2019
{
    public partial class 点评发型师 : Form
    {
        public 点评发型师()
        {
            InitializeComponent();
        }

        private void 点评发型师_Load(object sender, EventArgs e)
        {

        }

        bool zanting = true;

        ArrayList finishes = new ArrayList();
        #region  主函数
        public void run()

        {


            try

            {

                for (int i = Convert.ToInt32(textBox1.Text); i < 9999999; i++)
                {
                    if (!finishes.Contains(i))
                    {
                        finishes.Add(i);
                        label2.Text = i.ToString();

                        string html = method.GetUrl("https://m.dianping.com/technician/new/detail/inhale?source=0&technician_id=" + i, "utf-8");

                        string reviewhtml = method.GetUrl("https://m.dianping.com/technician/new/detail/allreview?technician_id=" + i, "utf-8");

                        Match liji = Regex.Match(html, @"middleText"":""([\s\S]*?)""");
                        Match reviews = Regex.Match(reviewhtml, @"reviewCount"":([\s\S]*?),");

                        if (liji.Groups[1].Value.Contains("认") && Convert.ToInt32(reviews.Groups[1].Value) >= 20)
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add("https://h5.dianping.com/app/app-page-new-technician/technician-detail.html?technician_id=" + i);

                            lv1.SubItems.Add(reviews.Groups[1].Value);
                            if (listView1.Items.Count - 1 > 1)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);
                            }

                            while (this.zanting == false)
                            {
                                Application.DoEvents();
                            }

                        }

                    }

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        private void skinButton2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                Thread thread = new Thread(new ThreadStart(run));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
            }
        }

        private void 跳转到该链接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(this.listView1.SelectedItems[0].SubItems[1].Text);
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void skinButton5_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void skinButton6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
