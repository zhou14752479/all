using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 股票数据管理
{
    public partial class 股票数据管理 : Form
    {
        public 股票数据管理()
        {
            InitializeComponent();
        }


        function fc = new function();
        public void shaixuan()
        {
            try
            {


                string sql = "select * from datas";

                DataTable dt = fc.chaxundata(sql);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns["code"].HeaderText = "代码";
                dataGridView1.Columns["name"].HeaderText = "名称";
                dataGridView1.Columns["a1"].HeaderText = "昨日最新";
                dataGridView1.Columns["a2"].HeaderText = "昨日涨幅";
                dataGridView1.Columns["a3"].HeaderText = "昨日最高";
                dataGridView1.Columns["a4"].HeaderText = "昨日最低";
                dataGridView1.Columns["a5"].HeaderText = "昨日开盘";
                dataGridView1.Columns["a6"].HeaderText = "昨日昨收";
                dataGridView1.Columns["b1"].HeaderText = "今日最新";
                dataGridView1.Columns["b2"].HeaderText = "今日涨幅";
                dataGridView1.Columns["b3"].HeaderText = "今日最高";
                dataGridView1.Columns["b4"].HeaderText = "今日最低";
                dataGridView1.Columns["b5"].HeaderText = "今日开盘";
                dataGridView1.Columns["b6"].HeaderText = "今日收盘";
                dataGridView1.Columns["yuce1"].HeaderText = "今日预测高点";
                dataGridView1.Columns["yuce2"].HeaderText = "今日预测买点";

                //fc.ShowDataInListView(dt, listView1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());


            }
        }
        private void 股票数据管理_Load(object sender, EventArgs e)
        {
            #region 通用检测


            string html = function.GetUrl("http://acaiji.com/index/index/vip.html","utf-8");

            if (!html.Contains(@"MBfRd"))
            {

                System.Diagnostics.Process.GetCurrentProcess().Kill(); ;
            }

            #endregion
        }

        private void 导入数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
             openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
           // openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                string result = fc.ExcelToData(openFileDialog1.FileName);
                MessageBox.Show(result);


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            shaixuan();
            try
            {
                this.dataGridView1.Sort(this.dataGridView1.Columns["昨日涨幅"], ListSortDirection.Descending); 
            }
            catch (Exception)
            {

            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                try
                {
                    
                    dataGridView1.Rows[i].Cells[15].Value = Convert.ToDouble(dataGridView1.Rows[i].Cells[8].Value) * Convert.ToDouble(textBox1.Text);
                    dataGridView1.Rows[i].Cells[16].Value = Convert.ToDouble(dataGridView1.Rows[i].Cells[15].Value) * Convert.ToDouble(textBox2.Text);
                }
                catch (Exception)
                {

                    continue;
                }

                try
                {
                    if (Convert.ToDouble(dataGridView1.Rows[i].Cells[10].Value) > 7.5)
                    {

                        dataGridView1.Rows[i].Cells[10].Style.BackColor = Color.Red;
                    }
                }
                catch (Exception)
                {

                    continue;
                }

            }

        }


        private void 导出查询数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            function.DataTableToExcel(fc.DgvToTable(dataGridView1), "Sheet1", true);
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

        private void button2_Click(object sender, EventArgs e)
        {
            添加数据 add = new 添加数据();
            add.Show();
        }
    }
}
