using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 基鹿工具箱
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }
        Util ut = new Util();
        Thread thread;
        public void dologin()
        {
            string html = Util.login(textBox1.Text.Trim(),textBox2.Text.Trim());
            textBox1.Text = html;
            string code = Regex.Match(html, @"code"":([\s\S]*?),").Groups[1].Value;
            
            string paytime = Regex.Match(html, @"pay_time"":""([\s\S]*?) ").Groups[1].Value;
            string service_type = Regex.Match(html, @"service_type"":""([\s\S]*?)""").Groups[1].Value;
           // string expiretime = "";
            if (code=="0")
            {
                Util.expiretime = paytime.ToString();
                基鹿工具箱 main = new 基鹿工具箱();
                main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("登录失败");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            dologin();
            
        }

        private void login_Load(object sender, EventArgs e)
        {
            label1.Parent = pictureBox1;
            label2.Parent = pictureBox1;
            label4.Parent = pictureBox1;
            label5.Parent = pictureBox1;
        }
        private Point mPoint = new Point();
        private void login_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void login_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
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

        private void label3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.asinlu.com/index/service_show.html?id=71");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.asinlu.com/?register");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.asinlu.com/?wechat");
        }
    }
}
