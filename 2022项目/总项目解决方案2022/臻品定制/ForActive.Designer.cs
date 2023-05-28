namespace Background
{
	// Token: 0x02000002 RID: 2
	public partial class ForActive : global::MUWEN.ForParent
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002218 File Offset: 0x00000418
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

		// Token: 0x06000008 RID: 8 RVA: 0x00002254 File Offset: 0x00000454
		private void InitializeComponent()
		{
			this.label1 = new global::System.Windows.Forms.Label();
			this.TxtMachineID = new global::System.Windows.Forms.TextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.TxtActivecode = new global::System.Windows.Forms.TextBox();
			this.BtnActive = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(24, 27);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(90, 21);
			this.label1.TabIndex = 0;
			this.label1.Text = "本机识别码";
			this.TxtMachineID.Font = new global::System.Drawing.Font("微软雅黑", 10f);
			this.TxtMachineID.Location = new global::System.Drawing.Point(120, 24);
			this.TxtMachineID.Name = "TxtMachineID";
			this.TxtMachineID.ReadOnly = true;
			this.TxtMachineID.Size = new global::System.Drawing.Size(285, 25);
			this.TxtMachineID.TabIndex = 1;
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(56, 75);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(58, 21);
			this.label2.TabIndex = 0;
			this.label2.Text = "激活码";
			this.TxtActivecode.Font = new global::System.Drawing.Font("微软雅黑", 10f);
			this.TxtActivecode.Location = new global::System.Drawing.Point(120, 72);
			this.TxtActivecode.Multiline = true;
			this.TxtActivecode.Name = "TxtActivecode";
			this.TxtActivecode.ScrollBars = global::System.Windows.Forms.ScrollBars.Both;
			this.TxtActivecode.Size = new global::System.Drawing.Size(285, 64);
			this.TxtActivecode.TabIndex = 1;
			this.BtnActive.Location = new global::System.Drawing.Point(120, 156);
			this.BtnActive.Name = "BtnActive";
			this.BtnActive.Size = new global::System.Drawing.Size(285, 29);
			this.BtnActive.TabIndex = 2;
			this.BtnActive.Text = "激活程序";
			this.BtnActive.UseVisualStyleBackColor = true;
			this.BtnActive.Click += new global::System.EventHandler(this.BtnActive_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(10f, 21f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(429, 213);
			base.Controls.Add(this.BtnActive);
			base.Controls.Add(this.TxtActivecode);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.TxtMachineID);
			base.Controls.Add(this.label1);
			base.Name = "ForActive";
			this.Text = "激活";
			base.Load += new global::System.EventHandler(this.ForActive_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000002 RID: 2
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000003 RID: 3
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000004 RID: 4
		private global::System.Windows.Forms.TextBox TxtMachineID;

		// Token: 0x04000005 RID: 5
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000006 RID: 6
		private global::System.Windows.Forms.TextBox TxtActivecode;

		// Token: 0x04000007 RID: 7
		private global::System.Windows.Forms.Button BtnActive;
	}
}
