namespace Background
{
	// Token: 0x02000008 RID: 8
	public partial class ForExtractDataJianJie : global::MUWEN.ForParent
	{
		// Token: 0x06000079 RID: 121 RVA: 0x00009578 File Offset: 0x00007778
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

		// Token: 0x0600007A RID: 122 RVA: 0x000095B4 File Offset: 0x000077B4
		private void InitializeComponent()
		{
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.TxtSourceText = new global::System.Windows.Forms.TextBox();
			this.BtnCompute = new global::System.Windows.Forms.Button();
			this.GroDaiLi = new global::System.Windows.Forms.GroupBox();
			this.LstDaiLiData = new global::System.Windows.Forms.ListBox();
			this.panel3 = new global::System.Windows.Forms.Panel();
			this.BtnShuYing = new global::System.Windows.Forms.Button();
			this.BtnClearData = new global::System.Windows.Forms.Button();
			this.groupBox4 = new global::System.Windows.Forms.GroupBox();
			this.LstDaiLi = new global::System.Windows.Forms.ListBox();
			this.panel4 = new global::System.Windows.Forms.Panel();
			this.BtnChangCi = new global::System.Windows.Forms.Button();
			this.BtnTotalShuYing = new global::System.Windows.Forms.Button();
			this.BtnClearAllData = new global::System.Windows.Forms.Button();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.BtnClearText = new global::System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.GroDaiLi.SuspendLayout();
			this.panel3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.groupBox1.Controls.Add(this.TxtSourceText);
			this.groupBox1.Controls.Add(this.panel1);
			this.groupBox1.Location = new global::System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new global::System.Drawing.Size(267, 641);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "原始文本";
			this.TxtSourceText.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.TxtSourceText.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.TxtSourceText.Location = new global::System.Drawing.Point(3, 25);
			this.TxtSourceText.Multiline = true;
			this.TxtSourceText.Name = "TxtSourceText";
			this.TxtSourceText.ScrollBars = global::System.Windows.Forms.ScrollBars.Both;
			this.TxtSourceText.Size = new global::System.Drawing.Size(261, 559);
			this.TxtSourceText.TabIndex = 0;
			this.BtnCompute.Dock = global::System.Windows.Forms.DockStyle.Left;
			this.BtnCompute.Location = new global::System.Drawing.Point(0, 0);
			this.BtnCompute.Name = "BtnCompute";
			this.BtnCompute.Size = new global::System.Drawing.Size(130, 54);
			this.BtnCompute.TabIndex = 8;
			this.BtnCompute.Text = "统计数据";
			this.BtnCompute.UseVisualStyleBackColor = true;
			this.BtnCompute.Click += new global::System.EventHandler(this.BtnCompute_Click);
			this.GroDaiLi.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.GroDaiLi.Controls.Add(this.LstDaiLiData);
			this.GroDaiLi.Controls.Add(this.panel3);
			this.GroDaiLi.Location = new global::System.Drawing.Point(558, 18);
			this.GroDaiLi.Name = "GroDaiLi";
			this.GroDaiLi.Size = new global::System.Drawing.Size(267, 635);
			this.GroDaiLi.TabIndex = 10;
			this.GroDaiLi.TabStop = false;
			this.GroDaiLi.Text = "当前代理人数据";
			this.LstDaiLiData.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.LstDaiLiData.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.LstDaiLiData.FormattingEnabled = true;
			this.LstDaiLiData.ItemHeight = 21;
			this.LstDaiLiData.Location = new global::System.Drawing.Point(3, 25);
			this.LstDaiLiData.Name = "LstDaiLiData";
			this.LstDaiLiData.Size = new global::System.Drawing.Size(261, 553);
			this.LstDaiLiData.TabIndex = 0;
			this.panel3.Controls.Add(this.BtnShuYing);
			this.panel3.Controls.Add(this.BtnClearData);
			this.panel3.Dock = global::System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new global::System.Drawing.Point(3, 578);
			this.panel3.Name = "panel3";
			this.panel3.Size = new global::System.Drawing.Size(261, 54);
			this.panel3.TabIndex = 2;
			this.BtnShuYing.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.BtnShuYing.Enabled = false;
			this.BtnShuYing.Location = new global::System.Drawing.Point(0, 0);
			this.BtnShuYing.Name = "BtnShuYing";
			this.BtnShuYing.Size = new global::System.Drawing.Size(134, 54);
			this.BtnShuYing.TabIndex = 2;
			this.BtnShuYing.Text = "输赢预测";
			this.BtnShuYing.UseVisualStyleBackColor = true;
			this.BtnShuYing.Click += new global::System.EventHandler(this.BtnShuYing_Click);
			this.BtnClearData.Dock = global::System.Windows.Forms.DockStyle.Right;
			this.BtnClearData.Enabled = false;
			this.BtnClearData.Location = new global::System.Drawing.Point(134, 0);
			this.BtnClearData.Name = "BtnClearData";
			this.BtnClearData.Size = new global::System.Drawing.Size(127, 54);
			this.BtnClearData.TabIndex = 1;
			this.BtnClearData.Text = "清空数据";
			this.BtnClearData.UseVisualStyleBackColor = true;
			this.BtnClearData.Click += new global::System.EventHandler(this.BtnClearData_Click);
			this.groupBox4.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.groupBox4.Controls.Add(this.LstDaiLi);
			this.groupBox4.Controls.Add(this.panel4);
			this.groupBox4.Location = new global::System.Drawing.Point(285, 18);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new global::System.Drawing.Size(267, 635);
			this.groupBox4.TabIndex = 9;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "代理人";
			this.LstDaiLi.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.LstDaiLi.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.LstDaiLi.FormattingEnabled = true;
			this.LstDaiLi.ItemHeight = 21;
			this.LstDaiLi.Location = new global::System.Drawing.Point(3, 25);
			this.LstDaiLi.Name = "LstDaiLi";
			this.LstDaiLi.Size = new global::System.Drawing.Size(261, 493);
			this.LstDaiLi.TabIndex = 0;
			this.LstDaiLi.SelectedIndexChanged += new global::System.EventHandler(this.LstDaiLi_SelectedIndexChanged);
			this.panel4.Controls.Add(this.BtnChangCi);
			this.panel4.Controls.Add(this.BtnTotalShuYing);
			this.panel4.Controls.Add(this.BtnClearAllData);
			this.panel4.Dock = global::System.Windows.Forms.DockStyle.Bottom;
			this.panel4.Location = new global::System.Drawing.Point(3, 518);
			this.panel4.Name = "panel4";
			this.panel4.Size = new global::System.Drawing.Size(261, 114);
			this.panel4.TabIndex = 2;
			this.BtnChangCi.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.BtnChangCi.Enabled = false;
			this.BtnChangCi.Location = new global::System.Drawing.Point(128, 60);
			this.BtnChangCi.Name = "BtnChangCi";
			this.BtnChangCi.Size = new global::System.Drawing.Size(133, 54);
			this.BtnChangCi.TabIndex = 4;
			this.BtnChangCi.Text = "场次";
			this.BtnChangCi.UseVisualStyleBackColor = true;
			this.BtnChangCi.Click += new global::System.EventHandler(this.BtnChangCi_Click);
			this.BtnTotalShuYing.Dock = global::System.Windows.Forms.DockStyle.Left;
			this.BtnTotalShuYing.Enabled = false;
			this.BtnTotalShuYing.Location = new global::System.Drawing.Point(0, 60);
			this.BtnTotalShuYing.Name = "BtnTotalShuYing";
			this.BtnTotalShuYing.Size = new global::System.Drawing.Size(128, 54);
			this.BtnTotalShuYing.TabIndex = 2;
			this.BtnTotalShuYing.Text = "输赢预测";
			this.BtnTotalShuYing.UseVisualStyleBackColor = true;
			this.BtnTotalShuYing.Click += new global::System.EventHandler(this.BtnTotalShuYing_Click);
			this.BtnClearAllData.Dock = global::System.Windows.Forms.DockStyle.Top;
			this.BtnClearAllData.Location = new global::System.Drawing.Point(0, 0);
			this.BtnClearAllData.Name = "BtnClearAllData";
			this.BtnClearAllData.Size = new global::System.Drawing.Size(261, 60);
			this.BtnClearAllData.TabIndex = 5;
			this.BtnClearAllData.Text = "清空数据";
			this.BtnClearAllData.UseVisualStyleBackColor = true;
			this.BtnClearAllData.Click += new global::System.EventHandler(this.BtnClearAllData_Click);
			this.panel1.Controls.Add(this.BtnClearText);
			this.panel1.Controls.Add(this.BtnCompute);
			this.panel1.Dock = global::System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new global::System.Drawing.Point(3, 584);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(261, 54);
			this.panel1.TabIndex = 9;
			this.BtnClearText.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.BtnClearText.Location = new global::System.Drawing.Point(130, 0);
			this.BtnClearText.Name = "BtnClearText";
			this.BtnClearText.Size = new global::System.Drawing.Size(131, 54);
			this.BtnClearText.TabIndex = 9;
			this.BtnClearText.Text = "清空";
			this.BtnClearText.UseVisualStyleBackColor = true;
			this.BtnClearText.Click += new global::System.EventHandler(this.BtnClearText_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(10f, 21f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(840, 665);
			base.Controls.Add(this.GroDaiLi);
			base.Controls.Add(this.groupBox4);
			base.Controls.Add(this.groupBox1);
			base.Name = "ForExtractDataJianJie";
			this.Text = "数据分析";
			base.Load += new global::System.EventHandler(this.ForExtractData_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.GroDaiLi.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x0400007A RID: 122
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x0400007B RID: 123
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x0400007C RID: 124
		private global::System.Windows.Forms.TextBox TxtSourceText;

		// Token: 0x0400007D RID: 125
		private global::System.Windows.Forms.Button BtnCompute;

		// Token: 0x0400007E RID: 126
		private global::System.Windows.Forms.GroupBox GroDaiLi;

		// Token: 0x0400007F RID: 127
		private global::System.Windows.Forms.ListBox LstDaiLiData;

		// Token: 0x04000080 RID: 128
		private global::System.Windows.Forms.Panel panel3;

		// Token: 0x04000081 RID: 129
		private global::System.Windows.Forms.Button BtnShuYing;

		// Token: 0x04000082 RID: 130
		private global::System.Windows.Forms.Button BtnClearData;

		// Token: 0x04000083 RID: 131
		private global::System.Windows.Forms.GroupBox groupBox4;

		// Token: 0x04000084 RID: 132
		private global::System.Windows.Forms.ListBox LstDaiLi;

		// Token: 0x04000085 RID: 133
		private global::System.Windows.Forms.Panel panel4;

		// Token: 0x04000086 RID: 134
		private global::System.Windows.Forms.Button BtnChangCi;

		// Token: 0x04000087 RID: 135
		private global::System.Windows.Forms.Button BtnTotalShuYing;

		// Token: 0x04000088 RID: 136
		private global::System.Windows.Forms.Button BtnClearAllData;

		// Token: 0x04000089 RID: 137
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x0400008A RID: 138
		private global::System.Windows.Forms.Button BtnClearText;
	}
}
