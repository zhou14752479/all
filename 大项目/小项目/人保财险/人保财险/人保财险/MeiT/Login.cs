using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeiT
{
    public partial class Login : Form

    {
        public static string username { get; set; }

        public Login()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("请输入账号信息！");
                return;
            }

            else
            {
                if (textBox1.Text == "sq41" && textBox2.Text == "sq10086")
                {
                   
                    Form1 fm1 = new Form1();
                    fm1.Show();                 
                    this.Hide();
                   
                }

                else
                {
                    MessageBox.Show("账号错误");
                }
                
                
           }
            
        }
    }
}
