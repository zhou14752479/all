using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using helper;

namespace 启动程序
{
	// Token: 0x0200000E RID: 14
	public partial class 宝钢价格 : Form
	{
		// Token: 0x06000076 RID: 118 RVA: 0x0000F91D File Offset: 0x0000DB1D
		public 宝钢价格()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000F950 File Offset: 0x0000DB50
		public ArrayList getliushui(string patten)
		{
			string url = method.GetUrl("http://ecommerce.ibaosteel.com/baosteel_csm/ne/productPriceQueryMainCas.cas?functionCode=productPriceQueryMainCas&month=202004&prodCode=HRB&seqNum=HRB0001", "GBK");
			MatchCollection matchCollection = Regex.Matches(url, patten);
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < matchCollection.Count - 1; i++)
			{
				arrayList.Add(matchCollection[i].Groups[1].Value);
			}
			return arrayList;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000F9BC File Offset: 0x0000DBBC
		public void HRB()
		{
			string text = "HRB";
			ArrayList arrayList = this.getliushui("HRB','([\\s\\S]*?)'");
			string[] array = new string[]
			{
				"700-899",
				"900-999",
				"1000-1800",
				"1801-2100"
			};
			try
			{
				foreach (object obj in arrayList)
				{
					string text2 = (string)obj;
					string url = string.Concat(new string[]
					{
						"http://ecommerce.ibaosteel.com/baosteel_csm/ne/productPriceQueryMainCas.cas?functionCode=productPriceQueryMainCas&month=",
						this.month,
						"&prodCode=",
						text,
						"&seqNum=",
						text2
					});
					string url2 = method.GetUrl(url, "GBK");
					Match match = Regex.Match(url2, ">牌号：([\\s\\S]*?)</td>");
					MatchCollection matchCollection = Regex.Matches(url2, "border-top:none'>([\\s\\S]*?)</td>");
					MatchCollection matchCollection2 = Regex.Matches(url2, "x:num=([\\s\\S]*?)>([\\s\\S]*?)</td>");
					ArrayList arrayList2 = new ArrayList();
					arrayList2.Add("1.50~1.79");
					foreach (object obj2 in matchCollection)
					{
						Match match2 = (Match)obj2;
						arrayList2.Add(match2.Groups[1].Value);
					}
					int num = 0;
					for (int i = 0; i < arrayList2.Count; i++)
					{
						foreach (string text3 in array)
						{
							ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
							listViewItem.SubItems.Add(this.month);
							listViewItem.SubItems.Add(text + "宝钢股份热轧");
							listViewItem.SubItems.Add(text2);
							listViewItem.SubItems.Add(match.Groups[1].Value.Trim());
							listViewItem.SubItems.Add(arrayList2[i].ToString());
							listViewItem.SubItems.Add(text3);
							listViewItem.SubItems.Add(matchCollection2[2 * num].Groups[2].Value.Trim());
							listViewItem.SubItems.Add("表列价格厚度公差为PT.A交货价格，按PT.B交货的加价50元/吨,按PT.C交货的加价200元/吨。表列价格宽度公差为PW.A交货价格，按PW.B交货的加价50元 / 吨（仅针对毛边板)。仅针对钢板不平度为PF.A交货不加价，按PF.B交货的加价100元 / 吨。");
							listViewItem.SubItems.Add("BQB标准+α加价100元/吨。精整卷、平整卷加价100元 / 吨毛边板、商品卷加价100元 / 吨切边板、切边卷加价200元 / 吨");
							num++;
							while (!this.zanting)
							{
								Application.DoEvents();
							}
							bool flag = !this.status;
							if (flag)
							{
								return;
							}
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000FCF4 File Offset: 0x0000DEF4
		public void HRC()
		{
			string text = "HRC";
			ArrayList arrayList = this.getliushui("HRC','([\\s\\S]*?)'");
			string[] array = new string[]
			{
				"700-799",
				"800-899",
				"900-999",
				"1000-1800",
				"1801-2100"
			};
			try
			{
				foreach (object obj in arrayList)
				{
					string text2 = (string)obj;
					string url = string.Concat(new string[]
					{
						"http://ecommerce.ibaosteel.com/baosteel_csm/ne/productPriceQueryMainCas.cas?functionCode=productPriceQueryMainCas&month=",
						this.month,
						"&prodCode=",
						text,
						"&seqNum=",
						text2
					});
					string url2 = method.GetUrl(url, "GBK");
					Match match = Regex.Match(url2, ">牌号：([\\s\\S]*?)</td>");
					MatchCollection matchCollection = Regex.Matches(url2, "border-top:none'>([\\s\\S]*?)</td>");
					MatchCollection matchCollection2 = Regex.Matches(url2, "x:num([\\s\\S]*?)>([\\s\\S]*?)</td>");
					ArrayList arrayList2 = new ArrayList();
					foreach (object obj2 in matchCollection)
					{
						Match match2 = (Match)obj2;
						arrayList2.Add(match2.Groups[1].Value);
					}
					int num = 0;
					for (int i = 0; i < arrayList2.Count; i++)
					{
						foreach (string text3 in array)
						{
							ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
							listViewItem.SubItems.Add(this.month);
							listViewItem.SubItems.Add(text + "青山基地热轧");
							listViewItem.SubItems.Add(text2);
							listViewItem.SubItems.Add(match.Groups[1].Value.Trim());
							listViewItem.SubItems.Add(arrayList2[i].ToString());
							listViewItem.SubItems.Add(text3);
							listViewItem.SubItems.Add(matchCollection2[num].Groups[2].Value.Trim());
							listViewItem.SubItems.Add("表列价格厚度公差为PT.A交货价格，按PT.B交货的加价50元/吨,按PT.C交货的加价200元/吨。表列价格宽度公差为PW.A交货价格，按PW.B交货的加价50元 / 吨（仅针对毛边板)。仅针对钢板不平度为PF.A交货不加价，按PF.B交货的加价100元 / 吨。");
							listViewItem.SubItems.Add("BQB标准+α加价100元/吨。精整卷、平整卷加价100元 / 吨毛边板、商品卷加价100元 / 吨切边板、切边卷加价200元 / 吨");
							num++;
							while (!this.zanting)
							{
								Application.DoEvents();
							}
							bool flag = !this.status;
							if (flag)
							{
								return;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00010034 File Offset: 0x0000E234
		public void HRM()
		{
			string text = "HRM";
			ArrayList arrayList = this.getliushui("'HRM','([\\s\\S]*?)'");
			try
			{
				foreach (object obj in arrayList)
				{
					string text2 = (string)obj;
					string url = string.Concat(new string[]
					{
						"http://ecommerce.ibaosteel.com/baosteel_csm/ne/productPriceQueryMainCas.cas?functionCode=productPriceQueryMainCas&month=",
						this.month,
						"&prodCode=",
						text,
						"&seqNum=",
						text2
					});
					string url2 = method.GetUrl(url, "GBK");
					Match match = Regex.Match(url2, ">牌号：([\\s\\S]*?)</td>");
					MatchCollection matchCollection = Regex.Matches(url2, "border-top:none'>([\\s\\S]*?)</td>");
					MatchCollection matchCollection2 = Regex.Matches(url2, "x:num([\\s\\S]*?)>([\\s\\S]*?)</td>");
					MatchCollection matchCollection3 = Regex.Matches(url2, "<td colspan=2([\\s\\S]*?)>([\\s\\S]*?)</td>");
					ArrayList arrayList2 = new ArrayList();
					arrayList2.Add("1.50~1.79");
					for (int i = 1; i < matchCollection.Count; i++)
					{
						arrayList2.Add(matchCollection[i].Groups[1].Value);
					}
					int num = 0;
					for (int j = 0; j < arrayList2.Count; j++)
					{
						foreach (object obj2 in matchCollection3)
						{
							Match match2 = (Match)obj2;
							ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
							listViewItem.SubItems.Add(this.month);
							listViewItem.SubItems.Add(text + "梅山、东山基地热轧");
							listViewItem.SubItems.Add(text2);
							listViewItem.SubItems.Add(match.Groups[1].Value.Trim());
							listViewItem.SubItems.Add(arrayList2[j].ToString());
							listViewItem.SubItems.Add(match2.Groups[2].Value);
							listViewItem.SubItems.Add(matchCollection2[2 * num].Groups[2].Value.Trim());
							listViewItem.SubItems.Add("表列价格厚度公差为PT.A交货价格，按PT.B交货的加价50元/吨,按PT.C交货的加价200元/吨。表列价格宽度公差为PW.A交货价格，按PW.B交货的加价50元 / 吨（仅针对毛边板)。仅针对钢板不平度为PF.A交货不加价，按PF.B交货的加价100元 / 吨。");
							listViewItem.SubItems.Add("BQB标准+α加价100元/吨。精整卷、平整卷加价100元 / 吨毛边板、商品卷加价100元 / 吨切边板、切边卷加价200元 / 吨");
							num++;
							while (!this.zanting)
							{
								Application.DoEvents();
							}
							bool flag = !this.status;
							if (flag)
							{
								return;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00010374 File Offset: 0x0000E574
		public void M00()
		{
			string text = "M00";
			ArrayList arrayList = this.getliushui("'M00','([\\s\\S]*?)'");
			string[] array = new string[]
			{
				"600-799",
				"800-999",
				"1000-1199",
				"1200-1499",
				"1500-1699",
				"1700-1850",
				"1851~ "
			};
			try
			{
				foreach (object obj in arrayList)
				{
					string text2 = (string)obj;
					string url = string.Concat(new string[]
					{
						"http://ecommerce.ibaosteel.com/baosteel_csm/ne/productPriceQueryMainCas.cas?functionCode=productPriceQueryMainCas&month=",
						this.month,
						"&prodCode=",
						text,
						"&seqNum=",
						text2
					});
					string url2 = method.GetUrl(url, "GBK");
					Match match = Regex.Match(url2, ">牌号：([\\s\\S]*?)</td>");
					MatchCollection matchCollection = Regex.Matches(url2, "<td rowspan=2 height=([\\s\\S]*?)>([\\s\\S]*?)</td>");
					MatchCollection matchCollection2 = Regex.Matches(url2, "border-left:none' x:num>([\\s\\S]*?)</td>");
					ArrayList arrayList2 = new ArrayList();
					for (int i = 0; i < matchCollection.Count; i++)
					{
						arrayList2.Add(matchCollection[i].Groups[2].Value);
					}
					for (int j = 0; j < arrayList2.Count; j++)
					{
						int num = 0;
						foreach (string text3 in array)
						{
							ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
							listViewItem.SubItems.Add(this.month);
							listViewItem.SubItems.Add(text + "热镀锌");
							listViewItem.SubItems.Add(text2);
							listViewItem.SubItems.Add("DC51D+Z、CR1、DX51D、SGCC、St01Z、St02Z、St03Z ");
							listViewItem.SubItems.Add(arrayList2[j].ToString());
							listViewItem.SubItems.Add(text3);
							listViewItem.SubItems.Add(matchCollection2[14 * j + num].Groups[1].Value.Trim());
							listViewItem.SubItems.Add("表列价格厚度公差为PT.A交货价格，按PT.B交货的加价50元/吨,按PT.C交货的加价200元/吨。表列价格宽度公差为PW.A交货价格，按PW.B交货的加价50元 / 吨（仅针对毛边板)。仅针对钢板不平度为PF.A交货不加价，按PF.B交货的加价100元 / 吨。");
							listViewItem.SubItems.Add("BQB标准+α加价100元/吨。精整卷、平整卷加价100元 / 吨毛边板、商品卷加价100元 / 吨切边板、切边卷加价200元 / 吨");
							num++;
							while (!this.zanting)
							{
								Application.DoEvents();
							}
							bool flag = !this.status;
							if (flag)
							{
								return;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00010688 File Offset: 0x0000E888
		public void ML0()
		{
			string text = "ML0";
			ArrayList arrayList = this.getliushui("'ML0','([\\s\\S]*?)'");
			string[] array = new string[]
			{
				"700-799",
				"800-899",
				"900-999",
				"1000-1199",
				"1200-1300"
			};
			try
			{
				foreach (object obj in arrayList)
				{
					string text2 = (string)obj;
					string url = string.Concat(new string[]
					{
						"http://ecommerce.ibaosteel.com/baosteel_csm/ne/productPriceQueryMainCas.cas?functionCode=productPriceQueryMainCas&month=",
						this.month,
						"&prodCode=",
						text,
						"&seqNum=",
						text2
					});
					string url2 = method.GetUrl(url, "GBK");
					Match match = Regex.Match(url2, ">牌号：([\\s\\S]*?)</td>");
					MatchCollection matchCollection = Regex.Matches(url2, "<td rowspan=2 height=([\\s\\S]*?)>([\\s\\S]*?)</td>");
					MatchCollection matchCollection2 = Regex.Matches(url2, "border-left:none' x:num=\"([\\s\\S]*?)\">([\\s\\S]*?)</td>");
					ArrayList arrayList2 = new ArrayList();
					for (int i = 0; i < matchCollection.Count; i++)
					{
						arrayList2.Add(matchCollection[i].Groups[2].Value);
					}
					for (int j = 0; j < arrayList2.Count; j++)
					{
						int num = 0;
						foreach (string text3 in array)
						{
							ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
							listViewItem.SubItems.Add(this.month);
							listViewItem.SubItems.Add(text + "镀铝锌");
							listViewItem.SubItems.Add(text2);
							listViewItem.SubItems.Add("DC51D＋AZ   (Q/BQB 425-2009) ");
							listViewItem.SubItems.Add(arrayList2[j].ToString());
							listViewItem.SubItems.Add(text3);
							listViewItem.SubItems.Add(matchCollection2[20 * j + 2 * num].Groups[2].Value.Trim());
							listViewItem.SubItems.Add("表列价格厚度公差为PT.A交货价格，按PT.B交货的加价50元/吨,按PT.C交货的加价200元/吨。表列价格宽度公差为PW.A交货价格，按PW.B交货的加价50元 / 吨（仅针对毛边板)。仅针对钢板不平度为PF.A交货不加价，按PF.B交货的加价100元 / 吨。");
							listViewItem.SubItems.Add("BQB标准+α加价100元/吨。精整卷、平整卷加价100元 / 吨毛边板、商品卷加价100元 / 吨切边板、切边卷加价200元 / 吨");
							num++;
							while (!this.zanting)
							{
								Application.DoEvents();
							}
							bool flag = !this.status;
							if (flag)
							{
								return;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0001098C File Offset: 0x0000EB8C
		public void N00()
		{
			string text = "N00";
			ArrayList arrayList = this.getliushui("'N00','([\\s\\S]*?)'");
			string[] array = new string[]
			{
				"700-899",
				"900-1399",
				"1400-1599",
				"1600-1850"
			};
			try
			{
				foreach (object obj in arrayList)
				{
					string text2 = (string)obj;
					string url = string.Concat(new string[]
					{
						"http://ecommerce.ibaosteel.com/baosteel_csm/ne/productPriceQueryMainCas.cas?functionCode=productPriceQueryMainCas&month=",
						this.month,
						"&prodCode=",
						text,
						"&seqNum=",
						text2
					});
					string url2 = method.GetUrl(url, "GBK");
					Match match = Regex.Match(url2, ">牌号：([\\s\\S]*?)</td>");
					MatchCollection matchCollection = Regex.Matches(url2, "<td rowspan=2 height=([\\s\\S]*?)>([\\s\\S]*?)</td>");
					MatchCollection matchCollection2 = Regex.Matches(url2, "border-left:none' x:num>([\\s\\S]*?)</td>");
					ArrayList arrayList2 = new ArrayList();
					for (int i = 0; i < matchCollection.Count; i++)
					{
						arrayList2.Add(matchCollection[i].Groups[2].Value);
					}
					for (int j = 0; j < arrayList2.Count; j++)
					{
						int num = 0;
						foreach (string text3 in array)
						{
							ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
							listViewItem.SubItems.Add(this.month);
							listViewItem.SubItems.Add(text + "电镀锌");
							listViewItem.SubItems.Add(text2);
							listViewItem.SubItems.Add("SECC,BLCE+Z,DC01E+Z,CR1 ");
							listViewItem.SubItems.Add(arrayList2[j].ToString());
							listViewItem.SubItems.Add(text3);
							listViewItem.SubItems.Add(matchCollection2[8 * j + num].Groups[1].Value.Trim());
							listViewItem.SubItems.Add("表列价格厚度公差为PT.A交货价格，按PT.B交货的加价50元/吨,按PT.C交货的加价200元/吨。表列价格宽度公差为PW.A交货价格，按PW.B交货的加价50元 / 吨（仅针对毛边板)。仅针对钢板不平度为PF.A交货不加价，按PF.B交货的加价100元 / 吨。");
							listViewItem.SubItems.Add("BQB标准+α加价100元/吨。精整卷、平整卷加价100元 / 吨毛边板、商品卷加价100元 / 吨切边板、切边卷加价200元 / 吨");
							num++;
							while (!this.zanting)
							{
								Application.DoEvents();
							}
							bool flag = !this.status;
							if (flag)
							{
								return;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00010C88 File Offset: 0x0000EE88
		public void O00()
		{
			string text = "O00";
			ArrayList arrayList = this.getliushui("'O00','([\\s\\S]*?)'");
			string[] array = new string[]
			{
				"600-699",
				"700-799",
				"800-899",
				"900-999",
				"1000-1199",
				"1200-1299",
				"1300-1500"
			};
			try
			{
				foreach (object obj in arrayList)
				{
					string text2 = (string)obj;
					string url = string.Concat(new string[]
					{
						"http://ecommerce.ibaosteel.com/baosteel_csm/ne/productPriceQueryMainCas.cas?functionCode=productPriceQueryMainCas&month=",
						this.month,
						"&prodCode=",
						text,
						"&seqNum=",
						text2
					});
					string url2 = method.GetUrl(url, "GBK");
					Match match = Regex.Match(url2, ">牌号：([\\s\\S]*?)</td>");
					MatchCollection matchCollection = Regex.Matches(url2, "<td rowspan=2 height=([\\s\\S]*?)>([\\s\\S]*?)</td>");
					MatchCollection matchCollection2 = Regex.Matches(url2, "border-left:none' x:num=\"([\\s\\S]*?)\">([\\s\\S]*?)</td>");
					ArrayList arrayList2 = new ArrayList();
					for (int i = 0; i < matchCollection.Count; i++)
					{
						arrayList2.Add(matchCollection[i].Groups[2].Value);
					}
					for (int j = 0; j < arrayList2.Count; j++)
					{
						int num = 0;
						foreach (string text3 in array)
						{
							ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
							listViewItem.SubItems.Add(this.month);
							listViewItem.SubItems.Add(text + "彩涂");
							listViewItem.SubItems.Add(text2);
							listViewItem.SubItems.Add("TDC51D+Z   (Q/BQB 440-2009) ");
							listViewItem.SubItems.Add(arrayList2[j].ToString());
							listViewItem.SubItems.Add(text3);
							listViewItem.SubItems.Add(matchCollection2[28 * j + 2 * num].Groups[2].Value.Trim());
							listViewItem.SubItems.Add("表列价格厚度公差为PT.A交货价格，按PT.B交货的加价50元/吨,按PT.C交货的加价200元/吨。表列价格宽度公差为PW.A交货价格，按PW.B交货的加价50元 / 吨（仅针对毛边板)。仅针对钢板不平度为PF.A交货不加价，按PF.B交货的加价100元 / 吨。");
							listViewItem.SubItems.Add("BQB标准+α加价100元/吨。精整卷、平整卷加价100元 / 吨毛边板、商品卷加价100元 / 吨切边板、切边卷加价200元 / 吨");
							num++;
							while (!this.zanting)
							{
								Application.DoEvents();
							}
							bool flag = !this.status;
							if (flag)
							{
								return;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00010F9C File Offset: 0x0000F19C
		public void S00()
		{
			string text = "S00";
			ArrayList arrayList = this.getliushui("'S00','([\\s\\S]*?)'");
			string[] array = new string[]
			{
				"800-899",
				"900-999",
				"1000-1099",
				"1100-1199",
				"1200-1250",
				"1251-1280"
			};
			try
			{
				foreach (object obj in arrayList)
				{
					string text2 = (string)obj;
					string url = string.Concat(new string[]
					{
						"http://ecommerce.ibaosteel.com/baosteel_csm/ne/productPriceQueryMainCas.cas?functionCode=productPriceQueryMainCas&month=",
						this.month,
						"&prodCode=",
						text,
						"&seqNum=",
						text2
					});
					string url2 = method.GetUrl(url, "GBK");
					MatchCollection matchCollection = Regex.Matches(url2, "width:51pt'>B([\\s\\S]*?)</td>");
					MatchCollection matchCollection2 = Regex.Matches(url2, "border-left:none' x:num>([\\s\\S]*?)</td>");
					ArrayList arrayList2 = new ArrayList();
					for (int i = 0; i < matchCollection.Count; i++)
					{
						arrayList2.Add(matchCollection[i].Groups[1].Value);
					}
					for (int j = 0; j < arrayList2.Count; j++)
					{
						int num = 0;
						foreach (string text3 in array)
						{
							ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
							listViewItem.SubItems.Add(this.month);
							listViewItem.SubItems.Add(text + "无取向电工钢");
							listViewItem.SubItems.Add(text2);
							listViewItem.SubItems.Add("B" + arrayList2[j].ToString());
							listViewItem.SubItems.Add("无");
							listViewItem.SubItems.Add(text3);
							listViewItem.SubItems.Add(matchCollection2[6 * j + num].Groups[1].Value.Trim());
							listViewItem.SubItems.Add("表列价格厚度公差为PT.A交货价格，按PT.B交货的加价50元/吨,按PT.C交货的加价200元/吨。表列价格宽度公差为PW.A交货价格，按PW.B交货的加价50元 / 吨（仅针对毛边板)。仅针对钢板不平度为PF.A交货不加价，按PF.B交货的加价100元 / 吨。");
							listViewItem.SubItems.Add("BQB标准+α加价100元/吨。精整卷、平整卷加价100元 / 吨毛边板、商品卷加价100元 / 吨切边板、切边卷加价200元 / 吨");
							num++;
							while (!this.zanting)
							{
								Application.DoEvents();
							}
							bool flag = !this.status;
							if (flag)
							{
								return;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000112A4 File Offset: 0x0000F4A4
		public void S0C()
		{
			string text = "S0C";
			ArrayList arrayList = this.getliushui("'S0C','([\\s\\S]*?)'");
			string[] array = new string[]
			{
				"800-899",
				"900-999",
				"1000-1099",
				"1100-1199",
				"1200-1250",
				"1251-1280"
			};
			try
			{
				foreach (object obj in arrayList)
				{
					string text2 = (string)obj;
					string url = string.Concat(new string[]
					{
						"http://ecommerce.ibaosteel.com/baosteel_csm/ne/productPriceQueryMainCas.cas?functionCode=productPriceQueryMainCas&month=",
						this.month,
						"&prodCode=",
						text,
						"&seqNum=",
						text2
					});
					string url2 = method.GetUrl(url, "GBK");
					MatchCollection matchCollection = Regex.Matches(url2, "width:50pt'>([\\s\\S]*?)</td>");
					MatchCollection matchCollection2 = Regex.Matches(url2, "border-left:none' x:num>([\\s\\S]*?)</td>");
					MessageBox.Show(matchCollection.Count.ToString());
					ArrayList arrayList2 = new ArrayList();
					for (int i = 2; i < matchCollection.Count - 2; i++)
					{
						arrayList2.Add(matchCollection[i].Groups[1].Value);
					}
					for (int j = 0; j < arrayList2.Count; j++)
					{
						int num = 0;
						foreach (string text3 in array)
						{
							ListViewItem listViewItem = this.listView1.Items.Add((this.listView1.Items.Count + 1).ToString());
							listViewItem.SubItems.Add(this.month);
							listViewItem.SubItems.Add(text + "青山基地无取向电工钢");
							listViewItem.SubItems.Add(text2);
							listViewItem.SubItems.Add(arrayList2[j].ToString());
							listViewItem.SubItems.Add("无");
							listViewItem.SubItems.Add(text3);
							listViewItem.SubItems.Add(matchCollection2[6 * j + num].Groups[1].Value.Trim());
							listViewItem.SubItems.Add("表列价格厚度公差为PT.A交货价格，按PT.B交货的加价50元/吨,按PT.C交货的加价200元/吨。表列价格宽度公差为PW.A交货价格，按PW.B交货的加价50元 / 吨（仅针对毛边板)。仅针对钢板不平度为PF.A交货不加价，按PF.B交货的加价100元 / 吨。");
							listViewItem.SubItems.Add("BQB标准+α加价100元/吨。精整卷、平整卷加价100元 / 吨毛边板、商品卷加价100元 / 吨切边板、切边卷加价200元 / 吨");
							num++;
							while (!this.zanting)
							{
								Application.DoEvents();
							}
							bool flag = !this.status;
							if (flag)
							{
								return;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000115B8 File Offset: 0x0000F7B8
		private void 宝钢价格_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000115BC File Offset: 0x0000F7BC
		private void Button1_Click(object sender, EventArgs e)
		{
			string url = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");
			bool flag = url.Contains("baogangjiage");
			if (flag)
			{
				this.month = this.textBox1.Text.Trim();
				this.button1.Enabled = false;
				this.status = true;
				bool @checked = this.checkBox1.Checked;
				if (@checked)
				{
					Thread thread = new Thread(new ThreadStart(this.HRB));
					thread.Start();
					Control.CheckForIllegalCrossThreadCalls = false;
				}
				bool checked2 = this.checkBox2.Checked;
				if (checked2)
				{
					Thread thread2 = new Thread(new ThreadStart(this.HRC));
					thread2.Start();
					Control.CheckForIllegalCrossThreadCalls = false;
				}
				bool checked3 = this.checkBox3.Checked;
				if (checked3)
				{
					Thread thread3 = new Thread(new ThreadStart(this.HRM));
					thread3.Start();
					Control.CheckForIllegalCrossThreadCalls = false;
				}
				bool checked4 = this.checkBox5.Checked;
				if (checked4)
				{
					Thread thread4 = new Thread(new ThreadStart(this.M00));
					thread4.Start();
					Control.CheckForIllegalCrossThreadCalls = false;
				}
				bool checked5 = this.checkBox6.Checked;
				if (checked5)
				{
					Thread thread5 = new Thread(new ThreadStart(this.ML0));
					thread5.Start();
					Control.CheckForIllegalCrossThreadCalls = false;
				}
				bool checked6 = this.checkBox7.Checked;
				if (checked6)
				{
					Thread thread6 = new Thread(new ThreadStart(this.N00));
					thread6.Start();
					Control.CheckForIllegalCrossThreadCalls = false;
				}
				bool checked7 = this.checkBox8.Checked;
				if (checked7)
				{
					Thread thread7 = new Thread(new ThreadStart(this.O00));
					thread7.Start();
					Control.CheckForIllegalCrossThreadCalls = false;
				}
				bool checked8 = this.checkBox9.Checked;
				if (checked8)
				{
					Thread thread8 = new Thread(new ThreadStart(this.S00));
					thread8.Start();
					Control.CheckForIllegalCrossThreadCalls = false;
				}
				bool checked9 = this.checkBox10.Checked;
				if (checked9)
				{
					Thread thread9 = new Thread(new ThreadStart(this.S0C));
					thread9.Start();
					Control.CheckForIllegalCrossThreadCalls = false;
				}
			}
			else
			{
				MessageBox.Show("验证失败");
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00011801 File Offset: 0x0000FA01
		private void Button4_Click(object sender, EventArgs e)
		{
			this.button1.Enabled = true;
			this.status = false;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00011818 File Offset: 0x0000FA18
		private void Button2_Click(object sender, EventArgs e)
		{
			this.zanting = false;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00011822 File Offset: 0x0000FA22
		private void Button3_Click(object sender, EventArgs e)
		{
			this.zanting = true;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0001182D File Offset: 0x0000FA2D
		private void Button5_Click(object sender, EventArgs e)
		{
			method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00011847 File Offset: 0x0000FA47
		private void Button6_Click(object sender, EventArgs e)
		{
			this.listView1.Items.Clear();
		}

		// Token: 0x0400014F RID: 335
		private bool status = true;

		// Token: 0x04000150 RID: 336
		private bool zanting = true;

		// Token: 0x04000151 RID: 337
		private string month = "";
	}
}
