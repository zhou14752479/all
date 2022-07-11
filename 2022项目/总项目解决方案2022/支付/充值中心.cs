using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 支付
{
    public partial class 充值中心 : Form
    {
        public 充值中心()
        {
            InitializeComponent();
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void 充值中心_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed= true;   
            webBrowser1.Navigate(Application.StartupPath + @"\index\index.html");
        }
    }
}
