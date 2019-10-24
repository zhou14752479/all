using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 商标生成
{
    public partial class new_renwu : Form
    {
        public new_renwu()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Form1 fm1 = new Form1();
            fm1.treeView1.Nodes.Add(textBox3.Text.Trim());
            
        }
    }
}
