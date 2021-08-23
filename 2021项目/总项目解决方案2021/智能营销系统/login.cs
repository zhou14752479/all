using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 智能营销系统
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }
       
        public void logins()
        {
            if (uiTextBox1.Text == "123456" && uiTextBox2.Text == "123456")
            {
                智能营销系统 main= new 智能营销系统();
                main.Show();
                this.Hide();
            }
            else
            {
                uiButton1.Text = "登录";
                MessageBox.Show("账号或密码错误");
            }
        }

        private void ThreadFunc()
        {
            uiButton1.Text = "正在登录...";
            Thread.Sleep(1000);
            MethodInvoker mi = new MethodInvoker(this.logins);
            this.BeginInvoke(mi);
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
           Thread FormThread = new Thread(new ThreadStart(ThreadFunc));
             FormThread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void login_Load(object sender, EventArgs e)
        {
           
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
    }
}
