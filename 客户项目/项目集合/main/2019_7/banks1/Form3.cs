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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void PictureBox4_Click(object sender, EventArgs e)
        {
            Form4 fm4 = new Form4();
            fm4.Show();
            fm4.pictureBox4.Visible = true;
            fm4.label1.Text = "平安银行";
        }

        private void PictureBox6_Click(object sender, EventArgs e)
        {
            Form4 fm4 = new Form4();
            fm4.Show();
            fm4.pictureBox6.Visible = true;
            fm4.label1.Text = "厦门银行";
        }

        private void PictureBox7_Click(object sender, EventArgs e)
        {
            Form4 fm4 = new Form4();
            fm4.Show();
            fm4.pictureBox7.Visible = true;
            fm4.label1.Text = "微众银行";
        }

        private void PictureBox8_Click(object sender, EventArgs e)
        {
            Form4 fm4 = new Form4();
            fm4.Show();
            fm4.pictureBox8.Visible = true;
            fm4.label1.Text = "重庆银行";
        }
    }
}
