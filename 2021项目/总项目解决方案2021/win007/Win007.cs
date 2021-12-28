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

namespace win007
{
    public partial class Win007 : Form
    {
        public Win007()
        {
            InitializeComponent();
        }


        function fc = new function();
        public void chaxun()
        {
            try
            {


                string sql = "select * from datas where";

                if (textBox1.Text.Trim() == "")
                {
                    sql = sql + (" isbn like '_%' and");
                }
                else
                {
                    sql = sql + (" isbn like '" + textBox1.Text.Trim() + "' and");
                }

                if (textBox2.Text.Trim() == "")
                {
                    sql = sql + (" name like '_%' and");
                }
                else
                {
                    sql = sql + (" name like '" + textBox2.Text.Trim() + "' and");
                }


                if (sql.Substring(sql.Length - 3, 3) == "and")
                {
                    sql = sql.Substring(0, sql.Length - 3);
                }

                DataTable dt = fc.chaxundata(sql);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns["supplyer"].HeaderText = "供应商";
                dataGridView1.Columns["name"].HeaderText = "书名";
                dataGridView1.Columns["cbs"].HeaderText = "出版社";
                dataGridView1.Columns["isbn"].HeaderText = "书号";
                dataGridView1.Columns["price"].HeaderText = "定价";
                dataGridView1.Columns["kucun"].HeaderText = "库存";
                dataGridView1.Columns["kuwei"].HeaderText = "库位";
                dataGridView1.Columns["zhekou"].HeaderText = "折扣";
                dataGridView1.Columns["dingshu"].HeaderText = "定数";
                dataGridView1.Columns["price"].Width = 60;
                dataGridView1.Columns["kucun"].Width = 60;
                dataGridView1.Columns["zhekou"].Width = 60;

                //fc.ShowDataInListView(dt, listView1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());


            }
        }


        private void Win007_Load(object sender, EventArgs e)
        {

        }
        public void run()
        {
            for (int day = 20210101; day < 20211225; day++)
            {
                label3.Text = day.ToString();
                fc.getdata(day.ToString());
            }

        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
           
        }
    }
}
