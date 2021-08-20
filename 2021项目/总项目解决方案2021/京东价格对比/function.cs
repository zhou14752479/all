using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 京东价格对比
{
    class function
    {

        /// <summary>
        /// 查询数据库
        /// </summary>
        public DataTable chaxundata(string sql)
        {
            try
            {
                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\jddata.db");
                mycon.Open();

                SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(sql, mycon);
                DataTable dt = new DataTable();
                mAdapter.Fill(dt);
                mycon.Close();
                return dt;

            }
            catch (SQLiteException ex)
            {
               
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
                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                using (SQLiteConnection con = new SQLiteConnection("Data Source=" + path + "\\jddata.db"))
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
               
                return null;


            }

        }

        /// <summary>
        /// 插入数据库
        /// </summary>
        public bool insertdata(string sql)
        {
            try
            {

                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\jddata.db");
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
            catch (SQLiteException ex)
            {
                return false;

            }

        }

    }
}
