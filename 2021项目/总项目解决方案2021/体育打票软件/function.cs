using gregn6Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 体育打票软件
{
    class function
    {
        #region ini读取
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }

        #endregion

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }


        #endregion

        string path = AppDomain.CurrentDomain.BaseDirectory;

        public static string getsuijima()
        {
            Random rd = new Random(Guid.NewGuid().GetHashCode()); //生成不重复的随机数，默认的话根据时间戳如果太快会相同

            string value1 = "";
            string value2 = "";
            string value3 = "";
            for (int i = 0; i < 15; i++)
            {

                int suiji = rd.Next(0, 10);
                value1 = value1 + suiji;
            }

            for (int i = 0; i < 8; i++)
            {

                int suiji = rd.Next(0, 10);
                value2 = value2 + suiji;
            }



            string zimu = "ABCDE0123456789";

            Random rd2 = new Random(Guid.NewGuid().GetHashCode()); //生成不重复的随机数，默认的话根据时间戳如果太快会相同

            for (int i = 0; i < 8; i++)
            {

                int suiji = rd.Next(0, 15);
                value3 = value3 + zimu[suiji];
            }

            return value1 + " " + value2 + " " + value3;
        }




        /// <summary>
        /// 混合过关判断表头
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public string panduan(MatchCollection results)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < results.Count; i++)
            {
                sb.Append("a" + results[i].Groups[2].Value + "a");

            }
            
            string maohao = Regex.Match(sb.ToString(), @":").Groups[0].Value;
            string shuzi = Regex.Match(sb.ToString(), @"a\da").Groups[0].Value;

            string sheng = Regex.Match(sb.ToString(), @"a胜a|a平a|a负a").Groups[0].Value;
            string rangsheng = Regex.Match(sb.ToString(), @"a让胜a|a让平a|a让负a").Groups[0].Value;
            string bansheng = Regex.Match(sb.ToString(), @"a胜胜a|a胜平a|a胜负a|a平胜a|a平平a|a平负a|a负胜a|a负平a|a负负a").Groups[0].Value;




            if (bansheng != "" && sheng == "" && rangsheng == "" && shuzi == "" && maohao == "")
            {
                return "竞彩足球半全场胜平负";
            }

            else if (bansheng == "" && sheng == "" && rangsheng != "" && shuzi == "" && maohao == "")
            {
                return "竞彩足球让球胜平负";
            }
            else if (bansheng == "" && sheng != "" && rangsheng == "" && shuzi == "" && maohao == "")
            {
                return "竞彩足球胜平负";
            }
            else if (bansheng == "" && sheng == "" && rangsheng == "" && shuzi != "" && maohao == "")
            {
                return "竞彩足球总进球数";
            }
            else if (bansheng == "" && sheng == "" && rangsheng == "" && shuzi == "" && maohao != "")
            {
                return "竞彩足球比分";
            }

            else
            {
                return "竞彩足球混合过关";
            }
        }


        

        #region  混合过关根据赔率获取让球文字
        public Dictionary<string, string> getrangqiu_hunhe(string html)
        {
            Dictionary<string, string> dics = new Dictionary<string, string>();
            MatchCollection peilVs = Regex.Matches(html, @"<label class=""rqCls rLine"">([\s\S]*?)</label>([\s\S]*?)solid;"">([\s\S]*?)<([\s\S]*?)solid;"">([\s\S]*?)</span><span([\s\S]*?)>([\s\S]*?)</span>");
           
            for (int i = 0; i < peilVs.Count; i++)
            {
                if (!dics.ContainsKey(peilVs[i].Groups[3].Value))
                {
                    dics.Add(peilVs[i].Groups[3].Value, peilVs[i].Groups[1].Value);
                }
                if (!dics.ContainsKey(peilVs[i].Groups[5].Value))
                {
                    dics.Add(peilVs[i].Groups[5].Value, peilVs[i].Groups[1].Value);
                }
                if (!dics.ContainsKey(peilVs[i].Groups[7].Value))
                {
                    dics.Add(peilVs[i].Groups[7].Value, peilVs[i].Groups[1].Value);
                }
                

            }

            return dics;
        }

        #endregion

        #region 混合过关(竞彩单打解析全部)
        public void getdata(GridppReport Report, string html, string ahtml,string guoguan)
        {

            string address = IniReadValue("values", "address");
            string haoma = IniReadValue("values", "haoma");
            string bianma = IniReadValue("values", "bianma");


            Report.LoadFromFile(path + "template\\" + "a.grf");
            try
            {
                string fangshi = "";
                string leixing = Regex.Match(html, @"<title>([\s\S]*?)</title>").Groups[1].Value;
                string suiji = bianma + function.getsuijima();
                string zhanhao = haoma;
                string jiangjin = Regex.Match(html, @"<span id=""bonus"">([\s\S]*?)</span>").Groups[1].Value;

                if (guoguan == "")
                {
                    guoguan = Regex.Match(html, @"checked="""">([\s\S]*?)</span>").Groups[1].Value;
                }
                string beishu = Regex.Match(html, @"x \d{1,2}倍").Groups[0].Value.Replace("x", "").Replace("倍", "").Trim();
                


                string jine = Regex.Match(html, @"<span id=""consume"">([\s\S]*?)</span>").Groups[1].Value;
                string ganxieyu = "感谢您为公益事业贡献 "+ Math.Round(Convert.ToDouble(Convert.ToDouble(jine) * 0.21), 2)+"元";
               
                string time = DateTime.Now.ToString("yy/MM/dd HH:mm:ss").Replace("-", "/");

                string zhushu = ((Convert.ToDouble(jine) / Convert.ToDouble(beishu)) / 2).ToString();
              

                // guoguan = Regex.Replace(guoguan, "x.*", "") + "x" + zhushu;


                MatchCollection matchIds = Regex.Matches(html, @"class=""mCodeCls""([\s\S]*?)>([\s\S]*?)</td>");
                MatchCollection matchNames = Regex.Matches(html, @"<span class=""AgainstInfo"">([\s\S]*?)</span>");

                Dictionary<string, string> dics = new Dictionary<string, string>();
                for (int i = 0; i < matchIds.Count; i++)
                {
                    string matchname = Regex.Replace(matchNames[i].Groups[1].Value, "<[^>]+>", "");
                    matchname = Regex.Replace(matchname, @"\[.*?\]", "");
                    dics.Add(matchIds[i].Groups[2].Value, matchname);
                }

                StringBuilder sb = new StringBuilder();



                MatchCollection value1 = Regex.Matches(html, @"<span>周([\s\S]*?)</span>");  //普通
                MatchCollection value2 = Regex.Matches(html, @"204\);"">周([\s\S]*?)</span>");  //让球




                MatchCollection vv = Regex.Matches(html, @"<span>周([\s\S]*?)\(([\s\S]*?)@");
                 fangshi = panduan(vv);


                if(value2.Count>0)
                {
                    fangshi = "竞彩足球混合过关";
                }
                if (value1.Count==0 && value2.Count > 0)
                {
                    fangshi = "竞彩足球让球胜平负";
                }




                List<string> lists = new List<string>();
                Dictionary<string, string> resultdics = new Dictionary<string, string>();

                for (int i = 0; i < value1.Count; i++)
                {
                    

                    string a2 = Regex.Match(value1[i].Groups[1].Value, @"\(([\s\S]*?)\)").Groups[1].Value;
                    //整数赔率保留2位
                    if (a2.Substring(a2.Length - 2, 2) == "00")
                    {
                        a2 = a2 + "元";
                    }
                    else
                    {
                        a2 = a2 + "0元";
                    }

                    //末尾文字 胜平负/进球数/比分判断
                    string moweiwenzi = "";
                   
                    if (a2.Substring(0, 2).Contains(":") || a2.Contains("其他"))
                    {
                        moweiwenzi = "比分";
                    }
                    else if (a2.Substring(0, 2).Contains("胜@") || a2.Substring(0, 2).Contains("平@")|| a2.Substring(0, 2).Contains("负@"))
                    {
                        moweiwenzi = "胜平负";
                    }
                    else if (a2.Substring(0, 2).Contains("胜胜")|| a2.Substring(0, 2).Contains("胜平") || a2.Substring(0, 2).Contains("胜负") || a2.Substring(0, 2).Contains("平胜") || a2.Substring(0, 2).Contains("平平") || a2.Substring(0, 2).Contains("平负") || a2.Substring(0, 2).Contains("负胜") || a2.Substring(0, 2).Contains("负平") || a2.Substring(0, 2).Contains("负负"))
                    {
                        moweiwenzi = "半全场胜平负";
                    }
                    else if (a2.Substring(0, 2).Contains("1@") || a2.Substring(0, 2).Contains("2@") || a2.Substring(0, 2).Contains("3@") || a2.Substring(0, 2).Contains("4@") || a2.Substring(0, 2).Contains("5@") || a2.Substring(0, 2).Contains("6@") || a2.Substring(0, 2).Contains("7+"))
                    {
                        moweiwenzi = "总进球数";
                    }
                    


                    if (!fangshi.Contains("混合过关"))
                    {
                        moweiwenzi = " ";
                    }







                    string a1 = Regex.Match("周" + value1[i].Groups[1].Value, @"周([\s\S]*?)\(").Groups[1].Value + " "+moweiwenzi;
                   
                    if (a2.Substring(0, 2) != "胜@" && a2.Substring(0, 2) != "平@" && a2.Substring(0, 2) != "负@")
                    {
                        a2 = "(" + a2.Replace("@", ")@");
                    }
                    if (resultdics.ContainsKey(a1))
                    {
                        if (!lists.Contains(a1 + a2))
                        {
                            lists.Add(a1 + a2);
                            resultdics[a1] = resultdics[a1].Replace("VS", "Vs") + "+" + a2;
                        }

                    }
                    else
                    {
                        lists.Add(a1 + a2);
                        resultdics.Add(a1, "主队:" + dics["周" + a1.Replace(moweiwenzi, "").Replace("让球", "").Trim()].Replace("VS", " Vs 客队:") + "\n" + a2);

                    }

                   
                   
                   
                }
              


                for (int i = 0; i < value2.Count; i++)
                {
                    string a2 = Regex.Match(value2[i].Groups[1].Value, @"\(([\s\S]*?)\)").Groups[1].Value;

                    //整数赔率保留2位
                    if (a2.Substring(a2.Length - 2, 2) == "00")
                    {
                        a2 = a2 + "元";
                    }
                    else
                    {
                        a2 = a2 + "0元";
                    }


                    string peilv= Regex.Match(a2, @"\@([\s\S]*?)0元").Groups[1].Value;
                    string rangqiuwenzi = "";
                    Dictionary<string,string> hunherangqiudic= getrangqiu_hunhe(html);

                    if (hunherangqiudic.ContainsKey(peilv))
                    {
                        if (hunherangqiudic[peilv].Contains("-"))
                        {
                            rangqiuwenzi = " 主队让" + hunherangqiudic[peilv].Replace("-", "") + "球";
                        }
                        if (hunherangqiudic[peilv].Contains("+"))
                        {
                            rangqiuwenzi = " 主队受让" + hunherangqiudic[peilv].Replace("+", "") + "球";
                        }
                    }

                   
                    string a1 = Regex.Match("周" + value2[i].Groups[1].Value, @"周([\s\S]*?)\(").Groups[1].Value + " 让球胜平负" + rangqiuwenzi;

                   


                    if (a2.Substring(0, 2) != "胜@" && a2.Substring(0, 2) != "平@" && a2.Substring(0, 2) != "负@")
                    {
                        a2 = "(" + a2.Replace("@",")@");
                    }
                    if (resultdics.ContainsKey(a1))
                    {
                        if (!lists.Contains(a2))
                        {
                            lists.Add(a2);
                            resultdics[a1] = resultdics[a1].Replace("VS", "Vs") + "+" + a2;
                        }

                    }
                    else
                    {
                        lists.Add(a2);
                        resultdics.Add(a1, "主队:" + dics["周" + a1.Replace(" 让球胜平负" + rangqiuwenzi, "").Trim()].Replace("VS", " Vs 客队:") + "\n" + a2);

                    }

                }

                resultdics = resultdics.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);//升序
                //resultdics =resultdics.OrderByDescending(o => o.Key).ToDictionary(o => o.Key, p => p.Value); //降序
             
                
                a = 0;
                foreach (var item in resultdics.Keys)
                {
                    a = a + 1;
                    if (fangshi == "竞彩足球让球胜平负")
                    {
                        sb.Append("第" + a + "场 周" + item.Replace("让球胜平负","") + "\n" + resultdics[item] + "\n");
                    }
                    else
                    {
                        if(fangshi == "竞彩足球混合过关")
                        {
                            sb.Append("第" + a + "场周" + item + "\n" + resultdics[item] + "\n");
                        }
                        else
                        {
                            sb.Append("第" + a + "场 周" + item + "\n" + resultdics[item] + "\n");
                        }
                        
                    }

                   
                   
                   
                }



              

                //单注倍数计算

                Dictionary<string, int> aaa = new Dictionary<string, int>();

                int aa = 1;
                MatchCollection  asa = Regex.Matches(html, @"<td style=""width: 80px;"">([\s\S]*?)<");
                foreach (Match item in asa)
                {
                    if (item.Groups[1].Value.Contains("x"))
                    {
                        if (!aaa.ContainsKey(item.Groups[1].Value))
                        {
                            aa = 1;
                            aaa.Add(item.Groups[1].Value,aa);
                        }
                        else
                        {
                            aa = aa + 1;
                            aaa[item.Groups[1].Value] = aa;
                        }
                    }
                    

                }

                StringBuilder sba = new StringBuilder();
                foreach (var item in aaa.Keys)
                {
                    sba.Append(item + "*" + aaa[item] + "注,");
                }


                sb.Append("(选项固定奖金额为每1元投注对应的奖金额)\n本票最高可能固定奖金:" + Convert.ToDouble(jiangjin).ToString("F2") + "元\n单倍注数:" +sba.ToString().Remove(sba.ToString().Length-1,1)+ ";共" + zhushu + "注");



                Report.ParameterByName("suiji").AsString = suiji;
                Report.ParameterByName("fangshi").AsString = fangshi;
                Report.ParameterByName("leixing").AsString = leixing;
                Report.ParameterByName("guoguan").AsString = "过关方式 " + guoguan;
                Report.ParameterByName("beishu").AsString = beishu;

                Report.ParameterByName("jine").AsString = jine;
                Report.ParameterByName("neirong").AsString = sb.ToString();

         
                Report.ParameterByName("dizhi").AsString = ganxieyu+"\n\n"+ address;
                Report.ParameterByName("zhanhao").AsString = haoma;
                Report.ParameterByName("time").AsString = time;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }



        #endregion

        public static int a = 0;
        Dictionary<string, string> rangqiudic = new Dictionary<string, string>();

        


        public string beishu_500 = "1";

        #region 500网
        public void getdata_500(GridppReport Report, string html, string ahtml)
        {

          

            string address = IniReadValue("values", "address");
            string haoma = IniReadValue("values", "haoma");
            string bianma = IniReadValue("values", "bianma");


            Report.LoadFromFile(path + "template\\" + "a.grf");
            try
            {
                string fangshi =  Regex.Match(ahtml, @"【([\s\S]*?)】").Groups[1].Value;
                string leixing = "";
                string suiji = bianma + function.getsuijima();
                string zhanhao = haoma;
                string jiangjin = Regex.Match(html, @"<span data-html=""bonus"">([\s\S]*?)</span>").Groups[1].Value;
                if(jiangjin.Contains("~"))
                {
                    jiangjin = jiangjin.Split(new string[] { "~" }, StringSplitOptions.None)[1];
                }

                string guoguan = Regex.Match(html, @"chkbox-on"" data-value=""([\s\S]*?)""").Groups[1].Value;
                
                if (guoguan == "")
                {
                    guoguan = Regex.Match(html, @"chkbox-on"" value=""([\s\S]*?)""").Groups[1].Value;
                }


                //倍数
                //在webbrowser浏览器里执行js代码并获取返回值，可以用于读取cookie
              
                


                string jine = Regex.Match(html, @"<span data-html=""prize"">([\s\S]*?)</span>").Groups[1].Value;
                string ganxieyu = "感谢您为公益事业贡献 " + Math.Round(Convert.ToDouble(Convert.ToDouble(jine) * 0.21), 2) + "元";

                string time = DateTime.Now.ToString("yy/MM/dd HH:mm:ss").Replace("-", "/");

                // string zhushu = ((Convert.ToDouble(jine) / Convert.ToDouble(beishu)) / 2).ToString();
                string zhushu = "";

               

                MatchCollection matchIds = Regex.Matches(ahtml, @"data-matchnum=""([\s\S]*?)"""); //周三001
                MatchCollection matchNames = Regex.Matches(ahtml, @"data-homesxname=""([\s\S]*?)"" data-awaysxname=""([\s\S]*?)"""); //队伍名

                Dictionary<string, string> dics = new Dictionary<string, string>();
                for (int i = 0; i < matchIds.Count; i++)
                {
                   
                    dics.Add(matchIds[i].Groups[1].Value, "主队："+matchNames[i].Groups[1].Value+" Vs 客队："+ matchNames[i].Groups[2].Value);
                }

                StringBuilder sb = new StringBuilder();


                MatchCollection value1 = Regex.Matches(html, @"删除</a></td><td>([\s\S]*?)</tr>"); //周一 队伍名 赔率都在

                if (value1.Count == 1 && guoguan.Contains("单关"))  //单关一场用a1模板
                {
                    Report.LoadFromFile(path + "template\\" + "a1.grf");
                }

                List<string> lists = new List<string>();
              
                for (int i = 0; i < value1.Count; i++)
                {

                    StringBuilder a2sb = new StringBuilder();

                    MatchCollection a2s = Regex.Matches(value1[i].Groups[1].Value, @"data-sort=""([\s\S]*?)"">([\s\S]*?)</a>");
                  
                    if (guoguan.Contains("单关"))
                    {
                        fangshi = getdanguanbiaotou_500(a2sb.ToString());
                        a2s = Regex.Matches(value1[i].Groups[1].Value, @"removeItemBy([\s\S]*?)"">([\s\S]*?)</a>");


                       
                    }
                   
                    
                    foreach (Match item in a2s)
                    {
                        string item1 = item.Groups[2].Value;
                        if (item.Groups[2].Value.Substring(0, 2) != "胜@" && item.Groups[2].Value.Substring(0, 2) != "平@" && item.Groups[2].Value.Substring(0, 2) != "负@")
                        {
                            item1 = "(" + item1.Replace("@", ")@");
                        }


                        //整数赔率保留2位
                        if (item1.Substring(item1.Length - 2, 2) == "00")
                        {
                            a2sb.Append(item1 + "元" + "+");
                        }
                        else
                        {
                            a2sb.Append(item1 + "0元" + "+");
                        }
                        
                    }
                    string a2 = a2sb.ToString().Remove(a2sb.ToString().Length-1,1);
                  
                    #region //末尾文字 胜平负/进球数/比分判断
                    string moweiwenzi = "";
                    if (a2.Substring(0, 2).Contains(":"))
                    {
                        moweiwenzi = " 比分";
                    }
                    else if (a2.Substring(0, 2).Contains("胜@") || a2.Substring(0, 2).Contains("平@") || a2.Substring(0, 2).Contains("负@"))
                    {
                        moweiwenzi = " 胜平负";
                    }
                    else if (a2.Substring(0, 2).Contains("胜胜") || a2.Substring(0, 2).Contains("胜平") || a2.Substring(0, 2).Contains("胜负") || a2.Substring(0, 2).Contains("平胜") || a2.Substring(0, 2).Contains("平平") || a2.Substring(0, 2).Contains("平负") || a2.Substring(0, 2).Contains("负胜") || a2.Substring(0, 2).Contains("负平") || a2.Substring(0, 2).Contains("负负"))
                    {
                        moweiwenzi = " 半全场胜平负";
                    }
                    else if (a2.Substring(0, 2).Contains("1@") || a2.Substring(0, 2).Contains("2@") || a2.Substring(0, 2).Contains("3@") || a2.Substring(0, 2).Contains("4@") || a2.Substring(0, 2).Contains("5@") || a2.Substring(0, 2).Contains("6@") || a2.Substring(0, 2).Contains("7+@"))
                    {
                        moweiwenzi = "总进球数";
                    }
                    else if (a2.Contains("[-1]"))
                    {
                        moweiwenzi = " 让球胜平负 主队让1球";
                        a2 = a2.Replace("[-1] ","");
                    }
                    else if (a2.Contains("[+1]"))
                    {
                        moweiwenzi = " 让球胜平负 主队受让1球";
                        a2 = a2.Replace("[+1] ", "");
                    }
                    else if (a2.Contains("[-2]"))
                    {
                        moweiwenzi = " 让球胜平负 主队让2球";
                        a2 = a2.Replace("[-2] ", "");
                    }
                    else if (a2.Contains("[+2]"))
                    {
                        moweiwenzi = " 让球胜平负 主队受让2球";
                        a2 = a2.Replace("[+2] ", "");
                    }

                    #endregion
                    string a1 = "第"+(i+1)+"场 周"+Regex.Match(value1[i].Groups[1].Value, @"周([\s\S]*?)</td>").Groups[1].Value + moweiwenzi;

                    string duiwunameA = Regex.Match(value1[i].Groups[1].Value, @"<span class=""team-l"">([\s\S]*?)</span>").Groups[1].Value;
                    string duiwunameB = Regex.Match(value1[i].Groups[1].Value, @"<span class=""team-r"">([\s\S]*?)</span>").Groups[1].Value;

                    a1 = a1 + "\n" + "主队：" + duiwunameA + " Vs 客队：" + duiwunameB;

                    if (sb.ToString().Contains("主队：" + duiwunameA + " Vs 客队：" + duiwunameB))
                    {
                        sb.Append("+"+a2.ToString());
                    }
                    else
                    {
                        if(i==0)
                        {
                            sb.Append( a1 + "\n" + a2.Trim());

                        }
                        else
                        {
                            sb.Append("\n" + a1 + "\n" + a2.Trim());
                        }
                       

                    }



                   
                }

                if ((sb.ToString().Split(new string[] { "\n" }, StringSplitOptions.None)).Length < 4 && guoguan.Contains("单关"))  //单关一场用a1模板
                {
                    Report.LoadFromFile(path + "template\\" + "a1.grf");
                   
                }
                if (guoguan.Contains("单关"))
                {
                    fangshi = getdanguanbiaotou_500(sb.ToString());
                }
                //单注倍数计算

                //Dictionary<string, int> aaa = new Dictionary<string, int>();

                //int aa = 1;
                //MatchCollection asa = Regex.Matches(html, @"<td style=""width: 80px;"">([\s\S]*?)<");
                //foreach (Match item in asa)
                //{
                //    if (item.Groups[1].Value.Contains("x"))
                //    {
                //        if (!aaa.ContainsKey(item.Groups[1].Value))
                //        {
                //            aa = 1;
                //            aaa.Add(item.Groups[1].Value, aa);
                //        }
                //        else
                //        {
                //            aa = aa + 1;
                //            aaa[item.Groups[1].Value] = aa;
                //        }
                //    }


                //}

                //StringBuilder sba = new StringBuilder();
                //foreach (var item in aaa.Keys)
                //{
                //    sba.Append(item + "*" + aaa[item] + "注,");
                //}

                if (guoguan.Contains("单关"))
                {
                    sb.Append("\n(选项固定奖金额为每1元投注对应的奖金额)\n本票最高可能固定奖金:" + Convert.ToDouble(jiangjin).ToString("F2") + "元\n单倍注数：单场*"+ value1.Count + "注;共" + value1.Count + "注");
                }
                else
                {
                    sb.Append("\n(选项固定奖金额为每1元投注对应的奖金额)\n本票最高可能固定奖金:" + Convert.ToDouble(jiangjin).ToString("F2") + "元\n单倍注数:" +guoguan+"*"+ value1.Count + "注;共" + value1.Count + "注");
                }
                //sb.Append("(选项固定奖金额为每1元投注对应的奖金额)\n本票最高可能固定奖金:" + jiangjin + "元\n单倍注数:" + sba.ToString().Remove(sba.ToString().Length - 1, 1) + ";共" + zhushu + "注");


               
                Report.ParameterByName("suiji").AsString = suiji;
                Report.ParameterByName("fangshi").AsString = fangshi;
                Report.ParameterByName("leixing").AsString = leixing;
                if (guoguan.Contains("单关"))
                {
                    guoguan = guoguan.Replace("单关", "1场-单场固定");
                    Report.ParameterByName("guoguan").AsString =  guoguan;
                }
                else
                {

                    Report.ParameterByName("guoguan").AsString = "过关方式 " + guoguan;
                }
               
                Report.ParameterByName("beishu").AsString = beishu_500;

                Report.ParameterByName("jine").AsString = jine;
                Report.ParameterByName("neirong").AsString = sb.ToString();


                Report.ParameterByName("dizhi").AsString = ganxieyu + "\n\n" + address;
                Report.ParameterByName("zhanhao").AsString = haoma;
                Report.ParameterByName("time").AsString = time;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region 500网单关获取表头类型
        public string getdanguanbiaotou_500(string sb)
        {

           
            string maohao = Regex.Match(sb.ToString(), @":").Groups[0].Value;
            string shuzi = Regex.Match(sb.ToString(), @"\(\d\)").Groups[0].Value;

            string sheng = Regex.Match(sb.ToString(), @"胜@|平@|负@").Groups[0].Value;
           
            string bansheng = Regex.Match(sb.ToString(), @"胜胜|胜平|胜负|平胜|平平|平负|负胜|负平|负负").Groups[0].Value;

            if (bansheng != "" && sheng == ""  && shuzi == "" && maohao == "")
            {
                return "竞彩足球半全场胜平负";
            }

            else if (bansheng == "" && sheng != ""  && shuzi == "" && maohao == "")
            {
                return "竞彩足球胜平负";
            }
           
            else if (bansheng == "" && sheng == ""  && shuzi == "" && maohao != "")
            {
                return "竞彩足球比分";
            }
            else if (bansheng == "" && sheng == "" && shuzi != "" && maohao == "")
            {
                return "竞彩足球总进球数";
            }
            else
            {
                return "竞彩足球混合过关";
            }

        }

        #endregion





        #region 组合获取
        /// <summary>
        /// 获得从n个不同元素中任意选取m个元素的组合的所有组合形式的列表
        /// </summary>
        /// <param name="elements">供组合选择的元素</param>
        /// <param name="m">组合中选取的元素个数</param>
        /// <returns>返回一个包含列表的列表，包含的每一个列表就是每一种组合可能</returns>
        public static List<List<string>> GetCombinationList(List<string> elements, int m)
        {
            List<List<string>> result = new List<List<string>>();//存放返回的列表
            List<List<string>> temp = null; //临时存放从下一级递归调用中返回的结果
            List<string> oneList = null; //存放每次选取的第一个元素构成的列表，当只需选取一个元素时，用来存放剩下的元素分别取其中一个构成的列表；
           string oneElment; //每次选取的元素
            List<string> source = new List<string>(elements); //将传递进来的元素列表拷贝出来进行处理，防止后续步骤修改原始列表，造成递归返回后原始列表被修改；
            int n = 0; //待处理的元素个数

            if (elements != null)
            {
                n = elements.Count;
            }
            if (n == m && m != 1)//n=m时只需将剩下的元素作为一个列表全部输出
            {
                result.Add(source);
                return result;
            }
            if (m == 1)  //只选取一个时，将列表中的元素依次列出
            {
                foreach (string el in source)
                {
                    oneList = new List<string>();
                    oneList.Add(el);
                    result.Add(oneList);
                    oneList = null;
                }
                return result;
            }

            for (int i = 0; i <= n - m; i++)
            {
                oneElment = source[0];
                source.RemoveAt(0);
                temp = GetCombinationList(source, m - 1);
                for (int j = 0; j < temp.Count; j++)
                {
                    oneList = new List<string>();
                    oneList.Add(oneElment);
                    oneList.AddRange(temp[j]);
                    result.Add(oneList);
                    oneList = null;
                }
            }


            return result;
        }


        #endregion


        


        #region 获取队伍名全名
        public string getfullname(string body)
        {
            string result = body;
            string url = "https://webapi.sporttery.cn/gateway/jc/football/getMatchListV1.qry?clientCode=3001";
            string html = GetUrl(url,"utf-8");
            MatchCollection addname = Regex.Matches(html, @"AbbName"":""([\s\S]*?)""");
            MatchCollection AllName = Regex.Matches(html, @"AllName"":""([\s\S]*?)""");
            for (int i = 0; i < addname.Count; i++)
            {
               
                result = Regex.Replace(result, addname[i].Groups[1].Value.Trim(), AllName[i].Groups[1].Value);
            }
            return result;
        }

        #endregion

    }
}
