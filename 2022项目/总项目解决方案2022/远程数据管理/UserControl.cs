using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 远程数据管理
{
    public partial class UserControl : System.Windows.Forms.UserControl
    {
     
        public string key;
       
        public UserControl()
        {
          
            InitializeComponent();
          
          
          
          
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            key = textBox1.Text;
          
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            key = textBox1.Text;
          
        }

       
    }
}
