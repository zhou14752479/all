using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace WebApplication3
{
    public class method
    {
        #region 获取Mac地址

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
        #region 注册码随机生成函数
        public static string Random()

        {
            //string[] array = {"A","B","C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
            string Hour = DateTime.Now.Hour.ToString();                    //获取当前小时
            string Hour2 = Math.Pow(Convert.ToDouble(Hour), 2).ToString();  //获取当前小时的平方
            //string zimu = array[DateTime.Now.Hour];                        //获取当前小时作为数组索引的字母
            string key = Hour + "1475" + (Hour2) + "2479" + Hour;

            return key;



        }


        #endregion

        #region GET请求
        public static string GetUrl(string Url)
        {
            try
            {
                string COOKIE = "_lxsdk_cuid=1609b86916cc8-08cd9a34568e29-36624209-1fa400-1609b86916dc8; _ga=GA1.2.1236833492.1515143511; _lx_utm=utm_source%3DBaidu%26utm_medium%3Dorganic; iuuid=528F52FB7FFA9E4668A9276A34739DE509B4A411CC08F2612EC2A51E0A8C1963; webp=1; latlng=33.96193,118.27549,1515660280105; cityname=%E5%8C%97%E4%BA%AC; i_extend=C_b2GimthomepagesearchH__a100001__b1; __utma=74597006.1236833492.1515143511.1515660278.1515660278.1; __utmz=74597006.1515660278.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); Hm_lvt_cc903faaed69cca18f7cf0997b2e62c9=1515659805; uuid=b88bdc17d9c849dfbdf2.1514437775.1.0.0; oc=iy-AAAVEh0gZfS-yGtUwEbVlM5pMtV-7Euvq2JQK04C5q_4NVbXpcz6xIrPM5JkvKyiGyKTXq7V95G6J9XLtCpBqcuNEGco58QajBfygzio_7FN9neSawQJ3GyM9kmuTRSS4T4Xjjvm08E8yETxxT0pXBz0rnJV93MVhmhAjHW0; ci=1; rvct=1%2C334%2C20%2C55%2C59%2C45%2C50%2C30%2C56; lat=39.924772; lng=116.600145; __mta=109370042.1514437776048.1515723458022.1515724799303.29; _lxsdk_s=160e7fb3fa7-b85-6d2-38%7C%7C60";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.112 Safari/537.36";

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



       
        private static readonly string conn = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;


        public static DataTable GetDataTable(string sql, CommandType type, params MySqlParameter[] pars)
        {
            using (MySqlConnection conn1 = new MySqlConnection(conn))
            {
                using (MySqlDataAdapter apter = new MySqlDataAdapter(sql, conn1))
                {
                    if (pars != null)
                    {
                        apter.SelectCommand.Parameters.AddRange(pars);
                    }
                    apter.SelectCommand.CommandType = type;
                    DataTable da = new DataTable();
                    apter.Fill(da);
                    return da;
                }
            }
        }


        public static object ExecuteScalar(string sql, CommandType type, params MySqlParameter[] pars)
        {
            using (MySqlConnection conn1 = new MySqlConnection(conn))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn1))
                {
                    if (pars != null)
                    {
                        cmd.Parameters.AddRange(pars);
                    }
                    cmd.CommandType = type;
                    conn1.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

    }
}