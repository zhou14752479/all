using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace helper
{
    public partial class cookieBrowser : Form
    {
        public static string webUrl = "";
        public static string cookie = "";
        public cookieBrowser(string url)
        {
            InitializeComponent();
            webUrl = url;
            method.SetFeatures(10000);
            webBrowser1.ScriptErrorsSuppressed = true;

            webBrowser1.Url = new Uri(webUrl);
          //  webBrowser1.Navigate(webUrl);
        }

        private void cookieBrowser_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cookie = method.GetCookies(webUrl);
            this.Hide();

        }
    }
}
