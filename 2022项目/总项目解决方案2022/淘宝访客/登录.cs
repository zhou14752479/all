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

namespace 淘宝访客
{
    public partial class 登录 : Form
    {
        public 登录()
        {
            InitializeComponent();
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
        }

        public static string cookie = "";
        private void button1_Click(object sender, EventArgs e)
        {
          cookie = method.GetCookies("https://sycm.taobao.com/flow/monitor/itemsource?dateRange=2022-04-28%7C2022-04-28&dateType=today&selfItemId=&spm=a21ag.11910113.LeftMenu.d604.4d4850a5p4htcU");
            this.Hide();
        }

        private void 登录_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://login.taobao.com/member/login.jhtml");
        }
    }
}
