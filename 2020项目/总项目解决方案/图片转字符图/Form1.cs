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

namespace 图片转字符图
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 将图片转换为字符画
        /// </summary>
        /// <param name="bitmap">Bitmap类型的对象</param>
        /// <param name="savaPath">保存路径</param>
        /// <param name="WAddNum">宽度缩小倍数（如果输入3，则以1/3倍的宽度显示）</param>
        /// <param name="HAddNum">高度缩小倍数（如果输入3，则以1/3倍的高度显示）</param>
        public static void ConvertToChar(Bitmap bitmap, String savaPath, int WAddNum, int HAddNum)
        {
            StringBuilder sb = new StringBuilder();
            String replaceChar = "@*#$%XB0H?OC7>+v=~^:_-'`. ";
            for (int i = 0; i < bitmap.Height; i += HAddNum)
            {
                for (int j = 0; j < bitmap.Width; j += WAddNum)
                {
                    //获取当前点的Color对象
                    Color c = bitmap.GetPixel(j, i);
                    //计算转化过灰度图之后的rgb值（套用已有的计算公式就行）
                    int rgb = (int)(c.R * .3 + c.G * .59 + c.B * .11);
                    //计算出replaceChar中要替换字符的index
                    //所以根据当前灰度所占总rgb的比例(rgb值最大为255，为了防止超出索引界限所以/256.0)
                    //（肯定是小于1的小数）乘以总共要替换字符的字符数，获取当前灰度程度在字符串中的复杂程度
                    int index = (int)(rgb / 256.0 * replaceChar.Length);
                    //追加进入sb
                    sb.Append(replaceChar[index]);
                }
                //添加换行
                sb.Append("\r\n");
            }
            //创建文件流
            using (FileStream fs = new FileStream(savaPath, FileMode.Create, FileAccess.Write))
            {
                //转码
                byte[] bs = Encoding.Default.GetBytes(sb.ToString());
                //写入
                fs.Write(bs, 0, bs.Length);
            }
        }


        public void run(string path, string savaPath)
        {
            StringBuilder sb = new StringBuilder();
            Bitmap bitMap = new Bitmap(path);
            for (int i = 0; i < bitMap.Width; i++)
            {
                for (int j = 0; j < bitMap.Height; j++)
                {
                    Color origalColor = bitMap.GetPixel(i, j);
                    int grayScale = (int)(origalColor.R * .3 + origalColor.G * .59 + origalColor.B * .11);
                    Color newColor = Color.FromArgb(grayScale, grayScale, grayScale);
                    bitMap.SetPixel(i, j, newColor);
                }
            }
            string chars = "#8XOHLTI)i=+;:,. ";
            for (int i = 0; i < bitMap.Width; i += 2)
            {
                for (int j = 0; j < bitMap.Height; j++)
                {
                    Color origalColor = bitMap.GetPixel(i, j);
                    int index = (int)((origalColor.R + origalColor.G + origalColor.B) / 768.0 * chars.Length);
                    sb.Append(chars[index]);
                }
                sb.Append("\r\n");
            }
            using (FileStream fs = new FileStream(savaPath, FileMode.Create, FileAccess.Write))
            {
                //转码
                byte[] bs = Encoding.Default.GetBytes(sb.ToString());
                //写入
                fs.Write(bs, 0, bs.Length);
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {
           

        }

        private void Button2_Click(object sender, EventArgs e)
        {

            //ConvertToChar(new Bitmap(textBox1.Text), @"D:\1.txt",20,20);
            //   run(textBox1.Text, @"D:\1.txt");

            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {


                textBox1.Text = openFileDialog1.FileName;
            }
        }
    }
}
