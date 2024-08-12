using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace 备案查询
{
	// Token: 0x02000003 RID: 3
	public partial class 备案查询 : Form
	{
		// Token: 0x06000002 RID: 2 RVA: 0x0000206B File Offset: 0x0000026B
		public 备案查询()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000208C File Offset: 0x0000028C
		private void button1_Click(object sender, EventArgs e)
		{
			bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
			if (flag)
			{
				this.textBox1.Text = this.openFileDialog1.FileName;
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020C8 File Offset: 0x000002C8
		public static string GetUrl(string Url, string charset)
		{
			string COOKIE = "";
			string result;
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
				request.Proxy = null;
				request.AllowAutoRedirect = true;
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
				request.Referer = Url;
				request.Headers.Add("Cookie", COOKIE);
				request.Headers.Add("Accept-Encoding", "gzip");
				request.KeepAlive = true;
				request.Accept = "*/*";
				request.Timeout = 5000;
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
			catch (Exception ex)
			{
				result = ex.ToString();
			}
			return result;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002220 File Offset: 0x00000420
		public void run()
		{
			string filename = DateTime.Now.ToString("MM月dd日HH时mm分");
			StreamReader sr = new StreamReader(this.textBox1.Text, 备案查询.EncodingType.GetTxtType(this.textBox1.Text));
			string texts = sr.ReadToEnd();
			string[] text = texts.Split(new string[]
			{
				"\r\n"
			}, StringSplitOptions.None);
			sr.Close();
			sr.Dispose();
			for (int i = 0; i < text.Length; i++)
			{
				bool flag = text[i] != "";
				if (flag)
				{
					this.label1.Text = "累计已查询：" + i.ToString();
					string yuming = text[i].Trim();
					//string url = "https://beian.tianyancha.com/search/" + yuming;
					string url = "http://micp.chinaz.com/" + yuming;
					string html = 备案查询.GetUrl(url, "utf-8");
					bool flag2 = html.Contains("备案号");
					if (flag2)
					{
						TextBox textBox = this.textBox2;
						textBox.Text = textBox.Text + DateTime.Now.ToString() + yuming + "查询成功：已备案\r\n";
						FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\" + filename + ".txt", FileMode.Append, FileAccess.Write);
						StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("UTF-8"));
						sw.WriteLine(yuming);
						sw.Close();
						fs.Close();
						sw.Dispose();
					}
					else
					{
						TextBox textBox2 = this.textBox2;
						textBox2.Text = textBox2.Text + DateTime.Now.ToString() + yuming + "查询成功：未备案\r\n";
					}
					bool flag3 = this.textBox2.Text.Length > 400;
					if (flag3)
					{
						this.textBox2.Text = "";
					}
					bool flag4 = !this.status;
					if (flag4)
					{
						break;
					}
				}
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002418 File Offset: 0x00000618
		private void button2_Click(object sender, EventArgs e)
		{
			
			if(DateTime.Now>Convert.ToDateTime("2024-09-01"))
            {
				return;
            }
			
			
			
			bool flag = this.textBox1.Text == "";
			if (flag)
			{
				MessageBox.Show("请导入文本");
			}
			else
			{
				this.status = true;
				bool flag2 = this.thread == null || !this.thread.IsAlive;
				if (flag2)
				{
					this.thread = new Thread(new ThreadStart(this.run));
					this.thread.Start();
					Control.CheckForIllegalCrossThreadCalls = false;
				}
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000249D File Offset: 0x0000069D
		private void button3_Click(object sender, EventArgs e)
		{
			this.status = false;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000024A8 File Offset: 0x000006A8
		private void 备案查询_FormClosing(object sender, FormClosingEventArgs e)
		{
			DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
			bool flag = dr == DialogResult.OK;
			if (flag)
			{
				Process.GetCurrentProcess().Kill();
			}
			else
			{
				e.Cancel = true;
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000024EC File Offset: 0x000006EC
		public static void TestForKillMyself()
		{
			string bat = "@echo off\r\n                           :tryagain\r\n                           del %1\r\n                           if exist %1 goto tryagain\r\n                           del %0";
			File.WriteAllText("killme.bat", bat);
			Process.Start(new ProcessStartInfo
			{
				FileName = "killme.bat",
				Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"",
				WindowStyle = ProcessWindowStyle.Hidden
			});
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000254C File Offset: 0x0000074C
		private void 备案查询_Load(object sender, EventArgs e)
		{
			
		}

		// Token: 0x04000001 RID: 1
		private bool status = true;

		// Token: 0x04000002 RID: 2
		private Thread thread;

		// Token: 0x02000007 RID: 7
		public class EncodingType
		{
			// Token: 0x06000014 RID: 20 RVA: 0x00002B30 File Offset: 0x00000D30
			public static Encoding GetTxtType(string FILE_NAME)
			{
				FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
				Encoding r = 备案查询.EncodingType.GetType(fs);
				fs.Close();
				return r;
			}

			// Token: 0x06000015 RID: 21 RVA: 0x00002B5C File Offset: 0x00000D5C
			public static Encoding GetType(FileStream fs)
			{
				byte[] array = new byte[]
				{
					byte.MaxValue,
					254,
					65
				};
				byte[] array2 = new byte[3];
				array2[0] = 254;
				array2[1] = byte.MaxValue;
				byte[] array3 = new byte[]
				{
					239,
					187,
					191
				};
				Encoding reVal = Encoding.Default;
				BinaryReader r = new BinaryReader(fs, Encoding.Default);
				int i;
				int.TryParse(fs.Length.ToString(), out i);
				byte[] ss = r.ReadBytes(i);
				bool flag = 备案查询.EncodingType.IsUTF8Bytes(ss) || (ss[0] == 239 && ss[1] == 187 && ss[2] == 191);
				if (flag)
				{
					reVal = Encoding.UTF8;
				}
				else
				{
					bool flag2 = ss[0] == 254 && ss[1] == byte.MaxValue && ss[2] == 0;
					if (flag2)
					{
						reVal = Encoding.BigEndianUnicode;
					}
					else
					{
						bool flag3 = ss[0] == byte.MaxValue && ss[1] == 254 && ss[2] == 65;
						if (flag3)
						{
							reVal = Encoding.Unicode;
						}
					}
				}
				r.Close();
				return reVal;
			}

			/// <summary> 
			/// 判断是否是不带 BOM 的 UTF8 格式 
			/// </summary> 
			/// <param name=“data“></param> 
			/// <returns></returns> 
			private static bool IsUTF8Bytes(byte[] data)
			{
				int charByteCounter = 1; //计算当前正分析的字符应还有的字节数 
				byte curByte; //当前分析的字节. 
				for (int i = 0; i < data.Length; i++)
				{
					curByte = data[i];
					if (charByteCounter == 1)
					{
						if (curByte >= 0x80)
						{
							//判断当前 
							while (((curByte <<= 1) & 0x80) != 0)
							{
								charByteCounter++;
							}
							//标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
							if (charByteCounter == 1 || charByteCounter > 6)
							{
								return false;
							}
						}
					}
					else
					{
						//若是UTF-8 此时第一位必须为1 
						if ((curByte & 0xC0) != 0x80)
						{
							return false;
						}
						charByteCounter--;
					}
				}
				if (charByteCounter > 1)
				{
					throw new Exception("非预期的byte格式");
				}
				return true;
			}
		



	}
	}
}
