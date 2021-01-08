namespace 政府房产交易网
{
    partial class 淄博房产
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
            this.logtxt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // logtxt
            // 
            this.logtxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logtxt.Location = new System.Drawing.Point(0, 0);
            this.logtxt.Margin = new System.Windows.Forms.Padding(4);
            this.logtxt.Multiline = true;
            this.logtxt.Name = "logtxt";
            this.logtxt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logtxt.Size = new System.Drawing.Size(800, 450);
            this.logtxt.TabIndex = 2;
            // 
            // 淄博房产
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.logtxt);
            this.Name = "淄博房产";
            this.Text = "淄博房产";
            this.Load += new System.EventHandler(this.淄博房产_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox logtxt;
    }
}