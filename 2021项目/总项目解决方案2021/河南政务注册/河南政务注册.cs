using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 河南政务注册
{
    public partial class 河南政务注册 : Form
    {
        public 河南政务注册()
        {
            InitializeComponent();
        }

        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void 河南政务注册_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate(path+ "index_encrypt.html");
            webBrowser1.ScriptErrorsSuppressed = true;
            //webBrowser1.Navigate("https://login.hnzwfw.gov.cn/tacs-uc/naturalMan/register");
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //textBox1.Text = webBrowser1.Document.InvokeScript("encrypt", new object[] { "430111198706162118" }).ToString();  
           string value=  webBrowser1.Document.InvokeScript("ceshi").ToString();
            MessageBox.Show(value);
        }
    }
}
