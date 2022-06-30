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

namespace 天猫店铺采集
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
            timer1.Start();
           webBrowser1.ScriptErrorsSuppressed = true;
            method.SetFeatures(11000);
            webBrowser1.Navigate("https://login.tmall.com/?redirectURL=https%3A%2F%2Frate.taobao.com%2Fuser-rate-UvCIWOmvbMFx4MF8LMgTT.htm%3Fspm%3Da1z10.1-b.1997427721.d4918097.b6e81d12WZm0oW");
        }

        private void button1_Click(object sender, EventArgs e)
        {
          cookie = method.GetCookies("https://rate.taobao.com/user-rate-UvCIWvmxuMmkWvFcYMWTT.htm?spm=a1z10.1-b-s.1997427721.d4918097.99af3bfc4UkOrm");
            天猫店铺采集.cookielist.Add(cookie);
           // this.Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
            cookie = method.GetCookies("https://rate.taobao.com/user-rate-UvCIWvmxuMmkWvFcYMWTT.htm?spm=a1z10.1-b-s.1997427721.d4918097.99af3bfc4UkOrm");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
            cookie = method.GetCookies("https://rate.taobao.com/user-rate-UvCIWvmxuMmkWvFcYMWTT.htm?spm=a1z10.1-b-s.1997427721.d4918097.99af3bfc4UkOrm");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
