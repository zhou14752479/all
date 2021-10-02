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

namespace 车辆采样消杀登记
{
    public partial class 车辆采样消杀登记 : Form
    {
        public 车辆采样消杀登记()
        {
            InitializeComponent();
        }

        function fc = new function();
     

        
        public void shaixuan(string sql)
        {
            try
            {
              
                dataGridView1.Columns.Clear();
                DataTable dt = fc.chaxundata(sql);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns["date"].HeaderText = "日期";
                dataGridView1.Columns["name"].HeaderText = "姓名";
                dataGridView1.Columns["minzu"].HeaderText = "民族";
                dataGridView1.Columns["carno"].HeaderText = "车号";
                dataGridView1.Columns["card"].HeaderText = "身份证号";
                dataGridView1.Columns["in_time"].HeaderText = "入场时间";
                dataGridView1.Columns["out_time"].HeaderText = "离场时间";
                dataGridView1.Columns["quehuo"].HeaderText = "是否缺货";
                dataGridView1.Columns["beizhu"].HeaderText = "备注";
                dataGridView1.Columns["add_date"].HeaderText = "添加时间";
                textBox1.Text = "";
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }



        public void getvalue(string value)
        {
            try
            {

                string sql = "select * from datas where card like '" + value + "' or name like '" + value + "'  ";

              
                DataTable dt = fc.chaxundata(sql);



                foreach (DataRow dr in dt.Rows)
                {
                    int index = dataGridView1.CurrentRow.Index;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {

                       
                        dataGridView1.Rows[index].Cells[i].Value = dr[i].ToString();
                    }

                    dataGridView1.Rows[index].Cells[10].Value = DateTime.Now.ToString("MM-dd");
                }


           


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "select * from datas where card like '" + textBox1.Text.Trim() + "' or name like '" + textBox1.Text.Trim() + "' ";
            if (textBox1.Text == "")
            {
                sql = "select * from datas";
            }

            shaixuan(sql);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
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
            else
            {
                
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                dataGridView1.Rows.Clear();
            }
            catch (Exception)
            {

                DataTable dt = (DataTable)dataGridView1.DataSource;
                dt.Rows.Clear();
                dataGridView1.DataSource = dt;
            }
          
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.DgvToTable(dataGridView1), "Sheet1", true);
        }
        public void inport()
        {
            try
            {


                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    try
                    {
                       
                        string date = dataGridView1.Rows[i].Cells[1].Value.ToString().Trim();
                        string name = dataGridView1.Rows[i].Cells[2].Value.ToString().Trim();
                        string minzu = dataGridView1.Rows[i].Cells[3].Value.ToString().Trim();
                        string carno = dataGridView1.Rows[i].Cells[4].Value.ToString().Trim();
                        string card = dataGridView1.Rows[i].Cells[5].Value.ToString().Trim();
                        string in_time = dataGridView1.Rows[i].Cells[6].Value.ToString().Trim();
                        string out_time = dataGridView1.Rows[i].Cells[7].Value.ToString().Trim();
                        string quehuo = dataGridView1.Rows[i].Cells[8].Value.ToString().Trim();
                        string beizhu = dataGridView1.Rows[i].Cells[9].Value.ToString().Trim();
                        string add_date = dataGridView1.Rows[i].Cells[10].Value.ToString().Trim();

                        if (date != "")
                        {
                            string sql = "INSERT INTO datas(date,name,minzu,carno,card,in_time,out_time,quehuo,beizhu,add_date)VALUES(" +
                                "'" + date + "'," +
                                "'" + name + "'," +
                                "'" + minzu + "'," +
                                "'" + carno + "'," +
                                "'" + card + "'," +
                                "'" + in_time + "'," +
                                "'" + out_time + "'," +
                                 "'" + quehuo + "'," +
                                 "'" + beizhu + "'," +
                                "'" + add_date + "')";
                            fc.insertdata(sql);
                        }
                    }
                    catch (Exception)
                    {

                     continue;
                    }
                }

                MessageBox.Show("添加成功");


            }
            catch (Exception ex)
            {

                 MessageBox.Show(ex.ToString());
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            inport();
        }



        //修改时触发事件
        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
           
            if (dataGridView1.IsCurrentCellDirty)
            {

              
            }
        }


        //修改完成触发事件
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int row = dataGridView1.CurrentRow.Index;
                int cel = dataGridView1.CurrentCell.ColumnIndex;
                string value = dataGridView1.Rows[row].Cells[cel].Value.ToString().Trim();

                getvalue(value);
            }
            catch (Exception)
            {

                ;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string sql = "select * from datas where add_date = '" + DateTime.Now.ToString("MM-dd") + "'  ";
            shaixuan(sql);
        }
    }
}
