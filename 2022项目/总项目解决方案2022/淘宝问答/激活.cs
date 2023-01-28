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

namespace 淘宝问答
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



        private void 激活_Load(object sender, EventArgs e)
        {
            string md5 = method.GetMD5(method.GetMacAddress());
            textBox1.Text = md5.ToUpper();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string value2 = method.Base64Decode(Encoding.GetEncoding("utf-8"),textBox2.Text);
                string value = method.Base64Decode(Encoding.GetEncoding("utf-8"), value2);
                string[] text = value.Split(new string[] { "*" }, StringSplitOptions.None);
                if(text[0] == textBox1.Text)
                {
                    MessageBox.Show("激活成功");
                    IniWriteValue("value","key",textBox2.Text);
                    淘宝问答 tb=new 淘宝问答(); 
                    tb.Show();  
                }
                else
                {
                    MessageBox.Show("激活失败"); 
                }
            }
            catch (Exception)
            {

                MessageBox.Show("激活失败"); 
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(textBox1.Text); //复制
        }
    }
}
