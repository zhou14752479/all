namespace 常用代码查询
{
    partial class 常用代码查询
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.search_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.result_text = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.add_btn = new System.Windows.Forms.Button();
            this.body_text = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.title_text = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button2);
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            this.splitContainer1.Panel1.Controls.Add(this.listView1);
            this.splitContainer1.Panel1.Controls.Add(this.search_btn);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.textBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(977, 548);
            this.splitContainer1.SplitterDistance = 375;
            this.splitContainer1.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("宋体", 10F);
            this.button2.Location = new System.Drawing.Point(161, 114);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(90, 31);
            this.button2.TabIndex = 341;
            this.button2.Text = "删除";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 10F);
            this.button1.Location = new System.Drawing.Point(65, 114);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 31);
            this.button1.TabIndex = 340;
            this.button1.Text = "修改";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listView1.Font = new System.Drawing.Font("宋体", 10F);
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 175);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(375, 373);
            this.listView1.TabIndex = 339;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            this.columnHeader1.Width = 40;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "标题";
            this.columnHeader2.Width = 300;
            // 
            // search_btn
            // 
            this.search_btn.Font = new System.Drawing.Font("宋体", 10F);
            this.search_btn.Location = new System.Drawing.Point(65, 77);
            this.search_btn.Name = "search_btn";
            this.search_btn.Size = new System.Drawing.Size(186, 31);
            this.search_btn.TabIndex = 5;
            this.search_btn.Text = "查找";
            this.search_btn.UseVisualStyleBackColor = true;
            this.search_btn.Click += new System.EventHandler(this.search_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "关键字：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(65, 15);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(200, 40);
            this.textBox1.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(598, 548);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.result_text);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(590, 522);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "---代码预览--- ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // result_text
            // 
            this.result_text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.result_text.Font = new System.Drawing.Font("宋体", 11F);
            this.result_text.Location = new System.Drawing.Point(3, 3);
            this.result_text.Multiline = true;
            this.result_text.Name = "result_text";
            this.result_text.Size = new System.Drawing.Size(584, 516);
            this.result_text.TabIndex = 5;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.add_btn);
            this.tabPage2.Controls.Add(this.body_text);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.title_text);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(590, 522);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "---添加代码--- ";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // add_btn
            // 
            this.add_btn.Font = new System.Drawing.Font("宋体", 10F);
            this.add_btn.Location = new System.Drawing.Point(9, 388);
            this.add_btn.Name = "add_btn";
            this.add_btn.Size = new System.Drawing.Size(91, 36);
            this.add_btn.TabIndex = 340;
            this.add_btn.Text = "添加";
            this.add_btn.UseVisualStyleBackColor = true;
            this.add_btn.Click += new System.EventHandler(this.add_btn_Click);
            // 
            // body_text
            // 
            this.body_text.Location = new System.Drawing.Point(9, 70);
            this.body_text.Multiline = true;
            this.body_text.Name = "body_text";
            this.body_text.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.body_text.Size = new System.Drawing.Size(558, 312);
            this.body_text.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10F);
            this.label2.Location = new System.Drawing.Point(6, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 14);
            this.label2.TabIndex = 7;
            this.label2.Text = "内容：";
            // 
            // title_text
            // 
            this.title_text.Location = new System.Drawing.Point(58, 17);
            this.title_text.Name = "title_text";
            this.title_text.Size = new System.Drawing.Size(352, 21);
            this.title_text.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10F);
            this.label3.Location = new System.Drawing.Point(6, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 14);
            this.label3.TabIndex = 5;
            this.label3.Text = "标题：";
            // 
            // 常用代码查询
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 548);
            this.Controls.Add(this.splitContainer1);
            this.Name = "常用代码查询";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "常用代码查询";
            this.Load += new System.EventHandler(this.常用代码查询_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button search_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button add_btn;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox result_text;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox body_text;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox title_text;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}