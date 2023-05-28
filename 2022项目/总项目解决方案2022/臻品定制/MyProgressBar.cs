using System;
using System.Drawing;
using System.Windows.Forms;

namespace Background
{
	// Token: 0x0200000E RID: 14
	internal class MyProgressBar : ProgressBar
	{
		// Token: 0x060000B1 RID: 177 RVA: 0x0000C7F3 File Offset: 0x0000A9F3
		public MyProgressBar()
		{
			base.SetStyle(ControlStyles.UserPaint, true);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000C808 File Offset: 0x0000AA08
		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle rectangle = new Rectangle(0, 0, base.Width, base.Height);
			rectangle.Height -= 4;
			rectangle.Width = (int)((double)rectangle.Width * ((double)base.Value / (double)base.Maximum)) - 4;
			SolidBrush brush = new SolidBrush(Color.FromArgb(144, 238, 144));
			e.Graphics.FillRectangle(brush, 2, 2, rectangle.Width, rectangle.Height);
		}
	}
}
