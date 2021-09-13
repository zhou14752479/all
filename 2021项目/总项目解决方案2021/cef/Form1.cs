using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cef
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string path = AppDomain.CurrentDomain.BaseDirectory+"index/";
        private void Form1_Load(object sender, EventArgs e)
        {
            ChromiumWebBrowser browser= new ChromiumWebBrowser(path+"index.html");
            Control.CheckForIllegalCrossThreadCalls = false;
            this.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;

        }
    }
}
