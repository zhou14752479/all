using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 冷宝分析
{
    public partial class UseLenBao : UserControl
    {
        string colorname = function.colorname;
        public UseLenBao(Animal animal)
        {

         
            this.BackColor = Color.FromName(colorname);

            this.InitializeComponent();
			
			this.LblName.Text = animal.Name;
			MyProgressBar parent = new MyProgressBar
			{
				Parent = this.PanProgress,
				Minimum = 0,
				Maximum = 100,
				BackColor = Color.FromName(colorname),
                
				Value = animal.WeiKaiChangShu>100 ? 100 : animal.WeiKaiChangShu,
				Dock = DockStyle.Fill
			};
			Label label = new Label
			{
				Parent = parent,
				BackColor = Color.Transparent,
				ForeColor = Color.Black,
				TextAlign = ContentAlignment.MiddleLeft,
				Dock = DockStyle.Fill
			};
			bool flag = animal.WeiKaiChangShu == 0;
			
			if (flag)
			{
				label.Text = "开宝";
				this.LblValue.Text = "开宝";
			}
			else
			{
				label.Text = animal.WeiKaiChangShu.ToString();

                //decimal tian= Convert.ToDecimal((animal.WeiKaiChangShu)) / 4;
                decimal tian = animal.WeiKaiTianShu;
                decimal tianshu = Math.Ceiling(tian);

                this.LblValue.Text = tianshu.ToString();
			}

		}
        protected override void Dispose(bool disposing)
        {
            bool flag = disposing && this.components != null;
            if (flag)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Token: 0x060000C2 RID: 194 RVA: 0x0000D6AC File Offset: 0x0000B8AC
        private void InitializeComponent()
        {
            string colorname = function.colorname;
            this.panel1 = new System.Windows.Forms.Panel();
            this.LblName = new System.Windows.Forms.Label();
            this.PanProgress = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.LblValue = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.LblName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(114, 61);
            this.panel1.TabIndex = 0;
            // 
            // LblName
            // 
            this.LblName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LblName.Location = new System.Drawing.Point(0, 0);
            this.LblName.Name = "LblName";
            this.LblName.Size = new System.Drawing.Size(114, 61);
            this.LblName.TabIndex = 0;
            this.LblName.Text = "label1";
            this.LblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // 
            // PanProgress
            // 
            this.PanProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanProgress.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.PanProgress.Location = new System.Drawing.Point(114, 0);
            this.PanProgress.Name = "PanProgress";
            this.PanProgress.Size = new System.Drawing.Size(264, 61);
            this.PanProgress.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.LblValue);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(378, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(127, 61);
            this.panel3.TabIndex = 2;
            // 
            // LblValue
            // 
            this.LblValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblValue.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LblValue.Location = new System.Drawing.Point(0, 0);
            this.LblValue.Name = "LblValue";
            this.LblValue.Size = new System.Drawing.Size(127, 61);
            this.LblValue.TabIndex = 0;
            this.LblValue.Text = "label2";
            this.LblValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UseLenBao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Color.FromName(colorname);
            this.Controls.Add(this.PanProgress);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "UseLenBao";
            this.Size = new System.Drawing.Size(505, 61);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        // Token: 0x040000CE RID: 206
        private IContainer components = null;

        // Token: 0x040000CF RID: 207
        private Panel panel1;

        // Token: 0x040000D0 RID: 208
        public Label LblName;

        // Token: 0x040000D1 RID: 209
        private Panel PanProgress;
        public Label LblValue;

        // Token: 0x040000D2 RID: 210
        private Panel panel3;
    }
}
