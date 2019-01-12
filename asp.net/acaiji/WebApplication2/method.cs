using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace WebApplication2
{
    public class method
    {
        #region 积分充值
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="point">当前积分</param>
        /// <param name="change">改动的积分</param>

        public static void addPoints(string username, int point, int change)
        {
           

                string constr = "Host =116.62.62.62;Database=vip;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                DateTime dt = DateTime.Now;
                string time = DateTime.Now.ToString();
                int points = point + change;

                MySqlCommand cmd = new MySqlCommand("UPDATE vip_points  SET points ='" + points + " ',points_t='" + time + " ' where username= '" + username + " '", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                 cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
            
           

        }
        #endregion

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {
            try
            {
                string COOKIE = "id58=c5/nn1qC89pTY5VqGmdcAg==; 58tj_uuid=c3d04c48-dd4d-49b3-816c-4633755f76cb; als=0; xxzl_deviceid=H3uwjdpchUrszAFFwEXWI5UaNJCSrPvfwBqXEZzrZl2NJTBICdPpn5Oz382zj99d; wmda_uuid=70c7fd7fd7b308b2e6a6206def584118; wmda_new_uuid=1; gr_user_id=7ec08bb8-eca4-45e4-9423-142f24f58994; Hm_lvt_d32bebe8de17afd6738ef3ad3ffa4be3=1518609564; UM_distinctid=161d0fb312c473-0f0b11be7fbcac-3b60490d-1fa400-161d0fb312d34b; wmda_visited_projects=%3B1731916484865%3B1409632296065%3B1732038237441%3B1732039838209%3B2385390625025; __utma=253535702.1777428119.1523013867.1523013867.1523013867.1; __utmz=253535702.1523013867.1.1.utmcsr=sh.58.com|utmccn=(referral)|utmcmd=referral|utmcct=/shangpuqg/0/; _ga=GA1.2.1777428119.1523013867; Hm_lvt_3bb04d7a4ca3846dcc66a99c3e861511=1526113480,1528285922; Hm_lvt_e15962162366a86a6229038443847be7=1526113481,1528285922; Hm_lvt_e2d6b2d0ec536275bb1e37b421085803=1526113565,1528285939; Hm_lvt_4d4cdf6bc3c5cb0d6306c928369fe42f=1530434006; final_history=33904487894826%2C33904487533008%2C34202955671375%2C34457504313538%2C34457504172715; mcity=sh; mcityName=%E4%B8%8A%E6%B5%B7; nearCity=%5B%7B%22cityName%22%3A%22%E5%8D%97%E4%BA%AC%22%2C%22city%22%3A%22nj%22%7D%2C%7B%22cityName%22%3A%22%E5%AE%BF%E8%BF%81%22%2C%22city%22%3A%22suqian%22%7D%2C%7B%22cityName%22%3A%22%E4%B8%8A%E6%B5%B7%22%2C%22city%22%3A%22sh%22%7D%5D; cookieuid1=mgjwFVtJtsaIVFa/CEaJAg==; Hm_lvt_5a7a7bfd6e7dfd9438b9023d5a6a4a96=1531557573; city=cq; 58home=cq; new_uv=41; utm_source=; spm=; init_refer=; f=n; new_session=0; qz_gdt=; _house_detail_show_time_=2; ppStore_fingerprint=FCCDFD5888AF9A96AB5621ECA8D12A45C2DA9181ABE0A5F0%EF%BC%BF1532790344974";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";

                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                ex.ToString();



            }
            return "";
        }
        #endregion

        #region 获取IP地区
        /// <summary>
        /// 获取IP地区
        /// </summary>
        /// <returns></returns>
        public static string GetIp()
        {
            try
            {

                string html = GetUrl("https://ip.cn/");

                MatchCollection match = Regex.Matches(html, @"<code>([\s\S]*?)</code>", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                return match[0].Groups[1].Value.Trim();

            }

            catch (Exception ex)
            {
                ex.ToString();

                return "获取IP错误";
            }

        }

        #endregion


        #region 获取Mac地址
        /// <summary>
        /// 获取Mac地址
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            try
            {
                string strMac = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        strMac = mo["MacAddress"].ToString();
                    }
                }
                moc = null;
                mc = null;
                return strMac;
            }
            catch
            {
                return "unknown";
            }
        }

        #endregion
        #region 读取积分
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username">用户名</param>


        public static int getPoints(string username)
        {

            string constr = "Host =116.62.62.62;Database=vip;Username=root;Password=zhoukaige";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();


            MySqlCommand cmd = new MySqlCommand("select * from vip_points where username = '"+ username+"'  ", mycon);         //SQL语句读取textbox的值'" + skinTextBox1.Text+"'
            MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源
            if (reader.Read())
            {
                string points = reader["points"].ToString().Trim();

                mycon.Close();
                reader.Close();

                return Convert.ToInt32(points);
            }
            else
            {
                reader.Close();
                MySqlCommand cmd2 = new MySqlCommand("select * from vip_points where phone = '"+username+"'  ", mycon);         //SQL语句读取textbox的值'" + skinTextBox1.Text+"'
                MySqlDataReader reader2 = cmd2.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                if (reader2.Read())
                {
                    string points = reader2["points"].ToString().Trim();

                    mycon.Close();
                    reader2.Close();
                    return Convert.ToInt32(points);
                }

                else
                {
                    return 0;
                }
                
            }


        }
        #endregion
    }
}