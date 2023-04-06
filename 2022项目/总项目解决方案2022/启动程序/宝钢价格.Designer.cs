namespace 启动程序
{
	// Token: 0x0200000E RID: 14
	public partial class 宝钢价格 : global::System.Windows.Forms.Form
	{
		// Token: 0x06000088 RID: 136 RVA: 0x0001185C File Offset: 0x0000FA5C
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00011894 File Offset: 0x0000FA94
		private void InitializeComponent()
		{
			this.columnHeader1 = new global::System.Windows.Forms.ColumnHeader();
			this.listView1 = new global::System.Windows.Forms.ListView();
			this.columnHeader2 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader9 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader10 = new global::System.Windows.Forms.ColumnHeader();
			this.button4 = new global::System.Windows.Forms.Button();
			this.button3 = new global::System.Windows.Forms.Button();
			this.button2 = new global::System.Windows.Forms.Button();
			this.button1 = new global::System.Windows.Forms.Button();
			this.splitContainer1 = new global::System.Windows.Forms.SplitContainer();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.button6 = new global::System.Windows.Forms.Button();
			this.button5 = new global::System.Windows.Forms.Button();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.checkBox12 = new global::System.Windows.Forms.CheckBox();
			this.checkBox11 = new global::System.Windows.Forms.CheckBox();
			this.checkBox10 = new global::System.Windows.Forms.CheckBox();
			this.checkBox9 = new global::System.Windows.Forms.CheckBox();
			this.checkBox8 = new global::System.Windows.Forms.CheckBox();
			this.checkBox7 = new global::System.Windows.Forms.CheckBox();
			this.checkBox6 = new global::System.Windows.Forms.CheckBox();
			this.checkBox5 = new global::System.Windows.Forms.CheckBox();
			this.checkBox4 = new global::System.Windows.Forms.CheckBox();
			this.checkBox3 = new global::System.Windows.Forms.CheckBox();
			this.checkBox2 = new global::System.Windows.Forms.CheckBox();
			this.checkBox1 = new global::System.Windows.Forms.CheckBox();
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.columnHeader1.Text = "序号";
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
				this.columnHeader10
			});
			this.listView1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new global::System.Drawing.Point(0, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new global::System.Drawing.Size(1209, 479);
			this.listView1.TabIndex = 36;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = global::System.Windows.Forms.View.Details;
			this.columnHeader2.Text = "月份";
			this.columnHeader2.Width = 70;
			this.columnHeader3.Text = "品种";
			this.columnHeader3.Width = 120;
			this.columnHeader4.Text = "流水号";
			this.columnHeader4.Width = 80;
			this.columnHeader5.Text = "牌号";
			this.columnHeader5.Width = 100;
			this.columnHeader6.Text = "厚度";
			this.columnHeader6.Width = 100;
			this.columnHeader7.Text = "宽度";
			this.columnHeader7.Width = 100;
			this.columnHeader8.Text = "基价";
			this.columnHeader8.Width = 80;
			this.columnHeader9.Text = "备注一";
			this.columnHeader9.Width = 200;
			this.columnHeader10.Text = "备注二";
			this.columnHeader10.Width = 200;
			this.button4.Location = new global::System.Drawing.Point(805, 68);
			this.button4.Name = "button4";
			this.button4.Size = new global::System.Drawing.Size(112, 30);
			this.button4.TabIndex = 38;
			this.button4.Text = "停止";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new global::System.EventHandler(this.Button4_Click);
			this.button3.Location = new global::System.Drawing.Point(670, 68);
			this.button3.Name = "button3";
			this.button3.Size = new global::System.Drawing.Size(112, 30);
			this.button3.TabIndex = 37;
			this.button3.Text = "继续";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new global::System.EventHandler(this.Button3_Click);
			this.button2.Location = new global::System.Drawing.Point(805, 32);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(112, 30);
			this.button2.TabIndex = 36;
			this.button2.Text = "暂停";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.Button2_Click);
			this.button1.Location = new global::System.Drawing.Point(670, 32);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(112, 30);
			this.button1.TabIndex = 34;
			this.button1.Text = "开始运行";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.Button1_Click);
			this.splitContainer1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new global::System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = global::System.Windows.Forms.Orientation.Horizontal;
			this.splitContainer1.Panel1.Controls.Add(this.textBox1);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.button6);
			this.splitContainer1.Panel1.Controls.Add(this.button5);
			this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
			this.splitContainer1.Panel1.Controls.Add(this.button4);
			this.splitContainer1.Panel1.Controls.Add(this.button3);
			this.splitContainer1.Panel1.Controls.Add(this.button2);
			this.splitContainer1.Panel1.Controls.Add(this.button1);
			this.splitContainer1.Panel2.Controls.Add(this.listView1);
			this.splitContainer1.Size = new global::System.Drawing.Size(1209, 639);
			this.splitContainer1.SplitterDistance = 156;
			this.splitContainer1.TabIndex = 2;
			this.textBox1.Location = new global::System.Drawing.Point(443, 32);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new global::System.Drawing.Size(100, 21);
			this.textBox1.TabIndex = 45;
			this.textBox1.Text = "202001";
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(372, 37);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(65, 12);
			this.label1.TabIndex = 44;
			this.label1.Text = "指定月份：";
			this.button6.Location = new global::System.Drawing.Point(937, 68);
			this.button6.Name = "button6";
			this.button6.Size = new global::System.Drawing.Size(112, 30);
			this.button6.TabIndex = 43;
			this.button6.Text = "清空结果";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new global::System.EventHandler(this.Button6_Click);
			this.button5.Location = new global::System.Drawing.Point(937, 32);
			this.button5.Name = "button5";
			this.button5.Size = new global::System.Drawing.Size(112, 30);
			this.button5.TabIndex = 42;
			this.button5.Text = "导出结果";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new global::System.EventHandler(this.Button5_Click);
			this.groupBox1.Controls.Add(this.checkBox12);
			this.groupBox1.Controls.Add(this.checkBox11);
			this.groupBox1.Controls.Add(this.checkBox10);
			this.groupBox1.Controls.Add(this.checkBox9);
			this.groupBox1.Controls.Add(this.checkBox8);
			this.groupBox1.Controls.Add(this.checkBox7);
			this.groupBox1.Controls.Add(this.checkBox6);
			this.groupBox1.Controls.Add(this.checkBox5);
			this.groupBox1.Controls.Add(this.checkBox4);
			this.groupBox1.Controls.Add(this.checkBox3);
			this.groupBox1.Controls.Add(this.checkBox2);
			this.groupBox1.Controls.Add(this.checkBox1);
			this.groupBox1.Location = new global::System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new global::System.Drawing.Size(353, 105);
			this.groupBox1.TabIndex = 41;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "品种选择";
			this.checkBox12.AutoSize = true;
			this.checkBox12.Enabled = false;
			this.checkBox12.Font = new global::System.Drawing.Font("宋体", 10f);
			this.checkBox12.Location = new global::System.Drawing.Point(267, 53);
			this.checkBox12.Name = "checkBox12";
			this.checkBox12.Size = new global::System.Drawing.Size(47, 18);
			this.checkBox12.TabIndex = 53;
			this.checkBox12.Text = "X02";
			this.checkBox12.UseVisualStyleBackColor = true;
			this.checkBox11.AutoSize = true;
			this.checkBox11.Enabled = false;
			this.checkBox11.Font = new global::System.Drawing.Font("宋体", 10f);
			this.checkBox11.Location = new global::System.Drawing.Point(219, 53);
			this.checkBox11.Name = "checkBox11";
			this.checkBox11.Size = new global::System.Drawing.Size(47, 18);
			this.checkBox11.TabIndex = 52;
			this.checkBox11.Text = "X00";
			this.checkBox11.UseVisualStyleBackColor = true;
			this.checkBox10.AutoSize = true;
			this.checkBox10.Font = new global::System.Drawing.Font("宋体", 10f);
			this.checkBox10.Location = new global::System.Drawing.Point(171, 53);
			this.checkBox10.Name = "checkBox10";
			this.checkBox10.Size = new global::System.Drawing.Size(47, 18);
			this.checkBox10.TabIndex = 51;
			this.checkBox10.Text = "S0C";
			this.checkBox10.UseVisualStyleBackColor = true;
			this.checkBox9.AutoSize = true;
			this.checkBox9.Font = new global::System.Drawing.Font("宋体", 10f);
			this.checkBox9.Location = new global::System.Drawing.Point(123, 53);
			this.checkBox9.Name = "checkBox9";
			this.checkBox9.Size = new global::System.Drawing.Size(47, 18);
			this.checkBox9.TabIndex = 50;
			this.checkBox9.Text = "S00";
			this.checkBox9.UseVisualStyleBackColor = true;
			this.checkBox8.AutoSize = true;
			this.checkBox8.Font = new global::System.Drawing.Font("宋体", 10f);
			this.checkBox8.Location = new global::System.Drawing.Point(66, 53);
			this.checkBox8.Name = "checkBox8";
			this.checkBox8.Size = new global::System.Drawing.Size(47, 18);
			this.checkBox8.TabIndex = 49;
			this.checkBox8.Text = "O00";
			this.checkBox8.UseVisualStyleBackColor = true;
			this.checkBox7.AutoSize = true;
			this.checkBox7.Font = new global::System.Drawing.Font("宋体", 10f);
			this.checkBox7.Location = new global::System.Drawing.Point(18, 53);
			this.checkBox7.Name = "checkBox7";
			this.checkBox7.Size = new global::System.Drawing.Size(47, 18);
			this.checkBox7.TabIndex = 48;
			this.checkBox7.Text = "N00";
			this.checkBox7.UseVisualStyleBackColor = true;
			this.checkBox6.AutoSize = true;
			this.checkBox6.Font = new global::System.Drawing.Font("宋体", 10f);
			this.checkBox6.Location = new global::System.Drawing.Point(267, 20);
			this.checkBox6.Name = "checkBox6";
			this.checkBox6.Size = new global::System.Drawing.Size(47, 18);
			this.checkBox6.TabIndex = 47;
			this.checkBox6.Text = "ML0";
			this.checkBox6.UseVisualStyleBackColor = true;
			this.checkBox5.AutoSize = true;
			this.checkBox5.Font = new global::System.Drawing.Font("宋体", 10f);
			this.checkBox5.Location = new global::System.Drawing.Point(219, 20);
			this.checkBox5.Name = "checkBox5";
			this.checkBox5.Size = new global::System.Drawing.Size(47, 18);
			this.checkBox5.TabIndex = 46;
			this.checkBox5.Text = "M00";
			this.checkBox5.UseVisualStyleBackColor = true;
			this.checkBox4.AutoSize = true;
			this.checkBox4.Enabled = false;
			this.checkBox4.Font = new global::System.Drawing.Font("宋体", 10f);
			this.checkBox4.Location = new global::System.Drawing.Point(171, 20);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new global::System.Drawing.Size(47, 18);
			this.checkBox4.TabIndex = 45;
			this.checkBox4.Text = "L00";
			this.checkBox4.UseVisualStyleBackColor = true;
			this.checkBox3.AutoSize = true;
			this.checkBox3.Font = new global::System.Drawing.Font("宋体", 10f);
			this.checkBox3.Location = new global::System.Drawing.Point(123, 20);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new global::System.Drawing.Size(47, 18);
			this.checkBox3.TabIndex = 44;
			this.checkBox3.Text = "HRM";
			this.checkBox3.UseVisualStyleBackColor = true;
			this.checkBox2.AutoSize = true;
			this.checkBox2.Font = new global::System.Drawing.Font("宋体", 10f);
			this.checkBox2.Location = new global::System.Drawing.Point(66, 20);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new global::System.Drawing.Size(47, 18);
			this.checkBox2.TabIndex = 43;
			this.checkBox2.Text = "HRC";
			this.checkBox2.UseVisualStyleBackColor = true;
			this.checkBox1.AutoSize = true;
			this.checkBox1.Font = new global::System.Drawing.Font("宋体", 10f);
			this.checkBox1.Location = new global::System.Drawing.Point(18, 20);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new global::System.Drawing.Size(47, 18);
			this.checkBox1.TabIndex = 42;
			this.checkBox1.Text = "HRB";
			this.checkBox1.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(1209, 639);
			base.Controls.Add(this.splitContainer1);
			base.Name = "宝钢价格";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "宝钢价格";
			base.Load += new global::System.EventHandler(this.宝钢价格_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x04000152 RID: 338
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000153 RID: 339
		private global::System.Windows.Forms.ColumnHeader columnHeader1;

		// Token: 0x04000154 RID: 340
		private global::System.Windows.Forms.ListView listView1;

		// Token: 0x04000155 RID: 341
		private global::System.Windows.Forms.Button button4;

		// Token: 0x04000156 RID: 342
		private global::System.Windows.Forms.Button button3;

		// Token: 0x04000157 RID: 343
		private global::System.Windows.Forms.Button button2;

		// Token: 0x04000158 RID: 344
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04000159 RID: 345
		private global::System.Windows.Forms.SplitContainer splitContainer1;

		// Token: 0x0400015A RID: 346
		private global::System.Windows.Forms.ColumnHeader columnHeader2;

		// Token: 0x0400015B RID: 347
		private global::System.Windows.Forms.ColumnHeader columnHeader3;

		// Token: 0x0400015C RID: 348
		private global::System.Windows.Forms.ColumnHeader columnHeader4;

		// Token: 0x0400015D RID: 349
		private global::System.Windows.Forms.ColumnHeader columnHeader5;

		// Token: 0x0400015E RID: 350
		private global::System.Windows.Forms.ColumnHeader columnHeader6;

		// Token: 0x0400015F RID: 351
		private global::System.Windows.Forms.ColumnHeader columnHeader7;

		// Token: 0x04000160 RID: 352
		private global::System.Windows.Forms.ColumnHeader columnHeader8;

		// Token: 0x04000161 RID: 353
		private global::System.Windows.Forms.ColumnHeader columnHeader9;

		// Token: 0x04000162 RID: 354
		private global::System.Windows.Forms.ColumnHeader columnHeader10;

		// Token: 0x04000163 RID: 355
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04000164 RID: 356
		private global::System.Windows.Forms.CheckBox checkBox12;

		// Token: 0x04000165 RID: 357
		private global::System.Windows.Forms.CheckBox checkBox11;

		// Token: 0x04000166 RID: 358
		private global::System.Windows.Forms.CheckBox checkBox10;

		// Token: 0x04000167 RID: 359
		private global::System.Windows.Forms.CheckBox checkBox9;

		// Token: 0x04000168 RID: 360
		private global::System.Windows.Forms.CheckBox checkBox8;

		// Token: 0x04000169 RID: 361
		private global::System.Windows.Forms.CheckBox checkBox7;

		// Token: 0x0400016A RID: 362
		private global::System.Windows.Forms.CheckBox checkBox6;

		// Token: 0x0400016B RID: 363
		private global::System.Windows.Forms.CheckBox checkBox5;

		// Token: 0x0400016C RID: 364
		private global::System.Windows.Forms.CheckBox checkBox4;

		// Token: 0x0400016D RID: 365
		private global::System.Windows.Forms.CheckBox checkBox3;

		// Token: 0x0400016E RID: 366
		private global::System.Windows.Forms.CheckBox checkBox2;

		// Token: 0x0400016F RID: 367
		private global::System.Windows.Forms.CheckBox checkBox1;

		// Token: 0x04000170 RID: 368
		private global::System.Windows.Forms.Button button6;

		// Token: 0x04000171 RID: 369
		private global::System.Windows.Forms.Button button5;

		// Token: 0x04000172 RID: 370
		private global::System.Windows.Forms.TextBox textBox1;

		// Token: 0x04000173 RID: 371
		private global::System.Windows.Forms.Label label1;
	}
}
