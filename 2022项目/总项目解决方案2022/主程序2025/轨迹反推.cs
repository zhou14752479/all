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



        class MouseMovement
        {
            private Random random;
            private const int maxPixelOffset = 3;
            private const double minSpeedFactor = 0.8;
            private const double maxSpeedFactor = 1.2;
            private const int minClickDuration = 50;
            private const int maxClickDuration = 150;

            public MouseMovement()
            {
                random = new Random();
            }

            public List<List<object>> GenerateNewTrajectory(List<JArray> originalTrajectory)
            {
                List<List<object>> newTrajectory = new List<List<object>>();
                int prevTime = 0;

                foreach (var point in originalTrajectory)
                {
                    try
                    {
                        string action = point[0].ToString();
                        int time = Convert.ToInt32(point[1]);
                        int x = Convert.ToInt32(point[2]);
                        int y = Convert.ToInt32(point[3]);
                        int buttonState = Convert.ToInt32(point[4]);

                        // 生成随机的像素偏移
                        int offsetX = random.Next(-maxPixelOffset, maxPixelOffset + 1);
                        int offsetY = random.Next(-maxPixelOffset, maxPixelOffset + 1);

                        // 调整时间戳
                        double speedFactor = minSpeedFactor + (maxSpeedFactor - minSpeedFactor) * random.NextDouble();
                        int newTime = (int)(time * speedFactor);
                        if (newTrajectory.Count > 0)
                        {
                            newTime += random.Next(-10, 11);
                            if (newTime < prevTime)
                            {
                                newTime = prevTime;
                            }
                        }
                        prevTime = newTime;

                        if (action == "mousemove")
                        {
                            newTrajectory.Add(new List<object> { action, newTime, x + offsetX, y + offsetY, buttonState });
                        }
                        else if (action == "mousedown")
                        {
                            newTrajectory.Add(new List<object> { action, newTime, x + offsetX, y + offsetY, buttonState });
                        }
                        else if (action == "mouseup")
                        {
                            int clickDuration = random.Next(minClickDuration, maxClickDuration + 1);
                            newTrajectory.Add(new List<object> { action, newTime + clickDuration, x + offsetX, y + offsetY, buttonState });
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing point {point}: {ex.Message}");
                    }
                }

                return newTrajectory;
            }
        }

        //参照这个帮我生成一个模拟人工的轨迹
        public void run()
        {
            string originalTrajectoryStr = @"[
[[""mousemove"", 31, 1038, 337, 0], [""mousemove"", 60, 1062, 342, 0], [""mousemove"", 105, 1168, 358, 0], [""mousemove"", 140, 1272, 380, 0], [""mousemove"", 146, 1270, 394, 0], [""mousemove"", 180, 1363, 410, 0], [""mousemove"", 188, 1378, 575, 0], [""mousemove"", 220, 1388, 590, 0], [""mousemove"", 260, 1643, 689, 0], [""mousemove"", 310, 2078, 729, 0], [""mousemove"", 356, 2195, 884, 0], [""mousemove"", 363, 2202, 1058, 0], [""mousemove"", 368, 2225, 1076, 0], [""mousemove"", 411, 2423, 1143, 0], [""mousemove"", 430, 2465, 1154, 0], [""mousemove"", 438, 2478, 1173, 0], [""mousemove"", 1120, 2112, 5728, 0], [""mousemove"", 1160, 1102, 6758, 0], [""mousemove"", 1200, 1048, 6808, 0], [""mousemove"", 1230, 848, 6804, 0], [""mousemove"", 1236, 838, 6818, 0], [""mousemove"", 1250, 828, 6833, 0], [""mousemove"", 1260, 823, 6843, 0], [""mousemove"", 1270, 814, 6853, 0], [""mousemove"", 1280, 823, 7000, 0], [""mousemove"", 1320, 828, 7148, 0], [""mousemove"", 1330, 838, 7160, 0], [""mousemove"", 1340, 853, 7170, 0], [""mousemove"", 1350, 868, 7188, 0], [""mousemove"", 1395, 908, 7274, 0], [""mousemove"", 1410, 968, 7288, 0], [""mousemove"", 1450, 1072, 7319, 0], [""mousemove"", 1480, 1102, 7368, 0], [""mousemove"", 1500, 1118, 7428, 0], [""mousemove"", 1510, 1118, 7443, 0], [""mousemove"", 1520, 1118, 7453, 0], [""mousemove"", 1530, 1118, 7463, 0], [""mousemove"", 1540, 1118, 7473, 0], [""mousemove"", 1550, 1118, 7483, 0], [""mousemove"", 1560, 1118, 7503, 0], [""mousemove"", 1570, 1118, 7513, 0], [""mousemove"", 1580, 1118, 7528, 0], [""mousemove"", 1590, 1118, 7538, 0], [""mousemove"", 1600, 1118, 7563, 0], [""mousemove"", 1610, 1118, 7593, 0], [""mousemove"", 1620, 1118, 7628, 0], [""mousemove"", 1630, 1123, 7638, 0], [""mousemove"", 1670, 1128, 7748, 0], [""mousemove"", 1690, 1133, 7798, 0], [""mousemove"", 1700, 1133, 7823, 0], [""mousemove"", 1710, 1133, 7843, 0], [""mousemove"", 1720, 1133, 7853, 0], [""mousemove"", 1770, 1133, 8038, 0], [""mousemove"", 1780, 1138, 8043, 0], [""mousemove"", 1800, 1136, 8201, 0], [""mousedown"", 1825, 1136, 8201, 1], [""mousemove"", 1835, 1143, 8308, 1], [""mousemove"", 1845, 1148, 8313, 1], [""mousemove"", 1855, 1158, 8323, 1], [""mousemove"", 1865, 1173, 8333, 1], [""mousemove"", 1875, 1188, 8343, 1], [""mousemove"", 1885, 1203, 8348, 1], [""mousemove"", 1895, 1233, 8383, 1], [""mousemove"", 1905, 1288, 8393, 1], [""mousemove"", 1915, 1308, 8403, 1], [""mousemove"", 1925, 1323, 8413, 1], [""mousemove"", 1935, 1328, 8423, 1], [""mousemove"", 1945, 1338, 8433, 1], [""mousemove"", 1955, 1343, 8443, 1], [""mousemove"", 1965, 1348, 8548, 1], [""mousemove"", 1985, 1346, 8771, 1], [""mouseup"", 2015, 1346, 8771, 0]]
]";

            List<JArray> originalTrajectory = JsonConvert.DeserializeObject<List<JArray>>(originalTrajectoryStr);

            MouseMovement movement = new MouseMovement();
            List<List<object>> newTrajectory = movement.GenerateNewTrajectory(originalTrajectory);

            foreach (var point in newTrajectory)
            {
                textBox1.Text+=($"[{string.Join(", ", point)}]");
            }
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
