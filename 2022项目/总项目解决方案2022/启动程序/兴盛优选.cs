using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using helper;

namespace 启动程序
{
	// Token: 0x0200000A RID: 10
	public partial class 兴盛优选 : Form
	{
		// Token: 0x06000046 RID: 70 RVA: 0x0000C1F5 File Offset: 0x0000A3F5
		public 兴盛优选()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000C214 File Offset: 0x0000A414
		public void run()
		{
			try
			{
				string text = HttpUtility.UrlEncode(this.textBox1.Text);
				for (int i = 1; i < 9999; i++)
				{
					string url = string.Concat(new object[]
					{
						"https://mall-store.xsyxsc.com/mall-store/store/queryStoreList?page=",
						i,
						"&rows=100&storeName=",
						HttpUtility.UrlEncode(this.textBox1.Text),
						"&userKey=a2422d77-69a1-4f7b-aa1a-39bec2b2db44"
					});
					string url2 = method.GetUrl(url, "utf-8");
					MatchCollection matchCollection = Regex.Matches(url2, "\"storeName\":\"([\\s\\S]*?)\"");
					MatchCollection matchCollection2 = Regex.Matches(url2, "\"contacts\":\"([\\s\\S]*?)\"");
					MatchCollection matchCollection3 = Regex.Matches(url2, "\"contactsTel\":\"([\\s\\S]*?)\"");
					MatchCollection matchCollection4 = Regex.Matches(url2, "\"detailAddress\":\"([\\s\\S]*?)\"");
					bool flag = matchCollection.Count == 0;
					if (flag)
					{
						break;
					}
					int j = 0;
					while (j < matchCollection.Count)
					{
						try
						{
							ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
							listViewItem.SubItems.Add(matchCollection[j].Groups[1].Value);
							listViewItem.SubItems.Add(matchCollection2[j].Groups[1].Value);
							listViewItem.SubItems.Add(matchCollection3[j].Groups[1].Value);
							listViewItem.SubItems.Add(matchCollection4[j].Groups[1].Value);
							bool flag2 = !this.status;
							if (flag2)
							{
								return;
							}
						}
						catch
						{
						}
						IL_198:
						j++;
						continue;
						goto IL_198;
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000C424 File Offset: 0x0000A624
		private void 兴盛优选_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000C428 File Offset: 0x0000A628
		private void button1_Click(object sender, EventArgs e)
		{
			this.button1.Enabled = false;
			string url = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");
			bool flag = url.Contains("xsyx");
			if (flag)
			{
				this.button1.Enabled = false;
				this.status = true;
				Thread thread = new Thread(new ThreadStart(this.run));
				thread.Start();
				Control.CheckForIllegalCrossThreadCalls = false;
			}
			else
			{
				MessageBox.Show("验证失败");
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000C4A8 File Offset: 0x0000A6A8
		private void button2_Click(object sender, EventArgs e)
		{
			this.button1.Enabled = true;
			this.status = false;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000C4BF File Offset: 0x0000A6BF
		private void button3_Click(object sender, EventArgs e)
		{
			method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000C4D9 File Offset: 0x0000A6D9
		private void button4_Click(object sender, EventArgs e)
		{
			this.listView1.Items.Clear();
		}

		// Token: 0x04000101 RID: 257
		private bool status = true;
	}
}
