using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 竞彩数据查询
{
    class function
    {
        //string path = System.Environment.CurrentDirectory + "\\jingcaidata.db"; //获取当前程序运行文件夹

        string path = System.Environment.CurrentDirectory + "\\lanqiudata.db"; //获取当前程序运行文件夹
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
        public ArrayList getsupplyers(string sql)
        {
            try
            {
                ArrayList lists = new ArrayList();
                //string sql = "select gongsi from datas";
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
        public bool insertdata(string date,string match,string zhu,string ke,string bifen,string sheng,string ping,string fu,string type,string hhl,string updatetime,string result)
        {
            try
            {
                string sql = "INSERT INTO datas(date,match,zhu,ke,bifen,sheng,ping,fu,type,hhl,updatetime,result)VALUES('" + date + "','" + match+ "'," +
                    "'" + zhu + "'," +
                     "'" + ke + "'," +
                      "'" + bifen+ "'," +
                      
                         "'" + sheng + "'," +
                           "'" + ping + "'," +
                             "'" + fu + "'," +
                              "'" + type + "'," +
                               "'" + hhl + "'," +
                                "'" + updatetime + "'," +
                    "'" + result+ "')";

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
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return false;
            }

        }

        /// <summary>
        /// 插入数据库
        /// </summary>
        public bool insertdata_lanqiu(string xuhao,string time,string liansai,string zhu,string ke,string zhu_fen, string ke_fen,string ke_sheng,string ke_daxiao,string ke_rang,string zhu_sheng,string zhu_daxiao,string zhu_rang_fen,string zhu_rang_peilv,string date)
        {
            try
            {
                string sql = "INSERT INTO datas(xuhao,time,liansai,zhu,ke,zhu_fen,ke_fen,ke_sheng,ke_daxiao,ke_rang,zhu_sheng,zhu_daxiao,zhu_rang_fen,zhu_rang_peilv,date)VALUES('" + xuhao + "','" + time + "'," +
                    "'" + liansai + "'," +
                     "'" + zhu + "'," +
                      "'" + ke+ "'," +
                         "'" + zhu_fen + "'," +
                           "'" + ke_fen + "'," +
                             "'" + ke_sheng+ "'," +
                              "'" + ke_daxiao + "'," +
                               "'" + ke_rang + "'," +
                                "'" + zhu_sheng + "'," +
                                 "'" + zhu_daxiao + "'," +
                                  "'" + zhu_rang_fen + "'," +
                                  "'" + zhu_rang_peilv + "'," +
                    "'" + date+ "')";

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
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
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
    }
}
