using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace wordToPdf
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

            //string str = Interaction.InputBox("请输入登录密码", "验证窗口", "", -1, -1);
            //if (str == "186092")
            //{

            //    Application.Run(new Form1());
            //}
            //else
            //{
            //    MessageBox.Show("密码错误");
            //}
            Application.Run(new Form1());

        }
    }
}
