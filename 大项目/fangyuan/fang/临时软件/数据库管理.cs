using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fang.临时软件
{
    public partial class 数据库管理 : Form
    {
        public 数据库管理()
        {
            InitializeComponent();
        }


        #region  获取数据库信息
        public void getDatas(string str)
        {
            dataGridView1.Columns.Clear();
            string constr = "Host =122.114.13.236;Database=users;Username=admin111;Password=admin111";
            MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
            DataSet Ds = new DataSet();
            da.Fill(Ds, "T_Class");

            //使用DataSet绑定时，必须同时指明DateMember 
            this.dataGridView1.DataSource = Ds;
            this.dataGridView1.DataMember = "T_Class";

            //也可以直接用DataTable来绑定 
            this.dataGridView1.DataSource = Ds.Tables["T_Class"];



            DataGridViewButtonColumn button = new DataGridViewButtonColumn();


            DataGridViewButtonColumn button1 = new DataGridViewButtonColumn();

            button1.HeaderText = "删除";
            this.dataGridView1.Columns.AddRange(button1);
            button1.UseColumnTextForButtonValue = true;
            button1.Text = "删除";
            button1.Width = 60;


            DataGridViewButtonColumn button2 = new DataGridViewButtonColumn();

            button2.HeaderText = "禁止登陆";
            this.dataGridView1.Columns.AddRange(button2);
            button2.UseColumnTextForButtonValue = true;
            button2.Text = "禁止登陆";
            button2.Width = 80;

            DataGridViewButtonColumn button3 = new DataGridViewButtonColumn();

            button3.HeaderText = "恢复登陆";
            this.dataGridView1.Columns.AddRange(button3);
            button3.UseColumnTextForButtonValue = true;
            button3.Text = "恢复登陆";
            button3.Width = 80;

        }
        #endregion
        private void 数据库管理_Load(object sender, EventArgs e)
        {

        }
        #region 反射函数调用Button事件
        private void callOnClick(Button btn)
        {
            //建立一个类型  
            Type t = typeof(Button);
            //参数对象  
            object[] p = new object[1];
            //产生方法  
            MethodInfo m = t.GetMethod("OnClick", BindingFlags.NonPublic | BindingFlags.Instance);
            //参数赋值。传入函数  
            p[0] = EventArgs.Empty;
            //调用  
            m.Invoke(btn, p);
            return;
        }
        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            getDatas("select id as ID, username as 账号, password as 密码, status as 登录状态 from zhanghaos");
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn column = dataGridView1.Columns[e.ColumnIndex];
           

            if (column.HeaderText == "删除")
            {
                int index = e.RowIndex;

                string id = dataGridView1.Rows[index].Cells[0].Value.ToString();

                string constr = "Host =122.114.13.236;Database=users;Username=admin111;Password=admin111";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("delete from zhanghaos where id='" + id + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("删除成功");
                    callOnClick(button1);

                }
            }

            else if (column.HeaderText == "禁止登陆")
            {
                int index = e.RowIndex;

                string id = dataGridView1.Rows[index].Cells[0].Value.ToString();

                string constr = "Host =122.114.13.236;Database=users;Username=admin111;Password=admin111";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("update zhanghaos set status='1' where id='" + id + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("修改成功");
                    callOnClick(button1);

                }
              
                }



            else if (column.HeaderText == "恢复登陆")
            {
                int index = e.RowIndex;

                string id = dataGridView1.Rows[index].Cells[0].Value.ToString();

                string constr = "Host =122.114.13.236;Database=users;Username=admin111;Password=admin111";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("update zhanghaos set status='0' where id='" + id + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("修改成功");
                    callOnClick(button1);

                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string constr = "Host =122.114.13.236;Database=users;Username=admin111;Password=admin111";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();

            MySqlCommand cmd = new MySqlCommand("insert into zhanghaos (username,password)VALUES ('"+textBox1.Text+"','"+textBox2.Text+"') ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'
            int result = cmd.ExecuteNonQuery();
            if (result > 0)
            {
                MessageBox.Show("新增成功");
                callOnClick(button1);

            }
        }
    }
}
