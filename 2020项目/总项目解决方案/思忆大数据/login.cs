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
          string status=  md.register(textBox3.Text.Trim(), textBox4.Text.Trim());
            MessageBox.Show(status);
            this.Hide();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("请输入账号密码");
                return;
            }
            string status= md.login(textBox1.Text.Trim(), textBox2.Text.Trim());
            MessageBox.Show(status);
            this.Hide();
        }
    }
}
