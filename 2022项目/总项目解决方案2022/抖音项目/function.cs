using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace 抖音项目
{
    class function
    {

        #region  Unicode转字符串
        public static string Unicode2String(string source)
        {
            return new Regex("\\\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(source, (Match x) => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)).ToString());
        }
        #endregion

        #region  时间戳转时间
        public static DateTime ConvertStringToDateTime(string timeStamp)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(Convert.ToDouble(timeStamp));
        }
        #endregion


        static string constr = "Host =localhost;Database=douyin_cms_com_person;Username=root;Password=root";
        #region  插入

        public static string adddata(string aweme_url, string photo_url, string duration, string author_nickname, string comment_count, string share_count, string collect_count, string digg_count, string create_time, string descs, string created_at, string updated_at, string whoslike, string author_sec_uid, string author_douyin_id, int dz_speed, string create_date_time, string unique_sign, string play_addr1)
        {

           
            try
            {
               
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();
                string sql= @"INSERT INTO zuopin(aweme_url, photo_url, duration, author_nickname, comment_count, share_count, collect_count, digg_count, create_time, descs, created_at, updated_at, whoslike, author_sec_uid, author_douyin_id, dz_speed, create_date_time, unique_sign, play_addr1)VALUES('" + aweme_url.Trim() +"', '" + photo_url.Trim() +"', '" + duration + "', '" + author_nickname + "', '" + comment_count + " ','" + share_count + "', '" + collect_count + "', '" + digg_count + "', '" + create_time + "', '" + descs + "', '" + created_at + "', '" + updated_at + "', '" + whoslike + " ', '" + author_sec_uid + "', '" + author_douyin_id + "', '" + dz_speed + "', '" + create_date_time + "', '" + unique_sign + "', '" + play_addr1 + "')";
                MySqlCommand cmd = new MySqlCommand(sql, mycon);

               

                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
               
                if (count > 0)
                {
                    mycon.Close();
                    return " --数据存储数据库成功成功";
                }
                else
                {
                    mycon.Close();
                    return " --数据存储数据库成功失败1";

                }
            }

            catch (System.Exception ex)
            {
                return " --数据存储数据库成功失败2";
                return ex.ToString();
            }

            #endregion

        }



        #region 获取数据库美团城市名称返回集合
        public static ArrayList getusers()
        {
            ArrayList list = new ArrayList();
            try
            {
              
                string str = "SELECT douyin_sec from fsh";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString().Trim());
                }

                
            }
            catch (MySqlException ee)
            {
               
                // return ex.ToString();
            }
            return list;

        }
        #endregion


    }
}
