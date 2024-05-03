using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using Microsoft.VisualBasic.Logging;
using myDLL;

namespace 校友邦
{
	// Token: 0x02000005 RID: 5
	public partial class 校友邦_定时签到 : Form
	{
		// Token: 0x0600000E RID: 14 RVA: 0x000028A4 File Offset: 0x00000AA4
		public 校友邦_定时签到()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000028F4 File Offset: 0x00000AF4
		private static int DateDiff(DateTime dateStart, DateTime dateEnd)
		{
			DateTime d = Convert.ToDateTime(dateStart.ToShortDateString());
			DateTime d2 = Convert.ToDateTime(dateEnd.ToShortDateString());
			return (d2 - d).Days;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002930 File Offset: 0x00000B30
		public void getdata()
		{
			this.listView1.Items.Clear();
			StreamReader streamReader = new StreamReader(this.path + "data.txt", method.EncodingType.GetTxtType(this.path + "data.txt"));
			string text = streamReader.ReadToEnd();
			string[] array = text.Split(new string[]
			{
				"\n"
			}, StringSplitOptions.None);
			int i = 0;
			while (i < array.Length)
			{
				string[] array2 = array[i].Split(new string[]
				{
					"#"
				}, StringSplitOptions.None);
				ListViewItem listViewItem = this.listView1.Items.Add(array2[0].Trim());
				try
				{
					this.listView1.Items[i].BackColor = Color.White;
					bool flag = array2.Length > 2;
					if (flag)
					{
						listViewItem.SubItems.Add(array2[1].Trim());
						listViewItem.SubItems.Add(array2[2].Trim());
						listViewItem.SubItems.Add(array2[3].Trim());
						listViewItem.SubItems.Add(array2[4].Trim());
						listViewItem.SubItems.Add(array2[5].Trim());
						listViewItem.SubItems.Add(array2[6].Trim());
						listViewItem.SubItems.Add("");
						listViewItem.SubItems.Add("");
						int num = 校友邦_定时签到.DateDiff(DateTime.Now, Convert.ToDateTime(array2[6].Trim()));
						listViewItem.SubItems.Add(num.ToString());
						bool flag2 = array2.Length > 7;
						if (flag2)
						{
							listViewItem.SubItems.Add(array2[7].Trim());
						}
						else
						{
							listViewItem.SubItems.Add("");
						}
					}
				}
				catch (Exception ex)
				{
					listViewItem.BackColor = Color.Red;
					this.textBox2.Text = DateTime.Now.ToString() + ex.ToString();
				}
				IL_212:
				i++;
				continue;
				goto IL_212;
			}
			streamReader.Close();
			streamReader.Dispose();
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002B8C File Offset: 0x00000D8C
		public void getpics()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(this.path + "//images");
			for (int i = 0; i < directoryInfo.GetFiles().Count<FileInfo>(); i++)
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(directoryInfo.GetFiles()[i].Name);
				this.pics.Add(fileNameWithoutExtension);
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002BEC File Offset: 0x00000DEC
		private void 校友邦_定时签到_Load(object sender, EventArgs e)
		{
		
			this.webBrowser1.Navigate(this.path + "static/index.html");
			method.SetFeatures(11000U);
			this.webBrowser1.ScriptErrorsSuppressed = true;
			this.getdata();
			this.getpics();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002C6C File Offset: 0x00000E6C
		public void run()
		{

		
			if (DateTime.Now>Convert.ToDateTime("2024-06-01"))
			{
				Process.GetCurrentProcess().Kill();
			}


			this.getpics();
			bool flag = DateTime.Now.Hour == 2 && this.refresh == 0;
			if (flag)
			{
				this.refresh = 1;
				this.getdata();
			}
			bool flag2 = DateTime.Now.Hour == 3;
			if (flag2)
			{
				this.refresh = 0;
			}
			this.label1.Text = DateTime.Now.ToString() + "：开始启动签到.....";
			int i = 0;
			while (i < this.listView1.Items.Count)
			{
				try
				{
					string text = this.listView1.Items[i].SubItems[1].Text;
					string text2 = this.listView1.Items[i].SubItems[2].Text;
					string text3 = this.listView1.Items[i].SubItems[3].Text;
					string text4 = this.listView1.Items[i].SubItems[4].Text;
					string text5 = this.listView1.Items[i].SubItems[5].Text;
					string text6 = this.listView1.Items[i].SubItems[6].Text;
					string text7 = this.listView1.Items[i].SubItems[7].Text;
					string text8 = this.listView1.Items[i].SubItems[8].Text;
					string text9 = this.listView1.Items[i].SubItems[10].Text.Trim();
					bool flag3 = DateTime.Now > Convert.ToDateTime(text6).AddDays(1.0);
					if (flag3)
					{
						this.listView1.Items[i].SubItems[7].Text = "日期超出，不签到";
					}
					else
					{
						bool flag4 = DateTime.Now < Convert.ToDateTime(text5);
						if (flag4)
						{
							this.listView1.Items[i].SubItems[7].Text = "日期未到，不签到";
						}
						else
						{
							string[] array = text4.Split(new string[]
							{
								","
							}, StringSplitOptions.None);
							bool flag5 = text9 != "";
							if (flag5)
							{
								bool flag6 = !text9.Contains(Convert.ToInt32(DateTime.Now.DayOfWeek).ToString());
								if (flag6)
								{
									this.listView1.Items[i].SubItems[7].Text = "指定星期不符合";
									goto IL_90C;
								}
							}

                            //if (array[4] == "true") //周末是否签到
                            if (array[4] == "1") //周末是否签到
                            {

                                //if (array[5] == "dan")  //单双周签到
                                if (array[5] == "1")
                                {
									bool flag9 = Convert.ToInt32(DateTime.Now.DayOfWeek) == 2 || Convert.ToInt32(DateTime.Now.DayOfWeek) == 4 || Convert.ToInt32(DateTime.Now.DayOfWeek) == 6 || Convert.ToInt32(DateTime.Now.DayOfWeek) == 0;
									if (flag9)
									{
										this.listView1.Items[i].SubItems[7].Text = "非周一三五不签到";
										goto IL_90C;
									}
								}

                                //if (array[5] == "shuang")
                                if (array[5] == "2")
                                {
									bool flag11 = Convert.ToInt32(DateTime.Now.DayOfWeek) == 1 || Convert.ToInt32(DateTime.Now.DayOfWeek) == 3 || Convert.ToInt32(DateTime.Now.DayOfWeek) == 5;
									if (flag11)
									{
										this.listView1.Items[i].SubItems[7].Text = "非周二四六不签到";
										goto IL_90C;
									}
								}
							}
							else
							{
								bool flag12 = Convert.ToInt32(DateTime.Now.DayOfWeek) == 6 || Convert.ToInt32(DateTime.Now.DayOfWeek) == 0;
								if (flag12)
								{
									this.listView1.Items[i].SubItems[7].Text = "非工作日不签到";
									goto IL_90C;
								}
							}
							bool flag13 = DateTime.Now.Hour > 15;
							if (flag13)
							{
								this.fc.status = "1";
								bool flag14 = array[2] == "无" || array[3] == "无";
								if (flag14)
								{
									this.listView1.Items[i].SubItems[8].Text = "不需要结束签到";
									goto IL_90C;
								}
								bool flag15 = DateTime.Now < Convert.ToDateTime(array[2]) || DateTime.Now > Convert.ToDateTime(array[3]);
								if (flag15)
								{
									bool flag16 = !text8.Contains("success");
									if (flag16)
									{
										this.listView1.Items[i].SubItems[8].Text = "结束签到时间未满足";
										goto IL_90C;
									}
								}
							}
							else
							{
								this.listView1.Items[i].SubItems[8].Text = "结束签到时间未满足";
								this.fc.status = "2";
								bool flag17 = DateTime.Now < Convert.ToDateTime(array[0]) || DateTime.Now > Convert.ToDateTime(array[1]);
								if (flag17)
								{
									bool flag18 = !text7.Contains("success");
									if (flag18)
									{
										this.listView1.Items[i].SubItems[7].Text = "开始签到时间未满足";
									}
									goto IL_90C;
								}
							}
							string text10 = this.fc.login(text, text2);
							
							bool flag19 = text10 == "";
							if (flag19)
							{
								this.listView1.Items[i].SubItems[7].Text = "登陆失败";
							}
							else
							{
								string planid = this.fc.getplanid(text10);
								string traineeId = this.fc.gettraineeId(planid, text10);
								string str = "";
								bool flag20 = this.fc.status == "2" && !text7.Contains("success");
								if (flag20)
								{
									bool flag21 = this.pics.Contains(text);
									if (flag21)
									{
										string myma = this.fc.getmyma(text10, traineeId);
										bool flag22 = this.listView1.Items[i].SubItems[0].Text.Contains("集中实习");
										if (flag22)
										{
											str = this.fc.shangchuanmaandtravelCodeImg(text10, myma);
										}
										else
										{
											str = this.fc.shangchuanma(text10, myma);
										}
									}
									string str2 = this.qiandao(text10, text3, traineeId);

									
									this.listView1.Items[i].SubItems[7].Text = str + "  " + str2;
								}
								bool flag23 = this.fc.status == "1" && !text8.Contains("success");
								if (flag23)
								{
									//bool flag24 = this.pics.Contains(text);
									//if (flag24)
									//{
									//	string myma2 = this.fc.getmyma(text10, traineeId);
									//	bool flag25 = this.listView1.Items[i].SubItems[0].Text.Contains("集中实习");
									//	if (flag25)
									//	{
									//		str = this.fc.shangchuanmaandtravelCodeImg(text10, myma2);
									//	}
									//	else
									//	{
									//		str = this.fc.shangchuanma(text10, myma2);
									//	}
									//}
									string str3 = this.qiandao(text10, text3, traineeId);
                                    
                                    this.listView1.Items[i].SubItems[8].Text = str + "  " + str3;
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					this.listView1.Items[i].BackColor = Color.Red;
					this.textBox2.Text = DateTime.Now.ToString() + ex.ToString();
				}
				IL_90C:
				i++;
				continue;
				goto IL_90C;
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000035C4 File Offset: 0x000017C4
		private void button1_Click(object sender, EventArgs e)
		{
			
			this.timer1.Interval = Convert.ToInt32(this.textBox1.Text) * 60 * 1000;
			this.timer1.Start();
			bool flag2 = this.thread == null || !this.thread.IsAlive;
			if (flag2)
			{
				this.thread = new Thread(new ThreadStart(this.run));
				this.thread.Start();
				Control.CheckForIllegalCrossThreadCalls = false;
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000367C File Offset: 0x0000187C
		private void timer1_Tick(object sender, EventArgs e)
		{
			bool flag = this.thread == null || !this.thread.IsAlive;
			if (flag)
			{
				this.thread = new Thread(new ThreadStart(this.run));
				this.thread.Start();
				Control.CheckForIllegalCrossThreadCalls = false;
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000036D4 File Offset: 0x000018D4
		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			List<string> list = new List<string>(File.ReadAllLines(this.path + "data.txt"));
			int i = 0;
			while (i < list.Count)
			{
				try
				{
					string[] array = list[i].Split(new string[]
					{
						"#"
					}, StringSplitOptions.None);
					string value = array[array.Length - 1];
					bool flag = Convert.ToDateTime(value) < DateTime.Now.AddDays(-1.0);
					if (flag)
					{
						list.RemoveAt(i);
					}
				}
				catch (Exception)
				{
				}
				IL_80:
				i++;
				continue;
				goto IL_80;
			}
			File.WriteAllLines(this.path + "data.txt", list.ToArray());
			this.getdata();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000037A8 File Offset: 0x000019A8
		private void 校友邦_定时签到_FormClosing(object sender, FormClosingEventArgs e)
		{
			DialogResult dialogResult = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
			bool flag = dialogResult == DialogResult.OK;
			if (flag)
			{
				Process.GetCurrentProcess().Kill();
			}
			else
			{
				e.Cancel = true;
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000037EC File Offset: 0x000019EC
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

		// Token: 0x06000019 RID: 25 RVA: 0x0000383B File Offset: 0x00001A3B
		private void button2_Click(object sender, EventArgs e)
		{
			this.timer1.Stop();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000384C File Offset: 0x00001A4C
		public string qiandao(string cookie, string addr, string traineeId)
		{
			string result;
			try
			{
				string url = "https://xcx.xybsyw.com/student/clock/GetPlan!detail.action";
				string postData = "traineeId=" + traineeId;
				string input = function.PostUrl(url, postData, cookie, "utf-8", "application/x-www-form-urlencoded", "");
				string text = Regex.Match(input, "\"address\":\"([\\s\\S]*?)\"").Groups[1].Value.Trim();
				string lat = Regex.Match(input, "\"lat\":([\\s\\S]*?),").Groups[1].Value.Trim();
				string lng = Regex.Match(input, "\"lng\":([\\s\\S]*?),").Groups[1].Value.Trim();
				bool flag = text.Trim() == "";
				if (flag)
				{
					text = HttpUtility.UrlEncode(addr);
					string url2 = "https://api.map.baidu.com/geocoding/v3/?address=" + text + "&output=json&ak=9DemeyQjUrIX14Fz8uEwVpGyKErUP4Sb&callback=showLocation";
					string url3 = myDLL.method.GetUrl(url2, "utf-8");
                    lat = Regex.Match(url3, "lat\":([\\s\\S]*?)}").Groups[1].Value.Trim();
                    lng = Regex.Match(url3, "lng\":([\\s\\S]*?),").Groups[1].Value.Trim();
				}
				string adcode = this.fc.getadcode(lng, lat);
				string url4 = "https://xcx.xybsyw.com/student/clock/PostNew.action";

				if(adcode == ""&& lat == "" && lng == "")
				{
                    adcode = "130110";
                    lat = "38.159145 ";
                    lng = "114.33003";
                }
				string postData2 = string.Concat(new string[]
				{
                    "model=iPhone%2013%3CiPhone14%2C5%3E&brand=iPhone&platform=ios&system=iOS%2016.1&traineeId=",
					traineeId,
                    
                    "&adcode=",
                    adcode,
					"&lat=",
                    lat,
					"&lng=",
                    lng,
					"&address=",
					text,
                    "&deviceName=iPhone%2013%3CiPhone14%2C5%3E&punchInStatus=1&clockStatus=",
					this.fc.status
				});
				//MessageBox.Show(postData2);
				bool flag2 = text.Contains("%");
				if (flag2)
				{
					text = HttpUtility.UrlDecode(text);
				}
				校友邦_定时签到.Encrypt method = new 校友邦_定时签到.Encrypt(this.getencrypt);
				IAsyncResult asyncResult = base.BeginInvoke(method, new object[]
				{
					traineeId,
                    adcode,
                    lat,
                    lng,
					text,
					this.fc.status
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
				this.textBox2.Text = ex.ToString();
				result = "failed";
			}
			return result;
		}

		// Token: 0x04000002 RID: 2
		private string path = AppDomain.CurrentDomain.BaseDirectory;

		// Token: 0x04000003 RID: 3
		private function fc = new function();

		// Token: 0x04000004 RID: 4
		private List<string> pics = new List<string>();

		// Token: 0x04000005 RID: 5
		private int refresh = 0;

		// Token: 0x04000006 RID: 6
		private Thread thread;

		// Token: 0x02000009 RID: 9
		// (Invoke) Token: 0x06000040 RID: 64
		private delegate string Encrypt(string traineeId, string adcode, string lat, string lng, string address, string clockStatus);
	}
}
