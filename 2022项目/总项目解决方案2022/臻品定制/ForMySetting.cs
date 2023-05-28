using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Common;
using MUWEN;

namespace Background
{
	// Token: 0x0200000C RID: 12
	public partial class ForMySetting : ForParent
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600009E RID: 158 RVA: 0x0000B9E9 File Offset: 0x00009BE9
		// (set) Token: 0x0600009F RID: 159 RVA: 0x0000B9F1 File Offset: 0x00009BF1
		public Manager Manager { get; set; }

		// Token: 0x060000A0 RID: 160 RVA: 0x0000B9FA File Offset: 0x00009BFA
		public ForMySetting()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000BA14 File Offset: 0x00009C14
		private void Init()
		{
			this.Manager = new Manager();
			this.TxtJieSuanShiJian.Text = this.Manager.MySetting.JieSuanTime.ToString();
			bool flag = !this.Manager.MySetting.OwnerName.IsNullOrEmpty();
			bool flag2 = flag;
			if (flag2)
			{
				this.TxtOwnerName.Text = this.Manager.MySetting.OwnerName;
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000BA8E File Offset: 0x00009C8E
		private void ForMySetting_Load(object sender, EventArgs e)
		{
			this.Init();
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000BA98 File Offset: 0x00009C98
		private void BtnSave_Click(object sender, EventArgs e)
		{
			bool flag = !FormHelper.MessageBoxYesNo("确实要保存吗？");
			bool flag2 = !flag;
			if (flag2)
			{
				int num = this.TxtJieSuanShiJian.Text.Toint();
				bool flag3 = num < 0 || num > 23;
				bool flag4 = flag3;
				if (flag4)
				{
					this.LblMessage.ShowMessage("时间必须在0到23之间");
				}
				else
				{
					this.Manager.MySetting.JieSuanTime = num;
					this.Manager.MySetting.OwnerName = this.TxtOwnerName.Text;
					this.Manager.SaveMySetting();
					this.LblMessage.ShowMessage("保存成功", Color.Green);
				}
			}
		}
	}
}
