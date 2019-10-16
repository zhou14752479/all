using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 常用代码管理软件
{
    public partial class 客户预测软件 : Form
    {
        public 客户预测软件()
        {
            InitializeComponent();
        }

        private void 客户预测软件_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }
        private Point mPoint = new Point();
        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }


        private void SkinTextBox1_Enter(object sender, EventArgs e)
        {
            skinTextBox1.Text = "";
        }

        private void SkinTextBox2_Enter(object sender, EventArgs e)
        {
            skinTextBox2.Text = "";

        }

        private void SkinTextBox3_Enter(object sender, EventArgs e)
        {
            skinTextBox3.Text = "";
        }

        private void SkinTextBox6_Enter(object sender, EventArgs e)
        {
            skinTextBox6.Text = "";
        }

        private void SkinTextBox5_Enter(object sender, EventArgs e)
        {
            skinTextBox5.Text = "";
        }

        private void SkinTextBox4_Enter(object sender, EventArgs e)
        {
            skinTextBox4.Text = "";
        }

        private void SkinTextBox7_Enter(object sender, EventArgs e)
        {
            skinTextBox7.Text = "";
        }

        private void SkinTextBox8_Enter(object sender, EventArgs e)
        {
            skinTextBox8.Text = "";
        }
        
       
        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                
            }
        }


        public void run()

        {
            if (skinTextBox1.Text.Contains("赔率") || skinTextBox2.Text.Contains("赔率") || skinTextBox3.Text.Contains("赔率") || skinTextBox4.Text.Contains("赔率") || skinTextBox5.Text.Contains("赔率") || skinTextBox6.Text.Contains("赔率"))
            {
                MessageBox.Show("请输入数据");
                return;
            }






            Random r = new Random();
            int num = r.Next(1, 51);
            int num1 = r.Next(1, 51);
            int num2 = 100 - (num + num1);

            string a = num.ToString() + "%";
            string b = num1.ToString() + "%";
            string c = num2.ToString() + "%";


            Graphics gra = this.pictureBox1.CreateGraphics();
            gra.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Brush bush = new SolidBrush(Color.Gray);//填充的颜色
            gra.FillEllipse(bush, 10, 10, 120, 120);//画填充椭圆的方法，x坐标、y坐标、宽、高，如果是100，则半径为50
            Font myFont = new Font("宋体", 20, FontStyle.Bold);
            Brush zibush = new SolidBrush(Color.White);//填充的颜色
            gra.DrawString(a, myFont, zibush, 40, 50);



            Graphics gra1 = this.pictureBox2.CreateGraphics();
            gra1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Brush bush1 = new SolidBrush(Color.Gray);//填充的颜色
            gra1.FillEllipse(bush1, 10, 10, 120, 120);//画填充椭圆的方法，x坐标、y坐标、宽、高，如果是100，则半径为50
            Font myFont1 = new Font("宋体", 20, FontStyle.Bold);
            Brush zibush1 = new SolidBrush(Color.White);//填充的颜色
            gra1.DrawString(b, myFont1, zibush1, 40, 50);






            Graphics gra2 = this.pictureBox3.CreateGraphics();
            gra2.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Brush bush2 = new SolidBrush(Color.Gray);//填充的颜色
            gra2.FillEllipse(bush2, 10, 10, 120, 120);//画填充椭圆的方法，x坐标、y坐标、宽、高，如果是100，则半径为50
            Font myFont2 = new Font("宋体", 20, FontStyle.Bold);
            Brush zibush2 = new SolidBrush(Color.White);//填充的颜色
            gra2.DrawString(c, myFont2, zibush2, 40, 50);

        }
        private void Label15_Click(object sender, EventArgs e)
        {
            run();
        }

        private void Label16_Click(object sender, EventArgs e)
        {
            run();
        }
    }
}
