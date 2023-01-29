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

namespace 家乐园开票
{
    public partial class 登录 : Form
    {
        public 登录()
        {
            InitializeComponent();
        }

        private void 登录_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://fapiao.subuy.com/bg/wechatInvoiceController.do?cxddxx=");
        }

        private void button1_Click(object sender, EventArgs e)
        {
           家乐园开票.cookie= method.GetCookies("https://10.4.188.1/portal/");
        }
    }
}
