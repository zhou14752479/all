using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 模型下载
{
    public partial class AAA模型共享 : Form
    {
        public AAA模型共享()
        {
            InitializeComponent();
        }


		

		private void AAA模型共享_Load(object sender, EventArgs e)
        {

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            MySqlHelper.login();
        }
    }
}
