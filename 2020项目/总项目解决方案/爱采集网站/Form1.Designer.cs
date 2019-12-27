namespace 爱采集网站
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "采集记录";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(14, 51);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(248, 308);
            this.textBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(442, 330);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 29);
            this.button1.TabIndex = 2;
            this.button1.Text = "开始发布";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox6);
            this.groupBox1.Controls.Add(this.textBox5);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Location = new System.Drawing.Point(283, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(412, 134);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "长关键词";
            // 
            // textBox2
            // 
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox2.Location = new System.Drawing.Point(3, 17);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(406, 21);
            this.textBox2.TabIndex = 0;
            this.textBox2.Text = "怎么采集美团商家数据号码？";
            // 
            // textBox3
            // 
            this.textBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox3.Location = new System.Drawing.Point(3, 38);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(406, 21);
            this.textBox3.TabIndex = 1;
            this.textBox3.Text = "比较好的58同城采集软件";
            // 
            // textBox4
            // 
            this.textBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox4.Location = new System.Drawing.Point(3, 59);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(406, 21);
            this.textBox4.TabIndex = 2;
            this.textBox4.Text = "高德地图采集软件怎么使用？如何购买？";
            // 
            // textBox5
            // 
            this.textBox5.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox5.Location = new System.Drawing.Point(3, 80);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(406, 21);
            this.textBox5.TabIndex = 3;
            this.textBox5.Text = "大数据营销软件，好用的电话营销系统，客户资源";
            // 
            // textBox6
            // 
            this.textBox6.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox6.Location = new System.Drawing.Point(3, 101);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(406, 21);
            this.textBox6.TabIndex = 4;
            this.textBox6.Text = "客户信息抓取，销售资源如何获取？";
            // 
            // timer1
            // 
            this.timer1.Interval = 86400000;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1132, 591);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "思忆软件平台后台管理";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Timer timer1;
    }
}

