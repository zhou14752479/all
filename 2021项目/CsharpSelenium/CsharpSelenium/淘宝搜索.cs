using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsharpSelenium
{
    public partial class 淘宝搜索 : Form
    {
        public 淘宝搜索()
        {
            InitializeComponent();
        }

        Thread thread;
        bool zanting = true;
        bool status = true;

        private void button6_Click(object sender, EventArgs e)
        {
        //    status = true;
        //    if (thread == null || !thread.IsAlive)
        //    {

        //        thread = new Thread(run);
        //        thread.Start();
        //        Control.CheckForIllegalCrossThreadCalls = false;
        //    }
        }

        

        private void 淘宝搜索_Load(object sender, EventArgs e)
        {

        }
    }
}
