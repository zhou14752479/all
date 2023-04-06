namespace 启动程序
{
	// Token: 0x02000015 RID: 21
	public partial class 短标题采集 : global::System.Windows.Forms.Form
	{
		// Token: 0x060000CC RID: 204 RVA: 0x00019444 File Offset: 0x00017644
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0001947C File Offset: 0x0001767C
		private void InitializeComponent()
		{
			this.button5 = new global::System.Windows.Forms.Button();
			this.splitContainer1 = new global::System.Windows.Forms.SplitContainer();
			this.button4 = new global::System.Windows.Forms.Button();
			this.button3 = new global::System.Windows.Forms.Button();
			this.button2 = new global::System.Windows.Forms.Button();
			this.button1 = new global::System.Windows.Forms.Button();
			this.listView1 = new global::System.Windows.Forms.ListView();
			this.columnHeader1 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new global::System.Windows.Forms.ColumnHeader();
			this.button6 = new global::System.Windows.Forms.Button();
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			base.SuspendLayout();
			this.button5.Location = new global::System.Drawing.Point(19, 104);
			this.button5.Name = "button5";
			this.button5.Size = new global::System.Drawing.Size(112, 30);
			this.button5.TabIndex = 40;
			this.button5.Text = "导出";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new global::System.EventHandler(this.button5_Click);
			this.splitContainer1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new global::System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = global::System.Windows.Forms.Orientation.Horizontal;
			this.splitContainer1.Panel1.Controls.Add(this.button6);
			this.splitContainer1.Panel1.Controls.Add(this.button5);
			this.splitContainer1.Panel1.Controls.Add(this.button4);
			this.splitContainer1.Panel1.Controls.Add(this.button3);
			this.splitContainer1.Panel1.Controls.Add(this.button2);
			this.splitContainer1.Panel1.Controls.Add(this.button1);
			this.splitContainer1.Panel2.Controls.Add(this.listView1);
			this.splitContainer1.Size = new global::System.Drawing.Size(284, 459);
			this.splitContainer1.SplitterDistance = 151;
			this.splitContainer1.TabIndex = 3;
			this.button4.Location = new global::System.Drawing.Point(154, 68);
			this.button4.Name = "button4";
			this.button4.Size = new global::System.Drawing.Size(112, 30);
			this.button4.TabIndex = 38;
			this.button4.Text = "停止";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new global::System.EventHandler(this.button4_Click);
			this.button3.Location = new global::System.Drawing.Point(19, 68);
			this.button3.Name = "button3";
			this.button3.Size = new global::System.Drawing.Size(112, 30);
			this.button3.TabIndex = 37;
			this.button3.Text = "继续";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new global::System.EventHandler(this.button3_Click);
			this.button2.Location = new global::System.Drawing.Point(154, 32);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(112, 30);
			this.button2.TabIndex = 36;
			this.button2.Text = "暂停";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.button2_Click);
			this.button1.Location = new global::System.Drawing.Point(19, 32);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(112, 30);
			this.button1.TabIndex = 34;
			this.button1.Text = "开始运行";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.listView1.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.columnHeader1,
				this.columnHeader2
			});
			this.listView1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new global::System.Drawing.Point(0, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new global::System.Drawing.Size(284, 304);
			this.listView1.TabIndex = 36;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = global::System.Windows.Forms.View.Details;
			this.columnHeader1.Text = "序号";
			this.columnHeader2.Text = "标题";
			this.columnHeader2.Width = 200;
			this.button6.Location = new global::System.Drawing.Point(154, 104);
			this.button6.Name = "button6";
			this.button6.Size = new global::System.Drawing.Size(112, 30);
			this.button6.TabIndex = 41;
			this.button6.Text = "清空";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new global::System.EventHandler(this.button6_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(284, 459);
			base.Controls.Add(this.splitContainer1);
			base.Name = "短标题采集";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "短标题采集";
			base.Load += new global::System.EventHandler(this.短标题采集_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x0400020F RID: 527
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000210 RID: 528
		private global::System.Windows.Forms.Button button5;

		// Token: 0x04000211 RID: 529
		private global::System.Windows.Forms.SplitContainer splitContainer1;

		// Token: 0x04000212 RID: 530
		private global::System.Windows.Forms.Button button4;

		// Token: 0x04000213 RID: 531
		private global::System.Windows.Forms.Button button3;

		// Token: 0x04000214 RID: 532
		private global::System.Windows.Forms.Button button2;

		// Token: 0x04000215 RID: 533
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04000216 RID: 534
		private global::System.Windows.Forms.ListView listView1;

		// Token: 0x04000217 RID: 535
		private global::System.Windows.Forms.ColumnHeader columnHeader1;

		// Token: 0x04000218 RID: 536
		private global::System.Windows.Forms.ColumnHeader columnHeader2;

		// Token: 0x04000219 RID: 537
		private global::System.Windows.Forms.Button button6;
	}
}
