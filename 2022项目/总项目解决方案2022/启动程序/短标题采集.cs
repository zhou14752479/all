using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using helper;

namespace 启动程序
{
	// Token: 0x02000015 RID: 21
	public partial class 短标题采集 : Form
	{
		// Token: 0x060000C2 RID: 194 RVA: 0x00019029 File Offset: 0x00017229
		public 短标题采集()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00019050 File Offset: 0x00017250
		public void run()
		{
			try
			{
				Random random = new Random();
				int num = random.Next(1, 21000);
				string url = "http://yp.jd.com/brand_sitemap_" + num + ".html";
				string url2 = method.GetUrl(url, "utf-8");
				MatchCollection matchCollection = Regex.Matches(url2, "<font size=\"2\">([\\s\\S]*?)</font>");
				foreach (object obj in matchCollection)
				{
					Match match = (Match)obj;
					ListViewItem listViewItem = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
					listViewItem.SubItems.Add(match.Groups[1].Value);
					bool flag = this.listView1.Items.Count > 2;
					if (flag)
					{
						this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
					}
					while (!this.zanting)
					{
						Application.DoEvents();
					}
					bool flag2 = !this.status;
					if (flag2)
					{
						break;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000191DC File Offset: 0x000173DC
		public void run1()
		{
			try
			{
				for (int i = 1; i < 750; i++)
				{
					string url = "https://so.m.jd.com/ware/search.action?keyword=%E6%96%87%E5%85%B7&keywordVal=&page=" + i;
					string url2 = method.GetUrl(url, "utf-8");
					MatchCollection matchCollection = Regex.Matches(url2, "\"warename\": \"([\\s\\S]*?) ");
					foreach (object obj in matchCollection)
					{
						Match match = (Match)obj;
						ListViewItem listViewItem = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
						listViewItem.SubItems.Add(match.Groups[1].Value);
						bool flag = this.listView1.Items.Count > 2;
						if (flag)
						{
							this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
						}
						while (!this.zanting)
						{
							Application.DoEvents();
						}
						bool flag2 = !this.status;
						if (flag2)
						{
							return;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0001936C File Offset: 0x0001756C
		private void 短标题采集_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00019370 File Offset: 0x00017570
		private void button1_Click(object sender, EventArgs e)
		{
			string url = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");
			bool flag = url.Contains("duanbiaoti");
			if (flag)
			{
				this.status = true;
				this.button1.Enabled = false;
				this.zanting = true;
				Thread thread = new Thread(new ThreadStart(this.run));
				thread.Start();
				Control.CheckForIllegalCrossThreadCalls = false;
			}
			else
			{
				MessageBox.Show("验证失败");
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000193EA File Offset: 0x000175EA
		private void button2_Click(object sender, EventArgs e)
		{
			this.zanting = false;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000193F4 File Offset: 0x000175F4
		private void button3_Click(object sender, EventArgs e)
		{
			this.zanting = true;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000193FE File Offset: 0x000175FE
		private void button4_Click(object sender, EventArgs e)
		{
			this.status = false;
			this.button1.Enabled = true;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00019415 File Offset: 0x00017615
		private void button5_Click(object sender, EventArgs e)
		{
			method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0001942F File Offset: 0x0001762F
		private void button6_Click(object sender, EventArgs e)
		{
			this.listView1.Items.Clear();
		}

		// Token: 0x0400020D RID: 525
		private bool zanting = true;

		// Token: 0x0400020E RID: 526
		private bool status = true;
	}
}
