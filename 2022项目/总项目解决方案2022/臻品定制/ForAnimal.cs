using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using MUWEN;

namespace Background
{
	// Token: 0x02000003 RID: 3
	public partial class ForAnimal : ForParent
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000025AB File Offset: 0x000007AB
		// (set) Token: 0x0600000A RID: 10 RVA: 0x000025B3 File Offset: 0x000007B3
		public Manager Manager { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000025BC File Offset: 0x000007BC
		// (set) Token: 0x0600000C RID: 12 RVA: 0x000025C4 File Offset: 0x000007C4
		public Animal CurrentAnimal { get; set; }

		// Token: 0x0600000D RID: 13 RVA: 0x000025CD File Offset: 0x000007CD
		public ForAnimal()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000025E8 File Offset: 0x000007E8
		public bool CheckControlValue()
		{
			bool flag = this.TxtAnimalName.Text.IsNullOrEmpty();
			bool flag2 = flag;
			bool result;
			if (flag2)
			{
				this.LblMessage.ShowMessage("动物名不能为空");
				result = false;
			}
			else
			{
				bool flag3 = this.TxtBeiLv.Text.IsNullOrEmpty();
				bool flag4 = flag3;
				if (flag4)
				{
					this.LblMessage.ShowMessage("倍率不能为空");
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000265F File Offset: 0x0000085F
		public void ClearControlValue()
		{
			FormHelper.ClearValue(new Control[]
			{
				this.TxtAnimalName,
				this.TxtNames,
				this.TxtBeiLv
			});
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000268C File Offset: 0x0000088C
		public void ModelToControl(Animal model)
		{
			this.TxtAnimalName.Text = model.Name;
			this.TxtNames.Text = model.Names;
			this.TxtBeiLv.Text = model.BeiLv.ToString();
			this.TxtWeiKaiChangShuPianYi.Text = model.WeiKaiChangShuPianYi.ToString();
			this.TxtWeiKaiTianShuPianYi.Text = model.WeiKaiTianShuPianYi.ToString();
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000270C File Offset: 0x0000090C
		public void Init()
		{
			this.Manager = new Manager();
			this.RefreshData();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002722 File Offset: 0x00000922
		private void ForModel_Load(object sender, EventArgs e)
		{
			this.Init();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000272C File Offset: 0x0000092C
		private void LstData_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.ClearControlValue();
			bool flag = this.LstData.SelectedIndex == -1;
			bool flag2 = flag;
			if (flag2)
			{
				this.CurrentAnimal = null;
				this.BtnDelete.Enabled = false;
			}
			else
			{
				this.CurrentAnimal = (this.LstData.SelectedItem as Animal);
				this.ModelToControl(this.CurrentAnimal);
				this.BtnDelete.Enabled = true;
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000027A4 File Offset: 0x000009A4
		private void BtnAddName_Click(object sender, EventArgs e)
		{
			bool flag = this.TxtName.Text.IsNullOrEmpty();
			bool flag2 = flag;
			if (flag2)
			{
				this.LblMessage.ShowMessage("动物名不能为空");
			}
			else
			{
				bool flag3 = this.TxtNames.Text.IsNullOrEmpty();
				bool flag4 = flag3;
				if (flag4)
				{
					this.TxtNames.Text = this.TxtName.Text.Trim();
				}
				else
				{
					string[] array = this.TxtNames.Text.Split(new char[]
					{
						' '
					});
					foreach (string text in array)
					{
						bool flag5 = text.Contains(this.TxtName.Text.Trim()) && text.Length == this.TxtName.Text.Trim().Length;
						bool flag6 = flag5;
						if (flag6)
						{
							this.LblMessage.ShowMessage("动物名不能重复");
							return;
						}
					}
					this.TxtNames.Text = this.TxtNames.Text + " " + this.TxtName.Text.Trim();
					array = this.TxtNames.Text.Split(new char[]
					{
						' '
					});
					StringBuilder stringBuilder = new StringBuilder();
					foreach (string str in from i in array
					orderby i.Length descending
					select i)
					{
						stringBuilder.Append(str + " ");
					}
					this.TxtNames.Text = stringBuilder.ToString().Substring(0, this.TxtNames.Text.Length);
				}
				this.TxtName.Text = null;
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000029B8 File Offset: 0x00000BB8
		private void BtnSave_Click(object sender, EventArgs e)
		{
			bool flag = !this.CheckControlValue();
			bool flag2 = !flag;
			if (flag2)
			{
				this.CurrentAnimal = (from i in this.Manager.Animals
				where i.Name == this.TxtAnimalName.Text
				select i).FirstOrDefault<Animal>();
				bool flag3 = this.CurrentAnimal.IsNull();
				bool flag4 = flag3;
				if (flag4)
				{
					this.CurrentAnimal = new Animal
					{
						BeiLv = this.TxtBeiLv.Text.Toint(),
						Name = this.TxtAnimalName.Text.Trim(),
						Names = this.TxtNames.Text
					};
					this.Manager.Animals.Add(this.CurrentAnimal);
				}
				else
				{
					this.CurrentAnimal.Names = this.TxtNames.Text;
					this.CurrentAnimal.BeiLv = this.TxtBeiLv.Text.Toint();
					this.CurrentAnimal.WeiKaiChangShuPianYi = this.TxtWeiKaiChangShuPianYi.Text.Toint();
					this.CurrentAnimal.WeiKaiTianShuPianYi = this.TxtWeiKaiTianShuPianYi.Text.Toint();
				}
				bool flag5 = this.CurrentAnimal.Names.IsNullOrEmpty();
				bool flag6 = flag5;
				if (flag6)
				{
					this.LblMessage.ShowMessage("至少需要一个别名");
				}
				else
				{
					this.Manager.SaveAnimals();
					this.LblMessage.ShowMessage("保存成功", Color.Green);
					this.RefreshData();
				}
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002B44 File Offset: 0x00000D44
		private void RefreshData()
		{
			this.LstData.DataSource = null;
			this.LstData.DisplayMember = "name";
			this.LstData.DataSource = this.Manager.Animals;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002B7C File Offset: 0x00000D7C
		private void BtnDelete_Click(object sender, EventArgs e)
		{
			bool flag = !FormHelper.MessageBoxYesNo("确实要删除吗？");
			bool flag2 = !flag;
			if (flag2)
			{
				this.Manager.Animals.Remove(this.CurrentAnimal);
				this.Manager.SaveAnimals();
				this.Manager.LoadAnimals();
				this.RefreshData();
				this.CurrentAnimal = null;
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002BE0 File Offset: 0x00000DE0
		private void BtnClearNames_Click(object sender, EventArgs e)
		{
			bool flag = !FormHelper.MessageBoxYesNo("确实要清空吗？");
			bool flag2 = !flag;
			if (flag2)
			{
				this.TxtNames.Text = null;
			}
		}
	}
}
