namespace Background
{
	// Token: 0x0200000D RID: 13
	public partial class ForShuYing : global::MUWEN.ForParent
	{
		// Token: 0x060000AF RID: 175 RVA: 0x0000C1C4 File Offset: 0x0000A3C4
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

		// Token: 0x060000B0 RID: 176 RVA: 0x0000C200 File Offset: 0x0000A400
		private void InitializeComponent()
		{
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.BtnCopy = new global::System.Windows.Forms.Button();
			this.BtnJieTu = new global::System.Windows.Forms.Button();
			this.LblTotalCount = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.label1 = new global::System.Windows.Forms.Label();
			this.PanAnimals = new global::System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.panel1.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.BtnCopy);
			this.panel1.Controls.Add(this.BtnJieTu);
			this.panel1.Controls.Add(this.LblTotalCount);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = global::System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new global::System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(675, 59);
			this.panel1.TabIndex = 0;
			this.BtnCopy.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right);
			this.BtnCopy.BackColor = global::System.Drawing.Color.Transparent;
			this.BtnCopy.FlatAppearance.BorderSize = 0;
			this.BtnCopy.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.BtnCopy.Font = new global::System.Drawing.Font("微软雅黑", 9f);
			this.BtnCopy.Location = new global::System.Drawing.Point(605, 2);
			this.BtnCopy.Name = "BtnCopy";
			this.BtnCopy.Size = new global::System.Drawing.Size(66, 25);
			this.BtnCopy.TabIndex = 3;
			this.BtnCopy.Text = "复制数据";
			this.BtnCopy.UseVisualStyleBackColor = false;
			this.BtnCopy.Click += new global::System.EventHandler(this.BtnCopy_Click);
			this.BtnJieTu.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right);
			this.BtnJieTu.BackColor = global::System.Drawing.Color.Transparent;
			this.BtnJieTu.FlatAppearance.BorderSize = 0;
			this.BtnJieTu.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.BtnJieTu.Font = new global::System.Drawing.Font("微软雅黑", 9f);
			this.BtnJieTu.Location = new global::System.Drawing.Point(605, 29);
			this.BtnJieTu.Name = "BtnJieTu";
			this.BtnJieTu.Size = new global::System.Drawing.Size(66, 25);
			this.BtnJieTu.TabIndex = 3;
			this.BtnJieTu.Text = "截图";
			this.BtnJieTu.UseVisualStyleBackColor = false;
			this.BtnJieTu.Click += new global::System.EventHandler(this.BtnJieTu_Click);
			this.LblTotalCount.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.LblTotalCount.Font = new global::System.Drawing.Font("微软雅黑", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.LblTotalCount.Location = new global::System.Drawing.Point(219, 0);
			this.LblTotalCount.Name = "LblTotalCount";
			this.LblTotalCount.Size = new global::System.Drawing.Size(265, 57);
			this.LblTotalCount.TabIndex = 1;
			this.LblTotalCount.Text = "￥0";
			this.LblTotalCount.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.label3.Dock = global::System.Windows.Forms.DockStyle.Right;
			this.label3.Font = new global::System.Drawing.Font("微软雅黑", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label3.Location = new global::System.Drawing.Point(484, 0);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(189, 57);
			this.label3.TabIndex = 2;
			this.label3.Text = "输赢预测";
			this.label3.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.label1.Dock = global::System.Windows.Forms.DockStyle.Left;
			this.label1.Font = new global::System.Drawing.Font("微软雅黑", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label1.Location = new global::System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(219, 57);
			this.label1.TabIndex = 0;
			this.label1.Text = "合计：";
			this.label1.TextAlign = global::System.Drawing.ContentAlignment.MiddleCenter;
			this.PanAnimals.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.PanAnimals.Location = new global::System.Drawing.Point(0, 59);
			this.PanAnimals.Margin = new global::System.Windows.Forms.Padding(0);
			this.PanAnimals.Name = "PanAnimals";
			this.PanAnimals.Size = new global::System.Drawing.Size(675, 793);
			this.PanAnimals.TabIndex = 1;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(10f, 21f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.BackColor = global::System.Drawing.Color.FromArgb(249, 191, 143);
			base.ClientSize = new global::System.Drawing.Size(675, 852);
			base.Controls.Add(this.PanAnimals);
			base.Controls.Add(this.panel1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "ForShuYing";
			this.Text = "输赢预测";
			base.Load += new global::System.EventHandler(this.ForShuYing_Load);
			this.panel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x040000B7 RID: 183
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x040000B8 RID: 184
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x040000B9 RID: 185
		private global::System.Windows.Forms.Label label3;

		// Token: 0x040000BA RID: 186
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040000BB RID: 187
		private global::System.Windows.Forms.Label LblTotalCount;

		// Token: 0x040000BC RID: 188
		private global::System.Windows.Forms.Panel PanAnimals;

		// Token: 0x040000BD RID: 189
		private global::System.Windows.Forms.Button BtnCopy;

		// Token: 0x040000BE RID: 190
		private global::System.Windows.Forms.Button BtnJieTu;
	}
}
