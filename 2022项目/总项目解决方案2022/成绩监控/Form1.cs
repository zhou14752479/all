using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 成绩监控
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll")]

        public static extern bool Beep(int freq, int duration);
        public Form1()
        {
            InitializeComponent();
        }

        string title = "";
        public void run()
        {
            string url = "http://www.sqsc.gov.cn/scq/gggs/list.shtml";
            string html = method.GetUrl(url,"utf-8");
            string newtitle=Regex.Match(html, @"<div class=""pageList"">([\s\S]*?)title=""([\s\S]*?)""").Groups[2].Value;
            textBox1.Text = DateTime.Now.ToString()+"："+ newtitle;

            if(title=="")
            {
                Beep(800, 2000);
                title = newtitle;
            }

            else if(title!=newtitle && newtitle!="")
            {
                Beep(800, 2000);
                MessageBox.Show(newtitle);
            }

        }

        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
