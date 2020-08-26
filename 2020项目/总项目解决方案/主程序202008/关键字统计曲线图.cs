using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using helper;

namespace 主程序202008
{
    public partial class 关键字统计曲线图 : Form
    {
        public 关键字统计曲线图()
        {
            InitializeComponent();
        }


        #region 系统自带曲线图


        //public void zhitu()
        //{
        //    float x1 = float.Parse("10");
        //    float x2 = float.Parse("20");
        //    float x3 = float.Parse("30");
        //    float x4 = float.Parse("10");
        //    float x5 = float.Parse("20");
        //    float x6 = float.Parse("30");
        //    float x7 = float.Parse("10");
        //    float x8 = float.Parse("20");
        //    float x9 = float.Parse("10");
        //    float x10 = float.Parse("30");
        //    float x11 = float.Parse("20");
        //    float x12 = float.Parse("10");
        //    var chart = chart1.ChartAreas[0];
        //    chart.AxisX.LabelStyle.Format = "yyyy-MM-dd";
        //    // chart.AxisX.IntervalType = DateTimeIntervalType.Number;

        //    //chart.AxisX.LabelStyle.Format = "";
        //    //chart.AxisY.LabelStyle.Format = "";
        //    chart.AxisY.LabelStyle.IsEndLabelVisible = true;

        //    chart.AxisX.Minimum = 1;
        //    chart.AxisX.Maximum = 12;
        //    chart.AxisY.Minimum = 0;
        //    chart.AxisY.Maximum = 50;
        //    chart.AxisX.Interval = 1;
        //    chart.AxisY.Interval = 5;

        //    chart1.Series.Add("line1");   //数据1

        //    chart1.Series["line1"].LegendText = "数据1";

        //    // chart1.Series["line1"].ChartType = SeriesChartType.Line; //绘制折线图

        //    chart1.Series["line1"].ChartType = SeriesChartType.Spline;  //绘制曲线图

        //    chart1.Series["line1"].Color = Color.Red;
        //    chart1.Series[0].IsVisibleInLegend = true;

        //    chart1.Series["line1"].Points.AddXY(DateTime.Now.ToOADate(), x1);
        //    chart1.Series["line1"].Points.AddXY(2, x2);
        //    chart1.Series["line1"].Points.AddXY(3, x3);
        //    chart1.Series["line1"].Points.AddXY(4, x4);
        //    chart1.Series["line1"].Points.AddXY(5, x5);
        //    chart1.Series["line1"].Points.AddXY(6, x6);
        //    chart1.Series["line1"].Points.AddXY(7, x7);
        //    chart1.Series["line1"].Points.AddXY(8, x8);
        //    chart1.Series["line1"].Points.AddXY(9, x9);
        //    chart1.Series["line1"].Points.AddXY(10, x10);
        //    chart1.Series["line1"].Points.AddXY(11, x11);
        //    chart1.Series["line1"].Points.AddXY(12, x12);


        //}


        #endregion


        #region 

        List<DateTime> xData2 = new List<DateTime>() { };
        List<int> yData2 = new List<int>() {  };
        public void hautu()
        {
            
            //这是添加的两组数据

            List<int> txData3 = new List<int>() { 2012 };
            List<int> tyData3 = new List<int>() { 7 };
            Chart ct = new Chart();
           
            ct.Parent= splitContainer1.Panel1;
            ct.Show();
            ct.Dock = DockStyle.Fill;
            //若为new一个Chart，需同时Add其Title，Series,ChartAreas等属性
            //若是直接拖入控件则只需在控件属性中自己调整就好
            //标题
            ct.Titles.Add("关键字统计");
            //背景
            ct.ChartAreas.Add(new ChartArea() { Name = "ca1" });     //背景框
            ct.ChartAreas["ca1"].AxisX.LabelStyle.Format = "yyyy-MM-dd";
                ct.ChartAreas["ca1"].AxisX.Minimum= Convert.ToDateTime("2010-12-31").ToOADate();
            ct.ChartAreas["ca1"].AxisX.Interval =300 ;
            ct.ChartAreas["ca1"].AxisX.Maximum = DateTime.Now.ToOADate();
            //图表数据区，有多个重叠则循环添加
            ct.Series.Add(new Series()); //添加一个图表序列
                                         // ct.Series[0].XValueType = ChartValueType.String;  //设置X轴上的值类型
            ct.Series[0].Label = "#VAL";                //设置显示X Y的值    
            ct.Series[0].ToolTip = "#VALX年\r#VAL";     //鼠标移动到对应点显示数值
            ct.Series[0].ChartArea = "ca1";                   //设置图表背景框
            ct.Series[0].ChartType = SeriesChartType.Spline;    //图类型(折线)
            ct.Series[0].Points.DataBindXY(xData2, yData2); //添加数据
            ct.Series[0].BorderWidth = 3;



            ct.Series.Add(new Series()); //添加一个图表序列
            ct.Series[1].Label = "#VAL";                //设置显示X Y的值
            ct.Series[1].ToolTip = "#VALX年\r#VAL";     //鼠标移动到对应点显示数值
            ct.Series[1].ChartType = SeriesChartType.Spline;    //图类型(曲线)
            ct.Series[1].Points.DataBindXY(txData3, tyData3); //添加数据
          

        }
        #endregion
        string path = AppDomain.CurrentDomain.BaseDirectory + "data\\";
        /// <summary>
        /// 判断包含并存储日期
        /// </summary>
        /// <param name="title"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public string baohan(string title,string date)
        {
            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != "")
                {
                   
                    if (title.Contains(text[i]))
                    {
                        if (!File.Exists(path + text[i] + ".txt"))
                        {
                            FileStream fs1 = new FileStream(path + text[i] + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                            StreamWriter sw = new StreamWriter(fs1);
                            sw.WriteLine(date);
                            sw.Close();
                            fs1.Close();
                        }
                        else
                        {
                            StreamWriter fs = new StreamWriter(path + text[i] + ".txt", true);
                            fs.WriteLine(date);
                            fs.Close();
                        }

                       return text[i];
                    }
                 }
            }
            return "";
        }

        public void run()
        {
           
                for (int page = 1; page < 2501; page++)
                {
                    
                    string url = "http://www.jiaoyizhe.com/forum-16-" + page + ".html";


                    string html = method.GetUrl(url, "gbk");

                    MatchCollection ids = Regex.Matches(html, @"<tbody id=""normalthread_([\s\S]*?)""");
                    MatchCollection titles = Regex.Matches(html, @"class=""s xst"">([\s\S]*?)</a>");
                    MatchCollection dates = Regex.Matches(html, @"<em><span([\s\S]*?)</span>");

                    for (int j = 0; j < ids.Count; j++)
                    {

                    Match DATE = Regex.Match(dates[j].Groups[1].Value, @"\d{4}-\d{1,2}-\d{1,2}");


                    string keyword = baohan(titles[j].Groups[1].Value, DATE.Groups[0].Value);
                    if (keyword != "")
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                        lv1.SubItems.Add(titles[j].Groups[1].Value);
                        lv1.SubItems.Add(keyword);
                        lv1.SubItems.Add(DATE.Groups[0].Value);
                    }

                    if (status == false)
                        return;
                }

            }



            

        }

        private void button1_Click(object sender, EventArgs e)
        {
          

            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"ceshi1111111"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入关键字");
                return;
            }
            status = true;
            button1.Enabled = false;

            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void 关键字统计曲线图_Load(object sender, EventArgs e)
        {
            //chart1.Series.Clear(); //清除默认的series
            
        }


        public void jisuan(string keyword)
        {
            StreamReader sr = new StreamReader(path+keyword+".txt", Encoding.Default);
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            Dictionary<string, int> dic = new Dictionary<string, int>();
            for (int i = 0; i < text.Length; i++)
            {
               
                string value = text[i].Trim();
                if (value != "")
                {
                    if (!dic.ContainsKey(value))
                    {
                        dic.Add(value, 1);   //1代表只有1个

                    }
                    else
                    {
                        dic[value]++;       //包含了则增加1
                    }
                }

            }

            var dicSort = from pair in dic orderby pair.Key select pair;
            foreach (KeyValuePair<string, int> item in dicSort)
            {
               // textBox1.Text += item.Key + " " + item.Value + "\r\n";
                xData2.Add(Convert.ToDateTime(item.Key));
                yData2.Add(item.Value);

            }
        }
        private void button2_Click(object sender, EventArgs e)
        {

            jisuan("交易");
            hautu();
           
        }
        bool status = true;
        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
            button1.Enabled = true;
        }
    }
}
