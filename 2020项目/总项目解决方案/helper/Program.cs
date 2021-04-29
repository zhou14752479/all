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
            //Application.Run(new cookieBrowser("https://dian.ysbang.cn/index.html#/indexContent?searchKey=%E9%B9%BF%E5%B7%9D%E6%B4%BB%E7%BB%9C%E8%83%B6%E5%9B%8A&_t=1618453221608"));
            // Application.Run(new cookieBrowser("https://login.dd373.com/"));
            //Application.Run(new 手动点击抓取());
            Application.Run(new cookieBrowser("http://www.jszwfw.gov.cn/jsjis/admin/verifyCode.do?code=4&var=rand&width=90&height=45&random=0.08807624779516066;"));
        }
    }
}
