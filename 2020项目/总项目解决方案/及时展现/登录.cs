using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 及时展现
{
    public partial class 登录 : Form
    {
        public 登录()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {

                try
                {



                    string constr = "Host =139.159.218.174;Database=data;Username=root;Password=123456";
                    MySqlConnection mycon = new MySqlConnection(constr);
                    mycon.Open();

                    MySqlCommand cmd = new MySqlCommand("select * from users where user='" + textBox1.Text.Trim() + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                    MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                label3.Text = "正在连接服务器......";

           
                System.Threading.Thread.Sleep(200);

                label3.Text = "正在验证用户名和密码......";


                if (reader.Read())
                    {

                    string username = reader["user"].ToString().Trim();
                    string password = reader["pass"].ToString().Trim();
                    string time = reader["time"].ToString().Trim();

                    DateTime dt = DateTime.Now;
                    if (dt < Convert.ToDateTime(time))
                    {
                        if (textBox2.Text.Trim() == password)

                        {
                           
                            客户端 kh = new 客户端();
                            kh.Show();
                            this.Hide();

                        }
                        else

                        {
                            MessageBox.Show("您的密码错误！");
                            label3.Text = "请重新输入密码！";
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("您的账号已过期！");
                        label3.Text = "您的账号已过期！";
                        return;

                    }

                }

                else
                {
                    MessageBox.Show("您输入的账号不存在！");
                    label3.Text = "您输入的账号不存在！";
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
