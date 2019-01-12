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

namespace zhaopin_58
{
    public partial class register : Form
    {
        public register()
        {
            InitializeComponent();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
   


            DateTime dt = DateTime.Now;
            string time = DateTime.Now.ToString();
            string ip = method.GetIp();
            string mac = login.GetMacAddress();


            if (skinTextBox1.Text == "" || skinTextBox2.Text == "" || skinTextBox3.Text == "" || skinTextBox4.Text == "")
            {
                MessageBox.Show("请完善账号信息！");
                return;
            }
            if (skinTextBox3.Text.Length < 11)
            {
                MessageBox.Show("请输入十一位有效手机号！");
                return;
            }

            if (skinTextBox4.Text.Trim() != login.Random())
            {
                MessageBox.Show("您的注册码错误！");
                return;
            }


            try
            {


                string constr = "Host =116.62.62.62;Database=vip;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                string username = skinTextBox1.Text.Trim();
                string password = skinTextBox2.Text.Trim();
                string phone = skinTextBox3.Text.Trim();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO wuba2019 (username,password,phone,register_t,ip,mac)VALUES('" + username + " ', '" + password + " ','" + phone + " ', '" + time + " ', '" + ip + " ', '" + mac + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                //  MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，注册不需要读取，直接执行SQL语句即可

                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    MessageBox.Show("注册成功！");
                    this.Close();
                    mycon.Close();
                    // reader.Close();                  
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

        private void label5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
