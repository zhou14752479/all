namespace CEF主程序
{
    partial class 京东E卡查询
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView1 = new System.Windows.Forms.ListView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button7 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "状态";
            this.columnHeader3.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "卡密";
            this.columnHeader2.Width = 150;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            this.columnHeader1.Width = 40;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 132);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(735, 364);
            this.listView1.TabIndex = 64;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(735, 132);
            this.panel1.TabIndex = 63;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(369, 41);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(88, 30);
            this.button7.TabIndex = 74;
            this.button7.Text = "停止抓取";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(285, 12);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 73;
            this.button4.Text = "导入文本";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(267, 21);
            this.textBox1.TabIndex = 72;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(191, 41);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(88, 30);
            this.button3.TabIndex = 71;
            this.button3.Text = "导出数据";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(102, 41);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 30);
            this.button2.TabIndex = 70;
            this.button2.Text = "暂停/继续";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(280, 41);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(88, 30);
            this.button6.TabIndex = 69;
            this.button6.Text = "清空记录";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 41);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 30);
            this.button1.TabIndex = 68;
            this.button1.Text = "开始抓取";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.button5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button5.Location = new System.Drawing.Point(366, 12);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(91, 26);
            this.button5.TabIndex = 67;
            this.button5.Text = "登录";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "查询时间";
            this.columnHeader4.Width = 150;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "余额";
            this.columnHeader5.Width = 80;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "有效期";
            this.columnHeader6.Width = 150;
            // 
            // textBox2
            // 
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.textBox2.Location = new System.Drawing.Point(473, 0);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(262, 132);
            this.textBox2.TabIndex = 75;
            // 
            // 京东E卡查询
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 496);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.panel1);
            this.Name = "京东E卡查询";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "京东E卡查询--未登录";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.京东E卡查询_FormClosing);
            this.Load += new System.EventHandler(this.京东E卡查询_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.TextBox textBox2;
    }
}