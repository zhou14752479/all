namespace TRC监控
{
    partial class TRC监控
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.复制付款地址ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.复制收款地址ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除此行ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出文本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label4 = new System.Windows.Forms.Label();
            this.复制交易idToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(820, 260);
            this.listView1.TabIndex = 55;
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
            this.columnHeader2.Text = "地址";
            this.columnHeader2.Width = 260;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "时间";
            this.columnHeader3.Width = 120;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "币种";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "数量";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "备注";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "id";
            this.columnHeader7.Width = 200;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "付款地址";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制付款地址ToolStripMenuItem,
            this.复制收款地址ToolStripMenuItem,
            this.删除此行ToolStripMenuItem,
            this.导出文本ToolStripMenuItem,
            this.复制交易idToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 114);
            // 
            // 复制付款地址ToolStripMenuItem
            // 
            this.复制付款地址ToolStripMenuItem.Name = "复制付款地址ToolStripMenuItem";
            this.复制付款地址ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.复制付款地址ToolStripMenuItem.Text = "复制付款地址";
            this.复制付款地址ToolStripMenuItem.Click += new System.EventHandler(this.复制付款地址ToolStripMenuItem_Click);
            // 
            // 复制收款地址ToolStripMenuItem
            // 
            this.复制收款地址ToolStripMenuItem.Name = "复制收款地址ToolStripMenuItem";
            this.复制收款地址ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.复制收款地址ToolStripMenuItem.Text = "复制收款地址";
            this.复制收款地址ToolStripMenuItem.Click += new System.EventHandler(this.复制收款地址ToolStripMenuItem_Click);
            // 
            // 删除此行ToolStripMenuItem
            // 
            this.删除此行ToolStripMenuItem.Name = "删除此行ToolStripMenuItem";
            this.删除此行ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.删除此行ToolStripMenuItem.Text = "删除此行";
            this.删除此行ToolStripMenuItem.Click += new System.EventHandler(this.删除此行ToolStripMenuItem_Click);
            // 
            // 导出文本ToolStripMenuItem
            // 
            this.导出文本ToolStripMenuItem.Name = "导出文本ToolStripMenuItem";
            this.导出文本ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.导出文本ToolStripMenuItem.Text = "导出文本";
            this.导出文本ToolStripMenuItem.Click += new System.EventHandler(this.导出文本ToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(597, 283);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 24);
            this.button1.TabIndex = 56;
            this.button1.Text = "添加网址";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(409, 320);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 28);
            this.button2.TabIndex = 57;
            this.button2.Text = "开启监控";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(503, 320);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(88, 28);
            this.button3.TabIndex = 58;
            this.button3.Text = "停止监控";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10F);
            this.label1.Location = new System.Drawing.Point(20, 286);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 14);
            this.label1.TabIndex = 59;
            this.label1.Text = "地址：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(66, 283);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(297, 21);
            this.textBox1.TabIndex = 60;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(413, 283);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(178, 21);
            this.textBox2.TabIndex = 62;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10F);
            this.label2.Location = new System.Drawing.Point(367, 286);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 14);
            this.label2.TabIndex = 61;
            this.label2.Text = "备注：";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(66, 320);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(46, 21);
            this.textBox3.TabIndex = 64;
            this.textBox3.Text = "5";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10F);
            this.label3.Location = new System.Drawing.Point(20, 323);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 14);
            this.label3.TabIndex = 63;
            this.label3.Text = "间隔：";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(684, 283);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(81, 24);
            this.button4.TabIndex = 65;
            this.button4.Text = "一键导入";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(119, 323);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 66;
            this.label4.Text = "未启动";
            // 
            // 复制交易idToolStripMenuItem
            // 
            this.复制交易idToolStripMenuItem.Name = "复制交易idToolStripMenuItem";
            this.复制交易idToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.复制交易idToolStripMenuItem.Text = "复制交易id";
            this.复制交易idToolStripMenuItem.Click += new System.EventHandler(this.复制交易idToolStripMenuItem_Click);
            // 
            // TRC监控
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 374);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView1);
            this.MaximizeBox = false;
            this.Name = "TRC监控";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TRC监控";
            this.Load += new System.EventHandler(this.TRC监控_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ToolStripMenuItem 复制付款地址ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 复制收款地址ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除此行ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出文本ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 复制交易idToolStripMenuItem;
    }
}