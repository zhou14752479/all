namespace 主程序202202
{
    partial class POSTGET测试
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
            this.urltxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.postdatatxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.contenttypecob = new System.Windows.Forms.ComboBox();
            this.charsetcob = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.bodytxt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.refertxt = new System.Windows.Forms.TextBox();
            this.cookietxt = new System.Windows.Forms.TextBox();
            this.useragenttxt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.redirectcheck = new System.Windows.Forms.CheckBox();
            this.KeepAlivecheck = new System.Windows.Forms.CheckBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // urltxt
            // 
            this.urltxt.Location = new System.Drawing.Point(82, 52);
            this.urltxt.Name = "urltxt";
            this.urltxt.Size = new System.Drawing.Size(574, 21);
            this.urltxt.TabIndex = 0;
            this.urltxt.Text = "http://";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "url";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(662, 50);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "请求";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "data";
            // 
            // postdatatxt
            // 
            this.postdatatxt.Location = new System.Drawing.Point(82, 85);
            this.postdatatxt.Multiline = true;
            this.postdatatxt.Name = "postdatatxt";
            this.postdatatxt.Size = new System.Drawing.Size(574, 56);
            this.postdatatxt.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "conten-type";
            // 
            // contenttypecob
            // 
            this.contenttypecob.FormattingEnabled = true;
            this.contenttypecob.Items.AddRange(new object[] {
            "application/x-www-form-urlencoded",
            "application/json"});
            this.contenttypecob.Location = new System.Drawing.Point(100, 153);
            this.contenttypecob.Name = "contenttypecob";
            this.contenttypecob.Size = new System.Drawing.Size(314, 20);
            this.contenttypecob.TabIndex = 6;
            this.contenttypecob.Text = "application/x-www-form-urlencoded";
            // 
            // charsetcob
            // 
            this.charsetcob.FormattingEnabled = true;
            this.charsetcob.Items.AddRange(new object[] {
            "utf-8",
            "gb2312"});
            this.charsetcob.Location = new System.Drawing.Point(100, 185);
            this.charsetcob.Name = "charsetcob";
            this.charsetcob.Size = new System.Drawing.Size(121, 20);
            this.charsetcob.TabIndex = 8;
            this.charsetcob.Text = "utf-8";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 187);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "charset";
            // 
            // bodytxt
            // 
            this.bodytxt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bodytxt.Location = new System.Drawing.Point(0, 324);
            this.bodytxt.Multiline = true;
            this.bodytxt.Name = "bodytxt";
            this.bodytxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.bodytxt.Size = new System.Drawing.Size(807, 172);
            this.bodytxt.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 220);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "refer";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 253);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "cookie";
            // 
            // refertxt
            // 
            this.refertxt.Location = new System.Drawing.Point(82, 217);
            this.refertxt.Name = "refertxt";
            this.refertxt.Size = new System.Drawing.Size(574, 21);
            this.refertxt.TabIndex = 12;
            this.refertxt.Text = "http://";
            // 
            // cookietxt
            // 
            this.cookietxt.Location = new System.Drawing.Point(82, 250);
            this.cookietxt.Name = "cookietxt";
            this.cookietxt.Size = new System.Drawing.Size(574, 21);
            this.cookietxt.TabIndex = 13;
            // 
            // useragenttxt
            // 
            this.useragenttxt.Location = new System.Drawing.Point(82, 283);
            this.useragenttxt.Name = "useragenttxt";
            this.useragenttxt.Size = new System.Drawing.Size(703, 21);
            this.useragenttxt.TabIndex = 15;
            this.useragenttxt.Text = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) " +
    "Chrome/78.0.3904.108 Safari/537.36";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 286);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "useragent";
            // 
            // redirectcheck
            // 
            this.redirectcheck.AutoSize = true;
            this.redirectcheck.Checked = true;
            this.redirectcheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.redirectcheck.Location = new System.Drawing.Point(436, 157);
            this.redirectcheck.Name = "redirectcheck";
            this.redirectcheck.Size = new System.Drawing.Size(126, 16);
            this.redirectcheck.TabIndex = 16;
            this.redirectcheck.Text = "AllowAutoRedirect";
            this.redirectcheck.UseVisualStyleBackColor = true;
            // 
            // KeepAlivecheck
            // 
            this.KeepAlivecheck.AutoSize = true;
            this.KeepAlivecheck.Checked = true;
            this.KeepAlivecheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.KeepAlivecheck.Location = new System.Drawing.Point(568, 157);
            this.KeepAlivecheck.Name = "KeepAlivecheck";
            this.KeepAlivecheck.Size = new System.Drawing.Size(78, 16);
            this.KeepAlivecheck.TabIndex = 17;
            this.KeepAlivecheck.Text = "KeepAlive";
            this.KeepAlivecheck.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(82, 13);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(41, 16);
            this.radioButton1.TabIndex = 18;
            this.radioButton1.Text = "GET";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(140, 12);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(47, 16);
            this.radioButton2.TabIndex = 19;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "POST";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // POSTGET测试
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 496);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.KeepAlivecheck);
            this.Controls.Add(this.redirectcheck);
            this.Controls.Add(this.useragenttxt);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cookietxt);
            this.Controls.Add(this.refertxt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bodytxt);
            this.Controls.Add(this.charsetcob);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.contenttypecob);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.postdatatxt);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.urltxt);
            this.Name = "POSTGET测试";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "POSTGET测试";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox urltxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox postdatatxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox contenttypecob;
        private System.Windows.Forms.ComboBox charsetcob;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox bodytxt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox refertxt;
        private System.Windows.Forms.TextBox cookietxt;
        private System.Windows.Forms.TextBox useragenttxt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox redirectcheck;
        private System.Windows.Forms.CheckBox KeepAlivecheck;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
    }
}