using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using myDLL;

namespace 校友邦
{
	// Token: 0x02000002 RID: 2
	internal class function
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static string PostUrl(string url, string postData, string COOKIE, string charset, string contenttype, string refer)
		{
			string result;
			try
			{
				refer = "https://servicewechat.com/wxf1c2e0bbdgfggs3c/335/page-frame.html";
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.Method = "Post";
				httpWebRequest.ContentType = contenttype;
				httpWebRequest.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
				httpWebRequest.AllowAutoRedirect = false;
				httpWebRequest.KeepAlive = true;
				httpWebRequest.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 15_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/8.0.18(0x18001230) NetType/WIFI Language/zh_CN";
				httpWebRequest.Headers.Add("Cookie", COOKIE);
				httpWebRequest.Referer = refer;
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

		// Token: 0x06000002 RID: 2 RVA: 0x00002160 File Offset: 0x00000360
		public string login(string username, string password)
		{
			password = method.GetMD5(password);
			string url = "https://xcx.xybsyw.com/login/login.action";
			string postData = string.Concat(new string[]
			{
				"username=",
				username,
				"&password=",
				password,
				"&openId=ooru94lH0MDBlYKT4dUwpEkRyAWQ&deviceId="
			});
			string input = function.PostUrl(url, postData, "", "utf-8", "application/x-www-form-urlencoded", "");
			string value = Regex.Match(input, "\"sessionId\":\"([\\s\\S]*?)\"").Groups[1].Value;
			bool flag = value != "";
			string result;
			if (flag)
			{
				result = "JSESSIONID=" + value + ";";
			}
			else
			{
				result = "";
			}
			return result;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002214 File Offset: 0x00000414
		public string getplanid(string cookie)
		{
			string url = "https://xcx.xybsyw.com/student/progress/ProjectList.action";
			string postData = "";
			string input = function.PostUrl(url, postData, cookie, "utf-8", "application/x-www-form-urlencoded", "");
			return Regex.Match(input, "\"planId\":([\\s\\S]*?),").Groups[1].Value;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000226C File Offset: 0x0000046C
		public string gettraineeId(string planid, string cookie)
		{
			string url = "https://xcx.xybsyw.com/student/clock/GetPlan!getDefault.action";
			string postData = "";
			string input = function.PostUrl(url, postData, cookie, "utf-8", "application/x-www-form-urlencoded", "");
			return Regex.Match(input, "\"traineeId\":([\\s\\S]*?)}").Groups[1].Value;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000022C4 File Offset: 0x000004C4
		public string getadcode(string lng, string lat)
		{
			string url = string.Concat(new string[]
			{
				"https://restapi.amap.com/v3/geocode/regeo?key=c222383ff12d31b556c3ad6145bb95f4&location=",
				lng,
				"%2C",
				lat,
				"&extensions=all&s=rsx&platform=WXJS&appname=c222383ff12d31b556c3ad6145bb95f4&sdkversion=1.2.0&logversion=2.0"
			});
			string url2 = method.GetUrl(url, "utf-8");
			return Regex.Match(url2, "\"adcode\":\"([\\s\\S]*?)\"").Groups[1].Value.Trim();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002330 File Offset: 0x00000530
		public string getmyma(string cookie, string traineeId)
		{
			string result = "https://ss0.xybsyw.com/temp/20220506/school/14563/epidemicsituation/4547945/1651818859152.jpg";
			string url = "https://xcx.xybsyw.com/student/clock/PunchIn!historyList.action";
			string postData = "traineeId=" + traineeId + "&months=2022-07";
			string input = function.PostUrl(url, postData, cookie, "utf-8", "application/x-www-form-urlencoded", "");
			MatchCollection matchCollection = Regex.Matches(input, "\"healthCodeImg\":\"([\\s\\S]*?)\"");
			for (int i = 0; i < matchCollection.Count; i++)
			{
				bool flag = Path.GetFileNameWithoutExtension(matchCollection[i].Groups[1].Value) != "1651818859152" && Path.GetFileNameWithoutExtension(matchCollection[i].Groups[1].Value) != "1646013660242";
				if (flag)
				{
					return matchCollection[i].Groups[1].Value;
				}
			}
			return result;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002428 File Offset: 0x00000628
		public string shangchuanma(string cookie, string myma)
		{
			string url = "https://xcx.xybsyw.com/student/clock/saveEpidemicSituation.action";
			string postData = "healthCodeStatus=0&locationRiskLevel=0&healthCodeImg=" + myma;
			string input = function.PostUrl(url, postData, cookie, "utf-8", "application/x-www-form-urlencoded", "");
			return Regex.Match(input, "\"msg\":\"([\\s\\S]*?)\"").Groups[1].Value;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002484 File Offset: 0x00000684
		public string shangchuanmaandtravelCodeImg(string cookie, string myma)
		{
			string url = "https://xcx.xybsyw.com/student/clock/saveEpidemicSituation.action";
			string postData = "healthCodeStatus=0&locationRiskLevel=0&healthCodeImg=" + myma + "&travelCodeImg=https%3A%2F%2Fss0.xybsyw.com%2Ftemp%2F20220602%2Fschool%2F13777%2Fepidemicsituation%2F4892071%2F1654135097325.jpg";
			string input = function.PostUrl(url, postData, cookie, "utf-8", "application/x-www-form-urlencoded", "");
			return Regex.Match(input, "\"msg\":\"([\\s\\S]*?)\"").Groups[1].Value;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000024E4 File Offset: 0x000006E4
		public string qiandao(string cookie, string addr, string traineeId)
		{
			string url = "https://xcx.xybsyw.com/student/clock/GetPlan!detail.action";
			string postData = "traineeId=" + traineeId;
			string input = function.PostUrl(url, postData, cookie, "utf-8", "application/x-www-form-urlencoded", "");
			string text = Regex.Match(input, "\"address\":\"([\\s\\S]*?)\"").Groups[1].Value.Trim();
			string text2 = Regex.Match(input, "\"lat\":([\\s\\S]*?),").Groups[1].Value.Trim();
			string text3 = Regex.Match(input, "\"lng\":([\\s\\S]*?),").Groups[1].Value.Trim();
			bool flag = text.Trim() == "";
			if (flag)
			{
				text = HttpUtility.UrlEncode(addr);
				string url2 = "https://api.map.baidu.com/geocoding/v3/?address=" + text + "&output=json&ak=9DemeyQjUrIX14Fz8uEwVpGyKErUP4Sb&callback=showLocation";
				string url3 = method.GetUrl(url2, "utf-8");
				text2 = Regex.Match(url3, "lat\":([\\s\\S]*?)}").Groups[1].Value.Trim();
				text3 = Regex.Match(url3, "lng\":([\\s\\S]*?),").Groups[1].Value.Trim();
			}
			string text4 = this.getadcode(text3, text2);
			string url4 = "https://xcx.xybsyw.com/student/clock/Post.action";
			string postData2 = string.Concat(new string[]
			{
				"traineeId=",
				traineeId,
				"&adcode=",
				text4,
				"&lat=",
				text2,
				"&lng=",
				text3,
				"&address=",
				text,
				"&deviceName=microsoft&punchInStatus=1&clockStatus=",
				this.status
			});
			string input2 = function.PostUrl(url4, postData2, cookie, "utf-8", "application/x-www-form-urlencoded", "");
			MatchCollection matchCollection = Regex.Matches(input2, "\"msg\":\"([\\s\\S]*?)\"");
			string value = matchCollection[matchCollection.Count - 1].Groups[1].Value;
			bool flag2 = value != "";
			string result;
			if (flag2)
			{
				result = value;
			}
			else
			{
				result = "failed";
			}
			return result;
		}

		// Token: 0x04000001 RID: 1
		public string status = "2";
	}
}
