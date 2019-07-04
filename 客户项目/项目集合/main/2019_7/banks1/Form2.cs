using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_7.banks1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "admin123456" && textBox2.Text == "admin147258369")
            {
                MessageBox.Show("登陆成功");
                Form3 sl = new Form3();
                sl.Show();
                this.Hide();

            }

            else
            {
                MessageBox.Show("账号或密码错误");

            }

           


        }
    }
}
