namespace 启动程序
{
	// Token: 0x02000010 RID: 16
	public partial class 抢单软件 : global::System.Windows.Forms.Form
	{
		// Token: 0x0600009F RID: 159 RVA: 0x00014BD8 File Offset: 0x00012DD8
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00014C10 File Offset: 0x00012E10
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			this.splitContainer1 = new global::System.Windows.Forms.SplitContainer();
			this.textBox5 = new global::System.Windows.Forms.TextBox();
			this.label5 = new global::System.Windows.Forms.Label();
			this.label4 = new global::System.Windows.Forms.Label();
			this.button2 = new global::System.Windows.Forms.Button();
			this.label3 = new global::System.Windows.Forms.Label();
			this.textBox4 = new global::System.Windows.Forms.TextBox();
			this.button1 = new global::System.Windows.Forms.Button();
			this.textBox2 = new global::System.Windows.Forms.TextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.textBox3 = new global::System.Windows.Forms.TextBox();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			base.SuspendLayout();
			this.splitContainer1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new global::System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = global::System.Windows.Forms.Orientation.Horizontal;
			this.splitContainer1.Panel1.Controls.Add(this.textBox5);
			this.splitContainer1.Panel1.Controls.Add(this.label5);
			this.splitContainer1.Panel1.Controls.Add(this.label4);
			this.splitContainer1.Panel1.Controls.Add(this.button2);
			this.splitContainer1.Panel1.Controls.Add(this.label3);
			this.splitContainer1.Panel1.Controls.Add(this.textBox4);
			this.splitContainer1.Panel1.Controls.Add(this.button1);
			this.splitContainer1.Panel1.Controls.Add(this.textBox2);
			this.splitContainer1.Panel1.Controls.Add(this.label2);
			this.splitContainer1.Panel1.Controls.Add(this.textBox1);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel2.Controls.Add(this.textBox3);
			this.splitContainer1.Size = new global::System.Drawing.Size(375, 598);
			this.splitContainer1.SplitterDistance = 246;
			this.splitContainer1.TabIndex = 0;
			this.textBox5.Font = new global::System.Drawing.Font("宋体", 10f);
			this.textBox5.Location = new global::System.Drawing.Point(77, 140);
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new global::System.Drawing.Size(119, 23);
			this.textBox5.TabIndex = 10;
			this.textBox5.Text = "10";
			this.label5.AutoSize = true;
			this.label5.Font = new global::System.Drawing.Font("宋体", 10f);
			this.label5.Location = new global::System.Drawing.Point(24, 143);
			this.label5.Name = "label5";
			this.label5.Size = new global::System.Drawing.Size(35, 14);
			this.label5.TabIndex = 9;
			this.label5.Text = "间隔";
			this.label4.AutoSize = true;
			this.label4.Location = new global::System.Drawing.Point(75, 217);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(0, 12);
			this.label4.TabIndex = 8;
			this.button2.Location = new global::System.Drawing.Point(214, 169);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(119, 31);
			this.button2.TabIndex = 7;
			this.button2.Text = "停止执行";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.Button2_Click);
			this.label3.AutoSize = true;
			this.label3.Font = new global::System.Drawing.Font("宋体", 10f);
			this.label3.Location = new global::System.Drawing.Point(8, 96);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(49, 14);
			this.label3.TabIndex = 6;
			this.label3.Text = "cookie";
			this.textBox4.Font = new global::System.Drawing.Font("宋体", 10f);
			this.textBox4.Location = new global::System.Drawing.Point(77, 93);
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new global::System.Drawing.Size(256, 23);
			this.textBox4.TabIndex = 5;
			this.button1.Location = new global::System.Drawing.Point(77, 169);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(119, 31);
			this.button1.TabIndex = 4;
			this.button1.Text = "开始执行";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.Button1_Click);
			this.textBox2.Font = new global::System.Drawing.Font("宋体", 10f);
			this.textBox2.Location = new global::System.Drawing.Point(77, 53);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new global::System.Drawing.Size(119, 23);
			this.textBox2.TabIndex = 3;
			this.textBox2.Text = "Lll12345678,";
			this.label2.AutoSize = true;
			this.label2.Font = new global::System.Drawing.Font("宋体", 10f);
			this.label2.Location = new global::System.Drawing.Point(8, 56);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(63, 14);
			this.label2.TabIndex = 2;
			this.label2.Text = "password";
			this.textBox1.Font = new global::System.Drawing.Font("宋体", 10f);
			this.textBox1.Location = new global::System.Drawing.Point(77, 24);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new global::System.Drawing.Size(119, 23);
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = "6021910031050";
			this.label1.AutoSize = true;
			this.label1.Font = new global::System.Drawing.Font("宋体", 10f);
			this.label1.Location = new global::System.Drawing.Point(38, 29);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(21, 14);
			this.label1.TabIndex = 0;
			this.label1.Text = "id";
			this.textBox3.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.textBox3.Font = new global::System.Drawing.Font("宋体", 9f);
			this.textBox3.Location = new global::System.Drawing.Point(0, 0);
			this.textBox3.Multiline = true;
			this.textBox3.Name = "textBox3";
			this.textBox3.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this.textBox3.Size = new global::System.Drawing.Size(375, 348);
			this.textBox3.TabIndex = 2;
			this.timer1.Interval = 10;
			this.timer1.Tick += new global::System.EventHandler(this.Timer1_Tick);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(375, 598);
			base.Controls.Add(this.splitContainer1);
			this.Font = new global::System.Drawing.Font("宋体", 9f);
			base.Name = "抢单软件";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "抢单软件";
			base.Load += new global::System.EventHandler(this.抢单软件_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x0400019F RID: 415
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x040001A0 RID: 416
		private global::System.Windows.Forms.SplitContainer splitContainer1;

		// Token: 0x040001A1 RID: 417
		private global::System.Windows.Forms.TextBox textBox2;

		// Token: 0x040001A2 RID: 418
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040001A3 RID: 419
		private global::System.Windows.Forms.TextBox textBox1;

		// Token: 0x040001A4 RID: 420
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040001A5 RID: 421
		private global::System.Windows.Forms.Button button1;

		// Token: 0x040001A6 RID: 422
		private global::System.Windows.Forms.TextBox textBox3;

		// Token: 0x040001A7 RID: 423
		private global::System.Windows.Forms.TextBox textBox4;

		// Token: 0x040001A8 RID: 424
		private global::System.Windows.Forms.Label label3;

		// Token: 0x040001A9 RID: 425
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x040001AA RID: 426
		private global::System.Windows.Forms.Button button2;

		// Token: 0x040001AB RID: 427
		private global::System.Windows.Forms.Label label4;

		// Token: 0x040001AC RID: 428
		private global::System.Windows.Forms.TextBox textBox5;

		// Token: 0x040001AD RID: 429
		private global::System.Windows.Forms.Label label5;
	}
}
