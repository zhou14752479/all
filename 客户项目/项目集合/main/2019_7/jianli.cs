using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_7
{
    public partial class jianli : Form
    {
        public jianli()
        {
            InitializeComponent();
        }
        bool jihuo = false;
        public void select()
        {
            string connetStr = "server=154.209.249.254;user=root;password=root;database=datas;"; //localhost不支持ssl连接时，最后一句一定要加！！！sslMode=none;
            MySqlConnection conn = new MySqlConnection(connetStr);
            try
            {
                conn.Open(); //连接数据库
                //MessageBox.Show("数据库连接成功", "提示", MessageBoxButtons.OK);
                string searchStr = "SELECT * FROM jianli ORDER BY rand() LIMIT 200 ";   //student表中数据
                MySqlDataAdapter adapter = new MySqlDataAdapter(searchStr, conn);
                DataTable a = new DataTable();
                adapter.Fill(a);
                this.dataGridView1.DataSource = a;
                dataGridView1.Columns[1].HeaderText = "用户名";
                dataGridView1.Columns[2].HeaderText = "真实姓名";
                dataGridView1.Columns[3].HeaderText = "电话";
                dataGridView1.Columns[4].HeaderText = "手机号";
                dataGridView1.Columns[5].HeaderText = "省份";
                dataGridView1.Columns[6].HeaderText = "城市";
                dataGridView1.Columns[7].HeaderText = "区域";
                dataGridView1.Columns[8].HeaderText = "地址";
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK);     //显示错误信息
            }

        }

        public void select1()
        {
            string connetStr = "server=154.209.249.254;user=root;password=root;database=datas;"; //localhost不支持ssl连接时，最后一句一定要加！！！sslMode=none;
            MySqlConnection conn = new MySqlConnection(connetStr);
            try
            {
                conn.Open(); //连接数据库
                //MessageBox.Show("数据库连接成功", "提示", MessageBoxButtons.OK);
                string searchStr = "SELECT * FROM jianli ORDER BY rand() LIMIT 1000 ";   //student表中数据
                MySqlDataAdapter adapter = new MySqlDataAdapter(searchStr, conn);
                DataTable a = new DataTable();
                adapter.Fill(a);
                this.dataGridView1.DataSource = a;
                dataGridView1.Columns[1].HeaderText = "用户名";
                dataGridView1.Columns[2].HeaderText = "真实姓名";
                dataGridView1.Columns[3].HeaderText = "电话";
                dataGridView1.Columns[4].HeaderText = "手机号";
                dataGridView1.Columns[5].HeaderText = "省份";
                dataGridView1.Columns[6].HeaderText = "城市";
                dataGridView1.Columns[7].HeaderText = "区域";
                dataGridView1.Columns[8].HeaderText = "地址";
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK);     //显示错误信息
            }

        }




        private void Jianli_Load(object sender, EventArgs e)
        {
            MessageBox.Show("未激活软件一次限制采集200个");
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (jihuo == false)
            {
            }
            else if (jihuo == true)
            {

            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.DgvToTable(dataGridView1), "Sheet1", true);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            string connetStr = "server=154.209.249.254;user=root;password=root;database=datas;"; //localhost不支持ssl连接时，最后一句一定要加！！！sslMode=none;
            MySqlConnection conn = new MySqlConnection(connetStr);
            try
            {
                conn.Open(); //连接数据库

                string Str = "select * from jhm where jihuoma='" + textBox1.Text.Trim() + "'  ";   //student表中数据
                MySqlCommand cmd = new MySqlCommand(Str, conn);
                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源



                if (reader.Read())
                {

                    // string jihuoma = reader["jihuoma"].ToString().Trim();
                    MessageBox.Show("激活成功");
                    jihuo = true;
                    textBox1.Text = "";
                }
                else
                {
                    MessageBox.Show("激活码错误");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK);     //显示错误信息
            }
        }
    }
}
