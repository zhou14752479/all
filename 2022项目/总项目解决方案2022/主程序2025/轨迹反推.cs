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
using System.Text.Json;

namespace 主程序2025
{
    public partial class 轨迹反推 : Form
    {
        public 轨迹反推()
        {
            InitializeComponent();
        }


        Thread thread;




        class TrajectoryGenerator
        {
            private static readonly Random random = new Random();

            // 生成高斯噪声
            private static double GenerateGaussianNoise(double mean, double stdDev)
            {
                double u1 = 1.0 - random.NextDouble();
                double u2 = 1.0 - random.NextDouble();
                double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
                return mean + stdDev * randStdNormal;
            }

            // 生成泊松分布的时间间隔
            private static int GeneratePoissonInterval(double lambda)
            {
                double L = Math.Exp(-lambda);
                int k = 0;
                double p = 1.0;
                do
                {
                    k++;
                    p *= random.NextDouble();
                } while (p > L);
                return k - 1;
            }

            // 贝塞尔曲线计算
            private static (int x, int y) BezierPoint((int x, int y) p0, (int x, int y) p1, (int x, int y) p2, double t)
            {
                double u = 1 - t;
                double tt = t * t;
                double uu = u * u;
                int x = (int)(uu * p0.x + 2 * u * t * p1.x + tt * p2.x);
                int y = (int)(uu * p0.y + 2 * u * t * p1.y + tt * p2.y);
                return (x, y);
            }

            public static List<List<object>> GenerateNewTrajectory(List<JsonElement> originalTrajectory)
            {
                var newTrajectory = new List<List<object>>();
                int prevTimestamp = originalTrajectory[0].EnumerateArray().ElementAt(1).GetInt32();

                for (int i = 0; i < originalTrajectory.Count - 2; i++)
                {
                    var currentPoint = originalTrajectory[i];
                    var nextPoint = originalTrajectory[i + 1];
                    var nextNextPoint = originalTrajectory[i + 2];

                    int currentX = currentPoint.EnumerateArray().ElementAt(2).GetInt32();
                    int currentY = currentPoint.EnumerateArray().ElementAt(3).GetInt32();
                    int nextX = nextPoint.EnumerateArray().ElementAt(2).GetInt32();
                    int nextY = nextPoint.EnumerateArray().ElementAt(3).GetInt32();
                    int nextNextX = nextNextPoint.EnumerateArray().ElementAt(2).GetInt32();
                    int nextNextY = nextNextPoint.EnumerateArray().ElementAt(3).GetInt32();

                    int currentTimestamp = currentPoint.EnumerateArray().ElementAt(1).GetInt32();
                    int nextTimestamp = nextPoint.EnumerateArray().ElementAt(1).GetInt32();
                    int timeDifference = nextTimestamp - currentTimestamp;

                    // 贝塞尔曲线控制点
                    var p0 = (currentX, currentY);
                    var p1 = ((currentX + nextX) / 2, (currentY + nextY) / 2);
                    var p2 = (nextX, nextY);

                    int steps = Math.Max(1, timeDifference / 10);
                    for (int j = 0; j < steps; j++)
                    {
                        double t = (double)j / steps;
                        var bezierPoint = BezierPoint(p0, p1, p2, t);

                        // 添加高斯噪声
                        int noisyX = (int)(bezierPoint.x + GenerateGaussianNoise(0, 5));
                        int noisyY = (int)(bezierPoint.y + GenerateGaussianNoise(0, 5));

                        // 泊松分布时间间隔
                        int poissonInterval = GeneratePoissonInterval(1.0);
                        prevTimestamp += poissonInterval;

                        var newPoint = new List<object>
                {
                    currentPoint.EnumerateArray().ElementAt(0).GetString(),
                    prevTimestamp,
                    noisyX,
                    noisyY,
                    currentPoint.EnumerateArray().ElementAt(4).GetInt32()
                };
                        newTrajectory.Add(newPoint);
                    }
                }

                // 添加最后一个点
                var lastPoint = originalTrajectory[1];
                var lastNewPoint = new List<object>
        {
            lastPoint.EnumerateArray().ElementAt(0).GetString(),
            lastPoint.EnumerateArray().ElementAt(1).GetInt32(),
            lastPoint.EnumerateArray().ElementAt(2).GetInt32(),
            lastPoint.EnumerateArray().ElementAt(3).GetInt32(),
            lastPoint.EnumerateArray().ElementAt(4).GetInt32()
        };
                newTrajectory.Add(lastNewPoint);

                return newTrajectory;
            }
        }




        //参照这个帮我生成一个模拟人工的轨迹
        public void run()
        {

            string inputJson = @"[
            [
                [""mousemove"", 3, 1028, 325, 0],
                [""mousemove"", 22, 1072, 347, 0],
                [""mousemove"", 41, 1149, 361, 0],
                [""mousemove"", 93, 1236, 393, 0],
                [""mousemove"", 105, 1299, 406, 0],
                [""mousemove"", 137, 1362, 422, 0],
                [""mousemove"", 138, 1371, 588, 0],
                [""mousemove"", 139, 1395, 607, 0],
                [""mousemove"", 146, 1642, 708, 0],
                [""mousemove"", 192, 2083, 740, 0],
                [""mousemove"", 205, 2192, 906, 0],
                [""mousemove"", 219, 2209, 1063, 0],
                [""mousemove"", 228, 2229, 1088, 0],
                [""mousemove"", 241, 2419, 1154, 0],
                [""mousedown"", 785, 1145, 8211, 1],
                [""mousemove"", 787, 1151, 8321, 1],
                [""mousemove"", 789, 1177, 8335, 1],
                [""mousemove"", 792, 1219, 8359, 1],
                [""mouseup"", 796, 1356, 8780, 1]
            ]
        ]";

            var originalTrajectory = System.Text.Json.JsonSerializer.Deserialize<List<JsonElement>>(inputJson);
            var newTrajectory = TrajectoryGenerator.GenerateNewTrajectory(originalTrajectory);
            string outputJson = System.Text.Json.JsonSerializer.Serialize(newTrajectory, new JsonSerializerOptions { WriteIndented = true });
            textBox1.Text = outputJson;

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
