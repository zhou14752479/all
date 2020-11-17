using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 常用软件非客户
{
    public partial class 图片转ASCII : Form
    {
        class Generate
        {
            /// <summary>
            /// 生成string
            /// </summary>
            /// <param name="bitmap">照片</param>
            /// <param name="rowSize">行大小</param>
            /// <param name="colSize">列大小</param>
            /// <param name="type">模式</param>
            /// <returns></returns>
            public static string GenerateStr(Bitmap bitmap, int rowSize, int colSize, int type)
            {
                StringBuilder result = new StringBuilder();
                char[] charset = { ' ', '.', ',', ':', ';', 'i', '1', 'r', 's', '5', '3', 'A', 'H', '9', '8', '&', '@', '#' };
                if (type == 1)
                {
                    charset = new char[] { ' ', '.', '1', '2', '0', '7', '5', '3', '4', '6', '9', '8' };
                }
                else if (type == 2)
                {
                    charset = new char[] { '丶', '卜', '乙', '日', '瓦', '車', '馬', '龠', '齱', '龖' };
                }
                int bitmapH = bitmap.Height;
                int bitmapW = bitmap.Width;
                for (int h = 0; h < bitmapH / rowSize; h++)
                {
                    int offsetY = h * rowSize;
                    for (int w = 0; w < bitmapW / colSize; w++)
                    {
                        int offSetX = w * colSize;
                        float averBright = 0;
                        for (int j = 0; j < rowSize; j++)
                        {
                            for (int i = 0; i < colSize; i++)
                            {
                                try
                                {
                                    Color color = bitmap.GetPixel(offSetX + 1, offsetY + j);
                                    averBright += color.GetBrightness();
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    averBright += 0;
                                }
                            }
                        }
                        averBright /= (rowSize * colSize);
                        int index = (int)(averBright * charset.Length);
                        if (index == charset.Length)
                            index--;
                        result.Append(charset[charset.Length - 1 - index]);
                    }
                    result.Append("\r\n");
                }
                return result.ToString();
            }
        }


        public 图片转ASCII()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = null;
            //上传照片
            OpenFileDialog op = new OpenFileDialog();
            if (op.ShowDialog() == DialogResult.OK)
            {
                var image = File.ReadAllBytes(op.FileName);
                MemoryStream ms1 = new MemoryStream(image);
                Bitmap bm = (Bitmap)Image.FromStream(ms1);
                str = Generate.GenerateStr(bm, int.Parse(comboBox2.Text),
                    int.Parse(comboBox1.Text), int.Parse(comboBox3.Text));
            }
            this.btnSave_Click(str);
            textBox1.Text = "已生成，文件位置E:\\test.txt";
        }

        private void btnSave_Click(string s)
        {
            StreamWriter sw = File.AppendText(@"E:\\test.txt"); //保存到指定路径
            sw.Write(s);
            sw.Flush();
            sw.Close();
        }
        /// <summary>
        /// 生成string
        /// </summary>
        /// <param name="bitmap">照片</param>
        /// <param name="rowSize">行大小</param>
        /// <param name="colSize">列大小</param>
        /// <param name="type">模式</param>
        /// <returns></returns>
        public static string GenerateStr(Bitmap bitmap, int rowSize, int colSize, int type)
        {
            StringBuilder result = new StringBuilder();
            char[] charset = { ' ', '.', ',', ':', ';', 'i', '1', 'r', 's', '5', '3', 'A', 'H', '9', '8', '&', '@', '#' };
            if (type == 1)
            {
                charset = new char[] { ' ', '.', '1', '2', '0', '7', '5', '3', '4', '6', '9', '8' };
            }
            else if (type == 2)
            {
                charset = new char[] { '丶', '卜', '乙', '日', '瓦', '車', '馬', '龠', '齱', '龖' };
            }
            int bitmapH = bitmap.Height;
            int bitmapW = bitmap.Width;
            for (int h = 0; h < bitmapH / rowSize; h++)
            {
                int offsetY = h * rowSize;
                for (int w = 0; w < bitmapW / colSize; w++)
                {
                    int offSetX = w * colSize;
                    float averBright = 0;
                    for (int j = 0; j < rowSize; j++)
                    {
                        for (int i = 0; i < colSize; i++)
                        {
                            try
                            {
                                Color color = bitmap.GetPixel(offSetX + 1, offsetY + j);
                                averBright += color.GetBrightness();
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                averBright += 0;
                            }
                        }
                    }
                    averBright /= (rowSize * colSize);
                    int index = (int)(averBright * charset.Length);
                    if (index == charset.Length)
                        index--;
                    result.Append(charset[charset.Length - 1 - index]);
                }
                result.Append("\r\n");
            }
            return result.ToString();
        }
        private void 图片转ASCII_Load(object sender, EventArgs e)
        {

        }
    }
}
