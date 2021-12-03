using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202110
{
    public partial class 屏幕坐标颜色 : Form
    {
        [DllImport("user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetDC(int hwnd);
        [DllImport("gdi32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetPixel(int hdc, int X, int y);

        private struct POINTAPI //确定坐标
        {
            private int X;
            private int y;
        }
        [DllImport("user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)] //确定坐标
        private static extern int ReleaseDC(int hwnd, int hdc);
        POINTAPI P; //确定坐标

        [DllImport("user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int ScreenToClient(int hwnd, ref POINTAPI lpPoint);
        [DllImport("user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int WindowFromPoint(int xPoint, int yPoint);



        public 屏幕坐标颜色()
        {
            InitializeComponent();
        }

        private void 屏幕坐标颜色_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = "X=" + System.Windows.Forms.Control.MousePosition.X.ToString() + " " +
  "Y=" + System.Windows.Forms.Control.MousePosition.Y.ToString();

            int blue;
            int green;
            int red;
            int hD;
            int h;
            int c;
            int a;
            int b;
            a = Convert.ToInt32(System.Windows.Forms.Control.MousePosition.X.ToString());
            b = Convert.ToInt32(System.Windows.Forms.Control.MousePosition.Y.ToString());
            h = WindowFromPoint(a, b);
            hD = GetDC(h);
            ScreenToClient(h, ref P);
            c = GetPixel(hD, a, b);

            red = c % 256;

            green = (c / 256) % 256;

            blue = c / 256 / 256;

            if (red != -1 && green != -1 && blue != -1)
            {
                textBox2.BackColor = System.Drawing.Color.FromArgb(red, green, blue);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
