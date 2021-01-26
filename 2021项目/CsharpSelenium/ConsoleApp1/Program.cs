using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        #region 获取另一系统文本框值  
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);

        [DllImport("User32.dll", EntryPoint = "FindEx")]
        public static extern IntPtr FindEx(IntPtr hwnd, IntPtr hwndChild, string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, StringBuilder lParam);
        #endregion
        public static int WM_GETTEXT = 0x000D;
        const int WM_SETTEXT = 0x000C;
        static void Main(string[] args)
        {
            IntPtr maindHwnd = FindWindow(null, "另存为"); //获得句柄  
            if (maindHwnd != IntPtr.Zero)
            {
                Console.WriteLine("找到了窗体！");
                IntPtr childHwnd = FindWindowEx(maindHwnd, IntPtr.Zero, null, null);
                if (childHwnd != IntPtr.Zero)
                {
                    Console.WriteLine("找到了控件！");
                    const int buffer_size = 1024;
                    StringBuilder buffer = new StringBuilder(buffer_size);

                    //buffer.Append("55555555");
                    //SendMessage(childHwnd, WM_SETTEXT, buffer_size, buffer);
                    //SendMessage(childHwnd, WM_SETTEXT, 0, buffer);
                    SendMessage(childHwnd, WM_GETTEXT, buffer_size, buffer);
                    Console.WriteLine(string.Format("取到的值是：{0}", buffer.ToString()));//取值一直是空字符串  


                  

                }

                else
                {
                    Console.WriteLine("没有找到控件");
                }
            }
            else
            {
                Console.WriteLine("没有找到窗口");
            }
            Console.ReadKey();
        }



    }
}
