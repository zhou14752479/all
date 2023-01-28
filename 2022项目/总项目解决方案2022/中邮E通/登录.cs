using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 中邮E通
{
    public partial class 登录 : Form
    {
        public 登录()
        {
            InitializeComponent();
        }

        public static string cookie = "";

        private void button1_Click(object sender, EventArgs e)
        {
            cookie = method.GetCookies("http://211.156.200.95:8081/userlogin.do");
        }

        private void 登录_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("http://211.156.200.95:8081/");
        }
    }
}
