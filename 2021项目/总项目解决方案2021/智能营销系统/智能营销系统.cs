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

namespace 智能营销系统
{
    public partial class 智能营销系统 : Form
    {
        public 智能营销系统()
        {
            InitializeComponent();
            webBrowser1.ScriptErrorsSuppressed = true;
            tupianlunbo();
            panel2.Visible = true;
            panel2.Width = 1185;
            panel2.Height = 578;
            panel2.Dock = DockStyle.Fill;
            panel3.Visible = false;
            panel4.Visible = false;


            uiHeaderButton1.MouseHover += new EventHandler(MouseHover_Event);
            uiHeaderButton1.MouseLeave += new EventHandler(MouseLeave_Event);

            uiHeaderButton2.MouseHover += new EventHandler(MouseHover_Event);
            uiHeaderButton2.MouseLeave += new EventHandler(MouseLeave_Event);

            uiHeaderButton3.MouseHover += new EventHandler(MouseHover_Event);
            uiHeaderButton3.MouseLeave += new EventHandler(MouseLeave_Event);

            uiHeaderButton4.MouseHover += new EventHandler(MouseHover_Event);
            uiHeaderButton4.MouseLeave += new EventHandler(MouseLeave_Event);

            uiHeaderButton5.MouseHover += new EventHandler(MouseHover_Event);
            uiHeaderButton5.MouseLeave += new EventHandler(MouseLeave_Event);

            uiHeaderButton6.MouseHover += new EventHandler(MouseHover_Event);
            uiHeaderButton6.MouseLeave += new EventHandler(MouseLeave_Event);

            uiHeaderButton7.MouseHover += new EventHandler(MouseHover_Event);
            uiHeaderButton7.MouseLeave += new EventHandler(MouseLeave_Event);

            uiHeaderButton8.MouseHover += new EventHandler(MouseHover_Event);
            uiHeaderButton8.MouseLeave += new EventHandler(MouseLeave_Event);
        }

        private void MouseHover_Event(object sender, EventArgs e)
        {
            ((Sunny.UI.UIHeaderButton)sender).FillColor = Color.Gray;
        }


        private void MouseLeave_Event(object sender, EventArgs e)
        {
            ((Sunny.UI.UIHeaderButton)sender).FillColor = Color.Transparent;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes&from=message&isappinstalled=0";
            System.Diagnostics.Process.Start( url);
        }
        private Point mPoint = new Point();
        private void 智能营销系统_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void 智能营销系统_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
              
            }

        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void uiHeaderButton1_Click(object sender, EventArgs e)
        {
            tupianlunbo();
            panel2.Visible = true;
            panel2.Width= 1185;
            panel2.Height = 578;
            panel2.Dock = DockStyle.Fill;
            panel3.Visible = false;
            panel4.Visible = false;
        }

        private void uiHeaderButton2_Click(object sender, EventArgs e)
        {
            tupianlunbo();
            panel3.Visible = true;
            panel3.Width = 1185;
            panel3.Height = 578;
            panel3.Dock = DockStyle.Fill;
            panel2.Visible = false;
            panel4.Visible = false;
        }

        private void uiHeaderButton3_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
            panel4.Width = 1185;
            panel4.Height = 578;
            panel4.Dock = DockStyle.Fill;
            panel2.Visible = false;
            panel3.Visible = false;
            webBrowser1.Navigate("http://123.57.167.138/member.php?act=login&");
        }


        /// <summary>
        /// 改变图片
        /// </summary>
        /// <param name="img">图片</param>
        /// <param name="millisecondsTimeOut">切换图片间隔时间</param>
        private void ChangeImage(Image img, int millisecondsTimeOut)
        {
            this.Invoke(new Action(() =>
            {
                pictureBox1.Image = img;
            })
                );
            Thread.Sleep(millisecondsTimeOut);
        }
        Thread th;
        public void tupianlunbo()
        {

            //timer1.Enabled = true;
            th = new Thread
                (
                    delegate ()
                    {
                        // 3就是要循环轮数了
                        for (int i = 0; i < 3; i++)
                        {
                            //调用方法

                            ChangeImage(Image.FromFile(path + "image/1.jpg"), 1000);
                            ChangeImage(Image.FromFile(path + "image/2.jpg"), 1000);
                            ChangeImage(Image.FromFile(path + "image/3.jpg"), 1000);
                        }
                    }
                );
            th.IsBackground = true;
            th.Start();
        }

        private void uiHeaderButton4_Click(object sender, EventArgs e)
        {
           
            webBrowser1.Navigate("http://132.232.11.181/");
        }

        private void uiHeaderButton5_Click(object sender, EventArgs e)
        {
           
            webBrowser1.Navigate("http://dy.zhangyou.ltd/app/index.php?c=entry&do=mobile&i=6&m=kz_mgcjbp&r=login");
        }

        private void uiHeaderButton6_Click(object sender, EventArgs e)
        {
           
            webBrowser1.Navigate("http://123.57.167.138/member.php?act=login&");
        }

        private void uiHeaderButton7_Click(object sender, EventArgs e)
        {
           
            webBrowser1.Navigate("http://123.57.167.138/member.php?act=login&");
        }

        private void uiHeaderButton8_Click(object sender, EventArgs e)
        {
           
            webBrowser1.Navigate("http://123.57.167.138/member.php?act=login&");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            groupBox1.Visible = true;
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.BackgroundImage = Image.FromFile(path +"image/金色奖杯.jpg");
            groupBox1.Visible = false;
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            groupBox1.Visible = false;
            this.BackgroundImage = Image.FromFile(path + "image/科技之光.jpg");
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            groupBox1.Visible = false;
            this.BackgroundImage = Image.FromFile(path + "image/蓝色梦幻.jpg");
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            groupBox1.Visible = false;
            this.BackgroundImage = Image.FromFile(path + "image/人工智能.jpg");
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            groupBox1.Visible = false;
            this.BackgroundImage = Image.FromFile(path + "image/沙滩大海.jpg");
        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start( "https://www.baidu.com");
        }

        private void 智能营销系统_Load(object sender, EventArgs e)
        {

        }
    }
}
