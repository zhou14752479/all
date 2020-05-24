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

namespace 淘宝实时工具
{
    public partial class 登陆 : Form
    {
        public 登陆()
        {
            InitializeComponent();
        }


        private void 登陆_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://login.taobao.com/member/login.jhtml");
            method.SetWebBrowserFeatures(method.IeVersion.IE10);
          //  webBrowser1.ScriptErrorsSuppressed = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.cookie = method.GetCookies("https://sycm.taobao.com/ipoll/live/visitor/getRtVisitor.json?_=1585793637702&device=2&limit=20&page=1&token=");
            Form1 fm1 = new Form1();
            fm1.Show();
            this.Hide();
        }
    }
}
