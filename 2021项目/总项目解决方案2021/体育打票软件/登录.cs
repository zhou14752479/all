using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 体育打票软件
{
    public partial class 登录 : Form
    {
        public 登录()
        {
            InitializeComponent();
        }

        private void login_btn_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Trim()=="666888")
            {
                if(textBox2.Text.Trim()== "11223344")
                {
                    体育打票软件 ti = new 体育打票软件();
                    ti.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("密码错误！");
                }
            }
            else
            {
                MessageBox.Show("用户名不存在或错误");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.textBox2.PasswordChar =true ? new char() : '*';

        }

        private void 登录_Load(object sender, EventArgs e)
        {

        }
    }
}
