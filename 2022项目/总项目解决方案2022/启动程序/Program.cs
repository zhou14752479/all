using System;
using System.Windows.Forms;

namespace 启动程序
{
	// Token: 0x02000003 RID: 3
	internal static class Program
	{
		// Token: 0x0600000B RID: 11 RVA: 0x0000306B File Offset: 0x0000126B
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new 子窗体());
		}
	}
}
