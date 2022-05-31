using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 软件工具箱
{
    public partial class 软件工具箱 : Form
    {
        public 软件工具箱()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void PictureBox4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(path + "//data//qcc.exe");
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(path + "//data//qcc.exe");
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(path + "//data//mt.exe");
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(path + "//data//map.exe");
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(path + "//data//mt.exe");
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(path + "//data//map.exe");
        }
        private Point mPoint = new Point();
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }

        private void 软件工具箱_Load(object sender, EventArgs e)
        {
            this.tabControl1.Region = new Region(new RectangleF(this.tabPage1.Left, this.tabPage1.Top, this.tabPage1.Width, this.tabPage1.Height));
        }

        private void button9_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "http://www.acaiji.com/alipay123/");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("添加微信：18033886802");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
              
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://xp.sfkjxx.com/store.php");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://xp.sfkjxx.com/store.php");
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://xp.sfkjxx.com/store.php");
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://xp.sfkjxx.com/store.php");
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://xp.sfkjxx.com/store.php");
        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://xp.sfkjxx.com/store.php");
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://xp.sfkjxx.com/store.php");
        }

        private void linkLabel11_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://xp.sfkjxx.com/store.php");
        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://81.68.136.112/");
        }

        private void linkLabel18_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://81.68.136.112/");
        }

        private void linkLabel17_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://81.68.136.112/");
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://81.68.136.112/");
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://81.68.136.112/");
        }

        private void linkLabel16_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://81.68.136.112/");
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://xp.sfkjxx.com/store/user/login");
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://xp.sfkjxx.com/agent.php");
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://xp.sfkjxx.com/agent/");
        }
    }
}
