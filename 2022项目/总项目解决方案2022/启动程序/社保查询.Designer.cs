namespace 启动程序
{
	// Token: 0x02000016 RID: 22
	public partial class 社保查询 : global::System.Windows.Forms.Form
	{
		// Token: 0x060000DA RID: 218 RVA: 0x0001A0D8 File Offset: 0x000182D8
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0001A110 File Offset: 0x00018310
		private void InitializeComponent()
		{
			this.columnHeader1 = new global::System.Windows.Forms.ColumnHeader();
			this.listView1 = new global::System.Windows.Forms.ListView();
			this.columnHeader2 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new global::System.Windows.Forms.ColumnHeader();
			this.splitContainer1 = new global::System.Windows.Forms.SplitContainer();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.button5 = new global::System.Windows.Forms.Button();
			this.button4 = new global::System.Windows.Forms.Button();
			this.button3 = new global::System.Windows.Forms.Button();
			this.button2 = new global::System.Windows.Forms.Button();
			this.button1 = new global::System.Windows.Forms.Button();
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.columnHeader1.Text = "序号";
			this.columnHeader1.Width = 40;
			this.listView1.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.columnHeader1,
				this.columnHeader2,
				this.columnHeader3,
				this.columnHeader4,
				this.columnHeader5,
				this.columnHeader6,
				this.columnHeader7
			});
			this.listView1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new global::System.Drawing.Point(0, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new global::System.Drawing.Size(584, 545);
			this.listView1.TabIndex = 46;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = global::System.Windows.Forms.View.Details;
			this.columnHeader2.Text = "姓名";
			this.columnHeader3.Text = "身份证";
			this.columnHeader3.Width = 130;
			this.columnHeader4.Text = "企业名称";
			this.columnHeader4.Width = 150;
			this.columnHeader5.Text = "人员编号";
			this.columnHeader6.Text = "保险类型";
			this.columnHeader7.Text = "联系地址";
			this.splitContainer1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new global::System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
			this.splitContainer1.Panel1.Controls.Add(this.button5);
			this.splitContainer1.Panel1.Controls.Add(this.button4);
			this.splitContainer1.Panel1.Controls.Add(this.button3);
			this.splitContainer1.Panel1.Controls.Add(this.button2);
			this.splitContainer1.Panel1.Controls.Add(this.button1);
			this.splitContainer1.Panel1.Paint += new global::System.Windows.Forms.PaintEventHandler(this.SplitContainer1_Panel1_Paint);
			this.splitContainer1.Panel2.Controls.Add(this.listView1);
			this.splitContainer1.Size = new global::System.Drawing.Size(822, 545);
			this.splitContainer1.SplitterDistance = 234;
			this.splitContainer1.TabIndex = 3;
			this.groupBox1.Controls.Add(this.textBox1);
			this.groupBox1.Dock = global::System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new global::System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new global::System.Drawing.Size(234, 423);
			this.groupBox1.TabIndex = 41;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "复制待查询的数据";
			this.textBox1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location = new global::System.Drawing.Point(3, 17);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new global::System.Drawing.Size(228, 403);
			this.textBox1.TabIndex = 40;
			this.button5.Location = new global::System.Drawing.Point(3, 501);
			this.button5.Name = "button5";
			this.button5.Size = new global::System.Drawing.Size(94, 30);
			this.button5.TabIndex = 40;
			this.button5.Text = "导出数据";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new global::System.EventHandler(this.Button5_Click);
			this.button4.Location = new global::System.Drawing.Point(101, 429);
			this.button4.Name = "button4";
			this.button4.Size = new global::System.Drawing.Size(92, 30);
			this.button4.TabIndex = 37;
			this.button4.Text = "停止";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new global::System.EventHandler(this.Button4_Click);
			this.button3.Location = new global::System.Drawing.Point(3, 465);
			this.button3.Name = "button3";
			this.button3.Size = new global::System.Drawing.Size(94, 30);
			this.button3.TabIndex = 36;
			this.button3.Text = "继续";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new global::System.EventHandler(this.Button3_Click);
			this.button2.Location = new global::System.Drawing.Point(3, 429);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(92, 30);
			this.button2.TabIndex = 35;
			this.button2.Text = "暂停";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.Button2_Click);
			this.button1.Location = new global::System.Drawing.Point(103, 465);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(90, 66);
			this.button1.TabIndex = 34;
			this.button1.Text = "开始运行";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(822, 545);
			base.Controls.Add(this.splitContainer1);
			base.Name = "社保查询";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "社保查询";
			base.Load += new global::System.EventHandler(this.社保查询_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x0400021F RID: 543
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000220 RID: 544
		private global::System.Windows.Forms.ColumnHeader columnHeader1;

		// Token: 0x04000221 RID: 545
		private global::System.Windows.Forms.ListView listView1;

		// Token: 0x04000222 RID: 546
		private global::System.Windows.Forms.SplitContainer splitContainer1;

		// Token: 0x04000223 RID: 547
		private global::System.Windows.Forms.Button button5;

		// Token: 0x04000224 RID: 548
		private global::System.Windows.Forms.Button button4;

		// Token: 0x04000225 RID: 549
		private global::System.Windows.Forms.Button button3;

		// Token: 0x04000226 RID: 550
		private global::System.Windows.Forms.Button button2;

		// Token: 0x04000227 RID: 551
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04000228 RID: 552
		private global::System.Windows.Forms.ColumnHeader columnHeader2;

		// Token: 0x04000229 RID: 553
		private global::System.Windows.Forms.ColumnHeader columnHeader3;

		// Token: 0x0400022A RID: 554
		private global::System.Windows.Forms.ColumnHeader columnHeader4;

		// Token: 0x0400022B RID: 555
		private global::System.Windows.Forms.ColumnHeader columnHeader5;

		// Token: 0x0400022C RID: 556
		private global::System.Windows.Forms.ColumnHeader columnHeader6;

		// Token: 0x0400022D RID: 557
		private global::System.Windows.Forms.ColumnHeader columnHeader7;

		// Token: 0x0400022E RID: 558
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x0400022F RID: 559
		private global::System.Windows.Forms.TextBox textBox1;
	}
}
