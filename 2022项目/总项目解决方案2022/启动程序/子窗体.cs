using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace 启动程序
{
	// Token: 0x0200000D RID: 13
	public partial class 子窗体 : Form
	{
		// Token: 0x06000065 RID: 101 RVA: 0x0000E5D6 File Offset: 0x0000C7D6
		public 子窗体()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000E608 File Offset: 0x0000C808
		public static string GetUrl(string Url)
		{
			try
			{
				string value = "Hm_lvt_fc2b90cbec55323dbc64f2b6400d86c7=1584760539; Hm_lpvt_fc2b90cbec55323dbc64f2b6400d86c7=1584761623";
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
				httpWebRequest.Referer = "https://www.jzj9999.com/news/index.aspx";
				httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
				httpWebRequest.AllowAutoRedirect = true;
				httpWebRequest.Headers.Add("Cookie", value);
				WebHeaderCollection headers = httpWebRequest.Headers;
				headers.Add("Sec-Fetch-Mode: cors");
				headers.Add("Sec-Fetch-Site: same-origin");
				headers.Add("X-Requested-With: XMLHttpRequest");
				httpWebRequest.KeepAlive = true;
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				httpWebRequest.Timeout = 5000;
				StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));
				string result = streamReader.ReadToEnd();
				streamReader.Close();
				httpWebResponse.Close();
				return result;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			return "";
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000E70C File Offset: 0x0000C90C
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.label2.Text = 价格计算.value1;
			this.label3.Text = 价格计算.value2;
			this.label10.Text = 价格计算.value3;
			this.label4.Text = 价格计算.jieguo1.ToString();
			this.label5.Text = 价格计算.jieguo2.ToString();
			this.label6.Text = 价格计算.jieguo3.ToString();
			this.label7.Text = 价格计算.jieguo4.ToString();
			this.label8.Text = 价格计算.jieguo5.ToString();
			this.label9.Text = 价格计算.jieguo6.ToString();
			this.label1.Text = 价格计算.time + " 停盘";
			bool flag = base.Top == 0;
			if (flag)
			{
				this.linkLabel1.LinkColor = Color.Red;
			}
			else
			{
				this.linkLabel1.LinkColor = Color.DarkBlue;
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000E824 File Offset: 0x0000CA24
		private void 子窗体_Load(object sender, EventArgs e)
		{
			Rectangle rectangle = default(Rectangle);
			base.Left = Screen.GetWorkingArea(this).Width - 235;
			base.Top = 0;
			base.TopMost = true;
			this.timer1.Start();
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000E874 File Offset: 0x0000CA74
		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Rectangle rectangle = default(Rectangle);
			base.Left = Screen.GetWorkingArea(this).Width - 235;
			base.Top = 0;
			base.TopMost = true;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000E8B8 File Offset: 0x0000CAB8
		private void 子窗体_FormClosing(object sender, FormClosingEventArgs e)
		{
			DialogResult dialogResult = MessageBox.Show("确认退出吗？", "退出询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
			bool flag = dialogResult != DialogResult.OK;
			if (flag)
			{
				e.Cancel = true;
			}
			else
			{
				Process.GetCurrentProcess().Kill();
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000E8FD File Offset: 0x0000CAFD
		private void 子窗体_MouseEnter(object sender, EventArgs e)
		{
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000E900 File Offset: 0x0000CB00
		private void PictureBox1_MouseEnter(object sender, EventArgs e)
		{
			this.pictureBox1.BackColor = Color.Red;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000E914 File Offset: 0x0000CB14
		private void PictureBox1_MouseLeave(object sender, EventArgs e)
		{
			this.pictureBox1.BackColor = Color.White;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000E928 File Offset: 0x0000CB28
		private void PictureBox1_Click(object sender, EventArgs e)
		{
			this.js.Show();
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000E937 File Offset: 0x0000CB37
		private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			base.Close();
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000E941 File Offset: 0x0000CB41
		private void 子窗体_MouseDown(object sender, MouseEventArgs e)
		{
			this.mPoint.X = e.X;
			this.mPoint.Y = e.Y;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000E968 File Offset: 0x0000CB68
		private void 子窗体_MouseMove(object sender, MouseEventArgs e)
		{
			bool flag = e.Button == MouseButtons.Left;
			if (flag)
			{
				Point mousePosition = Control.MousePosition;
				mousePosition.Offset(-this.mPoint.X, -this.mPoint.Y);
				base.Location = mousePosition;
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000E9B7 File Offset: 0x0000CBB7
		private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			base.WindowState = FormWindowState.Minimized;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000E9C4 File Offset: 0x0000CBC4
		private void LinkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string url = 子窗体.GetUrl("http://www.acaiji.com/index/index/vip.html");
			bool flag = url.Contains("jiagejisuan");
			if (flag)
			{
				this.js.Show();
				this.js.getPrice();
				this.js.jisuan();
				this.js.Hide();
			}
			else
			{
				MessageBox.Show("验证失败");
			}
		}

		// Token: 0x04000138 RID: 312
		private 价格计算 js = new 价格计算();

		// Token: 0x04000139 RID: 313
		private Point mPoint = default(Point);
	}
}
