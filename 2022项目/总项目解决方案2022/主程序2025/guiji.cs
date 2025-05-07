using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 主程序2025
{
    public class guiji
    {










public class TrajectoryGenerator
    {
        private static Random _random = new Random();

        // 三阶贝塞尔曲线生成核心算法[6,8](@ref)
        public static List<Point> GenerateBezierPath(Point start, Point end, int segments)
        {
            var path = new List<Point>();
            var ctrl1 = new Point(
                start.X + _random.Next(-80, 80),
                start.Y + _random.Next(-50, 50));
            var ctrl2 = new Point(
                end.X + _random.Next(-40, 40),
                end.Y + _random.Next(-30, 30));

            for (int i = 0; i <= segments; i++)
            {
                double t = (double)i / segments;
                double u = 1 - t;

                // 贝塞尔公式优化[7](@ref)
                int x = (int)(Math.Pow(u, 3) * start.X +
                            3 * Math.Pow(u, 2) * t * ctrl1.X +
                            3 * u * Math.Pow(t, 2) * ctrl2.X +
                            Math.Pow(t, 3) * end.X);

                int y = (int)(Math.Pow(u, 3) * start.Y +
                            3 * Math.Pow(u, 2) * t * ctrl1.Y +
                            3 * u * Math.Pow(t, 2) * ctrl2.Y +
                            Math.Pow(t, 3) * end.Y);

                path.Add(new Point(x, y));
            }
            return AddNoise(path);
        }

        // 高斯噪声注入算法[1](@ref)
        private static List<Point> AddNoise(List<Point> path)
        {
            const double sigma = 1.5;
            var noisyPath = new List<Point>();

            foreach (var point in path)
            {
                double rand1 = Math.Sqrt(-2 * Math.Log(_random.NextDouble()))
                             * Math.Cos(2 * Math.PI * _random.NextDouble());
                double rand2 = Math.Sqrt(-2 * Math.Log(_random.NextDouble()))
                             * Math.Sin(2 * Math.PI * _random.NextDouble());

                noisyPath.Add(new Point(
                    (int)(point.X + sigma * rand1),
                    (int)(point.Y + sigma * rand2)));
            }
            return noisyPath;
        }

        // 动态时间戳生成[1,6](@ref)
        public static List<TrajectoryPoint> GenerateTimestamps(List<Point> path)
        {
            var result = new List<TrajectoryPoint>();
            int currentTime = 0;

            foreach (var point in path)
            {
                // 泊松分布时间间隔[1](@ref)
                currentTime += Math.Max(10, PoissonRandom(35));
                result.Add(new TrajectoryPoint(point, currentTime));
            }
            return result;
        }

        private static int PoissonRandom(double lambda)
        {
            double L = Math.Exp(-lambda);
            int k = 0;
            double p = 1.0;

            do
            {
                k++;
                p *= _random.NextDouble();
            } while (p > L);

            return k - 1;
        }
    }

    public struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class TrajectoryPoint
    {
        public Point Position { get; }
        public int Timestamp { get; }

        public TrajectoryPoint(Point pos, int time)
        {
            Position = pos;
            Timestamp = time;
        }
    }







}
}
