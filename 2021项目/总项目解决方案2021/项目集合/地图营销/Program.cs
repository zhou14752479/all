using System;
using System.Windows.Forms;

namespace 地图营销
{
	// Token: 0x02000004 RID: 4
	internal static class Program
	{
		// Token: 0x0600002D RID: 45 RVA: 0x000070A8 File Offset: 0x000052A8
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new 地图采集());
		}
	}
}
