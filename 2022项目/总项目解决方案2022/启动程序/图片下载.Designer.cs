namespace 启动程序
{
	// Token: 0x0200000B RID: 11
	public partial class 图片下载 : global::System.Windows.Forms.Form
	{
		// Token: 0x06000057 RID: 87 RVA: 0x0000D158 File Offset: 0x0000B358
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000D190 File Offset: 0x0000B390
		private void InitializeComponent()
		{
			this.button1 = new global::System.Windows.Forms.Button();
			this.button2 = new global::System.Windows.Forms.Button();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			base.SuspendLayout();
			this.button1.Location = new global::System.Drawing.Point(31, 245);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(87, 34);
			this.button1.TabIndex = 0;
			this.button1.Text = "开始下载";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.button2.Location = new global::System.Drawing.Point(206, 245);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(75, 34);
			this.button2.TabIndex = 1;
			this.button2.Text = "停止下载";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.button2_Click);
			this.textBox1.Dock = global::System.Windows.Forms.DockStyle.Top;
			this.textBox1.Location = new global::System.Drawing.Point(0, 0);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new global::System.Drawing.Size(349, 222);
			this.textBox1.TabIndex = 2;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(349, 300);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.button1);
			base.Name = "图片下载";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "图片下载";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.图片下载_FormClosing);
			base.Load += new global::System.EventHandler(this.图片下载_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000116 RID: 278
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000117 RID: 279
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04000118 RID: 280
		private global::System.Windows.Forms.Button button2;

		// Token: 0x04000119 RID: 281
		private global::System.Windows.Forms.TextBox textBox1;
	}
}
