using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 代码生成器
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public delegate void SendMsg(string msg);
        public SendMsg sendmsg;
        private void button1_Click(object sender, EventArgs e)
        {
            sendmsg("发送消息到Form1");
        }
    }
}
