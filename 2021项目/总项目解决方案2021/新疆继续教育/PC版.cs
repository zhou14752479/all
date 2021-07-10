using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 新疆继续教育
{
    public partial class PC版 : Form
    {
        public PC版()
        {
            InitializeComponent();
        }

        private void PC版_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://platform.xjrsjxjy.com/");
            webBrowser1.ScriptErrorsSuppressed = true;
        }
    }
}
