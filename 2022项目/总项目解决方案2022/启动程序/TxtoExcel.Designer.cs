namespace 启动程序
{
	// Token: 0x02000005 RID: 5
	public partial class TxtoExcel : global::System.Windows.Forms.Form
	{
		// Token: 0x0600001A RID: 26 RVA: 0x00003B08 File Offset: 0x00001D08
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00003B40 File Offset: 0x00001D40
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			this.label1 = new global::System.Windows.Forms.Label();
			this.button1 = new global::System.Windows.Forms.Button();
			this.button2 = new global::System.Windows.Forms.Button();
			this.listView1 = new global::System.Windows.Forms.ListView();
			this.columnHeader1 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader9 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader10 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader11 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader12 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader13 = new global::System.Windows.Forms.ColumnHeader();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.复制网址ToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.复制串码ToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.重新扫描ToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.openFileDialog1 = new global::System.Windows.Forms.OpenFileDialog();
			this.button3 = new global::System.Windows.Forms.Button();
			this.contextMenuStrip1.SuspendLayout();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(12, 450);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(41, 12);
			this.label1.TabIndex = 52;
			this.label1.Text = "未开始";
			this.button1.Location = new global::System.Drawing.Point(326, 386);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(88, 29);
			this.button1.TabIndex = 51;
			this.button1.Text = "导入文本";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.Button1_Click);
			this.button2.Location = new global::System.Drawing.Point(708, 390);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(114, 29);
			this.button2.TabIndex = 47;
			this.button2.Text = "开始执行";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.Button2_Click);
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
				this.columnHeader9,
				this.columnHeader10,
				this.columnHeader11,
				this.columnHeader12,
				this.columnHeader13
			});
			this.listView1.ContextMenuStrip = this.contextMenuStrip1;
			this.listView1.Dock = global::System.Windows.Forms.DockStyle.Top;
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new global::System.Drawing.Point(0, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new global::System.Drawing.Size(942, 380);
			this.listView1.TabIndex = 46;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = global::System.Windows.Forms.View.Details;
			this.columnHeader1.Text = "姓名";
			this.columnHeader2.Text = "统一社会信用代码";
			this.columnHeader2.Width = 120;
			this.columnHeader3.Text = "险种类型";
			this.columnHeader4.Text = "基准参保关系ID";
			this.columnHeader4.Width = 100;
			this.columnHeader5.Text = "证件类型";
			this.columnHeader5.Width = 80;
			this.columnHeader6.Text = "证件号码";
			this.columnHeader6.Width = 120;
			this.columnHeader7.Text = "开始日期";
			this.columnHeader7.Width = 80;
			this.columnHeader8.Text = "终止日期";
			this.columnHeader8.Width = 80;
			this.columnHeader9.Text = "单位编号";
			this.columnHeader10.Text = "单位名称";
			this.columnHeader10.Width = 100;
			this.columnHeader11.Text = "行政区划代码";
			this.columnHeader11.Width = 100;
			this.columnHeader12.Text = "个人编号";
			this.columnHeader13.Text = "社会保障号码";
			this.columnHeader13.Width = 100;
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.复制网址ToolStripMenuItem,
				this.复制串码ToolStripMenuItem,
				this.重新扫描ToolStripMenuItem
			});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new global::System.Drawing.Size(125, 70);
			this.复制网址ToolStripMenuItem.Name = "复制网址ToolStripMenuItem";
			this.复制网址ToolStripMenuItem.Size = new global::System.Drawing.Size(124, 22);
			this.复制网址ToolStripMenuItem.Text = "复制网址";
			this.复制串码ToolStripMenuItem.Name = "复制串码ToolStripMenuItem";
			this.复制串码ToolStripMenuItem.Size = new global::System.Drawing.Size(124, 22);
			this.复制串码ToolStripMenuItem.Text = "复制串码";
			this.重新扫描ToolStripMenuItem.Name = "重新扫描ToolStripMenuItem";
			this.重新扫描ToolStripMenuItem.Size = new global::System.Drawing.Size(124, 22);
			this.重新扫描ToolStripMenuItem.Text = "重新扫描";
			this.textBox1.Font = new global::System.Drawing.Font("宋体", 10f);
			this.textBox1.Location = new global::System.Drawing.Point(0, 386);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new global::System.Drawing.Size(320, 29);
			this.textBox1.TabIndex = 49;
			this.openFileDialog1.FileName = "openFileDialog1";
			this.button3.Location = new global::System.Drawing.Point(828, 390);
			this.button3.Name = "button3";
			this.button3.Size = new global::System.Drawing.Size(114, 29);
			this.button3.TabIndex = 53;
			this.button3.Text = "输出表格";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new global::System.EventHandler(this.Button3_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(942, 431);
			base.Controls.Add(this.button3);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.listView1);
			base.Controls.Add(this.textBox1);
			base.Name = "TxtoExcel";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "TxtoExcel";
			this.contextMenuStrip1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000023 RID: 35
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000024 RID: 36
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000025 RID: 37
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04000026 RID: 38
		private global::System.Windows.Forms.Button button2;

		// Token: 0x04000027 RID: 39
		private global::System.Windows.Forms.ListView listView1;

		// Token: 0x04000028 RID: 40
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04000029 RID: 41
		private global::System.Windows.Forms.ToolStripMenuItem 复制网址ToolStripMenuItem;

		// Token: 0x0400002A RID: 42
		private global::System.Windows.Forms.ToolStripMenuItem 复制串码ToolStripMenuItem;

		// Token: 0x0400002B RID: 43
		private global::System.Windows.Forms.ToolStripMenuItem 重新扫描ToolStripMenuItem;

		// Token: 0x0400002C RID: 44
		private global::System.Windows.Forms.TextBox textBox1;

		// Token: 0x0400002D RID: 45
		private global::System.Windows.Forms.OpenFileDialog openFileDialog1;

		// Token: 0x0400002E RID: 46
		private global::System.Windows.Forms.Button button3;

		// Token: 0x0400002F RID: 47
		private global::System.Windows.Forms.ColumnHeader columnHeader1;

		// Token: 0x04000030 RID: 48
		private global::System.Windows.Forms.ColumnHeader columnHeader2;

		// Token: 0x04000031 RID: 49
		private global::System.Windows.Forms.ColumnHeader columnHeader3;

		// Token: 0x04000032 RID: 50
		private global::System.Windows.Forms.ColumnHeader columnHeader4;

		// Token: 0x04000033 RID: 51
		private global::System.Windows.Forms.ColumnHeader columnHeader5;

		// Token: 0x04000034 RID: 52
		private global::System.Windows.Forms.ColumnHeader columnHeader6;

		// Token: 0x04000035 RID: 53
		private global::System.Windows.Forms.ColumnHeader columnHeader7;

		// Token: 0x04000036 RID: 54
		private global::System.Windows.Forms.ColumnHeader columnHeader8;

		// Token: 0x04000037 RID: 55
		private global::System.Windows.Forms.ColumnHeader columnHeader9;

		// Token: 0x04000038 RID: 56
		private global::System.Windows.Forms.ColumnHeader columnHeader10;

		// Token: 0x04000039 RID: 57
		private global::System.Windows.Forms.ColumnHeader columnHeader11;

		// Token: 0x0400003A RID: 58
		private global::System.Windows.Forms.ColumnHeader columnHeader12;

		// Token: 0x0400003B RID: 59
		private global::System.Windows.Forms.ColumnHeader columnHeader13;
	}
}
