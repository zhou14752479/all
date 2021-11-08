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
using System.Xml;

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

        //删除7日以上xml文件
        public void delete7days()
        {
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] files = d.GetFiles();//文件
            foreach (FileInfo f in files)
            {
                if(f.Extension==".xml")
                {
                    if(Convert.ToDateTime(Path.GetFileNameWithoutExtension(f.FullName))<DateTime.Now.AddDays(-8))
                    {
                        File.Delete(f.FullName);
                    }
                }
                
            }
           
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
            string qita = Regex.Match(sb.ToString(), @"其他").Groups[0].Value;
            string maohao = Regex.Match(sb.ToString(), @":").Groups[0].Value;

            if (maohao == "")
            {
                maohao = qita;
            }


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
            else if (bansheng == "" && sheng == "" && rangsheng == "" && shuzi == "" && maohao != "")
            {
                return "竞彩足球比分";
            }
            else
            {
                return "竞彩足球混合过关";
            }
        }



        /// <summary>
        /// 混合过关_篮球判断表头
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public string panduan_lanqiu(MatchCollection results)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < results.Count; i++)
            {
                sb.Append("a" + results[i].Groups[2].Value + "a");

            }

           
            string shengfencha = Regex.Match(sb.ToString(), @"-").Groups[0].Value;
            string zhusheng = Regex.Match(sb.ToString(), @"a主胜a|a主负a").Groups[0].Value;
            string rangfen = Regex.Match(sb.ToString(), @"a让分主胜a|a让分主负a").Groups[0].Value;
            string daxiao = Regex.Match(sb.ToString(), @"a大a|a小a").Groups[0].Value;




            if (zhusheng != "" && rangfen == "" && daxiao == "" && shengfencha == "")
            {
                return "竞彩篮球胜负";
            }

            else if (zhusheng == "" && rangfen != "" && daxiao == "" && shengfencha == "")
            {
                return "竞彩篮球让分胜负";
            }
            else if (zhusheng == "" && rangfen == "" && daxiao != "" && shengfencha == "")
            {
                return "竞彩篮球大小分";
            }
            else if (zhusheng == "" && rangfen == "" && daxiao == "" && shengfencha != "")
            {
                return "竞彩篮球胜分差";
            }
         

            else
            {
                return "竞彩篮球混合过关";
            }
        }


        #region  混合过关根据赔率获取让球文字
        public Dictionary<string, string> getrangqiu_hunhe(string html)
        {
            //Dictionary<string, string> dics = new Dictionary<string, string>();
            //MatchCollection peilVs = Regex.Matches(html, @"<label class=""rqCls rLine"">([\s\S]*?)</label>([\s\S]*?)solid;"">([\s\S]*?)<([\s\S]*?)solid;"">([\s\S]*?)</span><span([\s\S]*?)>([\s\S]*?)</span>");
            //for (int i = 0; i < peilVs.Count; i++)
            //{
            //    if (!dics.ContainsKey(peilVs[i].Groups[3].Value))
            //    {
            //        dics.Add(peilVs[i].Groups[3].Value, peilVs[i].Groups[1].Value);
            //    }
            //    if (!dics.ContainsKey(peilVs[i].Groups[5].Value))
            //    {
            //        dics.Add(peilVs[i].Groups[5].Value, peilVs[i].Groups[1].Value);
            //    }
            //    if (!dics.ContainsKey(peilVs[i].Groups[7].Value))
            //    {
            //        dics.Add(peilVs[i].Groups[7].Value, peilVs[i].Groups[1].Value);
            //    }

            //}


            Dictionary<string, string> dics = new Dictionary<string, string>();

            MatchCollection zhous = Regex.Matches(html, @"<td class=""mCodeCls"" style=""width: 89px;"">([\s\S]*?)</td>");

            MatchCollection peilVs = Regex.Matches(html, @"<label class=""rqCls rLine"">([\s\S]*?)</label>([\s\S]*?)solid;"">([\s\S]*?)<([\s\S]*?)solid;"">([\s\S]*?)</span><span([\s\S]*?)>([\s\S]*?)</span>");
            for (int i = 0; i < peilVs.Count; i++)
            {
                string zhou = zhous[i].Groups[1].Value;

                // MessageBox.Show(zhou + " "+peilVs[i].Groups[3].Value+"  "+ zhou +" "+ peilVs[i].Groups[5].Value+"  "+ zhou +" "+ peilVs[i].Groups[7].Value);
                if (!dics.ContainsKey(zhou + peilVs[i].Groups[3].Value))
                {
                    dics.Add(zhou + peilVs[i].Groups[3].Value, peilVs[i].Groups[1].Value);
                }
                if (!dics.ContainsKey(zhou + peilVs[i].Groups[5].Value))
                {
                    dics.Add(zhou + peilVs[i].Groups[5].Value, peilVs[i].Groups[1].Value);
                }
                if (!dics.ContainsKey(zhou + peilVs[i].Groups[7].Value))
                {
                    dics.Add(zhou + peilVs[i].Groups[7].Value, peilVs[i].Groups[1].Value);
                }

            }

            return dics;
        }

        #endregion


        string lanqiu_rangfen = "0";
        string lanqiu_yushezongfen = "0";


        #region  混合过关根据赔率获取让球文字_篮球
        public void getrangqiu_hunhe_lanqiu(string html,string zhou,string leixing)
        {
            Dictionary<string, string> dics = new Dictionary<string, string>();
            MatchCollection zhous = Regex.Matches(html, @"<td class=""mCodeCls"" style=""width: 70px;"">([\s\S]*?)</td>([\s\S]*?)inline-block;"">([\s\S]*?)</label>([\s\S]*?)inline-block;"">([\s\S]*?)</label>");
            for (int i = 0; i < zhous.Count; i++)
            {
                if(zhou== zhous[i].Groups[1].Value)
                {
                    lanqiu_rangfen = zhous[i].Groups[3].Value.Replace("(", "").Replace(")", "");
                    lanqiu_yushezongfen = zhous[i].Groups[5].Value.Replace("(", "").Replace(")", "");
                    break;
                }
            }
                     
           
        }

        #endregion

        #region 混合过关足球(竞彩网)
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
                    else if (a2.Substring(0, 2).Contains("0@") || a2.Substring(0, 2).Contains("1@") || a2.Substring(0, 2).Contains("2@") || a2.Substring(0, 2).Contains("3@") || a2.Substring(0, 2).Contains("4@") || a2.Substring(0, 2).Contains("5@") || a2.Substring(0, 2).Contains("6@") || a2.Substring(0, 2).Contains("7+"))
                    {
                        moweiwenzi = "总进球数";
                    }
                    


                    if (!fangshi.Contains("混合过关"))
                    {
                        moweiwenzi = " ";
                    }





                   

                    string a1 = Regex.Match("周" + value1[i].Groups[1].Value, @"周([\s\S]*?)\(").Groups[1].Value + " "+moweiwenzi;
                   
                    if (a2.Substring(0, 2) != "胜@" && a2.Substring(0, 2) != "平@" && a2.Substring(0, 2) != "负@")  //胜平负不要括号
                    {
                        if (a2.Substring(0, 2)!="胜胜"& a2.Substring(0, 2) != "胜平" & a2.Substring(0, 2) != "胜负" & a2.Substring(0, 2) != "平胜" & a2.Substring(0, 2) != "平平" & a2.Substring(0, 2) != "平负" & a2.Substring(0, 2) != "负胜" & a2.Substring(0, 2) != "负平" & a2.Substring(0, 2) != "负负") //半全场不要括号
                        {
                            a2 = "(" + a2.Replace("@", ")@");
                        }
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

                 
                    string peilv = Regex.Match(a2, @"\@.*").Groups[0].Value.Replace("@", "");


                    //整数赔率保留2位
                    if (a2.Substring(a2.Length - 2, 2) == "00")
                    {
                        a2 = a2 + "元";
                    }
                    else
                    {
                        a2 = a2 + "0元";
                    }


                     //string peilv= Regex.Match(a2, @"\@([\s\S]*?)0元").Groups[1].Value;
                   

                    string rangqiuwenzi = "";
                    Dictionary<string,string> hunherangqiudic= getrangqiu_hunhe(html);

                    string zhouji ="周"+Regex.Match("周" + value2[i].Groups[1].Value, @"周([\s\S]*?)\(").Groups[1].Value;
                   
                    if (hunherangqiudic.ContainsKey(zhouji+peilv))
                    {
                        
                        if (hunherangqiudic[zhouji+peilv].Contains("-"))
                        {
                            rangqiuwenzi = " 主队让" + hunherangqiudic[zhouji+peilv].Replace("-", "") + "球";
                        }
                        if (hunherangqiudic[zhouji+peilv].Contains("+"))
                        {
                            rangqiuwenzi = " 主队受让" + hunherangqiudic[zhouji+peilv].Replace("+", "") + "球";
                        }
                    }

                   
                    string a1 = Regex.Match("周" + value2[i].Groups[1].Value, @"周([\s\S]*?)\(").Groups[1].Value + " 让球胜平负" + rangqiuwenzi;



                  
                    if (a2.Substring(0, 2) != "胜@" && a2.Substring(0, 2) != "平@" && a2.Substring(0, 2) != "负@")
                    {
                        a2 = "(" + a2.Replace("@", ")@");
                    }

                   
                    if (resultdics.ContainsKey(a1))
                    {
                       
                        if (!lists.Contains(a1 + a2))  //防止赔率相同 结果i相同 导致不添加
                        {
                            lists.Add(a1 + a2);
                            resultdics[a1] = resultdics[a1].Replace("VS", "Vs") + "+" + a2;
                        }

                    }
                    else
                    {
                      
                        lists.Add(a1 + a2);
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


                sb.Append("(选项固定奖金额为每1元投注对应的奖金额)\n本票最高可能固定奖金:" + string.Format("{0:N}", Convert.ToDouble(jiangjin)) + "元\n单倍注数:" +sba.ToString().Remove(sba.ToString().Length-1,1)+ ";共" + zhushu + "注");

                guoguan= "过关方式 " + guoguan;
              

                Report.ParameterByName("suiji").AsString = suiji;
                Report.ParameterByName("fangshi").AsString = fangshi;
                Report.ParameterByName("leixing").AsString = leixing;
                Report.ParameterByName("guoguan").AsString = guoguan; 
                Report.ParameterByName("beishu").AsString = beishu;

                Report.ParameterByName("jine").AsString = jine;
                Report.ParameterByName("neirong").AsString = sb.ToString();

         
                Report.ParameterByName("dizhi").AsString = ganxieyu+"\n\n"+ address;
                Report.ParameterByName("zhanhao").AsString = haoma;
                Report.ParameterByName("time").AsString = time;
                CreateXmlFile(fangshi, suiji, guoguan, beishu, jine, sb.ToString(), ganxieyu, address, haoma, time);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }



        #endregion

        #region 混合过关篮球(竞彩网)
        public void getdata_lanqiu(GridppReport Report, string html, string ahtml, string guoguan)
        {
            getrangqiu_hunhe_lanqiu(html,"","");
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
                string ganxieyu = "感谢您为公益事业贡献 " + Math.Round(Convert.ToDouble(Convert.ToDouble(jine) * 0.21), 2) + "元";

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
                fangshi = panduan_lanqiu(vv);


                if (value2.Count > 0)
                {
                    fangshi = "竞彩篮球混合过关";
                }
                if (value1.Count == 0 && value2.Count > 0)
                {
                    fangshi = "竞彩篮球让分胜负";
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

                   
                    if (a2.Contains("-") || a2.Contains("26+"))
                    {
                        moweiwenzi = " 胜分差";
                    }
                    else if (a2.Substring(0, 2).Contains("主负") || a2.Substring(0, 2).Contains("主胜") )
                    {
                        moweiwenzi = " 胜负";
                    }
                    else if (a2.Substring(0, 2).Contains("让分"))
                    {
                        moweiwenzi = " 让分胜负";
                    }
                    else if (a2.Substring(0, 2).Contains("大@") || a2.Substring(0, 2).Contains("小@"))
                    {
                        moweiwenzi = " 大小分";
                    }



                    if (!fangshi.Contains("混合过关"))
                    {
                        moweiwenzi = "";
                    }


                    string yushezongfen = "";
                    //大小分 预设总分
                    string zhou = "周" + Regex.Match("周" + value1[i].Groups[1].Value, @"周([\s\S]*?)\(").Groups[1].Value;

                    getrangqiu_hunhe_lanqiu(html, zhou, "");

                    if (a2.Substring(0, 2).Contains("大@") || a2.Substring(0, 2).Contains("小@"))
                    {
                        yushezongfen = "预设总分:" + lanqiu_yushezongfen;
                    }


                    moweiwenzi = moweiwenzi +" "+ yushezongfen;
              




                    string a1 = Regex.Match("周" + value1[i].Groups[1].Value, @"周([\s\S]*?)\(").Groups[1].Value + moweiwenzi;
                    
                    //胜分差去掉胜添加括号
                    if (a2.Contains("-") || a2.Contains("26+"))
                    {
                        a2 = "(" + a2.Replace("胜", "").Replace("@", ")@");
                    }
                    else
                    {
                        a2 = a2.Replace("主胜", "胜").Replace("主负", "负").Replace("让分", "");
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
                        resultdics.Add(a1, "客队:" + dics["周" + a1.Replace(moweiwenzi, "").Replace("让分", "").Trim()].Replace("VS", " Vs 主队:") + "\n" + a2);

                    }




                }


               //让分
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


                    string peilv = Regex.Match(a2, @"\@([\s\S]*?)0元").Groups[1].Value;
                    string rangqiuwenzi = "";



                  string zhou = "周"+ Regex.Match("周" + value2[i].Groups[1].Value, @"周([\s\S]*?)\(").Groups[1].Value;
                    
                    getrangqiu_hunhe_lanqiu(html,zhou,"");

                    if (lanqiu_rangfen.Contains("-"))
                    {
                        rangqiuwenzi = " 主队让" + lanqiu_rangfen.Replace("-", "").Replace("+", "") + "分";
                    }
                    if (lanqiu_rangfen.Contains("+"))
                    {
                        rangqiuwenzi = " 主队受让" + lanqiu_rangfen.Replace("-", "").Replace("+", "") + "分";
                    }




                    string a1 = Regex.Match("周" + value2[i].Groups[1].Value, @"周([\s\S]*?)\(").Groups[1].Value + " 让分胜负" + rangqiuwenzi;




                   if (a2.Contains("-"))
                    {
                        a2 = "(" + a2.Replace("胜", "").Replace("@", ")@");//胜分差去掉胜
                    }

                    a2 = a2.Replace("主胜","胜").Replace("主负", "负").Replace("让分", "");
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
                        resultdics.Add(a1, "客队:" + dics["周" + a1.Replace(" 让分胜负" + rangqiuwenzi, "").Trim()].Replace("VS", " Vs 主队:") + "\n" + a2);

                    }

                }

                resultdics = resultdics.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);//升序
                                                                                                   //resultdics =resultdics.OrderByDescending(o => o.Key).ToDictionary(o => o.Key, p => p.Value); //降序


                a = 0;
                foreach (var item in resultdics.Keys)
                {
                    a = a + 1;
                    if (fangshi == "竞彩篮球让分胜负")
                    {
                        sb.Append("第" + a + "场 周" + item.Replace("让分胜负", "") + "\n" + resultdics[item] + "\n");
                    }
                    else
                    {
                        if (fangshi == "竞彩篮球混合过关")
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
                MatchCollection asa = Regex.Matches(html, @"<td style=""width: 80px;"">([\s\S]*?)<");
                foreach (Match item in asa)
                {
                    if (item.Groups[1].Value.Contains("x"))
                    {
                        if (!aaa.ContainsKey(item.Groups[1].Value))
                        {
                            aa = 1;
                            aaa.Add(item.Groups[1].Value, aa);
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


                sb.Append("(选项固定奖金额为每1元投注对应的奖金额)\n本票最高可能固定奖金:" + string.Format("{0:N}", Convert.ToDouble(jiangjin)) + "元\n单倍注数:" + sba.ToString().Remove(sba.ToString().Length - 1, 1) + ";共" + zhushu + "注");


                guoguan = "过关方式 " + guoguan;
                Report.ParameterByName("suiji").AsString = suiji;
                Report.ParameterByName("fangshi").AsString = fangshi;
                Report.ParameterByName("leixing").AsString = leixing;
                Report.ParameterByName("guoguan").AsString =guoguan ;
                Report.ParameterByName("beishu").AsString = beishu;

                Report.ParameterByName("jine").AsString = jine;
                Report.ParameterByName("neirong").AsString = sb.ToString();


                Report.ParameterByName("dizhi").AsString = ganxieyu + "\n\n" + address;
                Report.ParameterByName("zhanhao").AsString = haoma;
                Report.ParameterByName("time").AsString = time;
                CreateXmlFile(fangshi, suiji, guoguan, beishu, jine, sb.ToString(), ganxieyu, address, haoma, time);
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

        #region 500网足球
        public void getdata_500(GridppReport Report, string html, string ahtml,string jiexi_jine,string jiexi_beishu)
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


                //调换顺序
                string fan_value1 = "";
                for (int s = value1.Count - 1; s >= 0; s--)
                {
                    fan_value1 += "删除</a></td><td>" + value1[s].Groups[1].Value + "</tr>";
                }
                value1 = Regex.Matches(fan_value1, @"删除</a></td><td>([\s\S]*?)</tr>"); //周一 队伍名 赔率都在
                //调换顺序结束




                if (value1.Count == 1 && guoguan.Contains("单关"))  //单关一场用a1模板
                {
                    Report.LoadFromFile(path + "template\\" + "a1.grf");
                }
                string jiangjin_max = "0";
                value1.Cast<Match>().Reverse();
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
                            if (item.Groups[2].Value.Substring(0, 2) != "胜胜" & item.Groups[2].Value.Substring(0, 2) != "胜平" & item.Groups[2].Value.Substring(0, 2) != "胜负" & item.Groups[2].Value.Substring(0, 2) != "平胜" & item.Groups[2].Value.Substring(0, 2) != "平平" & item.Groups[2].Value.Substring(0, 2) != "平负" & item.Groups[2].Value.Substring(0, 2) != "负胜" & item.Groups[2].Value.Substring(0, 2) != "负平" & item.Groups[2].Value.Substring(0, 2) != "负负") //半全场不要括号
                            {
                                item1 = "(" + item1.Replace("@", ")@");
                            }
                        }

                        if(item1.Contains("客胜"))
                        {
                            item1 = item1.Replace("客胜", "客");
                        }
                        if (item1.Contains("主胜"))
                        {
                            item1 = item1.Replace("主胜", "主");
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
                        moweiwenzi = "";
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

                  
                    //获取最大奖金
                    string jiangjins = Regex.Match(value1[i].Groups[1].Value, @"￥([\s\S]*?)</span>").Groups[1].Value;
                 
                        if (Convert.ToDouble(jiangjins) > Convert.ToDouble(jiangjin_max))
                        {

                            jiangjin_max= jiangjins;
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




              
               
                if(jiexi_jine!="" && beishu_500!="")
                {
                    beishu_500 = jiexi_beishu;
                    jiangjin = (Convert.ToDouble(jiangjin_max) / 10 * Convert.ToInt32(beishu_500)).ToString();
                    jine = jiexi_jine;
                   
                    ganxieyu = "感谢您为公益事业贡献 " + Math.Round(Convert.ToDouble(Convert.ToDouble(jine) * 0.21), 2) + "元";
                }
                
                if (guoguan.Contains("单关"))
                {
                    sb.Append("\n(选项固定奖金额为每1元投注对应的奖金额)\n本票最高可能固定奖金:" + string.Format("{0:N}", Convert.ToDouble(jiangjin)) + "元\n单倍注数：单场*"+ value1.Count + "注;共" + value1.Count + "注");
                }
                else
                {
                    sb.Append("\n(选项固定奖金额为每1元投注对应的奖金额)\n本票最高可能固定奖金:" + string.Format("{0:N}", Convert.ToDouble(jiangjin)) + "元\n单倍注数:" +guoguan+"*"+ value1.Count + "注;共" + value1.Count + "注");
                }
                //sb.Append("(选项固定奖金额为每1元投注对应的奖金额)\n本票最高可能固定奖金:" + jiangjin + "元\n单倍注数:" + sba.ToString().Remove(sba.ToString().Length - 1, 1) + ";共" + zhushu + "注");


               
                Report.ParameterByName("suiji").AsString = suiji;
                Report.ParameterByName("fangshi").AsString = fangshi;
                Report.ParameterByName("leixing").AsString = leixing;
                if (guoguan.Contains("单关"))
                {
                    guoguan = guoguan.Replace("单关", "1场-单场固定");
                   
                }
                else
                {

                    guoguan = "过关方式 " + guoguan;
                }
                Report.ParameterByName("guoguan").AsString = guoguan;
                Report.ParameterByName("beishu").AsString = beishu_500;

                Report.ParameterByName("jine").AsString = jine;
                Report.ParameterByName("neirong").AsString = sb.ToString().Replace("它","他");


                Report.ParameterByName("dizhi").AsString = ganxieyu + "\n\n" + address;
                Report.ParameterByName("zhanhao").AsString = haoma;
                Report.ParameterByName("time").AsString = time;
                CreateXmlFile(fangshi, suiji, guoguan, beishu_500, jine, sb.ToString(), ganxieyu, address, haoma, time);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region 500网篮球
        public void getdata_500_lanqiu(GridppReport Report, string html, string ahtml, string jiexi_jine, string jiexi_beishu)
        {



            string address = IniReadValue("values", "address");
            string haoma = IniReadValue("values", "haoma");
            string bianma = IniReadValue("values", "bianma");


            Report.LoadFromFile(path + "template\\" + "a.grf");
            try
            {
                string fangshi = Regex.Match(ahtml, @"【([\s\S]*?)】").Groups[1].Value;
                string leixing = "";
                string suiji = bianma + function.getsuijima();
                string zhanhao = haoma;
                string jiangjin = Regex.Match(html, @"<span data-html=""bonus"">([\s\S]*?)</span>").Groups[1].Value;
                if (jiangjin.Contains("~"))
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

                    dics.Add(matchIds[i].Groups[1].Value, "客队：" + matchNames[i].Groups[1].Value + " Vs 主队：" + matchNames[i].Groups[2].Value);
                }

                StringBuilder sb = new StringBuilder();


                MatchCollection value1 = Regex.Matches(html, @"删除</a></td><td>([\s\S]*?)</tr>"); //周一 队伍名 赔率都在



                //调换顺序
                string fan_value1 = "";
                for (int s = value1.Count - 1; s >= 0; s--)
                {
                    fan_value1 += "删除</a></td><td>" + value1[s].Groups[1].Value + "</tr>";
                }
                value1 = Regex.Matches(fan_value1, @"删除</a></td><td>([\s\S]*?)</tr>"); //周一 队伍名 赔率都在
                                                                                       //调换顺序结束




                if (value1.Count == 1 && guoguan.Contains("单关"))  //单关一场用a1模板
                {
                    Report.LoadFromFile(path + "template\\" + "a1.grf");
                }
                string jiangjin_max = "0";
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

                        if (item1.Contains("客胜"))
                        {
                            item1 = item1.Replace("客胜", "客");
                        }
                        if (item1.Contains("主胜"))
                        {
                            item1 = item1.Replace("主胜", "主");
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
                    string a2 = a2sb.ToString().Remove(a2sb.ToString().Length - 1, 1);

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
                        a2 = a2.Replace("[-1] ", "");
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
                    string a1 = "第" + (i + 1) + "场 周" + Regex.Match(value1[i].Groups[1].Value, @"周([\s\S]*?)</td>").Groups[1].Value + moweiwenzi;

                    string duiwunameA = Regex.Match(value1[i].Groups[1].Value, @"<span class=""team-l"">([\s\S]*?)</span>").Groups[1].Value;
                    string duiwunameB = Regex.Match(value1[i].Groups[1].Value, @"<span class=""team-r"">([\s\S]*?)</span>").Groups[1].Value;

                    a1 = a1 + "\n" + "客队：" + duiwunameA + " Vs 主队：" + duiwunameB;

                    if (sb.ToString().Contains("客队：" + duiwunameA + " Vs 主队：" + duiwunameB))
                    {
                        sb.Append("+" + a2.ToString());
                    }
                    else
                    {
                        if (i == 0)
                        {
                            sb.Append(a1 + "\n" + a2.Trim());

                        }
                        else
                        {
                            sb.Append("\n" + a1 + "\n" + a2.Trim());
                        }


                    }


                    //获取最大奖金
                    string jiangjins = Regex.Match(value1[i].Groups[1].Value, @"￥([\s\S]*?)</span>").Groups[1].Value;

                    if (Convert.ToDouble(jiangjins) > Convert.ToDouble(jiangjin_max))
                    {

                        jiangjin_max = jiangjins;
                    }

                }


                if (jiexi_jine != "" && beishu_500 != "")
                {
                    beishu_500 = jiexi_beishu;
                    jiangjin = (Convert.ToDouble(jiangjin_max) / 10 * Convert.ToInt32(beishu_500)).ToString();
                    jine = jiexi_jine;

                    ganxieyu = "感谢您为公益事业贡献 " + Math.Round(Convert.ToDouble(Convert.ToDouble(jine) * 0.21), 2) + "元";
                }


                if ((sb.ToString().Split(new string[] { "\n" }, StringSplitOptions.None)).Length < 4 && guoguan.Contains("单关"))  //单关一场用a1模板
                {
                    Report.LoadFromFile(path + "template\\" + "a1.grf");

                }
                if (guoguan.Contains("单关"))
                {
                    fangshi = getdanguanbiaotou_500_lanqiu(sb.ToString());
                }
              

                if (guoguan.Contains("单关"))
                {
                    sb.Append("\n(选项固定奖金额为每1元投注对应的奖金额)\n本票最高可能固定奖金:" + string.Format("{0:N}", Convert.ToDouble(jiangjin)) + "元\n单倍注数：单场*" + value1.Count + "注;共" + value1.Count + "注");
                }
                else
                {
                    sb.Append("\n(选项固定奖金额为每1元投注对应的奖金额)\n本票最高可能固定奖金:" + string.Format("{0:N}", Convert.ToDouble(jiangjin)) + "元\n单倍注数:" + guoguan + "*" + value1.Count + "注;共" + value1.Count + "注");
                }
                //sb.Append("(选项固定奖金额为每1元投注对应的奖金额)\n本票最高可能固定奖金:" + jiangjin + "元\n单倍注数:" + sba.ToString().Remove(sba.ToString().Length - 1, 1) + ";共" + zhushu + "注");



                Report.ParameterByName("suiji").AsString = suiji;
                Report.ParameterByName("fangshi").AsString = fangshi;
                Report.ParameterByName("leixing").AsString = leixing;
                if (guoguan.Contains("单关"))
                {
                    guoguan = guoguan.Replace("单关", "1场-单场固定");

                }
                else
                {

                    guoguan = "过关方式 " + guoguan;
                }
                Report.ParameterByName("guoguan").AsString = guoguan;
                Report.ParameterByName("beishu").AsString = beishu_500;

                Report.ParameterByName("jine").AsString = jine;
                Report.ParameterByName("neirong").AsString = sb.ToString();


                Report.ParameterByName("dizhi").AsString = ganxieyu + "\n\n" + address;
                Report.ParameterByName("zhanhao").AsString = haoma;
                Report.ParameterByName("time").AsString = time;
                CreateXmlFile(fangshi, suiji, guoguan, beishu_500, jine, sb.ToString(), ganxieyu, address, haoma, time);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region 500网单关获取表头类型_足球
        public string getdanguanbiaotou_500(string sb)
        {
           
            string maohao = Regex.Match(sb.ToString(), @":").Groups[0].Value;
            if(maohao=="")
            {
                maohao = Regex.Match(sb.ToString(), @"胜其|负其|平其").Groups[0].Value;
            }
            string shuzi = Regex.Match(sb.ToString(), @"\(\d\)").Groups[0].Value;

            if(shuzi=="")
            {
               shuzi = Regex.Match(sb.ToString(), @"\(7\+\)").Groups[0].Value;
            }
            

            string sheng = Regex.Match(sb.ToString(), @"胜@|平@|负@").Groups[0].Value;
           
            string bansheng = Regex.Match(sb.ToString(), @"胜胜|胜平|胜负|平胜|平平|平负|负胜|负平|负负").Groups[0].Value;

            if (bansheng != ""   && shuzi == "" && maohao == "")
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

        #region 500网单关获取表头类型_篮球
        public string getdanguanbiaotou_500_lanqiu(string sb)
        {


           
            sb = "a" + sb + "a";

            string shengfencha = Regex.Match(sb.ToString(), @"-").Groups[0].Value;
            string zhusheng = Regex.Match(sb.ToString(), @"a主胜a|a主负a").Groups[0].Value;
            string rangfen = Regex.Match(sb.ToString(), @"a让分主胜a|a让分主负a").Groups[0].Value;
            string daxiao = Regex.Match(sb.ToString(), @"a大a|a小a").Groups[0].Value;




            if (zhusheng != "" && rangfen == "" && daxiao == "" && shengfencha == "")
            {
                return "竞彩篮球胜负";
            }

            else if (zhusheng == "" && rangfen != "" && daxiao == "" && shengfencha == "")
            {
                return "竞彩篮球让分胜负";
            }
            else if (zhusheng == "" && rangfen == "" && daxiao != "" && shengfencha == "")
            {
                return "竞彩篮球大小分";
            }
            else if (zhusheng == "" && rangfen == "" && daxiao == "" && shengfencha != "")
            {
                return "竞彩篮球胜分差";
            }


            else
            {
                return "竞彩篮球混合过关";
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

     
        #region 获取队伍名全名足球
        public string getfullname_zuqiu(string body)
        {
            List<string> lists = new List<string>();
            string result = body;
            string url = "https://webapi.sporttery.cn/gateway/jc/football/getMatchListV1.qry?clientCode=3001";
            string html = GetUrl(url,"utf-8");
            MatchCollection addname = Regex.Matches(html, @"AbbName"":""([\s\S]*?)""");
            MatchCollection AllName = Regex.Matches(html, @"AllName"":""([\s\S]*?)""");
            for (int i = 0; i < addname.Count; i++)
            {
                if (!lists.Contains(AllName[i].Groups[1].Value))
                {
                    lists.Add(AllName[i].Groups[1].Value);
                   result = Regex.Replace(result, addname[i].Groups[1].Value.Trim(), AllName[i].Groups[1].Value);
                  
                }
                
            }
            return result;
        }

        #endregion

        #region 获取队伍名全名_篮球
        public string getfullname_lanqiu(string body)
        {
            List<string> lists = new List<string>();
            string result = body;
            string url = "https://webapi.sporttery.cn/gateway/jc/basketball/getMatchListV1.qry?clientCode=3001";
            string html = GetUrl(url, "utf-8");
            MatchCollection addname = Regex.Matches(html, @"AbbName"":""([\s\S]*?)""");
            MatchCollection AllName = Regex.Matches(html, @"AllName"":""([\s\S]*?)""");
            for (int i = 0; i < addname.Count; i++)
            {
                if (!lists.Contains(AllName[i].Groups[1].Value))
                {
                    lists.Add(AllName[i].Groups[1].Value);
                    result = Regex.Replace(result, addname[i].Groups[1].Value.Trim(), AllName[i].Groups[1].Value);
                }
            }
            return result;
        }

        #endregion

        #region 创建xml

        public void CreateXmlFile(string leixing,string suijima,string guoguan,string beishu,string jine,string neirong,string juankaun,string address,string bianhao,string time)
        {
            XmlDocument xmlDoc = new XmlDocument();
            if (File.Exists(path + "//" + DateTime.Now.ToString("yyyy年MM月dd日") + ".xml"))
            {

              
                xmlDoc.Load(path + "//" + DateTime.Now.ToString("yyyy年MM月dd日") + ".xml");
                //XmlNode root = doc.DocumentElement;
                //XmlElement MacAddress = doc.CreateElement("MacAddress");
                //XmlElement Mac = doc.CreateElement("Mac");
                //Mac.InnerText = "00-1E-90-91-07-AB";
                //XmlElement Time = doc.CreateElement("Time");
                //Time.InnerText = "16:33:21";
                //MacAddress.AppendChild(Mac);
                //MacAddress.AppendChild(Time);
                //root.AppendChild(MacAddress);
              
              
                //读取根节点  
                XmlNode root = xmlDoc.DocumentElement;
               

                XmlElement data = xmlDoc.CreateElement("记录" + DateTime.Now.ToString("yyyy年MM月dd日HH时mm分"));
                root.AppendChild(data);


                CreateNode(xmlDoc, data, "票据类型", leixing);
                CreateNode(xmlDoc, data, "随机编码", suijima);
                CreateNode(xmlDoc, data, "过关方式", guoguan);
                CreateNode(xmlDoc, data, "下注倍数", beishu);
                CreateNode(xmlDoc, data, "下注金额", jine);
                CreateNode(xmlDoc, data, "单子内容", neirong);
                CreateNode(xmlDoc, data, "公益捐款", juankaun);
                CreateNode(xmlDoc, data, "店铺地址", address);
                CreateNode(xmlDoc, data, "店铺编号", bianhao);
                CreateNode(xmlDoc, data, "打印时间", time);
             

            }
            else
            {
               
                //创建类型声明节点  
                XmlNode node = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
                xmlDoc.AppendChild(node);



                //创建根节点  
                XmlNode root = xmlDoc.CreateElement("记录");
                xmlDoc.AppendChild(root);


                XmlElement data= xmlDoc.CreateElement("记录" + DateTime.Now.ToString("yyyy年MM月dd日HH时mm分"));
                root.AppendChild(data);

                CreateNode(xmlDoc, data, "票据类型", leixing);
                CreateNode(xmlDoc, data, "随机编码", suijima);
                CreateNode(xmlDoc, data, "过关方式", guoguan);
                CreateNode(xmlDoc, data, "下注倍数", beishu);
                CreateNode(xmlDoc, data, "下注金额", jine);
                CreateNode(xmlDoc, data, "单子内容", neirong);
                CreateNode(xmlDoc, data, "公益捐款", juankaun);
                CreateNode(xmlDoc, data, "店铺地址", address);
                CreateNode(xmlDoc, data, "店铺编号", bianhao);
                CreateNode(xmlDoc, data, "打印时间", time);
        
            }
            try
            {
                xmlDoc.Save(path + "//" + DateTime.Now.ToString("yyyy年MM月dd日") + ".xml");
            }
            catch (Exception e)
            {
                //显示错误信息  
                MessageBox.Show(e.ToString());
            }


        }

        /// <summary>    
        /// 创建节点    
        /// </summary>    
        /// <param name="xmldoc"></param>  xml文档  
        /// <param name="parentnode"></param>父节点    
        /// <param name="name"></param>  节点名  
        /// <param name="value"></param>  节点值  
        ///   
        public void CreateNode(XmlDocument xmlDoc, XmlNode parentNode, string name, string value)
        {
            XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, name, null);
            node.InnerText = value;
            parentNode.AppendChild(node);
        }

        #endregion


        #region 读取xml

   public void readxml(ListView lst,string date,string time)
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlReaderSettings settings = new XmlReaderSettings();

            settings.IgnoreComments = true;//忽略文档里面的注释

            XmlReader reader = XmlReader.Create(path + "//"+date+".xml", settings);

            xmlDoc.Load(reader);

            //得到根节点

            XmlNode xn = xmlDoc.SelectSingleNode("记录");

            //得到根节点的所有子节点

            XmlNodeList xns= xn.ChildNodes;
           
            foreach (XmlNode xn1 in xns)
            {
              
                if (Convert.ToDateTime(xn1.ChildNodes[9].InnerText).ToString("yy/MM/dd HH:mm") == time)
                {
                    ListViewItem lv1 = lst.Items.Add((lst.Items.Count + 1).ToString());

                    foreach (XmlNode xn2 in xn1) //得到子节点的子节点

                    {
                        // 将节点转换为元素，便于得到节点的属性值
                        XmlElement xe = (XmlElement)xn2;

                        lv1.SubItems.Add(xn2.InnerText);
                        lv1.Checked = true;
                    }
                }
            }
          
         

        }


        #endregion

    }
}
