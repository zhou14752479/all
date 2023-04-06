namespace 启动程序
{
	// Token: 0x02000013 RID: 19
	public partial class 电商抓取 : global::System.Windows.Forms.Form
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x00017CB0 File Offset: 0x00015EB0
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00017CE8 File Offset: 0x00015EE8
		private void InitializeComponent()
		{
			this.splitContainer1 = new global::System.Windows.Forms.SplitContainer();
			this.button4 = new global::System.Windows.Forms.Button();
			this.button3 = new global::System.Windows.Forms.Button();
			this.button2 = new global::System.Windows.Forms.Button();
			this.button1 = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.listView1 = new global::System.Windows.Forms.ListView();
			this.columnHeader1 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader10 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader11 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader12 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader9 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader14 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new global::System.Windows.Forms.ColumnHeader();
			this.button5 = new global::System.Windows.Forms.Button();
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			base.SuspendLayout();
			this.splitContainer1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new global::System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = global::System.Windows.Forms.Orientation.Horizontal;
			this.splitContainer1.Panel1.Controls.Add(this.button5);
			this.splitContainer1.Panel1.Controls.Add(this.textBox1);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.button4);
			this.splitContainer1.Panel1.Controls.Add(this.button3);
			this.splitContainer1.Panel1.Controls.Add(this.button2);
			this.splitContainer1.Panel1.Controls.Add(this.button1);
			this.splitContainer1.Panel1.Paint += new global::System.Windows.Forms.PaintEventHandler(this.SplitContainer1_Panel1_Paint);
			this.splitContainer1.Panel2.Controls.Add(this.listView1);
			this.splitContainer1.Size = new global::System.Drawing.Size(1042, 594);
			this.splitContainer1.SplitterDistance = 105;
			this.splitContainer1.TabIndex = 2;
			this.button4.Location = new global::System.Drawing.Point(791, 57);
			this.button4.Name = "button4";
			this.button4.Size = new global::System.Drawing.Size(112, 30);
			this.button4.TabIndex = 37;
			this.button4.Text = "停止";
			this.button4.UseVisualStyleBackColor = true;
			this.button3.Location = new global::System.Drawing.Point(656, 57);
			this.button3.Name = "button3";
			this.button3.Size = new global::System.Drawing.Size(112, 30);
			this.button3.TabIndex = 36;
			this.button3.Text = "继续";
			this.button3.UseVisualStyleBackColor = true;
			this.button2.Location = new global::System.Drawing.Point(791, 21);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(112, 30);
			this.button2.TabIndex = 35;
			this.button2.Text = "暂停";
			this.button2.UseVisualStyleBackColor = true;
			this.button1.Location = new global::System.Drawing.Point(656, 21);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(112, 30);
			this.button1.TabIndex = 34;
			this.button1.Text = "开始运行";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(54, 30);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(65, 12);
			this.label1.TabIndex = 38;
			this.label1.Text = "输入编号：";
			this.textBox1.Location = new global::System.Drawing.Point(152, 27);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new global::System.Drawing.Size(180, 21);
			this.textBox1.TabIndex = 39;
			this.textBox1.Text = "6Q0 959 856";
			this.listView1.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.columnHeader1,
				this.columnHeader10,
				this.columnHeader12,
				this.columnHeader11,
				this.columnHeader7,
				this.columnHeader2,
				this.columnHeader14,
				this.columnHeader8,
				this.columnHeader9
			});
			this.listView1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new global::System.Drawing.Point(0, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new global::System.Drawing.Size(1042, 485);
			this.listView1.TabIndex = 46;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = global::System.Windows.Forms.View.Details;
			this.columnHeader1.Text = "序号";
			this.columnHeader1.Width = 40;
			this.columnHeader10.Text = "title";
			this.columnHeader10.Width = 150;
			this.columnHeader11.DisplayIndex = 3;
			this.columnHeader11.Text = "Application";
			this.columnHeader11.Width = 100;
			this.columnHeader12.DisplayIndex = 2;
			this.columnHeader12.Text = "weight";
			this.columnHeader12.Width = 100;
			this.columnHeader7.Text = "package-size";
			this.columnHeader7.Width = 100;
			this.columnHeader8.Text = "price-ebay";
			this.columnHeader8.Width = 100;
			this.columnHeader9.Text = "price-Amazon";
			this.columnHeader9.Width = 100;
			this.columnHeader14.Text = "price-Aliexpress";
			this.columnHeader14.Width = 150;
			this.columnHeader2.Text = "Interchange No.";
			this.columnHeader2.Width = 150;
			this.button5.Location = new global::System.Drawing.Point(918, 57);
			this.button5.Name = "button5";
			this.button5.Size = new global::System.Drawing.Size(112, 30);
			this.button5.TabIndex = 40;
			this.button5.Text = "导出数据";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new global::System.EventHandler(this.button5_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(1042, 594);
			base.Controls.Add(this.splitContainer1);
			base.Name = "电商抓取";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "电商抓取";
			base.Load += new global::System.EventHandler(this.电商抓取_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x040001E8 RID: 488
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x040001E9 RID: 489
		private global::System.Windows.Forms.SplitContainer splitContainer1;

		// Token: 0x040001EA RID: 490
		private global::System.Windows.Forms.Button button4;

		// Token: 0x040001EB RID: 491
		private global::System.Windows.Forms.Button button3;

		// Token: 0x040001EC RID: 492
		private global::System.Windows.Forms.Button button2;

		// Token: 0x040001ED RID: 493
		private global::System.Windows.Forms.Button button1;

		// Token: 0x040001EE RID: 494
		private global::System.Windows.Forms.TextBox textBox1;

		// Token: 0x040001EF RID: 495
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040001F0 RID: 496
		private global::System.Windows.Forms.ListView listView1;

		// Token: 0x040001F1 RID: 497
		private global::System.Windows.Forms.ColumnHeader columnHeader1;

		// Token: 0x040001F2 RID: 498
		private global::System.Windows.Forms.ColumnHeader columnHeader10;

		// Token: 0x040001F3 RID: 499
		private global::System.Windows.Forms.ColumnHeader columnHeader11;

		// Token: 0x040001F4 RID: 500
		private global::System.Windows.Forms.ColumnHeader columnHeader12;

		// Token: 0x040001F5 RID: 501
		private global::System.Windows.Forms.ColumnHeader columnHeader7;

		// Token: 0x040001F6 RID: 502
		private global::System.Windows.Forms.ColumnHeader columnHeader8;

		// Token: 0x040001F7 RID: 503
		private global::System.Windows.Forms.ColumnHeader columnHeader9;

		// Token: 0x040001F8 RID: 504
		private global::System.Windows.Forms.ColumnHeader columnHeader14;

		// Token: 0x040001F9 RID: 505
		private global::System.Windows.Forms.ColumnHeader columnHeader2;

		// Token: 0x040001FA RID: 506
		private global::System.Windows.Forms.Button button5;
	}
}
