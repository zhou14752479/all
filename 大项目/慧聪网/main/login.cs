using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"huicong"))
            {
                MessageBox.Show("账号或密码错误！");
                return;

            }


            #endregion

            if (textBox1.Text == "6666" && textBox2.Text == "6666")
            {
                MessageBox.Show("登陆成功！");
                main ma = new main();
                ma.Show();
                this.Hide();
            }

            else
            {
                MessageBox.Show("账号或密码错误！");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes");
        }
    }
}
