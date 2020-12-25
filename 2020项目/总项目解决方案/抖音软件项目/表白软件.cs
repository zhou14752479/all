using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 抖音软件项目
{
    public partial class 表白软件 : Form
    {
        public 表白软件()
        {
            InitializeComponent();
        }

       

        private void 表白软件_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("再想一下嘛！！");
            e.Cancel = true;
        }

        private void 表白软件_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("么么哒！！");
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        string[] txts = { "我喜欢你很久了", "想娶你", "真心喜欢你", "再考虑一下嘛", "认真的", "房子写你名", };
        private void button2_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int left = random.Next(300);
            int top = random.Next(200);//500是随机范围
            button2.Left = left;
            button2.Top = top;

            Random random1 = new Random();
            int suiji = random.Next(6);
            button2.Text = txts[suiji];
        }
    }
}
