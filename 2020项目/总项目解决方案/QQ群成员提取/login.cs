﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace QQ群成员提取
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

       
       
        private void login_Load(object sender, EventArgs e)
        {
           


            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("https://qun.qq.com/member.html#");
            timer1.Start();
          
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (webBrowser1.Url != null)
            //{
            //    string[] text = webBrowser1.Url.ToString().Split(new string[] { "#" }, StringSplitOptions.None);
            //    if (text.Length > 1)
            //    {
            //        if (text[0] == "https://qun.qq.com/member.html")
            //        {
            //            this.Hide();
            //            timer1.Stop();
            //            Form1 fm1 = new Form1();
            //            fm1.COOKIE = method.GetCookies("https://qun.qq.com/member.html");
            //            fm1.Show();



            //        }
            //    }
            //}

            string cookie = method.GetCookies("https://qun.qq.com/member.html");
           // textBox1.Text = cookie;
            if (cookie.Contains("pt4_token"))
            {
                this.Hide();
                timer1.Stop();
                Form1 fm1 = new Form1();
                fm1.COOKIE = cookie;
              
                fm1.Show();
            }
        }


    }
}
