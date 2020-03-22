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
            label4.Text = 价格计算.jieguo1.ToString();
            label5.Text = 价格计算.jieguo2.ToString();
            label6.Text = 价格计算.jieguo3.ToString();
            label7.Text = 价格计算.jieguo4.ToString();
            label8.Text = 价格计算.jieguo5.ToString();

            label1.Text = 价格计算.time+" 停盘";
        }

        private void 子窗体_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
          
            Rectangle rect = new Rectangle();
            rect = Screen.GetWorkingArea(this);
            //MessageBox.Show("本机器的分辨率是" + rect.Width.ToString() + "*" + rect.Height.ToString());
            this.Left = rect.Width-200;
            this.Top = 0;
            this.TopMost = true;
         
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            价格计算 js = new 价格计算();
            js.Show();
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
    }
}
