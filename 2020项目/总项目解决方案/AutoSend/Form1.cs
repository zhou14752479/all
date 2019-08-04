using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoSend
{
    public partial class Form1 : Form
    {
        //找窗体
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        //把窗体置于最前
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        //拖动窗体
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);

        }
        //发送消息
        public void sendMessage(int num)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "" || textBox3.Text.Trim() == "")
            {
                MessageBox.Show("有些框框是空的！！！");
            }
            else
            {
                string aioName = textBox1.Text.Trim();  //AIO名
                string info = textBox3.Text;            //要发送的消息
                string str = "";

                IntPtr k = FindWindow(null, aioName);   //查找窗口            
                if (k.ToString() != "0")
                {
                    SetForegroundWindow(k);             //把窗体置于最前
                    for (int i = 1; i <= num; i++)
                    {
                        //str = i + ": " + info;
                        str = info;
                        SendKeys.SendWait(str);
                        SendKeys.Send("{ENTER}");
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    MessageBox.Show("木有找到这个聊天窗口");
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int a = Convert.ToInt32(textBox2.Text);
            sendMessage(a);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }
    }
}
