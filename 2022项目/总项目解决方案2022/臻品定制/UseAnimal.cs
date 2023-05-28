using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Background
{
	// Token: 0x02000010 RID: 16
	public class UseAnimal : UserControl
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x0000C8A3 File Offset: 0x0000AAA3
		public UseAnimal(string name, int value)
		{
			this.InitializeComponent();
			this.LblName.Text = name;
			this.TxtValue.Text = value.ToString();
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000C8DC File Offset: 0x0000AADC
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			bool flag2 = flag;
			if (flag2)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000C918 File Offset: 0x0000AB18
		private void InitializeComponent()
		{
			this.LblName = new Label();
			this.TxtValue = new TextBox();
			base.SuspendLayout();
			this.LblName.Dock = DockStyle.Left;
			this.LblName.Font = new Font("宋体", 12f);
			this.LblName.Location = new Point(0, 0);
			this.LblName.Name = "LblName";
			this.LblName.Size = new Size(48, 27);
			this.LblName.TabIndex = 0;
			this.LblName.Text = "label1";
			this.LblName.TextAlign = ContentAlignment.MiddleCenter;
			this.TxtValue.Dock = DockStyle.Fill;
			this.TxtValue.Font = new Font("宋体", 12f);
			this.TxtValue.Location = new Point(48, 0);
			this.TxtValue.Name = "TxtValue";
			this.TxtValue.Size = new Size(202, 26);
			this.TxtValue.TabIndex = 1;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.TxtValue);
			base.Controls.Add(this.LblName);
			base.Name = "UseAnimal";
			base.Size = new Size(250, 27);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040000BF RID: 191
		private IContainer components = null;

		// Token: 0x040000C0 RID: 192
		public Label LblName;

		// Token: 0x040000C1 RID: 193
		public TextBox TxtValue;
	}
}
