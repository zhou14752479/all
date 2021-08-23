using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 授权库
{
    public partial class 授权库 : Form
    {
        public 授权库()
        {
            InitializeComponent();
        }
        function fc = new function();
        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            新增 add = new 新增();
            //add.MdiParent = this;
            //add.Dock = DockStyle.Fill;
            add.Show();

        }

        public void chaxun()
        {
            try
            {
                string type = comboBox1.Text;
            
                string pinpai = textBox1.Text.Trim();
                string cate1 = textBox2.Text.Trim();
                string cate2 = textBox3.Text.Trim();
                string sq_starttime = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                string sq_endtime = dateTimePicker2.Value.ToString("yyyy-MM-dd");

                
                string sql = "SELECT * from datas  where ";
                if (comboBox1.Text == "全部授权")
                {
                    sql = sql + ("type like '_%' AND");
                }
                else
                {
                    sql = sql + ("type like '" + type + "' AND ");
                }

                if (textBox1.Text == "")
                {
                    sql = sql + (" pinpai like '_%' AND");
                }
                else
                {
                    sql = sql + ("pinpai like '" + pinpai+ "' AND ");
                }

                if (textBox2.Text == "")
                {
                    sql = sql + (" cate1 like '_%' AND ");
                }
                else
                {
                    sql = sql + ("cate1 like '" + cate1 + "' AND ");
                }

                if (textBox3.Text == "")
                {
                    sql = sql + (" cate2 like '_%' AND ");
                }
                else
                {
                    sql = sql + ("cate2 like '" + cate2+ "' AND ");
                }


                sql = sql + ("sq_starttime >= '" + sq_starttime + "' AND ");
                sql = sql + ("sq_starttime <= '" + sq_endtime + "' ");

               
                DataTable dt = fc.getdata(sql);
               fc.ShowDataInListView(dt,listView1);

            }
            catch (Exception ex)
            {

               MessageBox.Show(ex.ToString());
            }

        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(chaxun);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i<listView1.Items.Count ; i++)
            {
                listView1.Items[i].Checked = true;
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Checked = false;
            }
        }

        private void 导出数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
