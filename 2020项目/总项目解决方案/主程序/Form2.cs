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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public void getnew()

        {
            


            string url = "https://api.api861861.com/pks/getPksHistoryList.do?lotCode=10012&date=" + DateTime.Now.ToString("yyyy-MM-dd");

           

            string html = method.GetUrl(url, "utf-8");

            MatchCollection qishus = Regex.Matches(html, @"""preDrawIssue"":([\s\S]*?),");
            
            MatchCollection times = Regex.Matches(html, @"""preDrawTime"":""([\s\S]*?)""");
            MatchCollection results = Regex.Matches(html, @"""preDrawCode"":""([\s\S]*?)""");

            for (int j = 0; j < qishus.Count; j++)
            {
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                lv1.SubItems.Add(qishus[j].Groups[1].Value);
               
                lv1.SubItems.Add(times[j].Groups[1].Value);
                lv1.SubItems.Add(results[j].Groups[1].Value);


            }

            if (qishus.Count > 3)

            {
                textBox2.Text = results[0].Groups[1].Value;
                textBox3.Text = results[1].Groups[1].Value;
                textBox4.Text = results[2].Groups[1].Value;
                textBox5.Text = results[3].Groups[1].Value;
            }



        }
      
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

        bool status = true;
        ArrayList resultList = new ArrayList();
        private void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            listView1.Items.Clear();
            getnew();
        }
        public int getxiangsi(string s, string y)
        {
            string[] shuru = s.Split(new string[] { "," }, StringSplitOptions.None);
            string[] yuan = y.Split(new string[] { "," }, StringSplitOptions.None);

            int geshu = 0;

            if (shuru[0] == yuan[0])
            {
                geshu = geshu + 1;
            }

            if (shuru[1] == yuan[1])
            {
                geshu = geshu + 1;
            }
            if (shuru[2] == yuan[2])
            {
                geshu = geshu + 1;
            }
            if (shuru[3] == yuan[3])
            {
                geshu = geshu + 1;
            }
            if (shuru[4] == yuan[4])
            {
                geshu = geshu + 1;
            }
            if (shuru[5] == yuan[5])
            {
                geshu = geshu + 1;
            }
            if (shuru[6] == yuan[6])
            {
                geshu = geshu + 1;
            }
            if (shuru[7] == yuan[7])
            {
                geshu = geshu + 1;
            }
            if (shuru[8] == yuan[8])
            {
                geshu = geshu + 1;
            }
            if (shuru[9] == yuan[9])
            {
                geshu = geshu + 1;
            }

            return geshu;
        }
        #region run1
        public void run1()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("值为空");
                return;
            }
            string shuru = "";
            if (textBox1.Text.Contains(","))
            {
                shuru = textBox1.Text.Trim();
            }
            else
            {
                foreach (var item in textBox1.Text.Trim())
                {
                    shuru += item + ",";
                }
                shuru = shuru.Remove(shuru.Length - 1, 1);

            }


            for (int i = 0; i < resultList.Count; i++)
            {
                int value = getxiangsi(shuru, resultList[i].ToString());

                label6.Text = "正在分析" + resultList[i];
                if (value > 6 && shuru != resultList[i].ToString())
                {
                    //textBox6.Text += resultList[i].ToString() + "\r\n";
                    ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString() + ":" + resultList[i - 1].ToString().Remove(resultList[i - 1].ToString().Length - 6, 6));
                    button2.Enabled = true;
                }

                if (status == false)
                    return;

                if (listView2.Items.Count > 6)
                    return;
            }




        }

        #endregion

        #region run4
        public void run4()
        {
            if (textBox4.Text == "")
            {
                MessageBox.Show("值为空");
                return;
            }
            string shuru = "";
            if (textBox4.Text.Contains(","))
            {
                shuru = textBox4.Text.Trim();
            }
            else
            {
                foreach (var item in textBox4.Text.Trim())
                {
                    shuru += item + ",";
                }
                shuru = shuru.Remove(shuru.Length - 1, 1);

            }


            for (int i = 0; i < resultList.Count; i++)
            {
                int value = getxiangsi(shuru, resultList[i].ToString());

                label9.Text = "正在分析" + resultList[i];
                if (value > 6 && shuru != resultList[i].ToString())
                {
                    //textBox6.Text += resultList[i].ToString() + "\r\n";
                    ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString() + ":" + resultList[i - 1].ToString().Remove(resultList[i - 1].ToString().Length - 6, 6));
                    button2.Enabled = true;
                }
                if (status == false)
                    return;

                if (listView2.Items.Count > 6)
                    return;
            }




        }

        #endregion

        #region run5
        public void run5()
        {
            if (textBox5.Text == "")
            {
                MessageBox.Show("值为空");
                return;
            }
            string shuru = "";
            if (textBox5.Text.Contains(","))
            {
                shuru = textBox5.Text.Trim();
            }
            else
            {
                foreach (var item in textBox5.Text.Trim())
                {
                    shuru += item + ",";
                }
                shuru = shuru.Remove(shuru.Length - 1, 1);

            }


            for (int i = 0; i < resultList.Count; i++)
            {
                int value = getxiangsi(shuru, resultList[i].ToString());

                label10.Text = "正在分析" + resultList[i];
                if (value > 6 && shuru != resultList[i].ToString())
                {
                    //textBox6.Text += resultList[i].ToString() + "\r\n";
                    ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString() + ":" + resultList[i - 1].ToString().Remove(resultList[i - 1].ToString().Length - 6, 6));
                    button2.Enabled = true;
                }

                if (status == false)
                    return;
                if (listView2.Items.Count > 6)
                    return;
            }




        }

        #endregion

        #region run2
        public void run2()
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("值为空");
                return;
            }
            string shuru = "";
            if (textBox2.Text.Contains(","))
            {
                shuru = textBox2.Text.Trim();
            }
            else
            {
                foreach (var item in textBox2.Text.Trim())
                {
                    shuru += item + ",";
                }
                shuru = shuru.Remove(shuru.Length - 1, 1);

            }


            for (int i = 0; i < resultList.Count; i++)
            {
                int value = getxiangsi(shuru, resultList[i].ToString());

                label7.Text = "正在分析" + resultList[i];
                if (value > 6 && shuru != resultList[i].ToString())
                {
                    //textBox6.Text += resultList[i].ToString() + "\r\n";
                    ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString() + ":" + resultList[i - 1].ToString().Remove(resultList[i - 1].ToString().Length - 6, 6));
                    button2.Enabled = true;
                }

                if (status == false)
                    return;
                if (listView2.Items.Count > 6)
                    return;
            }




        }

        #endregion

        #region run3
        public void run3()
        {
            if (textBox3.Text == "")
            {
                MessageBox.Show("值为空");
                return;
            }
            string shuru = "";
            if (textBox3.Text.Contains(","))
            {
                shuru = textBox3.Text.Trim();
            }
            else
            {
                foreach (var item in textBox3.Text.Trim())
                {
                    shuru += item + ",";
                }
                shuru = shuru.Remove(shuru.Length - 1, 1);

            }


            for (int i = 0; i < resultList.Count; i++)
            {
                int value = getxiangsi(shuru, resultList[i].ToString());

                label8.Text = "正在分析" + resultList[i];
                if (value > 6 && shuru != resultList[i].ToString())
                {
                    //textBox6.Text += resultList[i].ToString() + "\r\n";
                    ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString() + ":" + resultList[i - 1].ToString().Remove(resultList[i - 1].ToString().Length - 6, 6));
                    button2.Enabled = true;
                }
                if (status == false)
                    return;

                if (listView2.Items.Count > 6)
                    return;
            }




        }

        #endregion
        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Start();

            button2.Enabled = false;
            status = true;
            getdata();

            Thread thread1 = new Thread(new ThreadStart(run1));
            thread1.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            Thread thread2 = new Thread(new ThreadStart(run2));
            thread2.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            Thread thread3 = new Thread(new ThreadStart(run3));
            thread3.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            Thread thread4 = new Thread(new ThreadStart(run4));
            thread4.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            Thread thread5 = new Thread(new ThreadStart(run5));
            thread5.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
            danshuang();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
            daxiao();
        }
        /// <summary>
        /// 大小
        /// </summary>
        /// <param name="dan"></param>
        public void daxiao()

        {
            DateTime time = Convert.ToDateTime(DateTime.Now.ToString("yyyy/M/d") + " 0:0:0");


            


            string url = "https://api.api861861.com/pks/getPksHistoryList.do?lotCode=10012&date=" + DateTime.Now.ToString("yyyy-MM-dd");



            string html = method.GetUrl(url, "utf-8");

            MatchCollection qishus = Regex.Matches(html, @"""preDrawIssue"":([\s\S]*?),");

            MatchCollection times = Regex.Matches(html, @"""preDrawTime"":""([\s\S]*?)""");
            MatchCollection results = Regex.Matches(html, @"""preDrawCode"":""([\s\S]*?)""");

            for (int j = 0; j < qishus.Count; j++)
            {
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                lv1.SubItems.Add(qishus[j].Groups[1].Value);
               
                lv1.SubItems.Add(times[j].Groups[1].Value);

                StringBuilder sb = new StringBuilder();
                string[] text = results[j].Groups[1].Value.Split(new string[] { "," }, StringSplitOptions.None);
                for (int a = 0; a < text.Length; a++)
                {
                    if (Convert.ToInt32(text[a]) < 6)
                    {
                        sb.Append("小 ");
                    }
                    else
                    {
                        sb.Append("大 ");
                    }

                }

                lv1.SubItems.Add(sb.ToString());


            }



        }

        /// <summary>
        /// 单双
        /// </summary>
        /// <param name="dan"></param>
        public void danshuang()

        {
            DateTime time = Convert.ToDateTime(DateTime.Now.ToString("yyyy/M/d") + " 0:0:0");


            string url = "https://api.api861861.com/pks/getPksHistoryList.do?lotCode=10012&date=" + DateTime.Now.ToString("yyyy-MM-dd");



            string html = method.GetUrl(url, "utf-8");

            MatchCollection qishus = Regex.Matches(html, @"""preDrawIssue"":([\s\S]*?),");

            MatchCollection times = Regex.Matches(html, @"""preDrawTime"":""([\s\S]*?)""");
            MatchCollection results = Regex.Matches(html, @"""preDrawCode"":""([\s\S]*?)""");

            for (int j = 0; j < qishus.Count; j++)
            {
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                lv1.SubItems.Add(qishus[j].Groups[1].Value);
                
                lv1.SubItems.Add(times[j].Groups[1].Value);

                StringBuilder sb = new StringBuilder();
                string[] text = results[j].Groups[1].Value.Split(new string[] { "," }, StringSplitOptions.None);
                for (int a = 0; a < text.Length; a++)
                {
                    if (Convert.ToInt32(text[a]) % 2 == 1)
                    {
                        sb.Append("单 ");
                    }
                    else
                    {
                        sb.Append("双 ");
                    }

                }

                lv1.SubItems.Add(sb.ToString());


            }





        }





    }
}
