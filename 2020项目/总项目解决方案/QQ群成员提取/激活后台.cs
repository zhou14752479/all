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

namespace QQ群成员提取
{
    public partial class 激活后台 : Form
    {
        public 激活后台()
        {
            InitializeComponent();
        }

        private void 激活后台_Load(object sender, EventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            long expiretime = Convert.ToInt64(method.GetTimeStamp()) + Convert.ToInt32(textBox2.Text) * 86400;
            string jhuoma = method.GetMD5(textBox1.Text.Trim()) + expiretime;
            textBox3.Text = jhuoma.ToUpper();
        }


    }
}
