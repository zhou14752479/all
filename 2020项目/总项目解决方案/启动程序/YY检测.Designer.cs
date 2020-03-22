namespace 启动程序
{
    partial class YY检测
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.重新扫描ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.复制串码ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.复制网址ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 405);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(114, 29);
            this.button5.TabIndex = 38;
            this.button5.Text = "清空";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 268);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(261, 22);
            this.textBox1.TabIndex = 37;
            // 
            // 重新扫描ToolStripMenuItem
            // 
            this.重新扫描ToolStripMenuItem.Name = "重新扫描ToolStripMenuItem";
            this.重新扫描ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.重新扫描ToolStripMenuItem.Text = "重新扫描";
            // 
            // 复制串码ToolStripMenuItem
            // 
            this.复制串码ToolStripMenuItem.Name = "复制串码ToolStripMenuItem";
            this.复制串码ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.复制串码ToolStripMenuItem.Text = "复制串码";
            // 
            // 复制网址ToolStripMenuItem
            // 
            this.复制网址ToolStripMenuItem.Name = "复制网址ToolStripMenuItem";
            this.复制网址ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.复制网址ToolStripMenuItem.Text = "复制网址";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制网址ToolStripMenuItem,
            this.复制串码ToolStripMenuItem,
            this.重新扫描ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 70);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(414, 249);
            this.listView1.TabIndex = 34;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            this.columnHeader1.Width = 40;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "账号";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "类型";
            this.columnHeader3.Width = 200;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 461);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 40;
            this.label1.Text = "未开始";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(279, 268);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(62, 23);
            this.button1.TabIndex = 39;
            this.button1.Text = "导入";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 369);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(114, 30);
            this.button3.TabIndex = 36;
            this.button3.Text = "暂停";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 319);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(114, 29);
            this.button2.TabIndex = 35;
            this.button2.Text = "开始";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(132, 370);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(114, 29);
            this.button4.TabIndex = 41;
            this.button4.Text = "继续";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(132, 405);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(114, 29);
            this.button6.TabIndex = 42;
            this.button6.Text = "清空";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // YY检测
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 488);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Name = "YY检测";
            this.Text = "YY检测";
            this.Load += new System.EventHandler(this.YY检测_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripMenuItem 重新扫描ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 复制串码ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 复制网址ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button6;
    }
}