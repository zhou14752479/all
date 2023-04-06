using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using helper;

namespace 启动程序
{
	// Token: 0x02000002 RID: 2
	public partial class 厦门大学 : Form
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public 厦门大学()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002070 File Offset: 0x00000270
		public void run()
		{
			try
			{
				string cookies = method.GetCookies("http://moa.xmu.edu.cn/km/imissive/km_imissive_sign_main/kmImissiveSignMain.do?method=listChildren&categoryId=&q.s_raq=0.012296960934379708&pageno=2&rowsize=15&orderby=docCreateTime&ordertype=down&s_ajax=true");
				string url = "http://moa.xmu.edu.cn/km/review/km_review_index/kmReviewIndex.do?method=list&q.s_raq=0.08639820153425903&pageno=1&rowsize=999&orderby=docCreateTime&ordertype=down&s_ajax=true";
				string urlWithCookie = method.GetUrlWithCookie(url, cookies, "utf-8");
				MatchCollection matchCollection = Regex.Matches(urlWithCookie, "fdId\",\"value\":\"([\\s\\S]*?)\"");
				foreach (object obj in matchCollection)
				{
					Match match = (Match)obj;
					string url2 = "http://moa.xmu.edu.cn/km/review/km_review_main/kmReviewMain.do?method=view&fdId=" + match.Groups[1].Value;
					string urlWithCookie2 = method.GetUrlWithCookie(url2, cookies, "utf-8");
					Match match2 = Regex.Match(urlWithCookie2, "教工号([\\s\\S]*?)\\<\\/xformflag\\>");
					Match match3 = Regex.Match(urlWithCookie2, "出生省市([\\s\\S]*?)\\<\\/xformflag\\>");
					Match match4 = Regex.Match(urlWithCookie2, "姓名([\\s\\S]*?)\\<\\/xformflag\\>");
					Match match5 = Regex.Match(urlWithCookie2, "出生日期([\\s\\S]*?)\\<\\/xformflag\\>");
					Match match6 = Regex.Match(urlWithCookie2, "户口所在地([\\s\\S]*?)\\<\\/xformflag\\>");
					Match match7 = Regex.Match(urlWithCookie2, "所在学院\\/部门([\\s\\S]*?)\\<\\/xformflag\\>");
					Match match8 = Regex.Match(urlWithCookie2, "联系电话([\\s\\S]*?)\\<\\/xformflag\\>");
					Match match9 = Regex.Match(urlWithCookie2, "配偶姓名([\\s\\S]*?)\\<\\/xformflag\\>");
					Match match10 = Regex.Match(urlWithCookie2, "职务\\/职称([\\s\\S]*?)\\<\\/xformflag\\>");
					Match match11 = Regex.Match(urlWithCookie2, "邮箱([\\s\\S]*?)\\<\\/xformflag\\>");
					Match match12 = Regex.Match(urlWithCookie2, "配偶电话([\\s\\S]*?)\\<\\/xformflag\\>");
					Match match13 = Regex.Match(urlWithCookie2, "前往国家\\/地区([\\s\\S]*?)\\<\\/xformflag\\>");
					Match match14 = Regex.Match(urlWithCookie2, "包括转机城市\\）([\\s\\S]*?)\\<\\/xformflag\\>");
					Match match15 = Regex.Match(urlWithCookie2, "姓名\\（中文\\）([\\s\\S]*?)\\<\\/xformflag\\>");
					Match match16 = Regex.Match(urlWithCookie2, "启程日期([\\s\\S]*?)\\<\\/xformflag\\>");
					Match match17 = Regex.Match(urlWithCookie2, "内日期([\\s\\S]*?)\\<\\/xformflag\\>");
					Match match18 = Regex.Match(urlWithCookie2, "_Calculation\" value=\"([\\s\\S]*?)\"");
					Match match19 = Regex.Match(urlWithCookie2, "出访内容([\\s\\S]*?)\\<\\/xformflag\\>");
					bool flag = match2.Groups[1].Value != "";
					if (flag)
					{
						ListViewItem listViewItem = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
						listViewItem.SubItems.Add(Regex.Replace(match2.Groups[1].Value, "<[^>]+>", "").Trim());
						listViewItem.SubItems.Add(Regex.Replace(match3.Groups[1].Value, "<[^>]+>", "").Trim());
						listViewItem.SubItems.Add(Regex.Replace(match4.Groups[1].Value, "<[^>]+>", "").Trim());
						listViewItem.SubItems.Add(Regex.Replace(match5.Groups[1].Value, "<[^>]+>", "").Trim());
						listViewItem.SubItems.Add(Regex.Replace(match6.Groups[1].Value, "<[^>]+>", "").Trim());
						listViewItem.SubItems.Add(Regex.Replace(match7.Groups[1].Value, "<[^>]+>", "").Trim());
						listViewItem.SubItems.Add(Regex.Replace(match8.Groups[1].Value, "<[^>]+>", "").Trim());
						listViewItem.SubItems.Add(Regex.Replace(match9.Groups[1].Value, "<[^>]+>", "").Trim());
						listViewItem.SubItems.Add(Regex.Replace(match10.Groups[1].Value, "<[^>]+>", "").Trim());
						listViewItem.SubItems.Add(Regex.Replace(match11.Groups[1].Value, "<[^>]+>", "").Trim());
						listViewItem.SubItems.Add(Regex.Replace(match12.Groups[1].Value, "<[^>]+>", "").Trim());
						listViewItem.SubItems.Add(Regex.Replace(match13.Groups[1].Value, "<[^>]+>", "").Trim());
						listViewItem.SubItems.Add(Regex.Replace(match14.Groups[1].Value, "<[^>]+>", "").Trim());
						listViewItem.SubItems.Add(Regex.Replace(match15.Groups[1].Value, "<[^>]+>", "").Trim());
						listViewItem.SubItems.Add(Regex.Replace(match16.Groups[1].Value, "<[^>]+>", "").Trim());
						listViewItem.SubItems.Add(Regex.Replace(match17.Groups[1].Value, "<[^>]+>", "").Trim());
						listViewItem.SubItems.Add(Regex.Replace(match18.Groups[1].Value, "<[^>]+>", "").Trim());
						listViewItem.SubItems.Add(Regex.Replace(match19.Groups[1].Value, "<[^>]+>", "").Trim());
						while (!this.zanting)
						{
							Application.DoEvents();
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002654 File Offset: 0x00000854
		private void Form1_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002658 File Offset: 0x00000858
		private void Button1_Click(object sender, EventArgs e)
		{
			this.webBrowser1.Visible = false;
			Thread thread = new Thread(new ThreadStart(this.run));
			thread.Start();
			Control.CheckForIllegalCrossThreadCalls = false;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002693 File Offset: 0x00000893
		private void button2_Click(object sender, EventArgs e)
		{
			this.zanting = false;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000269D File Offset: 0x0000089D
		private void button3_Click(object sender, EventArgs e)
		{
			this.zanting = true;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000026A7 File Offset: 0x000008A7
		private void button4_Click(object sender, EventArgs e)
		{
			method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000026C1 File Offset: 0x000008C1
		private void button5_Click(object sender, EventArgs e)
		{
			this.webBrowser1.Visible = true;
			this.webBrowser1.Navigate("http://ids.xmu.edu.cn/authserver/login?service=http://moa.xmu.edu.cn/index.jsp");
		}

		// Token: 0x04000001 RID: 1
		private bool zanting = true;
	}
}
