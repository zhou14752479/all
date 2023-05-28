namespace Background
{
	// Token: 0x0200000C RID: 12
	public partial class ForMySetting : global::MUWEN.ForParent
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x0000BB50 File Offset: 0x00009D50
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

		// Token: 0x060000A5 RID: 165 RVA: 0x0000BB8C File Offset: 0x00009D8C
		private void InitializeComponent()
		{
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.TxtOwnerName = new global::System.Windows.Forms.TextBox();
			this.TxtJieSuanShiJian = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.BtnSave = new global::System.Windows.Forms.Button();
			this.LblMessage = new global::MUWEN.LabelMessage();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.TxtOwnerName);
			this.groupBox1.Controls.Add(this.TxtJieSuanShiJian);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new global::System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new global::System.Drawing.Size(493, 360);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(9, 105);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(90, 21);
			this.label2.TabIndex = 2;
			this.label2.Text = "使用者名称";
			this.TxtOwnerName.Location = new global::System.Drawing.Point(105, 102);
			this.TxtOwnerName.Name = "TxtOwnerName";
			this.TxtOwnerName.Size = new global::System.Drawing.Size(359, 29);
			this.TxtOwnerName.TabIndex = 1;
			this.TxtJieSuanShiJian.Location = new global::System.Drawing.Point(105, 58);
			this.TxtJieSuanShiJian.Name = "TxtJieSuanShiJian";
			this.TxtJieSuanShiJian.Size = new global::System.Drawing.Size(359, 29);
			this.TxtJieSuanShiJian.TabIndex = 0;
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(25, 61);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(74, 21);
			this.label1.TabIndex = 0;
			this.label1.Text = "结算时间";
			this.BtnSave.Location = new global::System.Drawing.Point(386, 378);
			this.BtnSave.Name = "BtnSave";
			this.BtnSave.Size = new global::System.Drawing.Size(119, 33);
			this.BtnSave.TabIndex = 1;
			this.BtnSave.Text = "保存";
			this.BtnSave.UseVisualStyleBackColor = true;
			this.BtnSave.Click += new global::System.EventHandler(this.BtnSave_Click);
			this.LblMessage.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.LblMessage.Location = new global::System.Drawing.Point(14, 378);
			this.LblMessage.Margin = new global::System.Windows.Forms.Padding(5);
			this.LblMessage.Name = "LblMessage";
			this.LblMessage.Size = new global::System.Drawing.Size(360, 33);
			this.LblMessage.TabIndex = 7;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(10f, 21f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(521, 425);
			base.Controls.Add(this.LblMessage);
			base.Controls.Add(this.BtnSave);
			base.Controls.Add(this.groupBox1);
			base.Name = "ForMySetting";
			this.Text = "参数配置";
			base.Load += new global::System.EventHandler(this.ForMySetting_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x040000AD RID: 173
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x040000AE RID: 174
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x040000AF RID: 175
		private global::System.Windows.Forms.TextBox TxtJieSuanShiJian;

		// Token: 0x040000B0 RID: 176
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040000B1 RID: 177
		private global::System.Windows.Forms.Button BtnSave;

		// Token: 0x040000B2 RID: 178
		private global::MUWEN.LabelMessage LblMessage;

		// Token: 0x040000B3 RID: 179
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040000B4 RID: 180
		private global::System.Windows.Forms.TextBox TxtOwnerName;
	}
}
