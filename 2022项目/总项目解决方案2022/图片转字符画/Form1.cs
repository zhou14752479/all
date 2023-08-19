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

namespace 图片转字符画
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private static string imgPath = @"C:\Users\Administrator\Desktop\f174deaae6bb77e5093f0eeba37be9d.jpg";
        private static string urlG = @"D:\g.jpg";
        private static string TxtPath = @"D:\Txt.txt";
        private static string app_dir = System.AppDomain.CurrentDomain.BaseDirectory;//当前运行进程的根目录  

        protected static readonly string charset = "MNHQ&OC?7>!:-;. ";
     

        //灰度图转字符画
        private void img2char()
        {
            try
            {
               
                char[] CharArr = charset.ToArray<char>();

                string Write2Txt = "";
                Bitmap bim = ToG();//灰度图

                for (int i = 0; i < bim.Height; i += 4)
                {
                    for (int j = 0; j < bim.Width; j += 2)
                    {
                        Color origalColor = bim.GetPixel(j, i);
                        int index = (int)((origalColor.R + origalColor.G + origalColor.B) / 768.0 * CharArr.Length);
                        Write2Txt += CharArr[index];
                    }
                    Write2Txt += "\r\n";
                }

                StreamWriter writer = new StreamWriter(TxtPath, false);
                writer.Write(Write2Txt);
                writer.Close();
                writer.Dispose();
            }
            catch (Exception e)
            {
                Console.Write("程序执行出现错误:" + e.ToString());
                Console.ReadKey();
            }
        }

        //将彩图转成灰度图
        private Bitmap ToG()
        {
            if (File.Exists(urlG))
                File.Delete(urlG);
            Bitmap bim = new Bitmap(imgPath);
            Bitmap bimG = new Bitmap(bim.Width, bim.Height);

            for (int i = 0; i < bim.Width; i++)
            {
                for (int j = 0; j < bim.Height; j++)
                {
                    Color c = bim.GetPixel(i, j);
                    int rgb = (int)(c.R * 0.299 + c.G * 0.587 + c.B * 0.114);
                    bimG.SetPixel(i, j, Color.FromArgb(rgb, rgb, rgb));
                }
            }
            bimG.Save(urlG);
            return bimG;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            img2char();
        }
    }
}
