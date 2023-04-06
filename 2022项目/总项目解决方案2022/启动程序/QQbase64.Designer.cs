namespace 启动程序
{
	// Token: 0x02000004 RID: 4
	public partial class QQbase64 : global::System.Windows.Forms.Form
	{
		// Token: 0x06000013 RID: 19 RVA: 0x00003390 File Offset: 0x00001590
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000033C8 File Offset: 0x000015C8
		private void InitializeComponent()
		{
			this.button1 = new global::System.Windows.Forms.Button();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.textBox2 = new global::System.Windows.Forms.TextBox();
			base.SuspendLayout();
			this.button1.Location = new global::System.Drawing.Point(74, 62);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "点击提取";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.textBox1.Font = new global::System.Drawing.Font("宋体", 10f);
			this.textBox1.Location = new global::System.Drawing.Point(74, 6);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new global::System.Drawing.Size(709, 50);
			this.textBox1.TabIndex = 1;
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(3, 9);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(65, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "输入链接：";
			this.textBox2.Location = new global::System.Drawing.Point(74, 91);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new global::System.Drawing.Size(222, 21);
			this.textBox2.TabIndex = 3;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(786, 120);
			base.Controls.Add(this.textBox2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.button1);
			base.Name = "QQbase64";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "QQbase64";
			base.Load += new global::System.EventHandler(this.QQbase64_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400001E RID: 30
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x0400001F RID: 31
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04000020 RID: 32
		private global::System.Windows.Forms.TextBox textBox1;

		// Token: 0x04000021 RID: 33
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000022 RID: 34
		private global::System.Windows.Forms.TextBox textBox2;
	}
}
