using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static 主程序2025.guiji;

namespace 主程序2025
{
    public partial class 轨迹反推 : Form
    {
        public 轨迹反推()
        {
            InitializeComponent();
        }


        Thread thread;




       




        //参照这个帮我生成一个模拟人工的轨迹
        public void run()
        {

          

        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
