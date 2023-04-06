using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using helper;

namespace 启动程序
{
	// Token: 0x02000009 RID: 9
	public partial class 八爪盒子 : Form
	{
		// Token: 0x06000039 RID: 57 RVA: 0x0000A5DF File Offset: 0x000087DF
		public 八爪盒子()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000A608 File Offset: 0x00008808
		public static string PostUrl(string url, string postData)
		{
			string result;
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.Method = "Post";
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				WebHeaderCollection headers = httpWebRequest.Headers;
				headers.Add("Authorization: Basic ZjMwNWMzNjBlNThmMWRiMDFmNGJhY2Q2MmU2ODFmYzk6ZTE1NDUxYmZiZjdjMzk0YzEyOTI2ODhhOTc3YjUxYTk=");
				headers.Add("Agent-info: client=ios;osVersion=12.3.1;screenWidth=1242;screenHeight=2208;appVersion");
				headers.Add("Agent-Info2: BaZhuaHeZiOK");
				httpWebRequest.ContentLength = (long)postData.Length;
				httpWebRequest.KeepAlive = true;
				httpWebRequest.UserAgent = "BaZhuaHeZi-iOS/5.1 Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148";
				httpWebRequest.Headers.Add("origin", "http://api.ibole.net");
				httpWebRequest.Referer = "https://accounts.ebay.com/acctxs/user";
				StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
				streamWriter.Write(postData);
				streamWriter.Flush();
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				httpWebResponse.GetResponseHeader("Set-Cookie");
				StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));
				string text = streamReader.ReadToEnd();
				streamReader.Close();
				httpWebResponse.Close();
				result = text;
			}
			catch (WebException ex)
			{
				result = ex.ToString();
			}
			return result;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000A74C File Offset: 0x0000894C
		public string getcount()
		{
			string url = "https://api.ibole.net/candidate/mine";
			string postData = string.Concat(new string[]
			{
				"offset=0&pageSize=10&storageDateEnd=",
				this.textBox2.Text,
				"%2023%3A59%3A59&storageDateStart=",
				this.textBox1.Text,
				"%2000%3A00%3A00"
			});
			string input = 八爪盒子.PostUrl(url, postData);
			Match match = Regex.Match(input, "\"totalCount\":([\\s\\S]*?)\\}");
			return match.Groups[1].Value;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000A7CC File Offset: 0x000089CC
		private DateTime ConvertStringToDateTime(string timeStamp)
		{
			DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			long ticks = long.Parse(timeStamp + "0000");
			TimeSpan value = new TimeSpan(ticks);
			return dateTime.Add(value);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000A818 File Offset: 0x00008A18
		public void run()
		{
			this.label5.Text = this.getcount();
			int num = 0;
			try
			{
				for (int i = 0; i < 100001; i += 10)
				{
					string url = "https://api.ibole.net/candidate/mine";
					string postData = string.Concat(new object[]
					{
						"offset=",
						i,
						"&pageSize=10&storageDateEnd=",
						this.textBox2.Text,
						"%2023%3A59%3A59&storageDateStart=",
						this.textBox1.Text,
						"%2000%3A00%3A00"
					});
					string input = 八爪盒子.PostUrl(url, postData);
					MatchCollection matchCollection = Regex.Matches(input, "\"candidateId\":\"([\\s\\S]*?)\"");
					bool flag = matchCollection.Count == 0;
					if (flag)
					{
						break;
					}
					foreach (object obj in matchCollection)
					{
						Match match = (Match)obj;
						string input2 = 八爪盒子.PostUrl("https://api.ibole.net/candidate/detail", "candidateId=" + match.Groups[1].Value);
						Match match2 = Regex.Match(input2, "\"realName\":\"([\\s\\S]*?)\"");
						Match match3 = Regex.Match(input2, "职位：([\\s\\S]*?)\\\\r");
						Match match4 = Regex.Match(input2, "地址：([\\s\\S]*?)\\\\r");
						Match match5 = Regex.Match(input2, "工作年限：([\\s\\S]*?)\\\\r");
						Match match6 = Regex.Match(input2, "性别：([\\s\\S]*?)\\\\r");
						Match match7 = Regex.Match(input2, "生日：([\\s\\S]*?)\\\\r");
						Match match8 = Regex.Match(input2, "\"companyName\":\"([\\s\\S]*?)\"");
						Match match9 = Regex.Match(input2, "\"school\":\"([\\s\\S]*?)\"");
						Match match10 = Regex.Match(input2, "专业：([\\s\\S]*?)\\\\r");
						Match match11 = Regex.Match(input2, "学位：([\\s\\S]*?)\\\\r");
						Match match12 = Regex.Match(input2, "手机：([\\s\\S]*?)\\\\r");
						Match match13 = Regex.Match(input2, "邮箱：([\\s\\S]*?)\\\\r");
						Match match14 = Regex.Match(input2, "自我评价：([\\s\\S]*?)\\\\r");
						Match match15 = Regex.Match(input2, "求职意向\\\\r\\\\n([\\s\\S]*?)  ([\\s\\S]*?) ");
						Match match16 = Regex.Match(input2, "\"eduList\":\\[([\\s\\S]*?)\\],");
						MatchCollection matchCollection2 = Regex.Matches(match16.Groups[1].Value, "startDate\":([\\s\\S]*?)\\}");
						MatchCollection matchCollection3 = Regex.Matches(match16.Groups[1].Value, "endDate\":([\\s\\S]*?),");
						MatchCollection matchCollection4 = Regex.Matches(match16.Groups[1].Value, "degree\":([\\s\\S]*?),");
						MatchCollection matchCollection5 = Regex.Matches(match16.Groups[1].Value, "schoolName\":\"([\\s\\S]*?)\"");
						MatchCollection matchCollection6 = Regex.Matches(match16.Groups[1].Value, "majorName\":\"([\\s\\S]*?)\"");
						Match match17 = Regex.Match(input2, "workList\":\\[([\\s\\S]*?)\\]");
						MatchCollection matchCollection7 = Regex.Matches(match17.Groups[1].Value, "startDate\":([\\s\\S]*?)\\,");
						MatchCollection matchCollection8 = Regex.Matches(match17.Groups[1].Value, "endDate\":([\\s\\S]*?)\\,");
						MatchCollection matchCollection9 = Regex.Matches(match17.Groups[1].Value, "companyName\":\"([\\s\\S]*?)\"");
						MatchCollection matchCollection10 = Regex.Matches(match17.Groups[1].Value, "companyType\":([\\s\\S]*?)\\,");
						MatchCollection matchCollection11 = Regex.Matches(match17.Groups[1].Value, "title\":\"([\\s\\S]*?)\"");
						MatchCollection matchCollection12 = Regex.Matches(match17.Groups[1].Value, "salary\":([\\s\\S]*?)\\,");
						MatchCollection matchCollection13 = Regex.Matches(match17.Groups[1].Value, "responsibility\":\"([\\s\\S]*?)\"");
						int num2 = (matchCollection2.Count > matchCollection7.Count) ? matchCollection2.Count : matchCollection7.Count;
						for (int j = 0; j < num2; j++)
						{
							ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
							for (int k = 0; k < 27; k++)
							{
								listViewItem.SubItems.Add("");
							}
							bool flag2 = j == 0;
							if (flag2)
							{
								listViewItem.SubItems[1].Text = match2.Groups[1].Value;
								listViewItem.SubItems[2].Text = match3.Groups[1].Value;
								listViewItem.SubItems[3].Text = match4.Groups[1].Value;
								listViewItem.SubItems[4].Text = match5.Groups[1].Value;
								listViewItem.SubItems[5].Text = match6.Groups[1].Value;
								listViewItem.SubItems[6].Text = match7.Groups[1].Value;
								listViewItem.SubItems[7].Text = match8.Groups[1].Value;
								listViewItem.SubItems[8].Text = match9.Groups[1].Value;
								listViewItem.SubItems[9].Text = match10.Groups[1].Value;
								listViewItem.SubItems[10].Text = match11.Groups[1].Value;
								listViewItem.SubItems[11].Text = match12.Groups[1].Value;
								listViewItem.SubItems[12].Text = match13.Groups[1].Value;
								listViewItem.SubItems[13].Text = match14.Groups[1].Value;
								listViewItem.SubItems[14].Text = match15.Groups[1].Value;
								listViewItem.SubItems[15].Text = match15.Groups[2].Value;
							}
							bool flag3 = j < matchCollection2.Count;
							if (flag3)
							{
								listViewItem.SubItems[16].Text = this.ConvertStringToDateTime(matchCollection2[j].Groups[1].Value).ToString();
								listViewItem.SubItems[17].Text = this.ConvertStringToDateTime(matchCollection3[j].Groups[1].Value).ToString();
								listViewItem.SubItems[18].Text = matchCollection4[j].Groups[1].Value;
								listViewItem.SubItems[19].Text = matchCollection5[j].Groups[1].Value;
								listViewItem.SubItems[20].Text = matchCollection6[j].Groups[1].Value;
							}
							bool flag4 = j < matchCollection7.Count;
							if (flag4)
							{
								listViewItem.SubItems[21].Text = this.ConvertStringToDateTime(matchCollection7[j].Groups[1].Value).ToString();
								listViewItem.SubItems[22].Text = this.ConvertStringToDateTime(matchCollection8[j].Groups[1].Value).ToString();
								listViewItem.SubItems[23].Text = matchCollection9[j].Groups[1].Value;
								listViewItem.SubItems[24].Text = matchCollection10[j].Groups[1].Value;
								listViewItem.SubItems[25].Text = matchCollection11[j].Groups[1].Value;
								listViewItem.SubItems[26].Text = matchCollection12[j].Groups[1].Value;
								listViewItem.SubItems[27].Text = matchCollection13[j].Groups[1].Value;
							}
						}
						num++;
						this.label6.Text = num.ToString();
						while (!this.zanting)
						{
							Application.DoEvents();
						}
						bool flag5 = !this.status;
						if (flag5)
						{
							return;
						}
						Thread.Sleep(1000);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000B188 File Offset: 0x00009388
		private void 八爪盒子_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0000B18C File Offset: 0x0000938C
		private void Button5_Click(object sender, EventArgs e)
		{
			string url = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");
			bool flag = url.Contains("bazhuahezi");
			if (flag)
			{
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

		// Token: 0x06000040 RID: 64 RVA: 0x0000B1F2 File Offset: 0x000093F2
		private void Button6_Click(object sender, EventArgs e)
		{
			method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000B20C File Offset: 0x0000940C
		private void Button2_Click(object sender, EventArgs e)
		{
			this.zanting = false;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000B216 File Offset: 0x00009416
		private void Button3_Click(object sender, EventArgs e)
		{
			this.zanting = true;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000B220 File Offset: 0x00009420
		private void Button4_Click(object sender, EventArgs e)
		{
			this.status = false;
		}

		// Token: 0x040000D2 RID: 210
		private bool zanting = true;

		// Token: 0x040000D3 RID: 211
		private bool status = true;
	}
}
