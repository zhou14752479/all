using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 基鹿工具箱
{
    public partial class 表格处理 : Form
    {
        public 表格处理()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                richTextBox3.Text += richTextBox1.Lines[i] + "," + richTextBox2.Lines[i]+"\r\n";
            }
        }
    }
}
