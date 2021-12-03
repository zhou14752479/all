using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace 图片点击显示
{
    public partial class 图片框 : Form
    {

        string picname = "";
        public 图片框(string picname)
        {
            this.picname = picname;
            InitializeComponent();
        }

        private void 图片框_Load(object sender, EventArgs e)
        {
            if(picname!="多图播放")
            {
                try
                {
                    pictureBox1.Image = Image.FromFile(Application.StartupPath + "/image/"+ picname + ".jpg");
                }
                catch (Exception)
                {

                    try
                    {
                        pictureBox1.Image = Image.FromFile(Application.StartupPath + "/image/"+ picname + ".png");
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("图片地址不存在");
                       
                    }
                }
            }

            else
            {

                timer1.Start();
                timer1.Interval = 1000;
                //多图播放
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(duotu);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
        }
        Thread thread;

        public void duotu()
        {
            DirectoryInfo d = new DirectoryInfo(Application.StartupPath + "/image/多图播放/");
            FileInfo[] files = d.GetFiles();//文件
          
            foreach (FileInfo f in files)
            {
                pictureBox1.Image = Image.FromFile(f.FullName);
                Thread.Sleep(5000);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(duotu);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
