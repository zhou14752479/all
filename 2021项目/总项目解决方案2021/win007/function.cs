using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using myDLL;
using NPOI.SS.Formula.Functions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace win007
{
    class function
    {
        string path = System.Environment.CurrentDirectory + "\\win007data_new.db"; //获取当前程序运行文件夹
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
        public bool insertdata(string id, string matchname, string zhu, string ke, string time, string gongsi, string bifen, string data1, string data2, string data3, string data4, string data5, string data6, string data7, string data8, string data9, string data10, string data11, string data12, string zhu_cj, string he_cj, string ke_cj, string zhu_yingkui, string he_yingkui, string ke_yingkui, string zhu_yingkuizs, string he_yingkuizs, string ke_yingkuizs, string rangqiu_daxiaoqiu)
        {
            try
            {
                string sql = "INSERT INTO datas(id,matchname,zhu,ke,time,gongsi,bifen,data1,data2,data3,data4,data5,data6,data7,data8,data9,data10,data11,data12,zhu_cj,he_cj,ke_cj,zhu_yingkui,he_yingkui,ke_yingkui,zhu_yingkuizs,he_yingkuizs,ke_yingkuizs,rangqiudaxiaoqiu)VALUES('" + id + "','" + matchname + "'," +
                    "'" + zhu + "'," +
                     "'" + ke + "'," +
                      "'" + time + "'," +
                       "'" + gongsi + "'," +

                          "'" + bifen + "'," +

                         "'" + data1 + "'," +
                          "'" + data2 + "'," +
                           "'" + data3 + "'," +
                            "'" + data4 + "'," +
                             "'" + data5 + "'," +
                              "'" + data6 + "'," +
                               "'" + data7 + "'," +
                                "'" + data8 + "'," +
                                 "'" + data9 + "'," +
                                  "'" + data10 + "'," +
                                   "'" + data11 + "'," +
                                    "'" + data12 + "'," +
                                  "'" + zhu_cj + "'," +
                                   "'" + he_cj + "'," +
                                    "'" + ke_cj + "'," +
                                    "'" + zhu_yingkui + "'," +
                                     "'" + he_yingkui + "'," +
                                      "'" + ke_yingkui + "'," +
                                       "'" + zhu_yingkuizs + "'," +
                                        "'" + he_yingkuizs + "'," +
                                 "'" + ke_yingkuizs + "'," +

                    "'" + rangqiu_daxiaoqiu + "')";

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

                //MessageBox.Show(ex.ToString());
                return false;
            }

        }

        /// <summary>
        /// 插入数据库
        /// </summary>
        public bool insertdata_yarang(string id, string data1, string data2, string data3, string data4, string data5, string data6)
        {
            try
            {
                string sql = "INSERT INTO data_yarang(id,data1,data2,data3,data4,data5,data6)VALUES('" + id + "','" + data1 + "'," +
                    "'" + data2 + "'," +
                     "'" + data3 + "'," +
                      "'" + data4 + "'," +
                       "'" + data5 + "'," +

                    "'" + data6 + "')";

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

                //MessageBox.Show(ex.ToString());
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
                string datajsurl = "http://1x2d.win007.com/" + ids[i].Groups[1].Value + ".js?r=007132848760362108507";
                string datajs = method.GetUrl(datajsurl, "gb2312");
                //insertdata(baseinfo,datajs,day);

            }


        }


        static Dictionary<string,string>  company_data_dic=new Dictionary<string,string>();

         static Dictionary<string, string> sortedDict = new Dictionary<string, string>();
        #region 获取当前实时比赛赔率_array
        public static string getshishidata_array(string matchid, System.Windows.Forms.ComboBox comb1)
        {

            sortedDict.Clear();
            Dictionary<string, string> dict = new Dictionary<string, string>();

            string datajsurl = "http://1x2d.titan007.com/" + matchid + ".js?r=007132848760362108507";
            string datajs = method.GetUrl(datajsurl, "gb2312");






            string datas = Regex.Match(datajs, @"game=Array\(([\s\S]*?)\);").Groups[1].Value;

            string[] datastext = datas.Split(new string[] { "\",\"" }, StringSplitOptions.None);


            StringBuilder sb = new StringBuilder();


            for (int j = 0; j < datastext.Length; j++)
            {

                string cid = Regex.Match(datastext[j], @"\d{8,10}").Groups[0].Value.Trim();
                string[] datasresult = datastext[j].Split(new string[] { "|" }, StringSplitOptions.None);


                if (datasresult.Length > 2)
                {
                    string company = datasresult[2];


                    string timeStr = datasresult[datasresult.Length - 4].Replace("-1", "");

                    string format = "yyyy,MM,dd,HH,mm,ss";

                    if (DateTime.TryParseExact(timeStr, format, null, System.Globalization.DateTimeStyles.None, out DateTime dt))
                    {
                        // 增加8小时
                        dt = dt.AddHours(8);

                        // 输出结果（保持相同格式）
                        string newTimeStr = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (comb1.Items.Contains(company))
                        {
                            dict.Add(company, newTimeStr);
                        }

                    }


                }

            }

             sortedDict = dict.OrderByDescending(pair => pair.Value)
                             .ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (string key in sortedDict.Keys)
            {
                sb.AppendLine(sortedDict[key] + " " + key);
            }

            return sb.ToString();
        }


        #endregion


        public static string paixuStr;

       public static Dictionary<string, string> paixudict = new Dictionary<string, string>();

        #region 获取当前实时比赛赔率（一次一家公司，三行数据）
        public static string getshishidata(string matchid, string company)
        {

            Dictionary<string, string> gongsi_dics = new Dictionary<string, string>();

            string datajsurl = "http://1x2d.titan007.com/" + matchid + ".js?r=007132848760362108507";
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
                            gongsi_dics.Add("SNAI", cid);
                            break;
                        case "Titanbet":
                            gongsi_dics.Add("Titanbet", cid);
                            break;
                        case "Bethard":
                            gongsi_dics.Add("Bethard", cid);
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
                            gongsi_dics.Add("Crown", cid);
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

                        case "Dafabet":
                            gongsi_dics.Add("Dafabet", cid);
                            break;





                        case "Betfair Exchange":
                            gongsi_dics.Add("Betfair Exchange", cid);
                            break;
                        case "Betfair":
                            gongsi_dics.Add("Betfair", cid);
                            break;
                        case "BetClic.fr":
                            gongsi_dics.Add("BetClic.fr", cid);
                            break;


                    }
                }

            }

            //动态添加公司ID结束








            string datas = Regex.Match(datajs, @"gameDetail=Array\(([\s\S]*?)\)").Groups[1].Value;

            string[] datastext = datas.Split(new string[] { "\",\"" }, StringSplitOptions.None);

            StringBuilder sb = new StringBuilder();

            for (int j = 0; j < datastext.Length; j++)
            {

                string cid = Regex.Match(datastext[j], @"\d{8,10}").Groups[0].Value.Trim();
                string[] datasresult = datastext[j].Split(new string[] { ";" }, StringSplitOptions.None);

                string data1 = "0";
                string data2 = "0";
                string data3 = "0";
                string data4 = "0";
                string data5 = "0";
                string data6 = "0";
                string data7 = "0";
                string data8 = "0";
                string data9 = "0";
                string time1 = "";
                try
                {

                    if (cid == gongsi_dics[company])
                    {

                        string[] data_a = datasresult[0].Split(new string[] { "|" }, StringSplitOptions.None);
                        if (data_a.Length > 2)
                        {
                            data1 = data_a[0].Replace(cid, "").Replace("^", "");
                            data2 = data_a[1];
                            data3 = data_a[2];
                            time1= data_a[3];   
                        }

                        string[] data_b = datasresult[1].Split(new string[] { "|" }, StringSplitOptions.None);
                        if (data_b.Length > 2)
                        {
                            data4 = data_b[0];
                            data5 = data_b[1];
                            data6 = data_b[2];
                        }


                        string[] data_c = datasresult[2].Split(new string[] { "|" }, StringSplitOptions.None);
                        if (data_c.Length > 2)
                        {
                            data7 = data_c[0];
                            data8 = data_c[1];
                            data9 = data_c[2];
                        }

                        sb.Append(cid + "," + data1 + "," + data2 + "," + data3 + "," + data4 + "," + data5 + "," + data6 + "," + data7 + "," + data8 + "," + data9 + "," +time1);
                    }

                  
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                }


            }
           
            return sb.ToString();
        }


        #endregion

        #region 获取当前实时比赛赔率（多个公司，四行数据）
        public static string getshishidata_4(string matchid, System.Windows.Forms.ComboBox comb1)
        {
            company_data_dic.Clear();
            paixudict.Clear();
            Dictionary<string, string> gongsi_dics = new Dictionary<string, string>();

            string datajsurl = "http://1x2d.titan007.com/" + matchid + ".js?r=007132848760362108507";
            string datajs = method.GetUrl(datajsurl, "gb2312");


            //获取比赛时间
            string timeStr = Regex.Match(datajs, @"MatchTime=""([\s\S]*?)""").Groups[1].Value.Replace("-1", ""); ;
            string format = "yyyy,MM,dd,HH,mm,ss";

           
            if (DateTime.TryParseExact(timeStr, format, null, System.Globalization.DateTimeStyles.None, out DateTime dt))
            {
                // 增加8小时
                dt = dt.AddHours(8);

                // 输出结果（保持相同格式）
                matchtime = dt.ToString("yyyy-MM-dd HH:mm:ss");
               

            }






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
                            gongsi_dics.Add("SNAI", cid);
                            break;
                        case "Titanbet":
                            gongsi_dics.Add("Titanbet", cid);
                            break;
                        case "Bethard":
                            gongsi_dics.Add("Bethard", cid);
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
                            gongsi_dics.Add("Crown", cid);
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

                        case "Dafabet":
                            gongsi_dics.Add("Dafabet", cid);
                            break;





                        case "Betfair Exchange":
                            gongsi_dics.Add("Betfair Exchange", cid);
                            break;
                        case "Betfair":
                            gongsi_dics.Add("Betfair", cid);
                            break;
                        case "BetClic.fr":
                            gongsi_dics.Add("BetClic.fr", cid);
                            break;


                    }
                }

            }

            //动态添加公司ID结束



            

            string datas = Regex.Match(datajs, @"gameDetail=Array\(([\s\S]*?)\)").Groups[1].Value;

            string[] datastext = datas.Split(new string[] { "\",\"" }, StringSplitOptions.None);

            StringBuilder sb = new StringBuilder();

            for (int j = 0; j < datastext.Length; j++)
            {

                string cid = Regex.Match(datastext[j], @"\d{8,10}").Groups[0].Value.Trim();
                string[] datasresult = datastext[j].Split(new string[] { ";" }, StringSplitOptions.None);

                string data1 = "0";
                string data2 = "0";
                string data3 = "0";

                string data4 = "0";
                string data5 = "0";
                string data6 = "0";

                string data7 = "0";
                string data8 = "0";
                string data9 = "0";

                string data10 = "0";
                string data11 = "0";
                string data12 = "0";

                string time1 = "";
                try
                {

                   
                    for (int i = 0; i < comb1.Items.Count; i++)
                    {

                        string company=comb1.Items[i].ToString();
                        
                        if(!gongsi_dics.ContainsKey(company))
                        {
                            continue;
                        }
                        if (cid == gongsi_dics[company])
                        {

                            if (datasresult.Length > 0)
                            {

                                string[] data_a = datasresult[0].Split(new string[] { "|" }, StringSplitOptions.None);
                                if (data_a.Length > 2)
                                {
                                    data1 = data_a[0].Replace(cid, "").Replace("^", "").Replace("\"", "");
                                    data2 = data_a[1];
                                    data3 = data_a[2];
                                    time1 = data_a[3];

                                   
                                    paixudict.Add(company, time1);


                                }
                            }
                            if (datasresult.Length > 1)
                            {
                                string[] data_b = datasresult[1].Split(new string[] { "|" }, StringSplitOptions.None);
                                if (data_b.Length > 2)
                                {
                                    data4 = data_b[0];
                                    data5 = data_b[1];
                                    data6 = data_b[2];
                                }
                            }

                            if (datasresult.Length > 2)
                            {
                                string[] data_c = datasresult[2].Split(new string[] { "|" }, StringSplitOptions.None);
                                if (data_c.Length > 2)
                                {
                                    data7 = data_c[0];
                                    data8 = data_c[1];
                                    data9 = data_c[2];
                                }
                            }
                            if (datasresult.Length > 3)
                            {
                                string[] data_d = datasresult[3].Split(new string[] { "|" }, StringSplitOptions.None);
                                if (data_d.Length > 2)
                                {
                                    data10 = data_d[0];
                                    data11 = data_d[1];
                                    data12 = data_d[2];
                                }
                            }

                            string data = company + "," + data1 + "," + data2 + "," + data3 + "," + data4 + "," + data5 + "," + data6 + "," + data7 + "," + data8 + "," + data9 + "," + data10 + "," + data11 + "," + data12;
                            sb.AppendLine(data);
                            
                            
                            if (!company_data_dic.ContainsKey(company))
                            {
                                
                                company_data_dic.Add(company, data);
                            }
                           

                        }

                    }



                    
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

               
            }
            var sortedDict = paixudict.OrderByDescending(pair => pair.Value)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);

            StringBuilder sb2 = new StringBuilder();

            foreach (string key in sortedDict.Keys)
            {

                sb2.AppendLine(sortedDict[key] + " " + key);
            }

            paixuStr = sb2.ToString();

            return sb.ToString();
        }


        #endregion


        


        #region 获取当前实时凯丽数据
        public static string getshishi_kailidata(string matchid, string com)
        {

            Dictionary<string, string> gongsi_dics = new Dictionary<string, string>();

            string datajsurl = "http://1x2d.titan007.com/" + matchid + ".js?r=007132848760362108507";
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
                            gongsi_dics.Add("SNAI", cid);
                            break;
                        case "Titanbet":
                            gongsi_dics.Add("Titanbet", cid);
                            break;
                        case "Bethard":
                            gongsi_dics.Add("Bethard", cid);
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
                            gongsi_dics.Add("Crown", cid);
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

                        case "Dafabet":
                            gongsi_dics.Add("Dafabet", cid);
                            break;





                        case "Betfair Exchange":
                            gongsi_dics.Add("Betfair Exchange", cid);
                            break;
                        case "Betfair":
                            gongsi_dics.Add("Betfair", cid);
                            break;
                        case "BetClic.fr":
                            gongsi_dics.Add("BetClic.fr", cid);
                            break;


                    }
                }

            }

            //动态添加公司ID结束








            string datas = Regex.Match(datajs, @"gameDetail=Array\(([\s\S]*?)\)").Groups[1].Value;

            string[] datastext = datas.Split(new string[] { "\",\"" }, StringSplitOptions.None);

            StringBuilder sb = new StringBuilder();

            for (int j = 0; j < datastext.Length; j++)
            {

                string cid = Regex.Match(datastext[j], @"\d{8,10}").Groups[0].Value.Trim();
                string[] datasresult = datastext[j].Split(new string[] { ";" }, StringSplitOptions.None);



                string kaili1 = "";
                string kaili2 = "";
                string kaili3 = "";
                string kaili4 = "";
                string kaili5 = "";
                string kaili6 = "";
                string kaili7 = "";
                string kaili8 = "";
                string kaili9 = "";

                try
                {

                    if (cid == gongsi_dics[com])
                    {

                        string[] data_a = datasresult[0].Split(new string[] { "|" }, StringSplitOptions.None);


                        //第一行凯丽指数
                        kaili1 = data_a[4];
                        kaili2 = data_a[5];
                        kaili3 = data_a[6];


                        string[] data_b = datasresult[1].Split(new string[] { "|" }, StringSplitOptions.None);

                        if (data_b.Length > 4)
                        {
                            //第二行凯丽
                            kaili4 = data_b[4];
                            kaili5 = data_b[5];
                            kaili6 = data_b[6];
                        }

                        string[] data_c = datasresult[2].Split(new string[] { "|" }, StringSplitOptions.None);
                        if (data_c.Length > 4)
                        {
                            //第三行凯丽
                            kaili7 = data_c[4];
                            kaili8 = data_c[5];
                            kaili9 = data_c[6];

                        }
                        sb.Append(kaili1 + "," + kaili2 + "," + kaili3 + "," + kaili4 + "," + kaili5 + "," + kaili6 + "," + kaili7 + "," + kaili8 + "," + kaili9);
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                }


            }

            return sb.ToString();
        }


        #endregion

        public static string matchtime = "";
        /// <summary>
        /// 特殊数值检测  三行运算
        /// </summary>
        public static string teshujiance(string id, System.Windows.Forms.ComboBox comb1)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            string data = function.getshishidata_4(id, comb1);

            

            string hang1 = "";
            string hang2 = "";
            string hang3 = "";
            string hang4 = "";
            string[] texts = data.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < texts.Length; i++)
            {
              
                if (texts[i].Trim()!="")
                {
                   
                    string[] text = texts[i].Split(new string[] { "," }, StringSplitOptions.None);

                    string company=text[0];

                    //string aaa = "";
                    //for (int a= 0; a< text.Length; a++)
                    //{
                    //    aaa = aaa + text[a].Trim() + "  ";
                    //}
                    //MessageBox.Show(company+aaa);

                    if (text.Length > 8)
                    {
                        //特殊数值检测
                        double zhu1=Convert.ToDouble(text[1]);
                        double he1 = Convert.ToDouble(text[2]);
                        double ke1 = Convert.ToDouble(text[3]);

                        double zhu2 = Convert.ToDouble(text[4]);
                        double he2 = Convert.ToDouble(text[5]);
                        double ke2 = Convert.ToDouble(text[6]);

                        if (he1==zhu1 || he1==ke1 || ke2==zhu2 || he2==ke2)
                        {
                            sb.Append(company+"   |     ");

                        }
                        


                        //三行计算
                        //第一行
                        double s11 = Convert.ToDouble(text[4]) - Convert.ToDouble(text[1]);
                        string v11= s11 > 0 ? "↓" : "↑";

                        double s12 = Convert.ToDouble(text[5]) - Convert.ToDouble(text[2]);
                        string v12 = s12 > 0 ? "↓" : "↑";

                        double s13 = Convert.ToDouble(text[6]) - Convert.ToDouble(text[3]);
                        string v13 = s13 > 0 ? "↓" : "↑";



                        double s21= Convert.ToDouble(text[7]) - Convert.ToDouble(text[4]);
                        string v21 = s21 > 0 ? "↓" : "↑";
                        double s22 = Convert.ToDouble(text[8]) - Convert.ToDouble(text[5]);
                        string v22 = s22 > 0 ? "↓" : "↑";
                        double s23 = Convert.ToDouble(text[9]) - Convert.ToDouble(text[6]);
                        string v23 = s23 > 0 ? "↓" : "↑";


                        double s31 = Convert.ToDouble(text[10]) - Convert.ToDouble(text[7]);
                        string v31 = s31 > 0 ? "↓" : "↑";
                        double s32 = Convert.ToDouble(text[11]) - Convert.ToDouble(text[8]);
                        string v32 = s32 > 0 ? "↓" : "↑";
                        double s33 = Convert.ToDouble(text[12]) - Convert.ToDouble(text[9]);
                        string v33 = s33 > 0 ? "↓" : "↑";


                        //string comdata = company+"\r\n"+s11.ToString("F2") + v11 + "  " + s12.ToString("F2") + v12 + "  " + s13.ToString("F2") + v13 + "\r\n" + s21.ToString("F2") + v21 + "  " + s22.ToString("F2") + v22 + "  " + s23.ToString("F2") + v23 + "\r\n" + s31.ToString("F2") + v31 + "  " + s32.ToString("F2") + v32 + "  " + s33.ToString("F2") + v33+"\r\n";

                        hang1 += "       " + company+"       ";
                        hang2 += s11.ToString("F2").Replace("-", "") + v11 + "," + s12.ToString("F2").Replace("-", "") + v12 + "," + s13.ToString("F2").Replace("-", "") + v13+"    ";    //去掉负号
                        hang3 += s21.ToString("F2").Replace("-", "") + v21 + "," + s22.ToString("F2").Replace("-", "") + v22 + "," + s23.ToString("F2").Replace("-", "") + v23 + "    ";//去掉负号
                        hang4 += s31.ToString("F2").Replace("-", "") + v31 + "," + s32.ToString("F2").Replace("-", "") + v32 + "," + s33.ToString("F2").Replace("-", "") + v33 + "    ";//去掉负号



                    }
                }


              


            }

            sb2.AppendLine(hang1);
            sb2.AppendLine(hang2);
            sb2.AppendLine(hang3);
            sb2.AppendLine(hang4);
            return sb.ToString()+"#"+sb2.ToString();

        }

        public static double FindDifference(double num1, double num2, double num3, double num4, double num5,double num6)
        {

            double[] numbers = { num1, num2, num3, num4, num5 ,num6};


            //为0代表没有值，不参与计算
            numbers = numbers.Where(num => num != 0).ToArray();

            return numbers.Max() - numbers.Min();
        }

        /// <summary>
        /// 获取三个值的最大值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string Find3Max(string value)
        {
            string[] text = value.Trim().Split(new string[] { " " }, StringSplitOptions.None);

            double a = Convert.ToDouble(text[0]);
            double b = Convert.ToDouble(text[1]);
            double c = Convert.ToDouble(text[2]);
            StringBuilder sb = new StringBuilder();

            double max = Math.Max(a, Math.Max(b, c));

            if (a == max || a >= 0.4)
            {
                sb.Append("1");
            }
            if (b == max || b >= 0.4)
            {
                sb.Append("2");
            }
            if (c == max || c >= 0.4)
            {
                sb.Append("3");
            }

            if (max < 0.4)
            {
                sb.Clear();
            }
            return sb.ToString();
        }


        //凯丽三个数值排序

        public static int CompareAndLabel(double a, double b, double c)
        {
            // 判断三个值是否全部相等
            if (a == b && b == c)
                return 333;
            if (a == b && a > c)
                return 332;
            if (b == c && b > a)
                return 233;
            if (a == c && a > b)
                return 323;
            if (a == c && a < b)
                return 232;

            // 确定最大值、中间值、最小值的索引，处理所有可能的相等情况
            int[] labels = new int[3];

            // 判断 a 的类型（最大、中间、最小）
            if (a >= b && a >= c)
            {
                labels[0] = 3;
                // 处理 b 和 c
                if (b >= c)
                {
                    labels[1] = (b == c) ? 2 : 2;
                    labels[2] = (b == c) ? 2 : 1;
                }
                else
                {
                    labels[1] = (c == b) ? 2 : 1;
                    labels[2] = (c == b) ? 2 : 2;
                }
            }
            else if (b >= a && b >= c)
            {
                labels[1] = 3; // b 是最大值，位置在第二位
                               // 处理 a 和 c
                if (a >= c)
                {
                    labels[0] = (a == c) ? 2 : 2;
                    labels[2] = (a == c) ? 2 : 1;
                }
                else
                {
                    labels[0] = (c == a) ? 2 : 1;
                    labels[2] = (c == a) ? 2 : 2;
                }
            }
            else
            {
                labels[2] = 3; // c 是最大值，位置在第三位
                               // 处理 a 和 b
                if (a >= b)
                {
                    labels[0] = (a == b) ? 2 : 2;
                    labels[1] = (a == b) ? 2 : 1;
                }
                else
                {
                    labels[0] = (b == a) ? 2 : 1;
                    labels[1] = (b == a) ? 2 : 2;
                }
            }

            // 构建结果，确保标签正确反映原始输入顺序
            return labels[0] * 100 + labels[1] * 10 + labels[2];
        }



        /// <summary>
        /// 母大差
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string chazhi_mu(string id)
        {

            // "Betfair", "Betfair Exchange", "SNAI", "Betsson", "Dafabet"

            string data1 = "", data2 = "", data3= "", data4 = "", data5 = "";

           
            if(company_data_dic.ContainsKey("Betfair"))
            {
                data1 = company_data_dic["Betfair"];
            }
            if (company_data_dic.ContainsKey("Betfair Exchange"))
            {
                data2 = company_data_dic["Betfair Exchange"];
            }
            if (company_data_dic.ContainsKey("SNAI"))
            {
                data3 = company_data_dic["SNAI"];
            }
            if (company_data_dic.ContainsKey("Betsson"))
            {
                data4 = company_data_dic["Betsson"];
            }
            if (company_data_dic.ContainsKey("Dafabet"))
            {
                data5 = company_data_dic["Dafabet"];
            }

          
            try
            {
                string[] text1 = data1.Split(new string[] { "," }, StringSplitOptions.None);
                string[] text2 = data2.Split(new string[] { "," }, StringSplitOptions.None);
                string[] text3 = data3.Split(new string[] { "," }, StringSplitOptions.None);
                string[] text4 = data4.Split(new string[] { "," }, StringSplitOptions.None);
                string[] text5 = data5.Split(new string[] { "," }, StringSplitOptions.None);

                double a0 = 0, a1 = 0, a2 = 0, b0 = 0, b1 = 0, b2 = 0, c0 = 0, c1 = 0, c2 = 0, d0 = 0, d1 = 0, d2 = 0, e0 = 0, e1 = 0, e2 = 0;


                if (text1.Length > 1)
                {
                    a0 = Convert.ToDouble(text1[1]);
                    a1 = Convert.ToDouble(text1[2]);
                    a2 = Convert.ToDouble(text1[3]);

                }
                if (text2.Length > 1)
                {
                    b0 = Convert.ToDouble(text2[1]);
                    b1 = Convert.ToDouble(text2[2]);
                    b2 = Convert.ToDouble(text2[3]);

                }
                if (text3.Length > 1)
                {
                    c0 = Convert.ToDouble(text3[1]);
                    c1 = Convert.ToDouble(text3[2]);
                    c2 = Convert.ToDouble(text3[3]);

                }
                if (text4.Length > 1)
                {
                    d0 = Convert.ToDouble(text4[1]);
                    d1 = Convert.ToDouble(text4[2]);
                    d2 = Convert.ToDouble(text4[3]);

                }
                if (text5.Length > 1)
                {
                    e0 = Convert.ToDouble(text5[1]);
                    e1 = Convert.ToDouble(text5[2]);
                    e2 = Convert.ToDouble(text5[3]);

                }

                //MessageBox.Show(a0+" "+b0+" "+c0+" "+d0+" "+e0);

                string value1 = function.FindDifference(a0, b0, c0, d0, e0,0).ToString("F2");
                string value2 = function.FindDifference(a1, b1, c1, d1, e1, 0).ToString("F2");
                string value3 = function.FindDifference(a2, b2, c2, d2, e2, 0).ToString("F2");

                return value1 + "  " + value2 + "  " + value3;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return "";
            }
        }


        /// <summary>
        /// 子大差
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string chazhi_zi(string id)
        {

            // "Betfair", "Betfair Exchange", "SNAI", "Betsson", "Dafabet"

            string data1 = "", data2 = "", data3 = "", data4 = "", data5 = "", data6 = "";

            DateTime b = Convert.ToDateTime(matchtime);

           

            string year = b.Year.ToString();
            
            if (company_data_dic.ContainsKey("William Hill"))
            {


                if ((Convert.ToDateTime(year+ "-" + paixudict["William Hill"]) - b).TotalHours < 1)
                {
                    data1 = company_data_dic["William Hill"];
                }


            }
            if (company_data_dic.ContainsKey("Bet-at-home"))
            {
                if ((Convert.ToDateTime(year + "-" + paixudict["Bet-at-home"]) - b).TotalHours < 1)
                {
                    data2 = company_data_dic["Bet-at-home"];
                }
            }
            if (company_data_dic.ContainsKey("SNAI"))
            {
                if ((Convert.ToDateTime(year + "-" + paixudict["SNAI"]) - b).TotalHours < 1)
                {
                    data3 = company_data_dic["SNAI"];
                }
            }
            if (company_data_dic.ContainsKey("Crown"))
            {
                if ((Convert.ToDateTime(year + "-" + paixudict["Crown"]) - b).TotalHours < 1)
                {
                    data4 = company_data_dic["Crown"];
                }
            }
            if (company_data_dic.ContainsKey("Dafabet"))
            {
                if ((Convert.ToDateTime(year + "-" + paixudict["Dafabet"]) - b).TotalHours < 1)
                {
                    data5 = company_data_dic["Dafabet"];
                }
            }
            if (company_data_dic.ContainsKey("Vcbet"))
            {
                if ((Convert.ToDateTime(year + "-" + paixudict["Vcbet"]) - b).TotalHours < 1)
                {
                    data6 = company_data_dic["Vcbet"];
                }
            }

            try
            {

                string[] text1 = data1.Split(new string[] { "," }, StringSplitOptions.None);
                string[] text2 = data2.Split(new string[] { "," }, StringSplitOptions.None);
                string[] text3 = data3.Split(new string[] { "," }, StringSplitOptions.None);
                string[] text4 = data4.Split(new string[] { "," }, StringSplitOptions.None);
                string[] text5 = data5.Split(new string[] { "," }, StringSplitOptions.None);
                string[] text6 = data6.Split(new string[] { "," }, StringSplitOptions.None);



                double a0 = 0, a1 = 0, a2 = 0, b0 = 0, b1 = 0, b2 = 0, c0 = 0, c1 = 0, c2 = 0, d0 = 0, d1 = 0, d2 = 0, e0 = 0, e1 = 0, e2 = 0,f0=0,f1=0,f2=0;


                if (text1.Length > 1)
                {
                    a0 = Convert.ToDouble(text1[1]);
                    a1 = Convert.ToDouble(text1[2]);
                    a2 = Convert.ToDouble(text1[3]);

                }
                if (text2.Length > 1)
                {
                    b0 = Convert.ToDouble(text2[1]);
                    b1 = Convert.ToDouble(text2[2]);
                    b2 = Convert.ToDouble(text2[3]);

                }
                if (text3.Length > 1)
                {
                    c0 = Convert.ToDouble(text3[1]);
                    c1 = Convert.ToDouble(text3[2]);
                    c2 = Convert.ToDouble(text3[3]);

                }
                if (text4.Length > 1)
                {
                    d0 = Convert.ToDouble(text4[1]);
                    d1 = Convert.ToDouble(text4[2]);
                    d2 = Convert.ToDouble(text4[3]);

                }
                if (text5.Length > 1)
                {
                    e0 = Convert.ToDouble(text5[1]);
                    e1 = Convert.ToDouble(text5[2]);
                    e2 = Convert.ToDouble(text5[3]);

                }

                if (text6.Length > 1)
                {
                    f0 = Convert.ToDouble(text6[1]);
                    f1 = Convert.ToDouble(text6[2]);
                    f2 = Convert.ToDouble(text6[3]);

                }

                string value1 = function.FindDifference(a0, b0, c0, d0, e0,f0).ToString("F2");
                string value2 = function.FindDifference(a1, b1, c1, d1, e1,f1).ToString("F2");
                string value3 = function.FindDifference(a2, b2, c2, d2, e2,f2).ToString("F2");

                return value1 + "  " + value2 + "  " + value3;
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.ToString());
                return "";
            }
        }





        /// <summary>
        /// 集团差
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string chazhi_jituan(string id)
        {
            // "Betfair", "Betfair Exchange", "SNAI", "Betsson", "Dafabet"

            string data1 = "", data2 = "", data3 = "";


            if (company_data_dic.ContainsKey("William Hill"))
            {
                data1 = company_data_dic["William Hill"];
            }
            if (company_data_dic.ContainsKey("Bet-at-home"))
            {
                data2 = company_data_dic["Bet-at-home"];
            }
            if (company_data_dic.ContainsKey("SNAI"))
            {
                data3 = company_data_dic["SNAI"];
            }
           

            try
            {
                string[] text1 = data1.Split(new string[] { "," }, StringSplitOptions.None);
                string[] text2 = data2.Split(new string[] { "," }, StringSplitOptions.None);
                string[] text3 = data3.Split(new string[] { "," }, StringSplitOptions.None);
              


                double a0 = 0, a1 = 0, a2 = 0, b0 = 0, b1 = 0, b2 = 0, c0 = 0, c1 = 0, c2 = 0;


                if (text1.Length > 1)
                {
                    a0 = Convert.ToDouble(text1[1]);
                    a1 = Convert.ToDouble(text1[2]);
                    a2 = Convert.ToDouble(text1[3]);

                }
                if (text2.Length > 1)
                {
                    b0 = Convert.ToDouble(text2[1]);
                    b1 = Convert.ToDouble(text2[2]);
                    b2 = Convert.ToDouble(text2[3]);

                }
                if (text3.Length > 1)
                {
                    c0 = Convert.ToDouble(text3[1]);
                    c1 = Convert.ToDouble(text3[2]);
                    c2 = Convert.ToDouble(text3[3]);

                }
               

                string value1 = function.FindDifference(a0, b0, c0, 0, 0, 0).ToString("F2");
                string value2 = function.FindDifference(a1, b1, c1, 0, 0, 0).ToString("F2");
                string value3 = function.FindDifference(a2, b2, c2, 0, 0, 0).ToString("F2");

                return value1 + "  " + value2 + "  " + value3;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return "";
            }

        }


        /// <summary>
        /// 获取指定比赛，皇冠亚让数据
        /// </summary>
        /// <param name="matchid"></param>
        public static string getdata_yarang(string matchid)
        {


            try
            {

                string aurl = "https://vip.titan007.com/changeDetail/handicap.aspx?id=" + matchid + "&companyID=3&l=0";
                string ahtml = method.GetUrl(aurl, "gb2312");

                MatchCollection trss = Regex.Matches(ahtml, @"<TR align=center([\s\S]*?)</TR>");


                List<string> gun_list = new List<string>();
                List<string> ji_list = new List<string>();
                List<string> zao_list = new List<string>();


                for (int j = 0; j < trss.Count; j++)
                {
                    string data = "";
                    string trhtml = trss[j].Groups[1].Value;

                    MatchCollection a = Regex.Matches(trhtml, @"<B>([\s\S]*?)</B>");
                    string rang = Regex.Match(trhtml, @"<TD><FONT color=([\s\S]*?)>([\s\S]*?)</TD>").Groups[2].Value;
                   
                    //客户不要时间
                    //string time = Regex.Match(trhtml, @"(0?[1-9]|1[0-2])-(0?[1-9]|[12][0-9]|3[01]) (0?[0-9]|1[0-9]|2[0-3]):([0-5][0-9])").Groups[0].Value;
                    if (a.Count == 2)
                    {
                        data = a[0].Groups[1].Value + "  ,  " + rang + "  ,  " + a[1].Groups[1].Value ;
                    }


                    if (trhtml.Contains("早"))
                    {
                       
                        zao_list.Add(data);
                    }

                    if (trhtml.Contains("即"))
                    {
                        ji_list.Add(data);
                    }

                    if (trhtml.Contains("滚") && !trhtml.Contains("封"))
                    {
                        gun_list.Add(data);
                    }
                }


                string data1 = "";
                string data2 = "";
                string data3 = "";
                string data4 = "";
                string data5 = "";
                string data6 = "";

                if (zao_list.Count > 0)
                {
                    data2 = zao_list[0];
                    data1 = zao_list[zao_list.Count - 1];
                }

                if (ji_list.Count > 0)
                {
                    data4 = ji_list[0];
                    data3 = ji_list[ji_list.Count - 1];
                }

                if (gun_list.Count > 0)
                {
                    data6 = gun_list[0];
                    data5 = gun_list[gun_list.Count - 1];
                }


                StringBuilder sb = new StringBuilder();

             

                if (data1.Trim() != "" && data3.Trim() != "" && data4.Trim() != "")//滚即早都有
                {
                    sb.AppendLine(data4);//使用data4替换历史盘口
                    sb.AppendLine(data3);//第一即
                    sb.AppendLine(data1);//第一个早
                }
                else if (data3.Trim() == "" && data4.Trim() == "")//没有即
                {
                   
                    sb.AppendLine(data2);//滚下面的早
                    sb.AppendLine(data1);//第一个早
                }


                return sb.ToString();



            }
            catch (Exception ex)
            {

                return "";
                //MessageBox.Show(ex.ToString());
            }



        }


        /// <summary>
        /// 凯利指数计算
        /// </summary>
        /// <param name="lv1"></param>
        public static void kaili_jisuan(System.Windows.Forms.ListView lv1)
        {
            try
            {
                string v11 = get_333(lv1.Items[0].SubItems[1].Text);
                string v12 = get_333(lv1.Items[0].SubItems[2].Text);
                string v13 = get_333(lv1.Items[0].SubItems[3].Text);
                string v14 = get_333(lv1.Items[0].SubItems[4].Text);
                string v15 = get_333(lv1.Items[0].SubItems[5].Text);

              


                int s1 = Convert.ToInt32(v11) + Convert.ToInt32(v12) + Convert.ToInt32(v13) + Convert.ToInt32(v14) + Convert.ToInt32(v15);
                int s2 = 0;
                int s4 = 0;
                int s6 = 0;
               

                string v21 = get_333(lv1.Items[1].SubItems[1].Text);
                string v22 = get_333(lv1.Items[1].SubItems[2].Text);
                string v23 = get_333(lv1.Items[1].SubItems[3].Text);
                string v24 = get_333(lv1.Items[1].SubItems[4].Text);
                string v25 = get_333(lv1.Items[1].SubItems[5].Text);

                int s3 = Convert.ToInt32(v21) + Convert.ToInt32(v22) + Convert.ToInt32(v23) + Convert.ToInt32(v24) + Convert.ToInt32(v25);


                int h1 = Convert.ToInt32(v11) + Convert.ToInt32(v21);
                int h2 = Convert.ToInt32(v12) + Convert.ToInt32(v22);
                int h3 = Convert.ToInt32(v13) + Convert.ToInt32(v23);
                int h4 = Convert.ToInt32(v14) + Convert.ToInt32(v24);
                int h5 = Convert.ToInt32(v15) + Convert.ToInt32(v25);

              s2=  get_he(h1)+ get_he(h2)+ get_he(h3)+ get_he(h4)+ get_he(h5);





                lv1.Items[0].SubItems[6].Text = s1.ToString() + s2.ToString();



                string v31 = get_333(lv1.Items[2].SubItems[1].Text);
                string v32 = get_333(lv1.Items[2].SubItems[2].Text);
                string v33 = get_333(lv1.Items[2].SubItems[3].Text);
                string v34 = get_333(lv1.Items[2].SubItems[4].Text);
                string v35 = get_333(lv1.Items[2].SubItems[5].Text);

                int s5 = Convert.ToInt32(v31) + Convert.ToInt32(v32) + Convert.ToInt32(v33) + Convert.ToInt32(v34) + Convert.ToInt32(v35);


                int h11 = Convert.ToInt32(v31) + Convert.ToInt32(v21);
                int h22 = Convert.ToInt32(v32) + Convert.ToInt32(v22);
                int h33 = Convert.ToInt32(v33) + Convert.ToInt32(v23);
                int h44 = Convert.ToInt32(v34) + Convert.ToInt32(v24);
                int h55 = Convert.ToInt32(v35) + Convert.ToInt32(v25);

                s4 = get_he(h11) + get_he(h22) + get_he(h33) + get_he(h44) + get_he(h55);

                s6= get_he3(h11+ Convert.ToInt32(v11)) + get_he3(h22+ Convert.ToInt32(v12)) + get_he3(h33+ Convert.ToInt32(v13)) + get_he3(h44+ Convert.ToInt32(v14)) + get_he3(h55+ Convert.ToInt32(v15));


                lv1.Items[1].SubItems[6].Text = s3.ToString() + s4.ToString();
                lv1.Items[2].SubItems[6].Text = s5.ToString() + s6.ToString();
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.ToString());
            }
        }


        public  static string get_333(string item)
        {
            string aa = "";
            switch (item.Trim())
               {
             

                    //主
                case "321":
                    aa = "01";
                    break;
                case "312":
                    aa = "01";
                    break;
                case "322":
                    aa = "01";
                    break;
                //和
                case "132":
                    aa = "11";
                    break;
                case "232":
                    aa = "11";
                    break;
                case "231":
                    aa = "11";
                    break;
                //客
                case "123":
                    aa = "10";
                    break;
                case "223":
                    aa = "10";
                    break;
                case "213":
                    aa = "10";
                    break;

                //大于2个3
                case "323":
                    aa = "11";
                    break;
                case "233":
                    aa = "11";
                    break;
                case "332":
                    aa = "11";
                    break;
                case "333":
                    aa = "11";
                    break;


                //为空
                case "":
                    aa = "0";
                    break;
            }

            return aa;    
        }

        //获取和值
        public static int get_he(int item)
        {
            int aa = 0;
            switch (item)
            {


                //11+01
                case 02:
                    aa = 01;
                    break;
                case 12:
                    aa = 01;
                    break;
             
                case 20:
                    aa = 10;
                    break;
               
                case 21:
                    aa = 10;
                    break;
              
                case 22:
                    aa = 11;
                    break;
              
               
            }

            return aa;
        }

        public static int get_he3(int item)
        {
            int aa = 0;
            switch (item)
            {

              
                case 13:
                    aa = 01;
                    break;
                case 23:
                    aa = 01;
                    break;
              
                case 03:
                    aa = 01;
                    break;
               

                case 30:
                    aa = 10;
                    break;
                case 31:
                    aa = 10;
                    break;
                case 32:
                    aa = 10;
                    break;
                //11+11+11

              
                case 33:
                    aa = 11;
                    break;



            }

            return aa;
        }







    }
}
