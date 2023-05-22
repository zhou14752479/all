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
            this.导出数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.后退ToolStripMenuItem,
            this.前进ToolStripMenuItem,
            this.获取cookieToolStripMenuItem,
            this.获取request参数ToolStripMenuItem,
            this.导出数据ToolStripMenuItem,
            this.toolStripTextBox1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1302, 27);
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
            // 导出数据ToolStripMenuItem
            // 
            this.导出数据ToolStripMenuItem.Name = "导出数据ToolStripMenuItem";
            this.导出数据ToolStripMenuItem.Size = new System.Drawing.Size(68, 23);
            this.导出数据ToolStripMenuItem.Text = "导出数据";
            this.导出数据ToolStripMenuItem.Click += new System.EventHandler(this.导出数据ToolStripMenuItem_Click);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(200, 23);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1302, 795);
            this.panel1.TabIndex = 2;
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.Color.White;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Right;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(1012, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(290, 795);
            this.listView1.TabIndex = 16;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            this.columnHeader1.Width = 40;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "UID";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "抖音号";
            this.columnHeader4.Width = 70;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "昵称";
            this.columnHeader5.Width = 80;
            // 
            // CEF主程序
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1302, 822);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "CEF主程序";
            this.Text = "CEF主程序";
            this.Load += new System.EventHandler(this.CEF主程序_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem 导出数据ToolStripMenuItem;
        internal System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
    }
}