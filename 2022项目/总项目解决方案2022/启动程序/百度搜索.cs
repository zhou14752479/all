using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace 启动程序
{
	// Token: 0x02000014 RID: 20
	public partial class 百度搜索 : Form
	{
		// Token: 0x060000B8 RID: 184 RVA: 0x00018584 File Offset: 0x00016784
		public 百度搜索()
		{
			this.InitializeComponent();
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000185A3 File Offset: 0x000167A3
		private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
		{
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000185A8 File Offset: 0x000167A8
		public static string GetUrl(string Url, string charset)
		{
			try
			{
				string value = "BIDUPSID=921AE8F8E5F0D4E93DDEF4BD9A1F627E; PSTM=1517917678; MCITY=277-277%3A; BD_UPN=12314753; BAIDUID=D28ED8EAA7251A9D96133C7D30DCA04B:FG=1; BDUSS=pyLUZVfk0zUmk1dTdMd2ptUFVsMkF-MmhofkRnV1V3RktiUkZpWjVaU2VGcTVlSUFBQUFBJCQAAAAAAAAAAAEAAABVvFgjztK0-MnPzqjSuwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAJ6Jhl6eiYZeT3; BDUSS_BFESS=pyLUZVfk0zUmk1dTdMd2ptUFVsMkF-MmhofkRnV1V3RktiUkZpWjVaU2VGcTVlSUFBQUFBJCQAAAAAAAAAAAEAAABVvFgjztK0-MnPzqjSuwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAJ6Jhl6eiYZeT3; BDORZ=B490B5EBF6F3CD402E515D22BCDA1598; indexpro=pmmq22dmmkpavn1tcudgtfjnj7; bdindexid=34okn15klp0do5mlpaggj6icb6; yjs_js_security_passport=068ae83ec78197dd658b5d32c582df7118c33e97_1586506470_js; H_PS_PSSID=1455_31169_21090_31254_31187_30905_31217_30824_31085_31164_22157; H_PS_645EC=d4712meJuZZ%2FkTi1MEgrRyWWwK1Yc0nJyMAIdz4xRh3UFjHrv%2FJ1keD2cak; WWW_ST=1586581634006";
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
				httpWebRequest.Referer = "https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&tn=baidu&wd=C%23%E9%80%9A%E8%BF%87webbrowser%E6%89%B9%E9%87%8F%E8%8E%B7%E5%8F%96%E6%90%9C%E7%B4%A2%E7%BB%93%E6%9E%9C&oq=%2526lt%253B%2523%25E9%2580%259A%25E8%25BF%2587webbrowser%25E6%2589%25B9%25E9%2587%258F%25E8%258E%25B7%25E5%258F%2596%25E6%2590%259C%25E7%25B4%25A2%25E7%25BB%2593%25E6%259E%259C&rsv_pq=eaba12fb0014d34a&rsv_t=8b5bcOV6%2ByVBy2beLA3KnQGRbWcqK9qq%2Baatz3%2Fg1YYB0KDDWOf5g%2Bge1d8&rqlang=cn&rsv_enter=0&rsv_dl=tb";
				httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
				httpWebRequest.AllowAutoRedirect = true;
				httpWebRequest.Headers.Add("Cookie", value);
				WebHeaderCollection headers = httpWebRequest.Headers;
				httpWebRequest.KeepAlive = true;
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				httpWebRequest.Timeout = 5000;
				StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding(charset));
				string result = streamReader.ReadToEnd();
				streamReader.Close();
				httpWebResponse.Close();
				return result;
			}
			catch (Exception ex)
			{
				ex.ToString();
			}
			return "";
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0001867C File Offset: 0x0001687C
		public void run()
		{
			StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.GetEncoding("utf-8"));
			string text = streamReader.ReadToEnd();
			string[] array = text.Split(new string[]
			{
				"\r\n"
			}, StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				bool flag = array[i] != "";
				if (flag)
				{
					string url = "https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&tn=baidu&wd=" + HttpUtility.UrlEncode(array[i]);
					string url2 = 百度搜索.GetUrl(url, "utf-8");
					Match match = Regex.Match(url2, "为您找到相关结果约([\\s\\S]*?)个");
					this.label1.Text = "正在抓取：" + array[i];
					ListViewItem listViewItem = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
					listViewItem.SubItems.Add(array[i]);
					listViewItem.SubItems.Add(match.Groups[1].Value);
					while (!this.zanting)
					{
						Application.DoEvents();
					}
					Thread.Sleep(100);
				}
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000187BC File Offset: 0x000169BC
		private void 百度搜索_Load(object sender, EventArgs e)
		{
			this.button3.Click += this.btn3_click;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000187D7 File Offset: 0x000169D7
		private void btn3_click(object sender, EventArgs e)
		{
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000187DC File Offset: 0x000169DC
		private void button1_Click(object sender, EventArgs e)
		{
			bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
			bool flag2 = flag;
			if (flag2)
			{
				this.textBox1.Text = this.openFileDialog1.FileName;
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00018818 File Offset: 0x00016A18
		private void button2_Click(object sender, EventArgs e)
		{
			string url = 百度搜索.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");
			bool flag = url.Contains("baidusousuo");
			if (flag)
			{
				bool flag2 = !this.zanting;
				if (flag2)
				{
					this.zanting = true;
				}
				else
				{
					Thread thread = new Thread(new ThreadStart(this.run));
					thread.Start();
					Control.CheckForIllegalCrossThreadCalls = false;
				}
			}
			else
			{
				MessageBox.Show("验证失败");
			}
		}

		// Token: 0x040001FB RID: 507
		private bool zanting = true;
	}
}
