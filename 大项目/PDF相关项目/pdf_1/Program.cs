using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pdf_1
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            //string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            //if (File.Exists(path + "//mac.txt"))
            //{

            //    StreamReader sr = new StreamReader(path + "//mac.txt");
            //    //一次性读取完 
            //    string texts = sr.ReadToEnd();

            //    if (texts.Trim() == register.GetMacAddress().Trim())

            //    {

            //        Application.EnableVisualStyles();
            //        Application.SetCompatibleTextRenderingDefault(false);
            //        Application.Run(new Form2());

            //    }
            //    sr.Close();
            //}

            //else
            //{
            //    Application.EnableVisualStyles();
            //    Application.SetCompatibleTextRenderingDefault(false);
            //    Application.Run(new register());
            //}

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form3());


        }
    }
}
