namespace main
{
    partial class dataView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.skinDataGridView1 = new CCWin.SkinControl.SkinDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.skinDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // skinDataGridView1
            // 
            this.skinDataGridView1.AlternatingCellBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.skinDataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.skinDataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.skinDataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
            this.skinDataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.skinDataGridView1.ColumnFont = null;
            this.skinDataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(246)))), ((int)(((byte)(239)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.skinDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.skinDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.skinDataGridView1.ColumnSelectForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(188)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.skinDataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.skinDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinDataGridView1.EnableHeadersVisualStyles = false;
            this.skinDataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.skinDataGridView1.HeadFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinDataGridView1.HeadForeColor = System.Drawing.Color.Black;
            this.skinDataGridView1.HeadSelectForeColor = System.Drawing.SystemColors.HighlightText;
            this.skinDataGridView1.LineNumberForeColor = System.Drawing.Color.BlueViolet;
            this.skinDataGridView1.Location = new System.Drawing.Point(0, 0);
            this.skinDataGridView1.MouseCellBackColor = System.Drawing.Color.White;
            this.skinDataGridView1.Name = "skinDataGridView1";
            this.skinDataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.skinDataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.skinDataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.skinDataGridView1.RowTemplate.Height = 23;
            this.skinDataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.skinDataGridView1.Size = new System.Drawing.Size(800, 450);
            this.skinDataGridView1.SkinGridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.skinDataGridView1.TabIndex = 60;
            this.skinDataGridView1.TitleBack = null;
            this.skinDataGridView1.TitleBackColorBegin = System.Drawing.Color.White;
            this.skinDataGridView1.TitleBackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.skinDataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.skinDataGridView1_CellContentClick);
            this.skinDataGridView1.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.skinDataGridView1_RowsAdded);
            this.skinDataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.skinDataGridView1_KeyDown);
            // 
            // dataView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.skinDataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "dataView";
            this.Text = "dataView";
            this.Load += new System.EventHandler(this.dataView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.skinDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinDataGridView skinDataGridView1;
    }
}