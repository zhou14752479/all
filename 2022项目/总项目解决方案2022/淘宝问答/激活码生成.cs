using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 淘宝问答
{
    public partial class 激活码生成 : Form
    {
        public 激活码生成()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           if(textBox1.Text=="")
            {
                MessageBox.Show("请输入机器码");
                return;
            }



            long timestamp = Convert.ToInt64(method.GetTimeStamp());    
            long guoqi = 0;
            if (radioButton1.Checked == true)
            {
                guoqi = timestamp + 86400;
            }
            if (radioButton2.Checked == true)
            {
                guoqi = timestamp + 86400 * 7;
            }
            if (radioButton3.Checked == true)
            {
                guoqi = timestamp + 86400 * 30;
            }
            if (radioButton4.Checked == true)
            {
                guoqi = timestamp + 86400 * 90;
            }
            if (radioButton5.Checked == true)
            {
                guoqi = timestamp + 86400 * 180;
            }
            if (radioButton6.Checked == true)
            {
                guoqi = timestamp + 86400 * 360;
            }
            if (radioButton7.Checked == true)
            {
                guoqi = timestamp + 86400 * 720;
            }
            if (radioButton8.Checked == true)
            {
                guoqi = timestamp + 86400 * 1000;
            }
            string value = textBox1.Text+ "*" + guoqi+"*"+numericUpDown1.Value;
            string value2 = method.Base64Encode(Encoding.GetEncoding("utf-8"),value);
            textBox2.Text = method.Base64Encode(Encoding.GetEncoding("utf-8"), value2);

            System.Windows.Forms.Clipboard.SetText(textBox2.Text); //复制
        }
    }
}
