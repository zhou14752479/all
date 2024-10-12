using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTHProject
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

            //FrmLogin frmLogin = new FrmLogin();

            //frmLogin.TopMost = true;

            //if(frmLogin.ShowDialog() == DialogResult.OK)
            //{
            //    Application.Run(new FrmMain());
            //}
            //else
            //{
            //    Application.Exit();
            //}
            Application.Run(new 工商企业查询());

        }
    }
}
