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

namespace 冷链系统
{
    public partial class Form1 : Form
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







        public Form1()
        {
            InitializeComponent();
        }
        method md = new method();
        private void Form1_Load(object sender, EventArgs e)
        {
            //读取config.ini
            if (ExistINIFile())
            {
                textBox1.Text = IniReadValue("values", "username");
                textBox2.Text = IniReadValue("values", "password");
                textBox7.Text = IniReadValue("values", "time");
                textBox3.Text = IniReadValue("values", "data_ku");
                textBox4.Text = IniReadValue("values", "url");
                textBox5.Text = IniReadValue("values", "data_user");
                textBox6.Text = IniReadValue("values", "data_pass");
            }

          
            md.constr= "Host =" + textBox4.Text.Trim() + ";Database="+textBox3.Text.Trim()+";Username=" + textBox5.Text.Trim() + ";Password=" + textBox6.Text.Trim();
           
            Control.CheckForIllegalCrossThreadCalls = false;
            md.getlogs += new method.GetLogs(setlog);
        }

        public void setlog(string str)
        {

            logtxtBox.Text += str + Environment.NewLine;
        }
        Thread thread;
        Thread thread1;
        Thread thread2;
        private void button1_Click(object sender, EventArgs e)
        {
           

            //写入config.ini配置文件
            IniWriteValue("values","username",textBox1.Text.Trim());
            IniWriteValue("values", "password", textBox2.Text.Trim());
            IniWriteValue("values", "time", textBox7.Text.Trim());
            IniWriteValue("values", "url", textBox4.Text.Trim());
            IniWriteValue("values", "data_ku", textBox3.Text.Trim());
            IniWriteValue("values", "data_user", textBox5.Text.Trim());
            IniWriteValue("values", "data_pass", textBox6.Text.Trim());


            md.adminuser = textBox1.Text.Trim();
            md.adminpass = textBox2.Text.Trim();
            md.Authorization = md.login();
            if (md.Authorization != "")
            {
                setlog(DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss") + "：登录成功");
            }
            else
            {
                setlog(DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss") + "：登录失败");
                return;

            }
            if (thread == null || !thread.IsAlive)
            {
                setlog(DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss") + "：预约管理已启动");
                thread = new Thread(md.yygl);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            if (thread1 == null || !thread1.IsAlive)
            {
                setlog(DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss") + "：仓内作业已启动");
                thread1 = new Thread(md.cnzy);
                thread1.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            if (thread2 == null || !thread2.IsAlive)
            {
                setlog(DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss") + "：出仓管理已启动");
                thread2 = new Thread(md.ccgl);
                thread2.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            timer1.Interval = (Convert.ToInt32(textBox7.Text) * 60 * 1000);
            timer1.Start();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            setlog(DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss") + "：等待本次执行结束定时器停止");
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
            if (thread.IsAlive == true || thread1.IsAlive == true || thread2.IsAlive == true)
            {
                setlog(DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss") + "：等待本次执行结束");
                return;
            }

           
            md.Authorization = md.login();
            if (md.Authorization != "")
            {
                setlog(DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss") + "：登录成功");
            }
            else {
               // setlog(DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss") + "：登录失败");
                return;

            }
            if (thread == null || !thread.IsAlive)
            {
                setlog(DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss") + "：预约管理已启动");
                thread = new Thread(md.yygl);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            if (thread1 == null || !thread1.IsAlive)
            {
                setlog(DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss") + "：仓内作业已启动");
                thread1 = new Thread(md.cnzy);
                thread1.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            if (thread2 == null || !thread2.IsAlive)
            {
                setlog(DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss") + "：出仓管理已启动");
                thread2 = new Thread(md.ccgl);
                thread2.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            md.adminuser = textBox1.Text.Trim();
            md.adminpass = textBox2.Text.Trim();
            md.Authorization = md.login();
            if (thread == null || !thread.IsAlive)
            {
                setlog(DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss") + "：预约管理已启动");
                thread = new Thread(md.yygl);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            if (thread1 == null || !thread1.IsAlive)
            {
                setlog(DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss") + "：仓内作业已启动");
                thread1 = new Thread(md.cnzy);
                thread1.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            if (thread2 == null || !thread2.IsAlive)
            {
                setlog(DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss") + "：出仓管理已启动");
                thread2 = new Thread(md.ccgl);
                thread2.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
