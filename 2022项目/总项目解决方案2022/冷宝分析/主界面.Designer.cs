namespace 冷宝分析
{
    partial class 主界面
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
            this.MenMain = new System.Windows.Forms.MenuStrip();
            this.场次管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开宝记录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.未开分析ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.基本设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenMain
            // 
            this.MenMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.场次管理ToolStripMenuItem,
            this.开宝记录ToolStripMenuItem,
            this.未开分析ToolStripMenuItem,
            this.基本设置ToolStripMenuItem});
            this.MenMain.Location = new System.Drawing.Point(0, 0);
            this.MenMain.Name = "MenMain";
            this.MenMain.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.MenMain.Size = new System.Drawing.Size(1059, 25);
            this.MenMain.TabIndex = 2;
            this.MenMain.Text = "MenuStrip1";
            // 
            // 场次管理ToolStripMenuItem
            // 
            this.场次管理ToolStripMenuItem.Name = "场次管理ToolStripMenuItem";
            this.场次管理ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.场次管理ToolStripMenuItem.Text = "场次管理";
            this.场次管理ToolStripMenuItem.Click += new System.EventHandler(this.场次管理ToolStripMenuItem_Click);
            // 
            // 开宝记录ToolStripMenuItem
            // 
            this.开宝记录ToolStripMenuItem.Name = "开宝记录ToolStripMenuItem";
            this.开宝记录ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.开宝记录ToolStripMenuItem.Text = "开宝记录";
            this.开宝记录ToolStripMenuItem.Click += new System.EventHandler(this.开宝记录ToolStripMenuItem_Click);
            // 
            // 未开分析ToolStripMenuItem
            // 
            this.未开分析ToolStripMenuItem.Name = "未开分析ToolStripMenuItem";
            this.未开分析ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.未开分析ToolStripMenuItem.Text = "未开分析";
            this.未开分析ToolStripMenuItem.Click += new System.EventHandler(this.未开分析ToolStripMenuItem_Click);
            // 
            // 基本设置ToolStripMenuItem
            // 
            this.基本设置ToolStripMenuItem.Name = "基本设置ToolStripMenuItem";
            this.基本设置ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.基本设置ToolStripMenuItem.Text = "基本设置";
            this.基本设置ToolStripMenuItem.Click += new System.EventHandler(this.基本设置ToolStripMenuItem_Click);
            // 
            // 主界面
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 677);
            this.Controls.Add(this.MenMain);
            this.IsMdiContainer = true;
            this.Name = "主界面";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "主界面";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.主界面_Load);
            this.MenMain.ResumeLayout(false);
            this.MenMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenMain;
        private System.Windows.Forms.ToolStripMenuItem 场次管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开宝记录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 未开分析ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 基本设置ToolStripMenuItem;
    }
}