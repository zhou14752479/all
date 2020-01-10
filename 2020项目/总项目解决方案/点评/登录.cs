using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 点评
{
    public partial class 登录 : Form
    {
        public string URL;
        public 登录(string url)
        {
            InitializeComponent();
            URL = url;
        }

        private void 登录_Load(object sender, EventArgs e)
        {
            method.SetWebBrowserFeatures(method.IeVersion.IE11);
            webBrowser1.Navigate(this.URL);
            webBrowser1.ScriptErrorsSuppressed = true;
        }
        public static string cookie = "";
        private void Button1_Click(object sender, EventArgs e)
        {
            cookie = method.GetCookies("http://www.dianping.com/shop/27029749");
            textBox1.Text = cookie;
            this.Hide();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = webBrowser1.Url.ToString();
        }
    }
}
