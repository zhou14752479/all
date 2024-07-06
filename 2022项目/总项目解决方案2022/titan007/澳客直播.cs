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
    public partial class 澳客直播 : Form
    {
        public 澳客直播()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            status = true;
            Thread thread = new Thread(okooo_run);
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

		string cookie = "LastUrl=; Hm_lvt_213d524a1d07274f17dfa17b79db318f=1718765882; __utma=56961525.2053112220.1718765883.1718765883.1718765883.1; __utmc=56961525; __utmz=56961525.1718765883.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); PHPSESSID=3f62aeba837d219615979d6bf8c261adb877ff0d; _ga=GA1.1.464067010.1718765883; pm=; tfstk=fYpnI7s9cB5QodZ--dXINNCRhvc9Ay65JUeRyTQr_N7_Jyep4U0k7F_da26pEFY6-7dKRDOl4EKR8QBdpg5kyFWR8Lx7Eg-yPD_-AfKBAT6rkq3xHHtIMuLUFp2z7h-1VgzeXkYN4T6rkVFTU6McFEIJA3hHbcjRqyzez6kZjgIlUJ5zL5zN5gWPzU5UQ5SRcJSzLwzZMe2PoLJ6Q2jXpmSJxFvGx1l9YNkyfdSHtZxeK8yz0MfhuH7ikSW2w1-5ZLZ7EsKeGUs28yulPdxwLI8qWWQeigxvZED4FGOe-d5kB0MDX9jlgT5g48-GQa5Dptk4SGOwvQJB-u2PAppAZZ1i481RQKC2geqSchWF4U1XeqwC83-XhIBmKrsH_nXG4mFagLR3FGoJQ7N5TGsGlVRAuwnWdSIxjcVjO6S1vqnij7N5TGsGkcmgGz1FfM3A.; IMUserID=33568270; IMUserName=%E7%BD%97%E8%A5%BF830164; OKSID=3f62aeba837d219615979d6bf8c261adb877ff0d; M_UserName=%22%5Cu7f57%5Cu897f830164%22; M_UserID=33568270; M_Ukey=09218c2cd07f97e6ff0d09917e524819; OkAutoUuid=bf207402d81e315b529e6c05bddf84cc; OkMsIndex=8; Hm_lpvt_213d524a1d07274f17dfa17b79db318f=1718766967; acw_tc=2f624a1b17187713405828109e099d0f4b48aac2531ebf84d7b6150aca965c; _ga_B3LCXP8H9E=GS1.1.1718765882.1.1.1718772475.60.0.0";
		#region 彩客网

		#region  亚盘初值
		public void okooo_run()
		{

			
			string html = method.GetUrlWithCookie("https://www.okooo.com/livecenter/football/",cookie, "gb2312");



			MatchCollection ids = Regex.Matches(html, @"matchid=""([\s\S]*?)""");

			
			MatchCollection matchs = Regex.Matches(html, @"class=""ssname""([\s\S]*?)>([\s\S]*?)</a>([\s\S]*?)<td class=""graytx"">([\s\S]*?)</td>");
			MatchCollection homes = Regex.Matches(html, @"homename jsJumpTo"" reversion=""0"">([\s\S]*?)</a>");
			MatchCollection aways = Regex.Matches(html, @"awayname jsJumpTo"" reversion=""0"">([\s\S]*?)</a>");
			for (int i = 0; i< ids.Count; i++)
			{


		

				try
				{
					

					string aurl = "https://www.okooo.com/soccer/match/"+ ids[i].Groups[1].Value + "/ah/ajax/?page=0&trnum=0&companytype=BaijiaBooks";

					string ahtml = method.GetUrlWithCookie(aurl, cookie, "gb2312");


					string bhtml = Regex.Match(ahtml, @"""avg"":([\s\S]*?)Radio").Groups[1].Value.Trim();
					
					MatchCollection a= Regex.Matches(bhtml, @"""home"":""([\s\S]*?)""");
					MatchCollection b = Regex.Matches(bhtml, @"""boundary"":""([\s\S]*?)""");
					MatchCollection c = Regex.Matches(bhtml, @"""away"":""([\s\S]*?)""");

					ListViewItem lv1 = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
					lv1.SubItems.Add(matchs[i].Groups[2].Value);
					lv1.SubItems.Add(homes[i].Groups[1].Value);
					lv1.SubItems.Add(aways[i].Groups[1].Value);
					lv1.SubItems.Add(matchs[i].Groups[4].Value);

					lv1.SubItems.Add(a[0].Groups[1].Value);
					lv1.SubItems.Add(b[0].Groups[1].Value);
					lv1.SubItems.Add(c[0].Groups[1].Value);
					lv1.SubItems.Add(a[1].Groups[1].Value);
					lv1.SubItems.Add(b[1].Groups[1].Value);
					lv1.SubItems.Add(c[1].Groups[1].Value);





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



		#region 大小球
		public void okooo_run2()
		{


			string html = method.GetUrlWithCookie("https://www.okooo.com/livecenter/football/", cookie, "gb2312");



			MatchCollection ids = Regex.Matches(html, @"matchid=""([\s\S]*?)""");


			MatchCollection matchs = Regex.Matches(html, @"class=""ssname""([\s\S]*?)>([\s\S]*?)</a>([\s\S]*?)<td class=""graytx"">([\s\S]*?)</td>");
			MatchCollection homes = Regex.Matches(html, @"homename jsJumpTo"" reversion=""0"">([\s\S]*?)</a>");
			MatchCollection aways = Regex.Matches(html, @"awayname jsJumpTo"" reversion=""0"">([\s\S]*?)</a>");
			for (int i = 0; i < ids.Count; i++)
			{




				try
				{


					string aurl = "https://www.okooo.com/soccer/match/" + ids[i].Groups[1].Value + "/overunder/ajax/?page=0&trnum=0&companytype=BaijiaBooks";

					string ahtml = method.GetUrlWithCookie(aurl, cookie, "gb2312");


					string bhtml = Regex.Match(ahtml, @"""avg"":([\s\S]*?)Radio").Groups[1].Value.Trim();

					MatchCollection a = Regex.Matches(bhtml, @"""over"":""([\s\S]*?)""");
					MatchCollection b = Regex.Matches(bhtml, @"""boundary"":""([\s\S]*?)""");
					MatchCollection c = Regex.Matches(bhtml, @"""under"":""([\s\S]*?)""");

					ListViewItem lv1 = this.listView2.Items.Add(this.listView2.Items.Count.ToString());
					lv1.SubItems.Add(matchs[i].Groups[2].Value);
					lv1.SubItems.Add(homes[i].Groups[1].Value);
					lv1.SubItems.Add(aways[i].Groups[1].Value);
					lv1.SubItems.Add(matchs[i].Groups[4].Value);

					lv1.SubItems.Add(a[0].Groups[1].Value);
					lv1.SubItems.Add(b[0].Groups[1].Value);
					lv1.SubItems.Add(c[0].Groups[1].Value);
					lv1.SubItems.Add(a[1].Groups[1].Value);
					lv1.SubItems.Add(b[1].Groups[1].Value);
					lv1.SubItems.Add(c[1].Groups[1].Value);





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

        #endregion

        private void button6_Click(object sender, EventArgs e)
        {
			listView2.Items.Clear();
			status = true;
			Thread thread = new Thread(okooo_run2);
			thread.Start();
			Control.CheckForIllegalCrossThreadCalls = false;
		}
    }
}
