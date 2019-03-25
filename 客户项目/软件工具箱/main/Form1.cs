using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox1 = new PictureBox();
           
            flowLayoutPanel1.Controls.Add(pictureBox1);
            Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(@"E:\火车头7.6破解\LocoyPlatform.exe");
            pictureBox1.Image = icon.ToBitmap();

            //for (int i = 0; i < SystemIconList.Count; i++)
            //{
            //    PictureBox pic = new PictureBox();
            //    pic.Size = new System.Drawing.Size(32, 32);
            //    flowLayoutPanel1.Controls.Add(pic);

            //    Bitmap p = SystemIconList[i].ToBitmap();
            //    pic.Image = p;
            //}
        }
    }
}
