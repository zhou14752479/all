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
using MySql.Data.MySqlClient;

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
            label41.Text = uid.Groups[0].Value;


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
            string constr = "Host =47.99.68.92;Database=vip_database;Username=root;Password=zhoukaige00.@*.";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();

            MySqlCommand cmd = new MySqlCommand("select * from vip where username='股票统计'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

            MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

            if (reader.Read())
            {

                string password = reader["password"].ToString().Trim();

                if (password != "股票统计")

                {
                    MessageBox.Show("验证失败");

                    Environment.Exit(0);
                }


              

            }








            if (textBox5.Text == "")
            {
                
                return;
            }
            
            
            //数据
            string a1 = label12.Text;
            string a2 = label13.Text;
            string a3 = label15.Text;
            string a4 = label16.Text;
            string a5 = label17.Text;
            string a6 = textBox2.Text;
            string a7 = label19.Text;
            string a8 = label20.Text;
            string a9 = label21.Text;
            //分类
            string a10 = comboBox1.Text;
            string a11 = comboBox2.Text;
            string a12 = comboBox3.Text;
            string a13 = comboBox4.Text;
            string a14 = comboBox5.Text;
            string a15 = comboBox6.Text;
            string a16 = comboBox7.Text;
            string a17 = comboBox8.Text;
            string a18 = textBox3.Text;
            string a19 = textBox4.Text;
            string a20 = textBox5.Text;
            string ids = label41.Text;

            string sql= "INSERT INTO datas VALUES( '" + a1 + "','" + a2 + "','" + a3 + "','" + a4 + "','" + a5 + "','" + a6 + "','" + a7 + "','" + a8 + "','" + a9 + "','" + a10 + "', '" + a11 + "', '" + a12+ "', '" + a13 + "', '" + a14 + "', '" + a15 + "', '" + a16 + "', '" + a17 + "', '" + a18 + "', '" + a19 + "', '" + a20 + "', '" + ids + "')";
            insertdata(sql);
            MessageBox.Show("存储成功");
        }
        //查询
        public string chaxun(string sql)
        {

            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

            SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\data.db");
            mycon.Open();

            SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

            Convert.ToInt32(cmd.ExecuteScalar()).ToString();
            return Convert.ToInt32(cmd.ExecuteScalar()).ToString();

            

        }

        //查询求和
        public string qiuhe()
        {
            string sql = "select * from datas where a10='" + comboBox1.Text + "' AND a11='" + comboBox2.Text + "' AND a12='" + comboBox3.Text + "' AND a13='" + comboBox4.Text + "' AND a14='" + comboBox5.Text + "' AND a15='" + comboBox6.Text + "' AND a16='" + comboBox7.Text + "'    ";
            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

            SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\data.db");
            mycon.Open();

            SQLiteCommand cmd = new SQLiteCommand(sql, mycon);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(rdr);
            Decimal result = 0;
            for (int i = 0; i < table.Rows.Count; i++) // 遍历行
            {
          
                    result  = result+ Convert.ToDecimal(table.Rows[i][19]); 
           
            }
            return result.ToString();


        }

        private void Button3_Click(object sender, EventArgs e)
        {
           
            string all = "select count(*) from datas where a10='" + comboBox1.Text + "' AND a11='" + comboBox2.Text + "' AND a12='" + comboBox3.Text + "' AND a13='" + comboBox4.Text + "' AND a14='" + comboBox5.Text + "' AND a15='" + comboBox6.Text + "' AND a16='" + comboBox7.Text + "'  AND a1> '" + DateTime.Now.AddDays(-100) + "'   ";
            linkLabel1.Text = chaxun(all);

            string ying = "select count(*) from datas where a10='" + comboBox1.Text + "' AND a11='" + comboBox2.Text + "' AND a12='" + comboBox3.Text + "' AND a13='" + comboBox4.Text + "' AND a14='" + comboBox5.Text + "' AND a15='" + comboBox6.Text + "' AND a16='" + comboBox7.Text + "' AND a17='赢'   ";
            linkLabel2.Text = chaxun(ying);

            string banying = "select count(*) from datas where a10='" + comboBox1.Text + "' AND a11='" + comboBox2.Text + "' AND a12='" + comboBox3.Text + "' AND a13='" + comboBox4.Text + "' AND a14='" + comboBox5.Text + "' AND a15='" + comboBox6.Text + "' AND a16='" + comboBox7.Text + "' AND a17='半赢'   ";
            linkLabel3.Text = chaxun(banying);

            string ping = "select count(*) from datas where a10='" + comboBox1.Text + "' AND a11='" + comboBox2.Text + "' AND a12='" + comboBox3.Text + "' AND a13='" + comboBox4.Text + "' AND a14='" + comboBox5.Text + "' AND a15='" + comboBox6.Text + "' AND a16='" + comboBox7.Text + "' AND a17='平'   ";
            linkLabel4.Text = chaxun(ping);

            string banshu = "select count(*) from datas where a10='" + comboBox1.Text + "' AND a11='" + comboBox2.Text + "' AND a12='" + comboBox3.Text + "' AND a13='" + comboBox4.Text + "' AND a14='" + comboBox5.Text + "' AND a15='" + comboBox6.Text + "' AND a16='" + comboBox7.Text + "' AND a17='半输'   ";
            linkLabel5.Text = chaxun(banshu);

            string shu = "select count(*) from datas where a10='" + comboBox1.Text + "' AND a11='" + comboBox2.Text + "' AND a12='" + comboBox3.Text + "' AND a13='" + comboBox4.Text + "' AND a14='" + comboBox5.Text + "' AND a15='" + comboBox6.Text + "' AND a16='" + comboBox7.Text + "' AND a17='输'   ";
            linkLabel6.Text = chaxun(shu);

            linkLabel7.Text = qiuhe();

           double baifenbi= (Convert.ToInt32(linkLabel2.Text) + Convert.ToInt32(linkLabel3.Text) * 0.5) / (Convert.ToInt32(linkLabel2.Text) + Convert.ToInt32(linkLabel3.Text) * 0.5 + Convert.ToInt32(linkLabel6.Text) + Convert.ToInt32(linkLabel5.Text) * 0.5);
            linkLabel8.Text = Convert.ToInt32(baifenbi * 100) + "%";
        }

        private void ComboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox8.Text == "赢" && comboBox6.Text == "大")
            {
                textBox5.Text = (Convert.ToDecimal(label19.Text)*100).ToString();
            }

            if (comboBox8.Text == "赢" && comboBox6.Text == "小")
            {
                textBox5.Text = (Convert.ToDecimal(label21.Text) * 100).ToString();
            }

            if (comboBox8.Text == "半赢" && comboBox6.Text == "大")
            {
                textBox5.Text = (Convert.ToDecimal(label19.Text) * 50).ToString();
            }

            if (comboBox8.Text == "半赢" && comboBox6.Text == "小")
            {
                textBox5.Text = (Convert.ToDecimal(label21.Text) * 50).ToString();
            }



            if (comboBox8.Text == "平")
            {
                textBox5.Text = "0";
            }
            if (comboBox8.Text == "半输")
            {
                textBox5.Text = "-50";
            }
            if (comboBox8.Text == "输")
            {
                textBox5.Text = "-100";
            }
        }
    }
}
