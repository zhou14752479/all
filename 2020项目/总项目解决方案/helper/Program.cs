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
            //Application.Run(new cookieBrowser("https://perbank.abchina.com/EbankSite/startup.do?r=B56ACA045A1F695E&token=WS93UzdEb2VqdFE4aVI5TjNKd0NMcHZwakhYZTRMSVF4U29BcXFZUlpVczBmWnF6M2RmTEllVnBBeFVKOTVOQ0FRSzZoU1ExL3NSbEx5RlQ5ZnRtYmZFTWxRV1ZacWFDVTRleWZJaWJPRS82YUJmSjJBSGdvQ2pORXRwTzdTb21IVVZhMGVOc2h6ZGY5MmJ1SGdFcHFXTG5qNUo5RGFnekh4WVd2TXplaHBzPQ%3d%3d"));
            Application.Run(new cookieBrowser("http://search.kongfz.com/product_result/?key=9787802501614&status=0&_stpmt=eyJzZWFyY2hfdHlwZSI6ImFjdGl2ZSJ9"));
        }
    }
}
