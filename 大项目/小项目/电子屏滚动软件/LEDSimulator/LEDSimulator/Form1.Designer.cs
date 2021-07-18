namespace LEDSimulator
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.m_TextTB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TextBtn = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.LEDPictureBox = new System.Windows.Forms.PictureBox();
            this.LEDPictureBox2 = new System.Windows.Forms.PictureBox();
            this.TextBtn2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.m_TextTB2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.log = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.LEDPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LEDPictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // m_TextTB
            // 
            this.m_TextTB.Location = new System.Drawing.Point(309, 139);
            this.m_TextTB.Name = "m_TextTB";
            this.m_TextTB.Size = new System.Drawing.Size(245, 21);
            this.m_TextTB.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(224, 142);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "显示文字一：";
            // 
            // TextBtn
            // 
            this.TextBtn.Location = new System.Drawing.Point(560, 137);
            this.TextBtn.Name = "TextBtn";
            this.TextBtn.Size = new System.Drawing.Size(75, 23);
            this.TextBtn.TabIndex = 4;
            this.TextBtn.Text = "设置文字";
            this.TextBtn.UseVisualStyleBackColor = true;
            this.TextBtn.Click += new System.EventHandler(this.TextBtn_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // LEDPictureBox
            // 
            this.LEDPictureBox.Location = new System.Drawing.Point(26, 47);
            this.LEDPictureBox.Name = "LEDPictureBox";
            this.LEDPictureBox.Size = new System.Drawing.Size(916, 64);
            this.LEDPictureBox.TabIndex = 6;
            this.LEDPictureBox.TabStop = false;
            // 
            // LEDPictureBox2
            // 
            this.LEDPictureBox2.Location = new System.Drawing.Point(22, 216);
            this.LEDPictureBox2.Name = "LEDPictureBox2";
            this.LEDPictureBox2.Size = new System.Drawing.Size(916, 64);
            this.LEDPictureBox2.TabIndex = 7;
            this.LEDPictureBox2.TabStop = false;
            // 
            // TextBtn2
            // 
            this.TextBtn2.Location = new System.Drawing.Point(560, 313);
            this.TextBtn2.Name = "TextBtn2";
            this.TextBtn2.Size = new System.Drawing.Size(75, 23);
            this.TextBtn2.TabIndex = 10;
            this.TextBtn2.Text = "设置文字";
            this.TextBtn2.UseVisualStyleBackColor = true;
            this.TextBtn2.Click += new System.EventHandler(this.TextBtn2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(224, 318);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "显示文字二：";
            // 
            // m_TextTB2
            // 
            this.m_TextTB2.Location = new System.Drawing.Point(309, 315);
            this.m_TextTB2.Name = "m_TextTB2";
            this.m_TextTB2.Size = new System.Drawing.Size(245, 21);
            this.m_TextTB2.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(224, 169);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "滚动速度：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(224, 345);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "滚动速度：";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(309, 169);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(97, 21);
            this.numericUpDown1.TabIndex = 15;
            this.numericUpDown1.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(309, 345);
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(97, 21);
            this.numericUpDown2.TabIndex = 16;
            this.numericUpDown2.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // log
            // 
            this.log.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.log.Font = new System.Drawing.Font("宋体", 15F);
            this.log.Location = new System.Drawing.Point(0, 413);
            this.log.Multiline = true;
            this.log.Name = "log";
            this.log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.log.Size = new System.Drawing.Size(972, 162);
            this.log.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 398);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 18;
            this.label5.Text = "历史设置文字：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 575);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.log);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TextBtn2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_TextTB2);
            this.Controls.Add(this.LEDPictureBox2);
            this.Controls.Add(this.LEDPictureBox);
            this.Controls.Add(this.TextBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_TextTB);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "城市交通诱导显示屏管理系统";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LEDPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LEDPictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox m_TextTB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button TextBtn;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox LEDPictureBox;
        private System.Windows.Forms.PictureBox LEDPictureBox2;
        private System.Windows.Forms.Button TextBtn2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox m_TextTB2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.TextBox log;
        private System.Windows.Forms.Label label5;
    }
}

