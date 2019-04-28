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
    public partial class register : Form
    {
        public register()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == method.Random(textBox1.Text).Trim())
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

        private void Register_Load(object sender, EventArgs e)
        {
            textBox1.Text = method.GetMacAddress();
        }
    }
}
