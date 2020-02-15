using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 通用项目
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            webBrowser1.ScriptErrorsSuppressed = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri("https://i.qq.com/");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri("https://user.qzone.qq.com/852266010/"); 
        }
    }
}
