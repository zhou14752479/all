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


		public static string[] ualist = { 
        "Mozilla/5.0 (Linux; U; Android 8.1.0; zh-cn; BLA-AL00 Build/HUAWEIBLA-AL00) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/8.9 Mobile Safari/537.36",
        "Mozilla/5.0 (Linux; Android 8.1; PAR-AL00 Build/HUAWEIPAR-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044304 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070333) NetType/WIFI Language/zh_CN Process/tools",
        "Mozilla/5.0 (Linux; Android 8.1.0; ALP-AL00 Build/HUAWEIALP-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/63.0.3239.83 Mobile Safari/537.36 T7/10.13 baiduboxapp/10.13.0.11 (Baidu; P1 8.1.0)",
        "Mozilla/5.0 (Linux; Android 8.1; EML-AL00 Build/HUAWEIEML-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/53.0.2785.143 Crosswalk/24.53.595.0 XWEB/358 MMWEBSDK/23 Mobile Safari/537.36 MicroMessenger/6.7.2.1340(0x2607023A) NetType/4G Language/zh_CN",
        "Mozilla/5.0 (Linux; U; Android 8.0.0; zh-CN; MHA-AL00 Build/HUAWEIMHA-AL00) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.108 UCBrowser/12.1.4.994 Mobile Safari/537.36",
        "Mozilla/5.0 (Linux; Android 8.0; MHA-AL00 Build/HUAWEIMHA-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044304 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070333) NetType/NON_NETWORK Language/zh_CN Process/tools",
        "Mozilla/5.0 (Linux; U; Android 8.0.0; zh-CN; MHA-AL00 Build/HUAWEIMHA-AL00) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/40.0.2214.89 UCBrowser/11.6.4.950 UWS/2.11.1.50 Mobile Safari/537.36 AliApp(DingTalk/4.5.8) com.alibaba.android.rimet/10380049 Channel/227200 language/zh-CN",
        "Mozilla/5.0 (Linux; U; Android 8.1.0; zh-CN; EML-AL00 Build/HUAWEIEML-AL00) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.108 UCBrowser/11.9.4.974 UWS/2.13.1.48 Mobile Safari/537.36 AliApp(DingTalk/4.5.11) com.alibaba.android.rimet/10487439 Channel/227200 language/zh-CN",
        "Mozilla/5.0 (Linux; U; Android 8.1.0; zh-CN; EML-TL00 Build/HUAWEIEML-TL00) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.108 UCBrowser/11.9.4.974 UWS/2.14.0.13 Mobile Safari/537.36 AliApp(TB/7.10.4) UCBS/2.11.1.1 TTID/227200@taobao_android_7.10.4 WindVane/8.3.0 1080X2244",
        "Mozilla/5.0 (Linux; U; Android 4.1.2; zh-cn; HUAWEI MT1-U06 Build/HuaweiMT1-U06) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30 baiduboxapp/042_2.7.3_diordna_8021_027/IEWAUH_61_2.1.4_60U-1TM+IEWAUH/7300001a/91E050E40679F078E51FD06CD5BF0A43%7C544176010472968/1",
        "Mozilla/5.0 (Linux; Android 8.0; MHA-AL00 Build/HUAWEIMHA-AL00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044304 Mobile Safari/537.36 MicroMessenger/6.7.3.1360(0x26070333) NetType/4G Language/zh_CN Process/tools",
        "Mozilla/5.0 (iPhone; CPU iPhone OS 12_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/16A366 MicroMessenger/6.7.3(0x16070321) NetType/WIFI Language/zh_CN",
        "Mozilla/5.0 (iPhone; CPU iPhone OS 12_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/16A366 MicroMessenger/6.7.3(0x16070321) NetType/WIFI Language/zh_HK",
        "Mozilla/5.0 (iPhone; CPU iPhone OS 11_2 like Mac OS X) AppleWebKit/604.3.5 (KHTML, like Gecko) Version/11.0 MQQBrowser/8.8.2 Mobile/15B87 Safari/604.1 MttCustomUA/2 QBWebViewType/1 WKType/1",
        "Mozilla/5.0 (iPhone 6s; CPU iPhone OS 11_4_1 like Mac OS X) AppleWebKit/604.3.5 (KHTML, like Gecko) Version/11.0 MQQBrowser/8.3.0 Mobile/15B87 Safari/604.1 MttCustomUA/2 QBWebViewType/1 WKType/1",
        "Mozilla/5.0 (iPhone; CPU iPhone OS 10_1 like Mac OS X) AppleWebKit/602.2.14 (KHTML, like Gecko) Version/10.0 MQQBrowser/8.8.2 Mobile/14B72c Safari/602.1 MttCustomUA/2 QBWebViewType/1 WKType/1",
        "Mozilla/5.0 (iPhone; CPU iPhone OS 11_0_2 like Mac OS X) AppleWebKit/604.1.38 (KHTML, like Gecko) Mobile/15A421 wxwork/2.5.8 MicroMessenger/6.3.22 Language/zh",
        "Mozilla/5.0 (iPhone; CPU iPhone OS 11_4_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15G77 wxwork/2.5.1 MicroMessenger/6.3.22 Language/zh",
        "Mozilla/5.0 (iPhone; CPU iPhone OS 10_1_1 like Mac OS X) AppleWebKit/602.2.14 (KHTML, like Gecko) Version/10.0 MQQBrowser/8.8.2 Mobile/14B100 Safari/602.1 MttCustomUA/2 QBWebViewType/1 WKType/1",
        "Mozilla/5.0 (Linux; Android 6.0.1; OPPO A57 Build/MMB29M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/48.0.2564.116 Mobile Safari/537.36 T7/9.1 baidubrowser/7.18.21.0 (Baidu; P1 6.0.1)",
        "Mozilla/5.0 (Linux; Android 6.0.1; OPPO A57 Build/MMB29M; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/63.0.3239.83 Mobile Safari/537.36 T7/10.13 baiduboxapp/10.13.0.10 (Baidu; P1 6.0.1)",
        "Mozilla/5.0 (Linux; U; Android 8.1.0; zh-CN; vivo Y85 Build/OPM1.171019.011) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.108 UCBrowser/11.9.6.976 Mobile Safari/537.36",
        "Mozilla/5.0 (Linux; Android 5.1.1; OPPO R9 Plustm A Build/LMY47V; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/63.0.3239.83 Mobile Safari/537.36 T7/10.12 baiduboxapp/10.12.0.12 (Baidu; P1 5.1.1)",
        "Mozilla/5.0 (Linux; Android 7.1.1; OPPO R11 Build/NMF26X; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/63.0.3239.83 Mobile Safari/537.36 T7/10.13 baiduboxapp/10.13.0.11 (Baidu; P1 7.1.1)",
        "Mozilla/5.0 (Linux; Android 5.1.1; vivo X6S A Build/LMY47V; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044207 Mobile Safari/537.36 MicroMessenger/6.7.3.1340(0x26070332) NetType/4G Language/zh_CN Process/tools",
        "Mozilla/5.0 (Linux; Android 8.1.0; PACM00 Build/O11019; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/63.0.3239.83 Mobile Safari/537.36 T7/10.13 baiduboxapp/10.13.0.11 (Baidu; P1 8.1.0)",
        "Mozilla/5.0 (Linux; Android 7.1.1; vivo X20A Build/NMF26X; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/6.2 TBS/044304 Mobile Safari/537.36 MicroMessenger/6.7.2.1340(0x2607023A) NetType/WIFI Language/zh_CN",
        "Mozilla/5.0 (Linux; Android 8.1.0; vivo Y71A Build/OPM1.171019.011; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/63.0.3239.83 Mobile Safari/537.36 T7/10.13 baiduboxapp/10.13.0.11 (Baidu; P1 8.1.0)",
        "Mozilla/5.0 (Linux; U; Android 8.0.0; zh-cn; Mi Note 2 Build/OPR1.170623.032) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/61.0.3163.128 Mobile Safari/537.36 XiaoMi/MiuiBrowser/10.1.1",
        "Mozilla/5.0 (Linux; U; Android 7.0; zh-cn; MI 5s Build/NRD90M) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/61.0.3163.128 Mobile Safari/537.36 XiaoMi/MiuiBrowser/10.2.2",
        "Mozilla/5.0 (Linux; Android 8.0.0; MI 6 Build/OPR1.170623.027; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/63.0.3239.83 Mobile Safari/537.36 T7/10.13 baiduboxapp/10.13.0.11 (Baidu; P1 8.0.0)",
        "Mozilla/5.0 (Linux; U; Android 8.0.0; zh-CN; MI 5 Build/OPR1.170623.032) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.108 UCBrowser/11.8.9.969 Mobile Safari/537.36",
        "Mozilla/5.0 (Linux; Android 8.0.0; MI 6 Build/OPR1.170623.027) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/62.0.3202.84 Mobile Safari/537.36 Maxthon/3235",
        "Mozilla/5.0 (Linux; U; Android 8.1.0; zh-cn; Mi Note 3 Build/OPM1.171019.019) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/61.0.3163.128 Mobile Safari/537.36 XiaoMi/MiuiBrowser/10.0.2",
        "Mozilla/5.0 (Linux; U; Android 5.1.1; zh-CN; SM-J3109 Build/LMY47X) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.108 UCBrowser/11.8.0.960 UWS/2.12.1.18 Mobile Safari/537.36 AliApp(TB/7.5.4) UCBS/2.11.1.1 WindVane/8.3.0 720X1280",
        "Mozilla/5.0 (Linux; Android 8.0.0; SM-G9650 Build/R16NW; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/63.0.3239.83 Mobile Safari/537.36 T7/10.13 baiduboxapp/10.13.0.11 (Baidu; P1 8.0.0)",
        "Mozilla/5.0 (Linux; Android 8.0.0; SM-N9500 Build/R16NW; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/63.0.3239.83 Mobile Safari/537.36 T7/10.13 baiduboxapp/10.13.0.11 (Baidu; P1 8.0.0)",
 };







		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static string PostUrl(string url, string postData, string COOKIE, string charset, string contenttype, string refer)
		{
			string result;
			try
			{

				Random rd=new Random();
				int x=rd.Next(ualist.Length-1);
				string useragent = ualist[x];	
				refer = "https://servicewechat.com/wxf1c2e0bbdgfggs3c/335/page-frame.html";
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.Method = "Post";
				httpWebRequest.ContentType = contenttype;
				httpWebRequest.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
				httpWebRequest.AllowAutoRedirect = false;
				httpWebRequest.KeepAlive = true;
				httpWebRequest.UserAgent = useragent;

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
