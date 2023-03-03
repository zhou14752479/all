using System;
using System.Windows.Forms;

namespace 校友邦
{
	// Token: 0x02000004 RID: 4
	internal static class Program
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00002889 File Offset: 0x00000A89
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new 校友邦_定时签到());
		}
	}
}
