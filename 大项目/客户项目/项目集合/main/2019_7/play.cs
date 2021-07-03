using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_7
{
    public partial class play : Form
    {
        public play()
        {
            InitializeComponent();
        }

        private void Play_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        }
        string path = AppDomain.CurrentDomain.BaseDirectory+ "\\config\\";

        private void SkinButton1_Click(object sender, EventArgs e)
        {       
            video v = new video();      
            v.Show();
            v.axWindowsMediaPlayer1.URL = path+ skinButton1.Text+".mp4";
            v.Text = skinButton1.Text;
        }

        private void SkinButton8_Click(object sender, EventArgs e)
        {
            video v = new video();
            v.Show();
            v.axWindowsMediaPlayer1.URL = path + skinButton8.Text + ".mp4";
            v.Text = skinButton8.Text;
        }

        private void SkinButton2_Click(object sender, EventArgs e)
        {
            video v = new video();
            v.Show();
            v.axWindowsMediaPlayer1.URL = path + skinButton2.Text + ".mp4";
            v.Text = skinButton2.Text;
        }

        private void SkinButton7_Click(object sender, EventArgs e)
        {
            video v = new video();
            v.Show();
            v.axWindowsMediaPlayer1.URL = path + skinButton7.Text + ".mp4";
            v.Text = skinButton7.Text;
        }

        private void SkinButton3_Click(object sender, EventArgs e)
        {
            video v = new video();
            v.Show();
            v.axWindowsMediaPlayer1.URL = path + skinButton3.Text + ".mp4";
            v.Text = skinButton3.Text;
        }

        private void SkinButton6_Click(object sender, EventArgs e)
        {
            video v = new video();
            v.Show();
            v.axWindowsMediaPlayer1.URL = path + skinButton6.Text + ".mp4";
            v.Text = skinButton6.Text;
        }

        private void SkinButton4_Click(object sender, EventArgs e)
        {
            video v = new video();
            v.Show();
            v.axWindowsMediaPlayer1.URL = path + skinButton4.Text + ".mp4";
            v.Text = skinButton4.Text;
        }

        private void SkinButton5_Click(object sender, EventArgs e)
        {
            video v = new video();
            v.Show();
            v.axWindowsMediaPlayer1.URL = path + skinButton5.Text + ".mp4";
            v.Text = skinButton5.Text;
        }

        private void SkinButton10_Click(object sender, EventArgs e)
        {
            video v = new video();
            v.Show();
            v.axWindowsMediaPlayer1.URL = path + skinButton10.Text + ".mp4";
            v.Text = skinButton10.Text;
        }

        private void SkinButton9_Click(object sender, EventArgs e)
        {
            video v = new video();
            v.Show();
            v.axWindowsMediaPlayer1.URL = path + skinButton9.Text + ".mp4";
            v.Text = skinButton9.Text;
        }

        private void SkinButton11_Click(object sender, EventArgs e)
        {
            video v = new video();
            v.Show();
            v.axWindowsMediaPlayer1.URL = path + skinButton11.Text + ".mp4";
            v.Text = skinButton11.Text;
        }
    }
}
