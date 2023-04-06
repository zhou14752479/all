using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace 启动程序
{
	// Token: 0x02000011 RID: 17
	public partial class 添加微博 : Form
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x000155A3 File Offset: 0x000137A3
		public 添加微博()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000155BC File Offset: 0x000137BC
		private void Button1_Click(object sender, EventArgs e)
		{
			添加微博.beizhu = this.textBox1.Text.Trim();
			添加微博.url = this.textBox2.Text.Trim();
			添加微博.jishua = string.Concat(new string[]
			{
				this.textBox3.Text,
				"-",
				this.textBox4.Text,
				" ",
				this.textBox5.Text,
				"-",
				this.textBox6.Text,
				" ",
				this.textBox7.Text,
				"-",
				this.textBox8.Text
			});
			添加微博.rengong = string.Concat(new string[]
			{
				this.textBox9.Text,
				"-",
				this.textBox10.Text,
				" ",
				this.textBox11.Text,
				"-",
				this.textBox12.Text,
				" ",
				this.textBox13.Text,
				"-",
				this.textBox14.Text
			});
			添加微博.cishu = this.textBox15.Text;
			DialogResult dialogResult = MessageBox.Show("是否继续添加？", "退出询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
			bool flag = dialogResult == DialogResult.OK;
			if (!flag)
			{
				base.Hide();
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0001574A File Offset: 0x0001394A
		private void 添加微博_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x040001AE RID: 430
		public static string beizhu = "";

		// Token: 0x040001AF RID: 431
		public static string url;

		// Token: 0x040001B0 RID: 432
		public static string jishua;

		// Token: 0x040001B1 RID: 433
		public static string rengong;

		// Token: 0x040001B2 RID: 434
		public static string cishu;
	}
}
