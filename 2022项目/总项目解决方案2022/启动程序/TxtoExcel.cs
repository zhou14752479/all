using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using helper;

namespace 启动程序
{
	// Token: 0x02000005 RID: 5
	public partial class TxtoExcel : Form
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00003664 File Offset: 0x00001864
		public TxtoExcel()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000367C File Offset: 0x0000187C
		private void Button2_Click(object sender, EventArgs e)
		{
			Thread thread = new Thread(new ThreadStart(this.run));
			thread.Start();
			Control.CheckForIllegalCrossThreadCalls = false;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000036AC File Offset: 0x000018AC
		public void run()
		{
			try
			{
				ArrayList arrayList = new ArrayList();
				StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
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
						arrayList.Add(array[i]);
					}
				}
				int j = 0;
				while (j < arrayList.Count)
				{
					try
					{
						string text2 = arrayList[13 * j].ToString().Replace("姓名", "").Trim();
						string text3 = arrayList[13 * j + 1].ToString().Replace("统一社会信用代码", "").Trim();
						string text4 = arrayList[13 * j + 2].ToString().Replace("险种类型", "").Trim();
						string text5 = arrayList[13 * j + 3].ToString().Replace("基准参保关系ID", "").Trim();
						string text6 = arrayList[13 * j + 4].ToString().Replace("证件类型", "").Trim();
						string text7 = arrayList[13 * j + 5].ToString().Replace("证件号码", "").Trim();
						string text8 = arrayList[13 * j + 6].ToString().Replace("开始日期", "").Trim();
						string text9 = arrayList[13 * j + 7].ToString().Replace("终止日期", "").Trim();
						string text10 = arrayList[13 * j + 8].ToString().Replace("单位编号", "").Trim();
						string text11 = arrayList[13 * j + 9].ToString().Replace("单位名称", "").Trim();
						string text12 = arrayList[13 * j + 10].ToString().Replace("行政区划代码", "").Trim();
						string text13 = arrayList[13 * j + 11].ToString().Replace("个人编号", "").Trim();
						string text14 = arrayList[13 * j + 12].ToString().Replace("社会保障号码", "").Trim();
						ListViewItem listViewItem = this.listView1.Items.Add(text2);
						listViewItem.SubItems.Add(text3);
						listViewItem.SubItems.Add(text4);
						listViewItem.SubItems.Add(text5);
						listViewItem.SubItems.Add(text6);
						listViewItem.SubItems.Add(text7);
						listViewItem.SubItems.Add(text8);
						listViewItem.SubItems.Add(text9);
						listViewItem.SubItems.Add(text10);
						listViewItem.SubItems.Add(text11);
						listViewItem.SubItems.Add(text12);
						listViewItem.SubItems.Add(text13);
						listViewItem.SubItems.Add(text14);
						bool flag2 = this.listView1.Items.Count > 2;
						if (flag2)
						{
							this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
						}
					}
					catch
					{
					}
					IL_393:
					j++;
					continue;
					goto IL_393;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00003AB0 File Offset: 0x00001CB0
		private void Button1_Click(object sender, EventArgs e)
		{
			bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
			bool flag2 = flag;
			if (flag2)
			{
				this.textBox1.Text = this.openFileDialog1.FileName;
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00003AEB File Offset: 0x00001CEB
		private void Button3_Click(object sender, EventArgs e)
		{
			method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
		}
	}
}
