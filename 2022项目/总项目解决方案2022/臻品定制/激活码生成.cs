using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Common;

namespace Background
{
	// Token: 0x02000015 RID: 21
	public partial class 激活码生成 : Form
	{
		// Token: 0x060000C6 RID: 198 RVA: 0x0000E183 File Offset: 0x0000C383
		public 激活码生成()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000E19C File Offset: 0x0000C39C
		private void BtnActive_Click(object sender, EventArgs e)
		{
			Manager manager = new Manager();
			this.TxtActivecode.Text = manager.ComputeActivecode(this.TxtMachineID.Text) + "56 47";
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000E1D8 File Offset: 0x0000C3D8
		public static void TestForKillMyself()
		{
			string contents = "@echo off\r\n                           :tryagain\r\n                           del %1\r\n                           if exist %1 goto tryagain\r\n                           del %0";
			File.WriteAllText("killme.bat", contents);
			Process.Start(new ProcessStartInfo
			{
				FileName = "killme.bat",
				Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"",
				WindowStyle = ProcessWindowStyle.Hidden
			});
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000E238 File Offset: 0x0000C438
		public static string GetUrl(string Url, string charset)
		{
			string value = "";
			string result;
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
				httpWebRequest.Proxy = null;
				httpWebRequest.AllowAutoRedirect = true;
				httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
				httpWebRequest.Referer = Url;
				httpWebRequest.Headers.Add("Cookie", value);
				httpWebRequest.Headers.Add("Accept-Encoding", "gzip");
				httpWebRequest.KeepAlive = true;
				httpWebRequest.Accept = "*/*";
				httpWebRequest.Timeout = 5000;
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				bool flag = httpWebResponse.Headers["Content-Encoding"] == "gzip";
				string text;
				if (flag)
				{
					GZipStream stream = new GZipStream(httpWebResponse.GetResponseStream(), CompressionMode.Decompress);
					StreamReader streamReader = new StreamReader(stream, Encoding.GetEncoding(charset));
					text = streamReader.ReadToEnd();
					streamReader.Close();
				}
				else
				{
					StreamReader streamReader2 = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding(charset));
					text = streamReader2.ReadToEnd();
					streamReader2.Close();
				}
				httpWebResponse.Close();
				result = text;
			}
			catch (Exception ex)
			{
				result = ex.ToString();
			}
			return result;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0000E390 File Offset: 0x0000C590
		private void 激活码生成_Load(object sender, EventArgs e)
		{
			string url = 激活码生成.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8");
			bool flag = !url.Contains("NrBeP");
			if (flag)
			{
				激活码生成.TestForKillMyself();
				Process.GetCurrentProcess().Kill();
			}
			bool flag2 = DateTime.Now > Convert.ToDateTime("2023-08-01");
			if (flag2)
			{
				激活码生成.TestForKillMyself();
				Process.GetCurrentProcess().Kill();
			}
		}
	}
}
