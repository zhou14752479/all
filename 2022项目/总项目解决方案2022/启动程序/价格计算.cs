using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace 启动程序
{
	// Token: 0x02000008 RID: 8
	public partial class 价格计算 : Form
	{
		// Token: 0x0600002B RID: 43 RVA: 0x0000521B File Offset: 0x0000341B
		public 价格计算()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00005234 File Offset: 0x00003434
		public static string GetUrl(string Url)
		{
			try
			{
				string value = "Hm_lvt_fc2b90cbec55323dbc64f2b6400d86c7=1586417382; Hm_lpvt_fc2b90cbec55323dbc64f2b6400d86c7=1586418296";
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
				httpWebRequest.Referer = "https://www.jzj9999.com/";
				httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
				httpWebRequest.AllowAutoRedirect = true;
				httpWebRequest.Headers.Add("Cookie", value);
				WebHeaderCollection headers = httpWebRequest.Headers;
				headers.Add("Sec-Fetch-Mode: cors");
				headers.Add("Sec-Fetch-Site: same-origin");
				headers.Add("X-Requested-With: XMLHttpRequest");
				httpWebRequest.KeepAlive = true;
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				httpWebRequest.Timeout = 5000;
				StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));
				string result = streamReader.ReadToEnd();
				streamReader.Close();
				httpWebResponse.Close();
				return result;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			return "";
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00005338 File Offset: 0x00003538
		public void getPrice()
		{
            try
            {
				this.timer1.Start();
				this.timer1.Interval = Convert.ToInt32(this.textBox6.Text) * 1000;
				价格计算.value1 = this.textBox12.Text;
				价格计算.value2 = this.textBox16.Text;
				价格计算.value3 = this.textBox33.Text;
				this.timer2.Start();

				if (this.radioButton1.Checked || this.radioButton3.Checked)
				{
					//string url = "https://www.jzj9999.com/pricedata/getdata.aspx?tmp=93861584761821632";
					string url = "https://i.jzj9999.com/quoteh5/?ivk_sa=1025883i";
					string url2 = 价格计算.GetUrl(url);
					//Match match = Regex.Match(url2, "pubtime\":\"([\\s\\S]*?)\"");
					//Match match2 = Regex.Match(url2, "JZJ_au\", \"askprice\": \"([\\s\\S]*?)\"");
					//Match match3 = Regex.Match(url2, "JZJ_ag\", \"askprice\": \"([\\s\\S]*?)\"");
					//Match match4 = Regex.Match(url2, "JZJ_pt\", \"askprice\": \"([\\s\\S]*?)\"");
					//Match match5 = Regex.Match(url2, "JZJ_pd\", \"askprice\": \"([\\s\\S]*?)\"");
					//   this.textBox1.Text = match.Groups[1].Value;
					//this.textBox2.Text = match2.Groups[1].Value;
					//this.textBox3.Text = match3.Groups[1].Value;
					//this.textBox4.Text = match4.Groups[1].Value;
					//this.textBox5.Text = match5.Groups[1].Value;

					//Match match = Regex.Match(url2, "pubtime\":\"([\\s\\S]*?)\"");
					Match match2 = Regex.Match(url2, @"JZJ_au""([\s\S]*?)bid:""([\s\S]*?)""");
					Match match3 = Regex.Match(url2, @"JZJ_ag""([\s\S]*?)bid:""([\s\S]*?)""");
					Match match4 = Regex.Match(url2, @"JZJ_pt""([\s\S]*?)bid:""([\s\S]*?)""");
					Match match5 = Regex.Match(url2, @"JZJ_pd""([\s\S]*?)bid:""([\s\S]*?)""");
					this.textBox1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					this.textBox2.Text = match2.Groups[2].Value;
					this.textBox3.Text = match3.Groups[2].Value;
					this.textBox4.Text = match4.Groups[2].Value;
					this.textBox5.Text = match5.Groups[2].Value;

					bool flag = match2.Groups[2].Value == "" || match2.Groups[2].Value == null;
					if (flag)
					{
						this.textBox2.Text = "0";
					}
					bool flag2 = match3.Groups[2].Value == "" || match3.Groups[2].Value == null;
					if (flag2)
					{
						this.textBox3.Text = "0";
					}
					bool flag3 = match4.Groups[2].Value == "" || match4.Groups[2].Value == null;
					if (flag3)
					{
						this.textBox4.Text = "0";
					}
					bool flag4 = match5.Groups[2].Value == "" || match5.Groups[2].Value == null;
					if (flag4)
					{
						this.textBox5.Text = "0";
					}
					价格计算.hjPrice = Convert.ToDouble(this.textBox2.Text);
					价格计算.byPrice = Convert.ToDouble(this.textBox3.Text);
					价格计算.bojPrice = Convert.ToDouble(this.textBox4.Text);
					价格计算.bajPrice = Convert.ToDouble(this.textBox5.Text);
					价格计算.time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				}

				if (this.radioButton2.Checked)
				{
					string url3 = "http://www.lfgold.cn/inte/goldprice.ashx?m_t1587100063820";
					string url4 = 价格计算.GetUrl(url3);
					Match match6 = Regex.Match(url4, "UpTime\":\"([\\s\\S]*?)\"");
					Match match7 = Regex.Match(url4, "黄金\",\"BP\":([\\s\\S]*?),\"SP\":([\\s\\S]*?),");
					Match match8 = Regex.Match(url4, "白银\",\"BP\":([\\s\\S]*?),\"SP\":([\\s\\S]*?),");
					Match match9 = Regex.Match(url4, "铂金\",\"BP\":([\\s\\S]*?),\"SP\":([\\s\\S]*?),");
					Match match10 = Regex.Match(url4, "钯金\",\"BP\":([\\s\\S]*?),\"SP\":([\\s\\S]*?),");
					this.textBox1.Text = match6.Groups[1].Value;
					this.textBox2.Text = match7.Groups[2].Value;
					this.textBox3.Text = match8.Groups[2].Value;
					this.textBox4.Text = match9.Groups[2].Value;
					this.textBox5.Text = match10.Groups[2].Value;
					bool flag5 = match7.Groups[1].Value == "" || match7.Groups[1].Value == null;
					if (flag5)
					{
						this.textBox2.Text = "0";
					}
					bool flag6 = match8.Groups[1].Value == "" || match8.Groups[1].Value == null;
					if (flag6)
					{
						this.textBox3.Text = "0";
					}
					bool flag7 = match9.Groups[1].Value == "" || match9.Groups[1].Value == null;
					if (flag7)
					{
						this.textBox4.Text = "0";
					}
					bool flag8 = match10.Groups[1].Value == "" || match10.Groups[1].Value == null;
					if (flag8)
					{
						this.textBox5.Text = "0";
					}
					价格计算.hjPrice = Convert.ToDouble(this.textBox2.Text);
					价格计算.byPrice = Convert.ToDouble(this.textBox3.Text);
					价格计算.bojPrice = Convert.ToDouble(this.textBox4.Text);
					价格计算.bajPrice = Convert.ToDouble(this.textBox5.Text);
					价格计算.time = match6.Groups[1].Value;
				}

			}
			catch (Exception)
            {

                ;
            }
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000058FC File Offset: 0x00003AFC
		private void 价格计算_Load(object sender, EventArgs e)
		{
			foreach (object obj in this.groupBox2.Controls)
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
						control.Text = text.Trim();
						streamReader.Close();
					}
				}
				bool flag3 = control is ComboBox;
				if (flag3)
				{
					string str2 = AppDomain.CurrentDomain.BaseDirectory + "value\\";
					bool flag4 = File.Exists(str2 + control.Name + ".txt");
					if (flag4)
					{
						StreamReader streamReader2 = new StreamReader(str2 + control.Name + ".txt", Encoding.GetEncoding("utf-8"));
						string text2 = streamReader2.ReadToEnd();
						control.Text = text2.Trim();
						streamReader2.Close();
					}
				}
			}
			foreach (object obj2 in this.groupBox1.Controls)
			{
				Control control2 = (Control)obj2;
				bool flag5 = control2 is TextBox;
				if (flag5)
				{
					string str3 = AppDomain.CurrentDomain.BaseDirectory + "value\\";
					bool flag6 = File.Exists(str3 + control2.Name + ".txt");
					if (flag6)
					{
						StreamReader streamReader3 = new StreamReader(str3 + control2.Name + ".txt", Encoding.GetEncoding("utf-8"));
						string text3 = streamReader3.ReadToEnd();
						control2.Text = text3.Trim();
						streamReader3.Close();
					}
				}
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00005B68 File Offset: 0x00003D68
		private void button1_Click(object sender, EventArgs e)
		{
			string url = 价格计算.GetUrl("http://www.acaiji.com/index/index/vip.html");
			bool flag = url.Contains("jiagejisuan");
			if (flag)
			{
				this.getPrice();
				this.timer1.Start();
				this.timer1.Interval = Convert.ToInt32(this.textBox6.Text) * 1000;
			}
			else
			{
				MessageBox.Show("验证失败");
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00005BD7 File Offset: 0x00003DD7
		private void 价格计算_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00005BDC File Offset: 0x00003DDC
		public void jisuan()
		{
			try
			{
				bool flag = this.comboBox1.Text.Trim() == "黄金";
				if (flag)
				{
					价格计算.price1 = 价格计算.hjPrice;
				}
				else
				{
					bool flag2 = this.comboBox1.Text.Trim() == "白银";
					if (flag2)
					{
						价格计算.price1 = 价格计算.byPrice;
					}
					else
					{
						bool flag3 = this.comboBox1.Text.Trim() == "铂金";
						if (flag3)
						{
							价格计算.price1 = 价格计算.bojPrice;
						}
						else
						{
							bool flag4 = this.comboBox1.Text.Trim() == "钯金";
							if (flag4)
							{
								价格计算.price1 = 价格计算.bajPrice;
							}
						}
					}
				}
				bool flag5 = this.comboBox2.Text.Trim() == "黄金";
				if (flag5)
				{
					价格计算.price2 = 价格计算.hjPrice;
				}
				else
				{
					bool flag6 = this.comboBox2.Text.Trim() == "白银";
					if (flag6)
					{
						价格计算.price2 = 价格计算.byPrice;
					}
					else
					{
						bool flag7 = this.comboBox2.Text.Trim() == "铂金";
						if (flag7)
						{
							价格计算.price2 = 价格计算.bojPrice;
						}
						else
						{
							bool flag8 = this.comboBox2.Text.Trim() == "钯金";
							if (flag8)
							{
								价格计算.price2 = 价格计算.bajPrice;
							}
						}
					}
				}
				bool flag9 = this.comboBox3.Text.Trim() == "黄金";
				if (flag9)
				{
					价格计算.price3 = 价格计算.hjPrice;
				}
				else
				{
					bool flag10 = this.comboBox3.Text.Trim() == "白银";
					if (flag10)
					{
						价格计算.price3 = 价格计算.byPrice;
					}
					else
					{
						bool flag11 = this.comboBox3.Text.Trim() == "铂金";
						if (flag11)
						{
							价格计算.price3 = 价格计算.bojPrice;
						}
						else
						{
							bool flag12 = this.comboBox3.Text.Trim() == "钯金";
							if (flag12)
							{
								价格计算.price3 = 价格计算.bajPrice;
							}
						}
					}
				}
				bool flag13 = this.comboBox4.Text.Trim() == "黄金";
				if (flag13)
				{
					价格计算.price4 = 价格计算.hjPrice;
				}
				else
				{
					bool flag14 = this.comboBox4.Text.Trim() == "白银";
					if (flag14)
					{
						价格计算.price4 = 价格计算.byPrice;
					}
					else
					{
						bool flag15 = this.comboBox4.Text.Trim() == "铂金";
						if (flag15)
						{
							价格计算.price4 = 价格计算.bojPrice;
						}
						else
						{
							bool flag16 = this.comboBox4.Text.Trim() == "钯金";
							if (flag16)
							{
								价格计算.price4 = 价格计算.bajPrice;
							}
						}
					}
				}
				bool flag17 = this.comboBox5.Text.Trim() == "黄金";
				if (flag17)
				{
					价格计算.price5 = 价格计算.hjPrice;
				}
				else
				{
					bool flag18 = this.comboBox5.Text.Trim() == "白银";
					if (flag18)
					{
						价格计算.price5 = 价格计算.byPrice;
					}
					else
					{
						bool flag19 = this.comboBox5.Text.Trim() == "铂金";
						if (flag19)
						{
							价格计算.price5 = 价格计算.bojPrice;
						}
						else
						{
							bool flag20 = this.comboBox5.Text.Trim() == "钯金";
							if (flag20)
							{
								价格计算.price5 = 价格计算.bajPrice;
							}
						}
					}
				}
				bool flag21 = this.comboBox6.Text.Trim() == "黄金";
				if (flag21)
				{
					价格计算.price6 = 价格计算.hjPrice;
				}
				else
				{
					bool flag22 = this.comboBox6.Text.Trim() == "白银";
					if (flag22)
					{
						价格计算.price6 = 价格计算.byPrice;
					}
					else
					{
						bool flag23 = this.comboBox6.Text.Trim() == "铂金";
						if (flag23)
						{
							价格计算.price6 = 价格计算.bojPrice;
						}
						else
						{
							bool flag24 = this.comboBox6.Text.Trim() == "钯金";
							if (flag24)
							{
								价格计算.price6 = 价格计算.bajPrice;
							}
						}
					}
				}
				double num = (价格计算.price1 + Convert.ToDouble(this.textBox7.Text.Trim())) * Convert.ToDouble(this.textBox8.Text.Trim()) + Convert.ToDouble(this.textBox9.Text.Trim());
				double num2 = (价格计算.price2 + Convert.ToDouble(this.textBox15.Text.Trim())) * Convert.ToDouble(this.textBox14.Text.Trim()) + Convert.ToDouble(this.textBox13.Text.Trim());
				double num3 = (价格计算.price3 + Convert.ToDouble(this.textBox20.Text.Trim())) * Convert.ToDouble(this.textBox19.Text.Trim()) + Convert.ToDouble(this.textBox18.Text.Trim());
				double num4 = (价格计算.price4 + Convert.ToDouble(this.textBox24.Text.Trim())) * Convert.ToDouble(this.textBox23.Text.Trim()) + Convert.ToDouble(this.textBox22.Text.Trim());
				double num5 = (价格计算.price5 + Convert.ToDouble(this.textBox28.Text.Trim())) * Convert.ToDouble(this.textBox27.Text.Trim()) + Convert.ToDouble(this.textBox26.Text.Trim());
				double num6 = (价格计算.price6 + Convert.ToDouble(this.textBox32.Text.Trim())) * Convert.ToDouble(this.textBox31.Text.Trim()) + Convert.ToDouble(this.textBox30.Text.Trim());
				bool flag25 = num > Convert.ToDouble(this.textBox10.Text);
				if (flag25)
				{
					价格计算.jieguo1 = num;
				}
				else
				{
					价格计算.jieguo1 = Convert.ToDouble(this.textBox10.Text);
				}
				bool flag26 = num2 > Convert.ToDouble(this.textBox11.Text);
				if (flag26)
				{
					价格计算.jieguo2 = num2;
				}
				else
				{
					价格计算.jieguo2 = Convert.ToDouble(this.textBox11.Text);
				}
				bool flag27 = num3 > Convert.ToDouble(this.textBox17.Text);
				if (flag27)
				{
					价格计算.jieguo3 = num3;
				}
				else
				{
					价格计算.jieguo3 = Convert.ToDouble(this.textBox17.Text);
				}
				bool flag28 = num4 > Convert.ToDouble(this.textBox21.Text);
				if (flag28)
				{
					价格计算.jieguo4 = num4;
				}
				else
				{
					价格计算.jieguo4 = Convert.ToDouble(this.textBox21.Text);
				}
				bool flag29 = num5 > Convert.ToDouble(this.textBox25.Text);
				if (flag29)
				{
					价格计算.jieguo5 = num5;
				}
				else
				{
					价格计算.jieguo5 = Convert.ToDouble(this.textBox25.Text);
				}
				bool flag30 = num6 > Convert.ToDouble(this.textBox29.Text);
				if (flag30)
				{
					价格计算.jieguo6 = num6;
				}
				else
				{
					价格计算.jieguo6 = Convert.ToDouble(this.textBox29.Text);
				}
				价格计算.jieguo1 = Math.Round(价格计算.jieguo1, Convert.ToInt32(this.textBox34.Text.Trim()));
				价格计算.jieguo2 = Math.Round(价格计算.jieguo2, Convert.ToInt32(this.textBox35.Text.Trim()));
				价格计算.jieguo3 = Math.Round(价格计算.jieguo3, Convert.ToInt32(this.textBox37.Text.Trim()));
				价格计算.jieguo4 = Math.Round(价格计算.jieguo4, Convert.ToInt32(this.textBox38.Text.Trim()));
				价格计算.jieguo5 = Math.Round(价格计算.jieguo5, Convert.ToInt32(this.textBox39.Text.Trim()));
				价格计算.jieguo6 = Math.Round(价格计算.jieguo6, Convert.ToInt32(this.textBox36.Text.Trim()));
			}
			catch (Exception ex)
			{
				ex.ToString();
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00006458 File Offset: 0x00004658
		private void button2_Click(object sender, EventArgs e)
		{
			价格计算.value1 = this.textBox12.Text;
			价格计算.value2 = this.textBox16.Text;
			价格计算.value3 = this.textBox33.Text;
			this.timer2.Start();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00006497 File Offset: 0x00004697
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.getPrice();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000064A1 File Offset: 0x000046A1
		private void timer2_Tick(object sender, EventArgs e)
		{
			this.jisuan();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000064AC File Offset: 0x000046AC
		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			foreach (object obj in this.groupBox2.Controls)
			{
				Control control = (Control)obj;
				bool flag = control is TextBox;
				if (flag)
				{
					string str = AppDomain.CurrentDomain.BaseDirectory + "value\\";
					FileStream fileStream = new FileStream(str + control.Name + ".txt", FileMode.Create, FileAccess.Write);
					StreamWriter streamWriter = new StreamWriter(fileStream);
					streamWriter.WriteLine(control.Text.Trim());
					streamWriter.Close();
					fileStream.Close();
				}
				bool flag2 = control is ComboBox;
				if (flag2)
				{
					string str2 = AppDomain.CurrentDomain.BaseDirectory + "value\\";
					FileStream fileStream2 = new FileStream(str2 + control.Name + ".txt", FileMode.Create, FileAccess.Write);
					StreamWriter streamWriter2 = new StreamWriter(fileStream2);
					streamWriter2.WriteLine(control.Text.Trim());
					streamWriter2.Close();
					fileStream2.Close();
				}
			}
			foreach (object obj2 in this.groupBox1.Controls)
			{
				Control control2 = (Control)obj2;
				bool flag3 = control2 is TextBox;
				if (flag3)
				{
					string str3 = AppDomain.CurrentDomain.BaseDirectory + "value\\";
					FileStream fileStream3 = new FileStream(str3 + control2.Name + ".txt", FileMode.Create, FileAccess.Write);
					StreamWriter streamWriter3 = new StreamWriter(fileStream3);
					streamWriter3.WriteLine(control2.Text.Trim());
					streamWriter3.Close();
					fileStream3.Close();
				}
			}
			base.Hide();
		}

		// Token: 0x04000053 RID: 83
		public static string time;

		// Token: 0x04000054 RID: 84
		public static double hjPrice;

		// Token: 0x04000055 RID: 85
		public static double byPrice;

		// Token: 0x04000056 RID: 86
		public static double bojPrice;

		// Token: 0x04000057 RID: 87
		public static double bajPrice;

		// Token: 0x04000058 RID: 88
		public static double price1;

		// Token: 0x04000059 RID: 89
		public static double price2;

		// Token: 0x0400005A RID: 90
		public static double price3;

		// Token: 0x0400005B RID: 91
		public static double price4;

		// Token: 0x0400005C RID: 92
		public static double price5;

		// Token: 0x0400005D RID: 93
		public static double price6;

		// Token: 0x0400005E RID: 94
		public static double jieguo1;

		// Token: 0x0400005F RID: 95
		public static double jieguo2;

		// Token: 0x04000060 RID: 96
		public static double jieguo3;

		// Token: 0x04000061 RID: 97
		public static double jieguo4;

		// Token: 0x04000062 RID: 98
		public static double jieguo5;

		// Token: 0x04000063 RID: 99
		public static double jieguo6;

		// Token: 0x04000064 RID: 100
		public static string value1 = "金 料";

		// Token: 0x04000065 RID: 101
		public static string value2 = "成 品";

		// Token: 0x04000066 RID: 102
		public static string value3 = "零 售";
	}
}
