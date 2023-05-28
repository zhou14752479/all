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
	// Token: 0x02000008 RID: 8
	public partial class ForExtractDataJianJie : ForParent
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00008D62 File Offset: 0x00006F62
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00008D6A File Offset: 0x00006F6A
		public Manager Manager { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00008D73 File Offset: 0x00006F73
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00008D7B File Offset: 0x00006F7B
		public DaiLi CurrentDaiLi { get; set; }

		// Token: 0x0600006D RID: 109 RVA: 0x00008D84 File Offset: 0x00006F84
		public ForExtractDataJianJie()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00008D9C File Offset: 0x00006F9C
		private void ForExtractData_Load(object sender, EventArgs e)
		{
			this.Init();
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00008DA8 File Offset: 0x00006FA8
		private void Init()
		{
			this.Manager = new Manager();
			this.LstDaiLi.DisplayMember = "name";
			this.LstDaiLi.DataSource = this.Manager.DaiLis;
			this.BtnChangCi.Enabled = (this.BtnTotalShuYing.Enabled = (this.Manager.DaiLis.Count != 0));
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00008E18 File Offset: 0x00007018
		private void LstDaiLi_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool flag = this.LstDaiLi.SelectedIndex == -1;
			bool flag2 = flag;
			if (flag2)
			{
				this.CurrentDaiLi = null;
				this.LstDaiLiData.DataSource = null;
			}
			else
			{
				this.CurrentDaiLi = (this.LstDaiLi.SelectedItem as DaiLi);
				this.RefreshDaiLiData();
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00008E74 File Offset: 0x00007074
		private void RefreshDaiLiData()
		{
			this.BtnShuYing.Enabled = (this.BtnClearData.Enabled = (this.CurrentDaiLi.Animals.Count != 0));
			List<string> list = new List<string>();
			foreach (Animal animal in from i in this.CurrentDaiLi.Animals
			orderby i.Count descending
			select i)
			{
				list.Add(string.Format("{0} : {1}", animal.Name, animal.Count));
			}
			this.LstDaiLiData.DataSource = list;
			this.GroDaiLi.Text = string.Format("{0}（{1}）", this.CurrentDaiLi.Name, this.CurrentDaiLi.Animals.Sum((Animal i) => i.Count));
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00008FA0 File Offset: 0x000071A0
		private void BtnCompute_Click(object sender, EventArgs e)
		{
			string text;
			List<Animal> list = this.Manager.ComputeData(this.TxtSourceText.Text, out text);
			using (List<Animal>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Animal item = enumerator.Current;
					Animal animal = (from i in this.CurrentDaiLi.Animals
					where i.ID == item.ID
					select i).FirstOrDefault<Animal>();
					bool flag = animal.IsNull();
					bool flag2 = flag;
					if (flag2)
					{
						animal = this.Manager.CopyAnimal(item);
						this.CurrentDaiLi.Animals.Add(animal);
					}
					else
					{
						animal.Count += item.Count;
					}
				}
			}
			this.CurrentDaiLi.TotalCount = this.CurrentDaiLi.Animals.Sum((Animal i) => i.Count);
			this.CurrentDaiLi.DaiLiFei = (int)Math.Round(this.CurrentDaiLi.TotalCount * this.CurrentDaiLi.DaiLiFeiLv);
			using (List<Animal>.Enumerator enumerator2 = this.CurrentDaiLi.Animals.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Animal item = enumerator2.Current;
					Animal animal2 = (from i in this.Manager.Animals
					where i.ID == item.ID
					select i).FirstOrDefault<Animal>();
					item.ShuYing = this.CurrentDaiLi.TotalCount - this.CurrentDaiLi.DaiLiFei - item.Count * animal2.BeiLv;
				}
			}
			this.Manager.ComputeBili(this.CurrentDaiLi.Animals);
			this.Manager.SaveDaiLis();
			this.RefreshDaiLiData();
			this.TxtSourceText.Text = text;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000091F4 File Offset: 0x000073F4
		private void BtnClearData_Click(object sender, EventArgs e)
		{
			bool flag = !FormHelper.MessageBoxYesNo("确实要清空吗？");
			bool flag2 = !flag;
			if (flag2)
			{
				this.CurrentDaiLi.Animals = new List<Animal>();
				this.Manager.SaveDaiLis();
				this.RefreshDaiLiData();
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00009240 File Offset: 0x00007440
		private void BtnShuYing_Click(object sender, EventArgs e)
		{
			ForShuYing forShuYing = new ForShuYing(this.CurrentDaiLi);
			forShuYing.ShowDialog();
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00009264 File Offset: 0x00007464
		private void BtnTotalShuYing_Click(object sender, EventArgs e)
		{
			DaiLi daiLi = new DaiLi();
			foreach (DaiLi daiLi2 in this.Manager.DaiLis)
			{
				daiLi.TotalCount += daiLi2.TotalCount;
				daiLi.DaiLiFei += daiLi2.DaiLiFei;
				using (List<Animal>.Enumerator enumerator2 = daiLi2.Animals.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Animal item2 = enumerator2.Current;
						Animal animal = (from i in daiLi.Animals
						where i.ID == item2.ID
						select i).FirstOrDefault<Animal>();
						bool flag = animal.IsNull();
						bool flag2 = flag;
						if (flag2)
						{
							animal = this.Manager.CopyAnimal(item2);
							daiLi.Animals.Add(animal);
						}
						else
						{
							animal.Count += item2.Count;
						}
					}
				}
			}
			using (List<Animal>.Enumerator enumerator3 = daiLi.Animals.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					Animal item = enumerator3.Current;
					Animal animal2 = (from i in this.Manager.Animals
					where i.ID == item.ID
					select i).FirstOrDefault<Animal>();
					item.ShuYing = daiLi.TotalCount - daiLi.DaiLiFei - item.Count * animal2.BeiLv;
				}
			}
			this.Manager.ComputeBili(daiLi.Animals);
			ForShuYing forShuYing = new ForShuYing(daiLi);
			forShuYing.ShowDialog();
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000094A4 File Offset: 0x000076A4
		private void BtnChangCi_Click(object sender, EventArgs e)
		{
			ForChangCi forChangCi = new ForChangCi();
			forChangCi.ShowDialog();
			this.RefreshDaiLiData();
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000094C8 File Offset: 0x000076C8
		private void BtnClearAllData_Click(object sender, EventArgs e)
		{
			bool flag = !FormHelper.MessageBoxYesNo("确实要清空吗？");
			bool flag2 = !flag;
			if (flag2)
			{
				foreach (DaiLi daiLi in this.Manager.DaiLis)
				{
					daiLi.Animals = new List<Animal>();
					daiLi.TotalCount = 0;
					daiLi.DaiLiFei = 0;
				}
				this.Manager.SaveDaiLis();
				this.RefreshDaiLiData();
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00009568 File Offset: 0x00007768
		private void BtnClearText_Click(object sender, EventArgs e)
		{
			this.TxtSourceText.Text = null;
		}
	}
}
