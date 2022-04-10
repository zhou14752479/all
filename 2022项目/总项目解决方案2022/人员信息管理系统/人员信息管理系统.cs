using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace 人员信息管理系统
{
    public partial class 人员信息管理系统 : Form
    {
        public 人员信息管理系统()
        {
            InitializeComponent();
        }
        function fc = new function();
       

      
        
       

        private void 导入数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            添加数据 add = new 添加数据();
            add.Show();
        }

        private void 添加字段ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            字段设置 set = new 字段设置("添加字段");
            set.Show();
        }

        private void 修改字段ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            字段设置 set = new 字段设置("修改字段");
            set.Show();
        }

        private void 开通子账号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(function.bumenid=="1")
            {
                添加子账号 add = new 添加子账号();
                add.Show();
            }
            else
            {
                MessageBox.Show("对不起，您没有权限增加子账号，请联系管理员添加");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
               
                DataTable dt = (DataTable)dataGridView1.DataSource;
                dt.Rows.Clear();

                dataGridView1.DataSource = dt;
            }
            catch (Exception)
            {
                dataGridView1.Rows.Clear();

            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getcount);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            toolStripStatusLabel1.Text = "开始查询数据---->";
            chaxun();
            toolStripStatusLabel1.Text = "查询数据成功---->";
        }


    
public void getcount()
        {
            try
            {
                string count = "";
                string sql = "SELECT COUNT(*) FROM datas";
                MySqlConnection mycon = new MySqlConnection(function.conn);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand(sql, mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                if (reader.Read())
                {
                    count = reader[0].ToString().Trim();

                }
                mycon.Close();
                reader.Close();
               
                MySqlConnection mycon2 = new MySqlConnection(function.conn);
                mycon2.Open();
                string sql2 = "SELECT COUNT(*) FROM datas group by hucode";
                MySqlCommand cmd2 = new MySqlCommand(sql2, mycon2);
                MySqlDataReader reader2 = cmd2.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源
                int hucount = 0;
               while(reader2.Read())
                {
                    hucount = hucount+1;
                    
                }
                mycon2.Close();
                reader2.Close();
                label7.Text = "当前数据库总人数：" + count+"，总户数："+hucount;

            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
        }

        public void chaxun()
        {
           
            try
            {
                string sql = "select * from datas where bumenid= '" +function.bumenid+ "' and";
                
               
                if(function.bumenid=="1")
                {
                    if(textBox1.Text==""&& textBox2.Text == "" && textBox3.Text == "" && textBox4.Text == "" && textBox5.Text == "" && textBox6.Text == "")
                    {
                        sql = "select * from datas";
                    }
                    else
                    {
                        sql = "select * from datas where";
                    }
                   
                }


                if (textBox1.Text != "")
                {
                    sql = sql + (" name like '%" + textBox1.Text.Trim() + "%' and");
                }
                if (textBox2.Text != "")
                {
                    sql = sql + (" card like'%" + textBox2.Text.Trim() + "%' and");
                }
                if (textBox3.Text != "")
                {
                    sql = sql + (" hucode like '%" + textBox3.Text.Trim() + "%' and");
                }
                if (textBox4.Text != "")
                {
                    sql = sql + (" suozaicun like '%" + textBox4.Text.Trim() + "%' and");
                }
                if (textBox5.Text != "")
                {
                    sql = sql + (" suozaizu like '%" + textBox5.Text.Trim() + "%' and");
                }
                if (textBox6.Text != "")
                {
                    sql = sql + (" address like '%" + textBox6.Text.Trim() + "%' and");
                }

                if (sql.Substring(sql.Length - 3, 3) == "and")
                {
                    sql = sql.Substring(0, sql.Length - 3);
                }
                MessageBox.Show(sql);
               
                DataTable dt = function.getdata(sql);
                dataGridView1.DataSource = dt;

                string texts = fc.readtxt();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                    try
                    {
                        string[] values = text[i].Split(new string[] { "#" }, StringSplitOptions.None);
                        if (values.Length > 1)
                        {
                            dataGridView1.Columns[values[0]].HeaderText = values[1];
                        }
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                    
                }
               


              
            }
            catch (Exception ex)
            {
                 MessageBox.Show(ex.ToString());


            }
        }

        Thread thread;
        private void 人员信息管理系统_Load(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getcount);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
           
        }

        private void 清空数据库谨慎管理员ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (function.bumenid != "1")
            {
                MessageBox.Show("对不起，您的权限不足");
                return;
            }
            DialogResult dr = MessageBox.Show("确定要清空吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                string sql = "delete from datas";
                bool status = function.SQL(sql);
                MessageBox.Show("清空成功");
            }
            else
            {

            }
        }

        private void 删除此行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count== 0)
            {
                MessageBox.Show("请选择需要删除的整行");
                return;
            }
               

            string uid = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string sql = "delete from datas where id='" + uid + "'  ";
           bool status= function.SQL(sql);
            if(status==true)
            {
                MessageBox.Show("删除成功");
                chaxun();
            }
            else
            {
                MessageBox.Show("删除失败");
            }
            
        }

        private void 人员信息管理系统_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            function.DataTableToExcel(function.DgvToTable(dataGridView1), "Sheet1", true);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string uid = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            string value= dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            string headertxt = dataGridView1.Columns[e.ColumnIndex].HeaderText;
            string ziduan = "";
            string texts = fc.readtxt();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                try
                {
                    string[] values = text[i].Split(new string[] { "#" }, StringSplitOptions.None);
                    if (values.Length > 1)
                    {
                        if (values[1] == headertxt)
                        {
                            ziduan = values[0];
                        }
                    }
                }
                catch (Exception)
                {

                    continue;
                }

            }

            string sql = "update datas set "+ziduan+" = '"+value+"'  where id='" + uid + "'  ";
          
            bool status = function.SQL(sql);
            if (status == true)
            {
                MessageBox.Show("修改成功");
                chaxun();
            }
            else
            {
                MessageBox.Show("修改失败");
            }
        }
    }
}
