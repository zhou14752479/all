using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using helper;

namespace 启动程序
{
	// Token: 0x02000017 RID: 23
	public partial class 票据网 : Form
	{
		// Token: 0x060000DC RID: 220 RVA: 0x0001A960 File Offset: 0x00018B60
		public static string PostUrl(string url, string postData, string COOKIE, string charset)
		{
			string result;
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.Method = "Post";
				WebHeaderCollection headers = httpWebRequest.Headers;
				headers.Add("authorization: Bearer " + 票据网.token);
				httpWebRequest.ContentType = "application/json";
				httpWebRequest.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
				httpWebRequest.KeepAlive = true;
				httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
				httpWebRequest.Headers.Add("Cookie", COOKIE);
				httpWebRequest.Referer = "https://www.tcpjw.com/B2BHall";
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

		// Token: 0x060000DD RID: 221 RVA: 0x0001AA88 File Offset: 0x00018C88
		public 票据网()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0001AB45 File Offset: 0x00018D45
		private void 票据网_Load(object sender, EventArgs e)
		{
			method.SetWebBrowserFeatures(method.IeVersion.IE10);
			this.webBrowser1.ScriptErrorsSuppressed = true;
			this.webBrowser1.Navigate("https://www.tcpjw.com/login");
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0001AB70 File Offset: 0x00018D70
		public string getdealPrice(string ticketId)
		{
			string url = "https://www.tcpjw.com/order-web/orderInfo/getQuoteOrderInfo";
			string postData = "{\"version\":\"3.5\",\"source\":\"HTML\",\"channel\":\"01\",\"ticketId\":" + ticketId + "}";
			string input = 票据网.PostUrl(url, postData, this.cookie, "utf-8");
			Match match = Regex.Match(input, "\"totalPrice\":([\\s\\S]*?),");
			return match.Groups[1].Value;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0001ABCC File Offset: 0x00018DCC
		public void buy(string ticketId, string ThousandCharge, string paytype, string endorseId, string yearrate, string ticketPrice, string ticketType)
		{
			string text = this.getdealPrice(ticketId);
			string url = "https://www.tcpjw.com/order-web/orderFlow/quoteOrder";
			string postData = string.Concat(new string[]
			{
				"{\"SOURCE\":\"HTML\",\"VERSION\":\"3.5\",\"CHANNEL\":\"01\",\"ticketId\":",
				ticketId,
				",\"hundredThousandCharge\":\"",
				ThousandCharge,
				"\",\"payType\":",
				paytype,
				",\"endorseId\":",
				endorseId,
				",\"yearRate\":",
				yearrate,
				",\"dealPrice\":",
				text,
				",\"ticketPrice\":",
				ticketPrice,
				",\"ticketType\":",
				ticketType,
				",\"useDefault\":false}"
			});
			string text2 = 票据网.PostUrl(url, postData, this.cookie, "utf-8");
			TextBox textBox = this.textBox6;
			textBox.Text = string.Concat(new string[]
			{
				textBox.Text,
				DateTime.Now.ToString(),
				"：",
				text2,
				"\r\n"
			});
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0001ACC8 File Offset: 0x00018EC8
		public void run()
		{
			this.cookie = method.GetCookies("https://www.tcpjw.com/tradingHall");
			bool flag = this.cookie == "";
			if (flag)
			{
				MessageBox.Show("未登录");
			}
			Match match = Regex.Match(this.cookie, "access_token=.*");
			string input = match.Groups[0].Value.Replace("access_token=", "");
			票据网.token = Regex.Replace(input, ";.*", "");
			try
			{
				string url = "https://www.tcpjw.com/order-web/orderInfo/getTradingOrderInfo";
				string postData = string.Concat(new string[]
				{
					"{\"source\":\"HTML\",\"version\":\"3.5\",\"channel\":\"01\",\"pageNum\":1,\"pageSize\":15,\"tradeStatus\":null,\"payType\":",
					this.paytype,
					",\"bid\":",
					this.bid,
					",\"bankName\":",
					this.bankName,
					",\"lastTime\":",
					this.lasttime,
					",\"lastTimeStart\":null,\"lastTimeEnd\":null,\"startDate\":null,\"endDate\":null,\"flawStatus\":\"",
					this.flawStatus,
					"\",\"priceType\":",
					this.pricetype,
					",\"priceSp\":",
					this.priceSp,
					",\"priceEp\":",
					this.priceEp,
					",\"yearQuote\":",
					this.yearQuote,
					",\"msw\":",
					this.msw,
					",\"mswStart\":null,\"mswEnd\":null,\"orderColumn\":null,\"sortType\":\"\",\"depositPay\":",
					this.depositPay,
					"}"
				});
				string input2 = 票据网.PostUrl(url, postData, this.cookie, "utf-8");
				MatchCollection matchCollection = Regex.Matches(input2, "\"ticketId\":([\\s\\S]*?),");
				MatchCollection matchCollection2 = Regex.Matches(input2, "\"publishTime\":\"([\\s\\S]*?)\"");
				MatchCollection matchCollection3 = Regex.Matches(input2, "\"bankName\":\"([\\s\\S]*?)\"");
				MatchCollection matchCollection4 = Regex.Matches(input2, "\"ticketPrice\":([\\s\\S]*?),");
				MatchCollection matchCollection5 = Regex.Matches(input2, "\"endTime\":\"([\\s\\S]*?)\"");
				MatchCollection matchCollection6 = Regex.Matches(input2, "\"sellPrice\":\"([\\s\\S]*?)\"");
				MatchCollection matchCollection7 = Regex.Matches(input2, "\"yearQuote\":([\\s\\S]*?),");
				MatchCollection matchCollection8 = Regex.Matches(input2, "\"flawDescription\":\"([\\s\\S]*?)\"");
				MatchCollection matchCollection9 = Regex.Matches(input2, "payName\":\\[\"([\\s\\S]*?)\\]");
				for (int i = 0; i < matchCollection2.Count; i++)
				{
					ListViewItem listViewItem = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
					listViewItem.SubItems.Add(matchCollection2[i].Groups[1].Value);
					listViewItem.SubItems.Add(matchCollection3[i].Groups[1].Value);
					listViewItem.SubItems.Add(matchCollection4[i].Groups[1].Value);
					listViewItem.SubItems.Add(matchCollection5[i].Groups[1].Value);
					listViewItem.SubItems.Add(matchCollection6[i].Groups[1].Value);
					listViewItem.SubItems.Add(matchCollection7[i].Groups[1].Value);
					listViewItem.SubItems.Add(matchCollection8[i].Groups[1].Value);
					listViewItem.SubItems.Add(matchCollection9[i].Groups[1].Value);
					string value = matchCollection[i].Groups[1].Value;
					string value2 = matchCollection6[i].Groups[1].Value;
					string text = "1";
					string endorseId = "15858";
					string value3 = matchCollection7[i].Groups[1].Value;
					string value4 = matchCollection4[i].Groups[1].Value;
					string value5 = matchCollection4[i].Groups[1].Value;
					string ticketType = "2";
					bool flag2 = !this.xiadans.Contains(value);
					if (flag2)
					{
						this.xiadans.Add(value);
						this.buy(value, value2, text, endorseId, value3, value5, ticketType);
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0001B140 File Offset: 0x00019340
		private void Button1_Click(object sender, EventArgs e)
		{
			bool flag = this.textBox2.Text != "";
			if (flag)
			{
				this.priceSp = "\"" + this.textBox2.Text.Trim() + "\"";
				this.priceEp = "\"" + this.textBox3.Text.Trim() + "\"";
			}
			bool flag2 = this.textBox4.Text != "";
			if (flag2)
			{
				this.yearQuote = "\"" + this.textBox4.Text.Trim() + "\"";
				this.msw = "\"" + this.textBox5.Text.Trim() + "\"";
			}
			bool flag3 = this.textBox1.Text != "";
			if (flag3)
			{
				this.bankName = "\"" + this.textBox1.Text.Trim() + "\"";
			}
			bool @checked = this.radioButton1.Checked;
			if (@checked)
			{
				this.paytype = "\"1\"";
			}
			bool checked2 = this.radioButton2.Checked;
			if (checked2)
			{
				this.paytype = "\"2\"";
			}
			bool checked3 = this.radioButton3.Checked;
			if (checked3)
			{
				this.paytype = "\"3\"";
			}
			bool checked4 = this.radioButton4.Checked;
			if (checked4)
			{
				this.depositPay = "true";
			}
			bool checked5 = this.radioButton5.Checked;
			if (checked5)
			{
				this.depositPay = "false";
			}
			bool checked6 = this.checkBox1.Checked;
			if (checked6)
			{
				this.bids.Append("\"1\",");
				this.bid = "[" + this.bids.ToString().Remove(this.bids.Length - 1, 1) + "]";
			}
			bool checked7 = this.checkBox2.Checked;
			if (checked7)
			{
				this.bids.Append("\"9\",");
				this.bid = "[" + this.bids.ToString().Remove(this.bids.Length - 1, 1) + "]";
			}
			bool checked8 = this.checkBox3.Checked;
			if (checked8)
			{
				this.bids.Append("\"2\",");
				this.bid = "[" + this.bids.ToString().Remove(this.bids.Length - 1, 1) + "]";
			}
			bool checked9 = this.checkBox4.Checked;
			if (checked9)
			{
				this.bids.Append("\"3\",");
				this.bid = "[" + this.bids.ToString().Remove(this.bids.Length - 1, 1) + "]";
			}
			bool checked10 = this.checkBox5.Checked;
			if (checked10)
			{
				this.bids.Append("\"6\",");
				this.bid = "[" + this.bids.ToString().Remove(this.bids.Length - 1, 1) + "]";
			}
			bool checked11 = this.checkBox6.Checked;
			if (checked11)
			{
				this.bids.Append("\"7\",");
				this.bid = "[" + this.bids.ToString().Remove(this.bids.Length - 1, 1) + "]";
			}
			bool checked12 = this.checkBox7.Checked;
			if (checked12)
			{
				this.bids.Append("\"10\",");
				this.bid = "[" + this.bids.ToString().Remove(this.bids.Length - 1, 1) + "]";
			}
			bool checked13 = this.checkBox8.Checked;
			if (checked13)
			{
				this.bids.Append("\"4\",");
				this.bid = "[" + this.bids.ToString().Remove(this.bids.Length - 1, 1) + "]";
			}
			bool checked14 = this.checkBox9.Checked;
			if (checked14)
			{
				this.bids.Append("\"8\",");
				this.bid = "[" + this.bids.ToString().Remove(this.bids.Length - 1, 1) + "]";
			}
			bool checked15 = this.radioButton6.Checked;
			if (checked15)
			{
				this.pricetype = "\"1\"";
			}
			bool checked16 = this.radioButton7.Checked;
			if (checked16)
			{
				this.pricetype = "\"2\"";
			}
			bool checked17 = this.radioButton8.Checked;
			if (checked17)
			{
				this.pricetype = "\"3\"";
			}
			bool checked18 = this.radioButton9.Checked;
			if (checked18)
			{
				this.pricetype = "\"4\"";
			}
			bool checked19 = this.radioButton10.Checked;
			if (checked19)
			{
				this.pricetype = "\"1\"";
			}
			bool checked20 = this.radioButton11.Checked;
			if (checked20)
			{
				this.pricetype = "\"2\"";
			}
			bool checked21 = this.radioButton12.Checked;
			if (checked21)
			{
				this.pricetype = "\"3\"";
			}
			bool checked22 = this.radioButton13.Checked;
			if (checked22)
			{
				this.pricetype = "\"4\"";
			}
			bool checked23 = this.radioButton14.Checked;
			if (checked23)
			{
				this.pricetype = "\"5\"";
			}
			bool checked24 = this.radioButton15.Checked;
			if (checked24)
			{
				this.flawStatus = "0";
			}
			bool checked25 = this.radioButton16.Checked;
			if (checked25)
			{
				this.flawStatus = "46";
			}
			bool checked26 = this.radioButton17.Checked;
			if (checked26)
			{
				this.flawStatus = "47";
			}
			bool checked27 = this.radioButton18.Checked;
			if (checked27)
			{
				this.flawStatus = "48";
			}
			bool checked28 = this.radioButton19.Checked;
			if (checked28)
			{
				this.flawStatus = "2";
			}
			bool checked29 = this.radioButton20.Checked;
			if (checked29)
			{
				this.flawStatus = "3";
			}
			bool checked30 = this.radioButton21.Checked;
			if (checked30)
			{
				this.flawStatus = "4";
			}
			bool checked31 = this.radioButton22.Checked;
			if (checked31)
			{
				this.flawStatus = "5";
			}
			bool checked32 = this.radioButton23.Checked;
			if (checked32)
			{
				this.flawStatus = "6";
			}
			this.timer1.Start();
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0001B823 File Offset: 0x00019A23
		private void Timer1_Tick(object sender, EventArgs e)
		{
			this.listView1.Items.Clear();
			this.run();
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0001B83E File Offset: 0x00019A3E
		private void button4_Click(object sender, EventArgs e)
		{
			this.timer1.Stop();
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0001B850 File Offset: 0x00019A50
		private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
			if (flag)
			{
				StreamReader streamReader = new StreamReader(this.openFileDialog1.FileName, Encoding.Default);
				string text = streamReader.ReadToEnd();
				string[] array = text.Split(new string[]
				{
					"\r\n"
				}, StringSplitOptions.None);
				for (int i = 0; i < array.Length; i++)
				{
					TextBox textBox = this.textBox1;
					textBox.Text = textBox.Text + array[i] + " ";
				}
			}
		}

		// Token: 0x04000230 RID: 560
		private static string token = "";

		// Token: 0x04000231 RID: 561
		private string cookie = "";

		// Token: 0x04000232 RID: 562
		private ArrayList xiadans = new ArrayList();

		// Token: 0x04000233 RID: 563
		private StringBuilder bids = new StringBuilder();

		// Token: 0x04000234 RID: 564
		private string paytype = "null";

		// Token: 0x04000235 RID: 565
		private string depositPay = "null";

		// Token: 0x04000236 RID: 566
		private string bid = "null";

		// Token: 0x04000237 RID: 567
		private string pricetype = "null";

		// Token: 0x04000238 RID: 568
		private string lasttime = "null";

		// Token: 0x04000239 RID: 569
		private string flawStatus = "";

		// Token: 0x0400023A RID: 570
		private string priceSp = "null";

		// Token: 0x0400023B RID: 571
		private string priceEp = "null";

		// Token: 0x0400023C RID: 572
		private string msw = "null";

		// Token: 0x0400023D RID: 573
		private string yearQuote = "null";

		// Token: 0x0400023E RID: 574
		private string bankName = "null";
	}
}
