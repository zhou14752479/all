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
	// Token: 0x02000007 RID: 7
	public partial class ForExtractData : ForParent
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00006F50 File Offset: 0x00005150
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00006F58 File Offset: 0x00005158
		public Manager Manager { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00006F61 File Offset: 0x00005161
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00006F69 File Offset: 0x00005169
		public DaiLi CurrentDaiLi { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00006F72 File Offset: 0x00005172
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00006F7A File Offset: 0x0000517A
		public List<Animal> CurrentAnimals { get; set; } = new List<Animal>();

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00006F83 File Offset: 0x00005183
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00006F8B File Offset: 0x0000518B
		private int Sourcetextindex { get; set; }

		// Token: 0x06000057 RID: 87 RVA: 0x00006F94 File Offset: 0x00005194
		public ForExtractData()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00006FB7 File Offset: 0x000051B7
		private void ForExtractData_Load(object sender, EventArgs e)
		{
			this.Init();
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00006FC4 File Offset: 0x000051C4
		private void Init()
		{
			this.Manager = new Manager();
			this.LstDaiLi.DisplayMember = "name";
			this.LstDaiLi.DataSource = this.Manager.DaiLis;
			this.BtnChangCi.Enabled = (this.BtnTotalShuYing.Enabled = (this.Manager.DaiLis.Count != 0));
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00007034 File Offset: 0x00005234
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

		// Token: 0x0600005B RID: 91 RVA: 0x00007090 File Offset: 0x00005290
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

		// Token: 0x0600005C RID: 92 RVA: 0x000071BC File Offset: 0x000053BC
		private void RefreshTongJi()
		{
			this.BtnClearCount.Enabled = (this.BtnUpdateCount.Enabled = (this.CurrentAnimals.Count != 0));
			this.BtnAddData.Enabled = (this.CurrentAnimals.Count != 0 && this.Manager.DaiLis.Count != 0);
			this.BtnSelect.Enabled = (this.BtnMoveToYuanShi.Enabled = !this.TxtProcessed.Text.IsNullOrEmpty());
			int num = 0;
			this.PanResults.Controls.Clear();
			foreach (Animal animal in from i in this.CurrentAnimals
			orderby i.Count descending
			select i)
			{
				UseAnimal value = new UseAnimal(animal.Name, animal.Count)
				{
					Width = 120,
					Height = 15
				};
				this.PanResults.Controls.Add(value);
				num += animal.Count;
			}
			this.GroResult.Text = string.Format("统计结果（{0}）", num);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00007324 File Offset: 0x00005524
		private void BtnCompute_Click(object sender, EventArgs e)
		{
			bool flag = this.TxtSourceText.Text.IsNullOrEmpty();
			bool flag2 = flag;
			if (flag2)
			{
				this.BtnUpdateCount.Enabled = false;
			}
			else
			{
				string text;
				List<Animal> list = this.Manager.ComputeData(this.TxtSourceText.Text, out text);
				using (List<Animal>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Animal item = enumerator.Current;
						Animal animal = (from i in this.CurrentAnimals
						where i.ID == item.ID
						select i).FirstOrDefault<Animal>();
						bool flag3 = animal.IsNull();
						bool flag4 = flag3;
						if (flag4)
						{
							animal = this.Manager.CopyAnimal(item);
							this.CurrentAnimals.Add(animal);
						}
						else
						{
							animal.Count += item.Count;
						}
					}
				}
				this.TxtProcessed.Text = text;
				this.RefreshTongJi();
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00007458 File Offset: 0x00005658
		private void BtnAddData_Click(object sender, EventArgs e)
		{
			bool flag = this.CurrentAnimals.IsNull();
			bool flag2 = !flag;
			if (flag2)
			{
				bool flag3 = !FormHelper.MessageBoxYesNo("确实要追加吗？");
				bool flag4 = !flag3;
				if (flag4)
				{
					using (List<Animal>.Enumerator enumerator = this.CurrentAnimals.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Animal item = enumerator.Current;
							Animal animal = (from i in this.CurrentDaiLi.Animals
							where i.ID == item.ID
							select i).FirstOrDefault<Animal>();
							bool flag5 = animal.IsNull();
							bool flag6 = flag5;
							if (flag6)
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
					this.CurrentDaiLi.DaiLiFei = (int)(this.CurrentDaiLi.TotalCount * this.CurrentDaiLi.DaiLiFeiLv);
					foreach (Animal animal2 in this.CurrentDaiLi.Animals)
					{
						animal2.ShuYing = this.CurrentDaiLi.TotalCount - this.CurrentDaiLi.DaiLiFei - animal2.Count * animal2.BeiLv;
					}
					this.Manager.ComputeBili(this.CurrentDaiLi.Animals);
					this.Manager.SaveDaiLis();
					this.RefreshDaiLiData();
					FormHelper.MessageBoxOK("追加成功");
				}
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000768C File Offset: 0x0000588C
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

		// Token: 0x06000060 RID: 96 RVA: 0x000076D8 File Offset: 0x000058D8
		private void BtnSelect_Click(object sender, EventArgs e)
		{
			string selectedText = this.TxtProcessed.SelectedText;
			bool flag = selectedText.IsNullOrEmpty();
			bool flag2 = flag;
			if (flag2)
			{
				FormHelper.MessageBoxOK("请选择要查询的文本段");
			}
			else
			{
				int num = this.TxtSourceText.Text.IndexOf(selectedText, this.Sourcetextindex);
				bool flag3 = num > -1;
				bool flag4 = flag3;
				if (flag4)
				{
					this.Sourcetextindex = num + selectedText.Length;
				}
				else
				{
					this.Sourcetextindex = 0;
					num = this.TxtSourceText.Text.IndexOf(selectedText, this.Sourcetextindex);
					FormHelper.MessageBoxOK("查询完成");
				}
				bool flag5 = num == -1;
				bool flag6 = flag5;
				if (flag6)
				{
					FormHelper.MessageBoxOK("源文本中未找到");
				}
				else
				{
					this.TxtSourceText.Select(num, selectedText.Length);
					this.TxtSourceText.ScrollToCaret();
				}
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000077B8 File Offset: 0x000059B8
		private void BtnUpdateCount_Click(object sender, EventArgs e)
		{
			foreach (object obj in this.PanResults.Controls)
			{
				UseAnimal ua = obj as UseAnimal;
				Animal animal = (from i in this.CurrentAnimals
				where i.Name == ua.LblName.Text
				select i).FirstOrDefault<Animal>();
				animal.Count = ua.TxtValue.Text.Toint();
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00007860 File Offset: 0x00005A60
		private void BtnMoveToYuanShi_Click(object sender, EventArgs e)
		{
			this.TxtSourceText.Text = this.TxtProcessed.Text;
			this.TxtProcessed.Text = null;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00007888 File Offset: 0x00005A88
		private void BtnClearCount_Click(object sender, EventArgs e)
		{
			bool flag = !FormHelper.MessageBoxYesNo("确实要清空吗？");
			bool flag2 = !flag;
			if (flag2)
			{
				this.CurrentAnimals = new List<Animal>();
				this.RefreshTongJi();
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000078C4 File Offset: 0x00005AC4
		private void BtnShuYing_Click(object sender, EventArgs e)
		{
			ForShuYing forShuYing = new ForShuYing(this.CurrentDaiLi);
			forShuYing.ShowDialog();
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000078E8 File Offset: 0x00005AE8
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
							animal.ShuYing += item2.ShuYing;
						}
					}
				}
			}
			this.Manager.ComputeBili(daiLi.Animals);
			ForShuYing forShuYing = new ForShuYing(daiLi);
			forShuYing.ShowDialog();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00007A90 File Offset: 0x00005C90
		private void BtnChangCi_Click(object sender, EventArgs e)
		{
			ForChangCi forChangCi = new ForChangCi();
			forChangCi.ShowDialog();
			this.RefreshDaiLiData();
		}
	}
}
