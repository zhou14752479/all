namespace 基鹿工具箱
{
    partial class 后台主界面
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
            this.标题导入ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.生意参谋指数导入ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.图标设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.标题导入ToolStripMenuItem,
            this.生意参谋指数导入ToolStripMenuItem,
            this.图标设置ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1149, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 标题导入ToolStripMenuItem
            // 
            this.标题导入ToolStripMenuItem.Name = "标题导入ToolStripMenuItem";
            this.标题导入ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.标题导入ToolStripMenuItem.Text = "标题导入";
            this.标题导入ToolStripMenuItem.Click += new System.EventHandler(this.标题导入ToolStripMenuItem_Click);
            // 
            // 生意参谋指数导入ToolStripMenuItem
            // 
            this.生意参谋指数导入ToolStripMenuItem.Name = "生意参谋指数导入ToolStripMenuItem";
            this.生意参谋指数导入ToolStripMenuItem.Size = new System.Drawing.Size(116, 21);
            this.生意参谋指数导入ToolStripMenuItem.Text = "生意参谋指数导入";
            this.生意参谋指数导入ToolStripMenuItem.Click += new System.EventHandler(this.生意参谋指数导入ToolStripMenuItem_Click);
            // 
            // 图标设置ToolStripMenuItem
            // 
            this.图标设置ToolStripMenuItem.Name = "图标设置ToolStripMenuItem";
            this.图标设置ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.图标设置ToolStripMenuItem.Text = "图标设置";
            this.图标设置ToolStripMenuItem.Click += new System.EventHandler(this.图标设置ToolStripMenuItem_Click);
            // 
            // 后台主界面
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1149, 817);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.Name = "后台主界面";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "后台主界面";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 标题导入ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 生意参谋指数导入ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图标设置ToolStripMenuItem;
    }
}