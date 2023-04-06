namespace 启动程序
{
	// Token: 0x0200000D RID: 13
	public partial class 子窗体 : global::System.Windows.Forms.Form
	{
		// Token: 0x06000074 RID: 116 RVA: 0x0000EA30 File Offset: 0x0000CC30
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000EA68 File Offset: 0x0000CC68
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::启动程序.子窗体));
			this.label40 = new global::System.Windows.Forms.Label();
			this.label33 = new global::System.Windows.Forms.Label();
			this.label12 = new global::System.Windows.Forms.Label();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.linkLabel1 = new global::System.Windows.Forms.LinkLabel();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.label4 = new global::System.Windows.Forms.Label();
			this.label5 = new global::System.Windows.Forms.Label();
			this.label6 = new global::System.Windows.Forms.Label();
			this.label7 = new global::System.Windows.Forms.Label();
			this.label8 = new global::System.Windows.Forms.Label();
			this.label9 = new global::System.Windows.Forms.Label();
			this.label10 = new global::System.Windows.Forms.Label();
			this.pictureBox1 = new global::System.Windows.Forms.PictureBox();
			this.linkLabel2 = new global::System.Windows.Forms.LinkLabel();
			this.linkLabel3 = new global::System.Windows.Forms.LinkLabel();
			this.linkLabel4 = new global::System.Windows.Forms.LinkLabel();
			this.label11 = new global::System.Windows.Forms.Label();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.label40.AutoSize = true;
			this.label40.Font = new global::System.Drawing.Font("宋体", 11f);
			this.label40.Location = new global::System.Drawing.Point(17, 229);
			this.label40.Name = "label40";
			this.label40.Size = new global::System.Drawing.Size(45, 15);
			this.label40.TabIndex = 60;
			this.label40.Text = "钯 金";
			this.label33.AutoSize = true;
			this.label33.Font = new global::System.Drawing.Font("宋体", 11f);
			this.label33.Location = new global::System.Drawing.Point(17, 198);
			this.label33.Name = "label33";
			this.label33.Size = new global::System.Drawing.Size(45, 15);
			this.label33.TabIndex = 59;
			this.label33.Text = "铂 金";
			this.label12.AutoSize = true;
			this.label12.Font = new global::System.Drawing.Font("宋体", 11f);
			this.label12.Location = new global::System.Drawing.Point(17, 165);
			this.label12.Name = "label12";
			this.label12.Size = new global::System.Drawing.Size(45, 15);
			this.label12.TabIndex = 57;
			this.label12.Text = "白 银";
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Font = new global::System.Drawing.Font("宋体", 11.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.linkLabel1.Location = new global::System.Drawing.Point(12, 35);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new global::System.Drawing.Size(39, 15);
			this.linkLabel1.TabIndex = 66;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "置顶";
			this.linkLabel1.LinkClicked += new global::System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(55, 38);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(41, 12);
			this.label1.TabIndex = 67;
			this.label1.Text = "label1";
			this.label2.AutoSize = true;
			this.label2.Font = new global::System.Drawing.Font("宋体", 11f);
			this.label2.Location = new global::System.Drawing.Point(17, 65);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(45, 15);
			this.label2.TabIndex = 69;
			this.label2.Text = "金 料";
			this.label3.AutoSize = true;
			this.label3.Font = new global::System.Drawing.Font("宋体", 11f);
			this.label3.Location = new global::System.Drawing.Point(16, 99);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(45, 15);
			this.label3.TabIndex = 70;
			this.label3.Text = "成 品";
			this.label4.AutoSize = true;
			this.label4.Font = new global::System.Drawing.Font("宋体", 15f, global::System.Drawing.FontStyle.Bold);
			this.label4.ForeColor = global::System.Drawing.Color.Red;
			this.label4.Location = new global::System.Drawing.Point(76, 60);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(20, 20);
			this.label4.TabIndex = 71;
			this.label4.Text = "0";
			this.label5.AutoSize = true;
			this.label5.Font = new global::System.Drawing.Font("宋体", 15f, global::System.Drawing.FontStyle.Bold);
			this.label5.ForeColor = global::System.Drawing.Color.Red;
			this.label5.Location = new global::System.Drawing.Point(76, 94);
			this.label5.Name = "label5";
			this.label5.Size = new global::System.Drawing.Size(20, 20);
			this.label5.TabIndex = 72;
			this.label5.Text = "0";
			this.label6.AutoSize = true;
			this.label6.Font = new global::System.Drawing.Font("宋体", 15f, global::System.Drawing.FontStyle.Bold);
			this.label6.ForeColor = global::System.Drawing.Color.Red;
			this.label6.Location = new global::System.Drawing.Point(76, 160);
			this.label6.Name = "label6";
			this.label6.Size = new global::System.Drawing.Size(20, 20);
			this.label6.TabIndex = 73;
			this.label6.Text = "0";
			this.label7.AutoSize = true;
			this.label7.Font = new global::System.Drawing.Font("宋体", 15f, global::System.Drawing.FontStyle.Bold);
			this.label7.ForeColor = global::System.Drawing.Color.Red;
			this.label7.Location = new global::System.Drawing.Point(76, 194);
			this.label7.Name = "label7";
			this.label7.Size = new global::System.Drawing.Size(20, 20);
			this.label7.TabIndex = 74;
			this.label7.Text = "0";
			this.label8.AutoSize = true;
			this.label8.Font = new global::System.Drawing.Font("宋体", 15f, global::System.Drawing.FontStyle.Bold);
			this.label8.ForeColor = global::System.Drawing.Color.Red;
			this.label8.Location = new global::System.Drawing.Point(76, 225);
			this.label8.Name = "label8";
			this.label8.Size = new global::System.Drawing.Size(20, 20);
			this.label8.TabIndex = 75;
			this.label8.Text = "0";
			this.label9.AutoSize = true;
			this.label9.Font = new global::System.Drawing.Font("宋体", 15f, global::System.Drawing.FontStyle.Bold);
			this.label9.ForeColor = global::System.Drawing.Color.Red;
			this.label9.Location = new global::System.Drawing.Point(76, 128);
			this.label9.Name = "label9";
			this.label9.Size = new global::System.Drawing.Size(20, 20);
			this.label9.TabIndex = 77;
			this.label9.Text = "0";
			this.label10.AutoSize = true;
			this.label10.Font = new global::System.Drawing.Font("宋体", 11f);
			this.label10.Location = new global::System.Drawing.Point(17, 132);
			this.label10.Name = "label10";
			this.label10.Size = new global::System.Drawing.Size(45, 15);
			this.label10.TabIndex = 76;
			this.label10.Text = "零 售";
			this.pictureBox1.BackColor = global::System.Drawing.Color.White;
			this.pictureBox1.BackgroundImage = (global::System.Drawing.Image)componentResourceManager.GetObject("pictureBox1.BackgroundImage");
			this.pictureBox1.Location = new global::System.Drawing.Point(172, 1);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new global::System.Drawing.Size(16, 20);
			this.pictureBox1.TabIndex = 78;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Click += new global::System.EventHandler(this.PictureBox1_Click);
			this.pictureBox1.MouseEnter += new global::System.EventHandler(this.PictureBox1_MouseEnter);
			this.pictureBox1.MouseLeave += new global::System.EventHandler(this.PictureBox1_MouseLeave);
			this.linkLabel2.AutoSize = true;
			this.linkLabel2.Font = new global::System.Drawing.Font("宋体", 11f, global::System.Drawing.FontStyle.Bold);
			this.linkLabel2.Location = new global::System.Drawing.Point(194, 6);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new global::System.Drawing.Size(23, 15);
			this.linkLabel2.TabIndex = 79;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "×";
			this.linkLabel2.LinkClicked += new global::System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel2_LinkClicked);
			this.linkLabel3.AutoSize = true;
			this.linkLabel3.Font = new global::System.Drawing.Font("宋体", 11.25f, global::System.Drawing.FontStyle.Bold | global::System.Drawing.FontStyle.Italic, global::System.Drawing.GraphicsUnit.Point, 134);
			this.linkLabel3.LinkColor = global::System.Drawing.Color.Black;
			this.linkLabel3.Location = new global::System.Drawing.Point(143, 6);
			this.linkLabel3.Name = "linkLabel3";
			this.linkLabel3.Size = new global::System.Drawing.Size(25, 15);
			this.linkLabel3.TabIndex = 80;
			this.linkLabel3.TabStop = true;
			this.linkLabel3.Text = "__";
			this.linkLabel3.LinkClicked += new global::System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel3_LinkClicked);
			this.linkLabel4.AutoSize = true;
			this.linkLabel4.Font = new global::System.Drawing.Font("宋体", 11.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.linkLabel4.LinkColor = global::System.Drawing.Color.Silver;
			this.linkLabel4.Location = new global::System.Drawing.Point(88, 6);
			this.linkLabel4.Name = "linkLabel4";
			this.linkLabel4.Size = new global::System.Drawing.Size(39, 15);
			this.linkLabel4.TabIndex = 81;
			this.linkLabel4.TabStop = true;
			this.linkLabel4.Text = "开始";
			this.linkLabel4.LinkClicked += new global::System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel4_LinkClicked);
			this.label11.AutoSize = true;
			this.label11.Font = new global::System.Drawing.Font("宋体", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label11.ForeColor = global::System.Drawing.Color.Red;
			this.label11.Location = new global::System.Drawing.Point(6, 5);
			this.label11.Name = "label11";
			this.label11.Size = new global::System.Drawing.Size(76, 16);
			this.label11.TabIndex = 82;
			this.label11.Text = "大海金行";
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.White;
			base.ClientSize = new global::System.Drawing.Size(219, 255);
			base.Controls.Add(this.label11);
			base.Controls.Add(this.linkLabel4);
			base.Controls.Add(this.linkLabel3);
			base.Controls.Add(this.linkLabel2);
			base.Controls.Add(this.pictureBox1);
			base.Controls.Add(this.label9);
			base.Controls.Add(this.label10);
			base.Controls.Add(this.label8);
			base.Controls.Add(this.label7);
			base.Controls.Add(this.label6);
			base.Controls.Add(this.label5);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.linkLabel1);
			base.Controls.Add(this.label40);
			base.Controls.Add(this.label33);
			base.Controls.Add(this.label12);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.None;
			base.MaximizeBox = false;
			base.Name = "子窗体";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "主程序";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.子窗体_FormClosing);
			base.Load += new global::System.EventHandler(this.子窗体_Load);
			base.MouseDown += new global::System.Windows.Forms.MouseEventHandler(this.子窗体_MouseDown);
			base.MouseEnter += new global::System.EventHandler(this.子窗体_MouseEnter);
			base.MouseMove += new global::System.Windows.Forms.MouseEventHandler(this.子窗体_MouseMove);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400013A RID: 314
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x0400013B RID: 315
		private global::System.Windows.Forms.Label label40;

		// Token: 0x0400013C RID: 316
		private global::System.Windows.Forms.Label label33;

		// Token: 0x0400013D RID: 317
		private global::System.Windows.Forms.Label label12;

		// Token: 0x0400013E RID: 318
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x0400013F RID: 319
		private global::System.Windows.Forms.LinkLabel linkLabel1;

		// Token: 0x04000140 RID: 320
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000141 RID: 321
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000142 RID: 322
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04000143 RID: 323
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04000144 RID: 324
		private global::System.Windows.Forms.Label label5;

		// Token: 0x04000145 RID: 325
		private global::System.Windows.Forms.Label label6;

		// Token: 0x04000146 RID: 326
		private global::System.Windows.Forms.Label label7;

		// Token: 0x04000147 RID: 327
		private global::System.Windows.Forms.Label label8;

		// Token: 0x04000148 RID: 328
		private global::System.Windows.Forms.Label label9;

		// Token: 0x04000149 RID: 329
		private global::System.Windows.Forms.Label label10;

		// Token: 0x0400014A RID: 330
		private global::System.Windows.Forms.PictureBox pictureBox1;

		// Token: 0x0400014B RID: 331
		private global::System.Windows.Forms.LinkLabel linkLabel2;

		// Token: 0x0400014C RID: 332
		private global::System.Windows.Forms.LinkLabel linkLabel3;

		// Token: 0x0400014D RID: 333
		private global::System.Windows.Forms.LinkLabel linkLabel4;

		// Token: 0x0400014E RID: 334
		private global::System.Windows.Forms.Label label11;
	}
}
