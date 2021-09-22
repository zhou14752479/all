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

namespace 图书管理
{
    public partial class 图书管理 : Form
    {
        public 图书管理()
        {
            InitializeComponent();
        }

        private void 导入数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            导入数据 daoru = new 导入数据();
            daoru.Show();
         }

        function fc = new function();
        public void shaixuan()
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

                    if (textBox3.Text.Trim() == "")
                    {
                        sql = sql + (" cbs like '_%' and");
                    }
                    else
                    {
                        sql = sql + (" cbs like '" + textBox2.Text.Trim() + "' and");
                    }

                if (comboBox1.Text.Trim() == "")
                {
                    sql = sql + (" supplyer like '_%' and");
                }
                else
                {
                    sql = sql + (" supplyer = '" + comboBox1.Text.Trim() + "' and");
                }


                long cucun_start = Convert.ToInt64(numericUpDown1.Value);
                    long cucun_end = Convert.ToInt64(numericUpDown2.Value);
                    sql = sql + " kucun >= " + cucun_start + " and kucun <=" + cucun_end + " and";

                double price_start = Convert.ToDouble(numericUpDown3.Value);
                double price_end = Convert.ToDouble(numericUpDown4.Value);
                sql = sql + " price >= " + price_start + " and price <=" + price_end + " and";

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

        
        private void button1_Click(object sender, EventArgs e)
        {
            shaixuan();
           
        }

        public void chaxun()
        {
            DataTable dt = fc.chaxun(textBox5.Text);
            dataGridView2.DataSource = dt;
            dataGridView2.Columns["supplyer"].HeaderText = "供应商";
            dataGridView2.Columns["name"].HeaderText = "书名";
            dataGridView2.Columns["cbs"].HeaderText = "出版社";
            dataGridView2.Columns["isbn"].HeaderText = "书号";
            dataGridView2.Columns["price"].HeaderText = "定价";
            dataGridView2.Columns["kucun"].HeaderText = "库存";
            dataGridView2.Columns["kuwei"].HeaderText = "库位";
            dataGridView2.Columns["zhekou"].HeaderText = "折扣";
            dataGridView2.Columns["price"].Width = 60;
            dataGridView2.Columns["kucun"].Width = 60;
            dataGridView2.Columns["zhekou"].Width = 60;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void 导出查询数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
          function.DataTableToExcel(fc.DgvToTable(dataGridView1), "Sheet1", true);
            
        }

        private void dataGridView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
          
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView3);
                row.Cells[0].Value = dataGridView2.CurrentRow.Cells[0].Value.ToString();
            row.Cells[1].Value = dataGridView2.CurrentRow.Cells[1].Value.ToString();
            row.Cells[2].Value = dataGridView2.CurrentRow.Cells[2].Value.ToString();
            row.Cells[3].Value = dataGridView2.CurrentRow.Cells[3].Value.ToString();
            row.Cells[4].Value = dataGridView2.CurrentRow.Cells[4].Value.ToString();
            row.Cells[5].Value = dataGridView2.CurrentRow.Cells[5].Value.ToString();
            row.Cells[6].Value = dataGridView2.CurrentRow.Cells[6].Value.ToString();
            row.Cells[7].Value = dataGridView2.CurrentRow.Cells[7].Value.ToString();
            row.Cells[8].Value = dataGridView2.CurrentRow.Cells[8].Value.ToString();
            dataGridView3.Rows.Add(row);
           
        }

        private void 清空查询数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            dt.Rows.Clear();

            dataGridView1.DataSource = dt;
        }

     
        private void groupBox5_MouseHover(object sender, EventArgs e)
        {
            IDataObject iData = Clipboard.GetDataObject();

            if (iData.GetDataPresent(DataFormats.Text))
            {
                string data = ((String)iData.GetData(DataFormats.Text));
              
                string id = Regex.Match(data, @"\d{8,}").Groups[0].Value;
                textBox5.Text = id;
                if (fuzhivalue != id)
                {
                    textBox5.Text = id;
                    chaxun();
                    fuzhivalue = id;
                }
            }
            if (iData.GetDataPresent(DataFormats.Bitmap))
            {
                Image img = (Bitmap)iData.GetData(DataFormats.Bitmap);
            }

        }

        public string fuzhivalue = "";
        private void textBox5_MouseHover(object sender, EventArgs e)
        {
            IDataObject iData = Clipboard.GetDataObject();

            if (iData.GetDataPresent(DataFormats.Text))
            {
                string data = ((String)iData.GetData(DataFormats.Text));

                string id = Regex.Match(data, @"\d{8,}").Groups[0].Value;
                if(fuzhivalue!=id)
                {
                    textBox5.Text = id;
                    chaxun();
                    fuzhivalue = id;
                }
               
            }
            if (iData.GetDataPresent(DataFormats.Bitmap))
            {
                Image img = (Bitmap)iData.GetData(DataFormats.Bitmap);
            }
        }

        private void 导出订单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            function.DataTableToExcel(fc.DgvToTable(dataGridView3), "Sheet1", true);
        }

        private void 清空数据库全部数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sql = "delete from datas";
            bool status = fc.SQL(sql);
            fc.SQL("VACUUM");
            fc.SQL("DELETE FROM sqlite_sequence WHERE name = 'datas';");
            if (status == true)
            {
                MessageBox.Show("清空成功");
            }
        }

        private void 清空数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView3.Rows.Clear();
        }

        private void 图书管理_Load(object sender, EventArgs e)
        {
           
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            comboBox1.Items.Clear();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            comboBox1.DataSource = fc.getsupplyers();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            DataTable dt = (DataTable)dataGridView1.DataSource;

            DataSet dataset = fc.SplitDataTable(dt,1000000);
            for (int i = 0; i < dataset.Tables.Count; i++)
            {
                fc.ExportCSV(dataset.Tables[i]);
            }
           
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox3.Text)
            {
                case "折扣正序":
                    this.dataGridView1.Sort(this.dataGridView1.Columns["zhekou"], ListSortDirection.Ascending);
                    break;
                case "折扣倒序":
                    this.dataGridView1.Sort(this.dataGridView1.Columns["zhekou"], ListSortDirection.Descending);
                    break;
                case "库存正序":
                    this.dataGridView1.Sort(this.dataGridView1.Columns["kucun"], ListSortDirection.Ascending);
                    break;
                case "库存倒序":
                    this.dataGridView1.Sort(this.dataGridView1.Columns["kucun"], ListSortDirection.Descending);
                    break;
            }
        }
    }
}
