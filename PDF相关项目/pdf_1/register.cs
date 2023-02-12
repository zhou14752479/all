using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pdf_1
{
    public partial class register : Form
    {
        public register()
        {

            
            InitializeComponent();
            
        }

        #region 获取Mac地址
        /// <summary>
        /// 获取Mac地址
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            try
            {
                string strMac = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        strMac = mo["MacAddress"].ToString();
                    }
                }
                moc = null;
                mc = null;
                return strMac;
            }
            catch
            {
                return "unknown";
            }
        }

        #endregion

        #region 注册码随机生成函数
        /// <summary>
        /// 注册码随机生成函数
        /// </summary>
        /// <returns></returns>
        public static string Random()

        {
         
            string Hour = DateTime.Now.Hour.ToString();                    //获取当前小时
            string Hour2 = Math.Pow(Convert.ToDouble(Hour), 2).ToString();  //获取当前小时的平方
                           //获取当前小时作为数组索引的字母
            string key = Hour + "1475" + (Hour2) + "2479" + Hour;

            return key;



        }


        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == Random())
            {
                Form2 fm2 = new Form2();
                fm2.Show();
                this.Hide();

                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                FileStream fs2 = new FileStream(path + "//mac.txt", FileMode.Create, FileAccess.Write);//在当前程序运行文件夹内创建文件 
                StreamWriter sw2 = new StreamWriter(fs2);
                sw2.WriteLine(GetMacAddress());//开始写入值
                sw2.Close();
                fs2.Close();
            }

            else
            {
                MessageBox.Show("注册码错误！！");
                return;
            }
        }

        private void register_Load(object sender, EventArgs e)
        {


        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes");
        }
    }
}
