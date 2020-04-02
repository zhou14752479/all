using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 启动程序
{
    public partial class 子窗体 : Form
    {
        public 子窗体()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = 价格计算.value1;
            label3.Text = 价格计算.value2;
            label10.Text = 价格计算.value3;







            label4.Text = 价格计算.jieguo1.ToString();
            label5.Text = 价格计算.jieguo2.ToString();
            label6.Text = 价格计算.jieguo3.ToString();
            label7.Text = 价格计算.jieguo4.ToString();
            label8.Text = 价格计算.jieguo5.ToString();
            label9.Text = 价格计算.jieguo6.ToString();

            label1.Text = 价格计算.time+" 停盘";

            if (this.Top == 0)
            {
                linkLabel1.LinkColor = Color.Red;
            }
            else
            {
                linkLabel1.LinkColor = Color.DarkBlue;
            }
        }

        private void 子窗体_Load(object sender, EventArgs e)
        {
            Rectangle rect = new Rectangle();
            rect = Screen.GetWorkingArea(this);
            //MessageBox.Show("本机器的分辨率是" + rect.Width.ToString() + "*" + rect.Height.ToString());
            this.Left = rect.Width - 235;
            this.Top = 0;
            this.TopMost = true;
            timer1.Start();

            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
          
            Rectangle rect = new Rectangle();
            rect = Screen.GetWorkingArea(this);
            //MessageBox.Show("本机器的分辨率是" + rect.Width.ToString() + "*" + rect.Height.ToString());
            this.Left = rect.Width- 235;
            this.Top = 0;
            this.TopMost = true;
         
        }



        private void 子窗体_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出吗？", "退出询问"
         , MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;//告诉窗体关闭这个任务取消

            }
            else
            {




                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void 子窗体_MouseEnter(object sender, EventArgs e)
        {
            
        }

        private void PictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.Red;
        }

        private void PictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.White;
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            价格计算 js = new 价格计算();
            js.Show();
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }
        private Point mPoint = new Point();
        private void 子窗体_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void 子窗体_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }

        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;//最小化 
        }
    }
}
