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



        class MouseTrajectoryGenerator
        {
            private Random random = new Random();

            public List<List<object>> GenerateTrajectory()
            {
                List<List<object>> trajectory = new List<List<object>>();
                int time = 0;
                int x = random.Next(800, 2500);
                int y = random.Next(300, 1200);

                // 起始阶段，缓慢移动
                for (int i = 0; i < random.Next(3, 6); i++)
                {
                    time += random.Next(20, 60);
                    x += GetRandomDelta(5);
                    y += GetRandomDelta(5);
                    trajectory.Add(new List<object> { "mousemove", time, x, y, 0 });
                }

                // 中间阶段，加速移动
                for (int i = 0; i < random.Next(8, 15); i++)
                {
                    time += random.Next(10, 40);
                    x += GetRandomDelta(15);
                    y += GetRandomDelta(15);
                    trajectory.Add(new List<object> { "mousemove", time, x, y, 0 });
                }

                // 接近目标阶段，减速微调
                for (int i = 0; i < random.Next(5, 8); i++)
                {
                    time += random.Next(20, 50);
                    x += GetRandomDelta(3);
                    y += GetRandomDelta(3);
                    trajectory.Add(new List<object> { "mousemove", time, x, y, 0 });
                }

                // 按下事件，添加停顿
                time += random.Next(50, 120);
                trajectory.Add(new List<object> { "mousedown", time, x, y, 1 });

                // 拖动阶段，速度相对均匀，有小波动
                int dragDirX = GetDragDirection();
                int dragDirY = GetDragDirection();
                for (int i = 0; i < random.Next(15, 25); i++)
                {
                    time += random.Next(15, 30);
                    x += dragDirX + GetRandomDelta(2);
                    y += dragDirY + GetRandomDelta(2);
                    trajectory.Add(new List<object> { "mousemove", time, x, y, 1 });
                }

                // 释放事件，添加停顿
                time += random.Next(30, 70);
                trajectory.Add(new List<object> { "mouseup", time, x, y, 0 });

                return trajectory;
            }

            private int GetRandomDelta(int max)
            {
                return random.Next(-max, max + 1);
            }

            private int GetDragDirection()
            {
                int[] directions = { -5, -3, -1, 1, 3, 5 };
                return directions[random.Next(directions.Length)];
            }
        }

        //参照这个帮我生成一个模拟人工的轨迹
        public void run()
        {
            MouseTrajectoryGenerator generator = new MouseTrajectoryGenerator();
            List<List<object>> trajectory = generator.GenerateTrajectory();

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None
            };

            string json = JsonConvert.SerializeObject(new List<List<List<object>>> { trajectory }, settings);
            textBox1.Text = json;

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
