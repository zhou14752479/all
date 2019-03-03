using Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class dataListDal
    {
        /// <summary>
        /// 将数据库读取的值一个一个对应的赋予类dataList的变量
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="row"></param>
        private static void LoadEntity(dataList dataList, DataRow row)
        {
            dataList.Id = Convert.ToInt32(row["id"]);
            dataList.UserName = row["user"] != DBNull.Value ? row["user"].ToString() : string.Empty;

            dataList.PassWord = row["password"] != DBNull.Value ? row["password"].ToString() : string.Empty;
           
            dataList.Mac = row["mac"].ToString();
            dataList.Ip = row["ip"].ToString();
        }


        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public static List<dataList> GetList()
        {
            string sql = "select * from vip_user";
            DataTable da = sqlHelper.GetDataTable(sql, CommandType.Text);
            List<dataList> list = null;
            if (da.Rows.Count > 0)
            {
                list = new List<dataList>();
                dataList datalist = null;
                foreach (DataRow row in da.Rows)
                {
                    datalist = new dataList();
                    LoadEntity(datalist, row);   //将数据库读取的值一个一个对应的赋予类dataList的变量
                    list.Add(datalist);
                }
            }
            return list;
        }

        /// <summary>
        /// 根据指定的范围，获取指定的数据
        /// </summary>
        /// <param name="start"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static List<dataList> GetPageList(int start, int num)
        {
            string sql = "select * from vip_user limit @start , @num";
            MySqlParameter[] pars = {
                                  new MySqlParameter("@start",MySqlDbType.Int32),
                                  new MySqlParameter("@num",MySqlDbType.Int32)

                                  };
            pars[0].Value = start;
            pars[1].Value = num;

            DataTable da = sqlHelper.GetDataTable(sql, CommandType.Text, pars);
            List<dataList> list = null;
            if (da.Rows.Count > 0)
            {
                list = new List<dataList>();
                dataList datalist = null;
                foreach (DataRow row in da.Rows)
                {
                    datalist = new dataList();
                    LoadEntity(datalist, row);
                    list.Add(datalist);
                }
            }
            return list;

        }

        /// <summary>
        /// 获取总的记录数
        /// </summary>
        /// <returns></returns>
        public static int GetRecordCount()
        {
            string sql = "select count(*) from vip_user";
            return Convert.ToInt32(sqlHelper.ExecuteScalar(sql, CommandType.Text));
        }


    }
}
