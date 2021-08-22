namespace CEF主程序
{
    partial class CEF主程序
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.后退ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.前进ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.获取cookieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.获取request参数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.后退ToolStripMenuItem,
            this.前进ToolStripMenuItem,
            this.获取cookieToolStripMenuItem,
            this.获取request参数ToolStripMenuItem,
            this.toolStripTextBox1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1081, 27);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 后退ToolStripMenuItem
            // 
            this.后退ToolStripMenuItem.Name = "后退ToolStripMenuItem";
            this.后退ToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.后退ToolStripMenuItem.Text = "后退";
            // 
            // 前进ToolStripMenuItem
            // 
            this.前进ToolStripMenuItem.Name = "前进ToolStripMenuItem";
            this.前进ToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.前进ToolStripMenuItem.Text = "前进";
            this.前进ToolStripMenuItem.Click += new System.EventHandler(this.前进ToolStripMenuItem_Click);
            // 
            // 获取cookieToolStripMenuItem
            // 
            this.获取cookieToolStripMenuItem.Name = "获取cookieToolStripMenuItem";
            this.获取cookieToolStripMenuItem.Size = new System.Drawing.Size(83, 23);
            this.获取cookieToolStripMenuItem.Text = "获取cookie";
            this.获取cookieToolStripMenuItem.Click += new System.EventHandler(this.获取cookieToolStripMenuItem_Click);
            // 
            // 获取request参数ToolStripMenuItem
            // 
            this.获取request参数ToolStripMenuItem.Name = "获取request参数ToolStripMenuItem";
            this.获取request参数ToolStripMenuItem.Size = new System.Drawing.Size(112, 23);
            this.获取request参数ToolStripMenuItem.Text = "获取request参数";
            this.获取request参数ToolStripMenuItem.Click += new System.EventHandler(this.获取request参数ToolStripMenuItem_Click);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(500, 23);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1081, 598);
            this.panel1.TabIndex = 2;
            // 
            // CEF主程序
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1081, 625);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "CEF主程序";
            this.Text = "CEF主程序";
            this.Load += new System.EventHandler(this.CEF主程序_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 后退ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 前进ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 获取cookieToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem 获取request参数ToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
    }
}