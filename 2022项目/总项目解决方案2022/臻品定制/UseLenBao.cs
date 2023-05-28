using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Common;

namespace Background
{
	// Token: 0x02000013 RID: 19
	public class UseLenBao : UserControl
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x0000D7B0 File Offset: 0x0000B9B0
		public UseLenBao(Animal animal)
		{
			this.InitializeComponent();
			this.LblName.Text = animal.Name;
			MyProgressBar parent = new MyProgressBar
			{
				Parent = this.PanProgress,
				Minimum = 0,
				Maximum = 100,
				BackColor = Color.FromArgb(249, 191, 143),
				Value = animal.BiLi,
				Dock = DockStyle.Fill
			};
			Label label = new Label
			{
				Parent = parent,
				BackColor = Color.Transparent,
				ForeColor = Color.Black,
				TextAlign = ContentAlignment.MiddleLeft,
				Dock = DockStyle.Fill
			};
			bool flag = animal.WeiKaiChangShu == 0;
			bool flag2 = flag;
			if (flag2)
			{
				label.Text = "开宝";
				this.LblValue.Text = "开宝";
			}
			else
			{
				label.Text = animal.WeiKaiChangShu.ToString();
				this.LblValue.Text = animal.WeiKaiTianShu.ToString();
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000D8D4 File Offset: 0x0000BAD4
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

		// Token: 0x060000C2 RID: 194 RVA: 0x0000D910 File Offset: 0x0000BB10
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
			this.LblName.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
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
			this.LblValue.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
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
			base.Name = "UseLenBao";
			base.Size = new Size(505, 61);
			this.panel1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x040000CF RID: 207
		private IContainer components = null;

		// Token: 0x040000D0 RID: 208
		private Panel panel1;

		// Token: 0x040000D1 RID: 209
		public Label LblName;

		// Token: 0x040000D2 RID: 210
		private Panel PanProgress;

		// Token: 0x040000D3 RID: 211
		private Panel panel3;

		// Token: 0x040000D4 RID: 212
		public Label LblValue;
	}
}
