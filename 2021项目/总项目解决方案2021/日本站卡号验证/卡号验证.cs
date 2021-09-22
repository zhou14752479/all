using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 日本站卡号验证
{
    public partial class 卡号验证 : Form
    {
        public 卡号验证()
        {
            InitializeComponent();
        }

        private void 卡号验证_Load(object sender, EventArgs e)
        {
           // webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("https://jp.triumph.com/login");
        }
    }
}
