using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zhaopin_58
{
    public partial class webbrowser : Form
    {
        public  string URL;
        public webbrowser(string url)
        {
            InitializeComponent();
            this.URL = url;
        }

        private void webbrowser_Load(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri(URL);
        }
    }
}
