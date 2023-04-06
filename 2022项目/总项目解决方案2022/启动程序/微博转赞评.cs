using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace 启动程序
{
	// Token: 0x0200000F RID: 15
	public partial class 微博转赞评 : Form
	{
		// Token: 0x0600008A RID: 138 RVA: 0x00012B4C File Offset: 0x00010D4C
		public 微博转赞评()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00012BB4 File Offset: 0x00010DB4
		public static string PostUrl(string url, string postData)
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
				httpWebRequest.Headers.Add("Cookie", "");
				httpWebRequest.Referer = "";
				StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
				streamWriter.Write(postData);
				streamWriter.Flush();
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
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

		// Token: 0x0600008C RID: 140 RVA: 0x00012CB8 File Offset: 0x00010EB8
		private void Button1_Click(object sender, EventArgs e)
		{
			添加微博 添加微博 = new 添加微博();
			添加微博.Show();
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00012CD4 File Offset: 0x00010ED4
		public void add()
		{
			ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
			listViewItem.SubItems.Add(添加微博.beizhu);
			listViewItem.SubItems.Add(添加微博.jishua);
			listViewItem.SubItems.Add(添加微博.rengong);
			listViewItem.SubItems.Add(添加微博.cishu);
			添加微博.beizhu = "";
			添加微博.url = "";
			添加微博.jishua = "";
			添加微博.rengong = "";
			添加微博.cishu = "";
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00012D84 File Offset: 0x00010F84
		public string MD5Encrypt(string password, int bit)
		{
			MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
			byte[] array = md5CryptoServiceProvider.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(password));
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in array)
			{
				stringBuilder.Append(b.ToString("x2"));
			}
			bool flag = bit == 16;
			string result;
			if (flag)
			{
				result = stringBuilder.ToString().Substring(8, 16);
			}
			else
			{
				bool flag2 = bit == 32;
				if (flag2)
				{
					result = stringBuilder.ToString();
				}
				else
				{
					result = string.Empty;
				}
			}
			return result;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00012E24 File Offset: 0x00011024
		public string GetTimeStamp()
		{
			return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00012E68 File Offset: 0x00011068
		public void weibo()
		{
			try
			{
				string url = "http://9b00.181sq.com.api.94sq.cn/api/order";
				string password = string.Concat(new string[]
				{
					"api_token=",
					this.textBox1.Text,
					"&gid=",
					this.Jzhuan,
					"&num=1&timestamp=",
					this.GetTimeStamp(),
					"&value1=1"
				});
				string text = this.MD5Encrypt(password, 32);
				string text2 = string.Concat(new string[]
				{
					"api_token=",
					this.textBox1.Text,
					"&timestamp=",
					this.GetTimeStamp(),
					"&sign=",
					text,
					"&gid=",
					this.Jzhuan,
					"&num=1&value1="
				});
				MessageBox.Show(text2);
				string text3 = 微博转赞评.PostUrl(url, text2);
				this.textBox9.Text = text3;
			}
			catch (Exception)
			{
				throw;
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00012F64 File Offset: 0x00011164
		private void 微博转赞评_Load(object sender, EventArgs e)
		{
			this.Jzhuan = this.textBox3.Text.Trim();
			this.Jzan = this.textBox4.Text.Trim();
			this.Jping = this.textBox5.Text.Trim();
			this.Rzhuan = this.textBox6.Text.Trim();
			this.Rzan = this.textBox7.Text.Trim();
			this.Rping = this.textBox8.Text.Trim();
			foreach (object obj in this.groupBox1.Controls)
			{
				Control control = (Control)obj;
				bool flag = control is TextBox;
				if (flag)
				{
					string str = AppDomain.CurrentDomain.BaseDirectory + "value\\";
					bool flag2 = File.Exists(str + control.Name + ".txt");
					if (flag2)
					{
						StreamReader streamReader = new StreamReader(str + control.Name + ".txt", Encoding.GetEncoding("utf-8"));
						string text = streamReader.ReadToEnd();
						control.Text = text;
						streamReader.Close();
					}
				}
			}
			foreach (object obj2 in this.groupBox2.Controls)
			{
				Control control2 = (Control)obj2;
				bool flag3 = control2 is TextBox;
				if (flag3)
				{
					string str2 = AppDomain.CurrentDomain.BaseDirectory + "value\\";
					bool flag4 = File.Exists(str2 + control2.Name + ".txt");
					if (flag4)
					{
						StreamReader streamReader2 = new StreamReader(str2 + control2.Name + ".txt", Encoding.GetEncoding("utf-8"));
						string text2 = streamReader2.ReadToEnd();
						control2.Text = text2;
						streamReader2.Close();
					}
				}
			}
			foreach (object obj3 in this.groupBox2.Controls)
			{
				Control control3 = (Control)obj3;
				bool flag5 = control3 is TextBox;
				if (flag5)
				{
					string str3 = AppDomain.CurrentDomain.BaseDirectory + "value\\";
					bool flag6 = File.Exists(str3 + control3.Name + ".txt");
					if (flag6)
					{
						StreamReader streamReader3 = new StreamReader(str3 + control3.Name + ".txt", Encoding.GetEncoding("utf-8"));
						string text3 = streamReader3.ReadToEnd();
						control3.Text = text3;
						streamReader3.Close();
					}
				}
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0001328C File Offset: 0x0001148C
		private void 微博转赞评_FormClosing(object sender, FormClosingEventArgs e)
		{
			DialogResult dialogResult = MessageBox.Show("确认退出吗？", "退出询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
			bool flag = dialogResult != DialogResult.OK;
			if (flag)
			{
				e.Cancel = true;
			}
			else
			{
				foreach (object obj in this.groupBox1.Controls)
				{
					Control control = (Control)obj;
					bool flag2 = control is TextBox;
					if (flag2)
					{
						string str = AppDomain.CurrentDomain.BaseDirectory + "value\\";
						FileStream fileStream = new FileStream(str + control.Name + ".txt", FileMode.Create, FileAccess.Write);
						StreamWriter streamWriter = new StreamWriter(fileStream);
						streamWriter.WriteLine(control.Text.Trim());
						streamWriter.Close();
						fileStream.Close();
					}
				}
				foreach (object obj2 in this.groupBox2.Controls)
				{
					Control control2 = (Control)obj2;
					bool flag3 = control2 is TextBox;
					if (flag3)
					{
						string str2 = AppDomain.CurrentDomain.BaseDirectory + "value\\";
						FileStream fileStream2 = new FileStream(str2 + control2.Name + ".txt", FileMode.Create, FileAccess.Write);
						StreamWriter streamWriter2 = new StreamWriter(fileStream2);
						streamWriter2.WriteLine(control2.Text.Trim());
						streamWriter2.Close();
						fileStream2.Close();
					}
				}
				foreach (object obj3 in this.groupBox3.Controls)
				{
					Control control3 = (Control)obj3;
					bool flag4 = control3 is TextBox;
					if (flag4)
					{
						string str3 = AppDomain.CurrentDomain.BaseDirectory + "value\\";
						FileStream fileStream3 = new FileStream(str3 + control3.Name + ".txt", FileMode.Create, FileAccess.Write);
						StreamWriter streamWriter3 = new StreamWriter(fileStream3);
						streamWriter3.WriteLine(control3.Text.Trim());
						streamWriter3.Close();
						fileStream3.Close();
					}
				}
				Process.GetCurrentProcess().Kill();
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00013528 File Offset: 0x00011728
		private void ListView1_MouseEnter(object sender, EventArgs e)
		{
			bool flag = 添加微博.beizhu != "";
			if (flag)
			{
				this.add();
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00013552 File Offset: 0x00011752
		private void button3_Click(object sender, EventArgs e)
		{
			this.weibo();
		}

		// Token: 0x04000174 RID: 372
		private string Jzhuan = "";

		// Token: 0x04000175 RID: 373
		private string Jzan = "";

		// Token: 0x04000176 RID: 374
		private string Jping = "";

		// Token: 0x04000177 RID: 375
		private string Rzhuan = "";

		// Token: 0x04000178 RID: 376
		private string Rzan = "";

		// Token: 0x04000179 RID: 377
		private string Rping = "";
	}
}
