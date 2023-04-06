namespace 启动程序
{
	// Token: 0x02000014 RID: 20
	public partial class 百度搜索 : global::System.Windows.Forms.Form
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x00018894 File Offset: 0x00016A94
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000188CC File Offset: 0x00016ACC
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			this.openFileDialog1 = new global::System.Windows.Forms.OpenFileDialog();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.重新扫描ToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.复制串码ToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.复制网址ToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip1 = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.listView1 = new global::System.Windows.Forms.ListView();
			this.columnHeader1 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new global::System.Windows.Forms.ColumnHeader();
			this.label1 = new global::System.Windows.Forms.Label();
			this.button1 = new global::System.Windows.Forms.Button();
			this.button2 = new global::System.Windows.Forms.Button();
			this.button3 = new global::System.Windows.Forms.Button();
			this.button4 = new global::System.Windows.Forms.Button();
			this.button5 = new global::System.Windows.Forms.Button();
			this.contextMenuStrip1.SuspendLayout();
			base.SuspendLayout();
			this.openFileDialog1.FileName = "openFileDialog1";
			this.textBox1.Location = new global::System.Drawing.Point(0, 255);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new global::System.Drawing.Size(227, 22);
			this.textBox1.TabIndex = 37;
			this.重新扫描ToolStripMenuItem.Name = "重新扫描ToolStripMenuItem";
			this.重新扫描ToolStripMenuItem.Size = new global::System.Drawing.Size(124, 22);
			this.重新扫描ToolStripMenuItem.Text = "重新扫描";
			this.复制串码ToolStripMenuItem.Name = "复制串码ToolStripMenuItem";
			this.复制串码ToolStripMenuItem.Size = new global::System.Drawing.Size(124, 22);
			this.复制串码ToolStripMenuItem.Text = "复制串码";
			this.复制网址ToolStripMenuItem.Name = "复制网址ToolStripMenuItem";
			this.复制网址ToolStripMenuItem.Size = new global::System.Drawing.Size(124, 22);
			this.复制网址ToolStripMenuItem.Text = "复制网址";
			this.contextMenuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.复制网址ToolStripMenuItem,
				this.复制串码ToolStripMenuItem,
				this.重新扫描ToolStripMenuItem
			});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new global::System.Drawing.Size(125, 70);
			this.listView1.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.columnHeader1,
				this.columnHeader2,
				this.columnHeader3
			});
			this.listView1.ContextMenuStrip = this.contextMenuStrip1;
			this.listView1.Dock = global::System.Windows.Forms.DockStyle.Top;
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new global::System.Drawing.Point(0, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new global::System.Drawing.Size(341, 249);
			this.listView1.TabIndex = 34;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = global::System.Windows.Forms.View.Details;
			this.columnHeader1.Text = "序号";
			this.columnHeader1.Width = 40;
			this.columnHeader2.Text = "关键词";
			this.columnHeader2.Width = 150;
			this.columnHeader3.Text = "结果";
			this.columnHeader3.Width = 100;
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(-2, 361);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(41, 12);
			this.label1.TabIndex = 40;
			this.label1.Text = "未开始";
			this.button1.Location = new global::System.Drawing.Point(233, 255);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(102, 23);
			this.button1.TabIndex = 39;
			this.button1.Text = "导入关键字";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.button2.Location = new global::System.Drawing.Point(1, 284);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(121, 29);
			this.button2.TabIndex = 35;
			this.button2.Text = "开始";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.button2_Click);
			this.button3.Location = new global::System.Drawing.Point(128, 284);
			this.button3.Name = "button3";
			this.button3.Size = new global::System.Drawing.Size(121, 29);
			this.button3.TabIndex = 41;
			this.button3.Text = "暂停";
			this.button3.UseVisualStyleBackColor = true;
			this.button4.Location = new global::System.Drawing.Point(1, 319);
			this.button4.Name = "button4";
			this.button4.Size = new global::System.Drawing.Size(121, 29);
			this.button4.TabIndex = 42;
			this.button4.Text = "继续";
			this.button4.UseVisualStyleBackColor = true;
			this.button5.Location = new global::System.Drawing.Point(128, 319);
			this.button5.Name = "button5";
			this.button5.Size = new global::System.Drawing.Size(121, 29);
			this.button5.TabIndex = 43;
			this.button5.Text = "导出";
			this.button5.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(341, 382);
			base.Controls.Add(this.button5);
			base.Controls.Add(this.button4);
			base.Controls.Add(this.button3);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.listView1);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.button2);
			base.Name = "百度搜索";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "百度搜索";
			base.Load += new global::System.EventHandler(this.百度搜索_Load);
			this.contextMenuStrip1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040001FC RID: 508
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x040001FD RID: 509
		private global::System.Windows.Forms.OpenFileDialog openFileDialog1;

		// Token: 0x040001FE RID: 510
		private global::System.Windows.Forms.TextBox textBox1;

		// Token: 0x040001FF RID: 511
		private global::System.Windows.Forms.ToolStripMenuItem 重新扫描ToolStripMenuItem;

		// Token: 0x04000200 RID: 512
		private global::System.Windows.Forms.ToolStripMenuItem 复制串码ToolStripMenuItem;

		// Token: 0x04000201 RID: 513
		private global::System.Windows.Forms.ToolStripMenuItem 复制网址ToolStripMenuItem;

		// Token: 0x04000202 RID: 514
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04000203 RID: 515
		private global::System.Windows.Forms.ListView listView1;

		// Token: 0x04000204 RID: 516
		private global::System.Windows.Forms.ColumnHeader columnHeader1;

		// Token: 0x04000205 RID: 517
		private global::System.Windows.Forms.ColumnHeader columnHeader2;

		// Token: 0x04000206 RID: 518
		private global::System.Windows.Forms.ColumnHeader columnHeader3;

		// Token: 0x04000207 RID: 519
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000208 RID: 520
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04000209 RID: 521
		private global::System.Windows.Forms.Button button2;

		// Token: 0x0400020A RID: 522
		private global::System.Windows.Forms.Button button3;

		// Token: 0x0400020B RID: 523
		private global::System.Windows.Forms.Button button4;

		// Token: 0x0400020C RID: 524
		private global::System.Windows.Forms.Button button5;
	}
}
