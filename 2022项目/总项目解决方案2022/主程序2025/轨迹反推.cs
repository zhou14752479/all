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

namespace 主程序2025
{
    public partial class 轨迹反推 : Form
    {
        public 轨迹反推()
        {
            InitializeComponent();
        }


        Thread thread;

        public void run()
        {

            string xxxx = richTextBox1.Text.Trim(); // 把你的JSON字符串放在这里
            JArray Qo = JArray.Parse(xxxx);

            JArray trackList = (JArray)Qo[140];
            List<object> li = new List<object>();
            bool hhh = false;

            foreach (JToken track in trackList)
            {
                int t4 = track[9].Value<int>();   // time
                int t17 = track[10].Value<int>();  // x
                int t5 = track[12].Value<int>();   // y
                int t14 = track[7].Value<int>();  // down

                int ro = t4;
                int ce = ro % 7;
                int X = Qo[115][ce].Value<int>();
                int pageX = t17 ^ X;
                int pageY = t5 ^ X;

                if (t14 == 1)
                {
                    if (hhh)
                    {
                        li.Add(new object[] { "mousemove", pageX, pageY, ro, 1 });
                    }
                    else
                    {
                        li.Add(new object[] { "mousemove", pageX, pageY, ro, 0 });
                    }
                }
                else if (t14 == 4)
                {
                    hhh = true;
                    li.Add(new object[] { "mousedown", pageX, pageY, ro, 1 });
                }
            }

            string json = JsonConvert.SerializeObject(li);
            json = "["+json+"]";
           richTextBox1.Text = json;
            System.IO.File.WriteAllText("ptguiji231.js", json);
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
