namespace Background
{
	// Token: 0x02000003 RID: 3
	public partial class ForAnimal : global::MUWEN.ForParent
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00002C14 File Offset: 0x00000E14
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

		// Token: 0x0600001A RID: 26 RVA: 0x00002C50 File Offset: 0x00000E50
		private void InitializeComponent()
		{
			this.SplSplit = new global::System.Windows.Forms.SplitContainer();
			this.LstData = new global::System.Windows.Forms.ListBox();
			this.TxtWeiKaiTianShuPianYi = new global::System.Windows.Forms.TextBox();
			this.label5 = new global::System.Windows.Forms.Label();
			this.TxtWeiKaiChangShuPianYi = new global::System.Windows.Forms.TextBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.BtnClearNames = new global::System.Windows.Forms.Button();
			this.BtnDelete = new global::System.Windows.Forms.Button();
			this.BtnSave = new global::System.Windows.Forms.Button();
			this.TxtAnimalName = new global::System.Windows.Forms.TextBox();
			this.TxtBeiLv = new global::System.Windows.Forms.TextBox();
			this.BtnAddName = new global::System.Windows.Forms.Button();
			this.TxtName = new global::System.Windows.Forms.TextBox();
			this.TxtNames = new global::System.Windows.Forms.TextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.label1 = new global::System.Windows.Forms.Label();
			this.LblMessage = new global::MUWEN.LabelMessage();
			((global::System.ComponentModel.ISupportInitialize)this.SplSplit).BeginInit();
			this.SplSplit.Panel1.SuspendLayout();
			this.SplSplit.Panel2.SuspendLayout();
			this.SplSplit.SuspendLayout();
			base.SuspendLayout();
			this.SplSplit.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.SplSplit.Location = new global::System.Drawing.Point(0, 0);
			this.SplSplit.Margin = new global::System.Windows.Forms.Padding(2);
			this.SplSplit.Name = "SplSplit";
			this.SplSplit.Panel1.Controls.Add(this.LstData);
			this.SplSplit.Panel2.Controls.Add(this.TxtWeiKaiTianShuPianYi);
			this.SplSplit.Panel2.Controls.Add(this.label5);
			this.SplSplit.Panel2.Controls.Add(this.TxtWeiKaiChangShuPianYi);
			this.SplSplit.Panel2.Controls.Add(this.label4);
			this.SplSplit.Panel2.Controls.Add(this.BtnClearNames);
			this.SplSplit.Panel2.Controls.Add(this.BtnDelete);
			this.SplSplit.Panel2.Controls.Add(this.BtnSave);
			this.SplSplit.Panel2.Controls.Add(this.TxtAnimalName);
			this.SplSplit.Panel2.Controls.Add(this.TxtBeiLv);
			this.SplSplit.Panel2.Controls.Add(this.BtnAddName);
			this.SplSplit.Panel2.Controls.Add(this.TxtName);
			this.SplSplit.Panel2.Controls.Add(this.TxtNames);
			this.SplSplit.Panel2.Controls.Add(this.label2);
			this.SplSplit.Panel2.Controls.Add(this.label3);
			this.SplSplit.Panel2.Controls.Add(this.label1);
			this.SplSplit.Panel2.Controls.Add(this.LblMessage);
			this.SplSplit.Size = new global::System.Drawing.Size(658, 475);
			this.SplSplit.SplitterDistance = 175;
			this.SplSplit.SplitterWidth = 3;
			this.SplSplit.TabIndex = 0;
			this.LstData.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.LstData.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.LstData.FormattingEnabled = true;
			this.LstData.ItemHeight = 21;
			this.LstData.Location = new global::System.Drawing.Point(0, 0);
			this.LstData.Margin = new global::System.Windows.Forms.Padding(2);
			this.LstData.Name = "LstData";
			this.LstData.Size = new global::System.Drawing.Size(175, 475);
			this.LstData.TabIndex = 0;
			this.LstData.SelectedIndexChanged += new global::System.EventHandler(this.LstData_SelectedIndexChanged);
			this.TxtWeiKaiTianShuPianYi.Location = new global::System.Drawing.Point(367, 317);
			this.TxtWeiKaiTianShuPianYi.Name = "TxtWeiKaiTianShuPianYi";
			this.TxtWeiKaiTianShuPianYi.Size = new global::System.Drawing.Size(100, 29);
			this.TxtWeiKaiTianShuPianYi.TabIndex = 11;
			this.label5.AutoSize = true;
			this.label5.Location = new global::System.Drawing.Point(239, 320);
			this.label5.Name = "label5";
			this.label5.Size = new global::System.Drawing.Size(122, 21);
			this.label5.TabIndex = 10;
			this.label5.Text = "未开天数偏移量";
			this.TxtWeiKaiChangShuPianYi.Location = new global::System.Drawing.Point(133, 317);
			this.TxtWeiKaiChangShuPianYi.Name = "TxtWeiKaiChangShuPianYi";
			this.TxtWeiKaiChangShuPianYi.Size = new global::System.Drawing.Size(100, 29);
			this.TxtWeiKaiChangShuPianYi.TabIndex = 11;
			this.label4.AutoSize = true;
			this.label4.Location = new global::System.Drawing.Point(5, 320);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(122, 21);
			this.label4.TabIndex = 10;
			this.label4.Text = "未开场数偏移量";
			this.BtnClearNames.Location = new global::System.Drawing.Point(364, 205);
			this.BtnClearNames.Name = "BtnClearNames";
			this.BtnClearNames.Size = new global::System.Drawing.Size(103, 29);
			this.BtnClearNames.TabIndex = 9;
			this.BtnClearNames.Text = "清空别名";
			this.BtnClearNames.UseVisualStyleBackColor = true;
			this.BtnClearNames.Click += new global::System.EventHandler(this.BtnClearNames_Click);
			this.BtnDelete.Enabled = false;
			this.BtnDelete.Location = new global::System.Drawing.Point(209, 424);
			this.BtnDelete.Name = "BtnDelete";
			this.BtnDelete.Size = new global::System.Drawing.Size(117, 39);
			this.BtnDelete.TabIndex = 8;
			this.BtnDelete.Text = "删除";
			this.BtnDelete.UseVisualStyleBackColor = true;
			this.BtnDelete.Click += new global::System.EventHandler(this.BtnDelete_Click);
			this.BtnSave.Location = new global::System.Drawing.Point(86, 424);
			this.BtnSave.Name = "BtnSave";
			this.BtnSave.Size = new global::System.Drawing.Size(117, 39);
			this.BtnSave.TabIndex = 8;
			this.BtnSave.Text = "保存";
			this.BtnSave.UseVisualStyleBackColor = true;
			this.BtnSave.Click += new global::System.EventHandler(this.BtnSave_Click);
			this.TxtAnimalName.Location = new global::System.Drawing.Point(86, 63);
			this.TxtAnimalName.Name = "TxtAnimalName";
			this.TxtAnimalName.Size = new global::System.Drawing.Size(381, 29);
			this.TxtAnimalName.TabIndex = 7;
			this.TxtBeiLv.Location = new global::System.Drawing.Point(86, 269);
			this.TxtBeiLv.Name = "TxtBeiLv";
			this.TxtBeiLv.Size = new global::System.Drawing.Size(381, 29);
			this.TxtBeiLv.TabIndex = 6;
			this.BtnAddName.Location = new global::System.Drawing.Point(255, 205);
			this.BtnAddName.Name = "BtnAddName";
			this.BtnAddName.Size = new global::System.Drawing.Size(103, 29);
			this.BtnAddName.TabIndex = 5;
			this.BtnAddName.Text = "添加";
			this.BtnAddName.UseVisualStyleBackColor = true;
			this.BtnAddName.Click += new global::System.EventHandler(this.BtnAddName_Click);
			this.TxtName.Location = new global::System.Drawing.Point(86, 206);
			this.TxtName.Name = "TxtName";
			this.TxtName.Size = new global::System.Drawing.Size(163, 29);
			this.TxtName.TabIndex = 4;
			this.TxtNames.Location = new global::System.Drawing.Point(86, 98);
			this.TxtNames.Multiline = true;
			this.TxtNames.Name = "TxtNames";
			this.TxtNames.ReadOnly = true;
			this.TxtNames.Size = new global::System.Drawing.Size(381, 102);
			this.TxtNames.TabIndex = 3;
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(38, 272);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(42, 21);
			this.label2.TabIndex = 2;
			this.label2.Text = "倍率";
			this.label3.AutoSize = true;
			this.label3.Location = new global::System.Drawing.Point(22, 66);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(58, 21);
			this.label3.TabIndex = 2;
			this.label3.Text = "动物名";
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(38, 101);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(42, 21);
			this.label1.TabIndex = 2;
			this.label1.Text = "别名";
			this.LblMessage.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.LblMessage.Location = new global::System.Drawing.Point(16, 382);
			this.LblMessage.Name = "LblMessage";
			this.LblMessage.Size = new global::System.Drawing.Size(456, 36);
			this.LblMessage.TabIndex = 1;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(10f, 21f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(658, 475);
			base.Controls.Add(this.SplSplit);
			base.Margin = new global::System.Windows.Forms.Padding(3, 4, 3, 4);
			base.Name = "ForAnimal";
			this.Text = "动物名管理";
			base.Load += new global::System.EventHandler(this.ForModel_Load);
			this.SplSplit.Panel1.ResumeLayout(false);
			this.SplSplit.Panel2.ResumeLayout(false);
			this.SplSplit.Panel2.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.SplSplit).EndInit();
			this.SplSplit.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x0400000A RID: 10
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x0400000B RID: 11
		private global::System.Windows.Forms.SplitContainer SplSplit;

		// Token: 0x0400000C RID: 12
		private global::System.Windows.Forms.ListBox LstData;

		// Token: 0x0400000D RID: 13
		private global::MUWEN.LabelMessage LblMessage;

		// Token: 0x0400000E RID: 14
		private global::System.Windows.Forms.TextBox TxtBeiLv;

		// Token: 0x0400000F RID: 15
		private global::System.Windows.Forms.Button BtnAddName;

		// Token: 0x04000010 RID: 16
		private global::System.Windows.Forms.TextBox TxtName;

		// Token: 0x04000011 RID: 17
		private global::System.Windows.Forms.TextBox TxtNames;

		// Token: 0x04000012 RID: 18
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000013 RID: 19
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000014 RID: 20
		private global::System.Windows.Forms.TextBox TxtAnimalName;

		// Token: 0x04000015 RID: 21
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04000016 RID: 22
		private global::System.Windows.Forms.Button BtnDelete;

		// Token: 0x04000017 RID: 23
		private global::System.Windows.Forms.Button BtnSave;

		// Token: 0x04000018 RID: 24
		private global::System.Windows.Forms.Button BtnClearNames;

		// Token: 0x04000019 RID: 25
		private global::System.Windows.Forms.TextBox TxtWeiKaiTianShuPianYi;

		// Token: 0x0400001A RID: 26
		private global::System.Windows.Forms.Label label5;

		// Token: 0x0400001B RID: 27
		private global::System.Windows.Forms.TextBox TxtWeiKaiChangShuPianYi;

		// Token: 0x0400001C RID: 28
		private global::System.Windows.Forms.Label label4;
	}
}
