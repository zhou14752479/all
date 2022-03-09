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

namespace 基鹿工具箱
{
    public partial class 基鹿工具箱 : Form
    {
        public 基鹿工具箱()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 6;
        }
        private Point mPoint = new Point();
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }

        private void 基鹿工具箱_Load(object sender, EventArgs e)
        {
            this.tabControl1.Region = new Region(new RectangleF(this.tabPage1.Left, this.tabPage1.Top, this.tabPage1.Width, this.tabPage1.Height));
            button1.Click += new System.EventHandler(btn_Click);
            button2.Click += new System.EventHandler(btn_Click);
            button3.Click += new System.EventHandler(btn_Click);
            button4.Click += new System.EventHandler(btn_Click);
            button5.Click += new System.EventHandler(btn_Click);
            button6.Click += new System.EventHandler(btn_Click);
            button7.Click += new System.EventHandler(btn_Click);
        }
        protected void btn_Click(object sender, EventArgs e)
        {
            foreach (Control ctl in panel2.Controls)
            {
                if (ctl is Button)
                {
                    if(ctl==(Button)sender)
                    {
                        ctl.BackColor = Color.LightGray;
                        ctl.ForeColor = Color.Black;
                    }
                    else
                    {
                       
                        ctl.BackColor = Color.FromArgb(255, 128, 0);
                        ctl.ForeColor = Color.White;
                    }
                   
                }
                

            }
           

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
           
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectedIndex = 7;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 7;
        }

        Thread thread;
        private void button12_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(aliexpress);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }


       
        public void aliexpress()
        {
            StreamReader sr = new StreamReader(key_txtbox.Text, Util.EncodingType.GetTxtType(key_txtbox.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] keywords = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存

            foreach (string keyword in keywords)
            {
                for (int page = 1; page < 10; page++)
                {
                    string url = "https://www.aliexpress.com/wholesale?trafficChannel=main&d=y&CatId=0&SearchText=" + System.Web.HttpUtility.UrlEncode(keyword) + "&ltype=wholesale&SortType=default&page=" + page;
                    string html = Util.GetUrl(url, "utf-8");
                    MatchCollection uids = Regex.Matches(html, @"""productId"":([\s\S]*?),");
                    for (int i = 0; i < uids.Count; i++)
                    {
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(keyword);
                        lv1.SubItems.Add(uids[i].Groups[1].Value);
                    }

                    Thread.Sleep(500);
                }
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                key_txtbox.Text = openFileDialog1.FileName;
               
            }
        }
    }
}
