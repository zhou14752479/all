using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 寺库
{
    public partial class web : Form
    {
        string URL;
        public web(string url)
        {
            this.URL = url;
            InitializeComponent();
           
        }

        private void Web_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate(URL);

        }
    }
}
