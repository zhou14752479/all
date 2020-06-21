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

namespace CefSharp谷歌
{
    public partial class 加群加入助手 : Form
    {
        public 加群加入助手()
        {
            InitializeComponent();
        }
        public ChromiumWebBrowser browser = new ChromiumWebBrowser("http://119.45.18.235/");
        private void 加群加入助手_Load(object sender, EventArgs e)
        {
            browser.Load("http://119.45.18.235/");
            browser.Parent = splitContainer1.Panel2;
            browser.Dock = DockStyle.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            browser.Load("http://119.45.18.235/");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("敬请期待");
        }
    }
}
