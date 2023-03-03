namespace 校友邦
{
	// Token: 0x02000005 RID: 5
	public partial class 校友邦_定时签到 : global::System.Windows.Forms.Form
	{
		// Token: 0x0600001B RID: 27 RVA: 0x00003B28 File Offset: 0x00001D28
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00003B60 File Offset: 0x00001D60
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			this.button1 = new global::System.Windows.Forms.Button();
			this.button2 = new global::System.Windows.Forms.Button();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.textBox2 = new global::System.Windows.Forms.TextBox();
			this.linkLabel1 = new global::System.Windows.Forms.LinkLabel();
			this.label3 = new global::System.Windows.Forms.Label();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label1 = new global::System.Windows.Forms.Label();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.tabControl1 = new global::System.Windows.Forms.TabControl();
			this.tabPage1 = new global::System.Windows.Forms.TabPage();
			this.tabPage2 = new global::System.Windows.Forms.TabPage();
			this.listView1 = new global::System.Windows.Forms.ListView();
			this.columnHeader1 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader10 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader9 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader11 = new global::System.Windows.Forms.ColumnHeader();
			this.webBrowser1 = new global::System.Windows.Forms.WebBrowser();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			base.SuspendLayout();
			this.button1.Font = new global::System.Drawing.Font("宋体", 10f);
			this.button1.Location = new global::System.Drawing.Point(638, 24);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(126, 46);
			this.button1.TabIndex = 2;
			this.button1.Text = "开启定时签到";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.button2.Font = new global::System.Drawing.Font("宋体", 10f);
			this.button2.Location = new global::System.Drawing.Point(795, 24);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(126, 46);
			this.button2.TabIndex = 3;
			this.button2.Text = "停止定时签到";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.button2_Click);
			this.panel1.Controls.Add(this.textBox2);
			this.panel1.Controls.Add(this.linkLabel1);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.textBox1);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.button2);
			this.panel1.Controls.Add(this.button1);
			this.panel1.Dock = global::System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new global::System.Drawing.Point(0, 473);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(1145, 100);
			this.panel1.TabIndex = 4;
			this.textBox2.Dock = global::System.Windows.Forms.DockStyle.Right;
			this.textBox2.Location = new global::System.Drawing.Point(1005, 0);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.ScrollBars = global::System.Windows.Forms.ScrollBars.Both;
			this.textBox2.Size = new global::System.Drawing.Size(140, 100);
			this.textBox2.TabIndex = 9;
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new global::System.Drawing.Point(236, 27);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new global::System.Drawing.Size(77, 12);
			this.linkLabel1.TabIndex = 8;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "删除过期账号";
			this.linkLabel1.LinkClicked += new global::System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			this.label3.AutoSize = true;
			this.label3.Location = new global::System.Drawing.Point(172, 27);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(29, 12);
			this.label3.TabIndex = 7;
			this.label3.Text = "分钟";
			this.textBox1.Location = new global::System.Drawing.Point(103, 24);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new global::System.Drawing.Size(63, 21);
			this.textBox1.TabIndex = 6;
			this.textBox1.Text = "2";
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(32, 27);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(65, 12);
			this.label2.TabIndex = 5;
			this.label2.Text = "监控频率：";
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(43, 58);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(41, 12);
			this.label1.TabIndex = 4;
			this.label1.Text = "未启动";
			this.groupBox1.Controls.Add(this.tabControl1);
			this.groupBox1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new global::System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new global::System.Drawing.Size(1145, 473);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "账号信息";
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new global::System.Drawing.Point(3, 17);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new global::System.Drawing.Size(1139, 453);
			this.tabControl1.TabIndex = 0;
			this.tabPage1.Controls.Add(this.listView1);
			this.tabPage1.Location = new global::System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new global::System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new global::System.Drawing.Size(1131, 427);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "数据页面";
			this.tabPage1.UseVisualStyleBackColor = true;
			this.tabPage2.Controls.Add(this.webBrowser1);
			this.tabPage2.Location = new global::System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new global::System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new global::System.Drawing.Size(1131, 427);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "IE浏览器";
			this.tabPage2.UseVisualStyleBackColor = true;
			this.listView1.CheckBoxes = true;
			this.listView1.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.columnHeader1,
				this.columnHeader2,
				this.columnHeader3,
				this.columnHeader5,
				this.columnHeader6,
				this.columnHeader4,
				this.columnHeader10,
				this.columnHeader7,
				this.columnHeader8,
				this.columnHeader9,
				this.columnHeader11
			});
			this.listView1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.listView1.Font = new global::System.Drawing.Font("宋体", 10f);
			this.listView1.ForeColor = global::System.Drawing.Color.Black;
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new global::System.Drawing.Point(3, 3);
			this.listView1.Name = "listView1";
			this.listView1.Size = new global::System.Drawing.Size(1125, 421);
			this.listView1.TabIndex = 194;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = global::System.Windows.Forms.View.Details;
			this.columnHeader1.Text = "序号";
			this.columnHeader1.Width = 50;
			this.columnHeader2.Text = "账号";
			this.columnHeader2.Width = 100;
			this.columnHeader3.Text = "密码";
			this.columnHeader3.Width = 100;
			this.columnHeader5.Text = "地址";
			this.columnHeader5.Width = 150;
			this.columnHeader6.Text = "次数";
			this.columnHeader6.Width = 200;
			this.columnHeader4.Text = "开始日期";
			this.columnHeader4.Width = 80;
			this.columnHeader10.Text = "结束日期";
			this.columnHeader10.Width = 80;
			this.columnHeader7.Text = "开始签到";
			this.columnHeader7.Width = 80;
			this.columnHeader8.Text = "结束签到";
			this.columnHeader8.Width = 80;
			this.columnHeader9.Text = "剩余天数";
			this.columnHeader9.Width = 80;
			this.columnHeader11.Text = "指定周几签到";
			this.columnHeader11.Width = 100;
			this.webBrowser1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.webBrowser1.Location = new global::System.Drawing.Point(3, 3);
			this.webBrowser1.MinimumSize = new global::System.Drawing.Size(20, 20);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.Size = new global::System.Drawing.Size(1125, 421);
			this.webBrowser1.TabIndex = 0;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(1145, 573);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.panel1);
			base.Name = "校友邦_定时签到";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "校友邦_定时签到";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.校友邦_定时签到_FormClosing);
			base.Load += new global::System.EventHandler(this.校友邦_定时签到_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x04000007 RID: 7
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000008 RID: 8
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04000009 RID: 9
		private global::System.Windows.Forms.Button button2;

		// Token: 0x0400000A RID: 10
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x0400000B RID: 11
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x0400000C RID: 12
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x0400000D RID: 13
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400000E RID: 14
		private global::System.Windows.Forms.Label label3;

		// Token: 0x0400000F RID: 15
		private global::System.Windows.Forms.TextBox textBox1;

		// Token: 0x04000010 RID: 16
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000011 RID: 17
		private global::System.Windows.Forms.LinkLabel linkLabel1;

		// Token: 0x04000012 RID: 18
		private global::System.Windows.Forms.TextBox textBox2;

		// Token: 0x04000013 RID: 19
		private global::System.Windows.Forms.TabControl tabControl1;

		// Token: 0x04000014 RID: 20
		private global::System.Windows.Forms.TabPage tabPage1;

		// Token: 0x04000015 RID: 21
		private global::System.Windows.Forms.ListView listView1;

		// Token: 0x04000016 RID: 22
		private global::System.Windows.Forms.ColumnHeader columnHeader1;

		// Token: 0x04000017 RID: 23
		private global::System.Windows.Forms.ColumnHeader columnHeader2;

		// Token: 0x04000018 RID: 24
		private global::System.Windows.Forms.ColumnHeader columnHeader3;

		// Token: 0x04000019 RID: 25
		private global::System.Windows.Forms.ColumnHeader columnHeader5;

		// Token: 0x0400001A RID: 26
		private global::System.Windows.Forms.ColumnHeader columnHeader6;

		// Token: 0x0400001B RID: 27
		private global::System.Windows.Forms.ColumnHeader columnHeader4;

		// Token: 0x0400001C RID: 28
		private global::System.Windows.Forms.ColumnHeader columnHeader10;

		// Token: 0x0400001D RID: 29
		private global::System.Windows.Forms.ColumnHeader columnHeader7;

		// Token: 0x0400001E RID: 30
		private global::System.Windows.Forms.ColumnHeader columnHeader8;

		// Token: 0x0400001F RID: 31
		private global::System.Windows.Forms.ColumnHeader columnHeader9;

		// Token: 0x04000020 RID: 32
		private global::System.Windows.Forms.ColumnHeader columnHeader11;

		// Token: 0x04000021 RID: 33
		private global::System.Windows.Forms.TabPage tabPage2;

		// Token: 0x04000022 RID: 34
		private global::System.Windows.Forms.WebBrowser webBrowser1;
	}
}
