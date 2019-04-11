using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_4
{
    public partial class txt合并 : Form
    {
        public txt合并()
        {
            InitializeComponent();
        }

        //获取子文件夹

        //DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
        //    foreach (DirectoryInfo info in directoryInfos)
        //    {
        //        //获取当前子目录的路径
        //        string path = info.FullName;
        //      }




        #region  获取文件夹内的所有.txt文件
        public static ArrayList GetTxtFiles(string filePath)
        {
            ArrayList lists = new ArrayList();
            if (!Directory.Exists(filePath))
            {

                return lists;
            }
            
            //创建一个DirectoryInfo的类
            DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
            //获取当前的目录的文件
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            foreach (FileInfo info in fileInfos)
            {
                //获取文件的名称(包括扩展名)
                string fullName = info.FullName;
                //获取文件的扩展名
                string extension = info.Extension.ToLower();
                if (extension == ".txt")
                {
                    //获取文件的大小
                    long length = info.Length;
                    lists.Add(fullName);
                    
                }
            }

            return lists;
        }
        #endregion

        static string  path = "";
        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                path= textBox1.Text= dialog.SelectedPath;
            }

        }


        public static void run()
        {

            if (path == "")
            {
                MessageBox.Show("请选择文本所在文件夹");
                return;
            }
            ArrayList lists = new ArrayList();

            lists = GetTxtFiles(path);

            for (int i = 0; i < lists.Count; i++)
            {

                string a = System.IO.File.ReadAllText("文件A的路径");
                string b = System.IO.File.ReadAllText("文件B的路径");
                System.IO.File.WriteAllText("文件C的路径", a + b);
            }
            

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
