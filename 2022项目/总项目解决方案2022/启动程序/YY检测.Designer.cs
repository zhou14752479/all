namespace 启动程序
{
	// Token: 0x02000006 RID: 6
	public partial class YY检测 : global::System.Windows.Forms.Form
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00004760 File Offset: 0x00002960
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00004798 File Offset: 0x00002998
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			this.openFileDialog1 = new global::System.Windows.Forms.OpenFileDialog();
			this.button5 = new global::System.Windows.Forms.Button();
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
			this.button3 = new global::System.Windows.Forms.Button();
			this.button2 = new global::System.Windows.Forms.Button();
			this.button4 = new global::System.Windows.Forms.Button();
			this.button6 = new global::System.Windows.Forms.Button();
			this.button7 = new global::System.Windows.Forms.Button();
			this.textBox2 = new global::System.Windows.Forms.TextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.contextMenuStrip1.SuspendLayout();
			base.SuspendLayout();
			this.openFileDialog1.FileName = "openFileDialog1";
			this.button5.Location = new global::System.Drawing.Point(12, 405);
			this.button5.Name = "button5";
			this.button5.Size = new global::System.Drawing.Size(114, 29);
			this.button5.TabIndex = 38;
			this.button5.Text = "清空";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new global::System.EventHandler(this.button5_Click);
			this.textBox1.Location = new global::System.Drawing.Point(12, 268);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new global::System.Drawing.Size(320, 22);
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
			this.listView1.Size = new global::System.Drawing.Size(581, 249);
			this.listView1.TabIndex = 34;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = global::System.Windows.Forms.View.Details;
			this.columnHeader1.Text = "序号";
			this.columnHeader1.Width = 40;
			this.columnHeader2.Text = "账号";
			this.columnHeader2.Width = 300;
			this.columnHeader3.Text = "类型";
			this.columnHeader3.Width = 100;
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(12, 461);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(41, 12);
			this.label1.TabIndex = 40;
			this.label1.Text = "未开始";
			this.button1.Location = new global::System.Drawing.Point(338, 268);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(62, 23);
			this.button1.TabIndex = 39;
			this.button1.Text = "导入";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.button3.Location = new global::System.Drawing.Point(12, 369);
			this.button3.Name = "button3";
			this.button3.Size = new global::System.Drawing.Size(114, 30);
			this.button3.TabIndex = 36;
			this.button3.Text = "暂停";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new global::System.EventHandler(this.button3_Click);
			this.button2.Location = new global::System.Drawing.Point(12, 319);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(114, 29);
			this.button2.TabIndex = 35;
			this.button2.Text = "开始";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.button2_Click);
			this.button4.Location = new global::System.Drawing.Point(132, 370);
			this.button4.Name = "button4";
			this.button4.Size = new global::System.Drawing.Size(114, 29);
			this.button4.TabIndex = 41;
			this.button4.Text = "继续";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new global::System.EventHandler(this.button4_Click);
			this.button6.Location = new global::System.Drawing.Point(132, 405);
			this.button6.Name = "button6";
			this.button6.Size = new global::System.Drawing.Size(114, 29);
			this.button6.TabIndex = 42;
			this.button6.Text = "导出结果";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new global::System.EventHandler(this.button6_Click);
			this.button7.Location = new global::System.Drawing.Point(252, 405);
			this.button7.Name = "button7";
			this.button7.Size = new global::System.Drawing.Size(114, 29);
			this.button7.TabIndex = 43;
			this.button7.Text = "恢复开始";
			this.button7.UseVisualStyleBackColor = true;
			this.button7.Click += new global::System.EventHandler(this.button7_Click);
			this.textBox2.Location = new global::System.Drawing.Point(191, 326);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this.textBox2.Size = new global::System.Drawing.Size(355, 22);
			this.textBox2.TabIndex = 44;
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(132, 329);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(53, 12);
			this.label2.TabIndex = 45;
			this.label2.Text = "代理IP：";
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(581, 488);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.textBox2);
			base.Controls.Add(this.button7);
			base.Controls.Add(this.button6);
			base.Controls.Add(this.button4);
			base.Controls.Add(this.button5);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.listView1);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.button3);
			base.Controls.Add(this.button2);
			base.Name = "YY检测";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "YY检测";
			base.Load += new global::System.EventHandler(this.YY检测_Load);
			this.contextMenuStrip1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400003D RID: 61
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x0400003E RID: 62
		private global::System.Windows.Forms.OpenFileDialog openFileDialog1;

		// Token: 0x0400003F RID: 63
		private global::System.Windows.Forms.Button button5;

		// Token: 0x04000040 RID: 64
		private global::System.Windows.Forms.TextBox textBox1;

		// Token: 0x04000041 RID: 65
		private global::System.Windows.Forms.ToolStripMenuItem 重新扫描ToolStripMenuItem;

		// Token: 0x04000042 RID: 66
		private global::System.Windows.Forms.ToolStripMenuItem 复制串码ToolStripMenuItem;

		// Token: 0x04000043 RID: 67
		private global::System.Windows.Forms.ToolStripMenuItem 复制网址ToolStripMenuItem;

		// Token: 0x04000044 RID: 68
		private global::System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		// Token: 0x04000045 RID: 69
		private global::System.Windows.Forms.ListView listView1;

		// Token: 0x04000046 RID: 70
		private global::System.Windows.Forms.ColumnHeader columnHeader1;

		// Token: 0x04000047 RID: 71
		private global::System.Windows.Forms.ColumnHeader columnHeader2;

		// Token: 0x04000048 RID: 72
		private global::System.Windows.Forms.ColumnHeader columnHeader3;

		// Token: 0x04000049 RID: 73
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400004A RID: 74
		private global::System.Windows.Forms.Button button1;

		// Token: 0x0400004B RID: 75
		private global::System.Windows.Forms.Button button3;

		// Token: 0x0400004C RID: 76
		private global::System.Windows.Forms.Button button2;

		// Token: 0x0400004D RID: 77
		private global::System.Windows.Forms.Button button4;

		// Token: 0x0400004E RID: 78
		private global::System.Windows.Forms.Button button6;

		// Token: 0x0400004F RID: 79
		private global::System.Windows.Forms.Button button7;

		// Token: 0x04000050 RID: 80
		private global::System.Windows.Forms.TextBox textBox2;

		// Token: 0x04000051 RID: 81
		private global::System.Windows.Forms.Label label2;
	}
}
