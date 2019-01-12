using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fang
{
    public partial class wenzigundong : Form
    {
        public wenzigundong()
        {
            InitializeComponent();
            
        }

        public static string wenzi1;
        public static string wenzi2;
        public static string wenzi3;
        public static string wenzi4;
        public static string wenzi5;
        public static string wenzi6;

       

        private void timer1_Tick(object sender, EventArgs e)
        {
            string str = label1.Text;
            char a = str[0];
            string b = str.Substring(1);

            if (a== wenzi1[wenzi1.Length - 1])
            {
                timer1.Stop();
                timer2.Start();
            }
            label1.Text = b + a;
                  

        }

        private void wenzigundong_Load(object sender, EventArgs e)
        {
            this.TopMost = true; //最上层

            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            if (File.Exists(path + "//1.txt"))
            {

                StreamReader sr = new StreamReader(path + "//1.txt",Encoding.Default, false);
                //一次性读取完 
                string texts1 = sr.ReadToEnd();
                label1.Text = texts1;
                wenzi1 = texts1;
                sr.Close();
            }
            if (File.Exists(path + "//2.txt"))
            {

                StreamReader sr = new StreamReader(path + "//2.txt", Encoding.Default, false);
                //一次性读取完 
                string texts1 = sr.ReadToEnd();
                label2.Text = texts1;
                wenzi2 = texts1;
                sr.Close();
            }
            if (File.Exists(path + "//3.txt"))
            {

                StreamReader sr = new StreamReader(path + "//3.txt", Encoding.Default, false);
                //一次性读取完 
                string texts1 = sr.ReadToEnd();
                label3.Text = texts1;
                wenzi3 = texts1;
                sr.Close();
            }
            if (File.Exists(path + "//4.txt"))
            {

                StreamReader sr = new StreamReader(path + "//4.txt", Encoding.Default, false);
                //一次性读取完 
                string texts1 = sr.ReadToEnd();
                label4.Text = texts1;
                wenzi4 = texts1;
                sr.Close();
            }
            if (File.Exists(path + "//5.txt"))
            {

                StreamReader sr = new StreamReader(path + "//5.txt", Encoding.Default, false);
                //一次性读取完 
                string texts1 = sr.ReadToEnd();
                label5.Text = texts1;
                wenzi5 = texts1;
                sr.Close();
            }
            if (File.Exists(path + "//6.txt"))
            {

                StreamReader sr = new StreamReader(path + "//6.txt", Encoding.Default, false);
                //一次性读取完 
                string texts1 = sr.ReadToEnd();
                label6.Text = texts1;
                wenzi6 = texts1;
                sr.Close();
            }

            Thread.Sleep(3000);
            timer1.Start();

        }



        private void timer2_Tick(object sender, EventArgs e)
        {
            string str = label2.Text;
            char a = str[0];
            string b = str.Substring(1);

            if (a == wenzi2[wenzi2.Length - 1])
            {
                timer2.Stop();
                timer3.Start();
            }
            label2.Text = b + a;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            string str = label3.Text;
            char a = str[0];
            string b = str.Substring(1);

            if (a == wenzi3[wenzi3.Length - 1])
            {
                timer3.Stop();
                timer4.Start();
            }
            label3.Text = b + a;
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            string str = label4.Text;
            char a = str[0];
            string b = str.Substring(1);

            if (a == wenzi4[wenzi4.Length - 1])
            {
                timer4.Stop();
                timer5.Start();
            }
            label4.Text = b + a;
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            string str = label5.Text;
            char a = str[0];
            string b = str.Substring(1);

            if (a == wenzi5[wenzi5.Length - 1])
            {
                timer5.Stop();
                timer6.Start();
            }
            label5.Text = b + a;
        }

        private void timer6_Tick(object sender, EventArgs e)
        {
            string str = label6.Text;
            char a = str[0];
            string b = str.Substring(1);

            if (a == wenzi6[wenzi6.Length - 1])
            {
                timer6.Stop();
                timer1.Start();
            }
            label6.Text = b + a;
        }
    }
}
