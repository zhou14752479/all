using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using helper;

namespace 启动程序
{
	// Token: 0x02000018 RID: 24
	public partial class 群抓取 : Form
	{
		// Token: 0x060000E9 RID: 233 RVA: 0x0001E5D2 File Offset: 0x0001C7D2
		public 群抓取()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0001E607 File Offset: 0x0001C807
		private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
		{
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0001E60C File Offset: 0x0001C80C
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

		// Token: 0x060000EC RID: 236 RVA: 0x0001E730 File Offset: 0x0001C930
		public static string GetUrl(string Url, string charset)
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
				headers.Add("sec-fetch-mode:navigate");
				headers.Add("sec-fetch-site:same-origin");
				headers.Add("sec-fetch-user:?1");
				headers.Add("upgrade-insecure-requests: 1");
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

		// Token: 0x060000ED RID: 237 RVA: 0x0001E838 File Offset: 0x0001CA38
		public void wxqun()
		{
			string input = method.PostUrl("http://app.jiaqun8.cn/portal/user/login", "{\"username\":\"17606117606\",\"password\":\"zhoukaige00\"}", this.cookie, "utf-8");
			Match match = Regex.Match(input, "data\":\"([\\s\\S]*?)\"");
			this.token = match.Groups[1].Value;
			try
			{
				string[] array = this.textBox1.Text.Split(new string[]
				{
					"\r\n"
				}, StringSplitOptions.None);
				for (int i = 0; i < array.Length; i++)
				{
					for (int j = 1; j < 9999; j++)
					{
						string url = string.Concat(new object[]
						{
							"http://app.jiaqun8.cn/portal/group/search/",
							j,
							"?keyword=",
							HttpUtility.UrlEncode(array[i])
						});
						string postData = "keyword: " + HttpUtility.UrlEncode(array[i]);
						string input2 = 群抓取.PostUrl(url, postData, this.cookie, "utf-8", match.Groups[1].Value);
						Match match2 = Regex.Match(input2, "publishTime\":\"([\\s\\S]*?)\"");
						Match match3 = Regex.Match(input2, "image\":\"([\\s\\S]*?)\"");
						bool flag = match3.Groups[1].Value == "";
						if (flag)
						{
							break;
						}
						ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
						listViewItem.SubItems.Add(match2.Groups[1].Value);
						listViewItem.SubItems.Add(match3.Groups[1].Value);
						listViewItem.SubItems.Add(array[i]);
						while (!this.zanting)
						{
							Application.DoEvents();
						}
					}
				}
				this.button4.Enabled = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0001EA68 File Offset: 0x0001CC68
		public void QQqun()
		{
			try
			{
				string[] array = this.textBox1.Text.Split(new string[]
				{
					"\r\n"
				}, StringSplitOptions.None);
				for (int i = 0; i < array.Length; i++)
				{
					bool flag = array[i] != "";
					if (flag)
					{
						string url = "https://qun.qq.com/cgi-bin/group_search/group_search?retype=2&keyword=游戏&page=0&wantnum=20&city_flag=0&distance=1&ver=1&from=9&bkn=765861501&style=1";
						string url2 = 群抓取.GetUrl(url, "utf-8");
						this.textBox1.Text = url2;
						Match match = Regex.Match(url2, "encryptedUserId\\\\\":\\\\\"([\\s\\S]*?)\\\\");
						ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
						listViewItem.SubItems.Add(array[i]);
						while (!this.zanting)
						{
							Application.DoEvents();
						}
						Thread.Sleep(2000);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0001EB80 File Offset: 0x0001CD80
		private void Button1_Click(object sender, EventArgs e)
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

		// Token: 0x060000F0 RID: 240 RVA: 0x0001EBEC File Offset: 0x0001CDEC
		private void ceshi(object sender, EventArgs e)
		{
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0001EBEF File Offset: 0x0001CDEF
		private void 群抓取_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0001EBF2 File Offset: 0x0001CDF2
		private void button3_Click(object sender, EventArgs e)
		{
			this.zanting = true;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0001EBFC File Offset: 0x0001CDFC
		private void button2_Click(object sender, EventArgs e)
		{
			this.button4.Enabled = true;
			this.zanting = false;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0001EC14 File Offset: 0x0001CE14
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

		// Token: 0x060000F5 RID: 245 RVA: 0x0001ECD4 File Offset: 0x0001CED4
		private void button5_Click(object sender, EventArgs e)
		{
			method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0001ECF0 File Offset: 0x0001CEF0
		private void button4_Click(object sender, EventArgs e)
		{
			this.button1.Enabled = true;
			string subPath = AppDomain.CurrentDomain.BaseDirectory + "image\\";
			for (int i = 0; i < this.listView1.Items.Count; i++)
			{
				this.label1.Text = "正在下载第" + i + "个二维码";
				this.downloadFile(this.listView1.Items[i].SubItems[2].Text, subPath, i + ".jpg", "");
			}
			this.label1.Text = "下载完成";
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0001EDB1 File Offset: 0x0001CFB1
		private void button6_Click(object sender, EventArgs e)
		{
			this.listView1.Items.Clear();
		}

		// Token: 0x04000289 RID: 649
		private bool zanting = true;

		// Token: 0x0400028A RID: 650
		private string token = "";

		// Token: 0x0400028B RID: 651
		private string cookie = "Hm_lvt_5cf1009b3f74aa0e7508611f719e561c=1586663370; Hm_lpvt_5cf1009b3f74aa0e7508611f719e561c=1586663556";
	}
}
