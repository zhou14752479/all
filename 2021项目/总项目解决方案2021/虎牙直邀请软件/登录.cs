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

namespace 虎牙直邀请软件
{
    public partial class 登录 : Form
    {
        public 登录()
        {
            InitializeComponent();
        }

        private void 登录_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("https://ow.huya.com/#/guru/contract/signed");
        }

        private void 登录_FormClosing(object sender, FormClosingEventArgs e)
        {
           虎牙直播邀请.cookie = method.GetCookies("https://ow.huya.com/#/guru/contract/signed");
        }
    }
}
