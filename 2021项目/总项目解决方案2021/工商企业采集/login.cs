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

namespace 工商企业采集
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
        public string login2(string user, string pass)
        {

            string html = method.GetUrl("http://www.acaiji.com/shangxueba/shangxueba.php?method=login&username=" + user + "&password=" + pass, "utf-8");
            return html;


        }


        public bool jiance()
        {

            string jihuoma = method.GetMD5(method.GetMacAddress());

            if (ExistINIFile())
            {
                string secret = IniReadValue("values", "secret");
                if (jihuoma == secret)
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
            else
            {
                IniWriteValue("values", "secret", jihuoma);
                return true;

            }

        }


        private void login_btn_Click(object sender, EventArgs e)
        {
          
            if (jiance() == false)
            {
                MessageBox.Show("当前机器未绑定账号");
                return;
            }

            if (user_text.Text == "" || pass_text.Text == "")
            {
                MessageBox.Show("请输入账号和密码");
                return;
            }
            string html = login2(user_text.Text.Trim(), pass_text.Text.Trim());
            MessageBox.Show(html.Trim());
            if (html.Contains("登录成功"))
            {
                工商企业采集 gs = new 工商企业采集();
                gs.Show();
            }
           
        }
    }
}
