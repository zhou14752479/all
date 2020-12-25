using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 思忆大数据
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }
        method md = new method();
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("请输入账号密码");
                return;
            }
            if (textBox3.Text.Length < 10)
            {
                MessageBox.Show("请输入正确手机号");
                return;
            }
          string status=  md.register(textBox3.Text.Trim(), textBox4.Text.Trim());
            MessageBox.Show(status);
           

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("请输入账号密码");
                return;
            }
            string status= md.login(textBox1.Text.Trim(), textBox2.Text.Trim());
            if (status.Contains("true"))
            {
                method.username = textBox1.Text;
                main ma = new main();
                ma.Show();
               
                this.Hide();
            }
            else
            {
                MessageBox.Show(status);
            }
           

           
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.acaiji.com");
            System.Diagnostics.Process.Start("http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes");
        }

        private void label4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.acaiji.com");
        }
    }
}
