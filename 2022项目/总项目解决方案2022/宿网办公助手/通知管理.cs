using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 宿网办公助手
{
    public partial class 通知管理 : Form
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

        public 通知管理()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            网站监控.mail = textBox1.Text.Trim();
            网站监控.shengyin = checkBox1.Checked.ToString();
            网站监控.tanchuang = checkBox2.Checked.ToString();

            IniWriteValue("values", "mail", textBox1.Text.Trim());


            IniWriteValue("values", "shengyin", checkBox1.Checked.ToString());
            IniWriteValue("values", "tanchuang", checkBox2.Checked.ToString());
            MessageBox.Show("保存成功");
            this.Hide();
        }

        private void 通知管理_Load(object sender, EventArgs e)
        {
           string shengyin= IniReadValue("values", "shengyin");
           string tanchuang= IniReadValue("values", "tanchuang");

            if(shengyin=="True")
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }


            if (tanchuang == "True")
            {
                checkBox2.Checked = true;
            }
            else
            {
                checkBox2.Checked = false;
            }


        }
    }
}
