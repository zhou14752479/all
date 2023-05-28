using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Common;
using MUWEN;

namespace Background
{
	// Token: 0x02000009 RID: 9
	public partial class ForKaiBao : ForParent
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600007B RID: 123 RVA: 0x0000A116 File Offset: 0x00008316
		// (set) Token: 0x0600007C RID: 124 RVA: 0x0000A11E File Offset: 0x0000831E
		public Manager Manager { get; set; }

		// Token: 0x0600007D RID: 125 RVA: 0x0000A127 File Offset: 0x00008327
		public ForKaiBao()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000A140 File Offset: 0x00008340
		private void Init()
		{
			this.Manager = new Manager();
			this.LblTitle.Text = string.Format("{0}截止{1}月份开宝记录", this.Manager.MySetting.OwnerName, DateTime.Now.Month);
			int num = 13;
			UseKaiBao value = new UseKaiBao(string.Format("{0}年", DateTime.Now.Year), "第一场", "第二场", "第三场", "第四场")
			{
				Location = new Point(0, 0),
				Height = num
			};
			this.PanKaiBao.Controls.Add(value);
			int i;
			
			int k;
			for (i = 0; i < DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i = k + 1)
			{
				IEnumerable<ChangCi> changCis = this.Manager.ChangCis;
				Func<ChangCi, bool> predicate=null;
				//if ((predicate = <>9__0) == null)
				//{
				//	predicate = (<>9__0 = ((ChangCi j) => j.Createtime.ToDate("-", "-", "") == string.Format("{0:yyyy-MM}-{1:00}", DateTime.Now, i + 1)));
				//}
				List<ChangCi> ccs = changCis.Where(predicate).ToList<ChangCi>();
				UseKaiBao value2 = new UseKaiBao(string.Format("{0:00}月{1:00}日", DateTime.Now.Month, i + 1), ccs)
				{
					Location = new Point(0, num * (i + 1)),
					Height = num
				};
				this.PanKaiBao.Controls.Add(value2);
				k = i;
			}
		}
		

		// Token: 0x0600007F RID: 127 RVA: 0x0000A2E8 File Offset: 0x000084E8
		private void ForKaiBao_Load(object sender, EventArgs e)
		{
			this.Init();
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000A2F2 File Offset: 0x000084F2
		private void BtnJieTu_Click(object sender, EventArgs e)
		{
			this.Manager.JieTu(this, new Point(20, 80), new Size(-5, 40));
		}
	}
}
