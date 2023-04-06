namespace 启动程序
{
	// Token: 0x02000009 RID: 9
	public partial class 八爪盒子 : global::System.Windows.Forms.Form
	{
		// Token: 0x06000044 RID: 68 RVA: 0x0000B22C File Offset: 0x0000942C
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000B264 File Offset: 0x00009464
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.label6 = new global::System.Windows.Forms.Label();
			this.label5 = new global::System.Windows.Forms.Label();
			this.label4 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.textBox2 = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.button6 = new global::System.Windows.Forms.Button();
			this.button4 = new global::System.Windows.Forms.Button();
			this.button3 = new global::System.Windows.Forms.Button();
			this.button2 = new global::System.Windows.Forms.Button();
			this.button5 = new global::System.Windows.Forms.Button();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.listView1 = new global::System.Windows.Forms.ListView();
			this.columnHeader1 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader13 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader14 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader15 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader16 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader17 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader18 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader19 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader20 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader21 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader22 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader23 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader9 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader10 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader11 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader12 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader24 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader25 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader26 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader27 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader28 = new global::System.Windows.Forms.ColumnHeader();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.textBox2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.textBox1);
			this.groupBox1.Controls.Add(this.button6);
			this.groupBox1.Controls.Add(this.button4);
			this.groupBox1.Controls.Add(this.button3);
			this.groupBox1.Controls.Add(this.button2);
			this.groupBox1.Controls.Add(this.button5);
			this.groupBox1.Dock = global::System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new global::System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new global::System.Drawing.Size(1405, 132);
			this.groupBox1.TabIndex = 34;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "参数设置";
			this.label6.AutoSize = true;
			this.label6.Font = new global::System.Drawing.Font("宋体", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label6.ForeColor = global::System.Drawing.Color.Red;
			this.label6.Location = new global::System.Drawing.Point(603, 73);
			this.label6.Name = "label6";
			this.label6.Size = new global::System.Drawing.Size(17, 16);
			this.label6.TabIndex = 50;
			this.label6.Text = "0";
			this.label5.AutoSize = true;
			this.label5.Font = new global::System.Drawing.Font("宋体", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label5.ForeColor = global::System.Drawing.Color.Red;
			this.label5.Location = new global::System.Drawing.Point(603, 31);
			this.label5.Name = "label5";
			this.label5.Size = new global::System.Drawing.Size(17, 16);
			this.label5.TabIndex = 49;
			this.label5.Text = "0";
			this.label4.AutoSize = true;
			this.label4.Font = new global::System.Drawing.Font("宋体", 11f);
			this.label4.Location = new global::System.Drawing.Point(467, 74);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(97, 15);
			this.label4.TabIndex = 48;
			this.label4.Text = "已抓取数量：";
			this.label3.AutoSize = true;
			this.label3.Font = new global::System.Drawing.Font("宋体", 11f);
			this.label3.Location = new global::System.Drawing.Point(467, 31);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(112, 15);
			this.label3.TabIndex = 47;
			this.label3.Text = "当前数据总数：";
			this.label2.AutoSize = true;
			this.label2.Font = new global::System.Drawing.Font("宋体", 11f);
			this.label2.Location = new global::System.Drawing.Point(34, 74);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(82, 15);
			this.label2.TabIndex = 46;
			this.label2.Text = "结束日期：";
			this.textBox2.Location = new global::System.Drawing.Point(122, 68);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new global::System.Drawing.Size(222, 21);
			this.textBox2.TabIndex = 45;
			this.textBox2.Text = "2019-12-31";
			this.label1.AutoSize = true;
			this.label1.Font = new global::System.Drawing.Font("宋体", 11f);
			this.label1.Location = new global::System.Drawing.Point(34, 31);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(82, 15);
			this.label1.TabIndex = 44;
			this.label1.Text = "起始日期：";
			this.textBox1.Location = new global::System.Drawing.Point(122, 25);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new global::System.Drawing.Size(222, 21);
			this.textBox1.TabIndex = 43;
			this.textBox1.Text = "2019-01-01";
			this.button6.Location = new global::System.Drawing.Point(958, 62);
			this.button6.Name = "button6";
			this.button6.Size = new global::System.Drawing.Size(112, 30);
			this.button6.TabIndex = 42;
			this.button6.Text = "导出";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new global::System.EventHandler(this.Button6_Click);
			this.button4.Location = new global::System.Drawing.Point(840, 62);
			this.button4.Name = "button4";
			this.button4.Size = new global::System.Drawing.Size(112, 30);
			this.button4.TabIndex = 41;
			this.button4.Text = "停止";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new global::System.EventHandler(this.Button4_Click);
			this.button3.Location = new global::System.Drawing.Point(958, 25);
			this.button3.Name = "button3";
			this.button3.Size = new global::System.Drawing.Size(112, 30);
			this.button3.TabIndex = 40;
			this.button3.Text = "继续";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new global::System.EventHandler(this.Button3_Click);
			this.button2.Location = new global::System.Drawing.Point(840, 25);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(112, 30);
			this.button2.TabIndex = 39;
			this.button2.Text = "暂停";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.Button2_Click);
			this.button5.Location = new global::System.Drawing.Point(722, 26);
			this.button5.Name = "button5";
			this.button5.Size = new global::System.Drawing.Size(112, 30);
			this.button5.TabIndex = 38;
			this.button5.Text = "开始运行";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new global::System.EventHandler(this.Button5_Click);
			this.timer1.Interval = 10000;
			this.listView1.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.columnHeader1,
				this.columnHeader2,
				this.columnHeader3,
				this.columnHeader13,
				this.columnHeader14,
				this.columnHeader15,
				this.columnHeader16,
				this.columnHeader17,
				this.columnHeader18,
				this.columnHeader19,
				this.columnHeader20,
				this.columnHeader21,
				this.columnHeader22,
				this.columnHeader23,
				this.columnHeader4,
				this.columnHeader5,
				this.columnHeader7,
				this.columnHeader8,
				this.columnHeader6,
				this.columnHeader28,
				this.columnHeader9,
				this.columnHeader10,
				this.columnHeader11,
				this.columnHeader12,
				this.columnHeader24,
				this.columnHeader25,
				this.columnHeader26,
				this.columnHeader27
			});
			this.listView1.Dock = global::System.Windows.Forms.DockStyle.Bottom;
			this.listView1.Font = new global::System.Drawing.Font("宋体", 9f);
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new global::System.Drawing.Point(0, 153);
			this.listView1.Name = "listView1";
			this.listView1.Size = new global::System.Drawing.Size(1405, 452);
			this.listView1.TabIndex = 35;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = global::System.Windows.Forms.View.Details;
			this.columnHeader1.Text = "序号";
			this.columnHeader2.Text = "姓名";
			this.columnHeader3.Text = "职位";
			this.columnHeader3.Width = 100;
			this.columnHeader13.Text = "地点";
			this.columnHeader14.Text = "工作年限";
			this.columnHeader15.Text = "性别";
			this.columnHeader16.Text = "出生日期";
			this.columnHeader16.Width = 80;
			this.columnHeader17.Text = "公司";
			this.columnHeader17.Width = 100;
			this.columnHeader18.Text = "大学";
			this.columnHeader18.Width = 100;
			this.columnHeader19.Text = "专业";
			this.columnHeader19.Width = 100;
			this.columnHeader20.Text = "学历";
			this.columnHeader21.Text = "手机";
			this.columnHeader21.Width = 80;
			this.columnHeader22.Text = "邮箱";
			this.columnHeader23.Text = "自我评价";
			this.columnHeader23.Width = 100;
			this.columnHeader4.Text = "期望职位";
			this.columnHeader5.Text = "期望地点";
			this.columnHeader7.Text = "教育开始时间";
			this.columnHeader7.Width = 100;
			this.columnHeader8.Text = "教育结束时间";
			this.columnHeader8.Width = 100;
			this.columnHeader6.Text = "学历1";
			this.columnHeader9.Text = "专业1";
			this.columnHeader10.Text = "工作开始时间";
			this.columnHeader10.Width = 100;
			this.columnHeader11.Text = "工作结束时间";
			this.columnHeader11.Width = 100;
			this.columnHeader12.Text = "就职公司";
			this.columnHeader12.Width = 100;
			this.columnHeader24.Text = "公司类型";
			this.columnHeader25.Text = "就职岗位";
			this.columnHeader25.Width = 100;
			this.columnHeader26.Text = "薪资";
			this.columnHeader27.Text = "职责";
			this.columnHeader27.Width = 100;
			this.columnHeader28.Text = "学校名称";
			this.columnHeader28.Width = 100;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(1405, 605);
			base.Controls.Add(this.listView1);
			base.Controls.Add(this.groupBox1);
			base.Name = "八爪盒子";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "八爪盒子";
			base.Load += new global::System.EventHandler(this.八爪盒子_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x040000D4 RID: 212
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x040000D5 RID: 213
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x040000D6 RID: 214
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x040000D7 RID: 215
		private global::System.Windows.Forms.Button button6;

		// Token: 0x040000D8 RID: 216
		private global::System.Windows.Forms.Button button4;

		// Token: 0x040000D9 RID: 217
		private global::System.Windows.Forms.Button button3;

		// Token: 0x040000DA RID: 218
		private global::System.Windows.Forms.Button button2;

		// Token: 0x040000DB RID: 219
		private global::System.Windows.Forms.Button button5;

		// Token: 0x040000DC RID: 220
		private global::System.Windows.Forms.ListView listView1;

		// Token: 0x040000DD RID: 221
		private global::System.Windows.Forms.ColumnHeader columnHeader1;

		// Token: 0x040000DE RID: 222
		private global::System.Windows.Forms.TextBox textBox1;

		// Token: 0x040000DF RID: 223
		private global::System.Windows.Forms.ColumnHeader columnHeader2;

		// Token: 0x040000E0 RID: 224
		private global::System.Windows.Forms.ColumnHeader columnHeader3;

		// Token: 0x040000E1 RID: 225
		private global::System.Windows.Forms.ColumnHeader columnHeader13;

		// Token: 0x040000E2 RID: 226
		private global::System.Windows.Forms.ColumnHeader columnHeader14;

		// Token: 0x040000E3 RID: 227
		private global::System.Windows.Forms.ColumnHeader columnHeader15;

		// Token: 0x040000E4 RID: 228
		private global::System.Windows.Forms.ColumnHeader columnHeader16;

		// Token: 0x040000E5 RID: 229
		private global::System.Windows.Forms.ColumnHeader columnHeader17;

		// Token: 0x040000E6 RID: 230
		private global::System.Windows.Forms.ColumnHeader columnHeader18;

		// Token: 0x040000E7 RID: 231
		private global::System.Windows.Forms.ColumnHeader columnHeader19;

		// Token: 0x040000E8 RID: 232
		private global::System.Windows.Forms.ColumnHeader columnHeader20;

		// Token: 0x040000E9 RID: 233
		private global::System.Windows.Forms.ColumnHeader columnHeader21;

		// Token: 0x040000EA RID: 234
		private global::System.Windows.Forms.ColumnHeader columnHeader22;

		// Token: 0x040000EB RID: 235
		private global::System.Windows.Forms.ColumnHeader columnHeader23;

		// Token: 0x040000EC RID: 236
		private global::System.Windows.Forms.ColumnHeader columnHeader4;

		// Token: 0x040000ED RID: 237
		private global::System.Windows.Forms.ColumnHeader columnHeader5;

		// Token: 0x040000EE RID: 238
		private global::System.Windows.Forms.ColumnHeader columnHeader7;

		// Token: 0x040000EF RID: 239
		private global::System.Windows.Forms.ColumnHeader columnHeader8;

		// Token: 0x040000F0 RID: 240
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040000F1 RID: 241
		private global::System.Windows.Forms.TextBox textBox2;

		// Token: 0x040000F2 RID: 242
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040000F3 RID: 243
		private global::System.Windows.Forms.Label label4;

		// Token: 0x040000F4 RID: 244
		private global::System.Windows.Forms.Label label3;

		// Token: 0x040000F5 RID: 245
		private global::System.Windows.Forms.Label label6;

		// Token: 0x040000F6 RID: 246
		private global::System.Windows.Forms.Label label5;

		// Token: 0x040000F7 RID: 247
		private global::System.Windows.Forms.ColumnHeader columnHeader6;

		// Token: 0x040000F8 RID: 248
		private global::System.Windows.Forms.ColumnHeader columnHeader9;

		// Token: 0x040000F9 RID: 249
		private global::System.Windows.Forms.ColumnHeader columnHeader10;

		// Token: 0x040000FA RID: 250
		private global::System.Windows.Forms.ColumnHeader columnHeader11;

		// Token: 0x040000FB RID: 251
		private global::System.Windows.Forms.ColumnHeader columnHeader12;

		// Token: 0x040000FC RID: 252
		private global::System.Windows.Forms.ColumnHeader columnHeader24;

		// Token: 0x040000FD RID: 253
		private global::System.Windows.Forms.ColumnHeader columnHeader25;

		// Token: 0x040000FE RID: 254
		private global::System.Windows.Forms.ColumnHeader columnHeader26;

		// Token: 0x040000FF RID: 255
		private global::System.Windows.Forms.ColumnHeader columnHeader27;

		// Token: 0x04000100 RID: 256
		private global::System.Windows.Forms.ColumnHeader columnHeader28;
	}
}
