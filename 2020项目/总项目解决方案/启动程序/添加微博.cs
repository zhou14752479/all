using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 启动程序
{
    public partial class 添加微博 : Form
    {
        public 添加微博()
        {
            InitializeComponent();
        }
        public static string beizhu ;
        public static string url;
        public static string jishua;
        public static string rengong;
        public static string cishu;
        private void Button1_Click(object sender, EventArgs e)
        {
            beizhu = textBox1.Text.Trim();
            url = textBox2.Text.Trim();
            jishua=textBox3.Text+"-"+textBox4.Text+" "+ textBox5.Text + "-" + textBox6.Text + " "+textBox7.Text + "-" + textBox8.Text;
            rengong = textBox9.Text + "-" + textBox10.Text + " " + textBox11.Text + "-" + textBox12.Text + " " + textBox13.Text + "-" + textBox14.Text;
            cishu = textBox15.Text;



            DialogResult result = MessageBox.Show("是否继续添加？", "退出询问"
          , MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {


            }
            else
            {
                this.Hide();
            }
           
        }





    }
}
