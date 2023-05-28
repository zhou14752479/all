namespace Background
{
	// Token: 0x0200000A RID: 10
	public partial class ForLenBao : global::MUWEN.ForParent
	{
		// Token: 0x0600008D RID: 141 RVA: 0x0000ABDC File Offset: 0x00008DDC
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

		// Token: 0x0600008E RID: 142 RVA: 0x0000AC18 File Offset: 0x00008E18
		private void InitializeComponent()
		{
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.BtnJieTu = new global::System.Windows.Forms.Button();
			this.LblTitle = new global::System.Windows.Forms.Label();
			this.panel2 = new global::System.Windows.Forms.Panel();
			this.PanContent = new global::System.Windows.Forms.Panel();
			this.panel3 = new global::System.Windows.Forms.Panel();
			this.label3 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label1 = new global::System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			base.SuspendLayout();
			this.panel1.Controls.Add(this.BtnJieTu);
			this.panel1.Controls.Add(this.LblTitle);
			this.panel1.Dock = global::System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new global::System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(675, 39);
			this.panel1.TabIndex = 0;
			this.BtnJieTu.FlatAppearance.BorderSize = 0;
			this.BtnJieTu.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.BtnJieTu.Font = new global::System.Drawing.Font("微软雅黑", 9f);
			this.BtnJieTu.Location = new global::System.Drawing.Point(626, 1);
			this.BtnJieTu.Name = "BtnJieTu";
			this.BtnJieTu.Size = new global::System.Drawing.Size(48, 25);
			this.BtnJieTu.TabIndex = 4;
			this.BtnJieTu.Text = "截图";
			this.BtnJieTu.UseVisualStyleBackColor = true;
			this.BtnJieTu.Click += new global::System.EventHandler(this.BtnJieTu_Click);
			this.LblTitle.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.LblTitle.Font = new global::System.Drawing.Font("微软雅黑", 16f);
			this.LblTitle.Location = new global::System.Drawing.Point(0, 0);
			this.LblTitle.Name = "LblTitle";
			this.LblTitle.Size = new global::System.Drawing.Size(675, 39);
			this.LblTitle.TabIndex = 0;
			this.LblTitle.Text = "label1";
			this.LblTitle.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.panel2.Controls.Add(this.PanContent);
			this.panel2.Controls.Add(this.panel3);
			this.panel2.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new global::System.Drawing.Point(0, 39);
			this.panel2.Name = "panel2";
			this.panel2.Size = new global::System.Drawing.Size(675, 813);
			this.panel2.TabIndex = 1;
			this.PanContent.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.PanContent.Location = new global::System.Drawing.Point(0, 34);
			this.PanContent.Name = "PanContent";
			this.PanContent.Size = new global::System.Drawing.Size(675, 779);
			this.PanContent.TabIndex = 1;
			this.panel3.Controls.Add(this.label3);
			this.panel3.Controls.Add(this.label2);
			this.panel3.Controls.Add(this.label1);
			this.panel3.Dock = global::System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new global::System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new global::System.Drawing.Size(675, 34);
			this.panel3.TabIndex = 0;
			this.label3.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.label3.Location = new global::System.Drawing.Point(443, 0);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(232, 34);
			this.label3.TabIndex = 0;
			this.label3.Text = "连续未开天数";
			this.label3.TextAlign = global::System.Drawing.ContentAlignment.MiddleCenter;
			this.label2.Dock = global::System.Windows.Forms.DockStyle.Left;
			this.label2.Location = new global::System.Drawing.Point(206, 0);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(237, 34);
			this.label2.TabIndex = 0;
			this.label2.Text = "连续未开场数";
			this.label2.TextAlign = global::System.Drawing.ContentAlignment.MiddleCenter;
			this.label1.Dock = global::System.Windows.Forms.DockStyle.Left;
			this.label1.Location = new global::System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(206, 34);
			this.label1.TabIndex = 0;
			this.label1.Text = "宝名称";
			this.label1.TextAlign = global::System.Drawing.ContentAlignment.MiddleCenter;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(10f, 21f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.FromArgb(249, 191, 143);
			base.ClientSize = new global::System.Drawing.Size(675, 852);
			base.Controls.Add(this.panel2);
			base.Controls.Add(this.panel1);
			base.Name = "ForLenBao";
			this.Text = "冷宝统计";
			base.Load += new global::System.EventHandler(this.ForLenBao_Load);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x04000094 RID: 148
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000095 RID: 149
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x04000096 RID: 150
		private global::System.Windows.Forms.Panel panel2;

		// Token: 0x04000097 RID: 151
		private global::System.Windows.Forms.Panel PanContent;

		// Token: 0x04000098 RID: 152
		private global::System.Windows.Forms.Panel panel3;

		// Token: 0x04000099 RID: 153
		private global::System.Windows.Forms.Label LblTitle;

		// Token: 0x0400009A RID: 154
		private global::System.Windows.Forms.Label label3;

		// Token: 0x0400009B RID: 155
		private global::System.Windows.Forms.Label label2;

		// Token: 0x0400009C RID: 156
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400009D RID: 157
		private global::System.Windows.Forms.Button BtnJieTu;
	}
}
