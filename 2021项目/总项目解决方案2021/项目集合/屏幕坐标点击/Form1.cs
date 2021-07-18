using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 屏幕坐标点击
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.TabIndex = 0;
           textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < Convert.ToInt32(textBox3.Text); i++)
            {
                string[] text = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int j = 0; j < text.Length; j++)
                {
                    string[] position = text[j].Split(new string[] { "," }, StringSplitOptions.None);
                    if (position.Length > 1)
                    {

                        // MouseFlag.MouseLefClickEvent(9, 1059, 0);
                        int positionx = Convert.ToInt32(position[0]);
                        int positiony = Convert.ToInt32(position[1]);
                        MouseFlag.MouseLefClickEvent(positionx, positiony, 0);
                        Thread.Sleep(2000);
                    }
                }
            }

        }

      

        private void timer1_Tick(object sender, EventArgs e)
        {
            int x = Control.MousePosition.X;
            int y = Control.MousePosition.Y;
            textBox1.Text = x + "," + y;
        }



        method md = new method();

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //if条件检测按下的是不是Enter键
            if (e.KeyCode == Keys.Enter)
            {
                textBox2.Text += textBox1.Text + "\r\n";

            }
        }



        string path = AppDomain.CurrentDomain.BaseDirectory;
      
        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
        }

        public void readtxt()
        {
            if (File.Exists(path + "code.txt"))
            {
                StreamReader sr = new StreamReader(path + "code.txt", myDLL.method.EncodingType.GetTxtType(path + "code.txt"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
               
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
                File.Delete(path + "code.txt");
                md.run(1);
              
               // Process.Start(path + "新疆专技继续教育.lnk");
               
               
            }
        }
        Thread thread;
        private void timer2_Tick(object sender, EventArgs e)
        {

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(readtxt);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
