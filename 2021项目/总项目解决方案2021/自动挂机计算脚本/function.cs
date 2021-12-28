using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 自动挂机计算脚本
{
    class function
    {
        public void getfiles(string dir, ListView lsv)
        {
            DirectoryInfo d = new DirectoryInfo(dir);
            FileInfo[] files = d.GetFiles();//文件
            DirectoryInfo[] directs = d.GetDirectories();//文件夹
            foreach (FileInfo f in files)
            {
                ListViewItem lv1 = lsv.Items.Add(lsv.Items.Count.ToString()); //使用Listview展示数据

                lv1.SubItems.Add(Path.GetFileNameWithoutExtension(f.FullName));
              
            }
           
        }


    }
}
