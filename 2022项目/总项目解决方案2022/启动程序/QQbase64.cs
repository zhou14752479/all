using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace 启动程序
{
	// Token: 0x02000004 RID: 4
	public partial class QQbase64 : Form
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00003086 File Offset: 0x00001286
		public QQbase64()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000030A0 File Offset: 0x000012A0
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

		// Token: 0x0600000E RID: 14 RVA: 0x0000312C File Offset: 0x0000132C
		public static string Base64Encode(Encoding encodeType, string source)
		{
			string result = string.Empty;
			byte[] bytes = encodeType.GetBytes(source);
			try
			{
				result = Convert.ToBase64String(bytes);
			}
			catch
			{
				result = source;
			}
			return result;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00003170 File Offset: 0x00001370
		public static string GetUrl(string Url, string charset)
		{
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				string value = "session-id=144-7450304-7580635; session-id-time=2082787201l; ubid-main=133-9411273-8184314; x-wl-uid=1AR+eCj1iY57TRhM7A2m5KF9SEb1ho13Om87l60jAFJAp10qHX8GNgnZcOFTknCbmNkftPnMho/k=; aws-priv=eyJ2IjoxLCJldSI6MCwic3QiOjB9; aws-target-static-id=1536650638823-915613; s_fid=16BD3861C3483809-386224FB67B4E94E; regStatus=pre-register; s_dslv=1536656308918; i18n-prefs=USD; lc-main=zh_CN; sp-cdn=\"L5Z9: CN\"; session-token=/8/yst6nJSzUghSOya1omO6MEhQ/Moyyq2FsFStf5zcm4cZPhl38RIpfC+UZyiw//J9HubG+McoZMSB4hRyykQZ0SH1X07eSi5nxcOjmHQshqSmCJD6tL8cgFOFCByRnF1EJMjmxRfVwTkZZ/4yLqjzBQ2Ik6WclU4tG1u7+4UCFeGDYa//WLb3fCGfB6RuU; csm-hit=tb:DT2JH7KAE9BTWY50PJA8+s-DT2JH7KAE9BTWY50PJA8|1585472314824&t:1585472314824&adb:adblk_no";
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
				httpWebRequest.Referer = "https://www.amazon.com/s?k=6Q0+959+856&__mk_zh_CN=%E4%BA%9A%E9%A9%AC%E9%80%8A%E7%BD%91%E7%AB%99&ref=nb_sb_noss";
				httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
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

		// Token: 0x06000010 RID: 16 RVA: 0x00003278 File Offset: 0x00001478
		public static string Base64Decode(Encoding encodeType, string result)
		{
			string result2 = string.Empty;
			byte[] bytes = Convert.FromBase64String(result);
			try
			{
				result2 = encodeType.GetString(bytes);
			}
			catch
			{
				result2 = result;
			}
			return result2;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000032BC File Offset: 0x000014BC
		private void QQbase64_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000032C0 File Offset: 0x000014C0
		private void button1_Click(object sender, EventArgs e)
		{
			string url = QQbase64.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");
			bool flag = url.Contains("QQbase64");
			if (flag)
			{
				Match match = Regex.Match(this.textBox1.Text, "accountId=([\\s\\S]*?)&");
				bool flag2 = match.Groups[1].Value != "";
				if (flag2)
				{
					string text = QQbase64.Base64Decode(Encoding.GetEncoding("utf-8"), match.Groups[1].Value);
					this.textBox2.Text = text;
					this.textBox1.Text = "";
				}
				else
				{
					this.textBox1.Text = "格式不正确";
				}
			}
			else
			{
				MessageBox.Show("验证失败");
			}
		}
	}
}
