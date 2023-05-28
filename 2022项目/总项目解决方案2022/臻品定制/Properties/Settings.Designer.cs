using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Background.Properties
{
	// Token: 0x02000017 RID: 23
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.1.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x0000E824 File Offset: 0x0000CA24
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x040000E3 RID: 227
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
