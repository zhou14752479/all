using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 体育打票软件
{
    public partial class 体育打票软件 : Form
    {
        public 体育打票软件()
        {
            InitializeComponent();
        }

        private void 体育打票软件_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://www.sporttery.cn/jc/jsq/zqhhgg/");
        }
    }
}
