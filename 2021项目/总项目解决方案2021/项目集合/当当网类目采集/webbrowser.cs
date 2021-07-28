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

namespace 当当网类目采集
{
    public partial class webbrowser : Form
    {
        public webbrowser()
        {
            InitializeComponent();
        }

        public static string cookie="";
        private void button1_Click(object sender, EventArgs e)
        {
          
            cookie = method.GetCookies("http://product.dangdang.com/25060451.html");
            this.Hide();
        }

        private void webbrowser_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://login.dangdang.com/signin.aspx");
            method.SetFeatures(11000);
            //webBrowser1.ScriptErrorsSuppressed = true;
        }

        private void webbrowser_KeyPress(object sender, KeyPressEventArgs e)
        {
         
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (webBrowser1.StatusText.Contains("signin"))
            {
                webBrowser1.ScriptErrorsSuppressed = false;
            }
            else
            {
                webBrowser1.ScriptErrorsSuppressed = true;

            }
        }
    }
}
