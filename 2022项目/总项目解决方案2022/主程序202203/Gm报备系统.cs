using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202203
{
    public partial class Gm报备系统 : Form
    {
        public Gm报备系统()
        {
            InitializeComponent();
        }

        private void Gm报备系统_Load(object sender, EventArgs e)
        {
            panel1.Parent = pictureBox1;
            panel1.BackColor = Color.FromArgb(150, 255, 255, 255);
           
         
            //label1.Parent = pictureBox1;
            //label2.Parent = pictureBox1;
            //label3.Parent = pictureBox1;
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入角色名称");
            }
            else
            {
                MessageBox.Show("提交成功！");
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {

                button1_Click(this, new EventArgs());
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < 400; i++)
            {
                comboBox1.Items.Add(textBox2.Text+ i + "区");
            }

            comboBox1.Text = textBox2.Text + "1区";

            button2.Visible = false;
            textBox2.Visible = false;
        }
    }
}
