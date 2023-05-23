using System;
using System.Windows.Forms;

namespace stockx网站价格
{
	// Token: 0x02000003 RID: 3
	internal static class Program
	{
		// Token: 0x0600000F RID: 15 RVA: 0x0000343A File Offset: 0x0000163A
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
	}
}
