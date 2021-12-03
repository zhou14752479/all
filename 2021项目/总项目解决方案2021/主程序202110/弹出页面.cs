using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202110
{
    public partial class 弹出页面 : Form
    {
       
        public 弹出页面(string keyword)
        {
            InitializeComponent();
            this.key = keyword;
        }

        string key;
        private void 弹出页面_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            richTextBox1.Text = 互动易网站监控.zhi ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            string[] str = richTextBox1.Text.Split(new string[] { "(*)" }, StringSplitOptions.RemoveEmptyEntries);
            int p2 = 0;
            for (int i = 0; i < str.Length; i++)
            {
                int p1 = richTextBox1.Text.IndexOf(key, p2);
                if (p1 != -1)
                {
                    richTextBox1.Select(p1, key.Length);
                    richTextBox1.SelectionColor = Color.Red;
                    FontStyle style = richTextBox1.SelectionFont.Style;
                    style = FontStyle.Bold;
                    richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, style);
                    p2 = p1 + 3;
                }
            }
            richTextBox1.Refresh();
        }
    }
}
