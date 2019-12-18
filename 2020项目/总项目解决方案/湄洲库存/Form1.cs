using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 湄洲库存
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int start = 1;
        bool zanting = true;
        bool status = true;

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentType = "application/json";
            request.ContentLength = postData.Length;
            //request.AllowAutoRedirect = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
            request.Headers.Add("Cookie", COOKIE);
            request.Referer = "http://data.imiker.com/all_search/hs/buy/all/621210";
            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(postData);
            sw.Flush();

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
            response.GetResponseHeader("Set-Cookie");
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

            string html = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return html;

        }

        #endregion

        /// <summary>
        /// 获取关键字
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string getkey(int id)
        {
           
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;//获取当前程序运行文件夹

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\DCKSTOCK.db");
              
                string sql = "SELECT * from partIndex where Field_4=" + id;
                mycon.Open();
              
                SQLiteCommand cmd = new SQLiteCommand(sql, mycon);
                
                SQLiteDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源
                reader.Read();
                string keyword = reader["Field_1"].ToString().Trim();
                reader.Close();
                mycon.Close();
                return keyword;
            }
            catch (SQLiteException ex)
            {
                 ex.ToString();
                return "";
            }
           

        }
        /// <summary>
        /// 插入数据库
        /// </summary>
        public void insertdata(string sql)
        {
            try
            {

                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\DCKSTOCK.db");
                mycon.Open();
                
                SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

                cmd.ExecuteNonQuery();  //执行sql语句
                mycon.Close();
            }
            catch (SQLiteException ex)
            {
              MessageBox.Show(  ex.ToString());

            }

        }

        #region 获取页码

        public string getpage(string key)
        {
            string url = "http://nddb.ic361.cn:8050/proc/b2b/mz_search";
            string postdata = "para%5Bpz%5D=&para%5Bis_key%5D=0&para%5Bis_exact%5D=0&para%5Bpn%5D=" + key + "&para%5Bpage%5D=1&para%5Bpage_size%5D=30&token=&uid=-1";
            string cookie = "";
            string html = PostUrl(url, postdata, cookie, "utf-8");

            Match page = Regex.Match(html, @"""total_page"":([\s\S]*?),");
            Match total = Regex.Match(html, @"""total"":([\s\S]*?)}");

            return page.Groups[1].Value.Trim()+","+ total.Groups[1].Value.Trim();
        }
        #endregion


        public void run()
        {
            try
            {
                for (int i = start; i < 999999999; i++)
                {

                    string keyword = getkey(i);
                   
                    int page = 0;

                    string[] text = getpage(keyword).Split(new string[] { "," }, StringSplitOptions.None);
                    if (text[0] != "")
                    {
                         page = Convert.ToInt32(text[0]);
                    }
                    string total = text[1];

                  
                    //if (page =="")
                    //    break;
                  

                    for (int j = 1; j <=page; j++)
                    {
                        string url = "http://nddb.ic361.cn:8050/proc/b2b/mz_search";
                        string postdata = "para%5Bpz%5D=&para%5Bis_key%5D=0&para%5Bis_exact%5D=0&para%5Bpn%5D=" + keyword + "&para%5Bpage%5D="+j+"&para%5Bpage_size%5D=30&token=&uid=-1";
                        string cookie = "";
                        string html = PostUrl(url, postdata, cookie, "utf-8");

                        MatchCollection a1s = Regex.Matches(html, @"""co_name"":([\s\S]*?),");
                        MatchCollection a2s = Regex.Matches(html, @"""pn"":([\s\S]*?),");
                        MatchCollection a3s = Regex.Matches(html, @"""qty"":([\s\S]*?),");
                        MatchCollection a4s = Regex.Matches(html, @"""price"":([\s\S]*?),");
                        MatchCollection a5s = Regex.Matches(html, @"""mfg"":([\s\S]*?),");
                        MatchCollection a6s = Regex.Matches(html, @"""dc"":([\s\S]*?),");
                        MatchCollection a7s = Regex.Matches(html, @"""pck"":([\s\S]*?),");

                        MatchCollection a8s = Regex.Matches(html, @"""pz"":([\s\S]*?),");
                        MatchCollection a9s = Regex.Matches(html, @"""stock_type_name"":([\s\S]*?),");
                        MatchCollection a10s = Regex.Matches(html, @"""warehouse"":([\s\S]*?),");
                        MatchCollection a11s = Regex.Matches(html, @"""comment"":([\s\S]*?),");
                        MatchCollection a12s = Regex.Matches(html, @"""mycoid"":([\s\S]*?),");

                        for (int z = 0; z < a1s.Count; z++)
                        {
                           
                           string a1 = a1s[z].Groups[1].Value.Replace("\"", "");
                            string a2 = a2s[z].Groups[1].Value.Replace("\"", "");
                            string a3 = a3s[z].Groups[1].Value.Replace("\"", "");
                            string a4 = a4s[z].Groups[1].Value.Replace("\"", "");
                            string a5 = a5s[z].Groups[1].Value.Replace("\"", "");
                            string a6 = a6s[z].Groups[1].Value.Replace("\"", "");
                            string a7 = a7s[z].Groups[1].Value.Replace("\"", "");
                            string a8 = a8s[z].Groups[1].Value.Replace("\"", "");
                            string a9 = a9s[z].Groups[1].Value.Replace("\"", "");
                            string a10 = a10s[z].Groups[1].Value.Replace("\"","");
                            string a11 = a11s[z].Groups[1].Value.Replace("\"", "");
                            string a12 = a12s[z].Groups[1].Value.Replace("\"", "");

                         
                           
                            label2.Text = "正在抓取：" +a1;
                            string sql = "INSERT INTO stockList VALUES( '" + i + "','" + a1 + "','" + a2 + "','" + a3 + "','" + a4 + "','" + a5 + "','" + a6 + "','" + a7 + "','" + a8 + "','" + a9 + "','" + a10 +"', '" + a11+ "', '" + keyword + "', '" + a12+ "')";
                            insertdata(sql);
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                            {
                                return;
                            }

                        }//当前页结束

                        Thread.Sleep(Convert.ToInt32(textBox2.Text));

                    }//翻页结束

                    textBox1.Text= textBox1.Text.Insert(0, DateTime.Now.ToString()+":"+ keyword + "，正在采集到的" + total + "数据"+"\r\n");
                    string sql2 = "INSERT INTO Keyword VALUES('" + i+ "','" + keyword + "','1','" + total + "')";
                    insertdata(sql2);
                    if (textBox1.Lines.Length==1000)
                    {
                        textBox1.Text = "";
                    }

                    //记录当前运行到的i
                    string path = AppDomain.CurrentDomain.BaseDirectory;
                    FileStream fs1 = new FileStream(path + "config.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine(i);
                    sw.Close();
                    fs1.Close();

                }//关键字结束
            }
            catch (Exception ex)
            {
               textBox1.Text= ex.ToString();
                
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            StreamReader sr = new StreamReader(path + "config.txt", Encoding.Default);
            string text = sr.ReadToEnd();
            if (text != "")
            {
                start = Convert.ToInt32(text);
            }
            sr.Close();

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            status = true;
            label2.Text = "开始采集中.....";

            string constr = "Host =47.99.68.92;Database=vip_database;Username=root;Password=zhoukaige00.@*.";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();

            MySqlCommand cmd = new MySqlCommand("select * from vip where username='湄洲库存'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

            MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

            if (reader.Read())
            {

                string password = reader["password"].ToString().Trim();

                if (password != "湄洲库存")

                {
                    MessageBox.Show("验证失败");

                    Environment.Exit(0);
                }


                    Thread thread = new Thread(new ThreadStart(run));
                    thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
               
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
          
            status = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }

        }
    }
}
