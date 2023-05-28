using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Common;
using MUWEN;

namespace Background
{
	// Token: 0x02000002 RID: 2
	public partial class ForActive : ForParent
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public Manager Manager { get; set; }

		// Token: 0x06000003 RID: 3 RVA: 0x00002061 File Offset: 0x00000261
		public ForActive()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002079 File Offset: 0x00000279
		private void ForActive_Load(object sender, EventArgs e)
		{
			this.Init();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002084 File Offset: 0x00000284
		private void Init()
		{
			this.Manager = new Manager();
			this.TxtMachineID.Text = this.Manager.Setting.MachineID;
			this.TxtActivecode.Text = this.Manager.Setting.Activecode;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020D8 File Offset: 0x000002D8
		private void BtnActive_Click(object sender, EventArgs e)
		{
			bool flag = this.TxtActivecode.Text.IsNullOrEmpty();
			bool flag2 = !flag;
			if (flag2)
			{
				bool flag3 = this.TxtActivecode.Text.Length < 15;
				bool flag4 = flag3;
				if (flag4)
				{
					FormHelper.MessageBoxOK("激活码无效");
				}
				else
				{
					Manager manager = new Manager();
					string text = this.TxtActivecode.Text.ToDecode();
					bool flag5 = manager.ComputeActivecode(this.TxtMachineID.Text).ToDecode() != text.Substring(0, 15);
					bool flag6 = flag5;
					if (flag6)
					{
						FormHelper.MessageBoxOK("激活码无效");
					}
					else
					{
						int volidday = text.Substring(15, text.Length - 15).Toint();
						manager.Setting.Volidday = volidday;
						manager.Setting.Activecode = this.TxtActivecode.Text;
						manager.Setting.MachineID = this.TxtMachineID.Text;
						manager.Setting.ActiveDate = DateTime.Now;
						manager.SaveSetting();
						FormHelper.MessageBoxOK("激活成功");
						base.DialogResult = DialogResult.OK;
					}
				}
			}
		}
	}
}
