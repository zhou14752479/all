using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 基鹿工具箱
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

        public void update()
        {
            string banbenhao= IniReadValue("values", "banben");
            string url = "http://47.96.189.55/jilusoft/update.php";
            string appName = Util.path + "update.exe";
            string html = Util.GetUrl(url, "utf-8");
            if (html==banbenhao)
            {

            }
            else
            {
                DialogResult dr = MessageBox.Show("发现新版本，是否要更新？", "更新", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    IniWriteValue("values", "banben", html.Trim());
                    //更新文件
                    try
                    {
                        Process proc = Process.Start(appName);


                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    System.Diagnostics.Process.GetCurrentProcess().Kill();

                }
                else
                {
                  
                }
                
            }
        }


        public login()
        {
            InitializeComponent();
            update();
        }
        Util ut = new Util();
        Thread thread;
        public void dologin()
        {
            string html = Util.login(textBox1.Text.Trim(),textBox2.Text.Trim());
            //textBox1.Text = html;
            string code = Regex.Match(html, @"code"":([\s\S]*?),").Groups[1].Value;
            string token = Regex.Match(html, @"token"":""([\s\S]*?)""").Groups[1].Value;
            //string paytime = Regex.Match(html, @"pay_time"":""([\s\S]*?) ").Groups[1].Value;
            //string service_type = Regex.Match(html, @"service_type"":""([\s\S]*?)""").Groups[1].Value;
          
            if (code=="0")
            {
                if (checkBox1.Checked == true)
                {
                    IniWriteValue("values", "user", textBox1.Text.Trim());
                    IniWriteValue("values", "pass", textBox2.Text.Trim());

                }

              
                Util.mobile = textBox1.Text;
                Util.logintoken = token;
                // Util.expiretime = paytime.ToString();

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
           // textBox1.Text = Util.getuser("13777373777", "e8e5319d18d61cc0f339e172cf65baa2370acf24");


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
           

           

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {

                IniWriteValue("values", "autologin", "true");
            }
            else
            {

                IniWriteValue("values", "autologin", "false");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ExistINIFile())
            {
               
                textBox1.Text = IniReadValue("values", "user");
                textBox2.Text = IniReadValue("values", "pass");
                string autologin = IniReadValue("values", "autologin");
                if (autologin == "true")
                {
                    dologin();
                }
            }

            timer1.Stop();  
        }
    }
}
