using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "6666" && textBox2.Text == "6666")
            {
                MessageBox.Show("登陆成功！");
                Form2 fm2 = new Form2();
                fm2.Show();
                this.Hide();
            }

            else
            {
                MessageBox.Show("账号或密码错误！");
            }
        }
    }
}
