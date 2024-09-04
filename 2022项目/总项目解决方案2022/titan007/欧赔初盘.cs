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
    public partial class 欧赔初盘 : Form
    {
        public 欧赔初盘()
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
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

		Thread thread;

		bool zanting = true;
		bool status = true;

		#region  亚盘初值
		public void run()
		{


			string html = method.GetUrl("http://live.titan007.com/vbsxml/bfdata_ut.js?r=0071657803475000", "gb2312");



			MatchCollection ids = Regex.Matches(html, "A\\[([\\s\\S]*?)\\]=\"([\\s\\S]*?)\\^");

			MatchCollection liansais = Regex.Matches(html, "A\\[([\\s\\S]*?)\\^([\\s\\S]*?)\\^([\\s\\S]*?)\\^");

			MatchCollection times = Regex.Matches(html, "A\\[([\\s\\S]*?)\\]=\"([\\s\\S]*?);");
			for (int x = 0; x < ids.Count; x++)
			{

				string time = Regex.Match(times[x].Groups[2].Value, @"\d{2}:\d{2}").Groups[0].Value.Trim();
				string date = Regex.Match(times[x].Groups[2].Value, @"\d{1,2}-\d{2}").Groups[0].Value.Trim();
				string a18 = "";
				string a18_2 = "";
				string a18_3 = "";
				string a18_4 = "";
				string a18_5 = "";
				string a18_6 = "";
				string a18_7 = "";
				string a18_8 = "";


				string ysb = "";
				string ysb_2 = "";
				string ysb_3 = "";
				string ysb_4 = "";
				string ysb_5 = "";
				string ysb_6 = "";
				string ysb_7 = "";
				string ysb_8 = "";


				string crown = "";
				string crown_2 = "";
				string crown_3 = "";
				string crown_4 = "";
				string crown_5 = "";
				string crown_6 = "";
				string crown_7 = "";
				string crown_8 = "";


				string a12 = "";
				string a12_2 = "";
				string a12_3 = "";
				string a12_4 = "";
				string a12_5 = "";
				string a12_6 = "";
				string a12_7 = "";
				string a12_8 = "";

				string li = "";
				string li_2 = "";
				string li_3 = "";
				string li_4 = "";
				string li_5 = "";
				string li_6 = "";
				string li_7 = "";
				string li_8 = "";



				//string a365 = "";
				//string a365_2 = "";
				//string a365_3 = "";
				//string a365_4 = "";
				//string a365_5 = "";
				//string a365_6 = "";
				//string a365_7 = "";
				//string a365_8 = "";

			


			

				//string aomen = "";
				//string aomen_2 = "";
				//string aomen_3 = "";
				//string aomen_4 = "";
				//string aomen_5 = "";
				//string aomen_6 = "";
				//string aomen_7 = "";
				//string aomen_8 = "";


			


				//string vcbet = "";
				//string vcbet_2 = "";
				//string vcbet_3 = "";
				//string vcbet_4 = "";
				//string vcbet_5 = "";
				//string vcbet_6 = "";
				//string vcbet_7 = "";
				//string vcbet_8 = "";
				try
				{

					string liansai = liansais[x].Groups[3].Value;

					string aurl = "https://op1.titan007.com/oddslist/" + ids[x].Groups[2].Value + ".htm";

					textBox1.Text = aurl;
					string ahtml = method.GetUrl(aurl, "utf-8");
					Match team = Regex.Match(ahtml, @"bold;"">([\s\S]*?)vs([\s\S]*?)</td>");



					MatchCollection data = Regex.Matches(ahtml, @"<TR align=center class='([\s\S]*?)'>([\s\S]*?)</TR>");

					for (int i = 0; i < data.Count; i++)
					{
						string gongsi = Regex.Match(data[i].Groups[2].Value, "<TD height='22'>([\\s\\S]*?)<").Groups[1].Value.Trim();
						MatchCollection a = Regex.Matches(data[i].Groups[2].Value, "<TD>([\\s\\S]*?)</TD>");

						if (a.Count != 0)
						{
							//if (gongsi.Contains("36*"))
							//{
							//	a365 = liansai;
							//	a365_2 = team.Groups[1].Value.Trim();
							//	a365_3 = team.Groups[2].Value.Trim();
							//	a365_4 = date + " " + time;
							//	a365_5 = gongsi.Replace("36*", "bet 365");
							//	a365_6 = a[0].Groups[1].Value;
							//	a365_7 = a[1].Groups[1].Value;
							//	a365_8 = a[2].Groups[1].Value;
							//}
							if (gongsi.Contains("18*"))
							{
								a18 = liansai;
								a18_2 = team.Groups[1].Value.Trim();
								a18_3 = team.Groups[2].Value.Trim();
								a18_4 = date + " " + time;
								a18_5 = gongsi.Replace("18*", "18bet");
								a18_6 = a[0].Groups[1].Value;
								a18_7 = a[1].Groups[1].Value;
								a18_8 = a[2].Groups[1].Value;
							}
							//if (gongsi.Contains("伟*"))
							//{
							//	vcbet = liansai;
							//	vcbet_2 = team.Groups[1].Value.Trim();
							//	vcbet_3 = team.Groups[2].Value.Trim();
							//	vcbet_4 = date + " " + time;
							//	vcbet_5 = gongsi.Replace("伟*", "伟德");
							//	vcbet_6 = a[0].Groups[1].Value;
							//	vcbet_7 = a[1].Groups[1].Value;
							//	vcbet_8 = a[2].Groups[1].Value;
							//}

							//if (gongsi.Contains("澳"))
							//{
							//	aomen = liansai;
							//	aomen_2 = team.Groups[1].Value.Trim();
							//	aomen_3 = team.Groups[2].Value.Trim();
							//	aomen_4 = match.Groups[2].Value.Trim();
							//	aomen_5 = gongsi.Replace("澳*", "澳门");
							//	aomen_6 = a[0].Groups[1].Value;
							//	aomen_7 = a[1].Groups[1].Value;
							//	aomen_8 = a[2].Groups[1].Value;
							//}

						
							if (gongsi.Contains("易*"))
                            {
                                ysb = liansai;
                                ysb_2 = team.Groups[1].Value.Trim();
                                ysb_3 = team.Groups[2].Value.Trim();
                                ysb_4 = date + " " + time;
                                ysb_5 = gongsi.Replace("易*", "易胜博");
                                ysb_6 = a[0].Groups[1].Value;
                                ysb_7 = a[1].Groups[1].Value;
                                ysb_8 = a[2].Groups[1].Value;
                            }

							if (gongsi.Contains("Crow*"))
							{
								crown = liansai;
								crown_2 = team.Groups[1].Value.Trim();
								crown_3 = team.Groups[2].Value.Trim();
								crown_4 = date + " " + time;
								crown_5 = gongsi.Replace("Crow*", "Crown");
								crown_6 = a[0].Groups[1].Value;
								crown_7 = a[1].Groups[1].Value;
								crown_8 = a[2].Groups[1].Value;
							}
							if (gongsi.Contains("12*"))
                            {
                                a12 = liansai;
                                a12_2 = team.Groups[1].Value.Trim();
                                a12_3 = team.Groups[2].Value.Trim();
                                a12_4 = date + " " + time;
                                a12_5 = gongsi.Replace("12*", "12bet");
                                a12_6 = a[0].Groups[1].Value;
                                a12_7 = a[1].Groups[1].Value;
                                a12_8 = a[2].Groups[1].Value;
                            }

                            if (gongsi.Contains("立*"))
                            {
                                li = liansai;
                                li_2 = team.Groups[1].Value.Trim();
                                li_3 = team.Groups[2].Value.Trim();
                                li_4 = date + " " + time;
                                li_5 = gongsi.Replace("立*", "立博");
                                li_6 = a[0].Groups[1].Value;
                                li_7 = a[1].Groups[1].Value;
                                li_8 = a[2].Groups[1].Value;
                            }

                        




                        }

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
					if (crown != "")
                    {
                        ListViewItem lv3 = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
                        lv3.SubItems.Add(crown);
                        lv3.SubItems.Add(crown_2);
                        lv3.SubItems.Add(crown_3);
                        lv3.SubItems.Add(crown_4);
                        lv3.SubItems.Add(crown_5);
                        lv3.SubItems.Add(crown_6);
                        lv3.SubItems.Add(crown_7);
                        lv3.SubItems.Add(crown_8);
                    }


                   

                    if (a12 != "")
                    {
                        ListViewItem lv3 = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
                        lv3.SubItems.Add(a12);
                        lv3.SubItems.Add(a12_2);
                        lv3.SubItems.Add(a12_3);
                        lv3.SubItems.Add(a12_4);
                        lv3.SubItems.Add(a12_5);
                        lv3.SubItems.Add(a12_6);
                        lv3.SubItems.Add(a12_7);
                        lv3.SubItems.Add(a12_8);
                    }

                    //if (aomen != "")
                    //{
                    //	ListViewItem lv3 = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
                    //	lv3.SubItems.Add(aomen);
                    //	lv3.SubItems.Add(aomen_2);
                    //	lv3.SubItems.Add(aomen_3);
                    //	lv3.SubItems.Add(aomen_4);
                    //	lv3.SubItems.Add(aomen_5);
                    //	lv3.SubItems.Add(aomen_6);
                    //	lv3.SubItems.Add(aomen_7);
                    //	lv3.SubItems.Add(aomen_8);
                    //}

                    if (li != "")
                    {
                        ListViewItem lv3 = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
                        lv3.SubItems.Add(li);
                        lv3.SubItems.Add(li_2);
                        lv3.SubItems.Add(li_3);
                        lv3.SubItems.Add(li_4);
                        lv3.SubItems.Add(li_5);
                        lv3.SubItems.Add(li_6);
                        lv3.SubItems.Add(li_7);
                        lv3.SubItems.Add(li_8);
                    }

                    //if (a365 != "")
                    //{
                    //	ListViewItem lv3 = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
                    //	lv3.SubItems.Add(a365);
                    //	lv3.SubItems.Add(a365_2);
                    //	lv3.SubItems.Add(a365_3);
                    //	lv3.SubItems.Add(a365_4);
                    //	lv3.SubItems.Add(a365_5);
                    //	lv3.SubItems.Add(a365_6);
                    //	lv3.SubItems.Add(a365_7);
                    //	lv3.SubItems.Add(a365_8);
                    //}
                    //if (vcbet != "")
                    //{
                    //	ListViewItem lv3 = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
                    //	lv3.SubItems.Add(vcbet);
                    //	lv3.SubItems.Add(vcbet_2);
                    //	lv3.SubItems.Add(vcbet_3);
                    //	lv3.SubItems.Add(vcbet_4);
                    //	lv3.SubItems.Add(vcbet_5);
                    //	lv3.SubItems.Add(vcbet_6);
                    //	lv3.SubItems.Add(vcbet_7);
                    //	lv3.SubItems.Add(vcbet_8);
                    //}















                    if (data.Count > 0)
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

        private void 欧赔初盘_Load(object sender, EventArgs e)
        {

        }
    }
}
