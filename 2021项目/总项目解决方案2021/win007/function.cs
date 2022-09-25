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
        public bool insertdata(string id, string matchname,  string zhu, string ke,string time,string gongsi,string bifen, string data1, string data2, string data3, string data4, string data5, string data6,string data7,string data8,string data9,string zhu_cj,string he_cj,string ke_cj,string zhu_yingkui,string he_yingkui,string ke_yingkui,string zhu_yingkuizs,string he_yingkuizs,string ke_yingkuizs,string rangqiu_daxiaoqiu)
        {
            try
            {
                string sql = "INSERT INTO datas(id,matchname,zhu,ke,time,gongsi,bifen,data1,data2,data3,data4,data5,data6,data7,data8,data9,zhu_cj,he_cj,ke_cj,zhu_yingkui,he_yingkui,ke_yingkui,zhu_yingkuizs,he_yingkuizs,ke_yingkuizs,rangqiu_daxiaoqiu)VALUES('" + id + "','" + matchname + "'," +
                    "'" + zhu + "'," +
                     "'" + ke + "'," +
                      "'" + time + "'," +
                       "'" +gongsi + "'," +
                      
                          "'" + bifen + "'," +
                           
                         "'" + data1 + "'," +
                          "'" + data2+ "'," +
                           "'" + data3 + "'," +
                            "'" + data4+ "'," +
                             "'" + data5+ "'," +
                              "'" + data6 + "'," +
                               "'" + data7 + "'," +
                                "'" + data8 + "'," +
                                 "'" + data9 + "'," +
                                  "'" + zhu_cj + "'," +
                                   "'" + he_cj + "'," +
                                    "'" + ke_cj + "'," +
                                    "'" + zhu_yingkui + "'," +
                                     "'" + he_yingkui + "'," +
                                      "'" + ke_yingkui + "'," +
                                       "'" + zhu_yingkuizs + "'," +
                                        "'" + he_yingkuizs + "'," +
                                 "'" + ke_yingkuizs + "'," +
                                
                    "'" + rangqiu_daxiaoqiu+ "')";

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





        #region 获取当前实时比赛赔率
        public static string getshishidata(string id,string com)
        {

           Dictionary<string, string> gongsi_dics = new Dictionary<string, string>();

        string datajsurl = "http://1x2d.titan007.com/" + id + ".js?r=007132848760362108507";
            string datajs = method.GetUrl(datajsurl, "gb2312");




            //动态添加公司ID
            MatchCollection gongsis = Regex.Matches(datajs, @"\d{1,5}\|\d{8,10}\|([\s\S]*?)\|");
            for (int a = 0; a < gongsis.Count; a++)
            {
                string cid = Regex.Match(gongsis[a].ToString(), @"\d{8,10}").Groups[0].Value;
                string cname = gongsis[a].Groups[1].ToString();

                if (!gongsi_dics.ContainsKey(cname))
                {
                    switch (cname)
                    {
                        case "SNAI":
                            gongsi_dics.Add("SNAI",cid);
                            break;
                        case "Titanbet":
                            gongsi_dics.Add("Titanbet", cid);
                            break;
                        case "Bethard":
                            gongsi_dics.Add( "Bethard", cid);
                            break;
                        case "ComeOn":
                            gongsi_dics.Add("ComeOn", cid);
                            break;
                        case "Intertops":
                            gongsi_dics.Add("Intertops", cid);
                            break;
                        case "Bet3000":
                            gongsi_dics.Add("Bet3000", cid);
                            break;
                        case "Crown":
                            gongsi_dics.Add( "Crown", cid);
                            break;
                        case "William Hill":
                            gongsi_dics.Add("William Hill", cid);
                            break;
                        case "Bet-at-home":
                            gongsi_dics.Add("Bet-at-home", cid);
                            break;
                        case "Lottery Official":
                            gongsi_dics.Add("Lottery Official", cid);
                            break;
                        case "Vcbet":
                            gongsi_dics.Add("Vcbet", cid);
                            break;

                            //bet瑞典
                        case "Betsson":
                            gongsi_dics.Add("Betsson", cid);
                            break;
                        case "TopSport":
                            gongsi_dics.Add("TopSport", cid);
                            break;


                    }
                }

            }

            //动态添加公司ID结束








            string datas = Regex.Match(datajs, @"gameDetail=Array\(([\s\S]*?)\)").Groups[1].Value;
           
            string[] datastext = datas.Split(new string[] { "\",\"" }, StringSplitOptions.None);

            StringBuilder sb=new StringBuilder();   

            for (int j = 0; j < datastext.Length; j++)
            {

                string cid = Regex.Match(datastext[j], @"\d{8,10}").Groups[0].Value.Trim();
                string[] datasresult = datastext[j].Split(new string[] { ";" }, StringSplitOptions.None);
               
                string data1 = "";
                string data2 = "";
                string data3 = "";
                string data4 = "";
                string data5 = "";
                string data6 = "";
                string data7 = "";
                string data8 = "";
                string data9 = "";
                try
                {

                    if (cid == gongsi_dics[com])
                    {
                      
                        string[] data_a = datasresult[0].Split(new string[] { "|" }, StringSplitOptions.None);
                        data1 = data_a[0].Replace(cid, "").Replace("^", "");
                        data2 = data_a[1];
                        data3 = data_a[2];


                        string[] data_b = datasresult[1].Split(new string[] { "|" }, StringSplitOptions.None);
                        data4 = data_b[0];
                        data5 = data_b[1];
                        data6 = data_b[2];


                        string[] data_c = datasresult[2].Split(new string[] { "|" }, StringSplitOptions.None);
                        data7 = data_c[0];
                        data8 = data_c[1];
                        data9 = data_c[2];

                        sb.Append(cid+","+data1 + "," + data2 + "," + data3 + "," + data4 + "," + data5 + "," + data6);
                    }
                }
                catch (Exception ex)
                {
                    
                }


            }

            return sb.ToString();
        }


        #endregion


       
        


    }
}
