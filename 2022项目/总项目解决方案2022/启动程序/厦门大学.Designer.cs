namespace 启动程序
{
	// Token: 0x02000002 RID: 2
	public partial class 厦门大学 : global::System.Windows.Forms.Form
	{
		// Token: 0x06000009 RID: 9 RVA: 0x000026E4 File Offset: 0x000008E4
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000271C File Offset: 0x0000091C
		private void InitializeComponent()
		{
			this.splitContainer1 = new global::System.Windows.Forms.SplitContainer();
			this.button4 = new global::System.Windows.Forms.Button();
			this.button3 = new global::System.Windows.Forms.Button();
			this.button2 = new global::System.Windows.Forms.Button();
			this.button1 = new global::System.Windows.Forms.Button();
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
			this.columnHeader14 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader15 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader16 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader17 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader18 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader19 = new global::System.Windows.Forms.ColumnHeader();
			this.button5 = new global::System.Windows.Forms.Button();
			this.webBrowser1 = new global::System.Windows.Forms.WebBrowser();
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			base.SuspendLayout();
			this.splitContainer1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new global::System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = global::System.Windows.Forms.Orientation.Horizontal;
			this.splitContainer1.Panel1.Controls.Add(this.button5);
			this.splitContainer1.Panel1.Controls.Add(this.button4);
			this.splitContainer1.Panel1.Controls.Add(this.button3);
			this.splitContainer1.Panel1.Controls.Add(this.button2);
			this.splitContainer1.Panel1.Controls.Add(this.button1);
			this.splitContainer1.Panel2.Controls.Add(this.webBrowser1);
			this.splitContainer1.Panel2.Controls.Add(this.listView1);
			this.splitContainer1.Size = new global::System.Drawing.Size(1210, 587);
			this.splitContainer1.SplitterDistance = 148;
			this.splitContainer1.TabIndex = 1;
			this.button4.Location = new global::System.Drawing.Point(530, 87);
			this.button4.Name = "button4";
			this.button4.Size = new global::System.Drawing.Size(112, 30);
			this.button4.TabIndex = 40;
			this.button4.Text = "导出结果";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new global::System.EventHandler(this.button4_Click);
			this.button3.Location = new global::System.Drawing.Point(265, 87);
			this.button3.Name = "button3";
			this.button3.Size = new global::System.Drawing.Size(112, 30);
			this.button3.TabIndex = 37;
			this.button3.Text = "继续";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new global::System.EventHandler(this.button3_Click);
			this.button2.Location = new global::System.Drawing.Point(400, 87);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(112, 30);
			this.button2.TabIndex = 36;
			this.button2.Text = "暂停";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.button2_Click);
			this.button1.Location = new global::System.Drawing.Point(117, 87);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(112, 30);
			this.button1.TabIndex = 34;
			this.button1.Text = "开始运行";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.Button1_Click);
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
				this.columnHeader13,
				this.columnHeader14,
				this.columnHeader15,
				this.columnHeader16,
				this.columnHeader17,
				this.columnHeader18,
				this.columnHeader19
			});
			this.listView1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new global::System.Drawing.Point(0, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new global::System.Drawing.Size(1210, 435);
			this.listView1.TabIndex = 36;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = global::System.Windows.Forms.View.Details;
			this.columnHeader1.Text = "序号";
			this.columnHeader2.Text = "教工号";
			this.columnHeader3.Text = "出生省市";
			this.columnHeader4.Text = "姓名";
			this.columnHeader5.Text = "出生日期";
			this.columnHeader6.Text = "户口所在地";
			this.columnHeader7.Text = "所在学院/部门";
			this.columnHeader8.Text = "联系电话";
			this.columnHeader9.Text = "配偶姓名";
			this.columnHeader10.Text = "职务/职称";
			this.columnHeader11.Text = "邮箱";
			this.columnHeader12.Text = "配偶电话";
			this.columnHeader13.Text = "前往国家/地区";
			this.columnHeader14.Text = "主要访问路线";
			this.columnHeader15.Text = "邀请人";
			this.columnHeader16.Text = "启程日期";
			this.columnHeader17.Text = "抵达日期";
			this.columnHeader18.Text = "停留天数";
			this.columnHeader19.Text = "出访内容";
			this.button5.Font = new global::System.Drawing.Font("宋体", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.button5.ForeColor = global::System.Drawing.Color.Red;
			this.button5.Location = new global::System.Drawing.Point(117, 40);
			this.button5.Name = "button5";
			this.button5.Size = new global::System.Drawing.Size(112, 30);
			this.button5.TabIndex = 41;
			this.button5.Text = "登陆";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new global::System.EventHandler(this.button5_Click);
			this.webBrowser1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.webBrowser1.Location = new global::System.Drawing.Point(0, 0);
			this.webBrowser1.MinimumSize = new global::System.Drawing.Size(20, 20);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.Size = new global::System.Drawing.Size(1210, 435);
			this.webBrowser1.TabIndex = 37;
			this.webBrowser1.Visible = false;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(1210, 587);
			base.Controls.Add(this.splitContainer1);
			base.Name = "厦门大学";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "数据抓取";
			base.Load += new global::System.EventHandler(this.Form1_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x04000002 RID: 2
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000003 RID: 3
		private global::System.Windows.Forms.SplitContainer splitContainer1;

		// Token: 0x04000004 RID: 4
		private global::System.Windows.Forms.Button button3;

		// Token: 0x04000005 RID: 5
		private global::System.Windows.Forms.Button button2;

		// Token: 0x04000006 RID: 6
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04000007 RID: 7
		private global::System.Windows.Forms.ListView listView1;

		// Token: 0x04000008 RID: 8
		private global::System.Windows.Forms.ColumnHeader columnHeader1;

		// Token: 0x04000009 RID: 9
		private global::System.Windows.Forms.ColumnHeader columnHeader2;

		// Token: 0x0400000A RID: 10
		private global::System.Windows.Forms.ColumnHeader columnHeader3;

		// Token: 0x0400000B RID: 11
		private global::System.Windows.Forms.ColumnHeader columnHeader4;

		// Token: 0x0400000C RID: 12
		private global::System.Windows.Forms.ColumnHeader columnHeader5;

		// Token: 0x0400000D RID: 13
		private global::System.Windows.Forms.ColumnHeader columnHeader6;

		// Token: 0x0400000E RID: 14
		private global::System.Windows.Forms.ColumnHeader columnHeader7;

		// Token: 0x0400000F RID: 15
		private global::System.Windows.Forms.ColumnHeader columnHeader8;

		// Token: 0x04000010 RID: 16
		private global::System.Windows.Forms.ColumnHeader columnHeader9;

		// Token: 0x04000011 RID: 17
		private global::System.Windows.Forms.ColumnHeader columnHeader10;

		// Token: 0x04000012 RID: 18
		private global::System.Windows.Forms.ColumnHeader columnHeader11;

		// Token: 0x04000013 RID: 19
		private global::System.Windows.Forms.ColumnHeader columnHeader12;

		// Token: 0x04000014 RID: 20
		private global::System.Windows.Forms.ColumnHeader columnHeader13;

		// Token: 0x04000015 RID: 21
		private global::System.Windows.Forms.ColumnHeader columnHeader14;

		// Token: 0x04000016 RID: 22
		private global::System.Windows.Forms.ColumnHeader columnHeader15;

		// Token: 0x04000017 RID: 23
		private global::System.Windows.Forms.ColumnHeader columnHeader16;

		// Token: 0x04000018 RID: 24
		private global::System.Windows.Forms.ColumnHeader columnHeader17;

		// Token: 0x04000019 RID: 25
		private global::System.Windows.Forms.ColumnHeader columnHeader18;

		// Token: 0x0400001A RID: 26
		private global::System.Windows.Forms.ColumnHeader columnHeader19;

		// Token: 0x0400001B RID: 27
		private global::System.Windows.Forms.Button button4;

		// Token: 0x0400001C RID: 28
		private global::System.Windows.Forms.Button button5;

		// Token: 0x0400001D RID: 29
		private global::System.Windows.Forms.WebBrowser webBrowser1;
	}
}
