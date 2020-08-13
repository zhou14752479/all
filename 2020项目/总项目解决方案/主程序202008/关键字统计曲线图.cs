using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace 主程序202008
{
    public partial class 关键字统计曲线图 : Form
    {
        public 关键字统计曲线图()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            float x1 = float.Parse("10");
            float x2 = float.Parse("20");
            float x3 = float.Parse("30");
            float x4 = float.Parse("10");
            float x5 = float.Parse("20");
            float x6 = float.Parse("30");
            float x7 = float.Parse("10");
            float x8 = float.Parse("20");
            float x9 = float.Parse("10");
            float x10 = float.Parse("30");
            float x11 = float.Parse("20");
            float x12 = float.Parse("10");
            var chart = chart1.ChartAreas[0];
            chart.AxisX.LabelStyle.Format = "yyyy-MM-dd";
           // chart.AxisX.IntervalType = DateTimeIntervalType.Number;

            //chart.AxisX.LabelStyle.Format = "";
            //chart.AxisY.LabelStyle.Format = "";
            chart.AxisY.LabelStyle.IsEndLabelVisible = true;

            chart.AxisX.Minimum = 1;
            chart.AxisX.Maximum = 12;
            chart.AxisY.Minimum = 0;
            chart.AxisY.Maximum = 50;
            chart.AxisX.Interval = 1;
            chart.AxisY.Interval = 5;

            chart1.Series.Add("line1");   //数据1
            
            chart1.Series["line1"].LegendText = "数据1";

           // chart1.Series["line1"].ChartType = SeriesChartType.Line; //绘制折线图

            chart1.Series["line1"].ChartType = SeriesChartType.Spline;  //绘制曲线图

            chart1.Series["line1"].Color = Color.Red;
            chart1.Series[0].IsVisibleInLegend = true;
            
            chart1.Series["line1"].Points.AddXY(DateTime.Now.ToOADate(), x1);
            chart1.Series["line1"].Points.AddXY(2, x2);
            chart1.Series["line1"].Points.AddXY(3, x3);
            chart1.Series["line1"].Points.AddXY(4, x4);
            chart1.Series["line1"].Points.AddXY(5, x5);
            chart1.Series["line1"].Points.AddXY(6, x6);
            chart1.Series["line1"].Points.AddXY(7, x7);
            chart1.Series["line1"].Points.AddXY(8, x8);
            chart1.Series["line1"].Points.AddXY(9, x9);
            chart1.Series["line1"].Points.AddXY(10, x10);
            chart1.Series["line1"].Points.AddXY(11, x11);
            chart1.Series["line1"].Points.AddXY(12, x12);

        }

        private void 关键字统计曲线图_Load(object sender, EventArgs e)
        {
            chart1.Series.Clear(); //清除默认的series
            
        }
    }
}
