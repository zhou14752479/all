using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace webView2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          webView21.Source = new Uri("https://login.taobao.com/member/login.jhtml?spm=a21bo.21814703.201864-2.d1.5af911d9ZE4f5j&f=top&redirectURL=http%3A%2F%2Fwww.taobao.com%2F");
            
        }
    
    
    }
}
