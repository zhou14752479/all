using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using myDLL;
using ProvinceCity;

namespace 地图营销
{
	// Token: 0x02000003 RID: 3
	public partial class 地图采集 : Form
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00002B48 File Offset: 0x00000D48
		public 地图采集()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002BAC File Offset: 0x00000DAC
		private void Form1_Load(object sender, EventArgs e)
		{
			

             ProvinceCity.ProvinceCity.BindProvince(this.comboBox2);
			this.tabControl1.Region = new Region(new RectangleF((float)this.tabPage1.Left, (float)this.tabPage1.Top, (float)this.tabPage1.Width, (float)this.tabPage1.Height));
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002C0C File Offset: 0x00000E0C
		public static string Unicode2String(string source)
		{
			return new Regex("\\\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(source, (Match x) => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)).ToString());
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002C50 File Offset: 0x00000E50
		public void baidu()
		{
			string[] array = this.textBox1.Text.Trim().Split(new string[]
			{
				"\r\n"
			}, StringSplitOptions.None);
			string[] array2 = this.textBox2.Text.Trim().Split(new string[]
			{
				"\r\n"
			}, StringSplitOptions.None);
			bool flag = array2.Length == 0;
			if (flag)
			{
				MessageBox.Show("请添加关键字");
			}
			else
			{
				try
				{
					foreach (string str in array)
					{
						foreach (string text in array2)
						{
							for (int k = 1; k < 100; k++)
							{
								string url = string.Concat(new object[]
								{
									"https://api.map.baidu.com/?qt=s&c=",
									HttpUtility.UrlEncode(str),
									"&wd=",
									HttpUtility.UrlEncode(text),
									"&rn=120&pn=",
									k,
									"&ie=utf-8&oue=1&fromproduct=jsapi&res=api&ak=nXZgaSQxzlRapagrvbD5CyfWETZ2tsIm"
								});
								string url2 = method.GetUrl(url, "utf-8");
								MatchCollection matchCollection = Regex.Matches(url2, "acc_flag([\\s\\S]*?)view_type");
								bool flag2 = matchCollection.Count == 0;
								if (flag2)
								{
									break;
								}
								for (int l = 0; l < matchCollection.Count; l++)
								{
									string value = Regex.Match(matchCollection[l].Groups[1].Value, "geo_type([\\s\\S]*?)name\":\"([\\s\\S]*?)\"").Groups[2].Value;
									string value2 = Regex.Match(matchCollection[l].Groups[1].Value, "\"tel\":\"([\\s\\S]*?)\"").Groups[1].Value;
									string value3 = Regex.Match(matchCollection[l].Groups[1].Value, "\"addr\":\"([\\s\\S]*?)\"").Groups[1].Value;
									string value4 = Regex.Match(matchCollection[l].Groups[1].Value, "\"city_name\":\"([\\s\\S]*?)\"").Groups[1].Value;
									string value5 = Regex.Match(matchCollection[l].Groups[1].Value, "diPointX\":([\\s\\S]*?),").Groups[1].Value;
									bool flag3 = this.shaixuan(value2) != "";
									if (flag3)
									{
										ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
										listViewItem.SubItems.Add(地图采集.Unicode2String(value));
										bool flag4 = !this.md.jihuo;
										if (flag4)
										{
											bool flag5 = value2.Length > 4;
											if (flag5)
											{
												listViewItem.SubItems.Add(value2.Substring(0, 4) + "*******");
											}
										}
										else
										{
											listViewItem.SubItems.Add(value2);
										}
										listViewItem.SubItems.Add(地图采集.Unicode2String(value3));
										listViewItem.SubItems.Add(地图采集.Unicode2String(value4));
										listViewItem.SubItems.Add(text);
										listViewItem.SubItems.Add(value5);
										bool flag6 = !this.status;
										if (flag6)
										{
											return;
										}
										this.count++;
										this.infolabel.Text = DateTime.Now.ToShortTimeString() + "： 采集总数-->" + this.count.ToString();
									}
								}
								Thread.Sleep(1000);
							}
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00003064 File Offset: 0x00001264
		public void tengxun()
		{
			string[] array = this.textBox1.Text.Trim().Split(new string[]
			{
				"\r\n"
			}, StringSplitOptions.None);
			string[] array2 = this.textBox2.Text.Trim().Split(new string[]
			{
				"\r\n"
			}, StringSplitOptions.None);
			bool flag = array2.Length == 0;
			if (flag)
			{
				MessageBox.Show("请添加关键字");
			}
			else
			{
				try
				{
					foreach (string str in array)
					{
						foreach (string text in array2)
						{
							for (int k = 1; k < 100; k++)
							{
								string url = string.Concat(new object[]
								{
									"https://apis.map.qq.com/ws/place/v1/search?keyword=",
									HttpUtility.UrlEncode(text),
									"&boundary=region(",
									HttpUtility.UrlEncode(str),
									",0)&key=7RWBZ-TKSK4-Z7IUA-DVYFV-K4EIF-7DFBY&page_size=20&page_index=",
									k,
									"&orderby=_distance%20HTTP/1.1"
								});
								string url2 = method.GetUrl(url, "utf-8");
								MatchCollection matchCollection = Regex.Matches(url2, "\"title\": \"([\\s\\S]*?)\"");
								MatchCollection matchCollection2 = Regex.Matches(url2, "\"tel\": \"([\\s\\S]*?)\"");
								MatchCollection matchCollection3 = Regex.Matches(url2, "\"address\": \"([\\s\\S]*?)\"");
								MatchCollection matchCollection4 = Regex.Matches(url2, "\"city\": \"([\\s\\S]*?)\"");
								MatchCollection matchCollection5 = Regex.Matches(url2, "\"lat\": ([\\s\\S]*?),");
								bool flag2 = matchCollection.Count == 0;
								if (flag2)
								{
									break;
								}
								for (int l = 0; l < matchCollection3.Count; l++)
								{
									string text2 = this.shaixuan(matchCollection2[l].Groups[1].Value.Replace("\"", "").Replace("[", "").Replace("]", ""));
									bool flag3 = text2 != "";
									if (flag3)
									{
										ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
										listViewItem.SubItems.Add(地图采集.Unicode2String(matchCollection[l].Groups[1].Value));
										bool flag4 = !this.md.jihuo;
										if (flag4)
										{
											bool flag5 = text2.Length > 4;
											if (flag5)
											{
												listViewItem.SubItems.Add(text2.Substring(0, 4) + "*******");
											}
										}
										else
										{
											listViewItem.SubItems.Add(text2);
										}
										listViewItem.SubItems.Add(地图采集.Unicode2String(matchCollection3[l].Groups[1].Value));
										listViewItem.SubItems.Add(地图采集.Unicode2String(matchCollection4[l].Groups[1].Value));
										listViewItem.SubItems.Add(text);
										listViewItem.SubItems.Add(matchCollection5[l].Groups[1].Value);
										bool flag6 = !this.status;
										if (flag6)
										{
											return;
										}
										this.count++;
										this.infolabel.Text = DateTime.Now.ToShortTimeString() + "： 采集总数-->" + this.count.ToString();
									}
								}
								Thread.Sleep(1000);
							}
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000344C File Offset: 0x0000164C
		public void sougou()
		{
			string[] array = this.textBox1.Text.Trim().Split(new string[]
			{
				"\r\n"
			}, StringSplitOptions.None);
			string[] array2 = this.textBox2.Text.Trim().Split(new string[]
			{
				"\r\n"
			}, StringSplitOptions.None);
			bool flag = array2.Length == 0;
			if (flag)
			{
				MessageBox.Show("请添加关键字");
			}
			else
			{
				try
				{
					foreach (string str in array)
					{
						foreach (string text in array2)
						{
							for (int k = 1; k < 100; k++)
							{
								string url = string.Concat(new object[]
								{
									"https://map.sogou.com/EngineV6/search/json?what=keyword:",
									HttpUtility.UrlEncode(str + text),
									"&range=bound:12633753.90625,2521527.34375,12646777.34375,2523988.28125:0&othercityflag=1&appid=1361&userdata=3&encrypt=1&pageinfo=",
									k,
									",50&locationsort=0&version=7.0&ad=0&level=14&exact=0&type=morebtn&attr=&order=&submittime=0&resultTypes=poi&sort=0"
								});
								string url2 = method.GetUrl(url, "gb2312");
								MatchCollection matchCollection = Regex.Matches(url2, "\"caption\":\"([\\s\\S]*?)\"");
								MatchCollection matchCollection2 = Regex.Matches(url2, "\"phone\":([\\s\\S]*?),");
								MatchCollection matchCollection3 = Regex.Matches(url2, "\"address\":([\\s\\S]*?),");
								MatchCollection matchCollection4 = Regex.Matches(url2, "\"city\":([\\s\\S]*?),");
								MatchCollection matchCollection5 = Regex.Matches(url2, "\"minx\":([\\s\\S]*?),");
								bool flag2 = matchCollection.Count == 0;
								if (flag2)
								{
									break;
								}
								for (int l = 0; l < matchCollection.Count; l++)
								{
									string text2 = this.shaixuan(matchCollection2[l].Groups[1].Value.Replace("\"", "").Replace("[", "").Replace("]", ""));
									bool flag3 = text2 != "";
									if (flag3)
									{
										ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
										listViewItem.SubItems.Add(matchCollection[l].Groups[1].Value);
										bool flag4 = !this.md.jihuo;
										if (flag4)
										{
											bool flag5 = text2.Length > 4;
											if (flag5)
											{
												listViewItem.SubItems.Add(text2.Substring(0, 4) + "*******");
											}
										}
										else
										{
											listViewItem.SubItems.Add(text2);
										}
										listViewItem.SubItems.Add(matchCollection3[l].Groups[1].Value.Replace("\"", ""));
										listViewItem.SubItems.Add(matchCollection4[l].Groups[1].Value.Replace("\"", ""));
										listViewItem.SubItems.Add(text);
										listViewItem.SubItems.Add(matchCollection5[l].Groups[1].Value.Replace("\"", ""));
										bool flag6 = !this.status;
										if (flag6)
										{
											return;
										}
										while (!this.zanting)
										{
											Application.DoEvents();
										}
										this.count++;
										this.infolabel.Text = DateTime.Now.ToShortTimeString() + "： 采集总数-->" + this.count.ToString();
									}
								}
								Thread.Sleep(1000);
							}
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00003860 File Offset: 0x00001A60
		public void gaode()
		{
			string[] array = this.textBox1.Text.Trim().Split(new string[]
			{
				"\r\n"
			}, StringSplitOptions.None);
			string[] array2 = this.textBox2.Text.Trim().Split(new string[]
			{
				"\r\n"
			}, StringSplitOptions.None);
			bool flag = array2.Length == 0;
			if (flag)
			{
				MessageBox.Show("请添加关键字");
			}
			else
			{
				try
				{
					foreach (string str in array)
					{
						foreach (string text in array2)
						{
							for (int k = 1; k < 100; k++)
							{
								string url = string.Concat(new object[]
								{
									"https://restapi.amap.com/v3/place/text?s=rsv3&children=&key=c51aeb4379b19d99f34f409cf5c57410&offset=50&page=",
									k,
									"&extensions=all&city=",
									HttpUtility.UrlEncode(str),
									"&language=zh_cn&callback=jsonp_814675_&platform=JS&logversion=2.0&appname=about%3Ablank&csid=B222C126-1764-4373-AFB9-C0C4ADF1F546&sdkversion=1.4.16&keywords=",
									HttpUtility.UrlEncode(text)
								});
								string url2 = method.GetUrl(url, "utf-8");
								MatchCollection matchCollection = Regex.Matches(url2, "\"name\":\"([\\s\\S]*?)\"");
								MatchCollection matchCollection2 = Regex.Matches(url2, "\"tel\":([\\s\\S]*?),");
								MatchCollection matchCollection3 = Regex.Matches(url2, "\"address\":([\\s\\S]*?),");
								MatchCollection matchCollection4 = Regex.Matches(url2, "\"cityname\":([\\s\\S]*?),");
								MatchCollection matchCollection5 = Regex.Matches(url2, "\"location\":([\\s\\S]*?),");
								bool flag2 = matchCollection.Count == 0;
								if (flag2)
								{
									break;
								}
								for (int l = 0; l < matchCollection.Count; l++)
								{
									string text2 = this.shaixuan(matchCollection2[l].Groups[1].Value.Replace("\"", "").Replace("[", "").Replace("]", ""));
									bool flag3 = text2 != "";
									if (flag3)
									{
										ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
										listViewItem.SubItems.Add(matchCollection[l].Groups[1].Value);
										bool flag4 = !this.md.jihuo;
										if (flag4)
										{
											bool flag5 = text2.Length > 4;
											if (flag5)
											{
												listViewItem.SubItems.Add(text2.Substring(0, 4) + "*******");
											}
										}
										else
										{
											listViewItem.SubItems.Add(text2);
										}
										listViewItem.SubItems.Add(matchCollection3[l].Groups[1].Value.Replace("\"", ""));
										listViewItem.SubItems.Add(matchCollection4[l].Groups[1].Value.Replace("\"", ""));
										listViewItem.SubItems.Add(text);
										listViewItem.SubItems.Add(matchCollection5[l].Groups[1].Value.Replace("\"", ""));
										bool flag6 = !this.status;
										if (flag6)
										{
											return;
										}
										while (!this.zanting)
										{
											Application.DoEvents();
										}
										this.count++;
										this.infolabel.Text = DateTime.Now.ToShortTimeString() + "： 采集总数-->" + this.count.ToString();
									}
								}
								Thread.Sleep(1000);
							}
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00003C78 File Offset: 0x00001E78
		public void ditu360()
		{
			string[] array = this.textBox1.Text.Trim().Split(new string[]
			{
				"\r\n"
			}, StringSplitOptions.None);
			string[] array2 = this.textBox2.Text.Trim().Split(new string[]
			{
				"\r\n"
			}, StringSplitOptions.None);
			bool flag = array2.Length == 0;
			if (flag)
			{
				MessageBox.Show("请添加关键字");
			}
			else
			{
				try
				{
					foreach (string str in array)
					{
						foreach (string text in array2)
						{
							for (int k = 1; k < 100; k++)
							{
								string url = string.Concat(new object[]
								{
									"https://restapi.map.so.com/newapi?jsoncallback=jQuery18308054370640882595_1625712067445&city_id=321300&cityname=",
									HttpUtility.UrlEncode(str),
									"&regionType=rectangle&citysuggestion=true&sid=1005&keyword=",
									HttpUtility.UrlEncode(text),
									"&batch=",
									k,
									"&zoom=11&jump=0&region_id=&mobile=1&_=1625712099802"
								});
								string url2 = method.GetUrl(url, "utf-8");
								MatchCollection matchCollection = Regex.Matches(url2, "\"poi_name\":\"([\\s\\S]*?)\"");
								MatchCollection matchCollection2 = Regex.Matches(url2, "\"tel\":\"([\\s\\S]*?)\"");
								MatchCollection matchCollection3 = Regex.Matches(url2, "\"address\":([\\s\\S]*?),");
								MatchCollection matchCollection4 = Regex.Matches(url2, "\"poi_city\":([\\s\\S]*?),");
								MatchCollection matchCollection5 = Regex.Matches(url2, "\"x\":([\\s\\S]*?),");
								bool flag2 = matchCollection.Count == 0;
								if (flag2)
								{
									break;
								}
								for (int l = 0; l < matchCollection.Count; l++)
								{
									string text2 = this.shaixuan(matchCollection2[l].Groups[1].Value.Replace("\"", "").Replace("[", "").Replace("]", ""));
									bool flag3 = text2 != "";
									if (flag3)
									{
										ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
										listViewItem.SubItems.Add(matchCollection[l].Groups[1].Value);
										bool flag4 = !this.md.jihuo;
										if (flag4)
										{
											bool flag5 = text2.Length > 4;
											if (flag5)
											{
												listViewItem.SubItems.Add(text2.Substring(0, 4) + "*******");
											}
										}
										else
										{
											listViewItem.SubItems.Add(text2);
										}
										listViewItem.SubItems.Add(matchCollection3[l].Groups[1].Value.Replace("\"", ""));
										listViewItem.SubItems.Add(matchCollection4[l].Groups[1].Value.Replace("\"", ""));
										listViewItem.SubItems.Add(text);
										listViewItem.SubItems.Add(matchCollection5[l].Groups[1].Value.Replace("\"", ""));
										bool flag6 = !this.status;
										if (flag6)
										{
											return;
										}
										while (!this.zanting)
										{
											Application.DoEvents();
										}
										this.count++;
										this.infolabel.Text = DateTime.Now.ToShortTimeString() + "： 采集总数-->" + this.count.ToString();
									}
								}
								Thread.Sleep(1000);
							}
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00004098 File Offset: 0x00002298
		public string chuli(string tel)
		{
			bool flag = !this.logined;
			string result;
			if (flag)
			{
				bool flag2 = tel.Length > 8;
				if (flag2)
				{
					string str = tel.Substring(0, 4);
					string str2 = tel.Substring(tel.Length - 3, 3);
					tel = str + "****" + str2;
				}
				result = tel;
			}
			else
			{
				result = tel;
			}
			return result;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000040F8 File Offset: 0x000022F8
		public string shaixuan(string tel)
		{
			string result;
			try
			{
				string text = tel;
				string[] array = tel.Split(new string[]
				{
					";"
				}, StringSplitOptions.None);
				bool flag = array.Length < 2;
				if (flag)
				{
					array = tel.Split(new string[]
					{
						","
					}, StringSplitOptions.None);
				}
				bool @checked = this.checkBox6.Checked;
				if (@checked)
				{
					bool flag2 = array.Length == 0;
					if (flag2)
					{
						return "";
					}
				}
				bool checked2 = this.checkBox7.Checked;
				if (checked2)
				{
					bool flag3 = array.Length == 1;
					if (flag3)
					{
						bool flag4 = !tel.Contains("-") && array[0].Length > 10;
						if (flag4)
						{
							return tel;
						}
						return "";
					}
					else
					{
						bool flag5 = array.Length >= 2;
						if (flag5)
						{
							bool flag6 = !array[0].Contains("-") && array[0].Length > 10;
							if (flag6)
							{
								text = array[0];
							}
							else
							{
								bool flag7 = !array[1].Contains("-") && array[1].Length > 10;
								if (flag7)
								{
									text = array[1];
								}
								else
								{
									text = "";
								}
							}
						}
					}
				}
				result = text;
			}
			catch (Exception ex)
			{
				result = "";
			}
			return result;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000426C File Offset: 0x0000246C
		private void button2_Click(object sender, EventArgs e)
		{
			this.tabControl1.SelectedIndex = 1;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000427C File Offset: 0x0000247C
		private void button1_Click(object sender, EventArgs e)
		{
			this.tabControl1.SelectedIndex = 0;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000428C File Offset: 0x0000248C
		private void button3_Click(object sender, EventArgs e)
		{
			this.tabControl1.SelectedIndex = 2;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000429C File Offset: 0x0000249C
		private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			DialogResult dialogResult = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
			bool flag = dialogResult == DialogResult.OK;
			if (flag)
			{
				Process.GetCurrentProcess().Kill();
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000042D6 File Offset: 0x000024D6
		private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			base.WindowState = FormWindowState.Minimized;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000042E1 File Offset: 0x000024E1
		private void panel1_MouseClick(object sender, MouseEventArgs e)
		{
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000042E4 File Offset: 0x000024E4
		private void panel1_MouseDown(object sender, MouseEventArgs e)
		{
			this.mPoint.X = e.X;
			this.mPoint.Y = e.Y;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000430C File Offset: 0x0000250C
		private void panel1_MouseMove(object sender, MouseEventArgs e)
		{
			bool flag = e.Button == MouseButtons.Left;
			if (flag)
			{
				Point mousePosition = Control.MousePosition;
				mousePosition.Offset(-this.mPoint.X, -this.mPoint.Y);
				base.Location = mousePosition;
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000435C File Offset: 0x0000255C
		private void button7_Click(object sender, EventArgs e)
		{
			//this.md.jihuoma();
			string url = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");
			bool flag = !url.Contains("abc147258");
			if (flag)
			{
				Process.GetCurrentProcess().Kill();
			}
			else
			{
				this.count = 0;
				this.infolabel.Text = DateTime.Now.ToShortTimeString() + "开始采集......";
				bool flag2 = this.textBox1.Text == "" || this.textBox2.Text == "";
				if (flag2)
				{
					MessageBox.Show("请选择城市和关键词");
				}
				else
				{
					this.status = true;
					bool @checked = this.checkBox1.Checked;
					if (@checked)
					{
						Thread thread = new Thread(new ThreadStart(this.gaode));
						thread.Start();
						Control.CheckForIllegalCrossThreadCalls = false;
					}
					bool checked2 = this.checkBox2.Checked;
					if (checked2)
					{
						Thread thread2 = new Thread(new ThreadStart(this.ditu360));
						thread2.Start();
						Control.CheckForIllegalCrossThreadCalls = false;
					}
					bool checked3 = this.checkBox3.Checked;
					if (checked3)
					{
						Thread thread3 = new Thread(new ThreadStart(this.baidu));
						thread3.Start();
						Control.CheckForIllegalCrossThreadCalls = false;
					}
					bool checked4 = this.checkBox4.Checked;
					if (checked4)
					{
						Thread thread4 = new Thread(new ThreadStart(this.sougou));
						thread4.Start();
						Control.CheckForIllegalCrossThreadCalls = false;
					}
					bool checked5 = this.checkBox5.Checked;
					if (checked5)
					{
						Thread thread5 = new Thread(new ThreadStart(this.tengxun));
						thread5.Start();
						Control.CheckForIllegalCrossThreadCalls = false;
					}
				}
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00004527 File Offset: 0x00002727
		private void button8_Click(object sender, EventArgs e)
		{
			method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00004541 File Offset: 0x00002741
		private void button4_Click(object sender, EventArgs e)
		{
			this.status = false;
			this.button6.Enabled = true;
			this.button8.Enabled = true;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00004565 File Offset: 0x00002765
		private void button6_Click(object sender, EventArgs e)
		{
			this.listView1.Items.Clear();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00004579 File Offset: 0x00002779
		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
            ProvinceCity.ProvinceCity.BindCity(this.comboBox2, this.comboBox3);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000458E File Offset: 0x0000278E
		private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
		{
			TextBox textBox = this.textBox1;
			textBox.Text = textBox.Text + this.comboBox3.Text + "\r\n";
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000045B8 File Offset: 0x000027B8
		public void getsoftinfo()
		{
			string url = method.GetUrl("http://www.acaiji.com/shangxueba/shangxueba.php?method=login&username=" + this.user_text.Text.Trim() + "&password=" + this.pass_text.Text.Trim(), "utf-8");
			string value = Regex.Match(url, "\"softname\": \"([\\s\\S]*?)\"").Groups[1].Value;
			string value2 = Regex.Match(url, "\"contacts\": \"([\\s\\S]*?)\"").Groups[1].Value;
			bool flag = value != "";
			if (flag)
			{
				this.label1.Text = value;
			}
			bool flag2 = value2 != "";
			if (flag2)
			{
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000466C File Offset: 0x0000286C
		public string login(string user, string pass)
		{
			return method.GetUrl("http://www.acaiji.com/shangxueba/shangxueba.php?method=login&username=" + user + "&password=" + pass, "utf-8");
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000042E1 File Offset: 0x000024E1
		private void login_btn_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000027 RID: 39 RVA: 0x0000469C File Offset: 0x0000289C
		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			for (int i = 0; i < this.comboBox3.Items.Count; i++)
			{
				TextBox textBox = this.textBox1;
				textBox.Text = textBox.Text + this.comboBox3.Items[i].ToString() + "\r\n";
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00004700 File Offset: 0x00002900
		public void creatVcf()
		{
			string text = method.GetTimeStamp() + ".vcf";
			bool flag = File.Exists(text);
			if (flag)
			{
				bool flag2 = MessageBox.Show("“" + text + "”已经存在，是否删除它？", "确认", MessageBoxButtons.YesNo) != DialogResult.Yes;
				if (flag2)
				{
					return;
				}
				File.Delete(text);
			}
			UTF8Encoding encoding = new UTF8Encoding(false);
			StreamWriter streamWriter = new StreamWriter(text, false, encoding);
			for (int i = 0; i < this.listView1.Items.Count; i++)
			{
				string text2 = this.listView1.Items[i].SubItems[1].Text.Trim();
				string text3 = this.listView1.Items[i].SubItems[2].Text.Trim();
				bool flag3 = text2 != "" && text3 != "";
				if (flag3)
				{
					streamWriter.WriteLine("BEGIN:VCARD");
					streamWriter.WriteLine("VERSION:3.0");
					streamWriter.WriteLine("N;CHARSET=UTF-8:" + text2);
					streamWriter.WriteLine("FN;CHARSET=UTF-8:" + text2);
					streamWriter.WriteLine("TEL;TYPE=CELL:" + text3);
					streamWriter.WriteLine("END:VCARD");
				}
			}
			streamWriter.Flush();
			streamWriter.Close();
			MessageBox.Show("生成成功！文件名是：" + text);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00004890 File Offset: 0x00002A90
		private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			bool flag = this.thread1 == null || !this.thread1.IsAlive;
			if (flag)
			{
				Thread thread = new Thread(new ThreadStart(this.creatVcf));
				thread.Start();
				Control.CheckForIllegalCrossThreadCalls = false;
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000048E0 File Offset: 0x00002AE0
		private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				Process.Start("http://121.40.209.61/alipay2/");
			}
			catch (Exception)
			{
				Process.Start("explorer.exe", "http://121.40.209.61/alipay2/");
			}
		}

		// Token: 0x04000003 RID: 3
		private map_method md = new map_method();

		// Token: 0x04000004 RID: 4
		private int count = 0;

		// Token: 0x04000005 RID: 5
		private bool status = true;

		// Token: 0x04000006 RID: 6
		private List<string> tellist = new List<string>();

		// Token: 0x04000007 RID: 7
		private Point mPoint = default(Point);

		// Token: 0x04000008 RID: 8
		private bool zanting = true;

		// Token: 0x04000009 RID: 9
		private bool logined = false;

		// Token: 0x0400000A RID: 10
		private Thread thread1;
	}
}
