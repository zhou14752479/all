using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace 启动程序.Properties
{
	// Token: 0x0200001C RID: 28
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x06000123 RID: 291 RVA: 0x00022547 File Offset: 0x00020747
		internal Resources()
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00022554 File Offset: 0x00020754
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				bool flag = Resources.resourceMan == null;
				if (flag)
				{
					ResourceManager resourceManager = new ResourceManager("启动程序.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000125 RID: 293 RVA: 0x0002259C File Offset: 0x0002079C
		// (set) Token: 0x06000126 RID: 294 RVA: 0x000225B3 File Offset: 0x000207B3
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

		// Token: 0x040002DB RID: 731
		private static ResourceManager resourceMan;

		// Token: 0x040002DC RID: 732
		private static CultureInfo resourceCulture;
	}
}
