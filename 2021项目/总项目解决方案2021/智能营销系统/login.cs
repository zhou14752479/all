using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 智能营销系统
{
    public partial class login : Form
    {

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }


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
            if (uiCheckBox1.Checked == true)
            {
                IniWriteValue("values", "user", uiTextBox1.Text);
                IniWriteValue("values", "pass", uiTextBox2.Text);
            }
            Thread FormThread = new Thread(new ThreadStart(ThreadFunc));
             FormThread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void login_Load(object sender, EventArgs e)
        {
            if (ExistINIFile())
            {
              uiTextBox1.Text = IniReadValue("values", "user");
                uiTextBox2.Text = IniReadValue("values", "pass");
              
            }
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
        private Point mPoint = new Point();
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {

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

        private void uiCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (uiCheckBox1.Checked == true)
            {
                IniWriteValue("values", "user", uiTextBox1.Text);
                IniWriteValue("values", "pass", uiTextBox2.Text);
            }
        }
    }
}
