using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 启动程序
{
    public partial class 微博转赞评 : Form
    {
        public 微博转赞评()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            添加微博 tj = new 添加微博();
            tj.Show();

        }
    }
}
