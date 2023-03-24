using System;
using System.Net;
using System.Text.RegularExpressions;

namespace 抖音扫码获取cookie
{
	// Token: 0x02000004 RID: 4
	internal class CookieHelper
	{
		// Token: 0x06000035 RID: 53 RVA: 0x00004878 File Offset: 0x00002A78
		public static CookieCollection GetCookiesByHeader(string setCookie)
		{
			CookieCollection cookieCollection = new CookieCollection();
			setCookie += ",T";
			MatchCollection listStr = CookieHelper.RegexSplitCookie2.Matches(setCookie);
			foreach (object obj in listStr)
			{
				Match item = (Match)obj;
				string[] cookieItem = item.Value.Split(new char[]
				{
					';'
				});
				Cookie cookie = new Cookie();
				int index = 0;
				while (index < cookieItem.Length)
				{
					string info = cookieItem[index];
					bool flag = info.Contains("=");
					if (flag)
					{
						int indexK = info.IndexOf('=');
						string name = info.Substring(0, indexK).Trim();
						string val = info.Substring(indexK + 1);
						bool flag2 = index == 0;
						if (flag2)
						{
							cookie.Name = name;
							cookie.Value = val;
						}
						else
						{
							bool flag3 = name.Equals("Domain", StringComparison.OrdinalIgnoreCase);
							if (flag3)
							{
								cookie.Domain = val;
							}
							else
							{
								bool flag4 = name.Equals("Expires", StringComparison.OrdinalIgnoreCase);
								if (flag4)
								{
									DateTime expires;
									DateTime.TryParse(val, out expires);
									cookie.Expires = expires;
								}
								else
								{
									bool flag5 = name.Equals("Path", StringComparison.OrdinalIgnoreCase);
									if (flag5)
									{
										cookie.Path = val;
									}
									else
									{
										bool flag6 = name.Equals("Version", StringComparison.OrdinalIgnoreCase);
										if (flag6)
										{
											cookie.Version = Convert.ToInt32(val);
										}
									}
								}
							}
						}
					}
					else
					{
						bool flag7 = info.Trim().Equals("HttpOnly", StringComparison.OrdinalIgnoreCase);
						if (flag7)
						{
							cookie.HttpOnly = true;
						}
					}
					IL_187:
					index++;
					continue;
					goto IL_187;
				}
				cookieCollection.Add(cookie);
			}
			return cookieCollection;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00004A74 File Offset: 0x00002C74
		public static string GetCookies(string setCookie, Uri uri)
		{
			string strCookies = string.Empty;
			CookieCollection cookies = CookieHelper.GetCookiesByHeader(setCookie);
			foreach (object obj in cookies)
			{
				Cookie cookie = (Cookie)obj;
				bool flag = cookie.Expires < DateTime.Now && cookie.Expires != DateTime.MinValue;
				if (!flag)
				{
					bool flag2 = uri.Host.Contains(cookie.Domain);
					if (flag2)
					{
						strCookies = string.Concat(new string[]
						{
							strCookies,
							cookie.Name,
							"=",
							cookie.Value,
							"; "
						});
					}
				}
			}
			return strCookies;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00004B5C File Offset: 0x00002D5C
		public static string GetCookieValueByName(string setCookie, string name)
		{
			Regex regex = new Regex("(?<=" + name + "=).*?(?=; )");
			return regex.IsMatch(setCookie) ? regex.Match(setCookie).Value : string.Empty;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00004BA0 File Offset: 0x00002DA0
		public static string SetCookieValueByName(string setCookie, string name, string value)
		{
			Regex regex = new Regex("(?<=" + name + "=).*?(?=; )");
			bool flag = regex.IsMatch(setCookie);
			if (flag)
			{
				setCookie = regex.Replace(setCookie, value);
			}
			return setCookie;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00004BE0 File Offset: 0x00002DE0
		public static string UpdateCookieValueByName(string oldCookie, string newCookie, string name)
		{
			Regex regex = new Regex("(?<=" + name + "=).*?[(?=; )|$]");
			bool flag = regex.IsMatch(oldCookie) && regex.IsMatch(newCookie);
			if (flag)
			{
				oldCookie = regex.Replace(oldCookie, regex.Match(newCookie).Value);
			}
			return oldCookie;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00004C38 File Offset: 0x00002E38
		public static string UpdateCookieValue(string oldCookie, string newCookie)
		{
			CookieCollection list = CookieHelper.GetCookiesByHeader(newCookie);
			foreach (object obj in list)
			{
				Cookie cookie = (Cookie)obj;
				Regex regex = new Regex("(?<=" + cookie.Name + "=).*?[(?=; )|$]");
				oldCookie = (regex.IsMatch(oldCookie) ? regex.Replace(oldCookie, cookie.Value) : string.Concat(new string[]
				{
					cookie.Name,
					"=",
					cookie.Value,
					"; ",
					oldCookie
				}));
			}
			return oldCookie;
		}

		// Token: 0x04000002 RID: 2
		private static readonly Regex RegexSplitCookie2 = new Regex("[^,][\\S\\s]+?;+[\\S\\s]+?(?=,\\S)");
	}
}
