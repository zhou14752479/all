namespace 主程序
{
    partial class 工厂针织服装软件
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("经编起绒织物");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("经编网服织物");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("经编丝绒织物");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("经编毛圈织物");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("经编", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("平针");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("罗纹");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("双反面");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("原组织", new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7,
            treeNode8});
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("变化平针");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("双罗纹");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("变化组织", new System.Windows.Forms.TreeNode[] {
            treeNode10,
            treeNode11});
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("提花、衬垫、长毛绒等");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("各种复合组织");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("花色组织", new System.Windows.Forms.TreeNode[] {
            treeNode13,
            treeNode14});
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("纬编", new System.Windows.Forms.TreeNode[] {
            treeNode9,
            treeNode12,
            treeNode15});
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.button3 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(871, 31);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("黑体", 13F);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "成型组织服装(羊毛衫)";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 31);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.splitContainer1.Panel2.Controls.Add(this.button3);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.button2);
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.textBox2);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.textBox1);
            this.splitContainer1.Size = new System.Drawing.Size(871, 546);
            this.splitContainer1.SplitterDistance = 289;
            this.splitContainer1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox1.Controls.Add(this.treeView1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(289, 546);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "菜单信息";
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("黑体", 13F);
            this.treeView1.ItemHeight = 30;
            this.treeView1.Location = new System.Drawing.Point(3, 17);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "1";
            treeNode1.Text = "经编起绒织物";
            treeNode2.Name = "2";
            treeNode2.Text = "经编网服织物";
            treeNode3.Name = "3";
            treeNode3.Text = "经编丝绒织物";
            treeNode4.Name = "4";
            treeNode4.Text = "经编毛圈织物";
            treeNode5.Name = "0";
            treeNode5.Text = "经编";
            treeNode6.Name = "7";
            treeNode6.Text = "平针";
            treeNode7.Name = "8";
            treeNode7.Text = "罗纹";
            treeNode8.Name = "9";
            treeNode8.Text = "双反面";
            treeNode9.Name = "6";
            treeNode9.Text = "原组织";
            treeNode10.Name = "11";
            treeNode10.Text = "变化平针";
            treeNode11.Name = "12";
            treeNode11.Text = "双罗纹";
            treeNode12.Name = "10";
            treeNode12.Text = "变化组织";
            treeNode13.Name = "14";
            treeNode13.Text = "提花、衬垫、长毛绒等";
            treeNode14.Name = "15";
            treeNode14.Text = "各种复合组织";
            treeNode15.Name = "13";
            treeNode15.Text = "花色组织";
            treeNode16.Name = "5";
            treeNode16.Text = "纬编";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode16});
            this.treeView1.Size = new System.Drawing.Size(283, 526);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(9, 511);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "点击修改";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("黑体", 12F);
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(218, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "请先选择选项";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(9, 269);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "选择图片";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(9, 467);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "点击保存";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(90, 236);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(442, 298);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("黑体", 12F);
            this.label4.Location = new System.Drawing.Point(44, 236);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "图片";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("黑体", 12F);
            this.label3.Location = new System.Drawing.Point(12, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "风格特征";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(90, 130);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(442, 100);
            this.textBox2.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("黑体", 12F);
            this.label2.Location = new System.Drawing.Point(12, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "结构特征";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(90, 28);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(442, 96);
            this.textBox1.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // 工厂针织服装软件
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 577);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Name = "工厂针织服装软件";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "工厂针织服装软件";
            this.Load += new System.EventHandler(this.工厂针织服装软件_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button3;
    }
}