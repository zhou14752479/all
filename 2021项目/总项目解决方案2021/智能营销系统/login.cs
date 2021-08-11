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
    }
}
