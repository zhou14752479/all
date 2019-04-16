using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }
        #region 注册码随机生成函数
        /// <summary>
        /// 注册码随机生成函数
        /// </summary>
        /// <returns></returns>
        public static string Random(string mac)

        {

            string Hour = DateTime.Now.Hour.ToString();                    //获取当前小时
            string Hour2 = Math.Pow(Convert.ToDouble(Hour), 2).ToString();  //获取当前小时的平方
            //string zimu = array[DateTime.Now.Hour];                        //获取当前小时作为数组索引的字母
            string key = Hour + "1475" + (Hour2)  + "2479" + Hour;

            return key;



        }


        #endregion

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox2.Text.Trim() == Random(textBox1.Text).Trim())
            {

                RegistryKey regkey = Registry.CurrentUser.CreateSubKey("zhucema");//要创建一个特殊的字符串，防止与系统或其他程序写注册表名称相同，如果存在则更新值。
                regkey.SetValue("mac", textBox1.Text.Trim());
                regkey.Close();
                MessageBox.Show("注册成功！请重启软件！");
                this.Close();


            }
            else
            {
                MessageBox.Show("注册码错误！");
                return;
            }
        }
    }
}
