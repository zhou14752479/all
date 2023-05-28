namespace Background
{
	// Token: 0x0200000B RID: 11
	public partial class ForMain : global::MUWEN.ForParent
	{
		// Token: 0x0600009C RID: 156 RVA: 0x0000B368 File Offset: 0x00009568
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			bool flag2 = flag;
			if (flag2)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0000B3A4 File Offset: 0x000095A4
		private void InitializeComponent()
		{
			this.MenMain = new global::System.Windows.Forms.MenuStrip();
			this.MenSystem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.MenExit = new global::System.Windows.Forms.ToolStripMenuItem();
			this.编辑ToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.MenAnimal = new global::System.Windows.Forms.ToolStripMenuItem();
			this.MenDaiLi = new global::System.Windows.Forms.ToolStripMenuItem();
			this.MenMySetting = new global::System.Windows.Forms.ToolStripMenuItem();
			this.工具ToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.MenExtractDataJianJie = new global::System.Windows.Forms.ToolStripMenuItem();
			this.MenExtractData = new global::System.Windows.Forms.ToolStripMenuItem();
			this.MenKaiBao = new global::System.Windows.Forms.ToolStripMenuItem();
			this.StatusStrip1 = new global::System.Windows.Forms.StatusStrip();
			this.MenMain.SuspendLayout();
			base.SuspendLayout();
			this.MenMain.ImageScalingSize = new global::System.Drawing.Size(20, 20);
			this.MenMain.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.MenSystem,
				this.编辑ToolStripMenuItem,
				this.工具ToolStripMenuItem
			});
			this.MenMain.Location = new global::System.Drawing.Point(0, 0);
			this.MenMain.Name = "MenMain";
			this.MenMain.Padding = new global::System.Windows.Forms.Padding(5, 2, 0, 2);
			this.MenMain.Size = new global::System.Drawing.Size(812, 25);
			this.MenMain.TabIndex = 1;
			this.MenMain.Text = "MenuStrip1";
			this.MenSystem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.MenExit
			});
			this.MenSystem.Name = "MenSystem";
			this.MenSystem.Size = new global::System.Drawing.Size(44, 21);
			this.MenSystem.Text = "系统";
			this.MenExit.Name = "MenExit";
			this.MenExit.Size = new global::System.Drawing.Size(100, 22);
			this.MenExit.Text = "退出";
			this.MenExit.Click += new global::System.EventHandler(this.MenExit_Click);
			this.编辑ToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.MenAnimal,
				this.MenDaiLi,
				this.MenMySetting
			});
			this.编辑ToolStripMenuItem.Name = "编辑ToolStripMenuItem";
			this.编辑ToolStripMenuItem.Size = new global::System.Drawing.Size(44, 21);
			this.编辑ToolStripMenuItem.Text = "编辑";
			this.MenAnimal.Name = "MenAnimal";
			this.MenAnimal.Size = new global::System.Drawing.Size(180, 22);
			this.MenAnimal.Text = "动物名管理";
			this.MenAnimal.Click += new global::System.EventHandler(this.MenAnimal_Click);
			this.MenDaiLi.Name = "MenDaiLi";
			this.MenDaiLi.Size = new global::System.Drawing.Size(180, 22);
			this.MenDaiLi.Text = "代理人管理";
			this.MenDaiLi.Click += new global::System.EventHandler(this.MenDaiLi_Click);
			this.MenMySetting.Name = "MenMySetting";
			this.MenMySetting.Size = new global::System.Drawing.Size(180, 22);
			this.MenMySetting.Text = "参数设置";
			this.MenMySetting.Click += new global::System.EventHandler(this.MenMySetting_Click);
			this.工具ToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.MenExtractDataJianJie,
				this.MenExtractData,
				this.MenKaiBao
			});
			this.工具ToolStripMenuItem.Name = "工具ToolStripMenuItem";
			this.工具ToolStripMenuItem.Size = new global::System.Drawing.Size(44, 21);
			this.工具ToolStripMenuItem.Text = "工具";
			this.MenExtractDataJianJie.Name = "MenExtractDataJianJie";
			this.MenExtractDataJianJie.Size = new global::System.Drawing.Size(180, 22);
			this.MenExtractDataJianJie.Text = "数据分析(简捷版)";
			this.MenExtractDataJianJie.Click += new global::System.EventHandler(this.MenExtractDataJianJie_Click);
			this.MenExtractData.Enabled = false;
			this.MenExtractData.Name = "MenExtractData";
			this.MenExtractData.Size = new global::System.Drawing.Size(180, 22);
			this.MenExtractData.Text = "数据分析(高级版)";
			this.MenExtractData.Click += new global::System.EventHandler(this.MenExtractData_Click);
			this.MenKaiBao.Name = "MenKaiBao";
			this.MenKaiBao.Size = new global::System.Drawing.Size(180, 22);
			this.MenKaiBao.Text = "开宝记录";
			this.MenKaiBao.Click += new global::System.EventHandler(this.MenKaiBao_Click);
			this.StatusStrip1.ImageScalingSize = new global::System.Drawing.Size(20, 20);
			this.StatusStrip1.Location = new global::System.Drawing.Point(0, 382);
			this.StatusStrip1.Name = "StatusStrip1";
			this.StatusStrip1.Padding = new global::System.Windows.Forms.Padding(1, 0, 12, 0);
			this.StatusStrip1.Size = new global::System.Drawing.Size(812, 22);
			this.StatusStrip1.TabIndex = 2;
			this.StatusStrip1.Text = "StatusStrip1";
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(10f, 21f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(812, 404);
			base.Controls.Add(this.StatusStrip1);
			base.Controls.Add(this.MenMain);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.Sizable;
			base.IsMdiContainer = true;
			base.MainMenuStrip = this.MenMain;
			base.Margin = new global::System.Windows.Forms.Padding(3, 4, 3, 4);
			base.MaximizeBox = true;
			base.MinimizeBox = true;
			base.Name = "ForMain";
			this.Text = "臻品定制";
			base.WindowState = global::System.Windows.Forms.FormWindowState.Maximized;
			base.Load += new global::System.EventHandler(this.ForMain_Load);
			this.MenMain.ResumeLayout(false);
			this.MenMain.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400009F RID: 159
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x040000A0 RID: 160
		private global::System.Windows.Forms.MenuStrip MenMain;

		// Token: 0x040000A1 RID: 161
		private global::System.Windows.Forms.ToolStripMenuItem MenSystem;

		// Token: 0x040000A2 RID: 162
		private global::System.Windows.Forms.ToolStripMenuItem MenExit;

		// Token: 0x040000A3 RID: 163
		private global::System.Windows.Forms.StatusStrip StatusStrip1;

		// Token: 0x040000A4 RID: 164
		private global::System.Windows.Forms.ToolStripMenuItem 编辑ToolStripMenuItem;

		// Token: 0x040000A5 RID: 165
		private global::System.Windows.Forms.ToolStripMenuItem MenAnimal;

		// Token: 0x040000A6 RID: 166
		private global::System.Windows.Forms.ToolStripMenuItem 工具ToolStripMenuItem;

		// Token: 0x040000A7 RID: 167
		private global::System.Windows.Forms.ToolStripMenuItem MenExtractData;

		// Token: 0x040000A8 RID: 168
		private global::System.Windows.Forms.ToolStripMenuItem MenDaiLi;

		// Token: 0x040000A9 RID: 169
		private global::System.Windows.Forms.ToolStripMenuItem MenMySetting;

		// Token: 0x040000AA RID: 170
		private global::System.Windows.Forms.ToolStripMenuItem MenKaiBao;

		// Token: 0x040000AB RID: 171
		private global::System.Windows.Forms.ToolStripMenuItem MenExtractDataJianJie;
	}
}
