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
	// Token: 0x0200000A RID: 10
	public partial class ForLenBao : ForParent
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000083 RID: 131 RVA: 0x0000A685 File Offset: 0x00008885
		// (set) Token: 0x06000084 RID: 132 RVA: 0x0000A68D File Offset: 0x0000888D
		private string CurrentChangCiID { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000085 RID: 133 RVA: 0x0000A696 File Offset: 0x00008896
		// (set) Token: 0x06000086 RID: 134 RVA: 0x0000A69E File Offset: 0x0000889E
		private string ChangCiCount { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000087 RID: 135 RVA: 0x0000A6A7 File Offset: 0x000088A7
		// (set) Token: 0x06000088 RID: 136 RVA: 0x0000A6AF File Offset: 0x000088AF
		private Manager Manager { get; set; }

		// Token: 0x06000089 RID: 137 RVA: 0x0000A6B8 File Offset: 0x000088B8
		public ForLenBao(string changciid, string changcicount)
		{
			this.InitializeComponent();
			this.CurrentChangCiID = changciid;
			this.ChangCiCount = changcicount;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000A6E0 File Offset: 0x000088E0
		private void Init()
		{
			this.Manager = new Manager();
			this.LblTitle.Text = string.Format("{0}截至{1:yyyy年M月d日}第{2}场冷宝统计", this.Manager.MySetting.OwnerName, DateTime.Now, this.ChangCiCount);
			string a = "";
			foreach (ChangCi changCi in this.Manager.ChangCis)
			{
				foreach (Animal animal in this.Manager.Animals)
				{
					Animal animal2 = animal;
					int num = animal2.WeiKaiChangShu;
					animal2.WeiKaiChangShu = num + 1;
					bool flag = a != changCi.Createtime.ToDate("-", "-", "");
					bool flag2 = flag;
					if (flag2)
					{
						Animal animal3 = animal;
						num = animal3.WeiKaiTianShu;
						animal3.WeiKaiTianShu = num + 1;
					}
				}
				a = changCi.Createtime.ToDate("-", "-", "");
				using (List<Animal>.Enumerator enumerator3 = changCi.Animals.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Animal item2 = enumerator3.Current;
						Animal animal4 = (from i in this.Manager.Animals
						where i.ID == item2.ID
						select i).FirstOrDefault<Animal>();
						animal4.WeiKaiChangShu = 0;
						animal4.WeiKaiTianShu = 0;
					}
				}
				bool flag3 = changCi.ID == this.CurrentChangCiID;
				bool flag4 = flag3;
				if (flag4)
				{
					break;
				}
			}
			foreach (Animal animal5 in this.Manager.Animals)
			{
				animal5.WeiKaiChangShu += animal5.WeiKaiChangShuPianYi;
				animal5.WeiKaiTianShu += animal5.WeiKaiTianShuPianYi;
			}
			ChangCi changCi2 = (from i in this.Manager.ChangCis
			where i.ID == this.CurrentChangCiID
			select i).FirstOrDefault<ChangCi>();
			using (List<Animal>.Enumerator enumerator5 = changCi2.Animals.GetEnumerator())
			{
				while (enumerator5.MoveNext())
				{
					Animal item = enumerator5.Current;
					Animal animal6 = (from i in this.Manager.Animals
					where i.ID == item.ID
					select i).FirstOrDefault<Animal>();
					animal6.WeiKaiChangShu = 0;
					animal6.WeiKaiTianShu = 0;
				}
			}
			int num2 = this.Manager.Animals.Max((Animal i) => i.WeiKaiChangShu);
			foreach (Animal animal7 in this.Manager.Animals)
			{
				animal7.BiLi = (int)((double)animal7.WeiKaiChangShu * 1.0 / (double)num2 * 100.0);
			}
			this.Manager.Animals = (from i in this.Manager.Animals
			orderby i.WeiKaiChangShu
			select i).ToList<Animal>();
			foreach (Animal animal8 in this.Manager.Animals)
			{
				UseLenBao useLenBao = new UseLenBao(animal8);
				useLenBao.Dock = DockStyle.Top;
				useLenBao.Height = 12;
				useLenBao.Parent = this.PanContent;
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000ABAC File Offset: 0x00008DAC
		private void ForLenBao_Load(object sender, EventArgs e)
		{
			this.Init();
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0000ABB6 File Offset: 0x00008DB6
		private void BtnJieTu_Click(object sender, EventArgs e)
		{
			this.Manager.JieTu(this, new Point(7, 30), new Size(-180, -7));
		}
	}
}
