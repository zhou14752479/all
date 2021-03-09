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

namespace 常用软件非客户
{
    public partial class 定时打开 : Form
    {
        [DllImport("shell32.dll")]
        public static extern int ShellExecute(IntPtr hwnd, StringBuilder lpszOp, StringBuilder lpszFile, StringBuilder lpszParams, StringBuilder lpszDir, int FsShowCmd);
        public 定时打开()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string nowtime= DateTime.Now.ToLongTimeString();
         
            if (nowtime == textBox3.Text.Trim())
            {
                timer1.Stop();
                //需要打开的地方插入此段代码
                ShellExecute(IntPtr.Zero, new StringBuilder("Open"), new StringBuilder(textBox2.Text), new StringBuilder(""), new StringBuilder(textBox1.Text), 1);
                
            }
        }

        private void 定时打开_Load(object sender, EventArgs e)
        {

        }
    }
}
