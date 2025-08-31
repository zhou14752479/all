namespace 微博Cookie
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.p1 = new System.Windows.Forms.Panel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.关闭图片ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清理cookieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清理历史记录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.读取cookieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.p2 = new System.Windows.Forms.Panel();
            this.p1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // p1
            // 
            this.p1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.p1.Controls.Add(this.toolStrip2);
            this.p1.Dock = System.Windows.Forms.DockStyle.Top;
            this.p1.Location = new System.Drawing.Point(0, 0);
            this.p1.Name = "p1";
            this.p1.Size = new System.Drawing.Size(939, 43);
            this.p1.TabIndex = 0;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton4,
            this.toolStripButton5,
            this.toolStripDropDownButton1,
            this.toolStripLabel1});
            this.toolStrip2.Location = new System.Drawing.Point(1, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(249, 25);
            this.toolStrip2.TabIndex = 0;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(60, 22);
            this.toolStripButton4.Text = "快捷输入";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(36, 22);
            this.toolStripButton5.Text = "登录";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关闭图片ToolStripMenuItem,
            this.清理cookieToolStripMenuItem,
            this.清理历史记录ToolStripMenuItem,
            this.读取cookieToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(45, 22);
            this.toolStripDropDownButton1.Text = "菜单";
            // 
            // 关闭图片ToolStripMenuItem
            // 
            this.关闭图片ToolStripMenuItem.Name = "关闭图片ToolStripMenuItem";
            this.关闭图片ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.关闭图片ToolStripMenuItem.Text = "关闭图片";
            this.关闭图片ToolStripMenuItem.Click += new System.EventHandler(this.关闭图片ToolStripMenuItem_Click);
            // 
            // 清理cookieToolStripMenuItem
            // 
            this.清理cookieToolStripMenuItem.Name = "清理cookieToolStripMenuItem";
            this.清理cookieToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.清理cookieToolStripMenuItem.Text = "清理cookie";
            this.清理cookieToolStripMenuItem.Click += new System.EventHandler(this.清理cookieToolStripMenuItem_Click);
            // 
            // 清理历史记录ToolStripMenuItem
            // 
            this.清理历史记录ToolStripMenuItem.Name = "清理历史记录ToolStripMenuItem";
            this.清理历史记录ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.清理历史记录ToolStripMenuItem.Text = "清理历史记录";
            this.清理历史记录ToolStripMenuItem.Click += new System.EventHandler(this.清理历史记录ToolStripMenuItem_Click);
            // 
            // 读取cookieToolStripMenuItem
            // 
            this.读取cookieToolStripMenuItem.ForeColor = System.Drawing.Color.Red;
            this.读取cookieToolStripMenuItem.Name = "读取cookieToolStripMenuItem";
            this.读取cookieToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.读取cookieToolStripMenuItem.Text = "开启读取cookie";
            this.读取cookieToolStripMenuItem.Click += new System.EventHandler(this.读取cookieToolStripMenuItem_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(96, 22);
            this.toolStripLabel1.Text = "toolStripLabel1";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // p2
            // 
            this.p2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.p2.Location = new System.Drawing.Point(0, 43);
            this.p2.Name = "p2";
            this.p2.Size = new System.Drawing.Size(939, 650);
            this.p2.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 693);
            this.Controls.Add(this.p2);
            this.Controls.Add(this.p1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.p1.ResumeLayout(false);
            this.p1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel p1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.Panel p2;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem 关闭图片ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清理cookieToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清理历史记录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 读取cookieToolStripMenuItem;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    }
}