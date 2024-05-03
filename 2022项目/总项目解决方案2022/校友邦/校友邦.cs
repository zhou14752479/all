using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using myDLL;

namespace 校友邦
{
	// Token: 0x02000006 RID: 6
	public partial class 校友邦 : Form
	{
		// Token: 0x0600001D RID: 29 RVA: 0x0000483C File Offset: 0x00002A3C
		public 校友邦()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600001E RID: 30
		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

		// Token: 0x0600001F RID: 31
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

		// Token: 0x06000020 RID: 32 RVA: 0x0000489B File Offset: 0x00002A9B
		public void IniWriteValue(string Section, string Key, string Value)
		{
			校友邦.WritePrivateProfileString(Section, Key, Value, this.inipath);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000048B0 File Offset: 0x00002AB0
		public string IniReadValue(string Section, string Key)
		{
			StringBuilder stringBuilder = new StringBuilder(500);
			int privateProfileString = 校友邦.GetPrivateProfileString(Section, Key, "", stringBuilder, 500, this.inipath);
			return stringBuilder.ToString();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000048EC File Offset: 0x00002AEC
		public bool ExistINIFile()
		{
			return File.Exists(this.inipath);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000490C File Offset: 0x00002B0C
		public void jiance()
		{
			bool flag = this.ExistINIFile();
			if (flag)
			{
				string a = this.IniReadValue("values", "key");
				string text = this.IniReadValue("values", "secret");
				string[] array = text.Split(new string[]
				{
					"asd147"
				}, StringSplitOptions.None);
				bool flag2 = Convert.ToInt32(array[1]) < Convert.ToInt32(method.GetTimeStamp());
				if (flag2)
				{
					MessageBox.Show("激活已过期");
					string text2 = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", method.GetMD5(method.GetMacAddress()), -1, -1);
					string[] array2 = text2.Split(new string[]
					{
						"asd"
					}, StringSplitOptions.None);
					bool flag3 = array2[0] == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian");
					if (flag3)
					{
						this.IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
						this.IniWriteValue("values", "secret", text2);
						MessageBox.Show("激活成功");
					}
					else
					{
						MessageBox.Show("激活码错误");
						Process.GetCurrentProcess().Kill();
					}
				}
				else
				{
					bool flag4 = array[0] != method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian") || a != method.GetMD5(method.GetMacAddress());
					if (flag4)
					{
						string text3 = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", method.GetMD5(method.GetMacAddress()), -1, -1);
						string[] array3 = text3.Split(new string[]
						{
							"asd147"
						}, StringSplitOptions.None);
						bool flag5 = array3[0] == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian");
						if (flag5)
						{
							this.IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
							this.IniWriteValue("values", "secret", text3);
							MessageBox.Show("激活成功");
						}
						else
						{
							MessageBox.Show("激活码错误");
							Process.GetCurrentProcess().Kill();
						}
					}
				}
			}
			else
			{
				string text4 = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", method.GetMD5(method.GetMacAddress()), -1, -1);
				string[] array4 = text4.Split(new string[]
				{
					"asd147"
				}, StringSplitOptions.None);
				bool flag6 = array4[0] == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian");
				if (flag6)
				{
					this.IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
					this.IniWriteValue("values", "secret", text4);
					MessageBox.Show("激活成功");
				}
				else
				{
					MessageBox.Show("激活码错误");
					Process.GetCurrentProcess().Kill();
				}
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00004BF7 File Offset: 0x00002DF7
		private void 校友邦_Load(object sender, EventArgs e)
		{
			this.jiance();
			this.webBrowser1.Navigate(this.path + "static/index.html");
			method.SetFeatures(11000U);
			this.webBrowser1.ScriptErrorsSuppressed = true;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00004C38 File Offset: 0x00002E38
		private void button1_Click(object sender, EventArgs e)
		{
			this.listView1.Items.Clear();
			OpenFileDialog openFileDialog = new OpenFileDialog();
			bool flag = openFileDialog.ShowDialog() == DialogResult.OK;
			if (flag)
			{
				this.textBox1.Text = openFileDialog.FileName;
				StreamReader streamReader = new StreamReader(openFileDialog.FileName, method.EncodingType.GetTxtType(openFileDialog.FileName));
				string text = streamReader.ReadToEnd();
				string[] array = text.Split(new string[]
				{
					"\r\n"
				}, StringSplitOptions.None);
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Split(new string[]
					{
						"#"
					}, StringSplitOptions.None);
					bool flag2 = array2.Length > 2;
					if (flag2)
					{
						ListViewItem listViewItem = this.listView1.Items.Add(array2[0]);
						listViewItem.SubItems.Add(array2[1]);
						listViewItem.SubItems.Add(array2[2]);
						listViewItem.SubItems.Add(array2[3]);
						listViewItem.SubItems.Add(array2[4]);
						listViewItem.SubItems.Add(array2[5]);
						listViewItem.SubItems.Add("");
					}
				}
				streamReader.Close();
				streamReader.Dispose();
			}
			for (int j = 0; j < this.listView1.Items.Count; j++)
			{
				bool flag3 = (Convert.ToDateTime(this.listView1.Items[j].SubItems[5].Text) - DateTime.Now).Days >= 3;
				if (flag3)
				{
					this.listView1.Items[j].BackColor = Color.Green;
				}
				else
				{
					this.listView1.Items[j].BackColor = Color.Red;
				}
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00004E40 File Offset: 0x00003040
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
			string input = method.PostUrl(url, postData, "", "utf-8", "application/x-www-form-urlencoded", "");
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

		// Token: 0x06000027 RID: 39 RVA: 0x00004EF4 File Offset: 0x000030F4
		public string getplanid(string cookie)
		{
			string url = "https://xcx.xybsyw.com/student/progress/ProjectList.action";
			string postData = "";
			string input = method.PostUrl(url, postData, cookie, "utf-8", "application/x-www-form-urlencoded", "");
			return Regex.Match(input, "\"planId\":([\\s\\S]*?),").Groups[1].Value;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00004F4C File Offset: 0x0000314C
		public string gettraineeId(string planid, string cookie)
		{
			string url = "https://xcx.xybsyw.com/student/clock/GetPlan!getDefault.action";
			string postData = "";
			string input = method.PostUrl(url, postData, cookie, "utf-8", "application/x-www-form-urlencoded", "");
			return Regex.Match(input, "\"traineeId\":([\\s\\S]*?)}").Groups[1].Value;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00004FA4 File Offset: 0x000031A4
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

		// Token: 0x0600002A RID: 42 RVA: 0x00005010 File Offset: 0x00003210
		public string qiandao(string cookie, string addr, string traineeId)
		{
			string result;
			try
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
					string url3 = myDLL.method.GetUrl(url2, "utf-8");
					text2 = Regex.Match(url3, "lat\":([\\s\\S]*?)}").Groups[1].Value.Trim();
					text3 = Regex.Match(url3, "lng\":([\\s\\S]*?),").Groups[1].Value.Trim();
				}
				string text4 = this.getadcode(text3, text2);
				string url4 = "https://xcx.xybsyw.com/student/clock/PostNew.action";
				string postData2 = string.Concat(new string[]
				{
					"model=microsoft&brand=microsoft&platform=windows&traineeId=",
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
				bool flag2 = text.Contains("%");
				if (flag2)
				{
					text = HttpUtility.UrlDecode(text);
				}
				校友邦.Encrypt method = new 校友邦.Encrypt(this.getencrypt);
				IAsyncResult asyncResult = base.BeginInvoke(method, new object[]
				{
					traineeId,
					text4,
					text2,
					text3,
					text,
					this.status
				});
				string text5 = base.EndInvoke(asyncResult).ToString();
				string[] array = text5.Split(new string[]
				{
					","
				}, StringSplitOptions.None);
				string t = array[0];
				string s = array[1];
				string m = array[2];
				string input2 = jiamipost.PostUrl(url4, postData2, cookie, m, t, s);
				MatchCollection matchCollection = Regex.Matches(input2, "\"msg\":\"([\\s\\S]*?)\"");
				string value = matchCollection[matchCollection.Count - 1].Groups[1].Value;
				bool flag3 = value != "";
				if (flag3)
				{
					result = value;
				}
				else
				{
					result = "failed";
				}
			}
			catch (Exception ex)
			{
				result = "failed";
			}
			return result;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000052C8 File Offset: 0x000034C8
		public string getencrypt(string traineeId, string adcode, string lat, string lng, string address, string clockStatus)
		{
			return this.webBrowser1.Document.InvokeScript("getdata", new object[]
			{
				traineeId,
				adcode,
				lat,
				lng,
				address,
				clockStatus
			}).ToString();
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00005318 File Offset: 0x00003518
		public string chongxinqiandao(string cookie, string addr, string traineeId)
		{
			string url = "https://xcx.xybsyw.com/student/clock/GetPlan!detail.action";
			string postData = "traineeId=" + traineeId;
			string input = method.PostUrl(url, postData, cookie, "utf-8", "application/x-www-form-urlencoded", "");
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
			string url4 = "https://xcx.xybsyw.com/student/clock/Post!updateClock.action";
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
			string input2 = method.PostUrl(url4, postData2, cookie, "utf-8", "application/x-www-form-urlencoded", "");
			string value = Regex.Match(input2, "\"msg\":\"([\\s\\S]*?)\"").Groups[1].Value;
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

		// Token: 0x0600002D RID: 45 RVA: 0x0000550C File Offset: 0x0000370C
		public void run()
		{
			bool flag = this.listView1.CheckedItems.Count == 0;
			if (flag)
			{
				MessageBox.Show("请勾选需要签到的数据行");
			}


			else
			{

				if (DateTime.Now > Convert.ToDateTime("2024-06-01"))
				{
					Process.GetCurrentProcess().Kill();
				}
				int i = 0;
				while (i < this.listView1.CheckedItems.Count)
				{
					try
					{
						string text = this.listView1.CheckedItems[i].SubItems[1].Text;
						string text2 = this.listView1.CheckedItems[i].SubItems[2].Text;
						string text3 = this.listView1.CheckedItems[i].SubItems[3].Text;
						string text4 = this.login(text, text2);
						bool flag2 = text4 == "";
						if (flag2)
						{
							this.listView1.Items[i].SubItems[6].Text = "登陆失败";
						}
						else
						{
							string planid = this.getplanid(text4);
							string traineeId = this.gettraineeId(planid, text4);
							string text5 = this.qiandao(text4, text3, traineeId);
							this.listView1.CheckedItems[i].SubItems[6].Text = text5;
							Thread.Sleep(1000);
							while (!this.zanting)
							{
								Application.DoEvents();
							}
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
					}
					IL_167:
					i++;
					continue;
					goto IL_167;
				}
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000056BC File Offset: 0x000038BC
		public void chongxin_run()
		{
			bool flag = this.listView1.CheckedItems.Count == 0;
			if (flag)
			{
				MessageBox.Show("请勾选需要签到的数据行");
			}
			else
			{
				int i = 0;
				while (i < this.listView1.CheckedItems.Count)
				{
					try
					{
						string text = this.listView1.CheckedItems[i].SubItems[1].Text;
						string text2 = this.listView1.CheckedItems[i].SubItems[2].Text;
						string text3 = this.listView1.CheckedItems[i].SubItems[3].Text;
						string text4 = this.login(text, text2);
						bool flag2 = text4 == "";
						if (flag2)
						{
							this.listView1.Items[i].SubItems[6].Text = "登陆失败";
						}
						else
						{
							string planid = this.getplanid(text4);
							string traineeId = this.gettraineeId(planid, text4);
							string text5 = this.chongxinqiandao(text4, text3, traineeId);
							this.listView1.CheckedItems[i].SubItems[6].Text = text5;
							Thread.Sleep(1000);
							while (!this.zanting)
							{
								Application.DoEvents();
							}
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
					}
					IL_167:
					i++;
					continue;
					goto IL_167;
				}
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000586C File Offset: 0x00003A6C
		private void button2_Click(object sender, EventArgs e)
		{
			this.status = "2";
			bool flag = this.thread == null || !this.thread.IsAlive;
			if (flag)
			{
				this.thread = new Thread(new ThreadStart(this.run));
				this.thread.Start();
				Control.CheckForIllegalCrossThreadCalls = false;
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000058D0 File Offset: 0x00003AD0
		private void button3_Click(object sender, EventArgs e)
		{
			bool flag = !this.zanting;
			if (flag)
			{
				this.zanting = true;
			}
			else
			{
				this.zanting = false;
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00005900 File Offset: 0x00003B00
		private void button4_Click(object sender, EventArgs e)
		{
			string url = method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8");
			bool flag = !url.Contains("QXTAe");
			if (!flag)
			{
				this.status = "2";
				bool flag2 = this.thread == null || !this.thread.IsAlive;
				if (flag2)
				{
					this.thread = new Thread(new ThreadStart(this.chongxin_run));
					this.thread.Start();
					Control.CheckForIllegalCrossThreadCalls = false;
				}
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00005988 File Offset: 0x00003B88
		private void button5_Click(object sender, EventArgs e)
		{
			this.status = "1";
			bool flag = this.thread == null || !this.thread.IsAlive;
			if (flag)
			{
				this.thread = new Thread(new ThreadStart(this.run));
				this.thread.Start();
				Control.CheckForIllegalCrossThreadCalls = false;
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000059EC File Offset: 0x00003BEC
		private void button6_Click(object sender, EventArgs e)
		{
			this.status = "1";
			bool flag = this.thread == null || !this.thread.IsAlive;
			if (flag)
			{
				this.thread = new Thread(new ThreadStart(this.chongxin_run));
				this.thread.Start();
				Control.CheckForIllegalCrossThreadCalls = false;
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00005A50 File Offset: 0x00003C50
		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			for (int i = 0; i < this.listView1.Items.Count; i++)
			{
				this.listView1.Items[i].Checked = true;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00005A98 File Offset: 0x00003C98
		private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			for (int i = 0; i < this.listView1.Items.Count; i++)
			{
				this.listView1.Items[i].Checked = false;
			}
		}

		// Token: 0x04000023 RID: 35
		private string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";

		// Token: 0x04000024 RID: 36
		private string path = AppDomain.CurrentDomain.BaseDirectory;

		// Token: 0x04000025 RID: 37
		private Thread thread;

		// Token: 0x04000026 RID: 38
		private bool zanting = true;

		// Token: 0x04000027 RID: 39
		private string status = "2";

		// Token: 0x0200000A RID: 10
		// (Invoke) Token: 0x06000044 RID: 68
		private delegate string Encrypt(string traineeId, string adcode, string lat, string lng, string address, string clockStatus);
	}
}
