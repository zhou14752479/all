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
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            string time = DateTime.Now.ToString();
         

            if (textBox3.Text == "" || textBox4.Text == "" )
            {
                MessageBox.Show("请完善账号信息！");
                return;
            }


            try
            {
                string constr = "Host =47.99.68.92;Database=shangbiao;Username=root;Password=zhoukaige00.@*.";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select * from users where username='" + textBox3.Text.Trim() + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                if (reader.Read())
                {
                    MessageBox.Show("您的账户已存在");
                }

                else
                {

                    mycon.Close();


                    MySqlConnection mycon1 = new MySqlConnection(constr);
                    mycon1.Open();

                    string username = textBox3.Text.Trim();
                    string password = textBox4.Text.Trim();


                    MySqlCommand cmd1 = new MySqlCommand("INSERT INTO users (username,password,endtime)VALUES('" + username + " ', '" + password + " ', '" + time + " ')", mycon1);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                    int count = cmd1.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                    if (count > 0)
                    {
                        MessageBox.Show("注册成功！");

                        mycon.Close();

                    }
                    else
                    {
                        MessageBox.Show("连接失败！");
                    }

                }
            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            try
            {
                DateTime dt = DateTime.Now;
                string constr = "Host =47.99.68.92;Database=shangbiao;Username=root;Password=zhoukaige00.@*.";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select * from users where username='" + textBox1.Text.Trim() + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                if (reader.Read())
                {
                   
                    string endtime = reader["endtime"].ToString().Trim();
                    string password = reader["password"].ToString().Trim();

                    if (Convert.ToDateTime(endtime) <= dt)
                    {
                        MessageBox.Show("您的账户已过期");
                        mycon.Close();
                        return;

                    }



                    //判断密码
                    if (textBox2.Text.Trim() == password)
                    {                          
                       Form1 fm1 = new Form1();
                        fm1.Show();
                        this.Hide();
                        mycon.Close();
                        return;
                    }

                    else

                    {
                        MessageBox.Show("您的密码错误！");
                       
                        mycon.Close();
                        return;
                    }

                }
                else
                {
                    MessageBox.Show("未查询到您的账户信息！请联系客服开通账号！");
                    mycon.Close();
                    return;
                }


            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
