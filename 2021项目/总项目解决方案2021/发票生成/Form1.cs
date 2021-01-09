using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 发票生成
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string v1="";
        public string v2 = "";
        public string v3 = "";
        public string v4 = "";
        public string v5 = "";
        public string v6 = "";
        public string v61 = "";
        public string v7 = "";
        public string v8 = "";
        public string v9 = "";
        public string v10 = "";
        public string v11 = "";
        public string v12 = "";
        public string v13 = "";
        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Parent = pictureBox1;
            label2.Parent = pictureBox1;
            label3.Parent = pictureBox1;
            label4.Parent = pictureBox1;
            label13.Parent = pictureBox1;
            label6.Parent = pictureBox1;
            label61.Parent = pictureBox1;
            label7.Parent = pictureBox1;
            label66.Parent = pictureBox1;
            label8.Parent = pictureBox1;
            label9.Parent = pictureBox1;
            label10.Parent = pictureBox1;
            label11.Parent = pictureBox1;
            label12.Parent = pictureBox1;
            label14.Parent = pictureBox1;

            label1.Text = v1.Trim();
            label2.Text = v2.Trim();
            label3.Text = v3.Trim();
            label4.Text = v4.Trim();
            label13.Text = v5.Trim();
            label6.Text = v6.Trim();
            label61.Text = v61.Trim();
            label7.Text = v7.Trim();
            label8.Text = v8.Trim();
            label9.Text = v9.Trim();
            label10.Text = v10.Trim();
            label11.Text = v11.Trim();
            label12.Text = v12.Trim();
            label14.Text = v13.Trim();
            string path = AppDomain.CurrentDomain.BaseDirectory;
          // pictureBox1.Image.Save(path+ GetTimeStamp()+".jpg");

        }

        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalSeconds);
            return a.ToString();
        }

    }
}
