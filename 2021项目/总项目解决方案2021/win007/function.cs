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





        #region 获取当前实时比赛赔率
        public static string getshishidata(string matchid, string com)
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
                try
                {

                    if (cid == gongsi_dics[com])
                    {

                        string[] data_a = datasresult[0].Split(new string[] { "|" }, StringSplitOptions.None);
                        if (data_a.Length > 2)
                        {
                            data1 = data_a[0].Replace(cid, "").Replace("^", "");
                            data2 = data_a[1];
                            data3 = data_a[2];
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

                        sb.Append(cid + "," + data1 + "," + data2 + "," + data3 + "," + data4 + "," + data5 + "," + data6 + "," + data7 + "," + data8 + "," + data9);
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





        public static double FindDifference(double num1, double num2, double num3, double num4, double num5)
        {
            double[] numbers = { num1, num2, num3, num4, num5 };
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




        public static string gongsi5chazhi(string id, string com1, string com2, string com3, string com4, string com5)
        {



            string data1 = function.getshishidata(id, com1);
            string data2 = function.getshishidata(id, com2);
            string data3 = function.getshishidata(id, com3);
            string data4 = function.getshishidata(id, com4);
            string data5 = function.getshishidata(id, com5);

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

                string value1 = function.FindDifference(a0, b0, c0, d0, e0).ToString("F2");
                string value2 = function.FindDifference(a1, b1, c1, d1, e1).ToString("F2");
                string value3 = function.FindDifference(a2, b2, c2, d2, e2).ToString("F2");

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
                    string rang = Regex.Match(trhtml, @"<TD><FONT color=>([\s\S]*?)</TD>").Groups[1].Value;
                   
                    //客户不要时间
                    //string time = Regex.Match(trhtml, @"(0?[1-9]|1[0-2])-(0?[1-9]|[12][0-9]|3[01]) (0?[0-9]|1[0-9]|2[0-3]):([0-5][0-9])").Groups[0].Value;
                    if (a.Count == 2)
                    {
                        data = a[0].Groups[1].Value + "," + rang + "," + a[1].Groups[1].Value ;
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

                if (zao_list.Count > 1)
                {
                    data2 = zao_list[0];
                    data1 = zao_list[zao_list.Count - 1];
                }

                if (ji_list.Count > 1)
                {
                    data4 = ji_list[0];
                    data3 = ji_list[ji_list.Count - 1];
                }

                if (gun_list.Count > 1)
                {
                    data6 = gun_list[0];
                    data5 = gun_list[gun_list.Count - 1];
                }


                StringBuilder sb = new StringBuilder();


                sb.AppendLine(data4);//使用data4替换历史盘口
                sb.AppendLine(data3);
                sb.AppendLine(data1);
                return sb.ToString();



            }
            catch (Exception ex)
            {

                return "";
                //MessageBox.Show(ex.ToString());
            }



        }



    }
}
