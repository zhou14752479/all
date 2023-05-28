namespace Background
{
	// Token: 0x02000009 RID: 9
	public partial class ForKaiBao : global::MUWEN.ForParent
	{
		// Token: 0x06000081 RID: 129 RVA: 0x0000A314 File Offset: 0x00008514
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

		// Token: 0x06000082 RID: 130 RVA: 0x0000A350 File Offset: 0x00008550
		private void InitializeComponent()
		{
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.BtnJieTu = new global::System.Windows.Forms.Button();
			this.LblTitle = new global::System.Windows.Forms.Label();
			this.PanKaiBao = new global::System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.panel1.Controls.Add(this.BtnJieTu);
			this.panel1.Controls.Add(this.LblTitle);
			this.panel1.Dock = global::System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new global::System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(826, 38);
			this.panel1.TabIndex = 0;
			this.BtnJieTu.FlatAppearance.BorderSize = 0;
			this.BtnJieTu.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.BtnJieTu.Font = new global::System.Drawing.Font("微软雅黑", 9f);
			this.BtnJieTu.Location = new global::System.Drawing.Point(774, 3);
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
			this.LblTitle.Size = new global::System.Drawing.Size(826, 38);
			this.LblTitle.TabIndex = 0;
			this.LblTitle.Text = "百乐豪截止6月份开宝记录";
			this.LblTitle.TextAlign = global::System.Drawing.ContentAlignment.MiddleCenter;
			this.PanKaiBao.AutoScroll = true;
			this.PanKaiBao.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.PanKaiBao.Location = new global::System.Drawing.Point(0, 38);
			this.PanKaiBao.Name = "PanKaiBao";
			this.PanKaiBao.Size = new global::System.Drawing.Size(826, 775);
			this.PanKaiBao.TabIndex = 1;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(10f, 21f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.FromArgb(249, 191, 143);
			base.ClientSize = new global::System.Drawing.Size(826, 813);
			base.Controls.Add(this.PanKaiBao);
			base.Controls.Add(this.panel1);
			base.Name = "ForKaiBao";
			this.Text = "开宝记录";
			base.Load += new global::System.EventHandler(this.ForKaiBao_Load);
			this.panel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x0400008C RID: 140
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x0400008D RID: 141
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x0400008E RID: 142
		private global::System.Windows.Forms.Label LblTitle;

		// Token: 0x0400008F RID: 143
		private global::System.Windows.Forms.Panel PanKaiBao;

		// Token: 0x04000090 RID: 144
		private global::System.Windows.Forms.Button BtnJieTu;
	}
}
