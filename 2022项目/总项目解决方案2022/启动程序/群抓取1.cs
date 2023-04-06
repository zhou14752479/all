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
	// Token: 0x02000019 RID: 25
	public partial class 群抓取1 : Form
	{
		// Token: 0x060000FA RID: 250 RVA: 0x0001F74D File Offset: 0x0001D94D
		public 群抓取1()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0001F78D File Offset: 0x0001D98D
		private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
		{
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0001F790 File Offset: 0x0001D990
		public void downloadFile(string URLAddress, string subPath, string name, string COOKIE)
		{
			try
			{
				string currentDirectory = Directory.GetCurrentDirectory();
				WebClient webClient = new WebClient();
				webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
				webClient.Headers.Add("Cookie", COOKIE);
				webClient.Headers.Add("x-access-token", this.token);
				webClient.Headers.Add("Referer", "http://app.jiaqun8.cn/");
				bool flag = !Directory.Exists(subPath);
				if (flag)
				{
					Directory.CreateDirectory(subPath);
				}
				webClient.DownloadFile(URLAddress, subPath + "\\" + name);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0001F850 File Offset: 0x0001DA50
		public static string PostUrl(string url, string postData, string COOKIE, string charset, string token)
		{
			string result;
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.Method = "Post";
				WebHeaderCollection headers = httpWebRequest.Headers;
				headers.Add("x-access-token:" + token);
				httpWebRequest.ContentType = "application/json";
				httpWebRequest.ContentLength = (long)postData.Length;
				httpWebRequest.AllowAutoRedirect = false;
				httpWebRequest.KeepAlive = true;
				httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
				httpWebRequest.Headers.Add("Cookie", COOKIE);
				httpWebRequest.Referer = "https://accounts.ebay.com/acctxs/user";
				StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
				streamWriter.Write(postData);
				streamWriter.Flush();
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				httpWebResponse.GetResponseHeader("Set-Cookie");
				StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding(charset));
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

		// Token: 0x060000FE RID: 254 RVA: 0x0001F974 File Offset: 0x0001DB74
		public static string GetUrl(string Url, string charset, string token)
		{
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				string value = "pgv_info=ssid=s6213364120; pgv_pvid=3922950760; qq_locale_id=2052; skey=MEkNEYphE2; uin=o0852266010; o_cookie=852266010; XWINDEXGREY=0; qz_gdt=r3wy6xq5byabsz637rwq; pac_uid=1_852266010; pvid=7405547776";
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
				httpWebRequest.Referer = " https://qqweb.qq.com/m/relativegroup/index.html?_bid=165&source=qun_rec&_wv=16777216&_cwv=8&keyword=%E6%B8%B8%E6%88%8F";
				httpWebRequest.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/16F203 QQ/8.2.9.604 V1_IPH_SQ_8.2.9_1_APP_A Pixel/1080 MiniAppEnable SimpleUISwitch/0 QQTheme/1000 Core/WKWebView Device/Apple(iPhone 7Plus) NetType/4G QBWebViewType/1 WKType/1";
				httpWebRequest.AllowAutoRedirect = true;
				httpWebRequest.Headers.Add("Cookie", value);
				WebHeaderCollection headers = httpWebRequest.Headers;
				headers.Add("x-access-token:" + token);
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				httpWebRequest.Timeout = 5000;
				StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding(charset));
				string result = streamReader.ReadToEnd();
				streamReader.Close();
				httpWebResponse.Close();
				return result;
			}
			catch (Exception ex)
			{
				ex.ToString();
			}
			return "";
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0001FA60 File Offset: 0x0001DC60
		public void wxqun()
		{
			bool @checked = this.radioButton1.Checked;
			if (@checked)
			{
				this.type = "1";
			}
			bool checked2 = this.radioButton2.Checked;
			if (checked2)
			{
				this.type = "2";
			}
			bool checked3 = this.radioButton3.Checked;
			if (checked3)
			{
				this.type = "3";
			}
			string input = method.PostUrl("http://app.jiaqun8.cn/portal/user/login", "{\"username\":\"17606117606\",\"password\":\"zhoukaige00\"}", this.cookie, "utf-8");
			Match match = Regex.Match(input, "data\":\"([\\s\\S]*?)\"");
			this.token = match.Groups[1].Value;
			try
			{
				for (int i = 1; i < 9999; i++)
				{
					string url = string.Concat(new object[]
					{
						"http://app.jiaqun8.cn/portal/group/view/",
						this.type,
						"/",
						i,
						"?all=false"
					});
					string url2 = 群抓取1.GetUrl(url, "utf-8", match.Groups[1].Value);
					Match match2 = Regex.Match(url2, "publishTime\":\"([\\s\\S]*?)\"");
					Match match3 = Regex.Match(url2, "image\":\"([\\s\\S]*?)\"");
					bool flag = match3.Groups[1].Value == "";
					if (flag)
					{
						break;
					}
					ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
					listViewItem.SubItems.Add(match2.Groups[1].Value);
					listViewItem.SubItems.Add(match3.Groups[1].Value);
					while (!this.zanting)
					{
						Application.DoEvents();
					}
				}
				this.button4.Enabled = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0001FC84 File Offset: 0x0001DE84
		private void 群抓取1_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0001FC88 File Offset: 0x0001DE88
		private void button1_Click(object sender, EventArgs e)
		{
			string url = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");
			bool flag = url.Contains("qunzhuaqu");
			if (flag)
			{
				this.button1.Enabled = false;
				Thread thread = new Thread(new ThreadStart(this.wxqun));
				thread.Start();
				Control.CheckForIllegalCrossThreadCalls = false;
			}
			else
			{
				MessageBox.Show("验证失败");
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0001FCF4 File Offset: 0x0001DEF4
		private void button2_Click(object sender, EventArgs e)
		{
			this.button4.Enabled = true;
			this.zanting = false;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0001FD0C File Offset: 0x0001DF0C
		public void downlode()
		{
			string subPath = AppDomain.CurrentDomain.BaseDirectory + "image\\";
			for (int i = 0; i < this.listView1.Items.Count; i++)
			{
				this.label1.Text = "正在下载第" + i + "个二维码";
				this.downloadFile(this.listView1.Items[i].SubItems[2].Text, subPath, i + ".jpg", "");
			}
			this.label1.Text = "下载完成";
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0001FDC0 File Offset: 0x0001DFC0
		private void button4_Click(object sender, EventArgs e)
		{
			this.button1.Enabled = true;
			Thread thread = new Thread(new ThreadStart(this.downlode));
			thread.Start();
			Control.CheckForIllegalCrossThreadCalls = false;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0001FDFB File Offset: 0x0001DFFB
		private void button3_Click(object sender, EventArgs e)
		{
			this.zanting = true;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0001FE05 File Offset: 0x0001E005
		private void button6_Click(object sender, EventArgs e)
		{
			this.listView1.Items.Clear();
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0001FE19 File Offset: 0x0001E019
		private void button5_Click(object sender, EventArgs e)
		{
			method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
		}

		// Token: 0x0400029C RID: 668
		private bool zanting = true;

		// Token: 0x0400029D RID: 669
		private string type = "1";

		// Token: 0x0400029E RID: 670
		private string token = "";

		// Token: 0x0400029F RID: 671
		private string cookie = "Hm_lvt_5cf1009b3f74aa0e7508611f719e561c=1586663370; Hm_lpvt_5cf1009b3f74aa0e7508611f719e561c=1586663556";
	}
}
