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

namespace CsharpSelenium
{
    public partial class 弹窗处理 : Form
    {
        /// <summary>
        /// 找到窗口
        /// </summary>
        /// <param name="lpClassName">窗口类名(例：Button)</param>
        /// <param name="lpWindowName">窗口标题</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 找到窗口
        /// </summary>
        /// <param name="hwndParent">父窗口句柄（如果为空，则为桌面窗口）</param>
        /// <param name="hwndChildAfter">子窗口句柄（从该子窗口之后查找）</param>
        /// <param name="lpszClass">窗口类名(例：Button</param>
        /// <param name="lpszWindow">窗口标题</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        private extern static IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="hwnd">消息接受窗口句柄</param>
        /// <param name="wMsg">消息</param>
        /// <param name="wParam">指定附加的消息特定信息</param>
        /// <param name="lParam">指定附加的消息特定信息</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);


        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, StringBuilder lParam);

        //窗口发送给按钮控件的消息，让按钮执行点击操作，可以模拟按钮点击
        private const int BM_CLICK = 0xF5;
        public static int WM_GETTEXT = 0x000D;
        const int WM_SETTEXT = 0x000C;
        const int WM_GETTEXTLENGTH = 0x000E;

        public 弹窗处理()
        {
            InitializeComponent();
        }



        public void clickButton()
        {
            Task task = new Task(() =>
            {
                while (true)
                {
                    //测试警告框
                    IntPtr maindHwnd = FindWindow(null, "另存为");//主窗口标题
                    if (maindHwnd != IntPtr.Zero)
                    {
                        IntPtr childHwnd = FindWindowEx(maindHwnd, IntPtr.Zero, null, "保存(&S)");//按钮控件标题
                        if (childHwnd != IntPtr.Zero)
                        {
                            SendMessage(childHwnd, BM_CLICK, 0, 0);

                        }
                    }
                }
            });

            task.Start();
        }



  

        private void button1_Click(object sender, EventArgs e)
        {
            clickButton();


           //  buffer.Append("55555555");
           //SendMessage(childHwnd, WM_SETTEXT, buffer_size, buffer);
           //SendMessage(childHwnd, WM_SETTEXT, 0, buffer);

        }

        private void 弹窗处理_Load(object sender, EventArgs e)
        {

        }
    }
}
