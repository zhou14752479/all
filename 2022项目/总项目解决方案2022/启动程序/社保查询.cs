using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using helper;

namespace 启动程序
{
	// Token: 0x02000016 RID: 22
	public partial class 社保查询 : Form
	{
		// Token: 0x060000CE RID: 206 RVA: 0x00019B2C File Offset: 0x00017D2C
		public 社保查询()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00019B7E File Offset: 0x00017D7E
		private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
		{
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00019B84 File Offset: 0x00017D84
		public static string GetTimeStamp()
		{
			return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds).ToString();
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00019BC8 File Offset: 0x00017DC8
		public string GetMD5(string txt)
		{
			string result;
			using (MD5 md = MD5.Create())
			{
				byte[] bytes = Encoding.Default.GetBytes(txt);
				byte[] array = md.ComputeHash(bytes);
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < array.Length; i++)
				{
					stringBuilder.Append(array[i].ToString("x2"));
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00019C54 File Offset: 0x00017E54
		public static string GetUrl(string Url, string charset)
		{
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				string value = "";
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
				httpWebRequest.Referer = "http://app.gjzwfw.gov.cn/jmopen/webapp/html5/unZip/21804951cd294d869e0f92c27ba118a6/qyylbacbrypc/index.html";
				httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
				httpWebRequest.AllowAutoRedirect = true;
				httpWebRequest.Headers.Add("Cookie", value);
				WebHeaderCollection headers = httpWebRequest.Headers;
				headers.Add("sec-fetch-mode:navigate");
				headers.Add("sec-fetch-site:same-origin");
				headers.Add("sec-fetch-user:?1");
				headers.Add("upgrade-insecure-requests: 1");
				httpWebRequest.KeepAlive = true;
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

		// Token: 0x060000D3 RID: 211 RVA: 0x00019D64 File Offset: 0x00017F64
		public void run()
		{
			string[] array = this.textBox1.Text.Split(new string[]
			{
				"\r\n"
			}, StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				Match match = Regex.Match(array[i], "[\\u4e00-\\u9fa5]*");
				string text = Regex.Replace(array[i], "[\\u4e00-\\u9fa5]*", "");
				string text2 = HttpUtility.UrlEncode(match.Groups[0].Value.Trim());
				string text3 = text.Trim();
				string url = string.Concat(new string[]
				{
					"http://app.gjzwfw.gov.cn/jmopen/interfaces/wxTransferPort.do?callback=jQuery18309492701749972507_",
					this.time,
					"&requestUrl=http%3A%2F%2Fapp.gjzwfw.gov.cn%2Fjimps%2Flink.do&datas=dhzkh%22param%22%3A%22dhzkh%5C%22from%5C%22%3A%5C%221%5C%22%2C%5C%22key%5C%22%3A%5C%2291da7d51a42542219852bb3df4399d03%5C%22%2C%5C%22requestTime%5C%22%3A%5C%22",
					this.time,
					"%5C%22%2C%5C%22sign%5C%22%3A%5C%22",
					this.sign,
					"%5C%22%2C%5C%22zj_ggsjpt_app_key%5C%22%3A%5C%22ada72850-2b2e-11e7-985b-008cfaeb3d74%5C%22%2C%5C%22zj_ggsjpt_sign%5C%22%3A%5C%22",
					this.ggsjpt_sign,
					"%5C%22%2C%5C%22zj_ggsjpt_time%5C%22%3A%5C%22",
					this.time,
					"%5C%22%2C%5C%22name%5C%22%3A%5C%22",
					text2,
					"%5C%22%2C%5C%22cardId%5C%22%3A%5C%22",
					text3,
					"%5C%22%2C%5C%22additional%5C%22%3A%5C%22%5C%22dhykh%22dhykh&heads=&_=",
					this.time
				});
				string url2 = 社保查询.GetUrl(url, "utf-8");
				Match match2 = Regex.Match(url2, "\"companyName\":\"([\\s\\S]*?)\"");
				Match match3 = Regex.Match(url2, "\"personelNo\":\"([\\s\\S]*?)\"");
				Match match4 = Regex.Match(url2, "\"insuranceType\":\"([\\s\\S]*?)\"");
				Match match5 = Regex.Match(url2, "\"addr\":\"([\\s\\S]*?)\"");
				ListViewItem listViewItem = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
				listViewItem.SubItems.Add(match.Groups[0].Value);
				listViewItem.SubItems.Add(text);
				listViewItem.SubItems.Add(match2.Groups[1].Value);
				listViewItem.SubItems.Add(match3.Groups[1].Value);
				listViewItem.SubItems.Add(match4.Groups[1].Value);
				listViewItem.SubItems.Add(match5.Groups[1].Value);
				Thread.Sleep(1000);
				while (!this.zanting)
				{
					Application.DoEvents();
				}
				bool flag = !this.status;
				if (flag)
				{
					break;
				}
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00019FD7 File Offset: 0x000181D7
		private void 社保查询_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00019FDC File Offset: 0x000181DC
		private void button1_Click(object sender, EventArgs e)
		{
			string url = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");
			bool flag = url.Contains("shebaochaxun");
			if (flag)
			{
				this.status = true;
				this.button1.Enabled = false;
				this.time = 社保查询.GetTimeStamp();
				this.sign = this.GetMD5("qyylbacbrypc" + this.time);
				this.ggsjpt_sign = this.GetMD5("ada72850-2b2e-11e7-985b-008cfaeb3d74995e00df72f14bbcb7833a9ca063adef" + this.time);
				Thread thread = new Thread(new ThreadStart(this.run));
				thread.Start();
				Control.CheckForIllegalCrossThreadCalls = false;
			}
			else
			{
				MessageBox.Show("验证失败");
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0001A092 File Offset: 0x00018292
		private void Button2_Click(object sender, EventArgs e)
		{
			this.zanting = false;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0001A09C File Offset: 0x0001829C
		private void Button3_Click(object sender, EventArgs e)
		{
			this.zanting = true;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0001A0A6 File Offset: 0x000182A6
		private void Button4_Click(object sender, EventArgs e)
		{
			this.button1.Enabled = true;
			this.status = false;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0001A0BD File Offset: 0x000182BD
		private void Button5_Click(object sender, EventArgs e)
		{
			method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
		}

		// Token: 0x0400021A RID: 538
		public string time = "";

		// Token: 0x0400021B RID: 539
		public string sign = "";

		// Token: 0x0400021C RID: 540
		public string ggsjpt_sign = "";

		// Token: 0x0400021D RID: 541
		private bool zanting = true;

		// Token: 0x0400021E RID: 542
		private bool status = true;
	}
}
