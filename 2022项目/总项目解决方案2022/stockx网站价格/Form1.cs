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
					IL_282:
					i++;
					continue;
					goto IL_282;
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
			webBrowser1.ScriptErrorsSuppressed = true;
			
			this.gethuilv();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002A48 File Offset: 0x00000C48
		private void button1_Click(object sender, EventArgs e)
		{
			bool flag = this.thread == null || !this.thread.IsAlive;
			if (flag)
			{
				this.button1.Text = "正在获取...";
				this.button1.Enabled = false;
				this.thread = new Thread(new ThreadStart(this.run));
				this.thread.Start();
				Control.CheckForIllegalCrossThreadCalls = false;
			}
		}

		
		private void Button2_Click(object sender, EventArgs e)
		{
			this.button1.Enabled = true;
			this.listView1.Items.Clear();
		}

		
		private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.textBox2.Text = "";
			this.gethuilv();
		}

		// Token: 0x04000001 RID: 1
		private string cookie = "stockx_device_id=ed25270a-c60a-42f0-99db-241176a6181e; language_code=zh; __pxvid=e2088da8-958c-11ed-aa1a-0242ac120002; ajs_anonymous_id=4bbc3852-972c-4c84-9e8f-7ee67c19d434; tracker_device=75325615-5e5c-4faa-ae4d-867554ea118e; __ssid=615cf9804e32318265fe9dc2cdabe06; rskxRunCookie=0; rCookie=g0gyrk9rojkslo1m74m8hnlcyp2zu9; RoktRecogniser=a2d4579a-7b37-4efd-9bf3-563ba5514e3b; stockx_homepage=sneakers; stockx_session_id=1cb5e82e-6481-42e1-9484-774af8133a57; stockx_session=aa5b106e-b2d9-496d-a45e-83611b4bdad6; __cf_bm=CzGBZ3EGIAr5oMZG44aNHk825xxSGC_w501c3Xz6KF0-1692097914-0-AfG+FbV3HDIhfi7ZWzLXiw3ayu1bZR/LyiQPs061Yk0p1okI8q7kaWxoC3yVkRdK5U3sajgpiaICyGWdox44QGA=; stockx_selected_region=CN; pxcts=8be0427f-3b5c-11ee-8ac1-69706c517445; _pxvid=e1e4c9e5-958c-11ed-8799-d0acddfc711b; _px3=0001c5a19616c3fcf7c3a6b83b2d9ff7b8adaf719452ee600f45c0fa837b9fa8:qA+WRI3WE9Wji2nfgIMOgFusaeIyyjbG7nIPMkByqyJa1vH+4r5K77glHkNGuayg8MOaFMKbcVUFgmRlpxUE7w==:1000:mGdo01aC4AqSe1SfP/ypRSIu8mwJsaYzPGLEkq1bRgEqVoDapFD3z+MHWDAL6z2CrBNcCIR/XoP0hSaKN5n6bnCbTYnDAd01C0ox2MK3aKUtH1bLKPa51CQuBKLLQVO4edc609wQ1LSeoNbJb4RGMEfSjJNJ8HpUnWrQujJJ0CvQ1Cl+/Wohl1CC7CmTTnONE6rhm98yDE6gzaimKaw/7w==; ftr_blst_1h=1692097922613; stockx_preferred_market_activity=sales; stockx_product_visits=1; lastRskxRun=1692097923350; _ga=GA1.1.1207394361.1692097924; _ga_TYYSNQDG4W=GS1.1.1692097923.1.1.1692097923.0.0.0; _gcl_au=1.1.474060180.1692097924; _uetsid=9034f5803b5c11eeb8848fcb39d9e925; _uetvid=903519503b5c11ee9ab8295f6a39bb6d; OptanonAlertBoxClosed=2023-08-15T11:12:04.451Z; OptanonConsent=isGpcEnabled=0&datestamp=Tue+Aug+15+2023+19%3A12%3A04+GMT%2B0800+(%E4%B8%AD%E5%9B%BD%E6%A0%87%E5%87%86%E6%97%B6%E9%97%B4)&version=202211.2.0&isIABGlobal=false&hosts=&consentId=816d683c-ae1c-4ce4-bae3-7e5d3ad86859&interactionCount=1&landingPath=NotLandingPage&groups=C0001%3A1%2CC0002%3A1%2CC0004%3A1%2CC0005%3A1%2CC0003%3A1; _pxde=f617e3a09f4d45f108bd63ad302aa0b5252352bc9ec16dcba26b70ad57a79ed2:eyJ0aW1lc3RhbXAiOjE2OTIwOTc5MjIwNTgsImZfa2IiOjB9; forterToken=8b0950916dcd4429aec3d29023454fc6_1692097921384__UDF43-m4_13ck; rbuid=rbos-d4fafa69-7e27-46f2-9f71-fb2e4b6b1ad5; _dd_s=rum=0&expire=1692098825485";
		// Token: 0x04000002 RID: 2
		private Thread thread;

        private void button3_Click(object sender, EventArgs e)
        {

			webBrowser1.Navigate("https://stockx.com/zh-cn/adidas-yeezy-slide-black-onyx");
        }

        private void button4_Click(object sender, EventArgs e)
        {
			cookie = method.GetCookies("https://stockx.com/zh-cn/adidas-yeezy-slide-black-onyx");
			MessageBox.Show(cookie);
        }
    }
}
