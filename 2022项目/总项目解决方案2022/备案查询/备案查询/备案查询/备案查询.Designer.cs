namespace 备案查询
{
	// Token: 0x02000003 RID: 3
	public partial class 备案查询 : global::System.Windows.Forms.Form
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002590 File Offset: 0x00000790
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000025C8 File Offset: 0x000007C8
		private void InitializeComponent()
		{
			this.button1 = new global::System.Windows.Forms.Button();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.button2 = new global::System.Windows.Forms.Button();
			this.button3 = new global::System.Windows.Forms.Button();
			this.textBox2 = new global::System.Windows.Forms.TextBox();
			this.openFileDialog1 = new global::System.Windows.Forms.OpenFileDialog();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			this.button1.Location = new global::System.Drawing.Point(404, 21);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "选择文本";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.textBox1.Location = new global::System.Drawing.Point(65, 21);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new global::System.Drawing.Size(333, 21);
			this.textBox1.TabIndex = 1;
			this.button2.Location = new global::System.Drawing.Point(12, 63);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(79, 28);
			this.button2.TabIndex = 2;
			this.button2.Text = "开始查询";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.button2_Click);
			this.button3.Location = new global::System.Drawing.Point(108, 63);
			this.button3.Name = "button3";
			this.button3.Size = new global::System.Drawing.Size(79, 28);
			this.button3.TabIndex = 3;
			this.button3.Text = "停止";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new global::System.EventHandler(this.button3_Click);
			this.textBox2.Dock = global::System.Windows.Forms.DockStyle.Bottom;
			this.textBox2.Font = new global::System.Drawing.Font("宋体", 10f);
			this.textBox2.Location = new global::System.Drawing.Point(0, 136);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this.textBox2.Size = new global::System.Drawing.Size(491, 304);
			this.textBox2.TabIndex = 4;
			this.openFileDialog1.FileName = "openFileDialog1";
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(12, 106);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(41, 12);
			this.label1.TabIndex = 5;
			this.label1.Text = "未开始";
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(10, 24);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(47, 12);
			this.label2.TabIndex = 6;
			this.label2.Text = "导入TXT";
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(491, 440);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.textBox2);
			base.Controls.Add(this.button3);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.button1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.Name = "备案查询";
			base.ShowIcon = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "备案查询";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.备案查询_FormClosing);
			base.Load += new global::System.EventHandler(this.备案查询_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000003 RID: 3
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000004 RID: 4
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04000005 RID: 5
		private global::System.Windows.Forms.TextBox textBox1;

		// Token: 0x04000006 RID: 6
		private global::System.Windows.Forms.Button button2;

		// Token: 0x04000007 RID: 7
		private global::System.Windows.Forms.Button button3;

		// Token: 0x04000008 RID: 8
		private global::System.Windows.Forms.TextBox textBox2;

		// Token: 0x04000009 RID: 9
		private global::System.Windows.Forms.OpenFileDialog openFileDialog1;

		// Token: 0x0400000A RID: 10
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400000B RID: 11
		private global::System.Windows.Forms.Label label2;
	}
}
