using System;
using System.Collections;
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

namespace 专辑
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool status = true;
        ArrayList wxs = new ArrayList();
        ArrayList quns = new ArrayList();
        #region 喜马拉雅
        public void ximalya()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入分类地址");
                return;
            }

            if (textBox5.Text == "")
            {
                MessageBox.Show("请输入保存文件关键字名称");
                return;
            }


            for (int i = 1; i < 1000; i++)
            {
                string url = textBox1.Text + "p" + i + "/";
                string html = method.GetUrl(url, "utf-8");

                MatchCollection matches = Regex.Matches(html, @"i2g"" href=""([\s\S]*?)""");
                
              
                if (matches.Count == 0)
                    break;
                for (int j = 0; j < matches.Count; j++)
                {
                    string URL = "https://www.ximalaya.com" + matches[j].Groups[1].Value;
                    //string URL = "https://www.ximalaya.com/shangye/6571744/";
                    string strhtml = method.GetUrl(URL, "utf-8");
                    Match zhuanji = Regex.Match(strhtml, @"""sourceKw"":""([\s\S]*?)""");
                    Match zhubo = Regex.Match(strhtml, @"""anchorName"":""([\s\S]*?)""");


                    Match  jieshao1 = Regex.Match(strhtml, @"""personalIntroduction"":""([\s\S]*?)""");
                    Match jieshao2 = Regex.Match(strhtml, @"<article class=""intro _lm"">([\s\S]*?)</article>");
                    string jie1 = Regex.Replace(jieshao1.Groups[1].Value, "<[^>]+>", "");
                    string jie2 = Regex.Replace(jieshao2.Groups[1].Value, "<[^>]+>", "");

                    string jie3 = Regex.Replace(jie1 + "," + jie2, "http.*/", ""); //去标签


                    MatchCollection weixins = Regex.Matches(jie3, @"[A-Za-z0-9]{5,20}");
                    StringBuilder sb = new StringBuilder();
                    foreach (Match item in weixins)
                    {
                        if (!wxs.Contains(item.Groups[0].Value))
                        {
                            wxs.Add(item.Groups[0].Value);
                            sb.Append(item.Groups[0].Value + ",");
                        }
                        
                    }

                    textBox3.Text += "正在采集"+ zhuanji.Groups[1].Value + "\r\n";
                    if (sb.ToString()!= "")
                    {
                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(URL);
                        listViewItem.SubItems.Add(zhuanji.Groups[1].Value);
                        listViewItem.SubItems.Add(zhubo.Groups[1].Value);
                        listViewItem.SubItems.Add(sb.ToString());
                        listViewItem.SubItems.Add(textBox5.Text.Trim());
                        if (status == false)
                        {
                            return;
                        }

                        Thread.Sleep(100);
                    }
                   
                }

               
            }
            string path = AppDomain.CurrentDomain.BaseDirectory;
            method.DataTableToExcelTime(method.listViewToDataTable(this.listView1), true,path+ DateTime.Now.ToString("yyyy-MM-dd") + textBox5.Text.Trim() + ".xlsx");
        }
        #endregion

        /// <summary>
        /// 腾讯课堂
        /// </summary>
        public void tengxun()
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("请输入关键字");
                return;
            }

            for (int i = 1; i < 1000; i++)
            {
                string url = "https://ke.qq.com/course/list/"+ System.Web.HttpUtility.UrlEncode(textBox2.Text)+"?page="+i;
                string html = method.GetUrl(url, "utf-8");

                MatchCollection matches = Regex.Matches(html, @"data-id=""([\s\S]*?)""");
               

                if (matches.Count == 0)
                    break;
                for (int j = 0; j < matches.Count; j++)
                {
                    string URL = "https://ke.qq.com/course/" + matches[j].Groups[1].Value;
                    string strhtml = method.GetUrl(URL, "utf-8");
                    Match zhuanji = Regex.Match(strhtml, @"<title>([\s\S]*?)-");
                    Match zhubo = Regex.Match(strhtml, @"""agency_name"":""([\s\S]*?)""");


                    Match jieshao1 = Regex.Match(strhtml, @"""agency_summay"":""([\s\S]*?)""");
                    Match jieshao2 = Regex.Match(strhtml, @"介</th>([\s\S]*?)</tr>");
                    string jie1 = Regex.Replace(jieshao1.Groups[1].Value, "<[^>]+>", "");
                    string jie2 = Regex.Replace(jieshao2.Groups[1].Value, "<[^>]+>", "");

                    Match qun = Regex.Match(strhtml, @"data-gc=""([\s\S]*?)""");
                    MatchCollection shoujis = Regex.Matches(strhtml, @"<i class=""item-icon icon-font i-phone""></i>([\s\S]*?)</p>");

                    MatchCollection weixins = Regex.Matches(jie1 + jie2, @"[A-Za-z0-9]{5,20}");
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sb1 = new StringBuilder();
                    foreach (Match item in weixins)
                    {
                        sb.Append(item.Groups[0].Value + ",");
                    }

                    foreach (Match item in shoujis)
                    {
                        sb1.Append(Regex.Replace(item.Groups[1].Value, "<[^>]+>", "").Trim() + ",");
                    }
                    //if (!quns.Contains(qun.Groups[1].Value))
                    //{
                    //    quns.Add(qun.Groups[1].Value);

                        textBox4.Text += "正在采集" + zhuanji.Groups[1].Value + "\r\n";
                        ListViewItem listViewItem = this.listView2.Items.Add((listView2.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(URL);
                        listViewItem.SubItems.Add(zhuanji.Groups[1].Value);
                        listViewItem.SubItems.Add(zhubo.Groups[1].Value);
                        listViewItem.SubItems.Add(sb.ToString());
                        listViewItem.SubItems.Add(qun.Groups[1].Value);
                        listViewItem.SubItems.Add(sb1.ToString());
                        listViewItem.SubItems.Add(textBox2.Text.Trim());
                        if (status == false)
                        {
                            return;
                        }

                        Thread.Sleep(1000);
                    
                }

                
            }

            string path = AppDomain.CurrentDomain.BaseDirectory;
            method.DataTableToExcelTime(method.listViewToDataTable(this.listView2), true, path + DateTime.Now.ToString("yyyy-MM-dd") + textBox2.Text.Trim() + ".xlsx");

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            status = true;
            string constr = "Host =47.99.68.92;Database=vip_database;Username=root;Password=zhoukaige00.@*.";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();

            MySqlCommand cmd = new MySqlCommand("select * from vip where username='专辑'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

            MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

            if (reader.Read())
            {

                string password = reader["password"].ToString().Trim();

                if (password != "专辑")

                {
                    MessageBox.Show("验证失败");

                    Environment.Exit(0);
                }


                Thread thread = new Thread(new ThreadStart(ximalya));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
     
            status = false;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            status = true;
            string constr = "Host =47.99.68.92;Database=vip_database;Username=root;Password=zhoukaige00.@*.";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();

            MySqlCommand cmd = new MySqlCommand("select * from vip where username='专辑'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

            MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

            if (reader.Read())
            {

                string password = reader["password"].ToString().Trim();

                if (password != "专辑")

                {
                    MessageBox.Show("验证失败");

                    Environment.Exit(0);
                }


                Thread thread = new Thread(new ThreadStart(tengxun));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }
        }
    }
}
