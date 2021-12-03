namespace 股票数据管理
{
    partial class 股票数据管理
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.导入数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出查询数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空数据库全部数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1111, 100);
            this.panel1.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(205)))), ((int)(((byte)(0)))));
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(969, 40);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(130, 38);
            this.button2.TabIndex = 19;
            this.button2.Text = "添加今日数据";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导入数据ToolStripMenuItem,
            this.导出查询数据ToolStripMenuItem,
            this.清空数据库全部数据ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1111, 25);
            this.menuStrip1.TabIndex = 18;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 导入数据ToolStripMenuItem
            // 
            this.导入数据ToolStripMenuItem.Name = "导入数据ToolStripMenuItem";
            this.导入数据ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.导入数据ToolStripMenuItem.Text = "导入数据";
            this.导入数据ToolStripMenuItem.Click += new System.EventHandler(this.导入数据ToolStripMenuItem_Click);
            // 
            // 导出查询数据ToolStripMenuItem
            // 
            this.导出查询数据ToolStripMenuItem.Name = "导出查询数据ToolStripMenuItem";
            this.导出查询数据ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.导出查询数据ToolStripMenuItem.Text = "导出查询数据";
            this.导出查询数据ToolStripMenuItem.Click += new System.EventHandler(this.导出查询数据ToolStripMenuItem_Click);
            // 
            // 清空数据库全部数据ToolStripMenuItem
            // 
            this.清空数据库全部数据ToolStripMenuItem.Name = "清空数据库全部数据ToolStripMenuItem";
            this.清空数据库全部数据ToolStripMenuItem.Size = new System.Drawing.Size(128, 21);
            this.清空数据库全部数据ToolStripMenuItem.Text = "清空数据库全部数据";
            this.清空数据库全部数据ToolStripMenuItem.Click += new System.EventHandler(this.清空数据库全部数据ToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(205)))), ((int)(((byte)(0)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(666, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 38);
            this.button1.TabIndex = 17;
            this.button1.Text = "更新数据";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 100);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1111, 575);
            this.dataGridView1.TabIndex = 379;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10F);
            this.label1.Location = new System.Drawing.Point(11, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 14);
            this.label1.TabIndex = 20;
            this.label1.Text = "今日预测高点=昨日昨收×";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(185, 51);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(60, 21);
            this.textBox1.TabIndex = 21;
            this.textBox1.Text = "1";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(496, 51);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(60, 21);
            this.textBox2.TabIndex = 23;
            this.textBox2.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10F);
            this.label2.Location = new System.Drawing.Point(294, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(196, 14);
            this.label2.TabIndex = 22;
            this.label2.Text = "今日预测买点=今日预测高点×";
            // 
            // 股票数据管理
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1111, 675);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Name = "股票数据管理";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "股票数据管理";
            this.Load += new System.EventHandler(this.股票数据管理_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 导入数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出查询数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清空数据库全部数据ToolStripMenuItem;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
    }
}