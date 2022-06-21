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

namespace douyinSelenium
{
    public partial class 卡密 : Form
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

        public 卡密()
        {
            InitializeComponent();
        }


        public void login()

        {
           button1.Enabled = false; 

            string kami = textBox1.Text.Trim();
            string msg = function.login(kami);
            if (msg == "验证成功")
            {
                IniWriteValue("values", "kami", kami);
                主控端 main = new 主控端();
                main.Show();
                this.Hide();
            }
            else
            {
                button1.Enabled = true;
                MessageBox.Show(msg);
            }
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            login();
        }

        private void 卡密_Load(object sender, EventArgs e)
        {
            if (ExistINIFile())
            {
                textBox1.Text = IniReadValue("values", "kami");

            }
        }
    }
}
