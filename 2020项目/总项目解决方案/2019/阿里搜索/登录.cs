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

namespace 阿里搜索
{
    public partial class 登录 : Form
    {
        public 登录()
        {
            InitializeComponent();
        }
        public static string cookie = "";
        private void 登录_Load(object sender, EventArgs e)
        {
            method.SetWebBrowserFeatures(method.IeVersion.IE11);
            webBrowser1.Url = new Uri("https://login.1688.com/member/signin.htm");
            webBrowser1.ScriptErrorsSuppressed = true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            cookie =  method.GetCookies("https://dnzmjx.1688.com/page/contactinfo.htm");
            this.Hide();
        }
    }
}
