using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 主程序202104
{
    public partial class MD5加密 : Form
    {
        public MD5加密()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           string md5 = method.GetMD5(textBox1.Text.Trim()+"siyiruanjian");
           int timestamp = Convert.ToInt32(method.GetTimeStamp());
            int guoqi = 0;
            if (radioButton1.Checked == true)
            {
                guoqi = timestamp + 86400;
            }
            if (radioButton2.Checked == true)
            {
                guoqi = timestamp + 86400*7;
            }
            if (radioButton3.Checked == true)
            {
                guoqi = timestamp + 86400*30;
            }
            if (radioButton4.Checked == true)
            {
                guoqi = timestamp + 86400*90;
            }
            if (radioButton5.Checked == true)
            {
                guoqi = timestamp + 86400*180;
            }
            if (radioButton6.Checked == true)
            {
                guoqi = timestamp + 86400*360;
            }
            textBox2.Text = md5 +"asd147"+ guoqi;
        }
    }
}
