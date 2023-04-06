using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using helper;

namespace 启动程序
{
	// Token: 0x0200001B RID: 27
	public partial class 题库抓取 : Form
	{
		// Token: 0x06000115 RID: 277 RVA: 0x000216FE File Offset: 0x0001F8FE
		public 题库抓取()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00021737 File Offset: 0x0001F937
		private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
		{
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0002173C File Offset: 0x0001F93C
		public void getitems()
		{
			this.comboBox1.Items.Clear();
			this.dic.Clear();
			string url = "https://manfenzhengzhi.ixunke.cn/api/questions_member?myQBank=true&page=1&pageSize=10&app=true&token=" + this.token;
			string url2 = method.GetUrl(url, "utf-8");
			MatchCollection matchCollection = Regex.Matches(url2, "\\{\"id\":([\\s\\S]*?)\\,\"title\":\"([\\s\\S]*?)\"");
			for (int i = 0; i < matchCollection.Count; i++)
			{
				this.comboBox1.Items.Add(matchCollection[i].Groups[2].Value);
				this.dic.Add(matchCollection[i].Groups[2].Value, matchCollection[i].Groups[1].Value);
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0002180C File Offset: 0x0001FA0C
		public void getitems2(string BankID)
		{
			this.comboBox2.Items.Clear();
			this.dic2.Clear();
			string url = "https://manfenzhengzhi.ixunke.cn/api/chapter?qBankId=" + BankID + "&app=true&token=" + this.token;
			string url2 = method.GetUrl(url, "utf-8");
			MatchCollection matchCollection = Regex.Matches(url2, "\\{\"id\":([\\s\\S]*?)\\,\"qBankId\":([\\s\\S]*?)\\,\"title\":\"([\\s\\S]*?)\"");
			for (int i = 0; i < matchCollection.Count; i++)
			{
				this.comboBox2.Items.Add(matchCollection[i].Groups[3].Value);
				this.dic2.Add(matchCollection[i].Groups[3].Value, matchCollection[i].Groups[1].Value);
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000218E4 File Offset: 0x0001FAE4
		public void run()
		{
			string text = this.dic[this.comboBox1.Text];
			string text2 = this.dic2[this.comboBox2.Text];
			try
			{
				string text3 = HttpUtility.UrlEncode(this.comboBox1.Text);
				bool flag = this.comboBox1.Text == "全部";
				if (flag)
				{
				}
				string url = string.Concat(new string[]
				{
					"https://manfenzhengzhi.ixunke.cn/api/question?app=true&token=",
					this.token,
					"&qBankId=",
					text,
					"&chapterId=",
					text2,
					"&practise=1&studentAnswer=true"
				});
				string url2 = method.GetUrl(url, "utf-8");
				MatchCollection matchCollection = Regex.Matches(url2, "\"stem\":\"([\\s\\S]*?)\"\\,");
				MatchCollection matchCollection2 = Regex.Matches(url2, "\"options\":\\[([\\s\\S]*?)\"\\]");
				MatchCollection matchCollection3 = Regex.Matches(url2, "\"analysis\":\"([\\s\\S]*?)\"\\,");
				MatchCollection matchCollection4 = Regex.Matches(url2, "\"answer\":\\[([\\s\\S]*?)\\]");
				int i = 0;
				while (i < matchCollection.Count)
				{
					try
					{
						ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
						listViewItem.SubItems.Add(matchCollection[i].Groups[1].Value);
						listViewItem.SubItems.Add(matchCollection4[i].Groups[1].Value);
						listViewItem.SubItems.Add(matchCollection3[i].Groups[1].Value);
						string[] array = matchCollection2[i].Groups[1].Value.Split(new string[]
						{
							","
						}, StringSplitOptions.None);
						listViewItem.SubItems.Add(array[0]);
						listViewItem.SubItems.Add(array[1]);
						listViewItem.SubItems.Add(array[2]);
						listViewItem.SubItems.Add(array[3]);
					}
					catch
					{
					}
					IL_20A:
					i++;
					continue;
					goto IL_20A;
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00021B50 File Offset: 0x0001FD50
		private void 题库抓取_Load(object sender, EventArgs e)
		{
			Thread thread = new Thread(new ThreadStart(this.getitems));
			thread.Start();
			Control.CheckForIllegalCrossThreadCalls = false;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00021B80 File Offset: 0x0001FD80
		private void Button1_Click(object sender, EventArgs e)
		{
			string url = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");
			bool flag = url.Contains("tikuzhuaqu");
			if (flag)
			{
				this.button1.Enabled = false;
				Thread thread = new Thread(new ThreadStart(this.run));
				thread.Start();
				Control.CheckForIllegalCrossThreadCalls = false;
			}
			else
			{
				MessageBox.Show("验证失败");
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00021BEC File Offset: 0x0001FDEC
		private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.getitems2(this.dic[this.comboBox1.SelectedItem.ToString()]);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00021C11 File Offset: 0x0001FE11
		private void Button3_Click(object sender, EventArgs e)
		{
			this.button1.Enabled = true;
			this.listView1.Items.Clear();
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00021C32 File Offset: 0x0001FE32
		private void Button2_Click(object sender, EventArgs e)
		{
			method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00021C4C File Offset: 0x0001FE4C
		public static void ceshi()
		{
			method.GetUrl("http://www.baidu.com", "utf-8");
			MessageBox.Show("1");
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00021C6A File Offset: 0x0001FE6A
		private void Timer1_Tick(object sender, EventArgs e)
		{
			题库抓取.ceshi();
		}

		// Token: 0x040002C5 RID: 709
		public string token = "VTJGc2RHVmtYMS9yU1lwV3hnaFVERy9zZXRJYTkvRHZwM2ozakhtdllnWlZFOS9ZUnJZcERFZThkVGo1UmNOV2Fhbk4zQkV5VzJUVzRhMnMxem9iQ2c9PSMxNTg1ODEwMTI3MDI0";

		// Token: 0x040002C6 RID: 710
		private Dictionary<string, string> dic = new Dictionary<string, string>();

		// Token: 0x040002C7 RID: 711
		private Dictionary<string, string> dic2 = new Dictionary<string, string>();
	}
}
