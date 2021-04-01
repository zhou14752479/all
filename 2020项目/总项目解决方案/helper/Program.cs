using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace helper
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new cookieBrowser("https://newbill.zt-express.com/prod/index.html?"));
            Application.Run(new cookieBrowser("https://login.dd373.com/"));
            //Application.Run(new 手动点击抓取());
        }
    }
}
