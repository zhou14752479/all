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
    public partial class main : Form
    {

        [DllImport("User32.dll")]
        public static extern int GetWindowText(IntPtr WinHandle, StringBuilder Title, int size);
        [DllImport("user32.dll")]
        public static extern int GetClassName(IntPtr WinHandle, StringBuilder Type, int size);
        public delegate bool EnumChildWindow(IntPtr WindowHandle, string num);
        [DllImport("User32.dll")]
        public static extern int EnumChildWindows(IntPtr WinHandle, EnumChildWindow ecw, string name);
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        private static extern int PostMessage(IntPtr hWnd, int Msg, string wParam, string lParam);
        [DllImport("User32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        private static extern int SendMessage(IntPtr hWnd, int WM_CHAR, int wParam, int lParam);
        [DllImport("User32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        private static extern int SendMessage(IntPtr hWnd, int WM_CHAR, string wParam, string lParam);
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        private static extern int PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_IME_KEYDOWN = 0x0290;
        private const int WM_IME_KEYUP = 0x0291;
        private const int WM_SETTEXT = 0x000C;



        public main()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 保存文件窗口 可以用
        /// </summary>
        public void SaveDialog()
        {

            var hWnd = IntPtr.Zero;
            var hChild = IntPtr.Zero;

            // Find Save File Dialog Box
            while (hWnd == IntPtr.Zero)
            {
                Thread.Sleep(500);
                hWnd = FindWindow("#32770", "另存为");
            }
            if (hWnd == IntPtr.Zero) return;

            // Enter fileName
            EnumChildWindows(hWnd, (handle, s) =>
            {
                //####取标题
                //StringBuilder title = new StringBuilder(100);
                //GetWindowText(handle, title, 100);//取标题
                //if (title.ToString() == s)
                //{
                //    hChild = handle;
                //    return false;
                //}
                //return true;


                //####取类型
                StringBuilder type = new StringBuilder(100);
                GetClassName(handle, type, 100);//取类型
                if (type.ToString() == s)
                {
                    hChild = handle;
                    return false;
                }

                return true;
            }, "Edit");
            SendMessage(hChild, WM_SETTEXT, null, "c:\\文件名.xps");

            // Press Save button
            hChild = FindWindowEx(hWnd, IntPtr.Zero, "Button", "保存(&S)");
            PostMessage(hChild, WM_IME_KEYDOWN, (int)Keys.S, 0);
            PostMessage(hChild, WM_IME_KEYUP, (int)Keys.S, 0);
        }




        /// <summary>
        /// 百度打开文件窗口 可以用
        /// </summary>
        public void OpenDialog()
        {

            var hWnd = IntPtr.Zero;
            var hChild = IntPtr.Zero;

            // Find Save File Dialog Box
            while (hWnd == IntPtr.Zero)
            {
                Thread.Sleep(500);
                hWnd = FindWindow("#32770", "打开");
            }
            if (hWnd == IntPtr.Zero) return;

            // Enter fileName
            EnumChildWindows(hWnd, (handle, s) =>
            {
                //####取标题
                //StringBuilder title = new StringBuilder(100);
                //GetWindowText(handle, title, 100);//取标题
                //if (title.ToString() == s)
                //{
                //    hChild = handle;
                //    return false;
                //}
                //return true;


                //####取类型
                StringBuilder type = new StringBuilder(100);
                GetClassName(handle, type, 100);//取类型
                if (type.ToString() == s)
                {
                    hChild = handle;
                    return false;
                }

                return true;
            }, "Edit");
            SendMessage(hChild, WM_SETTEXT, null, "\"财产属于谁、留给谁.ppt\" \"财产属于谁、留给谁.doc\" \"财产属于谁、留给谁.docx\" ");

            // Press Save button
            hChild = FindWindowEx(hWnd, IntPtr.Zero, "Button", "打开(&O)");
            PostMessage(hChild, WM_IME_KEYDOWN, (int)Keys.O, 0);
            PostMessage(hChild, WM_IME_KEYUP, (int)Keys.O, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OpenDialog();
        }

        private void main_Load(object sender, EventArgs e)
        {

        }
    }
}
