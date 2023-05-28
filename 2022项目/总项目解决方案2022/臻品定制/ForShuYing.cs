using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using MUWEN;

namespace Background
{
	// Token: 0x0200000D RID: 13
	public partial class ForShuYing : ForParent
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x0000BFBB File Offset: 0x0000A1BB
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x0000BFC3 File Offset: 0x0000A1C3
		public DaiLi CurrentDaiLi { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x0000BFCC File Offset: 0x0000A1CC
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x0000BFD4 File Offset: 0x0000A1D4
		public Manager Manager { get; set; } = new Manager();

		// Token: 0x060000AA RID: 170 RVA: 0x0000BFDD File Offset: 0x0000A1DD
		public ForShuYing(DaiLi dl)
		{
			this.InitializeComponent();
			this.CurrentDaiLi = dl;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000C008 File Offset: 0x0000A208
		private void ForShuYing_Load(object sender, EventArgs e)
		{
			this.Init();
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0000C014 File Offset: 0x0000A214
		private void Init()
		{
			this.LblTotalCount.Text = string.Format("￥{0}", this.CurrentDaiLi.TotalCount);
			foreach (Animal animal in from i in this.CurrentDaiLi.Animals
			orderby i.ShuYing descending
			select i)
			{
				UseShuYing useShuYing = new UseShuYing(animal.Name, animal.Count, animal.ShuYing, animal.BiLi);
				useShuYing.Dock = DockStyle.Top;
				useShuYing.Height = 12;
				useShuYing.Parent = this.PanAnimals;
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000C0EC File Offset: 0x0000A2EC
		private void BtnJieTu_Click(object sender, EventArgs e)
		{
			this.Manager.JieTu(this, new Point(27, 30), new Size(-80, -7));
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000C110 File Offset: 0x0000A310
		private void BtnCopy_Click(object sender, EventArgs e)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Animal animal in from i in this.CurrentDaiLi.Animals
			orderby i.Count descending
			select i)
			{
				stringBuilder.AppendLine(string.Format("{0} ￥{1} {2}", animal.Name, animal.Count, animal.ShuYing));
			}
			Clipboard.SetText(stringBuilder.ToString(), TextDataFormat.Text);
		}
	}
}
