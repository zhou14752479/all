using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 淘宝实时工具
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

     
        private void openFm2(object sender, EventArgs e)
        {
            Form2 fm2 = new Form2();
            fm2.Show();
        }
    }
}
