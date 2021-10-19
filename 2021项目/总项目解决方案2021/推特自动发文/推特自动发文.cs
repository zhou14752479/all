using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 推特自动发文
{
    public partial class 推特自动发文 : Form
    {
        public 推特自动发文()
        {
            InitializeComponent();
        }

        private void 推特自动发文_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("https://www.twitter.com/");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;



            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("获取登录信息失败");
        }
    }
}
