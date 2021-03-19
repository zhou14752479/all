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
            Application.Run(new cookieBrowser("https://newbill.zt-express.com/prod/index.html?"));
           // Application.Run(new cookieBrowser("http://account.okooo.com/login?urlname=okooo&urlfrom=http%3A%2F%2Fwww.okooo.com%2Fsoccer%2Fmatch%2F954582%2Fgoals%2Fchange%2F2%2F"));
            //Application.Run(new 手动点击抓取());
        }
    }
}
