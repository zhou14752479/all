using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace helper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string cookie = "";

        private void Form1_Load(object sender, EventArgs e)
        {
            method.SetWebBrowserFeatures(method.IeVersion.IE10);
            webBrowser1.ScriptErrorsSuppressed = true;
            //webBrowser1.Navigate("https://login.1688.com/member/signin.htm?tracelog=member_signout_signin");
            webBrowser1.Navigate("http://wawa.xinjy01.com/login");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cookie = method.GetCookies("http://wawa.xinjy01.com/ct-data/openCodeList?shortName=qqtxffc&num=50");
            this.Hide();

        }
    }
}
