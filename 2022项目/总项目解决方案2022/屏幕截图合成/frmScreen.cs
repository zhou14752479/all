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
    public partial class frmScreen : Form
    {

        public Bitmap bmp;
        public Point pStart;
        public Point pTemp;
        public Point pMove;
        public Point p2;
        public Rectangle eara = Rectangle.Empty;



        public frmScreen(Bitmap bmp)
        {
            InitializeComponent();
            this.bmp = bmp;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.BackgroundImage = bmp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void frmScreen_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(bmp, new Point(0, 0));

        }

       

        private void frmScreen_Load(object sender, EventArgs e)
        {

        }

        private void frmScreen_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && eara != Rectangle.Empty)
            {
                eara = Rectangle.Empty;
                pStart = Point.Empty;
                pTemp = Point.Empty;
                pMove = Point.Empty;
                p2 = Point.Empty;
                this.Invalidate();
                return;
            }
            if (e.Button == MouseButtons.Right && eara == Rectangle.Empty)
            {
                this.Close();
                return;
            }

        }

        private void frmScreen_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && eara == Rectangle.Empty)
            {
                pStart = new Point(e.X, e.Y);
                pTemp = pStart;
            }

        }

        private void frmScreen_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                pMove = new Point(e.X, e.Y);
                this.Invalidate();
            }

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            //1第二个点在pStart的右下方
            if (pTemp.X < pMove.X && pTemp.Y < pMove.Y)
            {
                pStart = pTemp;
                p2 = pMove;

            }

            //2第二个点在pStart的左下方
            if (pTemp.X > pMove.X && pTemp.Y < pMove.Y)
            {
                pStart.Y = pTemp.Y;
                pStart.X = pMove.X;
                p2.X = pTemp.X;
                p2.Y = pMove.Y;

            }
            //3第二个点在pStart的左上方
            if (pTemp.X > pMove.X && pTemp.Y > pMove.Y)
            {
                p2 = pTemp;
                pStart = pMove;


            }
            //4第二个点在pStart的右上方
            if (pTemp.X < pMove.X && pTemp.Y > pMove.Y)
            {
                pStart.Y = pMove.Y;
                pStart.X = pTemp.X;
                p2.Y = pTemp.Y;
                p2.X = pMove.X;



            }




            eara = new Rectangle(pStart, new Size(p2.X - pStart.X, p2.Y - pStart.Y));
            g.DrawRectangle(Pens.Green, eara);
            Brush bs = new SolidBrush(Color.FromArgb(50, Color.Gold));
            g.FillRectangle(bs, eara);

            //定义八个点
            ///
            /// 1-2-3
            /// 4-0-5
            /// 6-7-8
            ///
            //1
            Size pointSize = new Size(3, 3);
            Point pRight = new Point(2, 2);
            Rectangle eara1 = new Rectangle(eara.Location.X - 2, eara.Location.Y - 2, 5, 5);
            //2
            Rectangle eara2 = new Rectangle(eara.Location.X + eara.Width / 2 - 2, eara.Location.Y - 2, 5, 5);
            //3
            Rectangle eara3 = new Rectangle(eara.Location.X + eara.Width - 2, eara.Location.Y - 2, 5, 5);
            //4
            Rectangle eara4 = new Rectangle(eara.Location.X - 2, eara.Location.Y + eara.Height / 2 - 2, 5, 5);
            //5
            Rectangle eara5 = new Rectangle(eara.Location.X + eara.Width - 2, eara.Location.Y + eara.Height / 2 - 2, 5, 5);
            //6          
            Rectangle eara6 = new Rectangle(eara.Location.X - 2, eara.Location.Y + eara.Height - 2, 5, 5);
            //7
            Rectangle eara7 = new Rectangle(eara.Location.X + eara.Width / 2 - 2, eara.Location.Y + eara.Height - 2, 5, 5);
            //8      
            Rectangle eara8 = new Rectangle(eara.Location.X + eara.Width - 2, eara.Location.Y + eara.Height - 2, 5, 5);
            g.FillRectangle(Brushes.Goldenrod, eara1);
            g.FillRectangle(Brushes.Goldenrod, eara2);
            g.FillRectangle(Brushes.Goldenrod, eara3);
            g.FillRectangle(Brushes.Goldenrod, eara4);
            g.FillRectangle(Brushes.Goldenrod, eara5);
            g.FillRectangle(Brushes.Goldenrod, eara6);
            g.FillRectangle(Brushes.Goldenrod, eara7);
            g.FillRectangle(Brushes.Goldenrod, eara8);



        }

        private void frmScreen_MouseUp(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                eara = Rectangle.Empty;

                this.Invalidate();

            }
        }

        public delegate void SetImage(Image image);

        public SetImage setimage;
        

        private void frmScreen_DoubleClick(object sender, EventArgs e)
        {

            if ((eara != Rectangle.Empty) && eara.Contains(Cursor.Position))
            {

                //MessageBox.Show("保存截屏");
                Bitmap myPic = new Bitmap(eara.Width, eara.Height);
                Graphics g = Graphics.FromImage(bmp);
                myPic = bmp.Clone(eara, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

                setimage(myPic);


                //saveFileDialog1.Filter = "JPEG图像|*.jpeg|GIF图像|*.gif|PNG图像|*.png";
                //if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                //{

                //    if (saveFileDialog1.FileName.Trim() != "")
                //    {
                //        switch (saveFileDialog1.FilterIndex)
                //        {
                //            case 1:
                //                myPic.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                //                break;
                //            case 2:
                //                myPic.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Gif);
                //                break;
                //            case 3:
                //                myPic.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
                //                break;
                //        }

                //    }
                //    else
                //    {
                //        MessageBox.Show("保存失败");
                //    }

                //}


                this.Close();

            }
        }
    }
}
