using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _58二手房
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
           
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("http://www.acaiji.com/index/index/login.html");
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(getTitle);
        }

        private void getTitle(object sender, EventArgs e)
        {

            System.IO.StreamReader getReader = new System.IO.StreamReader(this.webBrowser1.DocumentStream, System.Text.Encoding.GetEncoding("gb2312"));

            string html = getReader.ReadToEnd();

            // Match title = Regex.Match(html, @"<title>([\s\S]*?)</title>");
            if (html.Contains("success login"))
            {
                Form1 fm1 = new Form1();
                fm1.Show();

                //商铺 sp = new 商铺();
                //sp.Show();

                this.Hide();
            }


        }


    }
}
