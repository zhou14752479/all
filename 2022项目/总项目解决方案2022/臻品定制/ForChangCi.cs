using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using MUWEN;

namespace Background
{
	// Token: 0x02000004 RID: 4
	public partial class ForChangCi : ForParent
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000037D0 File Offset: 0x000019D0
		// (set) Token: 0x0600001D RID: 29 RVA: 0x000037D8 File Offset: 0x000019D8
		public Manager Manager { get; set; } = new Manager();

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000037E1 File Offset: 0x000019E1
		// (set) Token: 0x0600001F RID: 31 RVA: 0x000037E9 File Offset: 0x000019E9
		public DaiLi CurrentDaiLi { get; set; }

		// Token: 0x06000020 RID: 32 RVA: 0x000037F2 File Offset: 0x000019F2
		public ForChangCi()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003818 File Offset: 0x00001A18
		private void Init()
		{
			List<DaiLi> daiLis = this.Manager.DaiLis;
			DaiLi daiLi = new DaiLi();
			daiLi.Name = "全部";
			daiLi.ID = "all";
			daiLi.DaiLiFeiLv = this.Manager.DaiLis.Sum((DaiLi i) => i.DaiLiFeiLv) / this.Manager.DaiLis.Count;
			daiLis.Add(daiLi);
			this.CmbDaiLi.DisplayMember = "name";
			this.CmbDaiLi.DataSource = this.Manager.DaiLis;
			this.LblDate.Text = string.Format("{0:MM月dd日}账目报告", DateTime.Now);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000038F0 File Offset: 0x00001AF0
		private void ForChangCi_Load(object sender, EventArgs e)
		{
			this.Init();
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000038FA File Offset: 0x00001AFA
		private void BtnJieTu_Click(object sender, EventArgs e)
		{
			this.Manager.JieTu(this, new Point(9, 90), new Size(-10, -28));
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000391C File Offset: 0x00001B1C
		private void BtnChangCiManager_Click(object sender, EventArgs e)
		{
			ForChangCiManager forChangCiManager = new ForChangCiManager();
			forChangCiManager.ShowDialog();
			this.Manager.LoadChangCis();
			this.RefreshData();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000394C File Offset: 0x00001B4C
		private void RefreshData()
		{
			bool flag = this.CmbDaiLi.SelectedIndex == -1;
			if (flag)
			{
				this.CurrentDaiLi = null;
			}
			else
			{
				this.PanContent.Controls.Clear();
				this.CurrentDaiLi = (this.CmbDaiLi.SelectedItem as DaiLi);
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				this.LblDaiLiFeiLv.Text = (this.CurrentDaiLi.DaiLiFeiLv * 100m).ToString("0") + "%";
				this.LblName.Text = this.CurrentDaiLi.Name;
				bool flag2 = this.CurrentDaiLi.Name == "全部";
				if (flag2)
				{
					bool flag3 = this.Manager.MySetting.OwnerName.IsNullOrEmpty();
					if (flag3)
					{
						this.LblName.Text = "未命名";
					}
					else
					{
						this.LblName.Text = this.Manager.MySetting.OwnerName;
					}
				}
				int i = 0;
				bool flag4 = DateTime.Now.Hour >= this.Manager.MySetting.JieSuanTime;
				DateTime dtStart;
				DateTime dtEnd;
				if (flag4)
				{
					dtStart = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd") + string.Format(" {0}:00", this.Manager.MySetting.JieSuanTime));
					dtEnd = Convert.ToDateTime(DateTime.Now.AddDays(1.0).ToString("yyyy/MM/dd") + string.Format(" {0}:00", this.Manager.MySetting.JieSuanTime));
				}
				else
				{
					dtStart = Convert.ToDateTime(DateTime.Now.AddDays(-1.0).ToString("yyyy/MM/dd") + string.Format(" {0}:00", this.Manager.MySetting.JieSuanTime));
					dtEnd = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd") + string.Format(" {0}:00", this.Manager.MySetting.JieSuanTime));
				}
				List<ChangCi> list = (from j in this.Manager.ChangCis
				where j.Createtime > dtStart && j.Createtime <= dtEnd
				select j).ToList<ChangCi>();
				bool @checked = this.CheQuanBuChangCi.Checked;
				if (@checked)
				{
					list = this.Manager.ChangCis;
				}
				
				while (i < list.Count)
				{
					ChangCi changCi = list[i];
					IEnumerable<ChangCi_DaiLi> daiLis = changCi.DaiLis;
					Func<ChangCi_DaiLi, bool> predicate=null;
					//if ((predicate = <>9__1) == null)
					//{
					//	predicate = (<>9__1 = ((ChangCi_DaiLi j) => j.DaiLiID == this.CurrentDaiLi.ID));
					//}
					ChangCi_DaiLi changCi_DaiLi = daiLis.Where(predicate).FirstOrDefault<ChangCi_DaiLi>();
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(string.Format("第{0}场 ", i + 1));
					bool flag5 = changCi.Animals.Count == 0;
					if (flag5)
					{
						stringBuilder.Append("未开 ");
					}
					foreach (Animal animal in changCi.Animals)
					{
						stringBuilder.Append(animal.Name + " ");
					}
					bool flag6 = changCi_DaiLi.IsNull();
					if (flag6)
					{
						changCi_DaiLi = new ChangCi_DaiLi();
					}
					num += changCi_DaiLi.ZongYaZhuE;
					num2 += changCi_DaiLi.DaiLiFei;
					num3 += changCi_DaiLi.ZhongBaoYaZhu;
					num4 += changCi_DaiLi.ZhongBaoXuYaoPeiDeQian;
					num5 += changCi_DaiLi.ShuYing;
					UseChangCi useChangCi = new UseChangCi(stringBuilder.ToString(), changCi_DaiLi.ZongYaZhuE.ToString(), changCi_DaiLi.DaiLiFei.ToString(), changCi_DaiLi.ZhongBaoYaZhu.ToString(), changCi_DaiLi.ZhongBaoXuYaoPeiDeQian.ToString(), changCi_DaiLi.ShuYing.ToString())
					{
						Parent = this.PanContent
					};
					useChangCi.Location = new Point(0, (useChangCi.Height - 2) * i);
					useChangCi.Width = 945;
					i++;
				}
				string str = (num5 > 0) ? "下家需要付上家" : "上家需要付下家";
				UseChangCi useChangCi2 = new UseChangCi("合计", num.ToString(), num2.ToString(), num3.ToString(), num4.ToString(), str + num5.ToString())
				{
					Parent = this.PanContent
				};
				useChangCi2.Width = 945;
				useChangCi2.Location = new Point(0, (useChangCi2.Height - 2) * i);
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00003E84 File Offset: 0x00002084
		private void CmbDaiLi_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.RefreshData();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003E84 File Offset: 0x00002084
		private void CheQuanBuChangCi_CheckedChanged(object sender, EventArgs e)
		{
			this.RefreshData();
		}
	}
}
