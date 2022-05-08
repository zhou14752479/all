namespace 阿里巴巴采集软件
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(主界面));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.商品采集ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.评论采集ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.商品采集ToolStripMenuItem,
            this.评论采集ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1070, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 商品采集ToolStripMenuItem
            // 
            this.商品采集ToolStripMenuItem.Name = "商品采集ToolStripMenuItem";
            this.商品采集ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.商品采集ToolStripMenuItem.Text = "商品采集";
            this.商品采集ToolStripMenuItem.Click += new System.EventHandler(this.商品采集ToolStripMenuItem_Click);
            // 
            // 评论采集ToolStripMenuItem
            // 
            this.评论采集ToolStripMenuItem.Name = "评论采集ToolStripMenuItem";
            this.评论采集ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.评论采集ToolStripMenuItem.Text = "评论采集";
            this.评论采集ToolStripMenuItem.Click += new System.EventHandler(this.评论采集ToolStripMenuItem_Click);
            // 
            // 主界面
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1070, 642);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "主界面";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "思忆1688采集软件";
            this.Load += new System.EventHandler(this.主界面_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 商品采集ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 评论采集ToolStripMenuItem;
    }
}