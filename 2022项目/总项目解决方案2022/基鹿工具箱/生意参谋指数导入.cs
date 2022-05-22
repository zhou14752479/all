using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 基鹿工具箱
{
    public partial class 生意参谋指数导入 : Form
    {
        public 生意参谋指数导入()
        {
            InitializeComponent();
        }

        private void 生意参谋指数导入_Load(object sender, EventArgs e)
        {
            textBox1.Text = AppDomain.CurrentDomain.BaseDirectory+"交易指数.txt";
            textBox2.Text = AppDomain.CurrentDomain.BaseDirectory + "搜索指数.txt";
            textBox3.Text = AppDomain.CurrentDomain.BaseDirectory + "供应商指数.txt";
            textBox4.Text = AppDomain.CurrentDomain.BaseDirectory + "商品指数.txt";
            textBox5.Text = AppDomain.CurrentDomain.BaseDirectory + "支付转化率指数.txt";
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                textBox1.Text = openFileDialog1.FileName;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Util.BCP_Mysql(textBox1.Text,"jyzs");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Util.BCP_Mysql(textBox2.Text, "sszs");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Util.BCP_Mysql(textBox3.Text, "gyszs");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Util.BCP_Mysql(textBox4.Text, "spzs");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Util.BCP_Mysql(textBox5.Text, "zfzhzs");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                textBox2.Text = openFileDialog1.FileName;

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                textBox3.Text = openFileDialog1.FileName;

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                textBox4.Text = openFileDialog1.FileName;

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                textBox5.Text = openFileDialog1.FileName;

            }
        }
    }
}
