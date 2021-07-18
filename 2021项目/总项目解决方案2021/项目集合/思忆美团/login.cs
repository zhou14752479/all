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

namespace 思忆美团
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }
        functions fc = new functions();
        Point mPoint = new Point();
        private void login_button_Click(object sender, EventArgs e)
        {
            if (usertxt.Text == "" || passtxt.Text == "" || usertxt.Text == "输入手机号" || passtxt.Text == "输入密码")
            {
                MessageBox.Show("输入错误");
                return;
            }
            main ma = new main();
            bool status=  fc.login(usertxt.Text.Trim(),passtxt.Text.Trim(),fc.jiqima);
            if(status==true)
            {
               Thread thread = new Thread(delegate () {
                   label1.Text = "正在连接服务器...";
                   Thread.Sleep(500);
                   label1.Text = "正在检测账号密码...";
                   Thread.Sleep(500);
                  
                  
               });
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
                ma.Show();
                this.Hide();


            }
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (usertxt_r.Text == "" || usertxt_r.Text == "" || passtxt_r.Text == "输入手机号" || passtxt_r.Text == "输入密码")
            {
                MessageBox.Show("输入错误");
                return;
            }

            if (usertxt_r.Text.Length < 11)
            {
                MessageBox.Show("手机格式错误");
                return;
            }

            if (passtxt_r.Text.Length <6)
            {
                MessageBox.Show("密码至少6位");
                return;
            }
            fc.register(usertxt_r.Text.Trim() + "#" + passtxt_r.Text.Trim() + "#" + fc.jiqima);
        }

        private void usertxt_r_MouseEnter(object sender, EventArgs e)
        {
            if (usertxt_r.Text == "输入手机号")
            {
                usertxt_r.Text = "";
            }
        }

        private void usertxt_MouseEnter(object sender, EventArgs e)
        {
            if (usertxt.Text == "输入手机号")
            {
                usertxt.Text = "";
            }
        }

        private void passtxt_MouseEnter(object sender, EventArgs e)
        {
            if (passtxt.Text == "输入密码")
            {
                passtxt.Text = "";
            }
        }

        private void passtxt_r_MouseEnter(object sender, EventArgs e)
        {
            if (passtxt_r.Text == "输入密码")
            {
                passtxt_r.Text = "";
            }
        }

        private void skinMenuStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void skinMenuStrip1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }

        private void login_Load(object sender, EventArgs e)
        {
            fc.jiqima = fc.getjiqima();
        }

        private void close_label_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MessageBox.Show("是否退出？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {

            }
            else
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
    }
}
