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
            Application.Run(new cookieBrowser("https://esearch.ipd.gov.hk/nis-pos-view/#/"));
           
            //Application.Run(new 手动点击抓取());
            //Application.Run(new cookieBrowser("https://perbank.abchina.com/EbankSite/startup.do?r=B56ACA045A1F695E&token=WS93UzdEb2VqdFE4aVI5TjNKd0NMcHZwakhYZTRMSVF4U29BcXFZUlpVczBmWnF6M2RmTEllVnBBeFVKOTVOQ0FRSzZoU1ExL3NSbEx5RlQ5ZnRtYmZFTWxRV1ZacWFDVTRleWZJaWJPRS82YUJmSjJBSGdvQ2pORXRwTzdTb21IVVZhMGVOc2h6ZGY5MmJ1SGdFcHFXTG5qNUo5RGFnekh4WVd2TXplaHBzPQ%3d%3d"));
          //  Application.Run(new cookieBrowser("http://search.kongfz.com/product_result/?key=9787802501614&status=0&_stpmt=eyJzZWFyY2hfdHlwZSI6ImFjdGl2ZSJ9"));
        }
    }
}
