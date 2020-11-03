using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 主程序202011
{
    public partial class 浙江政务网 : Form
    {
        public 浙江政务网()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
          string value= OCR.shibie("https://puser.zjzwfw.gov.cn/sso/usp.do?action=verifyimg", "");
           MessageBox.Show(value);
        }
    }
}
