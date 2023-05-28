using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Common;
using MUWEN;

namespace Background
{
	// Token: 0x02000012 RID: 18
	public class UseKaiBao : UserControl
	{
		// Token: 0x060000BA RID: 186 RVA: 0x0000D040 File Offset: 0x0000B240
		public UseKaiBao(string date, string diyichang, string dierchang, string disanchang, string disichang)
		{
			this.InitializeComponent();
			this.LblDate.Text = date;
			this.LblDiYiChang.Text = diyichang;
			this.LblDiSanChang.Text = disanchang;
			this.LblDiErChang.Text = dierchang;
			this.disichang.Text = disichang;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000D0A8 File Offset: 0x0000B2A8
		public UseKaiBao(string date, List<ChangCi> ccs)
		{
			this.InitializeComponent();
			this.LblDate.Text = date;
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			bool flag = ccs.Count > 0;
			bool flag2 = flag;
			if (flag2)
			{
				this.LblDiYiChang.Tag = ccs[0].ID + "1";
				foreach (Animal animal in ccs[0].Animals)
				{
					text = text + animal.Name + " ";
				}
				this.LabelSetValue(this.LblDiYiChang, text);
			}
			bool flag3 = ccs.Count > 1;
			bool flag4 = flag3;
			if (flag4)
			{
				this.LblDiErChang.Tag = ccs[1].ID + "2";
				foreach (Animal animal2 in ccs[1].Animals)
				{
					text2 = text2 + animal2.Name + " ";
				}
				this.LabelSetValue(this.LblDiErChang, text2);
			}
			bool flag5 = ccs.Count > 2;
			bool flag6 = flag5;
			if (flag6)
			{
				this.LblDiSanChang.Tag = ccs[2].ID + "3";
				foreach (Animal animal3 in ccs[2].Animals)
				{
					text3 = text3 + animal3.Name + " ";
				}
				this.LabelSetValue(this.LblDiSanChang, text3);
			}
			bool flag7 = ccs.Count > 3;
			bool flag8 = flag7;
			if (flag8)
			{
				this.disichang.Tag = ccs[3].ID + "4";
				foreach (Animal animal4 in ccs[3].Animals)
				{
					text4 = text4 + animal4.Name + " ";
				}
				this.LabelSetValue(this.disichang, text4);
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000D37C File Offset: 0x0000B57C
		private void LabelSetValue(Label l, string value)
		{
			l.Text = value;
			bool flag = !value.IsNullOrEmpty();
			bool flag2 = flag;
			if (flag2)
			{
				l.Cursor = Cursors.Hand;
				l.ForeColor = Color.Black;
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000D3BC File Offset: 0x0000B5BC
		private void LblDiYiChang_Click(object sender, EventArgs e)
		{
			Label label = sender as Label;
			bool flag = label.Text.IsNullOrEmpty() || label.Tag.IsNull();
			bool flag2 = !flag;
			if (flag2)
			{
				string changciid = label.Tag.ToString().Substring(0, 32);
				string changcicount = label.Tag.ToString().Substring(32, 1);
				ForLenBao forLenBao = new ForLenBao(changciid, changcicount);
				forLenBao.ShowDialog();
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000D434 File Offset: 0x0000B634
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

		// Token: 0x060000BF RID: 191 RVA: 0x0000D470 File Offset: 0x0000B670
		private void InitializeComponent()
		{
			this.disichang = new Label();
			this.LblDiYiChang = new Label();
			this.LblDiErChang = new Label();
			this.LblDiSanChang = new Label();
			this.LblDate = new Label();
			base.SuspendLayout();
			this.disichang.Dock = DockStyle.Right;
			this.disichang.Location = new Point(388, 0);
			this.disichang.Name = "disichang";
			this.disichang.Size = new Size(102, 44);
			this.disichang.TabIndex = 2;
			this.disichang.TextAlign = ContentAlignment.MiddleCenter;
			this.disichang.Click += this.LblDiYiChang_Click;
			this.LblDiYiChang.Dock = DockStyle.Right;
			this.LblDiYiChang.Location = new Point(103, 0);
			this.LblDiYiChang.Name = "LblDiYiChang";
			this.LblDiYiChang.Size = new Size(95, 44);
			this.LblDiYiChang.TabIndex = 3;
			this.LblDiYiChang.TextAlign = ContentAlignment.MiddleCenter;
			this.LblDiYiChang.Click += this.LblDiYiChang_Click;
			this.LblDiErChang.Dock = DockStyle.Right;
			this.LblDiErChang.Location = new Point(198, 0);
			this.LblDiErChang.Name = "LblDiErChang";
			this.LblDiErChang.Size = new Size(95, 44);
			this.LblDiErChang.TabIndex = 4;
			this.LblDiErChang.TextAlign = ContentAlignment.MiddleCenter;
			this.LblDiErChang.Click += this.LblDiYiChang_Click;
			this.LblDiSanChang.Dock = DockStyle.Right;
			this.LblDiSanChang.Location = new Point(293, 0);
			this.LblDiSanChang.Name = "LblDiSanChang";
			this.LblDiSanChang.Size = new Size(95, 44);
			this.LblDiSanChang.TabIndex = 5;
			this.LblDiSanChang.TextAlign = ContentAlignment.MiddleCenter;
			this.LblDiSanChang.Click += this.LblDiYiChang_Click;
			this.LblDate.Dock = DockStyle.Fill;
			this.LblDate.Location = new Point(0, 0);
			this.LblDate.Name = "LblDate";
			this.LblDate.Size = new Size(103, 44);
			this.LblDate.TabIndex = 6;
			this.LblDate.TextAlign = ContentAlignment.MiddleCenter;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.LblDate);
			base.Controls.Add(this.LblDiYiChang);
			base.Controls.Add(this.LblDiErChang);
			base.Controls.Add(this.LblDiSanChang);
			base.Controls.Add(this.disichang);
			base.Name = "UseKaiBao";
			base.Size = new Size(490, 44);
			base.ResumeLayout(false);
		}

		// Token: 0x040000C9 RID: 201
		private Label disichang;

		// Token: 0x040000CA RID: 202
		private Label LblDiYiChang;

		// Token: 0x040000CB RID: 203
		private Label LblDiErChang;

		// Token: 0x040000CC RID: 204
		private Label LblDiSanChang;

		// Token: 0x040000CD RID: 205
		private Label LblDate;

		// Token: 0x040000CE RID: 206
		private IContainer components = null;
	}
}
