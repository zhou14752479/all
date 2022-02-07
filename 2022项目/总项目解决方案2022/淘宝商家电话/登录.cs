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

namespace 淘宝商家电话
{
    public partial class 登录 : Form
    {
        public 登录()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }


        //声明传递
        public delegate void SetCookie(string cookie);
        public SetCookie setcookie;



        public static string cookie = "";
        private void button1_Click(object sender, EventArgs e)
        {
            cookie = method.GetCookies("https://item.manager.taobao.com/taobao/manager/render.htm?pagination.current=1&pagination.pageSize=20&tab=in_stock&table.sort.endDate_m=desc");
            //传递信号
            setcookie(cookie);
            this.Hide();
        }

        private void 登录_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;

            webBrowser1.Navigate("https://item.manager.taobao.com/taobao/manager/render.htm?tab=in_stock&table.sort.endDate_m=desc&spm=a217wi.openworkbeanchtmall");
        }
    }
}
