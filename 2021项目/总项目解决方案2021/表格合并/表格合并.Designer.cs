namespace 表格合并
{
    partial class 表格合并
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
            this.button1 = new System.Windows.Forms.Button();
            this.body_text = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(189, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 37);
            this.button1.TabIndex = 0;
            this.button1.Text = "执行";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // body_text
            // 
            this.body_text.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.body_text.Location = new System.Drawing.Point(0, 122);
            this.body_text.Multiline = true;
            this.body_text.Name = "body_text";
            this.body_text.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.body_text.Size = new System.Drawing.Size(564, 357);
            this.body_text.TabIndex = 9;
            // 
            // 表格合并
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 479);
            this.Controls.Add(this.body_text);
            this.Controls.Add(this.button1);
            this.Name = "表格合并";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "表格合并";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox body_text;
    }
}