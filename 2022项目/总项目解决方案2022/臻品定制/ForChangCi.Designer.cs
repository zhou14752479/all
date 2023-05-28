namespace Background
{
	// Token: 0x02000004 RID: 4
	public partial class ForChangCi : global::MUWEN.ForParent
	{
		// Token: 0x06000028 RID: 40 RVA: 0x00003E90 File Offset: 0x00002090
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

		// Token: 0x06000029 RID: 41 RVA: 0x00003ECC File Offset: 0x000020CC
		private void InitializeComponent()
		{
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.CheQuanBuChangCi = new global::System.Windows.Forms.CheckBox();
			this.BtnJieTu = new global::System.Windows.Forms.Button();
			this.BtnChangCiManager = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.CmbDaiLi = new global::System.Windows.Forms.ComboBox();
			this.panel2 = new global::System.Windows.Forms.Panel();
			this.PanContent = new global::System.Windows.Forms.Panel();
			this.panel5 = new global::System.Windows.Forms.Panel();
			this.label8 = new global::System.Windows.Forms.Label();
			this.label7 = new global::System.Windows.Forms.Label();
			this.label6 = new global::System.Windows.Forms.Label();
			this.label5 = new global::System.Windows.Forms.Label();
			this.label4 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.panel3 = new global::System.Windows.Forms.Panel();
			this.LblDate = new global::System.Windows.Forms.Label();
			this.LblDaiLiFeiLv = new global::System.Windows.Forms.Label();
			this.LblName = new global::System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel5.SuspendLayout();
			this.panel3.SuspendLayout();
			base.SuspendLayout();
			this.panel1.Controls.Add(this.CheQuanBuChangCi);
			this.panel1.Controls.Add(this.BtnJieTu);
			this.panel1.Controls.Add(this.BtnChangCiManager);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.CmbDaiLi);
			this.panel1.Dock = global::System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new global::System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(946, 62);
			this.panel1.TabIndex = 0;
			this.CheQuanBuChangCi.AutoSize = true;
			this.CheQuanBuChangCi.Location = new global::System.Drawing.Point(767, 23);
			this.CheQuanBuChangCi.Name = "CheQuanBuChangCi";
			this.CheQuanBuChangCi.Size = new global::System.Drawing.Size(93, 25);
			this.CheQuanBuChangCi.TabIndex = 4;
			this.CheQuanBuChangCi.Text = "全部场次";
			this.CheQuanBuChangCi.UseVisualStyleBackColor = true;
			this.CheQuanBuChangCi.CheckedChanged += new global::System.EventHandler(this.CheQuanBuChangCi_CheckedChanged);
			this.BtnJieTu.FlatAppearance.BorderSize = 0;
			this.BtnJieTu.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.BtnJieTu.Font = new global::System.Drawing.Font("微软雅黑", 9f);
			this.BtnJieTu.Location = new global::System.Drawing.Point(895, 3);
			this.BtnJieTu.Name = "BtnJieTu";
			this.BtnJieTu.Size = new global::System.Drawing.Size(48, 25);
			this.BtnJieTu.TabIndex = 3;
			this.BtnJieTu.Text = "截图";
			this.BtnJieTu.UseVisualStyleBackColor = true;
			this.BtnJieTu.Click += new global::System.EventHandler(this.BtnJieTu_Click);
			this.BtnChangCiManager.Location = new global::System.Drawing.Point(641, 20);
			this.BtnChangCiManager.Name = "BtnChangCiManager";
			this.BtnChangCiManager.Size = new global::System.Drawing.Size(120, 29);
			this.BtnChangCiManager.TabIndex = 2;
			this.BtnChangCiManager.Text = "场次管理";
			this.BtnChangCiManager.UseVisualStyleBackColor = true;
			this.BtnChangCiManager.Click += new global::System.EventHandler(this.BtnChangCiManager_Click);
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(25, 23);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(42, 21);
			this.label1.TabIndex = 1;
			this.label1.Text = "代理";
			this.CmbDaiLi.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CmbDaiLi.FormattingEnabled = true;
			this.CmbDaiLi.Location = new global::System.Drawing.Point(73, 20);
			this.CmbDaiLi.Name = "CmbDaiLi";
			this.CmbDaiLi.Size = new global::System.Drawing.Size(562, 29);
			this.CmbDaiLi.TabIndex = 0;
			this.CmbDaiLi.SelectedIndexChanged += new global::System.EventHandler(this.CmbDaiLi_SelectedIndexChanged);
			this.panel2.Controls.Add(this.PanContent);
			this.panel2.Controls.Add(this.panel5);
			this.panel2.Controls.Add(this.panel3);
			this.panel2.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new global::System.Drawing.Point(0, 62);
			this.panel2.Name = "panel2";
			this.panel2.Size = new global::System.Drawing.Size(946, 292);
			this.panel2.TabIndex = 1;
			this.PanContent.BackColor = global::System.Drawing.Color.FromArgb(249, 191, 143);
			this.PanContent.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.PanContent.Location = new global::System.Drawing.Point(0, 80);
			this.PanContent.Name = "PanContent";
			this.PanContent.Size = new global::System.Drawing.Size(946, 212);
			this.PanContent.TabIndex = 1;
			this.panel5.BackColor = global::System.Drawing.Color.FromArgb(249, 191, 143);
			this.panel5.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel5.Controls.Add(this.label8);
			this.panel5.Controls.Add(this.label7);
			this.panel5.Controls.Add(this.label6);
			this.panel5.Controls.Add(this.label5);
			this.panel5.Controls.Add(this.label4);
			this.panel5.Controls.Add(this.label3);
			this.panel5.Controls.Add(this.label2);
			this.panel5.Dock = global::System.Windows.Forms.DockStyle.Top;
			this.panel5.Location = new global::System.Drawing.Point(0, 40);
			this.panel5.Name = "panel5";
			this.panel5.Size = new global::System.Drawing.Size(946, 40);
			this.panel5.TabIndex = 2;
			this.label8.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.label8.Font = new global::System.Drawing.Font("微软雅黑", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label8.Location = new global::System.Drawing.Point(654, 0);
			this.label8.Name = "label8";
			this.label8.Size = new global::System.Drawing.Size(290, 38);
			this.label8.TabIndex = 0;
			this.label8.Text = "输赢（押注-代理费-中宝赔钱）";
			this.label8.TextAlign = global::System.Drawing.ContentAlignment.MiddleCenter;
			this.label7.Dock = global::System.Windows.Forms.DockStyle.Left;
			this.label7.Font = new global::System.Drawing.Font("微软雅黑", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label7.Location = new global::System.Drawing.Point(449, 0);
			this.label7.Name = "label7";
			this.label7.Size = new global::System.Drawing.Size(205, 38);
			this.label7.TabIndex = 0;
			this.label7.Text = "中宝需要赔的钱";
			this.label7.TextAlign = global::System.Drawing.ContentAlignment.MiddleCenter;
			this.label6.Dock = global::System.Windows.Forms.DockStyle.Left;
			this.label6.Font = new global::System.Drawing.Font("微软雅黑", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label6.Location = new global::System.Drawing.Point(339, 0);
			this.label6.Name = "label6";
			this.label6.Size = new global::System.Drawing.Size(110, 38);
			this.label6.TabIndex = 0;
			this.label6.Text = "中宝押注";
			this.label6.TextAlign = global::System.Drawing.ContentAlignment.MiddleCenter;
			this.label5.Dock = global::System.Windows.Forms.DockStyle.Left;
			this.label5.Font = new global::System.Drawing.Font("微软雅黑", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label5.Location = new global::System.Drawing.Point(224, 0);
			this.label5.Name = "label5";
			this.label5.Size = new global::System.Drawing.Size(115, 38);
			this.label5.TabIndex = 0;
			this.label5.Text = "代理费";
			this.label5.TextAlign = global::System.Drawing.ContentAlignment.MiddleCenter;
			this.label4.Dock = global::System.Windows.Forms.DockStyle.Left;
			this.label4.Font = new global::System.Drawing.Font("微软雅黑", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label4.Location = new global::System.Drawing.Point(120, 0);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(104, 38);
			this.label4.TabIndex = 0;
			this.label4.Text = "总押注额";
			this.label4.TextAlign = global::System.Drawing.ContentAlignment.MiddleCenter;
			this.label3.Dock = global::System.Windows.Forms.DockStyle.Left;
			this.label3.Font = new global::System.Drawing.Font("微软雅黑", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label3.Location = new global::System.Drawing.Point(60, 0);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(60, 38);
			this.label3.TabIndex = 0;
			this.label3.Text = "开宝";
			this.label3.TextAlign = global::System.Drawing.ContentAlignment.MiddleCenter;
			this.label2.Dock = global::System.Windows.Forms.DockStyle.Left;
			this.label2.Font = new global::System.Drawing.Font("微软雅黑", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label2.Location = new global::System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(60, 38);
			this.label2.TabIndex = 0;
			this.label2.Text = "场次";
			this.label2.TextAlign = global::System.Drawing.ContentAlignment.MiddleCenter;
			this.panel3.BackColor = global::System.Drawing.Color.FromArgb(249, 191, 143);
			this.panel3.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel3.Controls.Add(this.LblDate);
			this.panel3.Controls.Add(this.LblDaiLiFeiLv);
			this.panel3.Controls.Add(this.LblName);
			this.panel3.Dock = global::System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new global::System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new global::System.Drawing.Size(946, 40);
			this.panel3.TabIndex = 0;
			this.LblDate.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.LblDate.Font = new global::System.Drawing.Font("微软雅黑", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.LblDate.Location = new global::System.Drawing.Point(210, 0);
			this.LblDate.Name = "LblDate";
			this.LblDate.Size = new global::System.Drawing.Size(410, 38);
			this.LblDate.TabIndex = 1;
			this.LblDate.Text = "账目报告";
			this.LblDate.TextAlign = global::System.Drawing.ContentAlignment.MiddleCenter;
			this.LblDaiLiFeiLv.Dock = global::System.Windows.Forms.DockStyle.Right;
			this.LblDaiLiFeiLv.Font = new global::System.Drawing.Font("微软雅黑", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.LblDaiLiFeiLv.Location = new global::System.Drawing.Point(620, 0);
			this.LblDaiLiFeiLv.Name = "LblDaiLiFeiLv";
			this.LblDaiLiFeiLv.Size = new global::System.Drawing.Size(324, 38);
			this.LblDaiLiFeiLv.TabIndex = 1;
			this.LblDaiLiFeiLv.Text = "0%";
			this.LblDaiLiFeiLv.TextAlign = global::System.Drawing.ContentAlignment.MiddleCenter;
			this.LblName.Dock = global::System.Windows.Forms.DockStyle.Left;
			this.LblName.Font = new global::System.Drawing.Font("微软雅黑", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.LblName.Location = new global::System.Drawing.Point(0, 0);
			this.LblName.Name = "LblName";
			this.LblName.Size = new global::System.Drawing.Size(210, 38);
			this.LblName.TabIndex = 0;
			this.LblName.Text = "名称";
			this.LblName.TextAlign = global::System.Drawing.ContentAlignment.MiddleCenter;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(10f, 21f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			base.ClientSize = new global::System.Drawing.Size(946, 354);
			base.Controls.Add(this.panel2);
			base.Controls.Add(this.panel1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.Sizable;
			base.Name = "ForChangCi";
			this.Text = "场次";
			base.Load += new global::System.EventHandler(this.ForChangCi_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x0400001F RID: 31
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000020 RID: 32
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x04000021 RID: 33
		private global::System.Windows.Forms.Button BtnChangCiManager;

		// Token: 0x04000022 RID: 34
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000023 RID: 35
		private global::System.Windows.Forms.ComboBox CmbDaiLi;

		// Token: 0x04000024 RID: 36
		private global::System.Windows.Forms.Panel panel2;

		// Token: 0x04000025 RID: 37
		private global::System.Windows.Forms.Panel PanContent;

		// Token: 0x04000026 RID: 38
		private global::System.Windows.Forms.Panel panel3;

		// Token: 0x04000027 RID: 39
		private global::System.Windows.Forms.Label LblDaiLiFeiLv;

		// Token: 0x04000028 RID: 40
		private global::System.Windows.Forms.Label LblDate;

		// Token: 0x04000029 RID: 41
		private global::System.Windows.Forms.Label LblName;

		// Token: 0x0400002A RID: 42
		private global::System.Windows.Forms.Panel panel5;

		// Token: 0x0400002B RID: 43
		private global::System.Windows.Forms.Label label8;

		// Token: 0x0400002C RID: 44
		private global::System.Windows.Forms.Label label7;

		// Token: 0x0400002D RID: 45
		private global::System.Windows.Forms.Label label6;

		// Token: 0x0400002E RID: 46
		private global::System.Windows.Forms.Label label5;

		// Token: 0x0400002F RID: 47
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04000030 RID: 48
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04000031 RID: 49
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000032 RID: 50
		private global::System.Windows.Forms.Button BtnJieTu;

		// Token: 0x04000033 RID: 51
		private global::System.Windows.Forms.CheckBox CheQuanBuChangCi;
	}
}
