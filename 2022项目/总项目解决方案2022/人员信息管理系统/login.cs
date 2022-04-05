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
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("请联系管理员");
        }

        Thread thread;

        #region  登陆函数

        public void alogin()
        {       
            try
            {

                MySqlConnection mycon = new MySqlConnection(function.conn);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select * from bumen where username='" + textBox1.Text .Trim() + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                if (reader.Read())
                {
                    string uid = reader["id"].ToString().Trim();
                    string name = reader["name"].ToString().Trim();
                    string username = reader["username"].ToString().Trim();
                    string password = reader["password"].ToString().Trim();

                    button1.Text = "正在连接服务器......";
                    Application.DoEvents();
                    Thread.Sleep(200);

                    button1.Text = "正在验证用户名和密码......";
                    Application.DoEvents();
                    Thread.Sleep(200);
                    //判断密码
                    if (textBox2.Text.Trim() == password)

                    {
                      
                       function.bumenid  = uid;
                       人员信息管理系统 fm1 = new 人员信息管理系统();
                        fm1.Text = "人员信息管理系统                    "+name;
                        fm1.Show();
                        this.Hide();

                    }
                    else
                    {
                        MessageBox.Show("您的密码错误！");
                        button1.Text = "点击登录";
                        return;
                    }
                   
                }
              
                else
                {
                    MessageBox.Show("未查询到您的账户信息！请联系客服开通账号！");
                    return;
                }
                mycon.Close();
                reader.Close();

            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }


        #endregion
      
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入账号");
                return;
            }
            if (textBox2.Text=="")
            {
                MessageBox.Show("请输入密码");
                return;
            }
            alogin();
        }
    }
}
