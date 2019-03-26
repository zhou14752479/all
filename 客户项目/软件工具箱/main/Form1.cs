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
           
            //for (int i = 0; i < SystemIconList.Count; i++)
            //{
            //    PictureBox pic = new PictureBox();
            //    pic.Size = new System.Drawing.Size(32, 32);
            //    flowLayoutPanel1.Controls.Add(pic);

            //    Bitmap p = SystemIconList[i].ToBitmap();
            //    pic.Image = p;
            //}
        }

        private void 添加程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                PictureBox pictureBox1 = new PictureBox();

               tableLayoutPanel1.Controls.Add(pictureBox1);
                Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(openFileDialog1.FileName);

                pictureBox1.Click += new EventHandler(pictureBox1_Click);
                pictureBox1.Image = icon.ToBitmap();
                pictureBox1.Dock= DockStyle.Fill; 
            }
        }

        public void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            System.Diagnostics.Process.Start("");
        }

        private void 删除程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            
            tableLayoutPanel1.Controls.RemoveAt(2);
        }

        private void flowLayoutPanel1_MouseClick(object sender, MouseEventArgs e)
        {
          
        }
    }
}
