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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 fm1 = new Form1();
           
            fm1.v1 = textBox1.Text.Trim();
            fm1.v2 = textBox2.Text.Trim();
            fm1.v3 = textBox3.Text.Trim();
            fm1.v4 = textBox4.Text.Trim();
            fm1.v5 = textBox5.Text.Trim();
            fm1.v6 = textBox6.Text.Trim();
            fm1.v61 = "105";
            fm1.v7 = textBox7.Text.Trim();
            fm1.v8 = textBox8.Text.Trim();
            fm1.v9 = textBox9.Text.Trim();
            fm1.v10 = textBox10.Text.Trim();
            fm1.v11 = textBox11.Text.Trim();
            fm1.v12 = textBox12.Text.Trim();
            fm1.v13 = textBox13.Text.Trim();
            fm1.Show();
        }

        public string getRandom(int a,int b)
        {
           
                Random rd = new Random();
                int v1 = rd.Next(a,b);
               
            
            return v1.ToString();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
           



            textBox1.Text = 1+ getRandom(1111111,9999999);
            textBox3.Text = "0000" + getRandom(111111, 999999);
            textBox4.Text = 1 + getRandom(10, 99)+"G"+ getRandom(1, 9)+"BY";
            textBox5.Text = DateTime.Now.ToString("yyyy-MM-dd");

            textBox6.Text = "0" + getRandom(11111111, 99999999);
           
        }
    }
}
