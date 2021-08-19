using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;

namespace 自定义控件1
{
    public partial class UserControl1: UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }
        public void SetWindowRegion()
        {
            System.Drawing.Drawing2D.GraphicsPath FormPath;
            FormPath = new System.Drawing.Drawing2D.GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            FormPath = GetRoundedRectPath(rect, 10);
            this.Region = new Region(FormPath);

        }
        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            int diameter = radius;
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();

            // 左上角
            path.AddArc(arcRect, 180, 90);

            // 右上角
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);

            // 右下角
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);

            // 左下角
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure();//闭合曲线
            return path;
        }
        private void UserControl1_Load(object sender, EventArgs e)
        {
           
        }

        private void UserControl1_Resize(object sender, EventArgs e)
        {
            SetWindowRegion();
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("安装软件");
        }

        private void UserControl1_MouseEnter(object sender, EventArgs e)
        {
            button1.Visible = true;
            uiButton1.Visible = true;
          
        }

        private void UserControl1_MouseLeave(object sender, EventArgs e)
        {
           
            //button1.Visible = false;
            //uiButton1.Visible = false;

        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("视频录制");
        }
    }
}
