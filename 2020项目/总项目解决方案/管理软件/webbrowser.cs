using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 管理软件
{
    public partial class webbrowser : Form
    {
        public string URL;
        public webbrowser(string url)
        {
            InitializeComponent();
            URL = url;
        }

        private void Webbrowser_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate(URL);
            webBrowser1.ScriptErrorsSuppressed = true;
        }
    }
}
