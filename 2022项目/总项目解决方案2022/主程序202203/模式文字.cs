using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202203
{
    public partial class 模式文字 : Form
    {
        string wenzi = "安静模式";
        public 模式文字(string wenzi)
        {
           
            InitializeComponent();
            label1.Text = wenzi;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
