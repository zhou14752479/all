using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using helper;

namespace 启动程序
{
	// Token: 0x02000012 RID: 18
	public partial class 王者荣耀 : Form
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x00016AAF File Offset: 0x00014CAF
		public 王者荣耀()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00016AC8 File Offset: 0x00014CC8
		private void 王者荣耀_Load(object sender, EventArgs e)
		{
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			StreamReader streamReader = new StreamReader(baseDirectory + "pifu.txt", Encoding.GetEncoding("utf-8"));
			this.pifus = streamReader.ReadToEnd();
			this.webBrowser1.Navigate("https://user.qzone.qq.com/");
			this.webBrowser1.ScriptErrorsSuppressed = true;
			method.SetWebBrowserFeatures(method.IeVersion.IE10);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00016B30 File Offset: 0x00014D30
		public string getRoleId()
		{
			string result;
			try
			{
				string url = "https://yxzj.aci.game.qq.com/main?game=yxzj&area=1&partition=1226&callback=158701422552319029&sCloudApiName=ams.gameattr.role&iAmsActivityId=https%3A%2F%2Fpvp.qq.com%2Fweb201605%2Fartdetail.shtml";
				string urlWithCookie = method.GetUrlWithCookie(url, this.cookie, "gb2312");
				Match match = Regex.Match(urlWithCookie, "&openid=([\\s\\S]*?)&");
				result = match.Groups[1].Value;
			}
			catch (Exception ex)
			{
				result = ex.ToString();
			}
			return result;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00016B98 File Offset: 0x00014D98
		public void run()
		{
			try
			{
				string url = string.Concat(new string[]
				{
					"https://mapps.game.qq.com/yxzj/web201605/GetHeroSkin.php?appid=1104466820&area=",
					this.textBox2.Text.Trim(),
					"&partition=",
					this.textBox3.Text.Trim(),
					"&roleid=",
					this.getRoleId(),
					"&r=0.4968435418279442"
				});
				string urlWithCookie = method.GetUrlWithCookie(url, this.cookie, "gb2312");
				Match match = Regex.Match(urlWithCookie, "\"HeroSkinStr\":\\{([\\s\\S]*?)}");
				string[] array = match.Groups[1].Value.Split(new string[]
				{
					","
				}, StringSplitOptions.None);
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Split(new string[]
					{
						":"
					}, StringSplitOptions.None);
					char[] array3 = array2[1].Replace("\"", "").ToCharArray();
					ArrayList arrayList = new ArrayList();
					for (int j = array3.Length - 1; j >= 0; j--)
					{
						bool flag = array3[j].ToString() == "1";
						if (flag)
						{
							arrayList.Add(array3.Length - 1 - j);
						}
					}
					MatchCollection matchCollection = Regex.Matches(this.pifus, "\"iHeroId\":\"([\\s\\S]*?)\"");
					MatchCollection matchCollection2 = Regex.Matches(this.pifus, "\"szHeroTitle\":\"([\\s\\S]*?)\"");
					MatchCollection matchCollection3 = Regex.Matches(this.pifus, "\"szTitle\":\"([\\s\\S]*?)\"");
					ArrayList arrayList2 = new ArrayList();
					string text = "";
					for (int k = 0; k < matchCollection.Count; k++)
					{
						bool flag2 = array2[0].Replace("\"", "") == matchCollection[k].Groups[1].Value;
						if (flag2)
						{
							arrayList2.Add(matchCollection3[k].Groups[1].Value + " ");
							text = matchCollection2[k].Groups[1].Value;
						}
					}
					StringBuilder stringBuilder = new StringBuilder();
					int l = 0;
					while (l < arrayList.Count)
					{
						try
						{
							int index = Convert.ToInt32(arrayList[l]);
							bool flag3 = l != 0;
							if (flag3)
							{
								stringBuilder.Append(arrayList2[index]);
							}
						}
						catch
						{
							stringBuilder.Append("！");
						}
						IL_273:
						l++;
						continue;
						goto IL_273;
					}
					TextBox textBox = this.textBox1;
					textBox.Text = string.Concat(new string[]
					{
						textBox.Text,
						text,
						" ",
						stringBuilder.ToString(),
						" "
					});
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00016ED4 File Offset: 0x000150D4
		private void button1_Click(object sender, EventArgs e)
		{
			this.cookie = method.GetCookies("https://pvp.qq.com/web201605/artdetail.shtml");
			this.run();
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00016EEE File Offset: 0x000150EE
		private void button2_Click(object sender, EventArgs e)
		{
			this.textBox1.Text = "";
		}

		// Token: 0x040001D6 RID: 470
		private string pifus;

		// Token: 0x040001D7 RID: 471
		private string cookie;
	}
}
