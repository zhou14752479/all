using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Common;
using MUWEN;

namespace Background
{
	// Token: 0x0200000B RID: 11
	public partial class ForMain : ForParent
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000090 RID: 144 RVA: 0x0000B246 File Offset: 0x00009446
		// (set) Token: 0x06000091 RID: 145 RVA: 0x0000B24E File Offset: 0x0000944E
		public Manager Manager { get; set; }

		// Token: 0x06000092 RID: 146 RVA: 0x0000B257 File Offset: 0x00009457
		public ForMain()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000B26F File Offset: 0x0000946F
		private void MenExit_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000B279 File Offset: 0x00009479
		private void ForMain_Load(object sender, EventArgs e)
		{
			this.Init();
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000B284 File Offset: 0x00009484
		private void Init()
		{
			this.Manager = new Manager();
			string arg = this.Manager.MySetting.OwnerName.IsNullOrEmpty() ? "（未命名）" : ("（" + this.Manager.MySetting.OwnerName + "）");
			this.Text = string.Format("臻品定制 {0} - {1:0}天后到期", arg, this.Manager.ComputeDate());
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000B2FE File Offset: 0x000094FE
		private void MenAnimal_Click(object sender, EventArgs e)
		{
			ForParent.Create(new ForAnimal(), this);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000B30D File Offset: 0x0000950D
		private void MenExtractData_Click(object sender, EventArgs e)
		{
			ForParent.Create(new ForExtractData(), this);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000B31C File Offset: 0x0000951C
		private void MenDaiLi_Click(object sender, EventArgs e)
		{
			ForParent.Create(new ForDaiLi(), this);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000B32C File Offset: 0x0000952C
		private void MenMySetting_Click(object sender, EventArgs e)
		{
			ForMySetting forMySetting = new ForMySetting();
			forMySetting.ShowDialog();
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000B347 File Offset: 0x00009547
		private void MenKaiBao_Click(object sender, EventArgs e)
		{
			ForParent.Create(new ForKaiBao(), this);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0000B356 File Offset: 0x00009556
		private void MenExtractDataJianJie_Click(object sender, EventArgs e)
		{
			ForParent.Create(new ForExtractDataJianJie(), this);
		}
	}
}
