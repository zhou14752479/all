using System;
using System.IO;
using System.Net;
using System.Text;

namespace 校友邦
{
	// Token: 0x02000003 RID: 3
	internal class jiamipost
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002700 File Offset: 0x00000900
		public static string PostUrl(string url, string postData, string COOKIE, string m, string t, string s)
		{
			string result;
			try
			{
				string referer = "https://servicewechat.com/wxf1c2e0bbdgfggs3c/335/page-frame.html";
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.Method = "Post";
				WebHeaderCollection headers = httpWebRequest.Headers;
				headers.Add("m:" + m);
				headers.Add("s:" + s);
				headers.Add("t:" + t);
				headers.Add("n:content,deviceName,keyWord,blogBody,blogTitle,getType,responsibilities,street,text,reason,searchvalue,key,answers,leaveReason,personRemark,selfAppraisal,imgUrl,wxname,deviceId,avatarTempPath,file,file,model,brand,system,deviceId,platform");
				headers.Add("v:1.7.14");
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				httpWebRequest.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
				httpWebRequest.AllowAutoRedirect = false;
				httpWebRequest.KeepAlive = true;
				httpWebRequest.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 15_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/8.0.18(0x18001230) NetType/WIFI Language/zh_CN";
				httpWebRequest.Headers.Add("Cookie", COOKIE);
				httpWebRequest.Referer = referer;
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
	}
}
