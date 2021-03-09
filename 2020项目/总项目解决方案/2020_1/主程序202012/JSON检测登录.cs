using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 主程序202012
{
    public partial class JSON检测登录 : Form
    {
        public JSON检测登录()
        {
            InitializeComponent();
        }
        public string login(string username, string password)
        {
            try
            {
                string url = "http://47.242.69.104/api/do.php?method=login&username=" + username + "&password=" + password;
                string html = method.GetUrl(url,"utf-8");
                return html.Trim();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static string user;
        public static string xiugai(string username, string islogin)
        {
            try
            {
                string url = "http://47.242.69.104/api/do.php?method=xiugai&username=" + username + "&islogin=" + islogin;
                string html = method.GetUrl(url, "utf-8");
                return html.Trim();
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("请输入账号密码");
                return;
            }
            string status =login(textBox1.Text.Trim(), textBox2.Text.Trim());

            if (status.Contains("true"))
            {
                user = textBox1.Text.Trim();
                 网址JSON检测 ma = new 网址JSON检测();
                ma.Show();
               // xiugai(user, "1");
                this.Hide();
            }
            else
            {
                MessageBox.Show(status);
            }
        }

        private void JSON检测登录_Load(object sender, EventArgs e)
        {

        }
    }
}
