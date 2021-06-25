using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
//Renyi-2016年9月1日
namespace Totext
{
    public partial class Form1 : Form
    {
        [DllImport("AspriseOCR.dll", EntryPoint = "OCR", CallingConvention = CallingConvention.Cdecl)]

        public static extern IntPtr OCR(string file, int type);

        [DllImport("AspriseOCR.dll", EntryPoint = "OCRpart", CallingConvention = CallingConvention.Cdecl)]

        static extern IntPtr OCRpart(string file, int type, int startX, int startY, int width, int height);

        [DllImport("AspriseOCR.dll", EntryPoint = "OCRBarCodes", CallingConvention = CallingConvention.Cdecl)]

        static extern IntPtr OCRBarCodes(string file, int type);

        [DllImport("AspriseOCR.dll", EntryPoint = "OCRpartBarCodes", CallingConvention = CallingConvention.Cdecl)]

        static extern IntPtr OCRpartBarCodes(string file, int type, int startX, int startY, int width, int height);
        public Form1()
        {
            InitializeComponent();
        }
        string filePath = string.Empty;
        private void button1_Click(object sender, EventArgs e)
        {
            //创建一个对话框对象
            OpenFileDialog ofd = new OpenFileDialog();
            //为对话框设置标题
            ofd.Title = "请选择上传的图片";
            //设置筛选的图片格式
            //ofd.Filter = "图片格式|*.jpg,*.png";
            //设置是否允许多选
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //获得文件的完整路径（包括名字后后缀）
                filePath = ofd.FileName;
                this.textBox1.Text = filePath;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Image img = Image.FromFile(filePath);
            this.richTextBox1.Text = Marshal.PtrToStringAnsi(OCRpart(filePath, -1, 0, 0, img.Width, img.Height));
        }
    }
}
