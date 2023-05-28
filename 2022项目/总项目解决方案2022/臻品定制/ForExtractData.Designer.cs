namespace Background
{
	// Token: 0x02000007 RID: 7
	public partial class ForExtractData : global::MUWEN.ForParent
	{
		// Token: 0x06000067 RID: 103 RVA: 0x00007AB4 File Offset: 0x00005CB4
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

		// Token: 0x06000068 RID: 104 RVA: 0x00007AF0 File Offset: 0x00005CF0
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			this.TxtSourceText = new global::System.Windows.Forms.TextBox();
			this.TxtProcessed = new global::System.Windows.Forms.TextBox();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.BtnCompute = new global::System.Windows.Forms.Button();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.BtnMoveToYuanShi = new global::System.Windows.Forms.Button();
			this.BtnSelect = new global::System.Windows.Forms.Button();
			this.GroResult = new global::System.Windows.Forms.GroupBox();
			this.PanResults = new global::System.Windows.Forms.FlowLayoutPanel();
			this.panel2 = new global::System.Windows.Forms.Panel();
			this.BtnClearCount = new global::System.Windows.Forms.Button();
			this.BtnUpdateCount = new global::System.Windows.Forms.Button();
			this.groupBox4 = new global::System.Windows.Forms.GroupBox();
			this.LstDaiLi = new global::System.Windows.Forms.ListBox();
			this.BtnAddData = new global::System.Windows.Forms.Button();
			this.GroDaiLi = new global::System.Windows.Forms.GroupBox();
			this.LstDaiLiData = new global::System.Windows.Forms.ListBox();
			this.panel3 = new global::System.Windows.Forms.Panel();
			this.BtnShuYing = new global::System.Windows.Forms.Button();
			this.BtnClearData = new global::System.Windows.Forms.Button();
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			this.panel4 = new global::System.Windows.Forms.Panel();
			this.BtnTotalShuYing = new global::System.Windows.Forms.Button();
			this.panel5 = new global::System.Windows.Forms.Panel();
			this.BtnChangCi = new global::System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.GroResult.SuspendLayout();
			this.panel2.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.GroDaiLi.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel5.SuspendLayout();
			base.SuspendLayout();
			this.TxtSourceText.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.TxtSourceText.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.TxtSourceText.Location = new global::System.Drawing.Point(3, 25);
			this.TxtSourceText.Multiline = true;
			this.TxtSourceText.Name = "TxtSourceText";
			this.TxtSourceText.ScrollBars = global::System.Windows.Forms.ScrollBars.Both;
			this.TxtSourceText.Size = new global::System.Drawing.Size(261, 553);
			this.TxtSourceText.TabIndex = 0;
			this.TxtProcessed.BackColor = global::System.Drawing.Color.White;
			this.TxtProcessed.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.TxtProcessed.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.TxtProcessed.Location = new global::System.Drawing.Point(3, 25);
			this.TxtProcessed.Multiline = true;
			this.TxtProcessed.Name = "TxtProcessed";
			this.TxtProcessed.ReadOnly = true;
			this.TxtProcessed.ScrollBars = global::System.Windows.Forms.ScrollBars.Both;
			this.TxtProcessed.Size = new global::System.Drawing.Size(261, 553);
			this.TxtProcessed.TabIndex = 3;
			this.groupBox1.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.groupBox1.Controls.Add(this.TxtSourceText);
			this.groupBox1.Controls.Add(this.BtnCompute);
			this.groupBox1.Location = new global::System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new global::System.Drawing.Size(267, 635);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "原始文本";
			this.BtnCompute.Dock = global::System.Windows.Forms.DockStyle.Bottom;
			this.BtnCompute.Location = new global::System.Drawing.Point(3, 578);
			this.BtnCompute.Name = "BtnCompute";
			this.BtnCompute.Size = new global::System.Drawing.Size(261, 54);
			this.BtnCompute.TabIndex = 8;
			this.BtnCompute.Text = "统计数据";
			this.toolTip1.SetToolTip(this.BtnCompute, "将统计后的结果追加到统计结果中");
			this.BtnCompute.UseVisualStyleBackColor = true;
			this.BtnCompute.Click += new global::System.EventHandler(this.BtnCompute_Click);
			this.groupBox2.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.groupBox2.Controls.Add(this.TxtProcessed);
			this.groupBox2.Controls.Add(this.panel1);
			this.groupBox2.Location = new global::System.Drawing.Point(285, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new global::System.Drawing.Size(267, 635);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "处理后的文本";
			this.panel1.Controls.Add(this.BtnMoveToYuanShi);
			this.panel1.Controls.Add(this.BtnSelect);
			this.panel1.Dock = global::System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new global::System.Drawing.Point(3, 578);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(261, 54);
			this.panel1.TabIndex = 5;
			this.BtnMoveToYuanShi.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.BtnMoveToYuanShi.Enabled = false;
			this.BtnMoveToYuanShi.Location = new global::System.Drawing.Point(0, 0);
			this.BtnMoveToYuanShi.Name = "BtnMoveToYuanShi";
			this.BtnMoveToYuanShi.Size = new global::System.Drawing.Size(131, 54);
			this.BtnMoveToYuanShi.TabIndex = 5;
			this.BtnMoveToYuanShi.Text = "将文本移动至原始文本中";
			this.toolTip1.SetToolTip(this.BtnMoveToYuanShi, "将文本移动至原始文本后，可进行二次统计数据");
			this.BtnMoveToYuanShi.UseVisualStyleBackColor = true;
			this.BtnMoveToYuanShi.Click += new global::System.EventHandler(this.BtnMoveToYuanShi_Click);
			this.BtnSelect.Dock = global::System.Windows.Forms.DockStyle.Right;
			this.BtnSelect.Enabled = false;
			this.BtnSelect.Location = new global::System.Drawing.Point(131, 0);
			this.BtnSelect.Name = "BtnSelect";
			this.BtnSelect.Size = new global::System.Drawing.Size(130, 54);
			this.BtnSelect.TabIndex = 4;
			this.BtnSelect.Text = "从选中文本中查找原始文本位置";
			this.toolTip1.SetToolTip(this.BtnSelect, "查找前请行选择文本");
			this.BtnSelect.UseVisualStyleBackColor = true;
			this.BtnSelect.Click += new global::System.EventHandler(this.BtnSelect_Click);
			this.GroResult.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.GroResult.Controls.Add(this.PanResults);
			this.GroResult.Controls.Add(this.panel2);
			this.GroResult.Location = new global::System.Drawing.Point(558, 12);
			this.GroResult.Name = "GroResult";
			this.GroResult.Size = new global::System.Drawing.Size(267, 635);
			this.GroResult.TabIndex = 6;
			this.GroResult.TabStop = false;
			this.GroResult.Text = "统计结果";
			this.PanResults.AutoScroll = true;
			this.PanResults.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.PanResults.Location = new global::System.Drawing.Point(3, 25);
			this.PanResults.Name = "PanResults";
			this.PanResults.Size = new global::System.Drawing.Size(261, 553);
			this.PanResults.TabIndex = 0;
			this.panel2.Controls.Add(this.BtnClearCount);
			this.panel2.Controls.Add(this.BtnUpdateCount);
			this.panel2.Dock = global::System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new global::System.Drawing.Point(3, 578);
			this.panel2.Name = "panel2";
			this.panel2.Size = new global::System.Drawing.Size(261, 54);
			this.panel2.TabIndex = 2;
			this.BtnClearCount.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.BtnClearCount.Enabled = false;
			this.BtnClearCount.Location = new global::System.Drawing.Point(0, 0);
			this.BtnClearCount.Name = "BtnClearCount";
			this.BtnClearCount.Size = new global::System.Drawing.Size(132, 54);
			this.BtnClearCount.TabIndex = 2;
			this.BtnClearCount.Text = "清空数量";
			this.BtnClearCount.UseVisualStyleBackColor = true;
			this.BtnClearCount.Click += new global::System.EventHandler(this.BtnClearCount_Click);
			this.BtnUpdateCount.Dock = global::System.Windows.Forms.DockStyle.Right;
			this.BtnUpdateCount.Enabled = false;
			this.BtnUpdateCount.Location = new global::System.Drawing.Point(132, 0);
			this.BtnUpdateCount.Name = "BtnUpdateCount";
			this.BtnUpdateCount.Size = new global::System.Drawing.Size(129, 54);
			this.BtnUpdateCount.TabIndex = 1;
			this.BtnUpdateCount.Text = "更新数量";
			this.toolTip1.SetToolTip(this.BtnUpdateCount, "将文本框中修改后的数据同步至数据库，否则追加数据不能将修改后的数据叠加");
			this.BtnUpdateCount.UseVisualStyleBackColor = true;
			this.BtnUpdateCount.Click += new global::System.EventHandler(this.BtnUpdateCount_Click);
			this.groupBox4.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.groupBox4.Controls.Add(this.LstDaiLi);
			this.groupBox4.Controls.Add(this.panel4);
			this.groupBox4.Location = new global::System.Drawing.Point(831, 12);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new global::System.Drawing.Size(267, 635);
			this.groupBox4.TabIndex = 7;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "代理人";
			this.LstDaiLi.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.LstDaiLi.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.LstDaiLi.FormattingEnabled = true;
			this.LstDaiLi.ItemHeight = 21;
			this.LstDaiLi.Location = new global::System.Drawing.Point(3, 25);
			this.LstDaiLi.Name = "LstDaiLi";
			this.LstDaiLi.Size = new global::System.Drawing.Size(261, 502);
			this.LstDaiLi.TabIndex = 0;
			this.LstDaiLi.SelectedIndexChanged += new global::System.EventHandler(this.LstDaiLi_SelectedIndexChanged);
			this.BtnAddData.Dock = global::System.Windows.Forms.DockStyle.Right;
			this.BtnAddData.Enabled = false;
			this.BtnAddData.Location = new global::System.Drawing.Point(128, 0);
			this.BtnAddData.Name = "BtnAddData";
			this.BtnAddData.Size = new global::System.Drawing.Size(133, 54);
			this.BtnAddData.TabIndex = 1;
			this.BtnAddData.Text = "追加数据";
			this.toolTip1.SetToolTip(this.BtnAddData, "将统计结果的数据叠加到当前代理人数据上");
			this.BtnAddData.UseVisualStyleBackColor = true;
			this.BtnAddData.Click += new global::System.EventHandler(this.BtnAddData_Click);
			this.GroDaiLi.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.GroDaiLi.Controls.Add(this.LstDaiLiData);
			this.GroDaiLi.Controls.Add(this.panel3);
			this.GroDaiLi.Location = new global::System.Drawing.Point(1104, 12);
			this.GroDaiLi.Name = "GroDaiLi";
			this.GroDaiLi.Size = new global::System.Drawing.Size(267, 635);
			this.GroDaiLi.TabIndex = 8;
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
			this.toolTip1.AutomaticDelay = 100;
			this.toolTip1.AutoPopDelay = 10000;
			this.toolTip1.InitialDelay = 100;
			this.toolTip1.ReshowDelay = 20;
			this.panel4.Controls.Add(this.BtnChangCi);
			this.panel4.Controls.Add(this.panel5);
			this.panel4.Dock = global::System.Windows.Forms.DockStyle.Bottom;
			this.panel4.Location = new global::System.Drawing.Point(3, 527);
			this.panel4.Name = "panel4";
			this.panel4.Size = new global::System.Drawing.Size(261, 105);
			this.panel4.TabIndex = 2;
			this.BtnTotalShuYing.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.BtnTotalShuYing.Enabled = false;
			this.BtnTotalShuYing.Location = new global::System.Drawing.Point(0, 0);
			this.BtnTotalShuYing.Name = "BtnTotalShuYing";
			this.BtnTotalShuYing.Size = new global::System.Drawing.Size(128, 54);
			this.BtnTotalShuYing.TabIndex = 2;
			this.BtnTotalShuYing.Text = "输赢预测";
			this.BtnTotalShuYing.UseVisualStyleBackColor = true;
			this.BtnTotalShuYing.Click += new global::System.EventHandler(this.BtnTotalShuYing_Click);
			this.panel5.Controls.Add(this.BtnTotalShuYing);
			this.panel5.Controls.Add(this.BtnAddData);
			this.panel5.Dock = global::System.Windows.Forms.DockStyle.Bottom;
			this.panel5.Location = new global::System.Drawing.Point(0, 51);
			this.panel5.Name = "panel5";
			this.panel5.Size = new global::System.Drawing.Size(261, 54);
			this.panel5.TabIndex = 3;
			this.BtnChangCi.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.BtnChangCi.Enabled = false;
			this.BtnChangCi.Location = new global::System.Drawing.Point(0, 0);
			this.BtnChangCi.Name = "BtnChangCi";
			this.BtnChangCi.Size = new global::System.Drawing.Size(261, 51);
			this.BtnChangCi.TabIndex = 4;
			this.BtnChangCi.Text = "场次";
			this.BtnChangCi.UseVisualStyleBackColor = true;
			this.BtnChangCi.Click += new global::System.EventHandler(this.BtnChangCi_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(10f, 21f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(1382, 659);
			base.Controls.Add(this.GroDaiLi);
			base.Controls.Add(this.groupBox4);
			base.Controls.Add(this.GroResult);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.Sizable;
			base.MaximizeBox = true;
			base.MinimizeBox = true;
			base.Name = "ForExtractData";
			this.Text = "数据分析";
			base.Load += new global::System.EventHandler(this.ForExtractData_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.GroResult.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.GroDaiLi.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x0400005D RID: 93
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x0400005E RID: 94
		private global::System.Windows.Forms.TextBox TxtSourceText;

		// Token: 0x0400005F RID: 95
		private global::System.Windows.Forms.TextBox TxtProcessed;

		// Token: 0x04000060 RID: 96
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04000061 RID: 97
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x04000062 RID: 98
		private global::System.Windows.Forms.GroupBox GroResult;

		// Token: 0x04000063 RID: 99
		private global::System.Windows.Forms.FlowLayoutPanel PanResults;

		// Token: 0x04000064 RID: 100
		private global::System.Windows.Forms.GroupBox groupBox4;

		// Token: 0x04000065 RID: 101
		private global::System.Windows.Forms.ListBox LstDaiLi;

		// Token: 0x04000066 RID: 102
		private global::System.Windows.Forms.Button BtnCompute;

		// Token: 0x04000067 RID: 103
		private global::System.Windows.Forms.Button BtnAddData;

		// Token: 0x04000068 RID: 104
		private global::System.Windows.Forms.GroupBox GroDaiLi;

		// Token: 0x04000069 RID: 105
		private global::System.Windows.Forms.ListBox LstDaiLiData;

		// Token: 0x0400006A RID: 106
		private global::System.Windows.Forms.Button BtnClearData;

		// Token: 0x0400006B RID: 107
		private global::System.Windows.Forms.Button BtnSelect;

		// Token: 0x0400006C RID: 108
		private global::System.Windows.Forms.Button BtnUpdateCount;

		// Token: 0x0400006D RID: 109
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x0400006E RID: 110
		private global::System.Windows.Forms.Button BtnMoveToYuanShi;

		// Token: 0x0400006F RID: 111
		private global::System.Windows.Forms.Panel panel2;

		// Token: 0x04000070 RID: 112
		private global::System.Windows.Forms.Button BtnClearCount;

		// Token: 0x04000071 RID: 113
		private global::System.Windows.Forms.ToolTip toolTip1;

		// Token: 0x04000072 RID: 114
		private global::System.Windows.Forms.Panel panel3;

		// Token: 0x04000073 RID: 115
		private global::System.Windows.Forms.Button BtnShuYing;

		// Token: 0x04000074 RID: 116
		private global::System.Windows.Forms.Panel panel4;

		// Token: 0x04000075 RID: 117
		private global::System.Windows.Forms.Button BtnTotalShuYing;

		// Token: 0x04000076 RID: 118
		private global::System.Windows.Forms.Button BtnChangCi;

		// Token: 0x04000077 RID: 119
		private global::System.Windows.Forms.Panel panel5;
	}
}
