namespace 启动程序
{
	// Token: 0x02000019 RID: 25
	public partial class 群抓取1 : global::System.Windows.Forms.Form
	{
		// Token: 0x06000108 RID: 264 RVA: 0x0001FE34 File Offset: 0x0001E034
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0001FE6C File Offset: 0x0001E06C
		private void InitializeComponent()
		{
			this.columnHeader1 = new global::System.Windows.Forms.ColumnHeader();
			this.listView1 = new global::System.Windows.Forms.ListView();
			this.columnHeader2 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new global::System.Windows.Forms.ColumnHeader();
			this.splitContainer1 = new global::System.Windows.Forms.SplitContainer();
			this.button6 = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.button4 = new global::System.Windows.Forms.Button();
			this.button5 = new global::System.Windows.Forms.Button();
			this.button3 = new global::System.Windows.Forms.Button();
			this.button2 = new global::System.Windows.Forms.Button();
			this.button1 = new global::System.Windows.Forms.Button();
			this.radioButton1 = new global::System.Windows.Forms.RadioButton();
			this.radioButton2 = new global::System.Windows.Forms.RadioButton();
			this.radioButton3 = new global::System.Windows.Forms.RadioButton();
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			base.SuspendLayout();
			this.columnHeader1.Text = "序号";
			this.columnHeader1.Width = 40;
			this.listView1.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.columnHeader1,
				this.columnHeader2,
				this.columnHeader3
			});
			this.listView1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new global::System.Drawing.Point(0, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new global::System.Drawing.Size(398, 368);
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
			this.splitContainer1.Orientation = global::System.Windows.Forms.Orientation.Horizontal;
			this.splitContainer1.Panel1.Controls.Add(this.radioButton3);
			this.splitContainer1.Panel1.Controls.Add(this.radioButton2);
			this.splitContainer1.Panel1.Controls.Add(this.radioButton1);
			this.splitContainer1.Panel1.Controls.Add(this.button6);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.button4);
			this.splitContainer1.Panel1.Controls.Add(this.button5);
			this.splitContainer1.Panel1.Controls.Add(this.button3);
			this.splitContainer1.Panel1.Controls.Add(this.button2);
			this.splitContainer1.Panel1.Controls.Add(this.button1);
			this.splitContainer1.Panel1.Paint += new global::System.Windows.Forms.PaintEventHandler(this.SplitContainer1_Panel1_Paint);
			this.splitContainer1.Panel2.Controls.Add(this.listView1);
			this.splitContainer1.Size = new global::System.Drawing.Size(398, 513);
			this.splitContainer1.SplitterDistance = 141;
			this.splitContainer1.TabIndex = 5;
			this.button6.Location = new global::System.Drawing.Point(176, 78);
			this.button6.Name = "button6";
			this.button6.Size = new global::System.Drawing.Size(170, 30);
			this.button6.TabIndex = 44;
			this.button6.Text = "清空记录";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new global::System.EventHandler(this.button6_Click);
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(9, 111);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(41, 12);
			this.label1.TabIndex = 43;
			this.label1.Text = "未下载";
			this.button4.Enabled = false;
			this.button4.Location = new global::System.Drawing.Point(7, 78);
			this.button4.Name = "button4";
			this.button4.Size = new global::System.Drawing.Size(163, 30);
			this.button4.TabIndex = 42;
			this.button4.Text = "一键下载";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new global::System.EventHandler(this.button4_Click);
			this.button5.Location = new global::System.Drawing.Point(264, 42);
			this.button5.Name = "button5";
			this.button5.Size = new global::System.Drawing.Size(82, 30);
			this.button5.TabIndex = 40;
			this.button5.Text = "导出数据";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new global::System.EventHandler(this.button5_Click);
			this.button3.Location = new global::System.Drawing.Point(176, 42);
			this.button3.Name = "button3";
			this.button3.Size = new global::System.Drawing.Size(82, 30);
			this.button3.TabIndex = 36;
			this.button3.Text = "继续";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new global::System.EventHandler(this.button3_Click);
			this.button2.Location = new global::System.Drawing.Point(88, 42);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(82, 30);
			this.button2.TabIndex = 35;
			this.button2.Text = "暂停";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.button2_Click);
			this.button1.Location = new global::System.Drawing.Point(7, 42);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(75, 30);
			this.button1.TabIndex = 34;
			this.button1.Text = "开始抓取";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.radioButton1.AutoSize = true;
			this.radioButton1.Checked = true;
			this.radioButton1.Font = new global::System.Drawing.Font("宋体", 11f);
			this.radioButton1.Location = new global::System.Drawing.Point(13, 12);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new global::System.Drawing.Size(85, 19);
			this.radioButton1.TabIndex = 45;
			this.radioButton1.TabStop = true;
			this.radioButton1.Text = "当天发布";
			this.radioButton1.UseVisualStyleBackColor = true;
			this.radioButton2.AutoSize = true;
			this.radioButton2.Font = new global::System.Drawing.Font("宋体", 11f);
			this.radioButton2.Location = new global::System.Drawing.Point(104, 12);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new global::System.Drawing.Size(85, 19);
			this.radioButton2.TabIndex = 46;
			this.radioButton2.TabStop = true;
			this.radioButton2.Text = "昨天发布";
			this.radioButton2.UseVisualStyleBackColor = true;
			this.radioButton3.AutoSize = true;
			this.radioButton3.Font = new global::System.Drawing.Font("宋体", 11f);
			this.radioButton3.Location = new global::System.Drawing.Point(195, 12);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.Size = new global::System.Drawing.Size(85, 19);
			this.radioButton3.TabIndex = 47;
			this.radioButton3.TabStop = true;
			this.radioButton3.Text = "更早发布";
			this.radioButton3.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(398, 513);
			base.Controls.Add(this.splitContainer1);
			base.Name = "群抓取1";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "群抓取1";
			base.Load += new global::System.EventHandler(this.群抓取1_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x040002A0 RID: 672
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x040002A1 RID: 673
		private global::System.Windows.Forms.ColumnHeader columnHeader1;

		// Token: 0x040002A2 RID: 674
		private global::System.Windows.Forms.ListView listView1;

		// Token: 0x040002A3 RID: 675
		private global::System.Windows.Forms.ColumnHeader columnHeader2;

		// Token: 0x040002A4 RID: 676
		private global::System.Windows.Forms.ColumnHeader columnHeader3;

		// Token: 0x040002A5 RID: 677
		private global::System.Windows.Forms.SplitContainer splitContainer1;

		// Token: 0x040002A6 RID: 678
		private global::System.Windows.Forms.Button button6;

		// Token: 0x040002A7 RID: 679
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040002A8 RID: 680
		private global::System.Windows.Forms.Button button4;

		// Token: 0x040002A9 RID: 681
		private global::System.Windows.Forms.Button button5;

		// Token: 0x040002AA RID: 682
		private global::System.Windows.Forms.Button button3;

		// Token: 0x040002AB RID: 683
		private global::System.Windows.Forms.Button button2;

		// Token: 0x040002AC RID: 684
		private global::System.Windows.Forms.Button button1;

		// Token: 0x040002AD RID: 685
		private global::System.Windows.Forms.RadioButton radioButton3;

		// Token: 0x040002AE RID: 686
		private global::System.Windows.Forms.RadioButton radioButton2;

		// Token: 0x040002AF RID: 687
		private global::System.Windows.Forms.RadioButton radioButton1;
	}
}
