using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Background
{
	// Token: 0x02000011 RID: 17
	public class UseChangCi : UserControl
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x0000CAB4 File Offset: 0x0000ACB4
		public UseChangCi(string changci, string zongyazhue, string dailifei, string zhongbaoyazhu, string zhongbaoxuyaopeideqian, string shuying)
		{
			this.InitializeComponent();
			this.LblChangCi.Text = changci;
			this.LblZongYaZhuE.Text = zongyazhue;
			this.LblDaiLiFei.Text = dailifei;
			this.LblZhongBaoYaZhu.Text = zhongbaoyazhu;
			this.LblZhongBaoXuYaoPeiDeQian.Text = zhongbaoxuyaopeideqian;
			this.LblShuYing.Text = shuying;
			this.LblShuYing.ForeColor = (shuying.Contains("-") ? Color.Red : Color.Black);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000CB50 File Offset: 0x0000AD50
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

		// Token: 0x060000B9 RID: 185 RVA: 0x0000CB8C File Offset: 0x0000AD8C
		private void InitializeComponent()
		{
			this.LblChangCi = new Label();
			this.LblZongYaZhuE = new Label();
			this.LblDaiLiFei = new Label();
			this.LblZhongBaoYaZhu = new Label();
			this.LblZhongBaoXuYaoPeiDeQian = new Label();
			this.LblShuYing = new Label();
			base.SuspendLayout();
			this.LblChangCi.Dock = DockStyle.Left;
			this.LblChangCi.Font = new Font("微软雅黑", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.LblChangCi.Location = new Point(0, 0);
			this.LblChangCi.Name = "LblChangCi";
			this.LblChangCi.Size = new Size(73, 29);
			this.LblChangCi.TabIndex = 0;
			this.LblChangCi.Text = "场次";
			this.LblChangCi.TextAlign = ContentAlignment.MiddleCenter;
			this.LblZongYaZhuE.Dock = DockStyle.Left;
			this.LblZongYaZhuE.Font = new Font("微软雅黑", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.LblZongYaZhuE.Location = new Point(73, 0);
			this.LblZongYaZhuE.Name = "LblZongYaZhuE";
			this.LblZongYaZhuE.Size = new Size(51, 29);
			this.LblZongYaZhuE.TabIndex = 0;
			this.LblZongYaZhuE.Text = "场次";
			this.LblZongYaZhuE.TextAlign = ContentAlignment.MiddleCenter;
			this.LblDaiLiFei.Dock = DockStyle.Left;
			this.LblDaiLiFei.Font = new Font("微软雅黑", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.LblDaiLiFei.Location = new Point(124, 0);
			this.LblDaiLiFei.Name = "LblDaiLiFei";
			this.LblDaiLiFei.Size = new Size(58, 29);
			this.LblDaiLiFei.TabIndex = 0;
			this.LblDaiLiFei.Text = "场次";
			this.LblDaiLiFei.TextAlign = ContentAlignment.MiddleCenter;
			this.LblZhongBaoYaZhu.Dock = DockStyle.Left;
			this.LblZhongBaoYaZhu.Font = new Font("微软雅黑", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.LblZhongBaoYaZhu.Location = new Point(182, 0);
			this.LblZhongBaoYaZhu.Name = "LblZhongBaoYaZhu";
			this.LblZhongBaoYaZhu.Size = new Size(101, 29);
			this.LblZhongBaoYaZhu.TabIndex = 0;
			this.LblZhongBaoYaZhu.Text = "场次";
			this.LblZhongBaoYaZhu.TextAlign = ContentAlignment.MiddleCenter;
			this.LblZhongBaoXuYaoPeiDeQian.Dock = DockStyle.Left;
			this.LblZhongBaoXuYaoPeiDeQian.Font = new Font("微软雅黑", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.LblZhongBaoXuYaoPeiDeQian.Location = new Point(283, 0);
			this.LblZhongBaoXuYaoPeiDeQian.Name = "LblZhongBaoXuYaoPeiDeQian";
			this.LblZhongBaoXuYaoPeiDeQian.Size = new Size(66, 29);
			this.LblZhongBaoXuYaoPeiDeQian.TabIndex = 0;
			this.LblZhongBaoXuYaoPeiDeQian.Text = "场次";
			this.LblZhongBaoXuYaoPeiDeQian.TextAlign = ContentAlignment.MiddleCenter;
			this.LblShuYing.Dock = DockStyle.Fill;
			this.LblShuYing.Font = new Font("微软雅黑", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.LblShuYing.Location = new Point(349, 0);
			this.LblShuYing.Name = "LblShuYing";
			this.LblShuYing.Size = new Size(206, 29);
			this.LblShuYing.TabIndex = 0;
			this.LblShuYing.Text = "场次";
			this.LblShuYing.TextAlign = ContentAlignment.MiddleCenter;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.FromArgb(249, 191, 143);
			base.BorderStyle = BorderStyle.FixedSingle;
			base.Controls.Add(this.LblShuYing);
			base.Controls.Add(this.LblZhongBaoXuYaoPeiDeQian);
			base.Controls.Add(this.LblZhongBaoYaZhu);
			base.Controls.Add(this.LblDaiLiFei);
			base.Controls.Add(this.LblZongYaZhuE);
			base.Controls.Add(this.LblChangCi);
			base.Name = "UseChangCi";
			base.Size = new Size(555, 29);
			base.ResumeLayout(false);
		}

		// Token: 0x040000C2 RID: 194
		private IContainer components = null;

		// Token: 0x040000C3 RID: 195
		private Label LblChangCi;

		// Token: 0x040000C4 RID: 196
		private Label LblZongYaZhuE;

		// Token: 0x040000C5 RID: 197
		private Label LblDaiLiFei;

		// Token: 0x040000C6 RID: 198
		private Label LblZhongBaoYaZhu;

		// Token: 0x040000C7 RID: 199
		private Label LblZhongBaoXuYaoPeiDeQian;

		// Token: 0x040000C8 RID: 200
		private Label LblShuYing;
	}
}
