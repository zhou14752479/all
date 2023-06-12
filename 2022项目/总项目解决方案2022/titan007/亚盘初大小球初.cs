using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace titan007
{
    public partial class 亚盘初大小球初 : Form
    {
        public 亚盘初大小球初()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            status = true;
			Thread thread = new Thread(run);
			thread.Start();
			Control.CheckForIllegalCrossThreadCalls = false;

			

		}

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
			method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
		}


		Thread thread;

		bool zanting = true;
		bool status = true;

        #region  亚盘初值
        public void run()
		{
			string html = method.GetUrl("http://live.titan007.com/vbsxml/bfdata_ut.js?r=0071657803475000", "utf-8");
			MatchCollection ids = Regex.Matches(html, "A\\[([\\s\\S]*?)\\]=\"([\\s\\S]*?)\\^");
			foreach (object obj in ids)
			{
				Match id = (Match)obj;
				string a18= "";
				string a18_2 = "";
				string a18_3 = "";
				string a18_4 = "";
				string a18_5 = "";
				string a18_6 = "";
				string a18_7 = "";
				string a18_8 = "";

				string aomen= "";
				string aomen_2 = "";
				string aomen_3 = "";
				string aomen_4 = "";
				string aomen_5 = "";
				string aomen_6 = "";
				string aomen_7 = "";
				string aomen_8 = "";


				string ysb = "";
				string ysb_2 = "";
				string ysb_3 = "";
				string ysb_4 = "";
				string ysb_5 = "";
				string ysb_6 = "";
				string ysb_7 = "";
				string ysb_8 = "";
				try
				{
					string aurl = "https://vip.titan007.com/AsianOdds_n.aspx?id=" + id.Groups[2].Value;
					string ahtml = method.GetUrl(aurl, "utf-8");
					MatchCollection teams = Regex.Matches(ahtml, "alt=\"([\\s\\S]*?)\"");
					Match match = Regex.Match(ahtml, "class=\"LName\">([\\s\\S]*?)<span class=\"time\">([\\s\\S]*?)&nbsp");
					MatchCollection dataa = Regex.Matches(ahtml, "<tr bgcolor=\"#FFFFFF\"  >([\\s\\S]*?)</tr>");
					MatchCollection datab = Regex.Matches(ahtml, "<tr bgcolor=\"#FAFAFA\"  >([\\s\\S]*?)</tr>");
					for (int i = 0; i < dataa.Count; i++)
					{
						string gongsi = Regex.Match(dataa[i].Groups[1].Value, "<td height=\"25\">([\\s\\S]*?)<").Groups[1].Value;
						MatchCollection a = Regex.Matches(dataa[i].Groups[1].Value, "<td title=\"([\\s\\S]*?)>([\\s\\S]*?)</td>");


						if (a.Count != 0)
						{
							if (gongsi.Contains("18*"))
							{
								a18 = match.Groups[1].Value.Trim();
								a18_2 = teams[0].Groups[1].Value.Trim().Replace(" ", "");
								a18_3 = teams[1].Groups[1].Value.Trim();
								a18_4 = match.Groups[2].Value.Trim();
								a18_5 = gongsi.Replace("18*", "18bet");
								a18_6 = a[0].Groups[2].Value;
								a18_7 = a[1].Groups[2].Value;
								a18_8 = a[2].Groups[2].Value;
							}

							if (gongsi.Contains("澳"))
							{
								aomen = match.Groups[1].Value.Trim();
								aomen_2 = teams[0].Groups[1].Value.Trim().Replace(" ", "");
								aomen_3 = teams[1].Groups[1].Value.Trim();
								aomen_4 = match.Groups[2].Value.Trim();
								aomen_5 = gongsi.Replace("澳*", "澳门");
								aomen_6 = a[0].Groups[2].Value;
								aomen_7 = a[1].Groups[2].Value;
								aomen_8 = a[2].Groups[2].Value;
							}
							if (gongsi.Contains("易*"))
							{
								ysb = match.Groups[1].Value.Trim();
								ysb_2 = teams[0].Groups[1].Value.Trim().Replace(" ", "");
								ysb_3 = teams[1].Groups[1].Value.Trim();
								ysb_4 = match.Groups[2].Value.Trim();
								ysb_5 = gongsi.Replace("易*", "易胜博");
								ysb_6 = a[0].Groups[2].Value;
								ysb_7 = a[1].Groups[2].Value;
								ysb_8 = a[2].Groups[2].Value;
							}
						}

					}
					for (int j = 0; j < datab.Count; j++)
					{
						string gongsi2 = Regex.Match(datab[j].Groups[1].Value, "<td height=\"25\">([\\s\\S]*?)<").Groups[1].Value;
						MatchCollection a2 = Regex.Matches(datab[j].Groups[1].Value, "<td title=\"([\\s\\S]*?)>([\\s\\S]*?)</td>");

						if (a2.Count != 0)
						{
							if (gongsi2.Contains("18*"))
							{
								a18 = match.Groups[1].Value.Trim();
								a18_2 = teams[0].Groups[1].Value.Trim().Replace(" ", "");
								a18_3 = teams[1].Groups[1].Value.Trim();
								a18_4 = match.Groups[2].Value.Trim();
								a18_5 = gongsi2.Replace("18*", "18bet");
								a18_6 = a2[0].Groups[2].Value;
								a18_7 = a2[1].Groups[2].Value;
								a18_8 = a2[2].Groups[2].Value;
							}

							if (gongsi2.Contains("澳"))
							{
								aomen = match.Groups[1].Value.Trim();
								aomen_2 = teams[0].Groups[1].Value.Trim().Replace(" ", "");
								aomen_3 = teams[1].Groups[1].Value.Trim();
								aomen_4 = match.Groups[2].Value.Trim();
								aomen_5 = gongsi2.Replace("澳*", "澳门");
								aomen_6 = a2[0].Groups[2].Value;
								aomen_7 = a2[1].Groups[2].Value;
								aomen_8 = a2[2].Groups[2].Value;
							}
							if (gongsi2.Contains("易*"))
							{
								ysb = match.Groups[1].Value.Trim();
								ysb_2 = teams[0].Groups[1].Value.Trim().Replace(" ", "");
								ysb_3 = teams[1].Groups[1].Value.Trim();
								ysb_4 = match.Groups[2].Value.Trim();
								ysb_5 = gongsi2.Replace("易*", "易胜博");
								ysb_6 = a2[0].Groups[2].Value;
								ysb_7 = a2[1].Groups[2].Value;
								ysb_8 = a2[2].Groups[2].Value;
							}
						}
					}
				

					if (aomen != "")
					{
						ListViewItem lv3 = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
						lv3.SubItems.Add(aomen);
						lv3.SubItems.Add(aomen_2);
						lv3.SubItems.Add(aomen_3);
						lv3.SubItems.Add(aomen_4);
						lv3.SubItems.Add(aomen_5);
						lv3.SubItems.Add(aomen_6);
						lv3.SubItems.Add(aomen_7);
						lv3.SubItems.Add(aomen_8);
					}

					if (ysb != "")
					{
						ListViewItem lv3 = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
						lv3.SubItems.Add(ysb);
						lv3.SubItems.Add(ysb_2);
						lv3.SubItems.Add(ysb_3);
						lv3.SubItems.Add(ysb_4);
						lv3.SubItems.Add(ysb_5);
						lv3.SubItems.Add(ysb_6);
						lv3.SubItems.Add(ysb_7);
						lv3.SubItems.Add(ysb_8);
					}


					if (a18 != "")
					{
						ListViewItem lv3 = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
						lv3.SubItems.Add(a18);
						lv3.SubItems.Add(a18_2);
						lv3.SubItems.Add(a18_3);
						lv3.SubItems.Add(a18_4);
						lv3.SubItems.Add(a18_5);
						lv3.SubItems.Add(a18_6);
						lv3.SubItems.Add(a18_7);
						lv3.SubItems.Add(a18_8);
					}
					if (datab.Count > 0 || dataa.Count > 0)
					{
						ListViewItem lv4 = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
						lv4.SubItems.Add("------------");
						lv4.SubItems.Add("------------------------");
						lv4.SubItems.Add("------------------------");
						lv4.SubItems.Add("------------------------");
						lv4.SubItems.Add("------------");
						lv4.SubItems.Add("------------");
						lv4.SubItems.Add("------------");
						lv4.SubItems.Add("------------");
					}
					Thread.Sleep(1000);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}
				while (!this.zanting)
				{
					Application.DoEvents();
				}
				
				if (!this.status)
				{
					break;
				}
			}
		}
		#endregion


		#region  大小球初值
		public void run2()
		{
			string html = method.GetUrl("http://live.titan007.com/vbsxml/bfdata_ut.js?r=0071657803475000", "utf-8");
			MatchCollection ids = Regex.Matches(html, "A\\[([\\s\\S]*?)\\]=\"([\\s\\S]*?)\\^");
			foreach (object obj in ids)
			{
				Match id = (Match)obj;
				string a18 = "";
				string a18_2 = "";
				string a18_3 = "";
				string a18_4 = "";
				string a18_5 = "";
				string a18_6 = "";
				string a18_7 = "";
				string a18_8 = "";

				string aomen = "";
				string aomen_2 = "";
				string aomen_3 = "";
				string aomen_4 = "";
				string aomen_5 = "";
				string aomen_6 = "";
				string aomen_7 = "";
				string aomen_8 = "";


				string ysb = "";
				string ysb_2 = "";
				string ysb_3 = "";
				string ysb_4 = "";
				string ysb_5 = "";
				string ysb_6 = "";
				string ysb_7 = "";
				string ysb_8 = "";
				try
				{
					string aurl = "https://vip.titan007.com/OverDown_n.aspx?id=" + id.Groups[2].Value;
					string ahtml = method.GetUrl(aurl, "utf-8");
					MatchCollection teams = Regex.Matches(ahtml, "alt=\"([\\s\\S]*?)\"");
					Match match = Regex.Match(ahtml, "class=\"LName\">([\\s\\S]*?)<span class=\"time\">([\\s\\S]*?)&nbsp");
					MatchCollection dataa = Regex.Matches(ahtml, "<tr bgcolor=\"#FFFFFF\"  >([\\s\\S]*?)</tr>");
					MatchCollection datab = Regex.Matches(ahtml, "<tr bgcolor=\"#FAFAFA\"  >([\\s\\S]*?)</tr>");
					for (int i = 0; i < dataa.Count; i++)
					{
						string gongsi = Regex.Match(dataa[i].Groups[1].Value, "<td height=\"25\">([\\s\\S]*?)<").Groups[1].Value;
						MatchCollection a = Regex.Matches(dataa[i].Groups[1].Value, "<td title=\"([\\s\\S]*?)>([\\s\\S]*?)</td>");
						if (a.Count != 0)
						{

							if (gongsi.Contains("18*"))
							{
								a18 = match.Groups[1].Value.Trim();
								a18_2 = teams[0].Groups[1].Value.Trim().Replace(" ", "");
								a18_3 = teams[1].Groups[1].Value.Trim();
								a18_4 = match.Groups[2].Value.Trim();
								a18_5 = gongsi.Replace("18*", "18bet");
								a18_6 = a[0].Groups[2].Value;
								a18_7 = a[1].Groups[2].Value;
								a18_8 = a[2].Groups[2].Value;
							}

							if (gongsi.Contains("澳"))
							{
								aomen = match.Groups[1].Value.Trim();
								aomen_2 = teams[0].Groups[1].Value.Trim().Replace(" ", "");
								aomen_3 = teams[1].Groups[1].Value.Trim();
								aomen_4 = match.Groups[2].Value.Trim();
								aomen_5 = gongsi.Replace("澳*", "澳门");
								aomen_6 = a[0].Groups[2].Value;
								aomen_7 = a[1].Groups[2].Value;
								aomen_8 = a[2].Groups[2].Value;
							}
							if (gongsi.Contains("易*"))
							{
								ysb = match.Groups[1].Value.Trim();
								ysb_2 = teams[0].Groups[1].Value.Trim().Replace(" ", "");
								ysb_3 = teams[1].Groups[1].Value.Trim();
								ysb_4 = match.Groups[2].Value.Trim();
								ysb_5 = gongsi.Replace("易*", "易胜博");
								ysb_6 = a[0].Groups[2].Value;
								ysb_7 = a[1].Groups[2].Value;
								ysb_8 = a[2].Groups[2].Value;
							}
						}
					}
					for (int j = 0; j < datab.Count; j++)
					{
						string gongsi2 = Regex.Match(datab[j].Groups[1].Value, "<td height=\"25\">([\\s\\S]*?)<").Groups[1].Value;
						MatchCollection a2 = Regex.Matches(datab[j].Groups[1].Value, "<td title=\"([\\s\\S]*?)>([\\s\\S]*?)</td>");
						if (a2.Count != 0)
						{
							if (gongsi2.Contains("18*"))
							{
								a18 = match.Groups[1].Value.Trim();
								a18_2 = teams[0].Groups[1].Value.Trim().Replace(" ", "");
								a18_3 = teams[1].Groups[1].Value.Trim();
								a18_4 = match.Groups[2].Value.Trim();
								a18_5 = gongsi2.Replace("18*", "18bet");
								a18_6 = a2[0].Groups[2].Value;
								a18_7 = a2[1].Groups[2].Value;
								a18_8 = a2[2].Groups[2].Value;
							}

							if (gongsi2.Contains("澳"))
							{
								aomen = match.Groups[1].Value.Trim();
								aomen_2 = teams[0].Groups[1].Value.Trim().Replace(" ", "");
								aomen_3 = teams[1].Groups[1].Value.Trim();
								aomen_4 = match.Groups[2].Value.Trim();
								aomen_5 = gongsi2.Replace("澳*", "澳门");
								aomen_6 = a2[0].Groups[2].Value;
								aomen_7 = a2[1].Groups[2].Value;
								aomen_8 = a2[2].Groups[2].Value;
							}
							if (gongsi2.Contains("易*"))
							{
								ysb = match.Groups[1].Value.Trim();
								ysb_2 = teams[0].Groups[1].Value.Trim().Replace(" ", "");
								ysb_3 = teams[1].Groups[1].Value.Trim();
								ysb_4 = match.Groups[2].Value.Trim();
								ysb_5 = gongsi2.Replace("易*", "易胜博");
								ysb_6 = a2[0].Groups[2].Value;
								ysb_7 = a2[1].Groups[2].Value;
								ysb_8 = a2[2].Groups[2].Value;

							}
						}
					}





					

					if (aomen != "")
					{
						ListViewItem lv3 = this.listView2.Items.Add(this.listView2.Items.Count.ToString());
						lv3.SubItems.Add(aomen);
						lv3.SubItems.Add(aomen_2);
						lv3.SubItems.Add(aomen_3);
						lv3.SubItems.Add(aomen_4);
						lv3.SubItems.Add(aomen_5);
						lv3.SubItems.Add(aomen_6);
						lv3.SubItems.Add(aomen_7);
						lv3.SubItems.Add(aomen_8);
					}

					if (ysb != "")
					{
						ListViewItem lv3 = this.listView2.Items.Add(this.listView2.Items.Count.ToString());
						lv3.SubItems.Add(ysb);
						lv3.SubItems.Add(ysb_2);
						lv3.SubItems.Add(ysb_3);
						lv3.SubItems.Add(ysb_4);
						lv3.SubItems.Add(ysb_5);
						lv3.SubItems.Add(ysb_6);
						lv3.SubItems.Add(ysb_7);
						lv3.SubItems.Add(ysb_8);

					}
					if (a18 != "")
					{
						ListViewItem lv3 = this.listView2.Items.Add(this.listView2.Items.Count.ToString());
						lv3.SubItems.Add(a18);
						lv3.SubItems.Add(a18_2);
						lv3.SubItems.Add(a18_3);
						lv3.SubItems.Add(a18_4);
						lv3.SubItems.Add(a18_5);
						lv3.SubItems.Add(a18_6);
						lv3.SubItems.Add(a18_7);
						lv3.SubItems.Add(a18_8);
					}
					if (datab.Count > 0 || dataa.Count > 0)
					{
						ListViewItem lv4 = this.listView2.Items.Add(this.listView2.Items.Count.ToString());
						lv4.SubItems.Add("------------");
						lv4.SubItems.Add("------------------------");
						lv4.SubItems.Add("------------------------");
						lv4.SubItems.Add("------------------------");
						lv4.SubItems.Add("------------");
						lv4.SubItems.Add("------------");
						lv4.SubItems.Add("------------");
						lv4.SubItems.Add("------------");
					}

					Thread.Sleep(1000);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}
				while (!this.zanting)
				{
					Application.DoEvents();
				}

				if (!this.status)
				{
					break;
				}
			}
		}
        #endregion

        private void 亚盘初大小球初_FormClosing(object sender, FormClosingEventArgs e)
        {
			DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
			if (dr == DialogResult.OK)
			{
				// Environment.Exit(0);
				System.Diagnostics.Process.GetCurrentProcess().Kill();
			}
			else
			{
				e.Cancel = true;//点取消的代码 
			}
		}

        private void button6_Click(object sender, EventArgs e)
        {
			listView2.Items.Clear();
			status = true;
			Thread thread = new Thread(run2);
			thread.Start();
			Control.CheckForIllegalCrossThreadCalls = false;
		}

        private void button5_Click(object sender, EventArgs e)
        {
			listView1.Items.Clear();
			listView2.Items.Clear();
		}
    }
}
