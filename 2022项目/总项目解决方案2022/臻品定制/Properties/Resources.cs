using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Background.Properties
{
	// Token: 0x02000016 RID: 22
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x060000CD RID: 205 RVA: 0x0000E7B1 File Offset: 0x0000C9B1
		internal Resources()
		{
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000CE RID: 206 RVA: 0x0000E7BC File Offset: 0x0000C9BC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				bool flag = Resources.resourceMan == null;
				bool flag2 = flag;
				if (flag2)
				{
					ResourceManager resourceManager = new ResourceManager("Background.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000CF RID: 207 RVA: 0x0000E804 File Offset: 0x0000CA04
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x0000E81B File Offset: 0x0000CA1B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x040000E1 RID: 225
		private static ResourceManager resourceMan;

		// Token: 0x040000E2 RID: 226
		private static CultureInfo resourceCulture;
	}
}
