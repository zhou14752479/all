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
            webBrowser1.Navigate("https://pub.alimama.com/promo/search/index.htm?spm=a219t.11816995.1998910419.de727cf05.2a8f75a5Ac2Hj5");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cookie = method.GetCookies("http://wawa.xinjy01.com/ct-data/openCodeList?shortName=qqtxffc&num=50");
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
