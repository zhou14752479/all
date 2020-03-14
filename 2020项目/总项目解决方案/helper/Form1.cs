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
            webBrowser1.Navigate("https://login.1688.com/member/signin.htm?tracelog=member_signout_signin");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cookie = method.GetCookies("https://www.1688.com/?spm=a261p.8650866.0.0.66b736c3VipoIr");
            this.Hide();

        }
    }
}
