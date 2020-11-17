using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 主程序202006
{
    public partial class 监控 : Form
    {
        public 监控()
        { 
            InitializeComponent();
        }

       
        private void 监控_Load(object sender, EventArgs e)
        {

        }

        public void run()
        {
            string html = method.GetUrlWithCookie("https://wenku.baidu.com/wenkuverify?from=3",textBox1.Text, "gb2312");
            Match match = Regex.Match(html, @"<div class=""join-btn-container"">([\s\S]*?)</button>");
            textBox2.Text += Regex.Replace(match.Groups[1].Value, "<[^>]+>", "")+ "\r\n";


        }

        private void button1_Click(object sender, EventArgs e)
        {
            run();
            timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            run();
        }
    }
}
