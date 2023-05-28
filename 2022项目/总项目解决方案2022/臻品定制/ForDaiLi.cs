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
	// Token: 0x02000006 RID: 6
	public partial class ForDaiLi : ForParent
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000041 RID: 65 RVA: 0x000066CB File Offset: 0x000048CB
		// (set) Token: 0x06000042 RID: 66 RVA: 0x000066D3 File Offset: 0x000048D3
		public Manager Manager { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000043 RID: 67 RVA: 0x000066DC File Offset: 0x000048DC
		// (set) Token: 0x06000044 RID: 68 RVA: 0x000066E4 File Offset: 0x000048E4
		public DaiLi CurrentDaiLi { get; set; }

		// Token: 0x06000045 RID: 69 RVA: 0x000066ED File Offset: 0x000048ED
		public ForDaiLi()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00006705 File Offset: 0x00004905
		private void Init()
		{
			this.Manager = new Manager();
			this.RefreshData();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000671B File Offset: 0x0000491B
		private void ForDaiLi_Load(object sender, EventArgs e)
		{
			this.Init();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00006725 File Offset: 0x00004925
		private void RefreshData()
		{
			this.LstData.DataSource = null;
			this.LstData.DisplayMember = "name";
			this.LstData.DataSource = this.Manager.DaiLis;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00006760 File Offset: 0x00004960
		private void BtnSave_Click(object sender, EventArgs e)
		{
			bool flag = this.TxtName.Text.IsNullOrEmpty();
			bool flag2 = flag;
			if (flag2)
			{
				this.LblMessage.ShowMessage("名称不能为空");
			}
			else
			{
				DaiLi daiLi = (from i in this.Manager.DaiLis
				where i.Name == this.TxtName.Text
				select i).FirstOrDefault<DaiLi>();
				bool flag3 = daiLi.IsNull();
				bool flag4 = flag3;
				if (flag4)
				{
					daiLi = new DaiLi
					{
						Name = this.TxtName.Text,
						Animals = new List<Animal>()
					};
					this.Manager.DaiLis.Add(daiLi);
				}
				daiLi.DaiLiFeiLv = this.TxtDaiLiFei.Text.Todecimal();
				this.Manager.SaveDaiLis();
				this.RefreshData();
				this.LblMessage.ShowMessage("保存成功", Color.Green);
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00006848 File Offset: 0x00004A48
		private void BtnDelete_Click(object sender, EventArgs e)
		{
			bool flag = !FormHelper.MessageBoxYesNo("确实要删除吗？");
			bool flag2 = !flag;
			if (flag2)
			{
				this.Manager.DaiLis.Remove(this.CurrentDaiLi);
				this.CurrentDaiLi = null;
				this.BtnDelete.Enabled = false;
				this.Manager.SaveDaiLis();
				this.Manager.LoadDaiLis();
				this.RefreshData();
				this.LblMessage.ShowMessage("删除成功", Color.Green);
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000068D0 File Offset: 0x00004AD0
		private void LstData_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool flag = this.LstData.SelectedIndex == -1;
			bool flag2 = flag;
			if (flag2)
			{
				this.BtnDelete.Enabled = false;
				this.CurrentDaiLi = null;
			}
			else
			{
				this.BtnDelete.Enabled = true;
				this.CurrentDaiLi = (this.LstData.SelectedItem as DaiLi);
				this.TxtName.Text = this.CurrentDaiLi.Name;
				this.TxtDaiLiFei.Text = this.CurrentDaiLi.DaiLiFeiLv.ToString();
			}
		}
	}
}
