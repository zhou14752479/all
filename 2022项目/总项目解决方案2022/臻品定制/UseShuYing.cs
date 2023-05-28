using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Background
{
	// Token: 0x02000014 RID: 20
	public class UseShuYing : UserControl
	{
		// Token: 0x060000C3 RID: 195 RVA: 0x0000DC98 File Offset: 0x0000BE98
		public UseShuYing(string name, int count, int shuyingvalue, int bili)
		{
			this.InitializeComponent();
			this.LblName.Text = name;
			this.LblValue.Text = string.Format("￥{0}", shuyingvalue);
			bool flag = shuyingvalue > 0;
			bool flag2 = flag;
			if (flag2)
			{
				this.LblValue.ForeColor = Color.Black;
			}
			else
			{
				this.LblValue.ForeColor = Color.Red;
			}
			bool flag3 = bili < 0;
			bool flag4 = flag3;
			if (flag4)
			{
				bili = 0;
			}
			MyProgressBar parent = new MyProgressBar
			{
				Parent = this.PanProgress,
				Minimum = 0,
				Maximum = 100,
				BackColor = Color.FromArgb(249, 191, 143),
				Value = bili,
				Dock = DockStyle.Fill
			};
			Label label = new Label();
			label.Parent = parent;
			label.BackColor = Color.Transparent;
			label.ForeColor = Color.Black;
			label.TextAlign = ContentAlignment.MiddleLeft;
			label.Dock = DockStyle.Fill;
			label.Text = "￥" + count.ToString();
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000DDCC File Offset: 0x0000BFCC
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

		// Token: 0x060000C5 RID: 197 RVA: 0x0000DE08 File Offset: 0x0000C008
		private void InitializeComponent()
		{
			this.panel1 = new Panel();
			this.LblName = new Label();
			this.PanProgress = new Panel();
			this.panel3 = new Panel();
			this.LblValue = new Label();
			this.panel1.SuspendLayout();
			this.panel3.SuspendLayout();
			base.SuspendLayout();
			this.panel1.Controls.Add(this.LblName);
			this.panel1.Dock = DockStyle.Left;
			this.panel1.Location = new Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(114, 61);
			this.panel1.TabIndex = 0;
			this.LblName.Dock = DockStyle.Fill;
			this.LblName.Font = new Font("宋体", 12f, FontStyle.Bold);
			this.LblName.Location = new Point(0, 0);
			this.LblName.Name = "LblName";
			this.LblName.Size = new Size(114, 61);
			this.LblName.TabIndex = 0;
			this.LblName.Text = "label1";
			this.LblName.TextAlign = ContentAlignment.MiddleCenter;
			this.PanProgress.Dock = DockStyle.Fill;
			this.PanProgress.Font = new Font("宋体", 12f, FontStyle.Bold);
			this.PanProgress.Location = new Point(114, 0);
			this.PanProgress.Name = "PanProgress";
			this.PanProgress.Size = new Size(264, 61);
			this.PanProgress.TabIndex = 1;
			this.panel3.Controls.Add(this.LblValue);
			this.panel3.Dock = DockStyle.Right;
			this.panel3.Location = new Point(378, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new Size(127, 61);
			this.panel3.TabIndex = 2;
			this.LblValue.Dock = DockStyle.Fill;
			this.LblValue.Font = new Font("宋体", 12f, FontStyle.Bold);
			this.LblValue.Location = new Point(0, 0);
			this.LblValue.Name = "LblValue";
			this.LblValue.Size = new Size(127, 61);
			this.LblValue.TabIndex = 0;
			this.LblValue.Text = "label2";
			this.LblValue.TextAlign = ContentAlignment.MiddleLeft;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.FromArgb(249, 191, 143);
			base.Controls.Add(this.PanProgress);
			base.Controls.Add(this.panel3);
			base.Controls.Add(this.panel1);
			base.Name = "UseShuYing";
			base.Size = new Size(505, 61);
			this.panel1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x040000D5 RID: 213
		private IContainer components = null;

		// Token: 0x040000D6 RID: 214
		private Panel panel1;

		// Token: 0x040000D7 RID: 215
		public Label LblName;

		// Token: 0x040000D8 RID: 216
		private Panel PanProgress;

		// Token: 0x040000D9 RID: 217
		private Panel panel3;

		// Token: 0x040000DA RID: 218
		public Label LblValue;
	}
}
