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

namespace 文件二维码
{
    public partial class 二维码读取 : Form
    {
        public 二维码读取()
        {
            InitializeComponent();
        }

        public static string filepath = "";
       public ArrayList lists = new ArrayList();
        private void 二维码读取_Load(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = filepath;
            textBox1.Text = Path.GetFileNameWithoutExtension(filepath);
          
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int value =1;
            for (int i = 0; i < lists.Count; i++)
            {
                if (lists[i].ToString() == filepath)
                {
                    value = i;
                }

            }
            if (value < 1)
            {
                MessageBox.Show("已经是第一张");
                return;
            }
            filepath = lists[value - 1].ToString();
            pictureBox1.ImageLocation = lists[value-1].ToString();
            textBox1.Text = Path.GetFileNameWithoutExtension(lists[value - 1].ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)pictureBox1.Image;
            textBox2.Text = 二维码识别.DecodeQrCode(bmp);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            File.Delete(filepath);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int value = 1;
            for (int i = 0; i < lists.Count; i++)
            {
                if (lists[i].ToString() == filepath)
                {
                    value = i;
                }

            }
            if (value > lists.Count-2)
            {
                MessageBox.Show("已经是最后一张");
                return;
            }
            filepath = lists[value + 1].ToString();
            pictureBox1.ImageLocation = lists[value + 1].ToString();
            textBox1.Text = Path.GetFileNameWithoutExtension(lists[value + 1].ToString());
        }
    }
}
