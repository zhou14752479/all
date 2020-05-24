using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 管理软件
{
    public partial class 管理端 : Form
    {
        public 管理端()
        {
            InitializeComponent();
        }
        string constr = "Host =111.229.244.97;Database=links;Username=root;Password=root";
        #region 获取数据库IP
        public void getips()
        {
           
            MySqlDataAdapter sda = new MySqlDataAdapter("Select ip From ips ", constr);
            DataSet Ds = new DataSet();
            sda.Fill(Ds, "T_Class");

            this.dataGridView1.DataSource = Ds.Tables["T_Class"];

        }
        #endregion
        private void 管理端_Load(object sender, EventArgs e)
        {
            getips();
        }

        public void addIp()
        {
            try
            {


                
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                string IP = textBox1.Text.Trim();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO ips (ip)VALUES('" + IP + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    MessageBox.Show("添加成功！");

                    mycon.Close();
                    textBox1.Text = "";

                }
                else
                {
                    MessageBox.Show("连接失败！");
                }


            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public void addurl(string biao,string url)
        {
            try
            {


                
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO " + biao + " VALUES('" + url + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    MessageBox.Show("添加成功！");

                    mycon.Close();

                }
                else
                {
                    MessageBox.Show("连接失败！");
                    mycon.Close();
                }


            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addIp();
            getips();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            addurl("aaa",textBox2.Text.Trim());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            addurl("bbb", textBox3.Text.Trim());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            addurl("ccc", textBox4.Text.Trim());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            addurl("ddd", textBox5.Text.Trim());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();



            MySqlCommand cmd = new MySqlCommand("delete from ips", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


            int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
            if (count > 0)
            {


                mycon.Close();

            }
            else
            {

                mycon.Close();
            }

            getips();


        }
    }
}
