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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("未查询到相关信息");
        }

      

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
        }

        private void Panel1_MouseClick(object sender, MouseEventArgs e)
        {
            Form5 fm5 = new Form5();
            fm5.label7.Text = "风险提示";
            fm5.label11.Text = textBox1.Text;
            fm5.label12.Text = "有风险，暂不过审";
            fm5.label6.Text = label1.Text;
            fm5.Show();
        }

  

        private void Panel2_MouseClick(object sender, MouseEventArgs e)
        {
            Form5 fm5 = new Form5();
            fm5.label7.Text = "进度提醒";
            fm5.label11.Text = textBox1.Text;
            fm5.label12.Text = "风险已解除";
            fm5.label6.Text = label1.Text;
            fm5.Show();
        }

        private void Panel3_MouseClick(object sender, MouseEventArgs e)
        {
            Form5 fm5 = new Form5();
            fm5.label7.Text = "风险提示";
            fm5.label11.Text = textBox1.Text;
            fm5.label12.Text = "渠道需存保证金";
            fm5.label6.Text = label1.Text;
            fm5.Show();

        }

        private void Panel4_MouseClick(object sender, MouseEventArgs e)
        {
            Form5 fm5 = new Form5();
            fm5.label7.Text = "进度提醒";
            fm5.label11.Text = textBox1.Text;
            fm5.label12.Text = "审核未通过";
            fm5.label6.Text = label1.Text;
            fm5.Show();
        }
    }
}
