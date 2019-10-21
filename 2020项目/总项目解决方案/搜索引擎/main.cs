using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;
using MySql.Data.MySqlClient;

namespace 搜索引擎
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        bool zanting = true;

        #region  读取数据插入数据库

        public void insertData(string a, string b, string c, string d)
        {

            try
            {

                string constr = "Host =" + textBox1.Text.Trim() + ";Database=" + textBox2.Text.Trim() + ";Username=" + textBox3.Text.Trim() + ";Password=" + textBox4.Text.Trim();
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO '" + textBox6.Text.Trim() + " ' ('" + textBox7.Text.Trim() + " ','" + textBox8.Text.Trim() + " ','" + textBox9.Text.Trim() + " ','" + textBox10.Text.Trim() + " ')VALUES('" + a + " ', '" + b + " ','" + c + " ', '" + d + "')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {


                }

                mycon.Close();



            }

            catch (System.Exception ex)
            {
                ex.ToString();
            }


        }

        #endregion
       
        #region  百度新闻获取
        public void run()
        {
            try
            {
                label15.Text = "已启动正在采集......";
                string[] keywords = textBox5.Text.Split(new string[] { "," }, StringSplitOptions.None);
                foreach (string keyword in keywords)
                {


                    for (int i = 0; i < 9999; i = i + 10)
                    {

                        string url = "https://www.baidu.com/s?ie=utf-8&cl=2&medium=2&rtt=1&bsst=1&rsv_dl=news_b_pn&tn=news&wd=" + keyword + "&tfflag=0&x_bfe_rqs=03E80&x_bfe_tjscore=0.002154&tngroupname=organic_news&pn=" + i;
                        string html = method.GetUrl(url, "utf-8");

                        MatchCollection ids = Regex.Matches(html, @"baijiahao\.baidu\.com([\s\S]*?)""");


                        if (ids.Count == 0)
                            break;

                        for (int j = 0; j < ids.Count; j++)
                        {

                            string URL = "http://baijiahao.baidu.com" + ids[j].Groups[1].Value;

                            string strhtml = method.GetUrl(URL, "utf-8");



                            Match a1 = Regex.Match(strhtml, @"<h2>([\s\S]*?)</h2>");
                            Match a2 = Regex.Match(strhtml, @"dateUpdate"" content=""([\s\S]*?)""");
                            Match a3 = Regex.Match(strhtml, @"uthor-name"">([\s\S]*?)<");
                            Match a4 = Regex.Match(strhtml, @"<div class=""article-content"">([\s\S]*?)</div>");

                            //DateTime dt = Convert.ToDateTime(a2.Groups[1].Value);
                            //if (dateTimePicker1.Value < dt && dt < dateTimePicker2.Value)
                            //{
                            if (checkBox1.Checked == true)
                            {
                                insertData(a1.Groups[1].Value, a2.Groups[1].Value, a3.Groups[1].Value, a4.Groups[1].Value);
                            }


                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            listViewItem.SubItems.Add(a1.Groups[1].Value);
                            listViewItem.SubItems.Add(a2.Groups[1].Value);
                            listViewItem.SubItems.Add(a3.Groups[1].Value);
                            listViewItem.SubItems.Add(a4.Groups[1].Value);

                            listViewItem.SubItems.Add(URL);

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }

                            Thread.Sleep(100);
                            //    }


                        }
                    }


                }
            }




            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #endregion
        private void Main_Load(object sender, EventArgs e)
        {

        }
    }
}
