namespace 启动程序
{
	// Token: 0x0200000F RID: 15
	public partial class 微博转赞评 : global::System.Windows.Forms.Form
	{
		// Token: 0x06000095 RID: 149 RVA: 0x0001355C File Offset: 0x0001175C
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00013594 File Offset: 0x00011794
		private void InitializeComponent()
		{
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.textBox2 = new global::System.Windows.Forms.TextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.textBox5 = new global::System.Windows.Forms.TextBox();
			this.label6 = new global::System.Windows.Forms.Label();
			this.textBox3 = new global::System.Windows.Forms.TextBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.textBox4 = new global::System.Windows.Forms.TextBox();
			this.label5 = new global::System.Windows.Forms.Label();
			this.groupBox3 = new global::System.Windows.Forms.GroupBox();
			this.textBox6 = new global::System.Windows.Forms.TextBox();
			this.label7 = new global::System.Windows.Forms.Label();
			this.textBox7 = new global::System.Windows.Forms.TextBox();
			this.label8 = new global::System.Windows.Forms.Label();
			this.textBox8 = new global::System.Windows.Forms.TextBox();
			this.label9 = new global::System.Windows.Forms.Label();
			this.listView1 = new global::System.Windows.Forms.ListView();
			this.columnHeader1 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new global::System.Windows.Forms.ColumnHeader();
			this.button1 = new global::System.Windows.Forms.Button();
			this.button2 = new global::System.Windows.Forms.Button();
			this.button3 = new global::System.Windows.Forms.Button();
			this.listView2 = new global::System.Windows.Forms.ListView();
			this.columnHeader2 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader9 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader10 = new global::System.Windows.Forms.ColumnHeader();
			this.textBox9 = new global::System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Controls.Add(this.textBox2);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.textBox1);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new global::System.Drawing.Point(288, 9);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new global::System.Drawing.Size(520, 46);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "API设置";
			this.textBox2.Location = new global::System.Drawing.Point(260, 18);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new global::System.Drawing.Size(243, 21);
			this.textBox2.TabIndex = 3;
			this.textBox2.Text = "83885e9c12dde6e000f343e2166c436c";
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(207, 21);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(41, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "密钥：";
			this.textBox1.Location = new global::System.Drawing.Point(78, 18);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new global::System.Drawing.Size(100, 21);
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = "60397";
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(25, 21);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(47, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "Token：";
			this.label3.AutoSize = true;
			this.label3.Font = new global::System.Drawing.Font("宋体", 26.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label3.Location = new global::System.Drawing.Point(12, 7);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(159, 35);
			this.label3.TabIndex = 1;
			this.label3.Text = "微博设置";
			this.groupBox2.Controls.Add(this.textBox5);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.textBox3);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.textBox4);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Location = new global::System.Drawing.Point(12, 61);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new global::System.Drawing.Size(377, 46);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "机刷编号设置";
			this.textBox5.Location = new global::System.Drawing.Point(303, 18);
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new global::System.Drawing.Size(57, 21);
			this.textBox5.TabIndex = 5;
			this.textBox5.Text = "688479";
			this.label6.AutoSize = true;
			this.label6.Location = new global::System.Drawing.Point(256, 21);
			this.label6.Name = "label6";
			this.label6.Size = new global::System.Drawing.Size(41, 12);
			this.label6.TabIndex = 4;
			this.label6.Text = "评论：";
			this.textBox3.Location = new global::System.Drawing.Point(72, 18);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new global::System.Drawing.Size(57, 21);
			this.textBox3.TabIndex = 3;
			this.textBox3.Text = "772028";
			this.label4.AutoSize = true;
			this.label4.Location = new global::System.Drawing.Point(139, 21);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(41, 12);
			this.label4.TabIndex = 2;
			this.label4.Text = "点赞：";
			this.textBox4.Location = new global::System.Drawing.Point(186, 18);
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new global::System.Drawing.Size(57, 21);
			this.textBox4.TabIndex = 1;
			this.textBox4.Text = "688465";
			this.label5.AutoSize = true;
			this.label5.Location = new global::System.Drawing.Point(25, 21);
			this.label5.Name = "label5";
			this.label5.Size = new global::System.Drawing.Size(41, 12);
			this.label5.TabIndex = 0;
			this.label5.Text = "转发：";
			this.groupBox3.Controls.Add(this.textBox6);
			this.groupBox3.Controls.Add(this.label7);
			this.groupBox3.Controls.Add(this.textBox7);
			this.groupBox3.Controls.Add(this.label8);
			this.groupBox3.Controls.Add(this.textBox8);
			this.groupBox3.Controls.Add(this.label9);
			this.groupBox3.Location = new global::System.Drawing.Point(431, 61);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new global::System.Drawing.Size(377, 46);
			this.groupBox3.TabIndex = 3;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "人工编号设置";
			this.textBox6.Location = new global::System.Drawing.Point(72, 18);
			this.textBox6.Name = "textBox6";
			this.textBox6.Size = new global::System.Drawing.Size(57, 21);
			this.textBox6.TabIndex = 5;
			this.label7.AutoSize = true;
			this.label7.Location = new global::System.Drawing.Point(256, 21);
			this.label7.Name = "label7";
			this.label7.Size = new global::System.Drawing.Size(41, 12);
			this.label7.TabIndex = 4;
			this.label7.Text = "评论：";
			this.textBox7.Location = new global::System.Drawing.Point(186, 18);
			this.textBox7.Name = "textBox7";
			this.textBox7.Size = new global::System.Drawing.Size(57, 21);
			this.textBox7.TabIndex = 3;
			this.label8.AutoSize = true;
			this.label8.Location = new global::System.Drawing.Point(139, 21);
			this.label8.Name = "label8";
			this.label8.Size = new global::System.Drawing.Size(41, 12);
			this.label8.TabIndex = 2;
			this.label8.Text = "点赞：";
			this.textBox8.Location = new global::System.Drawing.Point(303, 18);
			this.textBox8.Name = "textBox8";
			this.textBox8.Size = new global::System.Drawing.Size(57, 21);
			this.textBox8.TabIndex = 1;
			this.label9.AutoSize = true;
			this.label9.Location = new global::System.Drawing.Point(25, 21);
			this.label9.Name = "label9";
			this.label9.Size = new global::System.Drawing.Size(41, 12);
			this.label9.TabIndex = 0;
			this.label9.Text = "转发：";
			this.listView1.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.columnHeader1,
				this.columnHeader3,
				this.columnHeader4,
				this.columnHeader5,
				this.columnHeader6,
				this.columnHeader7
			});
			this.listView1.Font = new global::System.Drawing.Font("宋体", 11f);
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new global::System.Drawing.Point(2, 113);
			this.listView1.Name = "listView1";
			this.listView1.Size = new global::System.Drawing.Size(833, 203);
			this.listView1.TabIndex = 38;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = global::System.Windows.Forms.View.Details;
			this.listView1.MouseEnter += new global::System.EventHandler(this.ListView1_MouseEnter);
			this.columnHeader1.Text = "序号";
			this.columnHeader3.Text = "备注名";
			this.columnHeader3.Width = 100;
			this.columnHeader4.Text = "机刷";
			this.columnHeader4.Width = 200;
			this.columnHeader5.Text = "人工";
			this.columnHeader5.Width = 200;
			this.columnHeader6.Text = "总计";
			this.columnHeader6.Width = 80;
			this.columnHeader7.Text = "当前次数";
			this.columnHeader7.Width = 80;
			this.button1.Font = new global::System.Drawing.Font("宋体", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.button1.Location = new global::System.Drawing.Point(110, 529);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(145, 43);
			this.button1.TabIndex = 40;
			this.button1.Text = "添加";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.Button1_Click);
			this.button2.Font = new global::System.Drawing.Font("宋体", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.button2.Location = new global::System.Drawing.Point(288, 529);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(145, 43);
			this.button2.TabIndex = 41;
			this.button2.Text = "保存配置";
			this.button2.UseVisualStyleBackColor = true;
			this.button3.Font = new global::System.Drawing.Font("宋体", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.button3.Location = new global::System.Drawing.Point(466, 529);
			this.button3.Name = "button3";
			this.button3.Size = new global::System.Drawing.Size(145, 43);
			this.button3.TabIndex = 42;
			this.button3.Text = "开始/暂停";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new global::System.EventHandler(this.button3_Click);
			this.listView2.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.columnHeader2,
				this.columnHeader8,
				this.columnHeader9,
				this.columnHeader10
			});
			this.listView2.Font = new global::System.Drawing.Font("宋体", 11f);
			this.listView2.FullRowSelect = true;
			this.listView2.GridLines = true;
			this.listView2.HideSelection = false;
			this.listView2.Location = new global::System.Drawing.Point(2, 320);
			this.listView2.Name = "listView2";
			this.listView2.Size = new global::System.Drawing.Size(833, 203);
			this.listView2.TabIndex = 43;
			this.listView2.UseCompatibleStateImageBehavior = false;
			this.listView2.View = global::System.Windows.Forms.View.Details;
			this.columnHeader2.Text = "序号";
			this.columnHeader8.Text = "备注名";
			this.columnHeader8.Width = 100;
			this.columnHeader9.Text = "时间";
			this.columnHeader9.Width = 150;
			this.columnHeader10.Text = "运行状态";
			this.columnHeader10.Width = 500;
			this.textBox9.Location = new global::System.Drawing.Point(270, 212);
			this.textBox9.Multiline = true;
			this.textBox9.Name = "textBox9";
			this.textBox9.Size = new global::System.Drawing.Size(328, 85);
			this.textBox9.TabIndex = 44;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(833, 584);
			base.Controls.Add(this.textBox9);
			base.Controls.Add(this.listView2);
			base.Controls.Add(this.button3);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.listView1);
			base.Controls.Add(this.groupBox3);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.groupBox1);
			base.Name = "微博转赞评";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "微博转赞评";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.微博转赞评_FormClosing);
			base.Load += new global::System.EventHandler(this.微博转赞评_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400017A RID: 378
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x0400017B RID: 379
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x0400017C RID: 380
		private global::System.Windows.Forms.TextBox textBox2;

		// Token: 0x0400017D RID: 381
		private global::System.Windows.Forms.Label label2;

		// Token: 0x0400017E RID: 382
		private global::System.Windows.Forms.TextBox textBox1;

		// Token: 0x0400017F RID: 383
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000180 RID: 384
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04000181 RID: 385
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x04000182 RID: 386
		private global::System.Windows.Forms.TextBox textBox5;

		// Token: 0x04000183 RID: 387
		private global::System.Windows.Forms.Label label6;

		// Token: 0x04000184 RID: 388
		private global::System.Windows.Forms.TextBox textBox3;

		// Token: 0x04000185 RID: 389
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04000186 RID: 390
		private global::System.Windows.Forms.TextBox textBox4;

		// Token: 0x04000187 RID: 391
		private global::System.Windows.Forms.Label label5;

		// Token: 0x04000188 RID: 392
		private global::System.Windows.Forms.GroupBox groupBox3;

		// Token: 0x04000189 RID: 393
		private global::System.Windows.Forms.TextBox textBox6;

		// Token: 0x0400018A RID: 394
		private global::System.Windows.Forms.Label label7;

		// Token: 0x0400018B RID: 395
		private global::System.Windows.Forms.TextBox textBox7;

		// Token: 0x0400018C RID: 396
		private global::System.Windows.Forms.Label label8;

		// Token: 0x0400018D RID: 397
		private global::System.Windows.Forms.TextBox textBox8;

		// Token: 0x0400018E RID: 398
		private global::System.Windows.Forms.Label label9;

		// Token: 0x0400018F RID: 399
		private global::System.Windows.Forms.ListView listView1;

		// Token: 0x04000190 RID: 400
		private global::System.Windows.Forms.ColumnHeader columnHeader1;

		// Token: 0x04000191 RID: 401
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04000192 RID: 402
		private global::System.Windows.Forms.Button button2;

		// Token: 0x04000193 RID: 403
		private global::System.Windows.Forms.Button button3;

		// Token: 0x04000194 RID: 404
		private global::System.Windows.Forms.ColumnHeader columnHeader3;

		// Token: 0x04000195 RID: 405
		private global::System.Windows.Forms.ColumnHeader columnHeader4;

		// Token: 0x04000196 RID: 406
		private global::System.Windows.Forms.ColumnHeader columnHeader5;

		// Token: 0x04000197 RID: 407
		private global::System.Windows.Forms.ColumnHeader columnHeader6;

		// Token: 0x04000198 RID: 408
		private global::System.Windows.Forms.ColumnHeader columnHeader7;

		// Token: 0x04000199 RID: 409
		private global::System.Windows.Forms.ListView listView2;

		// Token: 0x0400019A RID: 410
		private global::System.Windows.Forms.ColumnHeader columnHeader2;

		// Token: 0x0400019B RID: 411
		private global::System.Windows.Forms.ColumnHeader columnHeader8;

		// Token: 0x0400019C RID: 412
		private global::System.Windows.Forms.ColumnHeader columnHeader9;

		// Token: 0x0400019D RID: 413
		private global::System.Windows.Forms.ColumnHeader columnHeader10;

		// Token: 0x0400019E RID: 414
		private global::System.Windows.Forms.TextBox textBox9;
	}
}
