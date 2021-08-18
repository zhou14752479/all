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

namespace 体育打票软件
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }



        public void logins()
        {
            if (textBox1.Text == "xxf230230" && textBox2.Text == "XXF230230")
            {
                体育打票软件 main = new 体育打票软件();
                main.Show();
                this.Hide();
            }
            else
            {
                button1.Text = "登 录";
                MessageBox.Show("账号或密码错误");
            }
        }

        private void ThreadFunc()
        {
            button1.Text = "正在验证账号密码...";
            Thread.Sleep(500);
            button1.Text = "正在登录...";
            Thread.Sleep(500);
            MethodInvoker mi = new MethodInvoker(this.logins);
            this.BeginInvoke(mi);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Thread FormThread = new Thread(new ThreadStart(ThreadFunc));
            FormThread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
