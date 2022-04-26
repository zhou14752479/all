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

namespace 主程序202203
{
    public partial class 更改屏幕亮度 : Form
    {


        public const uint WM_SYSCOMMAND = 0x0112;
        public const uint SC_MONITORPOWER = 0xF170;
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint wMsg, uint wParam, int lParam);
        [DllImport("gdi32.dll")]
        private unsafe static extern bool SetDeviceGammaRamp(Int32 hdc, void* ramp);
        private static bool initialized = false;
        private static Int32 hdc;
        private static void InitializeClass()
        {
            if (initialized)
                return;
            hdc = Graphics.FromHwnd(IntPtr.Zero).GetHdc().ToInt32();
            initialized = true;
        }

        public 更改屏幕亮度()
        {
            InitializeComponent();
        }

        private void 更改屏幕亮度_Load(object sender, EventArgs e)
        {

        }
        public static unsafe bool SetBrightness(short brightness)
        {
            InitializeClass();
            if (brightness > 255)
                brightness = 255;
            if (brightness < 0)
                brightness = 0;
            short* gArray = stackalloc short[3 * 256];
            short* idx = gArray;
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 256; i++)
                {
                    int arrayVal = i * (brightness + 128);
                    if (arrayVal > 65535)
                        arrayVal = 65535;
                    *idx = (short)arrayVal;
                    idx++;
                }
            }
            bool retVal = SetDeviceGammaRamp(hdc, gArray);
            return retVal;

        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
           
            SetBrightness((short)trackBar1.Value);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void notifyIcon1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//判断鼠标的按键
            {
                //点击时判断form是否显示,显示就隐藏,隐藏就显示
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.WindowState = FormWindowState.Minimized;
                    this.Hide();
                }
                else if (this.WindowState == FormWindowState.Minimized)
                {
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                    this.Activate();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                //右键退出事件
                if (MessageBox.Show("是否需要关闭程序？", "提示:", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)//出错提示
                {
                    //关闭窗口
                    DialogResult = DialogResult.No;
                    Dispose();
                    Close();
                }
            }


        }

        private void 更改屏幕亮度_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
            return;
        }

        int status = 1;
        private void button1_Click(object sender, EventArgs e)
        {
            if(status==1)
            {
                SetBrightness((short)numericUpDown1.Value);
                status = 2;
                模式文字 moshi=new 模式文字("安静模式");
                moshi.Show();

            }
            else if (status == 2)
            {
                SetBrightness((short)numericUpDown2.Value);
                status = 3;
                模式文字 moshi = new 模式文字("均衡模式");
                moshi.Show();

            }
            else if (status == 3)
            {
                SetBrightness((short)numericUpDown3.Value);
                status = 1;
                模式文字 moshi = new 模式文字("高能模式");
                moshi.Show();

            }
        }

       
    }
}
