using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 主程序
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        
        //public void run()

        //{
        //    for (int i = 1167580800; i < 1583337601; i=i+86400)
        //    {
        //        string url = "https://mocdn.1394x.com/xyft/History?version=3000&timestamp="+i;
        //        string html = method.GetUrl(url,"utf-8");
        //        MatchCollection qishus = Regex.Matches(html, @"""period"":""([\s\S]*?)""");
        //        MatchCollection dates = Regex.Matches(html, @"""date"":""([\s\S]*?)""");
        //        MatchCollection times = Regex.Matches(html, @"""time"":""([\s\S]*?)""");
        //        MatchCollection results = Regex.Matches(html, @"""result"":""([\s\S]*?)""");

        //        for (int j = 0; j < qishus.Count; j++)
        //        {
        //            //ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
        //            //lv1.SubItems.Add(qishus[j].Groups[1].Value);
        //            //lv1.SubItems.Add(dates[j].Groups[1].Value);
        //            //lv1.SubItems.Add(times[j].Groups[1].Value);
        //            //lv1.SubItems.Add(results[j].Groups[1].Value);

        //            insertdata("INSERT INTO datas VALUES( '" + qishus[j].Groups[1].Value + "','" + dates[j].Groups[1].Value + "','" + times[j].Groups[1].Value + "','" + results[j].Groups[1].Value + "')");


        //        }
        //        label1.Text = ConvertStringToDateTime(i.ToString()).ToString();
        //        Thread.Sleep(1000);

        //    }

        //}
        /// <summary>
        /// 插入数据库
        /// </summary>
        public void insertdata(string sql)
        {
            try
            {

                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\data.db");
                mycon.Open();

                SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

                cmd.ExecuteNonQuery();  //执行sql语句
                mycon.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString());

            }

        }
        ArrayList resultList = new ArrayList();
        /// <summary>
        /// 读取数据库
        /// </summary>
        public void getdata()
        {
            try
            {

                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\data.db");
                mycon.Open();

                SQLiteCommand cmd = new SQLiteCommand("select result from datas", mycon);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(rdr);

                for (int i = 0; i < table.Rows.Count; i++) // 遍历行
                {

                    resultList.Add(table.Rows[i]["result"]);

                }
                mycon.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString());

            }

        }

        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(Convert.ToDouble(timeStamp));


        }
        


        public void run()
        {
            //  getdata();

            
            foreach (var a in textBox1.Text)
            {
                MessageBox.Show(a.ToString());
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {


            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
