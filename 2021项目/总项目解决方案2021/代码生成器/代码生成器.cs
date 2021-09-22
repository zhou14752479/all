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

namespace 代码生成器
{
    public partial class 代码生成器 : Form
    {
        public 代码生成器()
        {
            InitializeComponent();
        }
        string cookie = "JSESSIONID=OhAMpK8AHGskLrLnccti7wanhpRjAo6N4GxXhGfYy20ct-nCAyxx!1823864521";
        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = method.PostUrlDefault(textBox1.Text, "size=10&curPage=1&ram=0.008676108244635383", cookie);
        }
    }
}
