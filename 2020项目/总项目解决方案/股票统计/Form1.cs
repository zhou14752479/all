using System;
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

namespace 股票统计
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public void run()

        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入网址");
                return;
            }

            string url = textBox1.Text.Trim();
            Match uid = Regex.Match(url, @"\d{5,}");
            string URL = "http://info.win0168.com/analysis/odds/"+uid.Groups[0].Value+".htm?1577324575000";
           
            string html = method.GetUrl(url,"utf-8");
            string ahtml = method.GetUrl(URL, "utf-8");

            Match a1 = Regex.Match(html, @"strTime='([\s\S]*?) ([\s\S]*?)'");
           
            Match a3 = Regex.Match(html, @"<span class=""LName"">([\s\S]*?)</span>");
           
            Match a5 = Regex.Match(html, @"hometeam=""([\s\S]*?)""");
            Match a6 = Regex.Match(html, @"guestteam=""([\s\S]*?)""");
           


            label12.Text = a1.Groups[1].Value.Trim();
            label13.Text = a1.Groups[2].Value.Trim();
            
            label15.Text = a3.Groups[1].Value.Trim();
            label16.Text = a5.Groups[1].Value.Trim();
            label17.Text = a6.Groups[1].Value.Trim();
           


            Match a8 = Regex.Match(ahtml, @",;([\s\S]*?);");
            string[] texts = a8.Groups[1].Value.Trim().Split(new string[] { "," }, StringSplitOptions.None);
            label19.Text = texts[11];
            label20.Text = texts[12];
            label21.Text = texts[13];








        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
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
        private void Button2_Click(object sender, EventArgs e)
        {
            //数据
            string a1 = label12.Text;
            string a2 = label13.Text;
            string a3 = label15.Text;
            string a4 = label16.Text;
            string a5 = label17.Text;
            string a6 = textBox1.Text;
            string a7 = label19.Text;
            string a8 = label20.Text;
            string a9 = label21.Text;
            //分类
            string a10 = label12.Text;
            string a11 = label12.Text;
            string a12 = label12.Text;
            string a13 = label12.Text;
            string a14 = label12.Text;
            string a15 = label12.Text;
            string a16 = label12.Text;
            string a17 = label12.Text;
            string a18 = label12.Text;

            string sql= "INSERT INTO stockList VALUES( '" + a1 + "','" + a2 + "','" + a3 + "','" + a4 + "','" + a5 + "','" + a6 + "','" + a7 + "','" + a8 + "','" + a9 + "','" + a10 + "', '" + a11 + "', '" + a12+ "', '" + a13 + "')";
            insertdata(sql);
        }
    }
}
