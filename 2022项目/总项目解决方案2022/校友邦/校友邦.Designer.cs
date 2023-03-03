namespace 校友邦
{
	// Token: 0x02000006 RID: 6
	public partial class 校友邦 : global::System.Windows.Forms.Form
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00005AE0 File Offset: 0x00003CE0
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00005B18 File Offset: 0x00003D18
		private void InitializeComponent()
		{
			this.button1 = new global::System.Windows.Forms.Button();
			this.button2 = new global::System.Windows.Forms.Button();
			this.button3 = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.button4 = new global::System.Windows.Forms.Button();
			this.button5 = new global::System.Windows.Forms.Button();
			this.button6 = new global::System.Windows.Forms.Button();
			this.linkLabel1 = new global::System.Windows.Forms.LinkLabel();
			this.linkLabel2 = new global::System.Windows.Forms.LinkLabel();
			this.webBrowser1 = new global::System.Windows.Forms.WebBrowser();
			this.listView1 = new global::System.Windows.Forms.ListView();
			this.columnHeader1 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new global::System.Windows.Forms.ColumnHeader();
			base.SuspendLayout();
			this.button1.Location = new global::System.Drawing.Point(96, 50);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(88, 31);
			this.button1.TabIndex = 0;
			this.button1.Text = "导入账号";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.button2.Location = new global::System.Drawing.Point(202, 50);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(88, 31);
			this.button2.TabIndex = 193;
			this.button2.Text = "开始签到";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.button2_Click);
			this.button3.Location = new global::System.Drawing.Point(636, 50);
			this.button3.Name = "button3";
			this.button3.Size = new global::System.Drawing.Size(88, 31);
			this.button3.TabIndex = 194;
			this.button3.Text = "暂停/继续";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new global::System.EventHandler(this.button3_Click);
			this.label1.AutoSize = true;
			this.label1.Font = new global::System.Drawing.Font("宋体", 10f);
			this.label1.Location = new global::System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(77, 14);
			this.label1.TabIndex = 195;
			this.label1.Text = "文本路径：";
			this.textBox1.Font = new global::System.Drawing.Font("宋体", 10f);
			this.textBox1.Location = new global::System.Drawing.Point(96, 10);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new global::System.Drawing.Size(400, 23);
			this.textBox1.TabIndex = 196;
			this.button4.Location = new global::System.Drawing.Point(310, 50);
			this.button4.Name = "button4";
			this.button4.Size = new global::System.Drawing.Size(88, 31);
			this.button4.TabIndex = 197;
			this.button4.Text = "重新开始签到";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new global::System.EventHandler(this.button4_Click);
			this.button5.Location = new global::System.Drawing.Point(417, 50);
			this.button5.Name = "button5";
			this.button5.Size = new global::System.Drawing.Size(88, 31);
			this.button5.TabIndex = 198;
			this.button5.Text = "结束签到";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new global::System.EventHandler(this.button5_Click);
			this.button6.Location = new global::System.Drawing.Point(526, 50);
			this.button6.Name = "button6";
			this.button6.Size = new global::System.Drawing.Size(88, 31);
			this.button6.TabIndex = 199;
			this.button6.Text = "重新结束签到";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new global::System.EventHandler(this.button6_Click);
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Font = new global::System.Drawing.Font("宋体", 10f);
			this.linkLabel1.Location = new global::System.Drawing.Point(96, 97);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new global::System.Drawing.Size(35, 14);
			this.linkLabel1.TabIndex = 200;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "全选";
			this.linkLabel1.LinkClicked += new global::System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			this.linkLabel2.AutoSize = true;
			this.linkLabel2.Font = new global::System.Drawing.Font("宋体", 10f);
			this.linkLabel2.Location = new global::System.Drawing.Point(144, 97);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new global::System.Drawing.Size(49, 14);
			this.linkLabel2.TabIndex = 201;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "全不选";
			this.linkLabel2.LinkClicked += new global::System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
			this.webBrowser1.Location = new global::System.Drawing.Point(12, 143);
			this.webBrowser1.MinimumSize = new global::System.Drawing.Size(20, 20);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.Size = new global::System.Drawing.Size(746, 342);
			this.webBrowser1.TabIndex = 202;
			this.listView1.CheckBoxes = true;
			this.listView1.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.columnHeader1,
				this.columnHeader2,
				this.columnHeader3,
				this.columnHeader5,
				this.columnHeader6,
				this.columnHeader4,
				this.columnHeader7
			});
			this.listView1.Dock = global::System.Windows.Forms.DockStyle.Bottom;
			this.listView1.Font = new global::System.Drawing.Font("宋体", 10f);
			this.listView1.ForeColor = global::System.Drawing.Color.White;
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new global::System.Drawing.Point(0, 134);
			this.listView1.Name = "listView1";
			this.listView1.Size = new global::System.Drawing.Size(762, 388);
			this.listView1.TabIndex = 203;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = global::System.Windows.Forms.View.Details;
			this.columnHeader1.Text = "序号";
			this.columnHeader1.Width = 50;
			this.columnHeader2.Text = "账号";
			this.columnHeader2.Width = 100;
			this.columnHeader3.Text = "密码";
			this.columnHeader3.Width = 80;
			this.columnHeader5.Text = "地址";
			this.columnHeader5.Width = 200;
			this.columnHeader6.Text = "次数";
			this.columnHeader6.Width = 100;
			this.columnHeader4.Text = "时间";
			this.columnHeader4.Width = 100;
			this.columnHeader7.Text = "结果";
			this.columnHeader7.Width = 100;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(762, 522);
			base.Controls.Add(this.listView1);
			base.Controls.Add(this.webBrowser1);
			base.Controls.Add(this.linkLabel2);
			base.Controls.Add(this.linkLabel1);
			base.Controls.Add(this.button6);
			base.Controls.Add(this.button5);
			base.Controls.Add(this.button4);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.button3);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.button1);
			base.Name = "校友邦";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "一只丸助手";
			base.Load += new global::System.EventHandler(this.校友邦_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000028 RID: 40
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000029 RID: 41
		private global::System.Windows.Forms.Button button1;

		// Token: 0x0400002A RID: 42
		private global::System.Windows.Forms.Button button2;

		// Token: 0x0400002B RID: 43
		private global::System.Windows.Forms.Button button3;

		// Token: 0x0400002C RID: 44
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400002D RID: 45
		private global::System.Windows.Forms.TextBox textBox1;

		// Token: 0x0400002E RID: 46
		private global::System.Windows.Forms.Button button4;

		// Token: 0x0400002F RID: 47
		private global::System.Windows.Forms.Button button5;

		// Token: 0x04000030 RID: 48
		private global::System.Windows.Forms.Button button6;

		// Token: 0x04000031 RID: 49
		private global::System.Windows.Forms.LinkLabel linkLabel1;

		// Token: 0x04000032 RID: 50
		private global::System.Windows.Forms.LinkLabel linkLabel2;

		// Token: 0x04000033 RID: 51
		private global::System.Windows.Forms.WebBrowser webBrowser1;

		// Token: 0x04000034 RID: 52
		private global::System.Windows.Forms.ListView listView1;

		// Token: 0x04000035 RID: 53
		private global::System.Windows.Forms.ColumnHeader columnHeader1;

		// Token: 0x04000036 RID: 54
		private global::System.Windows.Forms.ColumnHeader columnHeader2;

		// Token: 0x04000037 RID: 55
		private global::System.Windows.Forms.ColumnHeader columnHeader3;

		// Token: 0x04000038 RID: 56
		private global::System.Windows.Forms.ColumnHeader columnHeader5;

		// Token: 0x04000039 RID: 57
		private global::System.Windows.Forms.ColumnHeader columnHeader6;

		// Token: 0x0400003A RID: 58
		private global::System.Windows.Forms.ColumnHeader columnHeader4;

		// Token: 0x0400003B RID: 59
		private global::System.Windows.Forms.ColumnHeader columnHeader7;
	}
}
