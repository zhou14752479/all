using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 小程序解包
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 微信小程序解包
        /// </summary>
        public class UnWxapkg
        {
            FileStream fileStream = null;
            BinaryReader binaryReader = null;
            Action<string, byte[]> action = null;
            string message;
            public UnWxapkg(string path)
            {
                fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                binaryReader = new BinaryReader(fileStream);

            }
            public string Go(Action<string, byte[]> action_ = null)
            {
                action = action_;
                if (!ReadFirstMark())
                {
                    return message;
                }
                ReadInfo();
                int indexInfoLength = ReadindexInfoLength();
                int bodyInfoLength = ReadBodyInfoLength();
                if (!ReadLastMark())
                {
                    return message;
                }
                int fileCount = ReadFileCount();
                for (int i = 0; i < fileCount; i++)
                {
                    ReadFile();
                }
                binaryReader.Dispose();
                fileStream.Dispose();
                binaryReader.Close();
                fileStream.Close();
                MessageBox.Show("完成");
                return "完成";
            }
            /// <summary>
            /// 读取标志位 用于判断是否为小程序包
            /// </summary>
            /// <param name="binaryReader"></param>
            public bool ReadFirstMark()
            {
                if (ReadInt(1) != 190)
                {
                    message = "当前文件不是微信小程序包！";
                    return false;
                }
                return true;
            }
            /// <summary>
            /// 读取小程序附加信息
            /// </summary>
            /// <returns></returns>
            public int ReadInfo()
            {
                return (int)binaryReader.ReadUInt32();
            }
            /// <summary>
            /// 读取索引长度
            /// </summary>
            /// <returns></returns>
            public int ReadindexInfoLength()
            {
                return ReadInt(4);
            }
            /// <summary>
            /// 读取数据长度
            /// </summary>
            /// <returns></returns>
            public int ReadBodyInfoLength()
            {
                return ReadInt(4);
            }
            /// <summary>
            /// 读取结尾标记
            /// </summary>
            /// <returns></returns>
            public bool ReadLastMark()
            {
                if (ReadInt(1) != 237)
                {
                    message = "当前文件不是微信小程序包！";
                    return false;
                }
                return true;
            }
            /// <summary>
            /// 读取文件数量
            /// </summary>
            /// <returns></returns>
            public int ReadFileCount()
            {
                return ReadInt(4);
            }
            /// <summary>
            /// 读文件信息
            /// </summary>
            public void ReadFile()
            {
                int fileNameLength = ReadInt(4);
                byte[] fileNameb = binaryReader.ReadBytes(fileNameLength);
                string fileName = System.Text.Encoding.Default.GetString(fileNameb);
                int file_start = ReadInt(4);
                int file_length = ReadInt(4);
                //先缓存当前索引
                long currindex = binaryReader.BaseStream.Position;
                binaryReader.BaseStream.Position = file_start;
                byte[] file_content = binaryReader.ReadBytes(file_length);
                //还原索引
                binaryReader.BaseStream.Position = currindex;
                if (action != null)
                {
                    action.Invoke(fileName, file_content);
                }

            }
            /// <summary>
            /// 读整数
            /// </summary>
            /// <param name="length"></param>
            /// <returns></returns>
            int ReadInt(int length)
            {
                int num = 0;
                for (int i = 0; i < length; i++)
                {
                    num = (num << 8) + ((int)binaryReader.ReadByte());
                }
                return num;
            }
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\zhou\Documents\WeChat Files\Applet\wx9f1c2e0bbc10673c\224\_secondBag_.wxapkg";
            UnWxapkg unWxapkg = new UnWxapkg(path);
            label1.Text="开始解包！";
            string mess = unWxapkg.Go((x, x1) => {
                //解包后的存储地址
                string filePath = @"E:\123\1" + x;
                string dirPath = Path.GetDirectoryName(filePath);
                //开始写出文件
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate);
                stream.Write(x1, 0, x1.Length);
                stream.Dispose();
                stream.Close();
                Console.WriteLine(x1.Length.ToString() + " " + @"E:\123\1" + x);
            });


        }
    }
}
