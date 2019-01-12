using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _58
{
    public partial class Buy : Form
    {
        public Buy()
        {
            InitializeComponent();
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                skinPictureBox1.Show();
                skinPictureBox2.Hide();
                skinPictureBox3.Hide();
                skinPictureBox4.Hide();
            }
        }


        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                skinPictureBox2.Show();
                skinPictureBox1.Hide();
                skinPictureBox3.Hide();
                skinPictureBox4.Hide();
            }
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                skinPictureBox3.Show();
                skinPictureBox1.Hide();
                skinPictureBox2.Hide();
                skinPictureBox4.Hide();
            }
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                skinPictureBox4.Show();
                skinPictureBox1.Hide();
                skinPictureBox2.Hide();
                skinPictureBox3.Hide();
            }
        }

     

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show("请选择微信支付！如有问题请咨询客服！");
        }

        private void visualButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("支付成功，请添加微信：17606117606，自动发送注册码！");
        }

       
    }
}
