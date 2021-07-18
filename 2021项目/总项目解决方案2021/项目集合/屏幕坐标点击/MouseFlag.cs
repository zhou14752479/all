using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace 屏幕坐标点击
{
   
        public class MouseFlag
        {
            enum MouseEventFlag : uint
            {
                Move = 0x0001,
                LeftDown = 0x0002,
                LeftUp = 0x0004,
                RightDown = 0x0008,
                RightUp = 0x0010,
                MiddleDown = 0x0020,
                MiddleUp = 0x0040,
                XDown = 0x0080,
                XUp = 0x0100,
                Wheel = 0x0800,
                VirtualDesk = 0x4000,
                Absolute = 0x8000
            }

            [DllImport("user32.dll")]
            static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);

            [DllImport("user32.dll")]
            public static extern int SetCursorPos(int x, int y);

            public static void MouseLefClickEvent(int dx, int dy, uint data)
            {
                SetCursorPos(dx, dy);
                mouse_event(MouseEventFlag.LeftDown, dx, dy, data, UIntPtr.Zero);
                mouse_event(MouseEventFlag.LeftUp, dx, dy, data, UIntPtr.Zero);
            }
        }

    
}
