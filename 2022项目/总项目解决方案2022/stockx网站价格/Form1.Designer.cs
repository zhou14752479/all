namespace stockx网站价格
{
	// Token: 0x02000002 RID: 2
	public partial class Form1 : global::System.Windows.Forms.Form
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00002AFC File Offset: 0x00000CFC
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002B34 File Offset: 0x00000D34
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			this.splitContainer1 = new global::System.Windows.Forms.SplitContainer();
			this.linkLabel1 = new global::System.Windows.Forms.LinkLabel();
			this.button2 = new global::System.Windows.Forms.Button();
			this.label2 = new global::System.Windows.Forms.Label();
			this.textBox2 = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.button1 = new global::System.Windows.Forms.Button();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.listView1 = new global::System.Windows.Forms.ListView();
			this.columnHeader2 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new global::System.Windows.Forms.ColumnHeader();
			this.openFileDialog1 = new global::System.Windows.Forms.OpenFileDialog();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			base.SuspendLayout();
			this.splitContainer1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new global::System.Drawing.Point(0, 0);
			this.splitContainer1.Margin = new global::System.Windows.Forms.Padding(4);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.Controls.Add(this.linkLabel1);
			this.splitContainer1.Panel1.Controls.Add(this.button2);
			this.splitContainer1.Panel1.Controls.Add(this.label2);
			this.splitContainer1.Panel1.Controls.Add(this.textBox2);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.button1);
			this.splitContainer1.Panel1.Controls.Add(this.textBox1);
			this.splitContainer1.Panel2.Controls.Add(this.listView1);
			this.splitContainer1.Size = new global::System.Drawing.Size(1268, 780);
			this.splitContainer1.SplitterDistance = 303;
			this.splitContainer1.SplitterWidth = 5;
			this.splitContainer1.TabIndex = 2;
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new global::System.Drawing.Point(200, 153);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new global::System.Drawing.Size(67, 15);
			this.linkLabel1.TabIndex = 23;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "刷新汇率";
			this.linkLabel1.LinkClicked += new global::System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
			this.button2.Font = new global::System.Drawing.Font("宋体", 11f);
			this.button2.Location = new global::System.Drawing.Point(95, 288);
			this.button2.Margin = new global::System.Windows.Forms.Padding(4);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(192, 38);
			this.button2.TabIndex = 22;
			this.button2.Text = "清空";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.Button2_Click);
			this.label2.AutoSize = true;
			this.label2.Font = new global::System.Drawing.Font("宋体", 12f);
			this.label2.Location = new global::System.Drawing.Point(12, 106);
			this.label2.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(69, 20);
			this.label2.TabIndex = 21;
			this.label2.Text = "汇率：";
			this.textBox2.Font = new global::System.Drawing.Font("宋体", 12f);
			this.textBox2.Location = new global::System.Drawing.Point(95, 106);
			this.textBox2.Margin = new global::System.Windows.Forms.Padding(4);
			this.textBox2.Name = "textBox2";
			this.textBox2.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this.textBox2.Size = new global::System.Drawing.Size(191, 30);
			this.textBox2.TabIndex = 20;
			this.textBox2.Text = "7.0813";
			this.label1.AutoSize = true;
			this.label1.Font = new global::System.Drawing.Font("宋体", 12f);
			this.label1.Location = new global::System.Drawing.Point(12, 44);
			this.label1.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(69, 20);
			this.label1.TabIndex = 19;
			this.label1.Text = "货号：";
			this.button1.Font = new global::System.Drawing.Font("宋体", 11f);
			this.button1.Location = new global::System.Drawing.Point(95, 231);
			this.button1.Margin = new global::System.Windows.Forms.Padding(4);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(192, 38);
			this.button1.TabIndex = 16;
			this.button1.Text = "点击获取";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.textBox1.Font = new global::System.Drawing.Font("宋体", 12f);
			this.textBox1.Location = new global::System.Drawing.Point(95, 40);
			this.textBox1.Margin = new global::System.Windows.Forms.Padding(4);
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new global::System.Drawing.Size(191, 30);
			this.textBox1.TabIndex = 14;
			this.textBox1.Text = "CD4991-100";
			this.listView1.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.columnHeader2,
				this.columnHeader4,
				this.columnHeader5
			});
			this.listView1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.listView1.Font = new global::System.Drawing.Font("微软雅黑", 15f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new global::System.Drawing.Point(0, 0);
			this.listView1.Margin = new global::System.Windows.Forms.Padding(4);
			this.listView1.Name = "listView1";
			this.listView1.Size = new global::System.Drawing.Size(960, 780);
			this.listView1.TabIndex = 26;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = global::System.Windows.Forms.View.Details;
			this.columnHeader2.Text = "尺码";
			this.columnHeader2.Width = 100;
			this.columnHeader4.Text = "lowest总价";
			this.columnHeader4.Width = 300;
			this.columnHeader5.Text = "highest总价";
			this.columnHeader5.Width = 300;
			this.openFileDialog1.FileName = "openFileDialog1";
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(8f, 15f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(1268, 780);
			base.Controls.Add(this.splitContainer1);
			base.Margin = new global::System.Windows.Forms.Padding(4);
			base.Name = "Form1";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "stockx网站实时价格";
			base.Load += new global::System.EventHandler(this.Form1_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x04000003 RID: 3
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000004 RID: 4
		private global::System.Windows.Forms.SplitContainer splitContainer1;

		// Token: 0x04000005 RID: 5
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04000006 RID: 6
		private global::System.Windows.Forms.TextBox textBox1;

		// Token: 0x04000007 RID: 7
		private global::System.Windows.Forms.OpenFileDialog openFileDialog1;

		// Token: 0x04000008 RID: 8
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x04000009 RID: 9
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400000A RID: 10
		private global::System.Windows.Forms.ListView listView1;

		// Token: 0x0400000B RID: 11
		private global::System.Windows.Forms.ColumnHeader columnHeader2;

		// Token: 0x0400000C RID: 12
		private global::System.Windows.Forms.Label label2;

		// Token: 0x0400000D RID: 13
		private global::System.Windows.Forms.TextBox textBox2;

		// Token: 0x0400000E RID: 14
		private global::System.Windows.Forms.ColumnHeader columnHeader4;

		// Token: 0x0400000F RID: 15
		private global::System.Windows.Forms.ColumnHeader columnHeader5;

		// Token: 0x04000010 RID: 16
		private global::System.Windows.Forms.Button button2;

		// Token: 0x04000011 RID: 17
		private global::System.Windows.Forms.LinkLabel linkLabel1;
	}
}
