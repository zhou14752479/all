using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using myDLL;

namespace 地图营销
{
	// Token: 0x02000002 RID: 2
	internal class map_method
	{
		// Token: 0x06000001 RID: 1
		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

		// Token: 0x06000002 RID: 2
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

		// Token: 0x06000003 RID: 3 RVA: 0x00002050 File Offset: 0x00000250
		public void IniWriteValue(string Section, string Key, string Value)
		{
			map_method.WritePrivateProfileString(Section, Key, Value, this.inipath);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002064 File Offset: 0x00000264
		public string IniReadValue(string Section, string Key)
		{
			StringBuilder stringBuilder = new StringBuilder(500);
			int privateProfileString = map_method.GetPrivateProfileString(Section, Key, "", stringBuilder, 500, this.inipath);
			return stringBuilder.ToString();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020A0 File Offset: 0x000002A0
		public bool ExistINIFile()
		{
			return File.Exists(this.inipath);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020C0 File Offset: 0x000002C0
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

		// Token: 0x06000007 RID: 7 RVA: 0x000023AC File Offset: 0x000005AC
		public void jiance2()
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
						this.jihuo = false;
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
							this.jihuo = false;
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
					this.jihuo = false;
				}
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000268C File Offset: 0x0000088C
		public void register(string jihuoma)
		{
			string url = method.GetUrl("http://www.acaiji.com/shangxueba2/shangxueba.php?method=register&username=" + jihuoma + "&password=1&days=1&type=1111jihuoma", "utf-8");
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000026B8 File Offset: 0x000008B8
		public bool login(string jihuoma)
		{
			string url = method.GetUrl("http://www.acaiji.com/shangxueba2/shangxueba.php?username=" + jihuoma + "&password=1&method=login", "utf-8");
			return url.Contains("true");
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000026FC File Offset: 0x000008FC
		public void jihuoma()
		{
			try
			{
				string md = method.GetMD5(method.GetMacAddress());
				long num = Convert.ToInt64(method.GetTimeStamp()) + 31536000L;
				bool flag = this.ExistINIFile();
				if (flag)
				{
					string text = this.IniReadValue("values", "key");
					string[] array = text.Split(new string[]
					{
						"asd147"
					}, StringSplitOptions.None);
					bool flag2 = array[0] != md;
					if (flag2)
					{
						this.jihuo = false;
						MessageBox.Show("激活失败，软件已绑定其他电脑");
					}
					else
					{
						bool flag3 = Convert.ToInt32(array[1]) < Convert.ToInt32(method.GetTimeStamp());
						if (flag3)
						{
							MessageBox.Show("激活已过期");
							string text2 = Interaction.InputBox("请购买激活码,使用正式版软件！\r\n\r\n无激活码点击确定免费试用", "激活软件", "", -1, -1);
							string jihuoma = text2;
							bool flag4 = this.login(jihuoma);
							if (flag4)
							{
								MessageBox.Show("激活失败，激活码失效");
								this.jihuo = false;
							}
							else
							{
								bool flag5 = text2.Length > 40;
								if (flag5)
								{
									text2 = text2.Remove(0, 10);
									text2 = text2.Remove(text2.Length - 10, 10);
									text2 = method.Base64Decode(Encoding.Default, text2);
									string a = text2.Remove(text2.Length - 16, 16);
									string value = text2.Substring(text2.Length - 10, 10);
									bool flag6 = Convert.ToInt64(method.GetTimeStamp()) - Convert.ToInt64(value) < 99999999L;
									if (flag6)
									{
										bool flag7 = a == "ling" || a == "san";
										if (flag7)
										{
											this.IniWriteValue("values", "key", md + "asd147" + num);
											MessageBox.Show("激活成功");
											this.register(jihuoma);
											return;
										}
									}
									bool flag8 = a == "si";
									if (flag8)
									{
										this.IniWriteValue("values", "key", md + "asd147" + 86400);
										MessageBox.Show("激活成功");
										this.register(jihuoma);
										return;
									}
								}
								MessageBox.Show("激活码错误，点击试用");
								this.jihuo = false;
							}
						}
					}
				}
				else
				{
					string text3 = Interaction.InputBox("请购买激活码,使用正式版软件！\r\n\r\n无激活码点击确定免费试用", "激活软件", "", -1, -1);
					string jihuoma2 = text3;
					bool flag9 = this.login(jihuoma2);
					if (flag9)
					{
						MessageBox.Show("激活失败，激活码失效");
						this.jihuo = false;
					}
					else
					{
						bool flag10 = text3.Length > 40;
						if (flag10)
						{
							text3 = text3.Remove(0, 10);
							text3 = text3.Remove(text3.Length - 10, 10);
							text3 = method.Base64Decode(Encoding.Default, text3);
							string a2 = text3.Remove(text3.Length - 16, 16);
							string value2 = text3.Substring(text3.Length - 10, 10);
							bool flag11 = Convert.ToInt64(method.GetTimeStamp()) - Convert.ToInt64(value2) < 99999999L;
							if (flag11)
							{
								bool flag12 = a2 == "ling" || a2 == "san";
								if (flag12)
								{
									this.IniWriteValue("values", "key", md + "asd147" + num);
									MessageBox.Show("激活成功");
									this.register(jihuoma2);
									return;
								}
							}
							bool flag13 = a2 == "si";
							if (flag13)
							{
								this.IniWriteValue("values", "key", md + "asd147" + 86400);
								MessageBox.Show("激活成功");
								this.register(jihuoma2);
								return;
							}
						}
						MessageBox.Show("激活码错误，点击试用");
						this.jihuo = false;
					}
				}
			}
			catch (Exception)
			{
				MessageBox.Show("激活码错误");
				this.jihuo = false;
			}
		}

		// Token: 0x04000001 RID: 1
		private string inipath = AppDomain.CurrentDomain.BaseDirectory + "mapconfig.ini";

		// Token: 0x04000002 RID: 2
		public bool jihuo = true;
	}
}
