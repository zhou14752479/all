using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LEDSimulator
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        public void login1()
        {
            if (textBox1.Text == "admin" && textBox2.Text == "admin")
            {
                button1.Text = "正在验证...";
                Thread.Sleep(200);
                button1.Text = "正在登录...";
                Thread.Sleep(200);
                Form1 fm1 = new Form1();
                fm1.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("账号或密码错误");
            }
        }
    
        private void button1_Click(object sender, EventArgs e)
        {

            login1();
        }

        private void login_Load(object sender, EventArgs e)
        {

        }
    }
}
