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



       public static int x1 = 0;
        public static int x2 = 0;
        public static int y1 = 0;
        public static int y2 = 0;

        private void button1_Click(object sender, EventArgs e)
        {

            //每次选择区域截图
            //Graphics g = Graphics.FromImage(bmp);
            //g.CopyFromScreen(0, 0, 0, 0, new Size(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height));
            //frmScreen f = new frmScreen(bmp);
            //f.Show();
            //f.setimage = new frmScreen.SetImage(setimage2);



            //选取一次区域，第二次不需要再选取



            
             
            //设置截屏区域
            //imgGraphics.CopyFromScreen(0, 0, 0, 0, new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));//全屏

            if (x1 == 0 && y1 == 0 && x2 == 0 && y2 == 0)
            {
                Graphics g = Graphics.FromImage(bmp);
                g.CopyFromScreen(0, 0, 0, 0, new Size(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height));
                frmScreen f = new frmScreen(bmp);
                f.Show();
                return;
            }
            Bitmap image = new Bitmap(x2, y2);  //设置画布大小
            Graphics imgGraphics = Graphics.FromImage(image);
            imgGraphics.CopyFromScreen(x1, y1, 0, 0, new Size(x2, y2));
            setimage2(image);
            //保存
           // image.Save("D:\\" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".jpg");
        }



        public void setimage2(Image image)
        { Control.ControlCollection sonControls = panel1.Controls;
            //遍历所有控件
            
            foreach (Control control in sonControls)
            {
                if (control is PictureBox)
                {
                    PictureBox pic = (PictureBox)control;

                    if (pic.Image == null)
                    {
                        
                        pic.Image = image;
                        break;
                    }
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
                if (i >= Convert.ToInt32(comboBox1.Text))
                {
                    PictureBox pic = new PictureBox();
                    pic.Size = new Size(Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));

                    pic.Left = 0 + TempInt * Convert.ToInt32(textBox1.Text);
                    pic.Top = 0 + TempInt2 * Convert.ToInt32(textBox2.Text);
                    pic.SizeMode = PictureBoxSizeMode.StretchImage;
                    //pic.SizeMode = PictureBoxSizeMode.Normal;
                    pic.Dock = DockStyle.None;
                    pic.BorderStyle = BorderStyle.Fixed3D;
                    this.panel1.Controls.Add(pic);

                    pic.Click += new System.EventHandler(this.pic_doubleClick);
                }
                else
                {
                    Label label = new Label();
                    label.Size = new Size(Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));

                    label.Left = 0 + TempInt * Convert.ToInt32(textBox1.Text);
                    label.Top = 0 + TempInt2 * Convert.ToInt32(textBox2.Text);
                    label.Font = new Font("宋体", 60, FontStyle.Bold);
                    label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    label.Dock = DockStyle.None;
                    label.BorderStyle = BorderStyle.Fixed3D;
                    this.panel1.Controls.Add(label);
                    label.Text = (i + 1).ToString();
                }

            }



        }

        bool a = true;
        private void pic_doubleClick(object sender, EventArgs e)
        {
            //if (((PictureBox)sender).Width == 150)
            //{
            //    ((PictureBox)sender).SetBounds(((PictureBox)sender).Location.X - 75, ((PictureBox)sender).Location.Y - 75, 300, 300);
            //}
            //else
            //{
            //    ((PictureBox)sender).SetBounds(((PictureBox)sender).Location.X + 75, ((PictureBox)sender).Location.Y + 75, 150, 150);
            //}

            if (a == true)
            {
                ((PictureBox)sender).Width = Convert.ToInt32(((PictureBox)sender).Width * 2);
                ((PictureBox)sender).Height = Convert.ToInt32(((PictureBox)sender).Height * 2);
                a = false;
            }
            else
            {
                ((PictureBox)sender).Width = Convert.ToInt32(((PictureBox)sender).Width * 0.5);
                ((PictureBox)sender).Height = Convert.ToInt32(((PictureBox)sender).Height * 0.5);
                a = true;
            }

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Control.ControlCollection sonControls = panel1.Controls;
            //遍历所有控件

            foreach (Control control in sonControls)
            {
                if (control is PictureBox)
                {
                    PictureBox pic = (PictureBox)control;
                    pic.Image = null; ;
                }



            }
        }

       
    }
}
