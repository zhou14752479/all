using SharpCompress.Reader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpCompress.Common;

namespace 主程序202202
{
    public partial class 文件解压缩处理 : Form
    {
        public 文件解压缩处理()
        {
            InitializeComponent();
        }

       




        private static String directoryPath = @"D:\all\2022项目\总项目解决方案2022\主程序202202\bin\Debug";

        public static void unTAR(String tarFilePath)
        {
            SharpCompress.Common.ArchiveEncoding.Default = Encoding.UTF7;

            using (Stream stream = File.OpenRead(tarFilePath))
            {
                var reader = ReaderFactory.Open(stream);

                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                        reader.WriteEntryToDirectory(directoryPath,
                           ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);

                }
            }
        }
       public void readcookie(string path1,string path2)
        {
            using (FileStream stream = File.OpenRead(path1))
            {
                byte[] content = new byte[stream.Length];

                for (int i = 0; i < content.Length; i++)
                {
                    content[i] = (byte)stream.ReadByte();
                    
                }
                MessageBox.Show(Encoding.Default.GetString(content));

            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string cookiepath = @"C:\Users\zhou\Desktop\Cookies.binarycookies";
            //FileStream file = new FileStream(cookiepath, FileMode.Open, FileAccess.Read);
            //BinaryReader read = new BinaryReader(file);
            //int count = (int)file.Length;
            //byte[] buffer = new byte[count];
            //read.Read(buffer, 0, buffer.Length);
            //string msg = Encoding.Default.GetString(buffer);

            //textBox1.Text = msg;


            readcookie(cookiepath, @"C:\Users\zhou\Desktop\Cookies.txt");



            //string tarfile = @"C:\Users\zhou\Desktop\压缩\20220117.tar";
            //unTAR(tarfile);


        }
    }
}
