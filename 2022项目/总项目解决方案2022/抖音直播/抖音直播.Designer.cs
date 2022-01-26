namespace 抖音直播
{
    partial class 抖音直播
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
            this.time_label = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.name_txt = new System.Windows.Forms.TextBox();
            this.shi_txt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // time_label
            // 
            this.time_label.AutoSize = true;
            this.time_label.Font = new System.Drawing.Font("微软雅黑", 15F);
            this.time_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.time_label.Location = new System.Drawing.Point(262, 724);
            this.time_label.Name = "time_label";
            this.time_label.Size = new System.Drawing.Size(214, 27);
            this.time_label.TabIndex = 0;
            this.time_label.Text = "2022-01-26 20:18:30";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 15F);
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(12, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(812, 27);
            this.label2.TabIndex = 2;
            this.label2.Text = "欢迎来到思忆抖音直播间，点击关注，弹幕发送姓名，系统自动通过大数据进行姓名作诗！";
            // 
            // name_txt
            // 
            this.name_txt.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Bold);
            this.name_txt.Location = new System.Drawing.Point(277, 101);
            this.name_txt.Name = "name_txt";
            this.name_txt.Size = new System.Drawing.Size(219, 38);
            this.name_txt.TabIndex = 3;
            this.name_txt.Text = "周凯";
            this.name_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // shi_txt
            // 
            this.shi_txt.Font = new System.Drawing.Font("微软雅黑 Light", 50F, System.Drawing.FontStyle.Italic);
            this.shi_txt.Location = new System.Drawing.Point(32, 164);
            this.shi_txt.Multiline = true;
            this.shi_txt.Name = "shi_txt";
            this.shi_txt.Size = new System.Drawing.Size(768, 403);
            this.shi_txt.TabIndex = 4;
            this.shi_txt.Text = "周公不为公 ，\r\n凯入系名王 。\r\n周公不为公 ，\r\n凯入系名王 。";
            this.shi_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 20F);
            this.label3.Location = new System.Drawing.Point(178, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 27);
            this.label3.TabIndex = 5;
            this.label3.Text = "姓名：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 20F);
            this.label4.Location = new System.Drawing.Point(512, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 27);
            this.label4.TabIndex = 6;
            this.label4.Text = "作诗：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 15F);
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(12, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(812, 27);
            this.label1.TabIndex = 7;
            this.label1.Text = "欢迎来到思忆抖音直播间，点击关注，弹幕发送姓名，系统自动通过大数据进行姓名作诗！";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(611, 108);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // 抖音直播
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(842, 760);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.shi_txt);
            this.Controls.Add(this.name_txt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.time_label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "抖音直播";
            this.Text = "抖音直播";
            this.Load += new System.EventHandler(this.抖音直播_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.抖音直播_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.抖音直播_MouseMove);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label time_label;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox name_txt;
        private System.Windows.Forms.TextBox shi_txt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}