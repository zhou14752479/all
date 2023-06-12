namespace Background
{
	// Token: 0x02000015 RID: 21
	public partial class 激活码生成 : global::System.Windows.Forms.Form
	{
		// Token: 0x060000CB RID: 203 RVA: 0x0000E400 File Offset: 0x0000C600
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000E438 File Offset: 0x0000C638
		private void InitializeComponent()
		{
            this.BtnActive = new System.Windows.Forms.Button();
            this.TxtActivecode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtMachineID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnActive
            // 
            this.BtnActive.Location = new System.Drawing.Point(123, 161);
            this.BtnActive.Name = "BtnActive";
            this.BtnActive.Size = new System.Drawing.Size(285, 29);
            this.BtnActive.TabIndex = 7;
            this.BtnActive.Text = "点击生成激活码";
            this.BtnActive.UseVisualStyleBackColor = true;
            this.BtnActive.Click += new System.EventHandler(this.BtnActive_Click);
            // 
            // TxtActivecode
            // 
            this.TxtActivecode.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.TxtActivecode.Location = new System.Drawing.Point(123, 74);
            this.TxtActivecode.Multiline = true;
            this.TxtActivecode.Name = "TxtActivecode";
            this.TxtActivecode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TxtActivecode.Size = new System.Drawing.Size(285, 64);
            this.TxtActivecode.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "激活码";
            // 
            // TxtMachineID
            // 
            this.TxtMachineID.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.TxtMachineID.Location = new System.Drawing.Point(123, 26);
            this.TxtMachineID.Name = "TxtMachineID";
            this.TxtMachineID.Size = new System.Drawing.Size(285, 25);
            this.TxtMachineID.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "机器识别码";
            // 
            // 激活码生成
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 202);
            this.Controls.Add(this.BtnActive);
            this.Controls.Add(this.TxtActivecode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TxtMachineID);
            this.Controls.Add(this.label1);
            this.Name = "激活码生成";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "(仅供学习交流，如作他用所承受的法律责任一概与作者无关(使用即代表你同意上述观点))";
            this.Load += new System.EventHandler(this.激活码生成_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		// Token: 0x040000DB RID: 219
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x040000DC RID: 220
		private global::System.Windows.Forms.Button BtnActive;

		// Token: 0x040000DD RID: 221
		private global::System.Windows.Forms.TextBox TxtActivecode;

		// Token: 0x040000DE RID: 222
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040000DF RID: 223
		private global::System.Windows.Forms.TextBox TxtMachineID;

		// Token: 0x040000E0 RID: 224
		private global::System.Windows.Forms.Label label1;
	}
}
