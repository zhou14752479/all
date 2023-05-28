namespace Background
{
	// Token: 0x02000006 RID: 6
	public partial class ForDaiLi : global::MUWEN.ForParent
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00006968 File Offset: 0x00004B68
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

		// Token: 0x0600004D RID: 77 RVA: 0x000069A4 File Offset: 0x00004BA4
		private void InitializeComponent()
		{
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.LstData = new global::System.Windows.Forms.ListBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.TxtName = new global::System.Windows.Forms.TextBox();
			this.TxtDaiLiFei = new global::System.Windows.Forms.TextBox();
			this.BtnSave = new global::System.Windows.Forms.Button();
			this.BtnDelete = new global::System.Windows.Forms.Button();
			this.LblMessage = new global::MUWEN.LabelMessage();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Controls.Add(this.LstData);
			this.groupBox1.Location = new global::System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new global::System.Drawing.Size(200, 426);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "代理人";
			this.LstData.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this.LstData.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.LstData.FormattingEnabled = true;
			this.LstData.ItemHeight = 21;
			this.LstData.Location = new global::System.Drawing.Point(3, 25);
			this.LstData.Name = "LstData";
			this.LstData.Size = new global::System.Drawing.Size(194, 398);
			this.LstData.TabIndex = 0;
			this.LstData.SelectedIndexChanged += new global::System.EventHandler(this.LstData_SelectedIndexChanged);
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(253, 91);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(42, 21);
			this.label1.TabIndex = 1;
			this.label1.Text = "名称";
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(237, 174);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(58, 21);
			this.label2.TabIndex = 1;
			this.label2.Text = "代理费";
			this.TxtName.Location = new global::System.Drawing.Point(301, 88);
			this.TxtName.Name = "TxtName";
			this.TxtName.Size = new global::System.Drawing.Size(187, 29);
			this.TxtName.TabIndex = 2;
			this.TxtDaiLiFei.Location = new global::System.Drawing.Point(301, 171);
			this.TxtDaiLiFei.Name = "TxtDaiLiFei";
			this.TxtDaiLiFei.Size = new global::System.Drawing.Size(187, 29);
			this.TxtDaiLiFei.TabIndex = 2;
			this.BtnSave.Location = new global::System.Drawing.Point(272, 348);
			this.BtnSave.Name = "BtnSave";
			this.BtnSave.Size = new global::System.Drawing.Size(105, 34);
			this.BtnSave.TabIndex = 3;
			this.BtnSave.Text = "保存";
			this.BtnSave.UseVisualStyleBackColor = true;
			this.BtnSave.Click += new global::System.EventHandler(this.BtnSave_Click);
			this.BtnDelete.Enabled = false;
			this.BtnDelete.Location = new global::System.Drawing.Point(383, 348);
			this.BtnDelete.Name = "BtnDelete";
			this.BtnDelete.Size = new global::System.Drawing.Size(105, 34);
			this.BtnDelete.TabIndex = 3;
			this.BtnDelete.Text = "删除";
			this.BtnDelete.UseVisualStyleBackColor = true;
			this.BtnDelete.Click += new global::System.EventHandler(this.BtnDelete_Click);
			this.LblMessage.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.LblMessage.Location = new global::System.Drawing.Point(218, 398);
			this.LblMessage.Name = "LblMessage";
			this.LblMessage.Size = new global::System.Drawing.Size(270, 34);
			this.LblMessage.TabIndex = 4;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(10f, 21f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(513, 444);
			base.Controls.Add(this.LblMessage);
			base.Controls.Add(this.BtnDelete);
			base.Controls.Add(this.BtnSave);
			base.Controls.Add(this.TxtDaiLiFei);
			base.Controls.Add(this.TxtName);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.groupBox1);
			base.Name = "ForDaiLi";
			this.Text = "代理人管理";
			base.Load += new global::System.EventHandler(this.ForDaiLi_Load);
			this.groupBox1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400004F RID: 79
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000050 RID: 80
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04000051 RID: 81
		private global::System.Windows.Forms.ListBox LstData;

		// Token: 0x04000052 RID: 82
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000053 RID: 83
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000054 RID: 84
		private global::System.Windows.Forms.TextBox TxtName;

		// Token: 0x04000055 RID: 85
		private global::System.Windows.Forms.TextBox TxtDaiLiFei;

		// Token: 0x04000056 RID: 86
		private global::System.Windows.Forms.Button BtnSave;

		// Token: 0x04000057 RID: 87
		private global::System.Windows.Forms.Button BtnDelete;

		// Token: 0x04000058 RID: 88
		private global::MUWEN.LabelMessage LblMessage;
	}
}
