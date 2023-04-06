namespace 启动程序
{
	// Token: 0x0200000C RID: 12
	public partial class 奇趣分分彩 : global::System.Windows.Forms.Form
	{
		// Token: 0x06000062 RID: 98 RVA: 0x0000DA24 File Offset: 0x0000BC24
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000DA5C File Offset: 0x0000BC5C
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.button7 = new global::System.Windows.Forms.Button();
			this.button6 = new global::System.Windows.Forms.Button();
			this.listView2 = new global::System.Windows.Forms.ListView();
			this.columnHeader6 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader9 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader14 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader15 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader16 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader17 = new global::System.Windows.Forms.ColumnHeader();
			this.columnHeader18 = new global::System.Windows.Forms.ColumnHeader();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.label4 = new global::System.Windows.Forms.Label();
			this.label5 = new global::System.Windows.Forms.Label();
			this.dataGridView1 = new global::System.Windows.Forms.DataGridView();
			this.Column1 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column3 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column4 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column5 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column6 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column7 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column8 = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.groupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
			base.SuspendLayout();
			this.timer1.Interval = 10000;
			this.timer1.Tick += new global::System.EventHandler(this.Timer1_Tick);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.button7);
			this.groupBox1.Controls.Add(this.button6);
			this.groupBox1.Dock = global::System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new global::System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new global::System.Drawing.Size(1076, 106);
			this.groupBox1.TabIndex = 32;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "参数设置";
			this.button7.Location = new global::System.Drawing.Point(585, 12);
			this.button7.Name = "button7";
			this.button7.Size = new global::System.Drawing.Size(98, 34);
			this.button7.TabIndex = 3;
			this.button7.Text = "停止监控";
			this.button7.UseVisualStyleBackColor = true;
			this.button7.Click += new global::System.EventHandler(this.Button7_Click);
			this.button6.Location = new global::System.Drawing.Point(371, 12);
			this.button6.Name = "button6";
			this.button6.Size = new global::System.Drawing.Size(93, 34);
			this.button6.TabIndex = 2;
			this.button6.Text = "开启监控";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new global::System.EventHandler(this.Button6_Click);
			this.listView2.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.columnHeader6,
				this.columnHeader7,
				this.columnHeader8,
				this.columnHeader9,
				this.columnHeader14,
				this.columnHeader15,
				this.columnHeader16,
				this.columnHeader17,
				this.columnHeader18
			});
			this.listView2.Dock = global::System.Windows.Forms.DockStyle.Bottom;
			this.listView2.Font = new global::System.Drawing.Font("宋体", 11f);
			this.listView2.FullRowSelect = true;
			this.listView2.GridLines = true;
			this.listView2.HideSelection = false;
			this.listView2.Location = new global::System.Drawing.Point(0, 482);
			this.listView2.Name = "listView2";
			this.listView2.Size = new global::System.Drawing.Size(1076, 167);
			this.listView2.TabIndex = 36;
			this.listView2.UseCompatibleStateImageBehavior = false;
			this.listView2.View = global::System.Windows.Forms.View.Details;
			this.columnHeader6.Text = "序号";
			this.columnHeader7.Text = "";
			this.columnHeader7.Width = 120;
			this.columnHeader8.Text = "奇趣分分彩";
			this.columnHeader8.Width = 150;
			this.columnHeader9.Text = "";
			this.columnHeader9.Width = 100;
			this.columnHeader14.Text = "奇趣三分彩";
			this.columnHeader14.Width = 150;
			this.columnHeader15.Text = "";
			this.columnHeader15.Width = 100;
			this.columnHeader16.Text = "奇趣五分彩";
			this.columnHeader16.Width = 150;
			this.columnHeader17.Text = "";
			this.columnHeader17.Width = 100;
			this.columnHeader18.Text = "奇趣十分彩";
			this.columnHeader18.Width = 150;
			this.label1.AutoSize = true;
			this.label1.Font = new global::System.Drawing.Font("宋体", 14.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label1.Location = new global::System.Drawing.Point(442, 460);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(129, 19);
			this.label1.TabIndex = 37;
			this.label1.Text = "同号统计分析";
			this.label2.AutoSize = true;
			this.label2.Font = new global::System.Drawing.Font("宋体", 14.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label2.Location = new global::System.Drawing.Point(120, 74);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(109, 19);
			this.label2.TabIndex = 38;
			this.label2.Text = "奇趣分分彩";
			this.label3.AutoSize = true;
			this.label3.Font = new global::System.Drawing.Font("宋体", 14.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label3.Location = new global::System.Drawing.Point(377, 74);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(109, 19);
			this.label3.TabIndex = 39;
			this.label3.Text = "奇趣三分彩";
			this.label4.AutoSize = true;
			this.label4.Font = new global::System.Drawing.Font("宋体", 14.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label4.Location = new global::System.Drawing.Point(640, 74);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(109, 19);
			this.label4.TabIndex = 40;
			this.label4.Text = "奇趣五分彩";
			this.label5.AutoSize = true;
			this.label5.Font = new global::System.Drawing.Font("宋体", 14.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label5.Location = new global::System.Drawing.Point(889, 74);
			this.label5.Name = "label5";
			this.label5.Size = new global::System.Drawing.Size(109, 19);
			this.label5.TabIndex = 41;
			this.label5.Text = "奇趣十分彩";
			this.dataGridView1.AutoSizeColumnsMode = global::System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dataGridView1.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[]
			{
				this.Column1,
				this.Column2,
				this.Column3,
				this.Column4,
				this.Column5,
				this.Column6,
				this.Column7,
				this.Column8
			});
			this.dataGridView1.Dock = global::System.Windows.Forms.DockStyle.Top;
			this.dataGridView1.Location = new global::System.Drawing.Point(0, 106);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowTemplate.Height = 23;
			this.dataGridView1.Size = new global::System.Drawing.Size(1076, 340);
			this.dataGridView1.TabIndex = 38;
			this.Column1.HeaderText = "期号";
			this.Column1.Name = "Column1";
			this.Column2.HeaderText = "开奖结果";
			this.Column2.Name = "Column2";
			this.Column3.HeaderText = "期号";
			this.Column3.Name = "Column3";
			this.Column4.HeaderText = "开奖结果";
			this.Column4.Name = "Column4";
			this.Column5.HeaderText = "期号";
			this.Column5.Name = "Column5";
			this.Column6.HeaderText = "开奖结果";
			this.Column6.Name = "Column6";
			this.Column7.HeaderText = "期号";
			this.Column7.Name = "Column7";
			this.Column8.HeaderText = "开奖结果";
			this.Column8.Name = "Column8";
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(1076, 649);
			base.Controls.Add(this.dataGridView1);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.listView2);
			base.Controls.Add(this.groupBox1);
			base.Name = "奇趣分分彩";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "奇趣分分彩";
			base.Load += new global::System.EventHandler(this.奇趣分分彩_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400011B RID: 283
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x0400011C RID: 284
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x0400011D RID: 285
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x0400011E RID: 286
		private global::System.Windows.Forms.Button button7;

		// Token: 0x0400011F RID: 287
		private global::System.Windows.Forms.Button button6;

		// Token: 0x04000120 RID: 288
		private global::System.Windows.Forms.ListView listView2;

		// Token: 0x04000121 RID: 289
		private global::System.Windows.Forms.ColumnHeader columnHeader6;

		// Token: 0x04000122 RID: 290
		private global::System.Windows.Forms.ColumnHeader columnHeader7;

		// Token: 0x04000123 RID: 291
		private global::System.Windows.Forms.ColumnHeader columnHeader8;

		// Token: 0x04000124 RID: 292
		private global::System.Windows.Forms.ColumnHeader columnHeader9;

		// Token: 0x04000125 RID: 293
		private global::System.Windows.Forms.ColumnHeader columnHeader14;

		// Token: 0x04000126 RID: 294
		private global::System.Windows.Forms.ColumnHeader columnHeader15;

		// Token: 0x04000127 RID: 295
		private global::System.Windows.Forms.ColumnHeader columnHeader16;

		// Token: 0x04000128 RID: 296
		private global::System.Windows.Forms.ColumnHeader columnHeader17;

		// Token: 0x04000129 RID: 297
		private global::System.Windows.Forms.ColumnHeader columnHeader18;

		// Token: 0x0400012A RID: 298
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400012B RID: 299
		private global::System.Windows.Forms.Label label2;

		// Token: 0x0400012C RID: 300
		private global::System.Windows.Forms.Label label5;

		// Token: 0x0400012D RID: 301
		private global::System.Windows.Forms.Label label4;

		// Token: 0x0400012E RID: 302
		private global::System.Windows.Forms.Label label3;

		// Token: 0x0400012F RID: 303
		private global::System.Windows.Forms.DataGridView dataGridView1;

		// Token: 0x04000130 RID: 304
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column1;

		// Token: 0x04000131 RID: 305
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column2;

		// Token: 0x04000132 RID: 306
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column3;

		// Token: 0x04000133 RID: 307
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column4;

		// Token: 0x04000134 RID: 308
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column5;

		// Token: 0x04000135 RID: 309
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column6;

		// Token: 0x04000136 RID: 310
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column7;

		// Token: 0x04000137 RID: 311
		private global::System.Windows.Forms.DataGridViewTextBoxColumn Column8;
	}
}
