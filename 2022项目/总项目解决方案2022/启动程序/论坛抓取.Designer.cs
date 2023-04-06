namespace 启动程序
{
	// Token: 0x0200001A RID: 26
	public partial class 论坛抓取 : global::System.Windows.Forms.Form
	{
		// Token: 0x06000113 RID: 275 RVA: 0x00020E2C File Offset: 0x0001F02C
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00020E64 File Offset: 0x0001F064
		private void InitializeComponent()
		{
			this.columnHeader1 = new global::System.Windows.Forms.ColumnHeader();
			this.listView1 = new global::System.Windows.Forms.ListView();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.button4 = new global::System.Windows.Forms.Button();
			this.button3 = new global::System.Windows.Forms.Button();
			this.button2 = new global::System.Windows.Forms.Button();
			this.button1 = new global::System.Windows.Forms.Button();
			this.splitContainer1 = new global::System.Windows.Forms.SplitContainer();
			this.button5 = new global::System.Windows.Forms.Button();
			this.columnHeader2 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader9 = new global::System.Windows.Forms.ColumnHeader();
			this.label1 = new global::System.Windows.Forms.Label();
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			base.SuspendLayout();
			this.columnHeader1.Text = "序号";
			this.listView1.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.columnHeader1,
				this.columnHeader2,
				this.columnHeader3,
				this.columnHeader4,
				this.columnHeader5,
				this.columnHeader6,
				this.columnHeader7,
				this.columnHeader8,
				this.columnHeader9
			});
			this.listView1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new global::System.Drawing.Point(0, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new global::System.Drawing.Size(1122, 487);
			this.listView1.TabIndex = 36;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = global::System.Windows.Forms.View.Details;
			this.textBox1.Location = new global::System.Drawing.Point(303, 28);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new global::System.Drawing.Size(816, 144);
			this.textBox1.TabIndex = 39;
			this.textBox1.Text = "https://www.tripadvisor.com/ShowTopic-g1-i12290-k10842388-Hyatt_Best_Rate_Guarantee_BRG_Scam_2017-Bargain_Travel.html";
			this.button4.Location = new global::System.Drawing.Point(154, 68);
			this.button4.Name = "button4";
			this.button4.Size = new global::System.Drawing.Size(112, 30);
			this.button4.TabIndex = 38;
			this.button4.Text = "停止";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new global::System.EventHandler(this.Button4_Click);
			this.button3.Location = new global::System.Drawing.Point(19, 68);
			this.button3.Name = "button3";
			this.button3.Size = new global::System.Drawing.Size(112, 30);
			this.button3.TabIndex = 37;
			this.button3.Text = "继续";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new global::System.EventHandler(this.Button3_Click);
			this.button2.Location = new global::System.Drawing.Point(154, 32);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(112, 30);
			this.button2.TabIndex = 36;
			this.button2.Text = "暂停";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.Button2_Click);
			this.button1.Location = new global::System.Drawing.Point(19, 32);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(112, 30);
			this.button1.TabIndex = 34;
			this.button1.Text = "开始运行";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.Button1_Click);
			this.splitContainer1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new global::System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = global::System.Windows.Forms.Orientation.Horizontal;
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.button5);
			this.splitContainer1.Panel1.Controls.Add(this.textBox1);
			this.splitContainer1.Panel1.Controls.Add(this.button4);
			this.splitContainer1.Panel1.Controls.Add(this.button3);
			this.splitContainer1.Panel1.Controls.Add(this.button2);
			this.splitContainer1.Panel1.Controls.Add(this.button1);
			this.splitContainer1.Panel2.Controls.Add(this.listView1);
			this.splitContainer1.Size = new global::System.Drawing.Size(1122, 679);
			this.splitContainer1.SplitterDistance = 188;
			this.splitContainer1.TabIndex = 2;
			this.button5.Location = new global::System.Drawing.Point(19, 104);
			this.button5.Name = "button5";
			this.button5.Size = new global::System.Drawing.Size(112, 30);
			this.button5.TabIndex = 40;
			this.button5.Text = "导出";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new global::System.EventHandler(this.Button5_Click);
			this.columnHeader2.Text = "title";
			this.columnHeader2.Width = 200;
			this.columnHeader3.Text = "name";
			this.columnHeader3.Width = 80;
			this.columnHeader4.Text = "location";
			this.columnHeader4.Width = 80;
			this.columnHeader5.Text = "body";
			this.columnHeader5.Width = 300;
			this.columnHeader6.Text = "postDate";
			this.columnHeader6.Width = 80;
			this.columnHeader7.Text = "posts";
			this.columnHeader8.Text = "reviews";
			this.columnHeader9.Text = "helpful votes";
			this.columnHeader9.Width = 100;
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(303, 10);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(89, 12);
			this.label1.TabIndex = 41;
			this.label1.Text = "一行一个链接：";
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(1122, 679);
			base.Controls.Add(this.splitContainer1);
			base.Name = "论坛抓取";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "论坛抓取";
			base.Load += new global::System.EventHandler(this.论坛抓取_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x040002B2 RID: 690
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x040002B3 RID: 691
		private global::System.Windows.Forms.ColumnHeader columnHeader1;

		// Token: 0x040002B4 RID: 692
		private global::System.Windows.Forms.ListView listView1;

		// Token: 0x040002B5 RID: 693
		private global::System.Windows.Forms.TextBox textBox1;

		// Token: 0x040002B6 RID: 694
		private global::System.Windows.Forms.Button button4;

		// Token: 0x040002B7 RID: 695
		private global::System.Windows.Forms.Button button3;

		// Token: 0x040002B8 RID: 696
		private global::System.Windows.Forms.Button button2;

		// Token: 0x040002B9 RID: 697
		private global::System.Windows.Forms.Button button1;

		// Token: 0x040002BA RID: 698
		private global::System.Windows.Forms.SplitContainer splitContainer1;

		// Token: 0x040002BB RID: 699
		private global::System.Windows.Forms.Button button5;

		// Token: 0x040002BC RID: 700
		private global::System.Windows.Forms.ColumnHeader columnHeader2;

		// Token: 0x040002BD RID: 701
		private global::System.Windows.Forms.ColumnHeader columnHeader3;

		// Token: 0x040002BE RID: 702
		private global::System.Windows.Forms.ColumnHeader columnHeader4;

		// Token: 0x040002BF RID: 703
		private global::System.Windows.Forms.ColumnHeader columnHeader5;

		// Token: 0x040002C0 RID: 704
		private global::System.Windows.Forms.ColumnHeader columnHeader6;

		// Token: 0x040002C1 RID: 705
		private global::System.Windows.Forms.ColumnHeader columnHeader7;

		// Token: 0x040002C2 RID: 706
		private global::System.Windows.Forms.ColumnHeader columnHeader8;

		// Token: 0x040002C3 RID: 707
		private global::System.Windows.Forms.ColumnHeader columnHeader9;

		// Token: 0x040002C4 RID: 708
		private global::System.Windows.Forms.Label label1;
	}
}
