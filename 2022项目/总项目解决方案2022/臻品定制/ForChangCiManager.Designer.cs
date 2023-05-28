namespace Background
{
	// Token: 0x02000005 RID: 5
	public partial class ForChangCiManager : global::MUWEN.ForParent
	{
		// Token: 0x0600003E RID: 62 RVA: 0x000058D8 File Offset: 0x00003AD8
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

		// Token: 0x0600003F RID: 63 RVA: 0x00005914 File Offset: 0x00003B14
		private void InitializeComponent()
		{
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.LstData = new global::System.Windows.Forms.ListBox();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.BtnDeleteAll = new global::System.Windows.Forms.Button();
			this.LblMessage = new global::MUWEN.LabelMessage();
			this.BtnResetChangCi = new global::System.Windows.Forms.Button();
			this.BtnAddChangCi = new global::System.Windows.Forms.Button();
			this.BtnClear = new global::System.Windows.Forms.Button();
			this.BtnAddAnimal = new global::System.Windows.Forms.Button();
			this.TxtNames = new global::System.Windows.Forms.TextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.CmbAnimals = new global::System.Windows.Forms.ComboBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.groupBox3 = new global::System.Windows.Forms.GroupBox();
			this.DatChangCiTime = new global::System.Windows.Forms.DateTimePicker();
			this.label3 = new global::System.Windows.Forms.Label();
			this.label4 = new global::System.Windows.Forms.Label();
			this.TxtChangCiAnimals = new global::System.Windows.Forms.TextBox();
			this.label5 = new global::System.Windows.Forms.Label();
			this.BtnAddZiDingYiChangCi = new global::System.Windows.Forms.Button();
			this.BtnDeleteChangCi = new global::System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Controls.Add(this.LstData);
			this.groupBox1.Location = new global::System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new global::System.Drawing.Size(200, 399);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "场次";
			this.LstData.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.LstData.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.LstData.FormattingEnabled = true;
			this.LstData.ItemHeight = 21;
			this.LstData.Location = new global::System.Drawing.Point(3, 25);
			this.LstData.Name = "LstData";
			this.LstData.Size = new global::System.Drawing.Size(194, 371);
			this.LstData.TabIndex = 0;
			this.LstData.SelectedIndexChanged += new global::System.EventHandler(this.LstData_SelectedIndexChanged);
			this.groupBox2.Controls.Add(this.BtnDeleteChangCi);
			this.groupBox2.Controls.Add(this.BtnDeleteAll);
			this.groupBox2.Controls.Add(this.LblMessage);
			this.groupBox2.Controls.Add(this.BtnResetChangCi);
			this.groupBox2.Controls.Add(this.BtnAddChangCi);
			this.groupBox2.Controls.Add(this.BtnClear);
			this.groupBox2.Controls.Add(this.BtnAddAnimal);
			this.groupBox2.Controls.Add(this.TxtNames);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.CmbAnimals);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Location = new global::System.Drawing.Point(218, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new global::System.Drawing.Size(418, 399);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "编辑";
			this.BtnDeleteAll.Enabled = false;
			this.BtnDeleteAll.Location = new global::System.Drawing.Point(273, 322);
			this.BtnDeleteAll.Name = "BtnDeleteAll";
			this.BtnDeleteAll.Size = new global::System.Drawing.Size(129, 29);
			this.BtnDeleteAll.TabIndex = 7;
			this.BtnDeleteAll.Text = "全部清空";
			this.BtnDeleteAll.UseVisualStyleBackColor = true;
			this.BtnDeleteAll.Click += new global::System.EventHandler(this.BtnDeleteAll_Click);
			this.LblMessage.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.LblMessage.Location = new global::System.Drawing.Point(6, 357);
			this.LblMessage.Name = "LblMessage";
			this.LblMessage.Size = new global::System.Drawing.Size(406, 36);
			this.LblMessage.TabIndex = 6;
			this.BtnResetChangCi.Enabled = false;
			this.BtnResetChangCi.Location = new global::System.Drawing.Point(273, 287);
			this.BtnResetChangCi.Name = "BtnResetChangCi";
			this.BtnResetChangCi.Size = new global::System.Drawing.Size(129, 29);
			this.BtnResetChangCi.TabIndex = 5;
			this.BtnResetChangCi.Text = "重置场次";
			this.BtnResetChangCi.UseVisualStyleBackColor = true;
			this.BtnResetChangCi.Click += new global::System.EventHandler(this.BtnDeleteChangCi_Click);
			this.BtnAddChangCi.Enabled = false;
			this.BtnAddChangCi.Location = new global::System.Drawing.Point(120, 287);
			this.BtnAddChangCi.Name = "BtnAddChangCi";
			this.BtnAddChangCi.Size = new global::System.Drawing.Size(139, 29);
			this.BtnAddChangCi.TabIndex = 5;
			this.BtnAddChangCi.Text = "添加场次";
			this.BtnAddChangCi.UseVisualStyleBackColor = true;
			this.BtnAddChangCi.Click += new global::System.EventHandler(this.BtnAddChangCi_Click);
			this.BtnClear.Enabled = false;
			this.BtnClear.Location = new global::System.Drawing.Point(265, 133);
			this.BtnClear.Name = "BtnClear";
			this.BtnClear.Size = new global::System.Drawing.Size(137, 29);
			this.BtnClear.TabIndex = 4;
			this.BtnClear.Text = "清空开宝动物";
			this.BtnClear.UseVisualStyleBackColor = true;
			this.BtnClear.Click += new global::System.EventHandler(this.BtnClear_Click);
			this.BtnAddAnimal.Enabled = false;
			this.BtnAddAnimal.Location = new global::System.Drawing.Point(265, 186);
			this.BtnAddAnimal.Name = "BtnAddAnimal";
			this.BtnAddAnimal.Size = new global::System.Drawing.Size(137, 29);
			this.BtnAddAnimal.TabIndex = 4;
			this.BtnAddAnimal.Text = "添加到开宝动物";
			this.BtnAddAnimal.UseVisualStyleBackColor = true;
			this.BtnAddAnimal.Click += new global::System.EventHandler(this.BtnAddAnimal_Click);
			this.TxtNames.Location = new global::System.Drawing.Point(120, 61);
			this.TxtNames.Multiline = true;
			this.TxtNames.Name = "TxtNames";
			this.TxtNames.ReadOnly = true;
			this.TxtNames.ScrollBars = global::System.Windows.Forms.ScrollBars.Both;
			this.TxtNames.Size = new global::System.Drawing.Size(282, 66);
			this.TxtNames.TabIndex = 3;
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(40, 64);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(74, 21);
			this.label2.TabIndex = 2;
			this.label2.Text = "开宝动物";
			this.CmbAnimals.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CmbAnimals.FormattingEnabled = true;
			this.CmbAnimals.Location = new global::System.Drawing.Point(120, 186);
			this.CmbAnimals.Name = "CmbAnimals";
			this.CmbAnimals.Size = new global::System.Drawing.Size(139, 29);
			this.CmbAnimals.TabIndex = 1;
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(40, 189);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(74, 21);
			this.label1.TabIndex = 0;
			this.label1.Text = "动物列表";
			this.groupBox3.Controls.Add(this.BtnAddZiDingYiChangCi);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Controls.Add(this.TxtChangCiAnimals);
			this.groupBox3.Controls.Add(this.label4);
			this.groupBox3.Controls.Add(this.label3);
			this.groupBox3.Controls.Add(this.DatChangCiTime);
			this.groupBox3.Location = new global::System.Drawing.Point(642, 12);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new global::System.Drawing.Size(272, 399);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "自定义场次";
			this.DatChangCiTime.Location = new global::System.Drawing.Point(96, 98);
			this.DatChangCiTime.Name = "DatChangCiTime";
			this.DatChangCiTime.Size = new global::System.Drawing.Size(170, 29);
			this.DatChangCiTime.TabIndex = 0;
			this.label3.AutoSize = true;
			this.label3.Location = new global::System.Drawing.Point(16, 104);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(74, 21);
			this.label3.TabIndex = 1;
			this.label3.Text = "场次时间";
			this.label4.AutoSize = true;
			this.label4.Location = new global::System.Drawing.Point(16, 169);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(74, 21);
			this.label4.TabIndex = 2;
			this.label4.Text = "场次动物";
			this.TxtChangCiAnimals.Location = new global::System.Drawing.Point(96, 166);
			this.TxtChangCiAnimals.Name = "TxtChangCiAnimals";
			this.TxtChangCiAnimals.Size = new global::System.Drawing.Size(170, 29);
			this.TxtChangCiAnimals.TabIndex = 3;
			this.label5.AutoSize = true;
			this.label5.Font = new global::System.Drawing.Font("微软雅黑", 10f);
			this.label5.ForeColor = global::System.Drawing.SystemColors.AppWorkspace;
			this.label5.Location = new global::System.Drawing.Point(92, 198);
			this.label5.Name = "label5";
			this.label5.Size = new global::System.Drawing.Size(163, 20);
			this.label5.TabIndex = 4;
			this.label5.Text = "动物名之间使用空格隔开";
			this.BtnAddZiDingYiChangCi.Location = new global::System.Drawing.Point(6, 354);
			this.BtnAddZiDingYiChangCi.Name = "BtnAddZiDingYiChangCi";
			this.BtnAddZiDingYiChangCi.Size = new global::System.Drawing.Size(260, 39);
			this.BtnAddZiDingYiChangCi.TabIndex = 5;
			this.BtnAddZiDingYiChangCi.Text = "添加场次";
			this.BtnAddZiDingYiChangCi.UseVisualStyleBackColor = true;
			this.BtnAddZiDingYiChangCi.Click += new global::System.EventHandler(this.BtnAddZiDingYiChangCi_Click);
			this.BtnDeleteChangCi.Enabled = false;
			this.BtnDeleteChangCi.Location = new global::System.Drawing.Point(120, 322);
			this.BtnDeleteChangCi.Name = "BtnDeleteChangCi";
			this.BtnDeleteChangCi.Size = new global::System.Drawing.Size(139, 29);
			this.BtnDeleteChangCi.TabIndex = 5;
			this.BtnDeleteChangCi.Text = "删除选中场次";
			this.BtnDeleteChangCi.UseVisualStyleBackColor = true;
			this.BtnDeleteChangCi.Click += new global::System.EventHandler(this.BtnDeleteChangCi_Click_1);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(10f, 21f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(947, 423);
			base.Controls.Add(this.groupBox3);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.Name = "ForChangCiManager";
			this.Text = "场次管理";
			base.Load += new global::System.EventHandler(this.ForChangCiManager_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x04000037 RID: 55
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000038 RID: 56
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04000039 RID: 57
		private global::System.Windows.Forms.ListBox LstData;

		// Token: 0x0400003A RID: 58
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x0400003B RID: 59
		private global::System.Windows.Forms.Button BtnResetChangCi;

		// Token: 0x0400003C RID: 60
		private global::System.Windows.Forms.Button BtnAddChangCi;

		// Token: 0x0400003D RID: 61
		private global::System.Windows.Forms.Button BtnClear;

		// Token: 0x0400003E RID: 62
		private global::System.Windows.Forms.Button BtnAddAnimal;

		// Token: 0x0400003F RID: 63
		private global::System.Windows.Forms.TextBox TxtNames;

		// Token: 0x04000040 RID: 64
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000041 RID: 65
		private global::System.Windows.Forms.ComboBox CmbAnimals;

		// Token: 0x04000042 RID: 66
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000043 RID: 67
		private global::MUWEN.LabelMessage LblMessage;

		// Token: 0x04000044 RID: 68
		private global::System.Windows.Forms.Button BtnDeleteAll;

		// Token: 0x04000045 RID: 69
		private global::System.Windows.Forms.GroupBox groupBox3;

		// Token: 0x04000046 RID: 70
		private global::System.Windows.Forms.Button BtnDeleteChangCi;

		// Token: 0x04000047 RID: 71
		private global::System.Windows.Forms.Button BtnAddZiDingYiChangCi;

		// Token: 0x04000048 RID: 72
		private global::System.Windows.Forms.Label label5;

		// Token: 0x04000049 RID: 73
		private global::System.Windows.Forms.TextBox TxtChangCiAnimals;

		// Token: 0x0400004A RID: 74
		private global::System.Windows.Forms.Label label4;

		// Token: 0x0400004B RID: 75
		private global::System.Windows.Forms.Label label3;

		// Token: 0x0400004C RID: 76
		private global::System.Windows.Forms.DateTimePicker DatChangCiTime;
	}
}
