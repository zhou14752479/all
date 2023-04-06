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

namespace 启动程序
{
	// Token: 0x0200000B RID: 11
	public partial class 图片下载 : Form
	{
		// Token: 0x0600004F RID: 79 RVA: 0x0000CCA5 File Offset: 0x0000AEA5
		public 图片下载()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000CCD4 File Offset: 0x0000AED4
		public static void downloadFile(string URLAddress, string subPath, string name, string COOKIE)
		{
			try
			{
				string currentDirectory = Directory.GetCurrentDirectory();
				WebClient webClient = new WebClient();
				webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
				webClient.Headers.Add("Cookie", COOKIE);
				webClient.Headers.Add("Referer", "https://m.mm131.net/chemo/89_5.html");
				bool flag = !Directory.Exists(subPath);
				if (flag)
				{
					Directory.CreateDirectory(subPath);
				}
				webClient.DownloadFile(URLAddress, subPath + "\\" + name);
			}
			catch
			{
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000CD70 File Offset: 0x0000AF70
		public static string GetUrl(string Url, string COOKIE)
		{
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
				httpWebRequest.Referer = "https://m.mm131.net/chemo/89_5.html";
				httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
				httpWebRequest.AllowAutoRedirect = true;
				httpWebRequest.Headers.Add("Cookie", COOKIE);
				WebHeaderCollection headers = httpWebRequest.Headers;
				headers.Add("sec-fetch-mode:navigate");
				headers.Add("sec-fetch-site:same-origin");
				headers.Add("sec-fetch-user:?1");
				headers.Add("upgrade-insecure-requests: 1");
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				httpWebRequest.Timeout = 5000;
				StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));
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

		// Token: 0x06000052 RID: 82 RVA: 0x0000CE74 File Offset: 0x0000B074
		private void 图片下载_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000CE78 File Offset: 0x0000B078
		public void run()
		{
			try
			{
				string cookie = "Hm_lvt_672e68bf7e214b45f4790840981cdf99=1586074013; UM_distinctid=1714960cd47138-019f4ff4f49f88-2393f61-1fa400-1714960cd48262; CNZZDATA1277874215=493529657-1586070373-%7C1586075773; Hm_lpvt_672e68bf7e214b45f4790840981cdf99=1586076817";
				for (int i = 0; i < 999; i++)
				{
					string url = "https://m.mm131.net/more.php?page=" + i;
					string url2 = 图片下载.GetUrl(url, cookie);
					MatchCollection matchCollection = Regex.Matches(url2, "data-img=\"https:\\/\\/img1\\.mmmw\\.net\\/pic\\/([\\s\\S]*?)\\/");
					MatchCollection matchCollection2 = Regex.Matches(url2, "alt=\"([\\s\\S]*?)\"");
					for (int j = 0; j < matchCollection.Count; j++)
					{
						string text = HttpUtility.UrlDecode(matchCollection2[j].Groups[1].Value);
						string subPath = this.path + text + "/";
						bool flag = !Directory.Exists(subPath);
						if (flag)
						{
							Directory.CreateDirectory(subPath);
						}
						for (int k = 1; k < 99; k++)
						{
							图片下载.downloadFile(string.Concat(new object[]
							{
								"https://img1.mmmw.net/pic/",
								matchCollection[j].Groups[1].Value,
								"/",
								k,
								".jpg"
							}), subPath, k + ".jpg", cookie);
							TextBox textBox = this.textBox1;
							textBox.Text = string.Concat(new object[]
							{
								textBox.Text,
								"正在下载：",
								text,
								"  第",
								k,
								"张\r\n"
							});
							this.textBox1.Focus();
							this.textBox1.Select(this.textBox1.TextLength, 0);
							this.textBox1.ScrollToCaret();
							Thread.Sleep(100);
							while (!this.zanting)
							{
								Application.DoEvents();
							}
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000D094 File Offset: 0x0000B294
		private void button1_Click(object sender, EventArgs e)
		{
			string url = 图片下载.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");
			bool flag = url.Contains("tupianxiazai");
			if (flag)
			{
				bool flag2 = !this.zanting;
				if (flag2)
				{
					this.zanting = true;
				}
				else
				{
					Thread thread = new Thread(new ThreadStart(this.run));
					Control.CheckForIllegalCrossThreadCalls = false;
					thread.Start();
				}
			}
			else
			{
				MessageBox.Show("验证失败");
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000D10D File Offset: 0x0000B30D
		private void button2_Click(object sender, EventArgs e)
		{
			this.zanting = false;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000D118 File Offset: 0x0000B318
		private void 图片下载_FormClosing(object sender, FormClosingEventArgs e)
		{
			DialogResult dialogResult = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
			bool flag = dialogResult == DialogResult.OK;
			if (flag)
			{
				Environment.Exit(0);
			}
			else
			{
				e.Cancel = true;
			}
		}

		// Token: 0x04000114 RID: 276
		private string path = AppDomain.CurrentDomain.BaseDirectory;

		// Token: 0x04000115 RID: 277
		private bool zanting = true;
	}
}
