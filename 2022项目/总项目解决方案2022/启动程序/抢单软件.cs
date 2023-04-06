using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace 启动程序
{
	// Token: 0x02000010 RID: 16
	public partial class 抢单软件 : Form
	{
		// Token: 0x06000097 RID: 151 RVA: 0x00014761 File Offset: 0x00012961
		public 抢单软件()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00014779 File Offset: 0x00012979
		private void 抢单软件_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0001477C File Offset: 0x0001297C
		public static string GetUrl(string Url, string charset)
		{
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				string value = "";
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
				httpWebRequest.Referer = "https://cn.bing.com/search?q=%e9%a6%99%e6%b8%af%e5%85%ad%e5%90%88%e5%bd%a9&qs=n&sp=-1&first=01&FORM=PORE";
				httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
				httpWebRequest.AllowAutoRedirect = true;
				httpWebRequest.Headers.Add("Cookie", value);
				httpWebRequest.KeepAlive = true;
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

		// Token: 0x0600009A RID: 154 RVA: 0x00014854 File Offset: 0x00012A54
		public static string PostUrl(string url, string postData, string COOKIE)
		{
			string result;
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.Method = "Post";
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				httpWebRequest.ContentLength = (long)postData.Length;
				httpWebRequest.AllowAutoRedirect = false;
				httpWebRequest.KeepAlive = true;
				httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
				httpWebRequest.Headers.Add("Cookie", COOKIE);
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

		// Token: 0x0600009B RID: 155 RVA: 0x00014954 File Offset: 0x00012B54
		public void run()
		{
			string url = "https://www.iciti.co/otc/push-buy";
			string text = this.textBox4.Text;
			string postData = "id=" + this.textBox1.Text.Trim() + "if&type=0&password=" + this.textBox2.Text.Trim();
			string text2 = 抢单软件.PostUrl(url, postData, text);
			bool flag = text2.Contains("購買成功");
			if (flag)
			{
				this.label4.Text = "成功，已暂停";
				this.timer1.Stop();
			}
			else
			{
				bool flag2 = this.textBox3.Lines.Length > 20;
				if (flag2)
				{
					this.textBox3.Text = "";
				}
				TextBox textBox = this.textBox3;
				textBox.Text = textBox.Text + DateTime.Now.ToString() + text2 + "\r\n";
				TextBox textBox2 = this.textBox3;
				textBox2.Text = textBox2.Text + DateTime.Now.ToString() + text2 + "\r\n";
				TextBox textBox3 = this.textBox3;
				textBox3.Text = textBox3.Text + DateTime.Now.ToString() + text2 + "\r\n";
				TextBox textBox4 = this.textBox3;
				textBox4.Text = textBox4.Text + DateTime.Now.ToString() + text2 + "\r\n";
				TextBox textBox5 = this.textBox3;
				textBox5.Text = textBox5.Text + DateTime.Now.ToString() + text2 + "\r\n";
				TextBox textBox6 = this.textBox3;
				textBox6.Text = textBox6.Text + DateTime.Now.ToString() + text2 + "\r\n";
				TextBox textBox7 = this.textBox3;
				textBox7.Text = textBox7.Text + DateTime.Now.ToString() + text2 + "\r\n";
				TextBox textBox8 = this.textBox3;
				textBox8.Text = textBox8.Text + DateTime.Now.ToString() + text2 + "\r\n";
				Thread.Sleep(1);
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00014B6C File Offset: 0x00012D6C
		private void Button1_Click(object sender, EventArgs e)
		{
			this.timer1.Start();
			this.timer1.Interval = Convert.ToInt32(this.textBox5.Text);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00014B98 File Offset: 0x00012D98
		private void Timer1_Tick(object sender, EventArgs e)
		{
			Thread thread = new Thread(new ThreadStart(this.run));
			thread.Start();
			Control.CheckForIllegalCrossThreadCalls = false;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00014BC6 File Offset: 0x00012DC6
		private void Button2_Click(object sender, EventArgs e)
		{
			this.timer1.Stop();
		}
	}
}
