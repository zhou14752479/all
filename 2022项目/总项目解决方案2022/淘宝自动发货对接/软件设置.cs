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

namespace 淘宝自动发货对接
{
    public partial class 软件设置 : Form
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        static string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public static void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, inipath);
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public static string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, inipath);
            return temp.ToString();
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public static bool ExistINIFile()
        {
            return File.Exists(inipath);
        }



        public 软件设置()
        {
            InitializeComponent();
        }

        public static int time= 1;
        public static string msg1 = "";
        public static string msg2 = "";
        public static string msg3 = "";

        public static void readconfig()
        {
            if (ExistINIFile())
            {
                time= Convert.ToInt32(IniReadValue("values", "time"));
                msg1= IniReadValue("values", "msg1");
                msg2 = IniReadValue("values", "msg2");
                msg3 = IniReadValue("values", "msg3");
            }
        }


        private void 软件设置_Load(object sender, EventArgs e)
        {
            if (ExistINIFile())
            {
                textBox1.Text = IniReadValue("values", "time");
                textBox2.Text = IniReadValue("values", "msg1");
                textBox3.Text = IniReadValue("values", "msg2");
                textBox4.Text = IniReadValue("values", "msg3");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int time2 = Convert.ToInt32(textBox1.Text) ;
            IniWriteValue("values", "time",time2.ToString() );
            IniWriteValue("values", "msg1", textBox2.Text.ToString());
            IniWriteValue("values", "msg2", textBox3.Text.ToString());
            IniWriteValue("values", "msg3", textBox4.Text.ToString());

            time = time2;
            msg1 = textBox2.Text.ToString();
            msg2 = textBox3.Text.ToString();
            msg3 = textBox4.Text.ToString();
            MessageBox.Show("保存成功！");

        }
    }
}
