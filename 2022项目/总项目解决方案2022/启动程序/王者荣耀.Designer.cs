namespace 启动程序
{
	// Token: 0x02000012 RID: 18
	public partial class 王者荣耀 : global::System.Windows.Forms.Form
	{
		// Token: 0x060000AD RID: 173 RVA: 0x00016F04 File Offset: 0x00015104
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00016F3C File Offset: 0x0001513C
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::启动程序.王者荣耀));
			this.tabControl1 = new global::System.Windows.Forms.TabControl();
			this.tabPage1 = new global::System.Windows.Forms.TabPage();
			this.webBrowser1 = new global::System.Windows.Forms.WebBrowser();
			this.tabPage2 = new global::System.Windows.Forms.TabPage();
			this.textBox4 = new global::System.Windows.Forms.TextBox();
			this.textBox3 = new global::System.Windows.Forms.TextBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.textBox2 = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.button2 = new global::System.Windows.Forms.Button();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.button1 = new global::System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			base.SuspendLayout();
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new global::System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new global::System.Drawing.Size(936, 608);
			this.tabControl1.TabIndex = 0;
			this.tabPage1.Controls.Add(this.webBrowser1);
			this.tabPage1.Location = new global::System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new global::System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new global::System.Drawing.Size(928, 582);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "登  陆";
			this.tabPage1.UseVisualStyleBackColor = true;
			this.webBrowser1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.webBrowser1.Location = new global::System.Drawing.Point(3, 3);
			this.webBrowser1.MinimumSize = new global::System.Drawing.Size(20, 20);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.Size = new global::System.Drawing.Size(922, 576);
			this.webBrowser1.TabIndex = 0;
			this.tabPage2.Controls.Add(this.textBox4);
			this.tabPage2.Controls.Add(this.textBox3);
			this.tabPage2.Controls.Add(this.label2);
			this.tabPage2.Controls.Add(this.textBox2);
			this.tabPage2.Controls.Add(this.label1);
			this.tabPage2.Controls.Add(this.button2);
			this.tabPage2.Controls.Add(this.textBox1);
			this.tabPage2.Controls.Add(this.button1);
			this.tabPage2.Location = new global::System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new global::System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new global::System.Drawing.Size(928, 582);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "抓  取";
			this.tabPage2.UseVisualStyleBackColor = true;
			this.textBox4.Font = new global::System.Drawing.Font("宋体", 10f);
			this.textBox4.Location = new global::System.Drawing.Point(17, 103);
			this.textBox4.Multiline = true;
			this.textBox4.Name = "textBox4";
			this.textBox4.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this.textBox4.Size = new global::System.Drawing.Size(212, 471);
			this.textBox4.TabIndex = 7;
			this.textBox4.Text = componentResourceManager.GetString("textBox4.Text");
			this.textBox3.Location = new global::System.Drawing.Point(280, 44);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new global::System.Drawing.Size(100, 21);
			this.textBox3.TabIndex = 6;
			this.textBox3.Text = "1011";
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(221, 47);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(53, 12);
			this.label2.TabIndex = 5;
			this.label2.Text = "大区ID：";
			this.textBox2.Location = new global::System.Drawing.Point(101, 44);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new global::System.Drawing.Size(100, 21);
			this.textBox2.TabIndex = 4;
			this.textBox2.Text = "1";
			this.textBox2.Visible = false;
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(41, 47);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(53, 12);
			this.label1.TabIndex = 3;
			this.label1.Text = "选择渠道";
			this.label1.Visible = false;
			this.button2.Location = new global::System.Drawing.Point(782, 38);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(129, 43);
			this.button2.TabIndex = 2;
			this.button2.Text = "清空";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.button2_Click);
			this.textBox1.Font = new global::System.Drawing.Font("宋体", 11f);
			this.textBox1.Location = new global::System.Drawing.Point(235, 103);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new global::System.Drawing.Size(690, 476);
			this.textBox1.TabIndex = 1;
			this.button1.Location = new global::System.Drawing.Point(628, 38);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(129, 43);
			this.button1.TabIndex = 0;
			this.button1.Text = "开始抓取";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(936, 608);
			base.Controls.Add(this.tabControl1);
			base.Name = "王者荣耀";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "王者荣耀";
			base.Load += new global::System.EventHandler(this.王者荣耀_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x040001D8 RID: 472
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x040001D9 RID: 473
		private global::System.Windows.Forms.TabControl tabControl1;

		// Token: 0x040001DA RID: 474
		private global::System.Windows.Forms.TabPage tabPage1;

		// Token: 0x040001DB RID: 475
		private global::System.Windows.Forms.WebBrowser webBrowser1;

		// Token: 0x040001DC RID: 476
		private global::System.Windows.Forms.TabPage tabPage2;

		// Token: 0x040001DD RID: 477
		private global::System.Windows.Forms.TextBox textBox1;

		// Token: 0x040001DE RID: 478
		private global::System.Windows.Forms.Button button1;

		// Token: 0x040001DF RID: 479
		private global::System.Windows.Forms.Button button2;

		// Token: 0x040001E0 RID: 480
		private global::System.Windows.Forms.TextBox textBox2;

		// Token: 0x040001E1 RID: 481
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040001E2 RID: 482
		private global::System.Windows.Forms.TextBox textBox3;

		// Token: 0x040001E3 RID: 483
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040001E4 RID: 484
		private global::System.Windows.Forms.TextBox textBox4;
	}
}
