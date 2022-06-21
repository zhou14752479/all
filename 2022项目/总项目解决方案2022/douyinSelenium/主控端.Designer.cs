namespace douyinSelenium
{
    partial class 主控端
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(主控端));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.选中ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.取消选中ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导入代理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全部退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出当前链接ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.更换此账号cookieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.隐藏窗口ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示窗口ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button7 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "代理密码";
            this.columnHeader9.Width = 100;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "代理IP";
            this.columnHeader7.Width = 100;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "返回信息";
            this.columnHeader6.Width = 200;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Cookies";
            this.columnHeader5.Width = 300;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "当前状态";
            this.columnHeader4.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "账号昵称";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "账号ID";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            this.columnHeader1.Width = 50;
            // 
            // listView1
            // 
            this.listView1.AllowDrop = true;
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Font = new System.Drawing.Font("宋体", 10F);
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 99);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1165, 584);
            this.listView1.TabIndex = 230;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView1_DragDrop);
            this.listView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView1_DragEnter);
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "代理账号";
            this.columnHeader8.Width = 100;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.选中ToolStripMenuItem,
            this.取消选中ToolStripMenuItem,
            this.删除ToolStripMenuItem,
            this.导入代理ToolStripMenuItem,
            this.全部退出ToolStripMenuItem,
            this.退出当前链接ToolStripMenuItem,
            this.更换此账号cookieToolStripMenuItem,
            this.隐藏窗口ToolStripMenuItem,
            this.显示窗口ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(176, 202);
            // 
            // 选中ToolStripMenuItem
            // 
            this.选中ToolStripMenuItem.Name = "选中ToolStripMenuItem";
            this.选中ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.选中ToolStripMenuItem.Text = "选中";
            this.选中ToolStripMenuItem.Click += new System.EventHandler(this.选中ToolStripMenuItem_Click);
            // 
            // 取消选中ToolStripMenuItem
            // 
            this.取消选中ToolStripMenuItem.Name = "取消选中ToolStripMenuItem";
            this.取消选中ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.取消选中ToolStripMenuItem.Text = "取消选中";
            this.取消选中ToolStripMenuItem.Click += new System.EventHandler(this.取消选中ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // 导入代理ToolStripMenuItem
            // 
            this.导入代理ToolStripMenuItem.Name = "导入代理ToolStripMenuItem";
            this.导入代理ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.导入代理ToolStripMenuItem.Text = "导入代理";
            this.导入代理ToolStripMenuItem.Click += new System.EventHandler(this.导入代理ToolStripMenuItem_Click);
            // 
            // 全部退出ToolStripMenuItem
            // 
            this.全部退出ToolStripMenuItem.Name = "全部退出ToolStripMenuItem";
            this.全部退出ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.全部退出ToolStripMenuItem.Text = "全部退出";
            this.全部退出ToolStripMenuItem.Click += new System.EventHandler(this.全部退出ToolStripMenuItem_Click);
            // 
            // 退出当前链接ToolStripMenuItem
            // 
            this.退出当前链接ToolStripMenuItem.Name = "退出当前链接ToolStripMenuItem";
            this.退出当前链接ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.退出当前链接ToolStripMenuItem.Text = "退出当前链接";
            this.退出当前链接ToolStripMenuItem.Click += new System.EventHandler(this.退出当前链接ToolStripMenuItem_Click);
            // 
            // 更换此账号cookieToolStripMenuItem
            // 
            this.更换此账号cookieToolStripMenuItem.Name = "更换此账号cookieToolStripMenuItem";
            this.更换此账号cookieToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.更换此账号cookieToolStripMenuItem.Text = "更换此账号cookie";
            this.更换此账号cookieToolStripMenuItem.Click += new System.EventHandler(this.更换此账号cookieToolStripMenuItem_Click);
            // 
            // 隐藏窗口ToolStripMenuItem
            // 
            this.隐藏窗口ToolStripMenuItem.Name = "隐藏窗口ToolStripMenuItem";
            this.隐藏窗口ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.隐藏窗口ToolStripMenuItem.Text = "隐藏窗口";
            this.隐藏窗口ToolStripMenuItem.Click += new System.EventHandler(this.隐藏窗口ToolStripMenuItem_Click);
            // 
            // 显示窗口ToolStripMenuItem
            // 
            this.显示窗口ToolStripMenuItem.Name = "显示窗口ToolStripMenuItem";
            this.显示窗口ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.显示窗口ToolStripMenuItem.Text = "显示窗口";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(30, 56);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 12;
            this.checkBox1.Text = "显示页面";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button7
            // 
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button7.Location = new System.Drawing.Point(1064, 12);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(89, 28);
            this.button7.TabIndex = 11;
            this.button7.Text = "退出全部链接";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button4.Location = new System.Drawing.Point(981, 12);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(77, 28);
            this.button4.TabIndex = 10;
            this.button4.Text = "进入链接";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button5.Location = new System.Drawing.Point(887, 12);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(88, 28);
            this.button5.TabIndex = 9;
            this.button5.Text = "重登录刷新CK";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button6.Location = new System.Drawing.Point(804, 12);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(77, 28);
            this.button6.TabIndex = 8;
            this.button6.Text = "登录账号";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(756, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "秒";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("宋体", 10F);
            this.textBox2.Location = new System.Drawing.Point(719, 17);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(31, 23);
            this.textBox2.TabIndex = 6;
            this.textBox2.Text = "2";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(660, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "随机延迟";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 10F);
            this.textBox1.Location = new System.Drawing.Point(331, 16);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(323, 23);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "https://www.douyin.com/?enter=guide";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(272, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "链接地址";
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Location = new System.Drawing.Point(191, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(77, 28);
            this.button3.TabIndex = 2;
            this.button3.Text = "删除选中";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(108, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(77, 28);
            this.button2.TabIndex = 1;
            this.button2.Text = "反选";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(25, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(77, 28);
            this.button1.TabIndex = 0;
            this.button1.Text = "全选";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1165, 99);
            this.panel1.TabIndex = 229;
            // 
            // 主控端
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1165, 683);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "主控端";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "主控端";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.主控端_FormClosing);
            this.Load += new System.EventHandler(this.主控端_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 选中ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 取消选中ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导入代理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部退出ToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem 退出当前链接ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 更换此账号cookieToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 隐藏窗口ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示窗口ToolStripMenuItem;
    }
}