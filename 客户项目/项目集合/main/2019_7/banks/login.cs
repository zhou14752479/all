using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_7.banks
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "admin123456" && textBox2.Text == "admin147258369")
            {
                MessageBox.Show("登陆成功");
                select sl = new select();
                sl.Show();
                this.Hide();

            }

            else
            {
                MessageBox.Show("账号或密码错误");

            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
        }
    }
}
