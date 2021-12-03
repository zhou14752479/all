using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 萌推发货
{
    public partial class 登录 : Form
    {
        public 登录()
        {
            InitializeComponent();
        }

        private void 登录_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://mms.mengtuiapp.com/login.html?redirect=https%3A%2F%2Fmms.mengtuiapp.com%2Fv2%2F%3Ft%3D1638061648822%23%2Forders%2Flist");
        }
        public string Autogetuseragent(WebBrowser wb)
        {
            //WebBrowser wb = new WebBrowser();
            //wb.Navigate("about:blank");
            //while (wb.IsBusy) Application.DoEvents();
            object window = wb.Document.Window.DomWindow;
            Type wt = window.GetType();
            object navigator = wt.InvokeMember("navigator", BindingFlags.GetProperty, null, window, new object[] { });
            Type nt = navigator.GetType();
            object userAgent = nt.InvokeMember("userAgent", BindingFlags.GetProperty, null, navigator, new object[] { });
            return userAgent.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            萌推发货.UA = Autogetuseragent(webBrowser1);
            萌推发货.COOKIE= method.GetCookies("https://mms.mengtuiapp.com/v2/?t=1638061648822#/orders/list");
            this.Hide();
        }
    }
}
