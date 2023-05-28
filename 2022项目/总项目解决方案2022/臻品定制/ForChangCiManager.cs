using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Common;
using MUWEN;

namespace Background
{
	// Token: 0x02000005 RID: 5
	public partial class ForChangCiManager : ForParent
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00004D34 File Offset: 0x00002F34
		// (set) Token: 0x0600002B RID: 43 RVA: 0x00004D3C File Offset: 0x00002F3C
		public Manager Manager { get; set; } = new Manager();

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00004D45 File Offset: 0x00002F45
		// (set) Token: 0x0600002D RID: 45 RVA: 0x00004D4D File Offset: 0x00002F4D
		public List<Animal> SelectedAnimals { get; set; } = new List<Animal>();

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00004D56 File Offset: 0x00002F56
		// (set) Token: 0x0600002F RID: 47 RVA: 0x00004D5E File Offset: 0x00002F5E
		public ChangCi CurrentChangCi { get; set; }

		// Token: 0x06000030 RID: 48 RVA: 0x00004D67 File Offset: 0x00002F67
		public ForChangCiManager()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00004D98 File Offset: 0x00002F98
		private void Init()
		{
			this.CmbAnimals.DisplayMember = "name";
			this.CmbAnimals.DataSource = this.Manager.Animals;
			this.BtnAddAnimal.Enabled = (this.Manager.Animals.Count > 0);
			bool flag = !this.Manager.ChangCis.Any((ChangCi i) => i.Createtime.ToDate("-", "-", "") == DateTime.Now.ToDate("-", "-", ""));
			bool flag2 = flag;
			if (flag2)
			{
				for (int j = 0; j < 3; j++)
				{
					Thread.Sleep(100);
					ChangCi item = new ChangCi
					{
						Name = DateTime.Now.ToString("yyyy-MM-dd")
					};
					this.Manager.ChangCis.Add(item);
				}
				this.Manager.SaveChangCis();
			}
			this.RefreshData();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00004E88 File Offset: 0x00003088
		private void ForChangCiManager_Load(object sender, EventArgs e)
		{
			this.Init();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00004E94 File Offset: 0x00003094
		private void ResetChangCi()
		{
			List<ChangCi> list = (from i in this.Manager.ChangCis
			where i.Createtime.ToDate("-", "-", "") == DateTime.Now.ToDate("-", "-", "")
			orderby i.Createtime descending
			select i).ToList<ChangCi>();
			bool flag = !list.Any((ChangCi i) => i.Animals.Count > 0);
			bool flag2 = flag;
			if (flag2)
			{
				this.LblMessage.ShowMessage("场次已全部重置");
			}
			else
			{
				foreach (ChangCi changCi in list)
				{
					bool flag3 = changCi.Animals.Count > 0;
					bool flag4 = flag3;
					if (flag4)
					{
						changCi.Animals = new List<Animal>();
						changCi.DaiLiFeiLv = 0;
						changCi.DaiLis = new List<ChangCi_DaiLi>();
						break;
					}
				}
				this.Manager.SaveChangCis();
				this.RefreshData();
				this.LblMessage.ShowMessage("重置成功", Color.Green);
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00004FEC File Offset: 0x000031EC
		private void BtnAddAnimal_Click(object sender, EventArgs e)
		{
			this.SelectedAnimals.Add(this.Manager.CopyAnimal(this.CmbAnimals.SelectedItem as Animal));
			this.ShowAnimalName(this.SelectedAnimals);
			this.BtnAddChangCi.Enabled = true;
			this.BtnClear.Enabled = true;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00005048 File Offset: 0x00003248
		private void ShowAnimalName(List<Animal> animals)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Animal animal in animals)
			{
				stringBuilder.Append(animal.Name + " ");
			}
			this.TxtNames.Text = stringBuilder.ToString();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000050C4 File Offset: 0x000032C4
		private void BtnClear_Click(object sender, EventArgs e)
		{
			this.SelectedAnimals = new List<Animal>();
			this.TxtNames.Text = null;
			this.LblMessage.ShowMessage("清空成功", Color.Green);
			this.BtnAddChangCi.Enabled = false;
			this.BtnClear.Enabled = false;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000511C File Offset: 0x0000331C
		private void BtnAddChangCi_Click(object sender, EventArgs e)
		{
			//int i;
			//List<ChangCi> list = (from i in this.Manager.ChangCis
			//where i.Createtime.ToDate("-", "-", "") == DateTime.Now.ToDate("-", "-", "")
			//select i).ToList<ChangCi>();
			//bool flag = list.Count == 3 && list[2].Animals.Count > 0;
			//bool flag2 = flag;
			//if (flag2)
			//{
			//	this.LblMessage.ShowMessage("场次已满");
			//}
			//else
			//{
			//	for (i = 0; i < list.Count; i++)
			//	{
			//		ChangCi changCi = list[i];
			//		bool flag3 = changCi.Animals.Count == 0;
			//		bool flag4 = flag3;
			//		if (flag4)
			//		{
			//			foreach (Animal animal in this.SelectedAnimals)
			//			{
			//				changCi.Animals.Add(this.Manager.CopyAnimal(animal));
			//			}
			//			ChangCi_DaiLi changCi_DaiLi = new ChangCi_DaiLi();
			//			foreach (DaiLi daiLi in this.Manager.DaiLis)
			//			{
			//				int num = 0;
			//				foreach (Animal animal2 in daiLi.Animals)
			//				{
			//					num += animal2.Count;
			//				}
			//				int num2 = 0;
			//				int num3 = 0;
			//				using (List<Animal>.Enumerator enumerator4 = this.SelectedAnimals.GetEnumerator())
			//				{
			//					while (enumerator4.MoveNext())
			//					{
			//						Animal item2 = enumerator4.Current;
			//						Animal animal3 = (from j in daiLi.Animals
			//						where j.ID == item2.ID
			//						select j).FirstOrDefault<Animal>();
			//						bool flag5 = animal3.IsNull();
			//						bool flag6 = !flag5;
			//						if (flag6)
			//						{
			//							num2 += animal3.Count;
			//							num3 += animal3.Count * animal3.BeiLv;
			//						}
			//					}
			//				}
			//				ChangCi_DaiLi changCi_DaiLi2 = new ChangCi_DaiLi
			//				{
			//					DaiLiFei = daiLi.DaiLiFei,
			//					DaiLiID = daiLi.ID,
			//					ZongYaZhuE = num,
			//					ZhongBaoYaZhu = num2,
			//					ZhongBaoXuYaoPeiDeQian = num3
			//				};
			//				changCi_DaiLi2.ShuYing = changCi_DaiLi2.ZongYaZhuE - changCi_DaiLi2.DaiLiFei - changCi_DaiLi2.ZhongBaoXuYaoPeiDeQian;
			//				changCi.DaiLis.Add(changCi_DaiLi2);
			//				changCi_DaiLi.DaiLiID = "all";
			//				changCi_DaiLi.DaiLiFei += changCi_DaiLi2.DaiLiFei;
			//				changCi_DaiLi.ZongYaZhuE += changCi_DaiLi2.ZongYaZhuE;
			//				changCi_DaiLi.ZhongBaoYaZhu += changCi_DaiLi2.ZhongBaoYaZhu;
			//				changCi_DaiLi.ZhongBaoXuYaoPeiDeQian += changCi_DaiLi2.ZhongBaoXuYaoPeiDeQian;
			//				changCi_DaiLi.ShuYing += changCi_DaiLi2.ShuYing;
			//			}
			//			changCi.DaiLis.Add(changCi_DaiLi);
			//			this.Manager.SaveChangCis();
			//			this.RefreshData();
			//			break;
			//		}
			//	}
			//	this.LblMessage.ShowMessage("添加成功", Color.Green);
			//	this.TxtNames.Text = null;
			//	this.SelectedAnimals = new List<Animal>();
			//	this.BtnAddChangCi.Enabled = false;
			//}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000550C File Offset: 0x0000370C
		private void LstData_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool flag = this.LstData.SelectedIndex == -1;
			bool flag2 = flag;
			if (flag2)
			{
				this.CurrentChangCi = null;
				this.BtnDeleteChangCi.Enabled = false;
			}
			else
			{
				this.CurrentChangCi = (this.LstData.SelectedItem as ChangCi);
				this.BtnDeleteChangCi.Enabled = true;
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00005570 File Offset: 0x00003770
		private void RefreshData()
		{
			this.LstData.DataSource = null;
			this.LstData.DisplayMember = "name";
			this.LstData.DataSource = (from i in this.Manager.ChangCis
			orderby i.Createtime descending
			select i).ToList<ChangCi>();
			this.BtnDeleteAll.Enabled = (this.Manager.ChangCis.Count > 0);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000055FB File Offset: 0x000037FB
		private void BtnDeleteChangCi_Click(object sender, EventArgs e)
		{
			this.ResetChangCi();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00005608 File Offset: 0x00003808
		private void BtnDeleteAll_Click(object sender, EventArgs e)
		{
			bool flag = !FormHelper.MessageBoxYesNo("确实要清空吗？");
			bool flag2 = !flag;
			if (flag2)
			{
				this.Manager.ChangCis = new List<ChangCi>();
				this.CurrentChangCi = null;
				this.Manager.SaveChangCis();
				this.Manager.LoadChangCis();
				this.RefreshData();
				this.LblMessage.ShowMessage("清空成功", Color.Green);
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000567C File Offset: 0x0000387C
		private void BtnDeleteChangCi_Click_1(object sender, EventArgs e)
		{
			bool flag = !FormHelper.MessageBoxYesNo("确实要删除吗？");
			bool flag2 = !flag;
			if (flag2)
			{
				this.Manager.ChangCis.Remove(this.CurrentChangCi);
				this.Manager.SaveChangCis();
				this.Manager.LoadChangCis();
				this.RefreshData();
				this.LblMessage.ShowMessage("删除成功", Color.Green);
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000056F0 File Offset: 0x000038F0
		private void BtnAddZiDingYiChangCi_Click(object sender, EventArgs e)
		{
			bool flag = !FormHelper.MessageBoxYesNo("确实要添加吗？");
			bool flag2 = !flag;
			if (flag2)
			{
				List<ChangCi> list = (from i in this.Manager.ChangCis
				where i.Createtime.ToDate("-", "-", "") == this.DatChangCiTime.Value.ToDate("-", "-", "")
				select i).ToList<ChangCi>();
				bool flag3 = list.Count == 4;
				bool flag4 = flag3;
				if (flag4)
				{
					this.LblMessage.ShowMessage("该日场次已满");
				}
				else
				{
					List<Animal> list2 = new List<Animal>();
					bool flag5 = !this.TxtChangCiAnimals.Text.IsNullOrEmpty();
					bool flag6 = flag5;
					if (flag6)
					{
						string[] array = this.TxtChangCiAnimals.Text.Split(new char[]
						{
							' '
						}, StringSplitOptions.RemoveEmptyEntries);
						string[] array2 = array;
						for (int j = 0; j < array2.Length; j++)
						{
							string item = array2[j];
							Animal animal = (from i in this.Manager.Animals
							where i.Name == item || i.Names.Contains(item)
							select i).FirstOrDefault<Animal>();
							bool flag7 = !animal.IsNull();
							bool flag8 = !flag7;
							if (flag8)
							{
								this.LblMessage.ShowMessage("动物名不存在");
								return;
							}
							list2.Add(this.Manager.CopyAnimal(animal));
						}
					}
					ChangCi item2 = new ChangCi
					{
						Animals = list2,
						Name = (base.Name = this.DatChangCiTime.Value.ToString("yyyy-MM-dd")),
						Createtime = this.DatChangCiTime.Value
					};
					this.Manager.ChangCis.Add(item2);
					this.Manager.SaveChangCis();
					this.RefreshData();
					this.LblMessage.ShowMessage("增加成功", Color.Green);
				}
			}
		}
	}
}
