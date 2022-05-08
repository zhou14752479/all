using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
namespace 浙江企业基础信息查询
{
    public partial class 激活 : Form
    {
        public 激活()
        {
            InitializeComponent();
        }


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



        private void Button1_Click(object sender, EventArgs e)
        {
            string jihuoma = method.GetMD5(textBox1.Text.Trim()).ToUpper();
            if (textBox2.Text.Length > 32)
            {

                if (textBox2.Text.Trim().Substring(0, 32) == jihuoma)
                {
                    IniWriteValue("values", "mac", jihuoma.Trim());
                    IniWriteValue("values", "ex", textBox2.Text.Replace(jihuoma, "").Trim());
                    MessageBox.Show("激活成功");
                    查询1 lg = new 查询1();
                    lg.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("激活码错误");
                }
            }
            else
            {
                MessageBox.Show("激活码错误");
            }
        }

        private void 激活_Load(object sender, EventArgs e)
        {
            textBox1.Text = method.GetMD5(method.GetMacAddress()).ToUpper();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
