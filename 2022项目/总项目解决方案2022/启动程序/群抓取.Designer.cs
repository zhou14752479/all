namespace 启动程序
{
	// Token: 0x02000018 RID: 24
	public partial class 群抓取 : global::System.Windows.Forms.Form
	{
		// Token: 0x060000F8 RID: 248 RVA: 0x0001EDC8 File Offset: 0x0001CFC8
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0001EE00 File Offset: 0x0001D000
		private void InitializeComponent()
		{
			this.columnHeader1 = new global::System.Windows.Forms.ColumnHeader();
			this.listView1 = new global::System.Windows.Forms.ListView();
			this.columnHeader2 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new global::System.Windows.Forms.ColumnHeader();
			this.splitContainer1 = new global::System.Windows.Forms.SplitContainer();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.button5 = new global::System.Windows.Forms.Button();
			this.button3 = new global::System.Windows.Forms.Button();
			this.button2 = new global::System.Windows.Forms.Button();
			this.button1 = new global::System.Windows.Forms.Button();
			this.columnHeader4 = new global::System.Windows.Forms.ColumnHeader();
			this.button4 = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.button6 = new global::System.Windows.Forms.Button();
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
				this.columnHeader4
			});
			this.listView1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new global::System.Drawing.Point(0, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new global::System.Drawing.Size(499, 547);
			this.listView1.TabIndex = 46;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = global::System.Windows.Forms.View.Details;
			this.columnHeader2.Text = "发布时间";
			this.columnHeader2.Width = 150;
			this.columnHeader3.Text = "二维码地址";
			this.columnHeader3.Width = 200;
			this.splitContainer1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new global::System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.Controls.Add(this.button6);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.button4);
			this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
			this.splitContainer1.Panel1.Controls.Add(this.button5);
			this.splitContainer1.Panel1.Controls.Add(this.button3);
			this.splitContainer1.Panel1.Controls.Add(this.button2);
			this.splitContainer1.Panel1.Controls.Add(this.button1);
			this.splitContainer1.Panel1.Paint += new global::System.Windows.Forms.PaintEventHandler(this.SplitContainer1_Panel1_Paint);
			this.splitContainer1.Panel2.Controls.Add(this.listView1);
			this.splitContainer1.Size = new global::System.Drawing.Size(684, 547);
			this.splitContainer1.SplitterDistance = 181;
			this.splitContainer1.TabIndex = 4;
			this.groupBox1.Controls.Add(this.textBox1);
			this.groupBox1.Dock = global::System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new global::System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new global::System.Drawing.Size(181, 339);
			this.groupBox1.TabIndex = 41;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "输入关键字";
			this.textBox1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location = new global::System.Drawing.Point(3, 17);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new global::System.Drawing.Size(175, 319);
			this.textBox1.TabIndex = 40;
			this.textBox1.Text = "英雄联盟";
			this.button5.Location = new global::System.Drawing.Point(8, 420);
			this.button5.Name = "button5";
			this.button5.Size = new global::System.Drawing.Size(82, 30);
			this.button5.TabIndex = 40;
			this.button5.Text = "导出数据";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new global::System.EventHandler(this.button5_Click);
			this.button3.Location = new global::System.Drawing.Point(8, 384);
			this.button3.Name = "button3";
			this.button3.Size = new global::System.Drawing.Size(82, 30);
			this.button3.TabIndex = 36;
			this.button3.Text = "继续";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new global::System.EventHandler(this.button3_Click);
			this.button2.Location = new global::System.Drawing.Point(8, 348);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(82, 30);
			this.button2.TabIndex = 35;
			this.button2.Text = "暂停";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.button2_Click);
			this.button1.Location = new global::System.Drawing.Point(96, 348);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(75, 102);
			this.button1.TabIndex = 34;
			this.button1.Text = "开始抓取";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.Button1_Click);
			this.columnHeader4.Text = "关键字";
			this.button4.Enabled = false;
			this.button4.Location = new global::System.Drawing.Point(8, 456);
			this.button4.Name = "button4";
			this.button4.Size = new global::System.Drawing.Size(163, 30);
			this.button4.TabIndex = 42;
			this.button4.Text = "一键下载";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new global::System.EventHandler(this.button4_Click);
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(6, 523);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(41, 12);
			this.label1.TabIndex = 43;
			this.label1.Text = "未下载";
			this.button6.Location = new global::System.Drawing.Point(8, 490);
			this.button6.Name = "button6";
			this.button6.Size = new global::System.Drawing.Size(163, 30);
			this.button6.TabIndex = 44;
			this.button6.Text = "清空记录";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new global::System.EventHandler(this.button6_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(684, 547);
			base.Controls.Add(this.splitContainer1);
			base.Name = "群抓取";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "群抓取";
			base.Load += new global::System.EventHandler(this.群抓取_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x0400028C RID: 652
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x0400028D RID: 653
		private global::System.Windows.Forms.ColumnHeader columnHeader1;

		// Token: 0x0400028E RID: 654
		private global::System.Windows.Forms.ListView listView1;

		// Token: 0x0400028F RID: 655
		private global::System.Windows.Forms.ColumnHeader columnHeader2;

		// Token: 0x04000290 RID: 656
		private global::System.Windows.Forms.ColumnHeader columnHeader3;

		// Token: 0x04000291 RID: 657
		private global::System.Windows.Forms.SplitContainer splitContainer1;

		// Token: 0x04000292 RID: 658
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04000293 RID: 659
		private global::System.Windows.Forms.TextBox textBox1;

		// Token: 0x04000294 RID: 660
		private global::System.Windows.Forms.Button button5;

		// Token: 0x04000295 RID: 661
		private global::System.Windows.Forms.Button button3;

		// Token: 0x04000296 RID: 662
		private global::System.Windows.Forms.Button button2;

		// Token: 0x04000297 RID: 663
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04000298 RID: 664
		private global::System.Windows.Forms.ColumnHeader columnHeader4;

		// Token: 0x04000299 RID: 665
		private global::System.Windows.Forms.Button button4;

		// Token: 0x0400029A RID: 666
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400029B RID: 667
		private global::System.Windows.Forms.Button button6;
	}
}
