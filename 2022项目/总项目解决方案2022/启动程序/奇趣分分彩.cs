using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using helper;

namespace 启动程序
{
	// Token: 0x0200000C RID: 12
	public partial class 奇趣分分彩 : Form
	{
		// Token: 0x06000059 RID: 89 RVA: 0x0000D3FD File Offset: 0x0000B5FD
		public 奇趣分分彩()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000D418 File Offset: 0x0000B618
		public static string GetUrlWithCookie(string Url, string COOKIE)
		{
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
				httpWebRequest.AllowAutoRedirect = true;
				httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
				httpWebRequest.Headers.Add("Cookie", COOKIE);
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				httpWebRequest.KeepAlive = true;
				StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));
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

		// Token: 0x0600005B RID: 91 RVA: 0x0000D4D4 File Offset: 0x0000B6D4
		private void 奇趣分分彩_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000D4D8 File Offset: 0x0000B6D8
		private void Button1_Click(object sender, EventArgs e)
		{
			Form1 form = new Form1();
			form.Show();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000D4F4 File Offset: 0x0000B6F4
		public void chongfu()
		{
			for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
			{
				for (int j = 0; j < this.dataGridView1.Rows.Count; j++)
				{
					bool flag = this.dataGridView1.Rows[j].Cells[3].Value == this.dataGridView1.Rows[i].Cells[1].Value;
					if (flag)
					{
						this.dataGridView1.Rows[j].Cells[3].Style.BackColor = Color.Red;
						this.dataGridView1.Rows[i].Cells[1].Style.BackColor = Color.Red;
					}
				}
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000D5F4 File Offset: 0x0000B7F4
		public void run()
		{
			this.dataGridView1.Rows.Clear();
			string url = "https://yx-668.com/APIV2/GraphQL?l=zh-cn";
			string postData = "{\"operationName\":\"GetLotteryCycle\",\"variables\":{\"game_id\":190,\"row_count\":50},\"query\":\"query GetLotteryCycle($game_id: Int!, $row_count: Int) {\n LotteryGame(game_id: $game_id) {\n game_value\n game_id\n base_game\n start_number\n end_number\n number_count\n lottery_result_history(row_count: $row_count) {\n cycle_value\n game_result\n __typename\n    }\n trend_chart(row_count: $row_count) {\n titles\n __typename\n    }\n __typename\n  }\n}\n\"}";
			string postData2 = "{\"operationName\":\"GetLotteryCycle\",\"variables\":{\"game_id\":237,\"row_count\":50},\"query\":\"query GetLotteryCycle($game_id: Int!, $row_count: Int) {\n LotteryGame(game_id: $game_id) {\n game_value\n game_id\n base_game\n start_number\n end_number\n number_count\n lottery_result_history(row_count: $row_count) {\n cycle_value\n game_result\n __typename\n    }\n trend_chart(row_count: $row_count) {\n titles\n __typename\n    }\n __typename\n  }\n}\n\"}";
			string postData3 = "{\"operationName\":\"GetLotteryCycle\",\"variables\":{\"game_id\":191,\"row_count\":50},\"query\":\"query GetLotteryCycle($game_id: Int!, $row_count: Int) {\n LotteryGame(game_id: $game_id) {\n game_value\n game_id\n base_game\n start_number\n end_number\n number_count\n lottery_result_history(row_count: $row_count) {\n cycle_value\n game_result\n __typename\n    }\n trend_chart(row_count: $row_count) {\n titles\n __typename\n    }\n __typename\n  }\n}\n\"}";
			string postData4 = "{\"operationName\":\"GetLotteryCycle\",\"variables\":{\"game_id\":192,\"row_count\":50},\"query\":\"query GetLotteryCycle($game_id: Int!, $row_count: Int) {\n LotteryGame(game_id: $game_id) {\n game_value\n game_id\n base_game\n start_number\n end_number\n number_count\n lottery_result_history(row_count: $row_count) {\n cycle_value\n game_result\n __typename\n    }\n trend_chart(row_count: $row_count) {\n titles\n __typename\n    }\n __typename\n  }\n}\n\"}";
			string input = method.PostUrl(url, postData, "", "utf-8");
			string input2 = method.PostUrl(url, postData2, "", "utf-8");
			string input3 = method.PostUrl(url, postData3, "", "utf-8");
			string input4 = method.PostUrl(url, postData4, "", "utf-8");
			MatchCollection matchCollection = Regex.Matches(input, "game_result\":\\[([\\s\\S]*?)\\]");
			MatchCollection matchCollection2 = Regex.Matches(input2, "game_result\":\\[([\\s\\S]*?)\\]");
			MatchCollection matchCollection3 = Regex.Matches(input3, "game_result\":\\[([\\s\\S]*?)\\]");
			MatchCollection matchCollection4 = Regex.Matches(input4, "game_result\":\\[([\\s\\S]*?)\\]");
			MatchCollection matchCollection5 = Regex.Matches(input, "\"cycle_value\":\"([\\s\\S]*?)\"");
			MatchCollection matchCollection6 = Regex.Matches(input2, "\"cycle_value\":\"([\\s\\S]*?)\"");
			MatchCollection matchCollection7 = Regex.Matches(input3, "\"cycle_value\":\"([\\s\\S]*?)\"");
			MatchCollection matchCollection8 = Regex.Matches(input4, "\"cycle_value\":\"([\\s\\S]*?)\"");
			for (int i = 0; i < matchCollection.Count; i++)
			{
				int index = this.dataGridView1.Rows.Add();
				string value = matchCollection[i].Groups[1].Value.Trim().Replace("\"", "");
				string value2 = matchCollection2[i].Groups[1].Value.Trim().Replace("\"", "");
				string value3 = matchCollection3[i].Groups[1].Value.Trim().Replace("\"", "");
				string value4 = matchCollection4[i].Groups[1].Value.Trim().Replace("\"", "");
				this.dataGridView1.Rows[index].Cells[0].Value = matchCollection5[i].Groups[1].Value;
				this.dataGridView1.Rows[index].Cells[1].Value = value;
				this.dataGridView1.Rows[index].Cells[2].Value = matchCollection6[i].Groups[1].Value;
				this.dataGridView1.Rows[index].Cells[3].Value = value2;
				this.dataGridView1.Rows[index].Cells[4].Value = matchCollection7[i].Groups[1].Value;
				this.dataGridView1.Rows[index].Cells[5].Value = value3;
				this.dataGridView1.Rows[index].Cells[6].Value = matchCollection8[i].Groups[1].Value;
				this.dataGridView1.Rows[index].Cells[7].Value = value4;
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000D960 File Offset: 0x0000BB60
		private void Button6_Click(object sender, EventArgs e)
		{
			string url = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");
			bool flag = url.Contains("qqtxffc");
			if (flag)
			{
				MessageBox.Show("监控已开启");
				奇趣分分彩.cookie = Form1.cookie;
				Thread thread = new Thread(new ThreadStart(this.run));
				thread.Start();
				Control.CheckForIllegalCrossThreadCalls = false;
			}
			else
			{
				MessageBox.Show("验证失败");
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x0000D9D4 File Offset: 0x0000BBD4
		private void Timer1_Tick(object sender, EventArgs e)
		{
			Thread thread = new Thread(new ThreadStart(this.run));
			thread.Start();
			Control.CheckForIllegalCrossThreadCalls = false;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000DA02 File Offset: 0x0000BC02
		private void Button7_Click(object sender, EventArgs e)
		{
			this.chongfu();
			MessageBox.Show("监控已关闭");
			this.timer1.Stop();
		}

		// Token: 0x0400011A RID: 282
		public static string cookie = "";
	}
}
