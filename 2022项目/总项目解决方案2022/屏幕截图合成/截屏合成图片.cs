using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 屏幕截图合成
{
    public partial class 截屏合成图片 : Form
    {
        public 截屏合成图片()
        {
            InitializeComponent();
            
           
        }
       public Bitmap bmp = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);
        private void button1_Click(object sender, EventArgs e)
        {
            //if (checkBox1.Checked == true)
            //{
            //    this.WindowState = FormWindowState.Minimized;
            //}
           
        Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(0, 0, 0, 0, new Size(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height));

            frmScreen f = new frmScreen(bmp);
            f.Show();

            f.setimage = new frmScreen.SetImage(setimage2);

          
        }



        public void setimage2(Image image)
        { Control.ControlCollection sonControls = panel1.Controls;
            //遍历所有控件
            foreach (PictureBox control in sonControls)
            {
                if (control.Image == null)
                {
                    control.Image = image;
                    break;
                }



            }
        }

        private void 截屏合成图片_Load(object sender, EventArgs e)
        {
         
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            for (int i = 0; i < Convert.ToInt32(comboBox1.Text)*4; i++)
            {
                int TempInt = i % Convert.ToInt32(comboBox1.Text);//取余数，计算横坐标用。
                int TempInt2 = (int)i / Convert.ToInt32(comboBox1.Text);//取整数，看放置在第几行。
                PictureBox pic = new PictureBox();
                pic.Size = new Size(Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));

                pic.Left = 0 + TempInt * Convert.ToInt32(textBox1.Text);
                pic.Top = 0 + TempInt2 * Convert.ToInt32(textBox2.Text);
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.Dock = DockStyle.None;
                pic.BorderStyle = BorderStyle.Fixed3D;
                this.panel1.Controls.Add(pic);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
