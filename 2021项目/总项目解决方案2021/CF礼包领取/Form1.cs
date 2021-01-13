using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace CF礼包领取
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
     
     


        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Parent = panel1;
            method.SetFeatures(9000);
            webBrowser1.Navigate("https://xui.ptlogin2.qq.com/cgi-bin/xlogin?proxy_url=https%3A//qzs.qq.com/qzone/v6/portal/proxy.html&daid=5&&hide_title_bar=1&low_login=0&qlogin_auto_login=1&no_verifyimg=1&link_target=blank&appid=549000912&style=22&target=self&s_url=https%3A%2F%2Fqzs.qq.com%2Fqzone%2Fv5%2Floginsucc.html%3Fpara%3Dizone&pt_qr_app=手机QQ空间&pt_qr_link=https%3A//z.qzone.com/download.html&self_regurl=https%3A//qzs.qq.com/qzone/v6/reg/index.html&pt_qr_help_link=https%3A//z.qzone.com/download.html&pt_no_auth=0");
            button1.Click+= new System.EventHandler(btn_Click);
            button2.Click += new System.EventHandler(btn_Click);
            button3.Click += new System.EventHandler(btn_Click);
            button4.Click += new System.EventHandler(btn_Click);
            button5.Click += new System.EventHandler(btn_Click);
            button6.Click += new System.EventHandler(btn_Click);

        }

        protected void btn_Click(object sender, EventArgs e)
        {
            foreach (Control ctl in panel1.Controls)
            {
                if (ctl is Button)
                {
                     ctl.ForeColor = Color.FromArgb(169, 169, 169);
                   
                }

            }
            ((Control)sender).ForeColor = Color.White;

        }



        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
