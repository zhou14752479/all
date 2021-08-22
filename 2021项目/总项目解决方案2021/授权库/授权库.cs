using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 授权库
{
    public partial class 授权库 : Form
    {
        public 授权库()
        {
            InitializeComponent();
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            新增 add = new 新增();
            //add.MdiParent = this;
            //add.Dock = DockStyle.Fill;
            add.Show();

        }
    }
}
