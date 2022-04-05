using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 人员信息管理系统
{
    public partial class 添加子账号 : Form
    {
        public 添加子账号()
        {
            InitializeComponent();
        }
        #region  注册函数

        public void register()
        {

            try
            {


                MySqlConnection mycon = new MySqlConnection(function.conn);
                mycon.Open();

                string name = textBox1.Text.Trim();
                string username = textBox2.Text.Trim();
                string password = textBox3.Text.Trim();


                MySqlCommand cmd = new MySqlCommand("INSERT INTO bumen (name,username,password)VALUES('" + name + " ','" + username + " ', '" + password + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
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

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="" || textBox2.Text == ""|| textBox3.Text == "")
            {
                MessageBox.Show("请输入信息");
                return;
            }
            register();
        }



    }
}
