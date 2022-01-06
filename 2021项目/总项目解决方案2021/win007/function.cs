using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace win007
{
    class function
    {
        string path = System.Environment.CurrentDirectory + "\\win007data.db"; //获取当前程序运行文件夹
        /// <summary>
        /// 查询数据库
        /// </summary>
        public DataTable chaxundata(string sql)
        {
            try
            {


                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path);
                mycon.Open();

                SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(sql, mycon);
                DataTable dt = new DataTable();
                mAdapter.Fill(dt);
                mycon.Close();
                return dt;

            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString());
                return null;


            }

        }

        /// <summary>
        /// 查询字段
        /// </summary>
        public string chaxunvalue(string sql)
        {
            try
            {
                string value = "";

                using (SQLiteConnection con = new SQLiteConnection("Data Source=" + path))
                {
                    con.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = string.Format(sql);
                        using (SQLiteDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (dr.Read())
                            {
                                value = dr["body"].ToString();

                            }
                        }
                    }
                    con.Close();
                }

                return value;

            }
            catch (SQLiteException ex)
            {

                return ex.ToString();


            }

        }


        /// <summary>
        /// 获取供货商
        /// </summary>
        public ArrayList getsupplyers()
        {
            try
            {
                ArrayList lists = new ArrayList();
                string sql = "select gongsi from datas";
                using (SQLiteConnection con = new SQLiteConnection("Data Source=" + path))
                {
                    con.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = string.Format(sql);
                        using (SQLiteDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (dr.Read())
                            {
                                string supplyer = dr["gongsi"].ToString();
                                if (!lists.Contains(supplyer))
                                {
                                    lists.Add(supplyer);
                                }


                            }
                        }
                    }
                    con.Close();
                }

                return lists;

            }
            catch (SQLiteException ex)
            {

                return null;


            }

        }

        /// <summary>
        /// 插入数据库
        /// </summary>
        public bool insertdata(string matchname,  string zhu, string ke,string time,string gongsi,string url,string bifen,string rangqiu, string data1, string data2, string data3, string data4, string data5, string data6,string data7,string data8,string data9)
        {
            try
            {
                string sql = "INSERT INTO datas(matchname,zhu,ke,time,gongsi,url,bifen,rangqiu,data1,data2,data3,data4,data5,data6,data7,data8,data9)VALUES('" + matchname + "'," +
                    "'" + zhu + "'," +
                     "'" + ke + "'," +
                      "'" + time + "'," +
                       "'" +gongsi + "'," +
                        "'" + url+ "'," +
                          "'" + bifen + "'," +
                            "'" + rangqiu + "'," +
                         "'" + data1 + "'," +
                          "'" + data2+ "'," +
                           "'" + data3 + "'," +
                            "'" + data4+ "'," +
                             "'" + data5+ "'," +
                              "'" + data6 + "'," +
                               "'" + data7 + "'," +
                                "'" + data8 + "'," +
                    "'" + data9 + "')";

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path);
                mycon.Open();

                SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

                int status = cmd.ExecuteNonQuery();  //执行sql语句
                if (status > 0)
                {
                    return true;

                }

                mycon.Close();
                return false;
            }
            catch (Exception)
            {


                return false;
            }

        }


        /// <summary>
        /// 执行SQL
        /// </summary>
        public bool SQL(string sql)
        {
            try
            {

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path);
                mycon.Open();

                SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

                int status = cmd.ExecuteNonQuery();  //执行sql语句
                if (status > 0)
                {
                    return true;

                }

                mycon.Close();
                return false;
            }
            catch (Exception)
            {


                return false;
            }

        }



        public void getdata(string day)
        {
            //http://bf.win007.com/football/Over_20211221.htm
            //http://op1.win007.com/oddslist/2128529.htm


            string url = "http://bf.win007.com/football/Over_" + day + ".htm";
            string html = method.GetUrl(url, "gb2312");
            MatchCollection tds = Regex.Matches(html, @"id='ls_([\s\S]*?)'>([\s\S]*?)</tr>");
            MatchCollection ids = Regex.Matches(html, @"showgoallist\(([\s\S]*?)\)");


            for (int i = 0; i < tds.Count; i++)
            {
                string baseinfo = Regex.Replace(tds[i].Groups[2].Value, "<[^>]+>", "");
                string datajsurl = "http://1x2d.win007.com/"+ids[i].Groups[1].Value+".js?r=007132848760362108507";
                string datajs = method.GetUrl(datajsurl, "gb2312");
                //insertdata(baseinfo,datajs,day);
               
            }


        }







    }
}
