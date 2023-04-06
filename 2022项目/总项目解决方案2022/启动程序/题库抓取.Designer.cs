namespace 启动程序
{
	// Token: 0x0200001B RID: 27
	public partial class 题库抓取 : global::System.Windows.Forms.Form
	{
		// Token: 0x06000121 RID: 289 RVA: 0x00021C74 File Offset: 0x0001FE74
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00021CAC File Offset: 0x0001FEAC
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.splitContainer1 = new global::System.Windows.Forms.SplitContainer();
			this.listView1 = new global::System.Windows.Forms.ListView();
			this.columnHeader1 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new global::System.Windows.Forms.ColumnHeader();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label1 = new global::System.Windows.Forms.Label();
			this.comboBox2 = new global::System.Windows.Forms.ComboBox();
			this.comboBox1 = new global::System.Windows.Forms.ComboBox();
			this.button3 = new global::System.Windows.Forms.Button();
			this.button2 = new global::System.Windows.Forms.Button();
			this.button1 = new global::System.Windows.Forms.Button();
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			base.SuspendLayout();
			this.timer1.Interval = 1000;
			this.timer1.Tick += new global::System.EventHandler(this.Timer1_Tick);
			this.splitContainer1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new global::System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = global::System.Windows.Forms.Orientation.Horizontal;
			this.splitContainer1.Panel1.Controls.Add(this.listView1);
			this.splitContainer1.Panel1.Paint += new global::System.Windows.Forms.PaintEventHandler(this.SplitContainer1_Panel1_Paint);
			this.splitContainer1.Panel2.Controls.Add(this.label2);
			this.splitContainer1.Panel2.Controls.Add(this.label1);
			this.splitContainer1.Panel2.Controls.Add(this.comboBox2);
			this.splitContainer1.Panel2.Controls.Add(this.comboBox1);
			this.splitContainer1.Panel2.Controls.Add(this.button3);
			this.splitContainer1.Panel2.Controls.Add(this.button2);
			this.splitContainer1.Panel2.Controls.Add(this.button1);
			this.splitContainer1.Size = new global::System.Drawing.Size(967, 580);
			this.splitContainer1.SplitterDistance = 471;
			this.splitContainer1.TabIndex = 6;
			this.listView1.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.columnHeader1,
				this.columnHeader2,
				this.columnHeader7,
				this.columnHeader8,
				this.columnHeader3,
				this.columnHeader4,
				this.columnHeader5,
				this.columnHeader6
			});
			this.listView1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new global::System.Drawing.Point(0, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new global::System.Drawing.Size(967, 471);
			this.listView1.TabIndex = 47;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = global::System.Windows.Forms.View.Details;
			this.columnHeader1.Text = "序号";
			this.columnHeader1.Width = 40;
			this.columnHeader2.Text = "问题";
			this.columnHeader2.Width = 200;
			this.columnHeader3.Text = "选项A";
			this.columnHeader3.Width = 100;
			this.columnHeader4.Text = "选项B";
			this.columnHeader4.Width = 100;
			this.columnHeader5.Text = "选项C";
			this.columnHeader5.Width = 100;
			this.columnHeader6.Text = "选项D";
			this.columnHeader6.Width = 100;
			this.columnHeader7.Text = "答案";
			this.columnHeader8.Text = "解析";
			this.columnHeader8.Width = 300;
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(192, 39);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(65, 12);
			this.label2.TabIndex = 15;
			this.label2.Text = "选择课程：";
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(44, 39);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(65, 12);
			this.label1.TabIndex = 14;
			this.label1.Text = "选择题库：";
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Location = new global::System.Drawing.Point(194, 57);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new global::System.Drawing.Size(121, 20);
			this.comboBox2.TabIndex = 13;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new global::System.Drawing.Point(44, 57);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new global::System.Drawing.Size(121, 20);
			this.comboBox1.TabIndex = 12;
			this.comboBox1.SelectedIndexChanged += new global::System.EventHandler(this.ComboBox1_SelectedIndexChanged);
			this.button3.Location = new global::System.Drawing.Point(639, 43);
			this.button3.Name = "button3";
			this.button3.Size = new global::System.Drawing.Size(99, 34);
			this.button3.TabIndex = 9;
			this.button3.Text = "清空结果";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new global::System.EventHandler(this.Button3_Click);
			this.button2.Location = new global::System.Drawing.Point(500, 43);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(99, 34);
			this.button2.TabIndex = 8;
			this.button2.Text = "导出结果";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.Button2_Click);
			this.button1.Location = new global::System.Drawing.Point(348, 43);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(128, 34);
			this.button1.TabIndex = 7;
			this.button1.Text = "开始";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.Button1_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(967, 580);
			base.Controls.Add(this.splitContainer1);
			base.Name = "题库抓取";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "题库抓取";
			base.Load += new global::System.EventHandler(this.题库抓取_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x040002C8 RID: 712
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x040002C9 RID: 713
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x040002CA RID: 714
		private global::System.Windows.Forms.SplitContainer splitContainer1;

		// Token: 0x040002CB RID: 715
		private global::System.Windows.Forms.ListView listView1;

		// Token: 0x040002CC RID: 716
		private global::System.Windows.Forms.ColumnHeader columnHeader1;

		// Token: 0x040002CD RID: 717
		private global::System.Windows.Forms.ColumnHeader columnHeader2;

		// Token: 0x040002CE RID: 718
		private global::System.Windows.Forms.ColumnHeader columnHeader3;

		// Token: 0x040002CF RID: 719
		private global::System.Windows.Forms.ColumnHeader columnHeader4;

		// Token: 0x040002D0 RID: 720
		private global::System.Windows.Forms.Button button1;

		// Token: 0x040002D1 RID: 721
		private global::System.Windows.Forms.ColumnHeader columnHeader5;

		// Token: 0x040002D2 RID: 722
		private global::System.Windows.Forms.ColumnHeader columnHeader6;

		// Token: 0x040002D3 RID: 723
		private global::System.Windows.Forms.ColumnHeader columnHeader7;

		// Token: 0x040002D4 RID: 724
		private global::System.Windows.Forms.ColumnHeader columnHeader8;

		// Token: 0x040002D5 RID: 725
		private global::System.Windows.Forms.ComboBox comboBox1;

		// Token: 0x040002D6 RID: 726
		private global::System.Windows.Forms.ComboBox comboBox2;

		// Token: 0x040002D7 RID: 727
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040002D8 RID: 728
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040002D9 RID: 729
		private global::System.Windows.Forms.Button button3;

		// Token: 0x040002DA RID: 730
		private global::System.Windows.Forms.Button button2;
	}
}
