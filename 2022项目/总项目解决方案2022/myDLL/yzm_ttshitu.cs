using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace myDLL
{
	// Token: 0x02000003 RID: 3
	public class yzm_ttshitu
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00004540 File Offset: 0x00002740
		public static string PostUrl(string url, string postData)
		{
			string result;
			try
			{
				string charset = "utf-8";
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = "Post";
				WebHeaderCollection headers = request.Headers;
				request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
				request.ContentLength = (long)postData.Length;
				request.AllowAutoRedirect = false;
				request.Headers.Add("Cookie", yzm_ttshitu.cookie);
				request.KeepAlive = true;
				request.Accept = "application/json, text/javascript, */*; q=0.01";
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
				request.Referer = "https://login.hnzwfw.gov.cn/tacs-uc/naturalMan/register";
				StreamWriter sw = new StreamWriter(request.GetRequestStream());
				sw.Write(postData);
				sw.Flush();
				HttpWebResponse response = request.GetResponse() as HttpWebResponse;
				bool flag = response.Headers["Content-Encoding"] == "gzip";
				string html;
				if (flag)
				{
					GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
					StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
					html = reader.ReadToEnd();
					reader.Close();
				}
				else
				{
					StreamReader reader2 = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
					html = reader2.ReadToEnd();
					reader2.Close();
				}
				response.Close();
				result = html;
			}
			catch (WebException ex)
			{
				result = ex.Message;
			}
			return result;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000046C8 File Offset: 0x000028C8
		public static Bitmap UrlToBitmap(string url)
		{
			byte[] Bytes = new WebClient
			{
				Headers = 
				{
					{
						"Cookie",
						yzm_ttshitu.cookie
					}
				}
			}.DownloadData(url);
			Bitmap result;
			using (MemoryStream ms = new MemoryStream(Bytes))
			{
				Image outputImg = Image.FromStream(ms);
				Bitmap map = new Bitmap(outputImg);
				result = map;
			}
			return result;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00004734 File Offset: 0x00002934
		public static string ImgToBase64String(Bitmap bmp)
		{
			string result;
			try
			{
				MemoryStream ms = new MemoryStream();
				bmp.Save(ms, ImageFormat.Jpeg);
				byte[] arr = new byte[ms.Length];
				ms.Position = 0L;
				ms.Read(arr, 0, (int)ms.Length);
				ms.Close();
				result = Convert.ToBase64String(arr);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000047A4 File Offset: 0x000029A4
		public static string shibie(string yzmusername, string yzmpassword, string yzmurl)
		{
			string result2;
			try
			{
				Bitmap image = yzm_ttshitu.UrlToBitmap(yzmurl);
				string param = string.Concat(new string[]
				{
					"{\"username\":\"",
					yzmusername,
					"\",\"password\":\"",
					yzmpassword,
					"\",\"image\":\"",
					yzm_ttshitu.ImgToBase64String(image),
					"\"}"
				});
				string PostResult = yzm_ttshitu.PostUrl("http://api.ttshitu.com/base64", param);
				Match result = Regex.Match(PostResult, "result\":\"([\\s\\S]*?)\"");
				bool flag = result.Groups[1].Value != "";
				if (flag)
				{
					result2 = result.Groups[1].Value;
				}
				else
				{
					result2 = PostResult;
				}
			}
			catch (Exception ex)
			{
				result2 = ex.ToString();
			}
			return result2;
		}

		// Token: 0x04000001 RID: 1
		public static string cookie = "";
	}
}
