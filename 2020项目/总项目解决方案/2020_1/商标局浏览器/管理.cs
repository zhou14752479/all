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

namespace 商标局浏览器
{
    public partial class 管理 : Form
    {
        public 管理()
        {
            InitializeComponent();
        }

        public void run()
        {
            string constr = "Host =47.99.68.92;Database=shangbiao;Username=root;Password=zhoukaige00.@*.";
            MySqlDataAdapter sda = new MySqlDataAdapter("Select username,password,endtime From users ", constr);
            DataSet Ds = new DataSet();
            sda.Fill(Ds, "T_Class");

            this.dataGridView1.DataSource = Ds.Tables["T_Class"];
        }
        private void 管理_Load(object sender, EventArgs e)
        {
            run();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string constr = "Host =47.99.68.92;Database=shangbiao;Username=root;Password=zhoukaige00.@*.";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();

            MySqlCommand cmd = new MySqlCommand("delete from users where username='" + textBox3.Text.Trim() + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

            int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
            if (count > 0)
            {
                MessageBox.Show("删除成功！");
                run();
                mycon.Close();

            }
            else
            {
                MessageBox.Show("删除失败！");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string constr = "Host =47.99.68.92;Database=shangbiao;Username=root;Password=zhoukaige00.@*.";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();

            MySqlCommand cmd = new MySqlCommand("UPDATE users SET password= '" + textBox2.Text.Trim() + "'  where username='" + textBox1.Text.Trim() + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

            int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
            if (count > 0)
            {
                MessageBox.Show("重置成功！");
                run();
                mycon.Close();

            }
            else
            {
                MessageBox.Show("重置失败！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            string endtime = dt.AddDays(Convert.ToInt32(comboBox1.Text)).ToString();

            string constr = "Host =47.99.68.92;Database=shangbiao;Username=root;Password=zhoukaige00.@*.";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();

            MySqlCommand cmd = new MySqlCommand("UPDATE users SET endtime= '" + endtime + "'  where username='" + textBox4.Text.Trim() + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

            int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
            if (count > 0)
            {
                MessageBox.Show("设置成功！");
                run();
                mycon.Close();

            }
            else
            {
                MessageBox.Show("设置失败！");
            }
        }
    }
}
