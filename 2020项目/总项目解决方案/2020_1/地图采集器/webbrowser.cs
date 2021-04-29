using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 地图采集器
{
    public partial class webbrowser : Form
    {
        public webbrowser()
        {
            InitializeComponent();
        }

        private void webbrowser_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://lbs.qq.com/tool/getpoint/get-point.html");
        }
    }
}
