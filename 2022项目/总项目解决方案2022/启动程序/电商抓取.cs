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
	// Token: 0x02000013 RID: 19
	public partial class 电商抓取 : Form
	{
		// Token: 0x060000AF RID: 175 RVA: 0x00017741 File Offset: 0x00015941
		public 电商抓取()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00017772 File Offset: 0x00015972
		private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
		{
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00017778 File Offset: 0x00015978
		public void aliexpress()
		{
			try
			{
				string url = "https://www.aliexpress.com/wholesale?catId=0&initiative_id=SB_20200328235416&SearchText=" + HttpUtility.UrlEncode(this.keyword);
				string url2 = method.GetUrl(url, "utf-8");
				MatchCollection matchCollection = Regex.Matches(url2, "\"productId\":([\\s\\S]*?),");
				for (int i = 0; i < matchCollection.Count; i++)
				{
					string url3 = "https://www.aliexpress.com/item/" + matchCollection[i].Groups[1].Value + ".html";
					string url4 = method.GetUrl(url3, "utf-8");
					Match match = Regex.Match(url4, "<title>([\\s\\S]*?)</title>");
					Match match2 = Regex.Match(url4, "Item Weight([\\s\\S]*?)attrValue\":\"([\\s\\S]*?)\"");
					Match match3 = Regex.Match(url4, "subject\":\"([\\s\\S]*?)\"");
					Match match4 = Regex.Match(url4, "formatedActivityPrice\":\"([\\s\\S]*?)\"");
					Match match5 = Regex.Match(url4, "formatedActivityPrice\":\"([\\s\\S]*?)\"");
					Match match6 = Regex.Match(url4, "Part No\\.([\\s\\S]*?)attrValue\":\"([\\s\\S]*?)\"");
					ListViewItem listViewItem = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
					listViewItem.SubItems.Add(match.Groups[1].Value);
					listViewItem.SubItems.Add(match2.Groups[2].Value);
					listViewItem.SubItems.Add(match3.Groups[1].Value);
					listViewItem.SubItems.Add(match4.Groups[1].Value);
					listViewItem.SubItems.Add(match6.Groups[2].Value);
					listViewItem.SubItems.Add(match5.Groups[1].Value);
					while (!this.zanting)
					{
						Application.DoEvents();
					}
					bool flag = !this.status;
					if (flag)
					{
						break;
					}
					Thread.Sleep(1000);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000179AC File Offset: 0x00015BAC
		public void amazon()
		{
			try
			{
				string url = "https://www.amazon.com/s?k=" + HttpUtility.UrlEncode(this.keyword) + "&__mk_zh_CN=%E4%BA%9A%E9%A9%AC%E9%80%8A%E7%BD%91%E7%AB%99&ref=nb_sb_noss";
				string url2 = method.GetUrl(url, "utf-8");
				MatchCollection matchCollection = Regex.Matches(url2, "<div data-asin=\"([\\s\\S]*?)\"");
				for (int i = 0; i < matchCollection.Count; i++)
				{
					string url3 = "https://www.amazon.com/-/zh/dp/" + matchCollection[i].Groups[1].Value + "/ref=sr_1_1";
					string url4 = method.GetUrl(url3, "utf-8");
					Match match = Regex.Match(url4, "<title>([\\s\\S]*?)</title>");
					Match match2 = Regex.Match(url4, "Item Weight([\\s\\S]*?)attrValue\":\"([\\s\\S]*?)\"");
					Match match3 = Regex.Match(url4, "subject\":\"([\\s\\S]*?)\"");
					Match match4 = Regex.Match(url4, "Item Package Quantity([\\s\\S]*?)</td>");
					Match match5 = Regex.Match(url4, "data-asin-price=\"([\\s\\S]*?)\"");
					Match match6 = Regex.Match(url4, "零件编号([\\s\\S]*?)</td>");
					ListViewItem listViewItem = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
					listViewItem.SubItems.Add(match.Groups[1].Value);
					listViewItem.SubItems.Add(match2.Groups[2].Value);
					listViewItem.SubItems.Add(match3.Groups[1].Value);
					listViewItem.SubItems.Add(Regex.Replace(match4.Groups[1].Value, "<[^>]+>", "").Trim());
					listViewItem.SubItems.Add(Regex.Replace(match6.Groups[1].Value, "<[^>]+>", "").Trim());
					listViewItem.SubItems.Add(match5.Groups[1].Value);
					while (!this.zanting)
					{
						Application.DoEvents();
					}
					bool flag = !this.status;
					if (flag)
					{
						break;
					}
					Thread.Sleep(1000);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00017C0C File Offset: 0x00015E0C
		private void 电商抓取_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00017C10 File Offset: 0x00015E10
		private void button1_Click(object sender, EventArgs e)
		{
			this.keyword = this.textBox1.Text;
			string url = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");
			bool flag = url.Contains("aliexpress");
			if (flag)
			{
				this.status = true;
				this.button1.Enabled = false;
				Thread thread = new Thread(new ThreadStart(this.amazon));
				thread.Start();
				Control.CheckForIllegalCrossThreadCalls = false;
			}
			else
			{
				MessageBox.Show("验证失败");
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00017C94 File Offset: 0x00015E94
		private void button5_Click(object sender, EventArgs e)
		{
			method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
		}

		// Token: 0x040001E5 RID: 485
		private string keyword = "";

		// Token: 0x040001E6 RID: 486
		private bool zanting = true;

		// Token: 0x040001E7 RID: 487
		private bool status = true;
	}
}
