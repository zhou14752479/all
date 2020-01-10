using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 一机一码
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "admin" && textBox2.Text == "147258369")
            {
                Form1 fm1 = new Form1();
                fm1.Show();
            }
            else if (textBox1.Text == "abc147258" && textBox2.Text == "abc159357")
            {
                Form1 fm1 = new Form1();
                fm1.Show();
            }
            else if(textBox1.Text == "jht147258" && textBox2.Text == "jht159357")
            {
                Form1 fm1 = new Form1();
                fm1.Show();
            }
            else if (textBox1.Text == "kgt147258" && textBox2.Text == "kgt159357")
            {
                Form1 fm1 = new Form1();
                fm1.Show();
            }
            else if (textBox1.Text == "fth147258" && textBox2.Text == "fth159357")
            {
                Form1 fm1 = new Form1();
                fm1.Show();
            }

            else if (textBox1.Text == "qaz11256" && textBox2.Text == "djk159369")
            {
                Form1 fm1 = new Form1();
                fm1.Show();
            }

            else if (textBox1.Text == "qaz25369" && textBox2.Text == "djk126588")
            {
                Form1 fm1 = new Form1();
                fm1.Show();
            }

            else
            {
                MessageBox.Show("您的密码错误！"); 
            }
        }

        private void login_Load(object sender, EventArgs e)
        {

        }
    }
}
