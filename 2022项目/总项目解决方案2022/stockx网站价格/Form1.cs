using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using CsharpHttpHelper;
using Microsoft.Win32;
using myDLL;

namespace stockx网站价格
{
	// Token: 0x02000002 RID: 2
	public partial class Form1 : Form
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public Form1()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002074 File Offset: 0x00000274
		public static string gethtml(string url, string COOKIE)
		{
			HttpHelper httpHelper = new HttpHelper();
			HttpItem httpItem = new HttpItem
			{
				URL = url,
				Method = "GET",
				Timeout = 100000,
				ReadWriteTimeout = 30000,
				IsToLower = false,
				Cookie = COOKIE,
				UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36",
				Accept = "text/html, application/xhtml+xml, */*",
				ContentType = "text/html",
				Referer = "https://stockx.com/api/products/air-jordan-6-retro-dmp-2020?includes=market,360&currency=USD&country=HK",
				Allowautoredirect = true,
				AutoRedirectCookie = true,
				Postdata = "",
				ResultType = 0
			};
			HttpResult html = httpHelper.GetHtml(httpItem);
			string html2 = html.Html;
			string text = html.Cookie;
			return html2;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000213C File Offset: 0x0000033C
		public static string GetUrl(string Url, string charset)
		{
			string value = "";
			string result;
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
				httpWebRequest.AllowAutoRedirect = true;
				httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
				httpWebRequest.Referer = "https://finance.sina.com.cn/money/forex/hq/USDCNY.shtml";
				httpWebRequest.Headers.Add("Cookie", value);
				httpWebRequest.Headers.Add("Accept-Encoding", "gzip");
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				httpWebRequest.KeepAlive = true;
				httpWebRequest.Accept = "*/*";
				httpWebRequest.Timeout = 5000;
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

		// Token: 0x06000004 RID: 4 RVA: 0x00002290 File Offset: 0x00000490
		public string PostUrl(string url, string postData, string ua)
		{
			string result;
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.Method = "Post";
				httpWebRequest.ContentType = "application/json";
				httpWebRequest.ContentLength = (long)postData.Length;
				httpWebRequest.AllowAutoRedirect = false;
				httpWebRequest.KeepAlive = true;
				httpWebRequest.UserAgent = ua;
				httpWebRequest.Headers.Add("Cookie", this.cookie);
				WebHeaderCollection headers = httpWebRequest.Headers;
				headers.Add("x-algolia-api-key: 6b5e76b49705eb9f51a06d3c82f7acee");
				headers.Add("x-algolia-application-id: XW7SBCT9V6");
				//headers.Add("X-Stockx-Device-Id: ed25270a-c60a-42f0-99db-241176a6181e");
				//headers.Add("X-Stockx-Session-Id: 1cb5e82e-6481-42e1-9484-774af8133a57");
				httpWebRequest.Referer = "https://finance.sina.com.cn/money/forex/hq/USDCNY.shtml";
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

		// Token: 0x06000005 RID: 5 RVA: 0x000023CC File Offset: 0x000005CC
		private string Request_stockx_com(string sku, string price)
		{
			string result;
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://stockx.com/api/pricing?currency=USD&include_taxes=true");
				httpWebRequest.KeepAlive = true;
				httpWebRequest.Headers.Add("sec-ch-ua", "\" Not;A Brand\";v=\"99\", \"Google Chrome\";v=\"97\", \"Chromium\";v=\"97\"");
				httpWebRequest.Headers.Add("grails-user", "eyJDdXN0b21lciI6eyJCaWxsaW5nIjp7ImNhcmRUeXBlIjoiUGF5UGFsIiwidG9rZW4iOiJqZGd2NDZmIiwibGFzdDQiOm51bGwsImFjY291bnRFbWFpbCI6ImN5YmluM0Bob3RtYWlsLmNvbSIsImV4cGlyYXRpb25EYXRlIjpudWxsLCJjYXJkaG9sZGVyTmFtZSI6ImN5YmluIE1TSVBNIiwiQWRkcmVzcyI6eyJmaXJzdE5hbWUiOiJNU0lQTSIsImxhc3ROYW1lIjoiY3liaW4iLCJ0ZWxlcGhvbmUiOiI2MjY3NzQ3MDczIiwic3RyZWV0QWRkcmVzcyI6IjU3ODMgTkUgQ29sdW1iaWEgQmx2ZCIsImV4dGVuZGVkQWRkcmVzcyI6IiIsImxvY2FsaXR5IjoiUE9SVExBTkQiLCJyZWdpb24iOiJPUiIsInBvc3RhbENvZGUiOiI5NzIxOCIsImNvdW50cnlDb2RlQWxwaGEyIjoiVVMiLCJtYXJrZXQiOiJVUyJ9fSwiU2hpcHBpbmciOnsiQWRkcmVzcyI6eyJmaXJzdE5hbWUiOiJNU0lQTSIsImxhc3ROYW1lIjoiY3liaW4iLCJ0ZWxlcGhvbmUiOiI2MjY3NzQ3MDczIiwic3RyZWV0QWRkcmVzcyI6IjU3ODMgTkUgQ29sdW1iaWEgQmx2ZCIsImV4dGVuZGVkQWRkcmVzcyI6IiIsImxvY2FsaXR5IjoiUE9SVExBTkQiLCJyZWdpb24iOiJPUiIsInBvc3RhbENvZGUiOiI5NzIxOCIsImNvdW50cnlDb2RlQWxwaGEyIjoiVVMiLCJtYXJrZXQiOiJVUyJ9fSwidXVpZCI6IjI5MzJiMWNhLTA3ZjktMTFlNy1hYTc5LTEyY2RhMWM5YjZhNSIsImlkIjoiMjYxMjE1IiwiaGFzQnV5ZXJSZXdhcmQiOmZhbHNlfX0=");
				httpWebRequest.Headers.Add("sec-ch-ua-mobile", "?0");
				httpWebRequest.Headers.Set(HttpRequestHeader.Authorization, "Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Ik5USkNNVVEyUmpBd1JUQXdORFk0TURRelF6SkZRelV4TWpneU5qSTNNRFJGTkRZME0wSTNSQSJ9.eyJodHRwczovL3N0b2NreC5jb20vY3VzdG9tZXJfdXVpZCI6IjI5MzJiMWNhLTA3ZjktMTFlNy1hYTc5LTEyY2RhMWM5YjZhNSIsImh0dHBzOi8vc3RvY2t4LmNvbS9nYV9ldmVudCI6IkxvZ2dlZCBJbiIsImlzcyI6Imh0dHBzOi8vYWNjb3VudHMuc3RvY2t4LmNvbS8iLCJzdWIiOiJhdXRoMHwyOTMyYjFjYS0wN2Y5LTExZTctYWE3OS0xMmNkYTFjOWI2YTUiLCJhdWQiOiJnYXRld2F5LnN0b2NreC5jb20iLCJpYXQiOjE2NDczMzMyNjAsImV4cCI6MTY0NzM3NjQ2MCwiYXpwIjoiT1Z4cnQ0VkpxVHg3TElVS2Q2NjFXMER1Vk1wY0ZCeUQiLCJzY29wZSI6Im9mZmxpbmVfYWNjZXNzIn0.nBhswZ_wg7Xvc37r7WWJAB2hOF_zJGk2gbAJTc641QyIhalHQHn2xtF-WX0aBRL-0wPhct4Z5qqLjT14Tz2NbMiezaq_K8YaPfzBC5GnlpRiORSjidHyMpTwIyMdwoCCCpzxmk_9zaMRyunxnSBBM2sGZtIK-8x1tYZ9EKLqEzXWw25ffwH8NND9XMseZYvHllHvcu8dgssvhLDmevknA1hvAzw9EGGz7wqZWR4b-JBz49Rnrs6LQQBU8nn4BQ0MJtLVvXvJbVqTXmLGPust4iptjMYrou66sDek5x1VMOgfX1LsTU4AFS3KL7hsea66Be28rDPciBc-BFfeyzYDjg");
				httpWebRequest.ContentType = "application/json";
				httpWebRequest.Headers.Add("appVersion", "0.1");
				httpWebRequest.Headers.Add("appOS", "web");
				httpWebRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
				httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36";
				httpWebRequest.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
				httpWebRequest.Accept = "*/*";
				httpWebRequest.Headers.Add("Origin", "https://stockx.com");
				httpWebRequest.Headers.Add("Sec-Fetch-Site", "same-origin");
				httpWebRequest.Headers.Add("Sec-Fetch-Mode", "cors");
				httpWebRequest.Headers.Add("Sec-Fetch-Dest", "empty");
				httpWebRequest.Referer = "https://stockx.com/zh-cn/buy/nike-kobe-5-protro-chaos?size=8.5";
				httpWebRequest.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
				httpWebRequest.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh,sq;q=0.9,zh-CN;q=0.8,oc;q=0.7,de;q=0.6,en;q=0.5");
				httpWebRequest.Headers.Set(HttpRequestHeader.Cookie, "stockx_device_id=6cd4a569-03f7-49c4-8ac0-d02b66fb74f3; _pxvid=ad9e18ec-a351-11ec-94e4-4c7157426957; ab_one_buy_now_v2_button=true; _ga=undefined; __ssid=b3c80f66c64a6a013c3c86aca09f024; rskxRunCookie=0; rCookie=aya2akwqhzk1ykdorbv5xyl0q83tk6; ajs_anonymous_id=20e5872f-e94f-476c-b011-be6bf9d3650e; QuantumMetricUserID=ecef40ca4923138fc0879e389771d324; rbuid=rbcr-7e0a0df2-4cab-4cd2-af70-2264a6de30ac; _ga=GA1.2.1509626082.1647233290; ajs_user_id=2932b1ca-07f9-11e7-aa79-12cda1c9b6a5; _gcl_au=1.1.243599321.1647233293; _scid=3eaec287-8ce0-4995-bd29-b9057f842dfb; __pdst=f752cc7f90c746f3a1572824a18777e0; tracker_device=7229b4c0-60bd-4fcd-b6f6-622b6e9b5085; _rdt_uuid=1647235630500.ffe44bd4-a4f8-49ad-9e54-39b93caafe8f; stockx_homepage=sneakers; __cf_bm=jRvcp2slDeTzH1fCNpLV2F1dnKSbyZeQeuM95t5wJMs-1647333257-0-ARmvw0P0mVrmGSf96JIipViKJCmClyGMInUKWKyvbtjd5zcutCX+dKcqRqOw6IsjgfxFacgl6T3Fqbny2O/mUHw=; mfaLogin=err; token=eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Ik5USkNNVVEyUmpBd1JUQXdORFk0TURRelF6SkZRelV4TWpneU5qSTNNRFJGTkRZME0wSTNSQSJ9.eyJodHRwczovL3N0b2NreC5jb20vY3VzdG9tZXJfdXVpZCI6IjI5MzJiMWNhLTA3ZjktMTFlNy1hYTc5LTEyY2RhMWM5YjZhNSIsImh0dHBzOi8vc3RvY2t4LmNvbS9nYV9ldmVudCI6IkxvZ2dlZCBJbiIsImlzcyI6Imh0dHBzOi8vYWNjb3VudHMuc3RvY2t4LmNvbS8iLCJzdWIiOiJhdXRoMHwyOTMyYjFjYS0wN2Y5LTExZTctYWE3OS0xMmNkYTFjOWI2YTUiLCJhdWQiOiJnYXRld2F5LnN0b2NreC5jb20iLCJpYXQiOjE2NDczMzMyNjAsImV4cCI6MTY0NzM3NjQ2MCwiYXpwIjoiT1Z4cnQ0VkpxVHg3TElVS2Q2NjFXMER1Vk1wY0ZCeUQiLCJzY29wZSI6Im9mZmxpbmVfYWNjZXNzIn0.nBhswZ_wg7Xvc37r7WWJAB2hOF_zJGk2gbAJTc641QyIhalHQHn2xtF-WX0aBRL-0wPhct4Z5qqLjT14Tz2NbMiezaq_K8YaPfzBC5GnlpRiORSjidHyMpTwIyMdwoCCCpzxmk_9zaMRyunxnSBBM2sGZtIK-8x1tYZ9EKLqEzXWw25ffwH8NND9XMseZYvHllHvcu8dgssvhLDmevknA1hvAzw9EGGz7wqZWR4b-JBz49Rnrs6LQQBU8nn4BQ0MJtLVvXvJbVqTXmLGPust4iptjMYrou66sDek5x1VMOgfX1LsTU4AFS3KL7hsea66Be28rDPciBc-BFfeyzYDjg; language_code=zh; stockx_selected_currency=USD; stockx_product_visits=1; stockx_default_sneakers_size=8.5; stockx_preferred_market_activity=sales; stockx_session=%2249b7c1da-fbfc-46e0-9050-35c42659e480%22; loggedIn=2932b1ca-07f9-11e7-aa79-12cda1c9b6a5; pxcts=b8ccec5e-a43a-11ec-91cb-4343774f6556; riskified_recover_updated_verbiage=true; ops_banner_id=bltf0ff6f9ef26b6bdb; lastRskxRun=1647333270917; _gid=GA1.2.1569939799.1647333332; _clck=9nwi44|1|ezs|0; _clsk=1v6wyzr|1647333336987|1|1|b.clarity.ms/collect; forterToken=67c1ff70bd394a2b93c79d3cec6ea654_1647333338425__UDF43_13ck; _uetsid=e71eb060a43a11eca8b539f1cf3e853e; _uetvid=d76dd6b0a35111ecb8ce914c7e0239d5; IR_gbd=stockx.com; IR_9060=1647333343639%7C0%7C1647333343639%7C%7C; IR_PI=55db0870-b722-3bc1-b297-6d9c88226856%7C1647419743639; QuantumMetricSessionID=39a44a71f4d179b99577fa085807bff6; _px3=cc47dfbf708be1feb5eacf832bab40a5feb8a27c7499348e459bc9aafce895b7:jISHEoM3WNPxQzxHRq4lt9yBfvmWGlpaAce3N8pw8Pd5drNBDMmoqLJLZBiVg3nW7BJyN05RuofmC+wXndYD7w==:1000:phIp7GV+l1vxWhpVseKlGqLAvTksU1Da7NH2axCk2xrKKiK7GPZaPrYJ5fLinJp75Lqa/SpVfQCngr1Kf8Yz6WNBNqoMfh4AKJ8aCmBgfECnayOgVlJvRE8M8Umcnot0x+5NB2XKlh2tJ+H0kW5BFRq6r8m6I224P5MqqONHTmf1qi8/xoP2CLLfkj1T9L1zoM4tLcEpwMab3TwpRlZzNA==; _gat=1; _dd_s=rum=0&expire=1647334620219");
				httpWebRequest.Method = "POST";
				httpWebRequest.ServicePoint.Expect100Continue = false;
				string s = string.Concat(new string[]
				{
					"{\"context\":\"buying\",\"products\":[{\"sku\":\"",
					sku,
					"\",\"amount\":",
					price,
					",\"quantity\":1}],\"discountCodes\":[\"\"]}"
				});
				byte[] bytes = Encoding.UTF8.GetBytes(s);
				httpWebRequest.ContentLength = (long)bytes.Length;
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
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

		// Token: 0x06000006 RID: 6 RVA: 0x0000262C File Offset: 0x0000082C
		public string gettotalfee(string sku, string price)
		{
			Thread.Sleep(2000);
			string text = string.Concat(new string[]
			{
				"{\"context\":\"buying\",\"products\":[{\"sku\":\"",
				sku,
				"\",\"amount\":",
				price,
				",\"quantity\":1}],\"discountCodes\":[\"\"]}"
			});
			Random random = new Random();
			string text2 = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904." + random.Next(0, 9999).ToString() + " Safari/537.36";
			string text3 = this.Request_stockx_com(sku, price);
			MessageBox.Show(text3);
			return Regex.Match(text3, "\"totalUSD\":([\\s\\S]*?),").Groups[1].Value;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000026DC File Offset: 0x000008DC
		public void gethuilv()
		{
			string url = "https://webapi.huilv.cc/api/exchange?num=1&chiyouhuobi=USD&duihuanhuobi=CNY&type=1&callback=jisuanjieguo&_=1645687133740";
			string url2 = Form1.GetUrl(url, "utf-8");
			string value = Regex.Match(url2, "\"dangqianhuilv\":\"([\\s\\S]*?)\"").Groups[1].Value;
			this.textBox2.Text = value;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002728 File Offset: 0x00000928
		public void run()
		{
			try
			{
				string url = "https://xw7sbct9v6-2.algolianet.com/1/indexes/products/query?x-algolia-agent=Algolia%20for%20JavaScript%20(4.8.4)%3B%20Browser";
				string postData = "{\"params\":\"query=" + this.textBox1.Text.Trim() + "&facets=*&filters=\"}";
				string ua = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36";
				string input = this.PostUrl(url, postData, ua);
				
				Match match = Regex.Match(input, "\"url\":\"([\\s\\S]*?)\"");
				string url2 = "https://stockx.com/api/products/" + match.Groups[1].Value + "?includes=market,360&currency=USD&country=US";

				
				string input2 = Form1.gethtml(url2, this.cookie);
				
				Match match2 = Regex.Match(input2, "\"highestBid\":([\\s\\S]*?),");
				MatchCollection matchCollection = Regex.Matches(input2, "\"skuUuid\":\"([\\s\\S]*?)\"");
				MatchCollection matchCollection2 = Regex.Matches(input2, "\"lowestAskSize\":([\\s\\S]*?),");
				MatchCollection matchCollection3 = Regex.Matches(input2, "\"lowestAsk\":([\\s\\S]*?),");
				MatchCollection matchCollection4 = Regex.Matches(input2, "\"highestBidFloat\":([\\s\\S]*?)}");
				int i = 1;
				while (i < matchCollection3.Count)
				{
					bool flag = matchCollection2[i].Groups[1].Value != "null";
					if (flag)
					{
						double num = Convert.ToDouble(this.textBox2.Text) * Convert.ToDouble(matchCollection3[i].Groups[1].Value);
						double num2 = Convert.ToDouble(this.textBox2.Text) * Convert.ToDouble(matchCollection4[i].Groups[1].Value);
						double num3 = Convert.ToDouble(this.textBox2.Text) * 0.06 * Convert.ToDouble(matchCollection3[i].Groups[1].Value);
						double num4 = Convert.ToDouble(this.textBox2.Text) * 0.06 * Convert.ToDouble(matchCollection4[i].Groups[1].Value);
						double num5 = Convert.ToDouble(this.textBox2.Text) * 14.95;
						double num6 = num + num3 + num5;
						double num7 = num2 + num4 + num5;
						try
						{
							ListViewItem listViewItem = this.listView1.Items.Add("US " + matchCollection2[i].Groups[1].Value.Replace("null", "-"));
							listViewItem.SubItems.Add(num6.ToString());
							listViewItem.SubItems.Add(num7.ToString());
						}
						catch (Exception)
						{
							Thread.Sleep(2000);
							i--;
						}
					}
					
					i++;
					continue;
					
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			this.button1.Text = "点击获取";
			this.button1.Enabled = true;
		}

		public void run_new()
		{
			try
			{
				string url = "https://xw7sbct9v6-2.algolianet.com/1/indexes/products/query?x-algolia-agent=Algolia%20for%20JavaScript%20(4.8.4)%3B%20Browser";
				string postData = "{\"params\":\"query=" + this.textBox1.Text.Trim() + "&facets=*&filters=\"}";
				string ua = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36";
				string input = this.PostUrl(url, postData, ua);
				
				Match match = Regex.Match(input, "\"url\":\"([\\s\\S]*?)\"");
				//MessageBox.Show(match.Groups[1].Value);
				string html = Request_new(match.Groups[1].Value) ;
				textBox3.Text = html;
				//html =Regex.Match(html, @"variants([\s\S]*?)minimumBid").Groups[1].Value;

				
				MatchCollection highestBidSize = Regex.Matches(html, "\"highestBidSize\":([\\s\\S]*?),");


				MatchCollection aid = Regex.Matches(html, @"""id"":""([\s\S]*?)""");
				MatchCollection lowestAsk = Regex.Matches(html, @"""id"":""([\s\S]*?)lowestAsk"":([\s\S]*?),");
				MatchCollection highestBid = Regex.Matches(html, @"""id"":""([\s\S]*?)highestBid"":([\s\S]*?),");
			



				for (int i = 0; i < aid.Count; i++)
                {
					//if(highestBidSize[i].Groups[1].Value.Contains("null"))
					//{
					//	continue;
					//}

					//MessageBox.Show(highestBid[i].Groups[1].Value);
					double high = 0;
					double low = 0;

					string highaa = highestBid[i].Groups[2].Value.Replace("\"", "").Replace("amount", "").Replace("}", "").Replace(":", "").Replace("{", "");
					string lowaa = highestBid[i].Groups[2].Value.Replace("\"", "").Replace("amount", "").Replace("}", "").Replace(":", "").Replace("{", "");

					
					if (highaa != "null" && highaa != "")
                    {
						high =  Convert.ToDouble(highaa);
					}
					if(lowaa != "null" && lowaa != "")
                    {
						low = Convert.ToDouble(lowaa);
					}

					//high = Convert.ToDouble(this.textBox2.Text) * (high + high * 0.08 + 14.95);
					//low = Convert.ToDouble(this.textBox2.Text) * (low + low * 0.08 + 14.95);


					ListViewItem listViewItem = this.listView1.Items.Add("US " + aid[i].Groups[1].Value.Replace("\"", ""));
					listViewItem.SubItems.Add(low.ToString("F2"));
					listViewItem.SubItems.Add(high.ToString("F2"));
					
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			this.button1.Text = "点击获取";
			this.button1.Enabled = true; 
		}


		// Token: 0x06000009 RID: 9 RVA: 0x00002A3C File Offset: 0x00000C3C
		private void Form1_Load(object sender, EventArgs e)
		{
			
			method.SetFeatures(11000);
			//webBrowser1.ScriptErrorsSuppressed = true;
			
			this.gethuilv();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002A48 File Offset: 0x00000C48
		private void button1_Click(object sender, EventArgs e)
		{

			if (DateTime.Now > Convert.ToDateTime("2025-09-11"))
			{
				return;
			}
			bool flag = this.thread == null || !this.thread.IsAlive;
			if (flag)
			{
				this.button1.Text = "正在获取...";
				this.button1.Enabled = false;
				this.thread = new Thread(new ThreadStart(this.run_new));
				this.thread.Start();
				Control.CheckForIllegalCrossThreadCalls = false;
			}
		}

		
		private void Button2_Click(object sender, EventArgs e)
		{
			this.button1.Enabled = true;
			this.listView1.Items.Clear();
		}


		
		private string Request_new(string aid)
		{
			

			try
			{
				string charset = "utf-8";
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://stockx.com/api/p/e");

				request.KeepAlive = true;
				request.Headers.Add("sec-ch-ua", @"""Not/A)Brand"";v=""99"", ""Google Chrome"";v=""115"", ""Chromium"";v=""115""");
				request.Headers.Add("apollographql-client-name", @"Iron");
				request.Headers.Add("App-Version", @"2023.10.01.01");
				request.Headers.Add("x-operation-name", @"GetMarketData");
				request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh-CN");
				request.Headers.Add("sec-ch-ua-mobile", @"?0");
				request.Headers.Add("App-Platform", @"Iron");
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36";
				request.ContentType = "application/json";
				request.Accept = "*/*";
				request.Headers.Add("selected-country", @"US");
				request.Headers.Add("x-stockx-device-id", @"32b6dbed-3f70-4657-a8be-b272375d26bb");
				request.Headers.Add("apollographql-client-version", @"2025.03.16.00");
				request.Headers.Add("x-stockx-session-id", @"d6dae96c-64c4-498e-ada8-8b4112e15125");
				request.Headers.Add("sec-ch-ua-platform", @"""Windows""");
				request.Headers.Add("Origin", @"https://stockx.com");
				request.Headers.Add("Sec-Fetch-Site", @"same-origin");
				request.Headers.Add("Sec-Fetch-Mode", @"cors");
				request.Headers.Add("Sec-Fetch-Dest", @"empty");
				request.Referer = "https://stockx.com/zh-cn/nike-dunk-low-cacao-wow-womens";
				//request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
				request.Headers.Set(HttpRequestHeader.Cookie, @"stockx_device_id=32b6dbed-3f70-4657-a8be-b272375d26bb; stockx_session_id=d6dae96c-64c4-498e-ada8-8b4112e15125; stockx_session=7439c87d-6078-4de5-b13a-34dd2d0b6e9d; pxcts=2d49d892-08a4-11f0-9da4-7ea5f1a97390; _pxvid=2d49ccc8-08a4-11f0-9da2-e45ef3e445ee; chakra-ui-color-mode=light; rskxRunCookie=0; rCookie=6b9z8xqdgqbkr4jd9d12iam8mzrl31; language_code=zh; stockx_selected_locale=zh; stockx_selected_currency=USD; stockx_selected_region=CN; stockx_dismiss_modal=true; stockx_dismiss_modal_set=2025-03-24T11%3A36%3A41.189Z; stockx_dismiss_modal_expiration=2026-03-24T11%3A36%3A41.189Z; display_location_selector=false; cf_clearance=s5T9zJNyChDV3PZl8aUcT32aX2.Jew7bvTeOhjbpEgQ-1742816320-1.2.1.1-IDZXsBdvLiJMxQ7JzgC7GVGJDi2DTILLJUAQOpdefEDzQhPvW8iWNj9ZsTw6hHLcGL01caffD8ixagjngBAPYZCf3lci3kTvGFhpLixA9Qnu391D2iQbT25YEUKZpxueRgQ0.iR6UHZWxnB8XtkQDrZ_Gw2K2e94tmc1kH.8pDXHWis7bko69OgerXIiRJKc076E6i2Z1JaAH_RQ6tEWQ1SlXE9FnOmV0WWEKbrNzAssxntIcG.8zKVQAOnd.fH4Lm94YsaQ8IqPEXrbM3xRcPn_A8Tyr2BYgJv9cB3mWFd1c2uiMreD_2CUArDnJR2CFtVpmi90FYm7oMukHbZO9opobE86.V4cQfnqLi7fDviqjKFZwV40C5BEDLn6R02pspH8.Jby4lANRO.QOe4.I4DK.qMZYFJoBezoyKuQL4w; is_gdpr=false; stockx_ip_region=CN; _px3=11ee5c6eba171c0b6b07b9e1b34a18e0527fbd896e9f337702e88352c6251444:w8lCJOdwr9t5W/paMxTWem5cfk5RkJvXjLIAJsKhcQdrxTF4VswPjsg0fL5fSYiBau2ZlRTtZMLTrJGpVnxhZw==:1000:jmSRgCupoYyybr8PYoPVe59UddrEu8hnykykck9ujpnppeSQcIA7he6if9V1frUSfhDDkEk2RWvU9EfRJoDOD5XoV9bijd7ZeiGG9ez2IoSM/ZAdoj/EyBa2uPM7Lf2e2aPCz7uUoss1b+7h5V0UDdpx1mohyBqlHuNMbF5SuFYjhAceFyim36aIITWC0+Org8rHnLOBvfu+M0UI6ONRNimTQxW3xI/DeGwOhP7MXxw=; _pxde=e8f23c92726642ae39ecd1fe1d31bbcccfd0586feb158c8095a1f9909ab3178a:eyJ0aW1lc3RhbXAiOjE3NDI4MTYzNTEzMzEsImZfa2IiOjB9; _dd_s=rum=0&expire=1742817279174&logs=1&id=c3c400a0-5e5d-47fd-94a9-5d30446e3866&created=1742816193537; lastRskxRun=1742816379422");

				request.Method = "POST";
				request.ServicePoint.Expect100Continue = false;

				//string body = @"{""query"":""query GetMarketData($id: String!, $currencyCode: CurrencyCode, $countryCode: String!, $marketName: String) {\n  product(id: $id) {\n    id\n    urlKey\n    title\n    uuid\n    contentGroup\n    market(currencyCode: $currencyCode) {\n      bidAskData(country: $countryCode, market: $marketName) {\n        highestBid\n        highestBidSize\n        lowestAsk\n        lowestAskSize\n      }\n      salesInformation {\n        lastSale\n        salesLast72Hours\n      }\n    }\n    variants {\n      id\n      market(currencyCode: $currencyCode) {\n        bidAskData(country: $countryCode, market: $marketName) {\n          highestBid\n          highestBidSize\n          lowestAsk\n          lowestAskSize\n        }\n        salesInformation {\n          lastSale\n          salesLast72Hours\n        }\n      }\n    }\n    ...BidButtonFragment\n    ...BidButtonContentFragment\n    ...BuySellFragment\n    ...BuySellContentFragment\n    ...XpressAskPDPFragment\n    ...LastSaleFragment\n  }\n}\n\nfragment BidButtonFragment on Product {\n  id\n  title\n  urlKey\n  sizeDescriptor\n  productCategory\n  market(currencyCode: $currencyCode) {\n    bidAskData(country: $countryCode, market: $marketName) {\n      highestBid\n      highestBidSize\n      lowestAsk\n      lowestAskSize\n    }\n  }\n  media {\n    imageUrl\n  }\n  variants {\n    id\n    market(currencyCode: $currencyCode) {\n      bidAskData(country: $countryCode, market: $marketName) {\n        highestBid\n        highestBidSize\n        lowestAsk\n        lowestAskSize\n      }\n    }\n  }\n}\n\nfragment BidButtonContentFragment on Product {\n  id\n  urlKey\n  sizeDescriptor\n  productCategory\n  lockBuying\n  lockSelling\n  minimumBid(currencyCode: $currencyCode)\n  market(currencyCode: $currencyCode) {\n    bidAskData(country: $countryCode, market: $marketName) {\n      highestBid\n      highestBidSize\n      lowestAsk\n      lowestAskSize\n      numberOfAsks\n    }\n  }\n  variants {\n    id\n    market(currencyCode: $currencyCode) {\n      bidAskData(country: $countryCode, market: $marketName) {\n        highestBid\n        highestBidSize\n        lowestAsk\n        lowestAskSize\n        numberOfAsks\n      }\n    }\n  }\n}\n\nfragment BuySellFragment on Product {\n  id\n  title\n  urlKey\n  sizeDescriptor\n  productCategory\n  lockBuying\n  lockSelling\n  market(currencyCode: $currencyCode) {\n    bidAskData(country: $countryCode, market: $marketName) {\n      highestBid\n      highestBidSize\n      lowestAsk\n      lowestAskSize\n    }\n  }\n  media {\n    imageUrl\n  }\n  variants {\n    id\n    market(currencyCode: $currencyCode) {\n      bidAskData(country: $countryCode, market: $marketName) {\n        highestBid\n        highestBidSize\n        lowestAsk\n        lowestAskSize\n      }\n    }\n  }\n}\n\nfragment BuySellContentFragment on Product {\n  id\n  urlKey\n  sizeDescriptor\n  productCategory\n  lockBuying\n  lockSelling\n  market(currencyCode: $currencyCode) {\n    bidAskData(country: $countryCode, market: $marketName) {\n      highestBid\n      highestBidSize\n      lowestAsk\n      lowestAskSize\n    }\n  }\n  variants {\n    id\n    market(currencyCode: $currencyCode) {\n      bidAskData(country: $countryCode, market: $marketName) {\n        highestBid\n        highestBidSize\n        lowestAsk\n        lowestAskSize\n      }\n    }\n  }\n}\n\nfragment XpressAskPDPFragment on Product {\n  market(currencyCode: $currencyCode) {\n    state(country: $countryCode) {\n      numberOfCustodialAsks\n    }\n  }\n  variants {\n    market(currencyCode: $currencyCode) {\n      state(country: $countryCode) {\n        numberOfCustodialAsks\n      }\n    }\n  }\n}\n\nfragment LastSaleFragment on Product {\n  id\n  market(currencyCode: $currencyCode) {\n    statistics(market: $marketName) {\n      ...LastSaleMarketStatistics\n    }\n  }\n  variants {\n    id\n    market(currencyCode: $currencyCode) {\n      statistics(market: $marketName) {\n        ...LastSaleMarketStatistics\n      }\n    }\n  }\n}\n\nfragment LastSaleMarketStatistics on MarketStatistics {\n  lastSale {\n    amount\n    changePercentage\n    changeValue\n    sameFees\n  }\n}"",""variables"":{""id"":"""+aid+"\",\"currencyCode\":\"USD\",\"countryCode\":\"US\",\"marketName\":null},\"operationName\":\"GetMarketData\"}";
				string body = @"{""query"":""query GetMarketData($id: String!, $currencyCode: CurrencyCode, $countryCode: String!, $marketName: String, $viewerContext: MarketViewerContext) {\n product(id: $id) {\n id\n urlKey\n listingType\n title\n uuid\n contentGroup\n sizeDescriptor\n productCategory\n lockBuying\n lockSelling\n media {\n imageUrl\n    }\n minimumBid(currencyCode: $currencyCode)\n market(currencyCode: $currencyCode) {\n state(country: $countryCode, market: $marketName) {\n lowestAsk {\n amount\n chainId\n        }\n highestBid {\n amount\n        }\n askServiceLevels {\n expressExpedited {\n count\n lowest {\n amount\n chainId\n            }\n delivery {\n expectedDeliveryDate\n latestDeliveryDate\n            }\n          }\n expressStandard {\n count\n lowest {\n amount\n            }\n delivery {\n expectedDeliveryDate\n latestDeliveryDate\n            }\n          }\n standard {\n count\n lowest {\n amount\n chainId\n            }\n          }\n        }\n numberOfAsks\n numberOfBids\n      }\n salesInformation {\n lastSale\n salesLast72Hours\n      }\n statistics(market: $marketName, viewerContext: $viewerContext) {\n lastSale {\n amount\n changePercentage\n changeValue\n sameFees\n        }\n      }\n    }\n variants {\n id\n market(currencyCode: $currencyCode) {\n state(country: $countryCode, market: $marketName) {\n lowestAsk {\n amount\n chainId\n          }\n highestBid {\n amount\n          }\n askServiceLevels {\n expressExpedited {\n count\n lowest {\n amount\n chainId\n              }\n delivery {\n expectedDeliveryDate\n latestDeliveryDate\n              }\n            }\n expressStandard {\n count\n lowest {\n amount\n chainId\n              }\n delivery {\n expectedDeliveryDate\n latestDeliveryDate\n              }\n            }\n standard {\n count\n lowest {\n amount\n chainId\n              }\n            }\n          }\n numberOfAsks\n numberOfBids\n        }\n salesInformation {\n lastSale\n salesLast72Hours\n        }\n statistics(market: $marketName, viewerContext: $viewerContext) {\n lastSale {\n amount\n changePercentage\n changeValue\n sameFees\n          }\n        }\n      }\n    }\n  }\n}"",""variables"":{""id"":""" + aid + "\",\"currencyCode\":\"USD\",\"countryCode\":\"US\",\"marketName\":\"US\",\"viewerContext\":\"BUYER\"},\"operationName\":\"GetMarketData\"}";
				
				
				byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
				request.ContentLength = postBytes.Length;
				Stream stream = request.GetRequestStream();
				stream.Write(postBytes, 0, postBytes.Length);
				stream.Close();


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

				//textBox3.Text= html;
				return html;

			}
			catch (WebException e)
			{
				//textBox3.Text = e.ToString();
				return "";
			}
			catch (Exception e)
			{
				//textBox3.Text = e.ToString();
				return "";
			}

			
		}

		private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.textBox2.Text = "";
			this.gethuilv();
		}

		// Token: 0x04000001 RID: 1
		private string cookie = "stockx_device_id=ed25270a-c60a-42f0-99db-241176a6181e; language_code=zh; __pxvid=e2088da8-958c-11ed-aa1a-0242ac120002; ajs_anonymous_id=4bbc3852-972c-4c84-9e8f-7ee67c19d434; tracker_device=75325615-5e5c-4faa-ae4d-867554ea118e; __ssid=615cf9804e32318265fe9dc2cdabe06; rskxRunCookie=0; rCookie=g0gyrk9rojkslo1m74m8hnlcyp2zu9; RoktRecogniser=a2d4579a-7b37-4efd-9bf3-563ba5514e3b; stockx_homepage=sneakers; _pxvid=e1e4c9e5-958c-11ed-8799-d0acddfc711b; _ga=GA1.1.1207394361.1692097924; _gcl_au=1.1.474060180.1692097924; OptanonAlertBoxClosed=2023-08-15T11:12:04.451Z; _ga_TYYSNQDG4W=GS1.1.1692097923.1.1.1692097927.0.0.0; _ga=GA1.1.1207394361.1692097924; OptanonConsent=isGpcEnabled=0&datestamp=Tue+Aug+15+2023+19%3A12%3A08+GMT%2B0800+(%E4%B8%AD%E5%9B%BD%E6%A0%87%E5%87%86%E6%97%B6%E9%97%B4)&version=202211.2.0&isIABGlobal=false&hosts=&consentId=816d683c-ae1c-4ce4-bae3-7e5d3ad86859&interactionCount=1&landingPath=NotLandingPage&groups=C0001%3A1%2CC0002%3A1%2CC0004%3A1%2CC0005%3A1%2CC0003%3A1&geolocation=CN%3BGD&AwaitingReconsent=false; _uetvid=903519503b5c11ee9ab8295f6a39bb6d; stockx_session_id=af245aac-387a-4628-a68d-4e66319fe61e; stockx_session=bbe5fd7e-dd7f-46c9-89bc-d1bd92884670; __cf_bm=ECQxrBlFbaN1V3BUjC5PiV7BTy3AMr5zssF9HXLU_uw-1696396413-0-AbBamSRep++iOJUkJfJat5FNVehIUfXfXsTrDCmg3wu4ONi2r4BpKJSflbSySARH0kQG/4CADc9ZJ22nmpiOTrY=; stockx_selected_region=CN; pxcts=c578d0da-6274-11ee-88da-a92b4397520b; display_location_selector=false; cf_clearance=ndDgeHWcM1bMBxi1LTbQ5xDrkM132VkJm1iynBAzBXc-1696396418-0-1-1005a0c1.f77d388d.14d174ad-0.2.1696396418; ftr_blst_1h=1696396421874; lastRskxRun=1696396459467; forterToken=8b0950916dcd4429aec3d29023454fc6_1696396459438__UDF43-m4_13ck; _pxde=3ad19381353981242be3eec81fb331757005ef1b0eb3484d24d7cff855b4bff3:eyJ0aW1lc3RhbXAiOjE2OTYzOTY0NTk2NzQsImZfa2IiOjB9; _px3=95b3784fbabd4a6e76c5f264d6432d12343bd1524b0db47189459b9b747e1534:cN/iy6rQgWwV0icxBBmx+jN9O0QkOwcV6+yvmBm0ey/x6e593ONCY0cTJXw/LFot5zXO1AcItocGEEg0mDS6ug==:1000:6csGFOJ3KPg40kvlJx6b2RGqJnmSyC7Gs//MoUOvdv7uTHsztt3//hnOJGEVttyAShYHUOfGM1Ifu3/Pp+U4ev12ua4r4NVnLk+xu717/WbUxmRA6tulJ0/3hPeaW0y4J7GfOr3TJnJA/459xEoXiEAGVDLCQOJcZ5j0Bkb/ZsY=; _dd_s=rum=0&expire=1696397364588&logs=1&id=57ecaf16-d983-4dbb-b5b8-608241e8a314&created=1696396419058";
		// Token: 0x04000002 RID: 2
		private Thread thread;

        private void button3_Click(object sender, EventArgs e)
        {

			webBrowser1.Navigate("https://stockx.com/zh-cn/nike-kobe-5-protro-chaos");
        }

        private void button4_Click(object sender, EventArgs e)
        {
			cookie = method.GetCookies("https://stockx.com/zh-cn/adidas-yeezy-slide-black-onyx");
			MessageBox.Show(cookie);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
			DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
			if (dr == DialogResult.OK)
			{
				// Environment.Exit(0);
				System.Diagnostics.Process.GetCurrentProcess().Kill();
			}
			else
			{
				e.Cancel = true;//点取消的代码 
			}
		}
    }
}
