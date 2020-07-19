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
            //method.SetWebBrowserFeatures(method.IeVersion.IE10);
            method.SetFeatures(1100);
            webBrowser1.ScriptErrorsSuppressed = true;
            //webBrowser1.Navigate("https://login.1688.com/member/signin.htm?tracelog=member_signout_signin");
            webBrowser1.Navigate("https://www.nike.com/cn/t/air-zoom-bb-nxt-ep-%E7%94%B7-%E5%A5%B3%E7%AF%AE%E7%90%83%E9%9E%8B-Wp9dZd/CK5708-001");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cookie = method.GetCookies("https://www.nike.com/cn/t/air-zoom-bb-nxt-ep-%E7%94%B7-%E5%A5%B3%E7%AF%AE%E7%90%83%E9%9E%8B-Wp9dZd/CK5708-001");
            this.Hide();

        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            string url = this.webBrowser1.StatusText;
            this.webBrowser1.Url = new Uri(url);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
