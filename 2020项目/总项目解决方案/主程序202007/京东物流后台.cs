using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202007
{
    

    public partial class 京东物流后台 : Form
    {
        public 京东物流后台()
        {
            InitializeComponent();  

        }

        public class EllipseButton : Button
        {
            protected override void OnPaint(PaintEventArgs pevent)
            {
                GraphicsPath gPath = new GraphicsPath();
                // 绘制椭圆形区域
                gPath.AddEllipse(0, 0, this.ClientSize.Width, this.ClientSize.Height);

                // 将区域赋值给Region
                this.Region = new System.Drawing.Region(gPath);

                base.OnPaint(pevent);
            }
        }
        private void 京东物流后台_Load(object sender, EventArgs e)
        {

        }
    }
}
