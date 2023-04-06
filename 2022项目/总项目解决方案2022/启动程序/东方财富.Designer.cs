namespace 启动程序
{
	// Token: 0x02000007 RID: 7
	public partial class 东方财富 : global::System.Windows.Forms.Form
	{
		// Token: 0x06000029 RID: 41 RVA: 0x000051AC File Offset: 0x000033AC
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000051E3 File Offset: 0x000033E3
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(800, 450);
			this.Text = "东方财富";
		}

		// Token: 0x04000052 RID: 82
		private global::System.ComponentModel.IContainer components = null;
	}
}
