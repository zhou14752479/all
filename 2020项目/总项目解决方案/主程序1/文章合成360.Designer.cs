namespace 主程序1
{
    partial class 文章合成360
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
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(375, 449);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(168, 29);
            this.button5.TabIndex = 38;
            this.button5.Text = "清空";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(23, 363);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(329, 22);
            this.textBox1.TabIndex = 37;
            this.textBox1.Text = "C:\\Users\\86176\\Desktop\\关键词888888888.txt";
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 394);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 40;
            this.label1.Text = "未开始";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(358, 363);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 23);
            this.button1.TabIndex = 39;
            this.button1.Text = "导入关键词";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(201, 448);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(168, 30);
            this.button3.TabIndex = 36;
            this.button3.Text = "导出";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(27, 448);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(168, 29);
            this.button2.TabIndex = 35;
            this.button2.Text = "开始";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 12);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(583, 274);
            this.textBox2.TabIndex = 41;
            // 
            // 文章合成360
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 528);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Name = "文章合成360";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "文章合成360";
            this.Load += new System.EventHandler(this.文章合成360_Load);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox2;
    }
}