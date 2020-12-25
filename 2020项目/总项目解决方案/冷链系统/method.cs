using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace 冷链系统
{
    class method
    {
        public string Authorization { get; set; }
        public delegate void GetLogs(string log);
        public event GetLogs getlogs;
        public string constr = "";
        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public string GetUrl(string Url, string charset)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                WebHeaderCollection headers = request.Headers;
                headers.Add("Authorization:" + Authorization);
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();


                reader.Close();
                response.Close();
                return content;


            }
            catch (System.Exception ex)
            {
                getlogs(ex.ToString());

            }
            return "";
        }

       
        public  string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                WebHeaderCollection headers = request.Headers;
                headers.Add("Authorization:" + Authorization);
                request.ContentType = "application/json";
                //request.ContentLength = postData.Length;
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

     



        public bool insert(string sql)
        {
          
            try
            {

               
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand(sql, mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    return true;

                }
                else
                {
                    return false;
                }


            }

            catch (Exception ex)
            {
                getlogs(ex.ToString());
                return false;
            }

        }
        public bool insertGoods(string sheet, string goodsName, string hsCode, string batchNumber, string spec, string goodsTypeName, string remainNumber, string packageNumber, string entryDate, string portName, string uid)
        {
            string sql = "INSERT INTO "+sheet+" (goodsName ," +
                            "hsCode," +
                            "batchNumber," +
                            "spec," +
                            "goodsTypeName," +
                            "remainNumber," +
                            "packageNumber," +
                            "entryDate," +
                            "portName," +
                            "uid)" +
                            "VALUES('" + goodsName + " '," +
                            " '" + hsCode + " '," +
                            " '" + batchNumber + " '," +
                            " '" + spec + " '," +
                            " '" + goodsTypeName + " '," +
                            " '" + remainNumber + " '," +
                            " '" + packageNumber + " '," +
                            " '" + entryDate + " '," +
                            " '" + portName + " '," +
                             " '" + uid+ " ')";
            try
            {


                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand(sql, mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    return true;

                }
                else
                {
                    return false;
                }


            }

            catch (Exception ex)
            {
                getlogs(ex.ToString());
                return false;
            }

        }

        public bool TRUNCATE(string sql)
        {

            try
            {


                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand(sql, mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    return true;

                }
                else
                {
                    return false;
                }


            }

            catch (Exception ex)
            {
                getlogs(ex.ToString());
                return false;
            }

        }

        public string getimages(string uid)
        {
            try
            {
                string param = "{\"ownerid\":\""+uid+"\",\"name\":\"消毒证明\"}";

                string html = PostUrl("http://117.73.254.122:8099/api/uploadfile/filedownload", param, "", "utf-8");

              
                MatchCollection files = Regex.Matches(html, @"""fileUrl"":""([\s\S]*?)""");
                StringBuilder sb = new StringBuilder();
                foreach (Match file in files)
                {
                    sb.Append(file.Groups[1].Value+",");
                }
                if(sb.ToString().Length>2)
                {
                    return sb.ToString().Substring(0, sb.ToString().Length - 1);
                    
                }
                else
                {
                    return sb.ToString();
                }
               

            }
            catch (Exception ex)
            {

                getlogs(ex.ToString());
                return "";
            }
        }

        public string shibie()
        {
            try
            {
                string html = GetUrl("http://117.73.254.122:8099/api/captchaImage","utf-8");

                string img= Regex.Match(html, @"""img"":""([\s\S]*?)""").Groups[1].Value;
                string uuid = Regex.Match(html, @"""uuid"":""([\s\S]*?)""").Groups[1].Value;

                string param = "{\"username\":\"zhou14752479\",\"password\":\"zhoukaige00\",\"image\":\"" + img + "\"}";

                string PostResult = PostUrl("http://api.ttshitu.com/base64", param,"","utf-8");

                string result = Regex.Match(PostResult, @"result"":""([\s\S]*?)""").Groups[1].Value;
                string loginparam = "{\"username\":\"13500000015\",\"password\":\"a123456\",\"code\":\""+result+"\",\"uuid\":\""+uuid+"\"}";
                return loginparam;

            }
            catch (Exception ex)
            {
   
                return "";
            }
        }
        public string login()
        {
            try
            {
                

                string param = shibie();

                string PostResult = PostUrl("http://117.73.254.122:8099/api/loginStore", param, "", "utf-8");

                string result = Regex.Match(PostResult, @"token"":""([\s\S]*?)""").Groups[1].Value;
                return result;

            }
            catch (Exception ex)
            {

                return "";
            }
        }

        ArrayList uidlist = new ArrayList();

        public string gettimestamp(string date)
        {
            try
            {
                Match date1= Regex.Match(date, @"20([\s\S]*?) ");


                DateTime dt = Convert.ToDateTime("20"+date1.Groups[1].Value).Date;
                DateTime DateStart = new DateTime(1970, 1, 1, 8, 0, 0);

                return Convert.ToInt32((dt - DateStart).TotalSeconds).ToString();
            }
            catch (Exception)
            {

                return "";
            }




        }

        #region 获取数据库vxcode集合
        public void getuids()
        {

            try
            {

                string str = "SELECT uid from ccgl";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    uidlist.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ee)
            {
                ;
            }


        }
        #endregion

        public void yygl()
        {
            TRUNCATE("TRUNCATE TABLE yygl_goods");
            TRUNCATE("TRUNCATE TABLE yygl");
            try
            {
                
                    string url = "http://117.73.254.122:8099/api/storeApp/listForStore?pageNum=1&pageSize=1000";
                    string ahtml = GetUrl(url,"utf-8");
                    Match strhtml = Regex.Match(ahtml, @"pageInfo([\s\S]*?)msg");
                    MatchCollection ids = Regex.Matches(strhtml.Groups[1].Value, @"""storeAppId"":""([\s\S]*?)""");

                    if (ids.Count == 0)
                        return;
                    foreach (Match id in ids)
                    {
                       
                        string aurl = "http://117.73.254.122:8099/api/storeApp/getById?id="+id.Groups[1].Value;
                        string html = GetUrl(aurl, "utf-8");
                       
                        string warehouseName = Regex.Match(html, @"""warehouseName"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string appSeqNo = Regex.Match(html, @"""appSeqNo"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string companyName = Regex.Match(html, @"""companyName"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string companyCreditCode = Regex.Match(html, @"""companyCreditCode"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string companyContactName = Regex.Match(html, @"""companyContactName"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string companyContactPhone = Regex.Match(html, @"""companyContactPhone"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string supplierCompanyName = Regex.Match(html, @"""supplierCompanyName"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string portNo = Regex.Match(html, @"""portNo"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string boxNo = Regex.Match(html, @"""boxNo"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string statusRemark = Regex.Match(html, @"""statusRemark"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string createTime = Regex.Match(html, @"""createTime"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string expectedTime = Regex.Match(html, @"""expectedTime"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string noticeInTime = Regex.Match(html, @"""noticeInTime"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string noticeOutTime = Regex.Match(html, @"""noticeOutTime"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string trafficNo = Regex.Match(html, @"""trafficNo"":([\s\S]*?),").Groups[1].Value.Replace("\"","");
                        string trafficContactName = Regex.Match(html, @"""trafficContactName"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string trafficContactPhone = Regex.Match(html, @"""trafficContactPhone"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string takeTrafficNo = Regex.Match(html, @"""takeTrafficNo"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string takeTrafficContactName = Regex.Match(html, @"""takeTrafficContactName"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string takeTrafficContactPhone = Regex.Match(html, @"""takeTrafficContactPhone"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                    MatchCollection goodsNames = Regex.Matches(html, @"""goodsName"":([\s\S]*?),");
                    MatchCollection hsCodes = Regex.Matches(html, @"""hsCode"":([\s\S]*?),");
                    MatchCollection batchNumbers = Regex.Matches(html, @"""batchNumber"":([\s\S]*?),");
                    MatchCollection specs = Regex.Matches(html, @"""spec"":([\s\S]*?),");
                    MatchCollection goodsTypeNames = Regex.Matches(html, @"""goodsTypeName"":([\s\S]*?),");
                    MatchCollection remainNumbers = Regex.Matches(html, @"""remainNumber"":([\s\S]*?),");
                    MatchCollection packageNumbers = Regex.Matches(html, @"""packageNumber"":([\s\S]*?),");
                    MatchCollection entryDates = Regex.Matches(html, @"""entryDate"":([\s\S]*?),");
                    MatchCollection portNames = Regex.Matches(html, @"""portName"":([\s\S]*?),");
                    string images = getimages(id.Groups[1].Value);


                    string createTimeInt = gettimestamp(createTime);
                    string expectedTimeInt = gettimestamp(expectedTime);
                    string noticeInTimeInt = gettimestamp(noticeInTime);
                    string noticeOutTimeInt = gettimestamp(noticeOutTime);

                    string sql = "INSERT INTO yygl (warehouseName," +
                            "appSeqNo," +
                            "companyName," +
                            "companyCreditCode," +
                            "companyContactName," +
                            "companyContactPhone," +
                            "supplierCompanyName," +
                            "portNo," +
                            "boxNo," +
                            "statusRemark," +
                            "createTime," +
                             "expectedTime," +
                            "noticeInTime," +
                            "noticeOutTime," +
                            "trafficNo," +
                             "trafficContactName," +
                            "trafficContactPhone," +
                            "takeTrafficNo," +
                            "takeTrafficContactName," +
                             "takeTrafficContactPhone," +
                             "createTimeInt," +
                            "expectedTimeInt," +
                            "noticeInTimeInt," +
                            "noticeOutTimeInt," +
                             "images," +
                            "uid)" +
                            "VALUES('" + warehouseName + " '," +
                            " '" + appSeqNo + " ', " +
                            " '" + companyName + " ', " +
                            "'" + companyCreditCode + " '," +
                            " '" + companyContactName + " '," +
                            " '" + companyContactPhone + " '," +
                            " '" + supplierCompanyName + " '," +
                            " '" + portNo + " '," +
                            " '" + boxNo + " '," +
                            " '" + statusRemark + " '," +
                            " '" + createTime + " '," +
                            " '" + expectedTime + " '," +
                            " '" + noticeInTime + " '," +
                            " '" + noticeOutTime + " '," +
                            " '" + trafficNo + " '," +
                            " '" + trafficContactName + " '," +
                            " '" + trafficContactPhone + " '," +
                            " '" + takeTrafficNo + " '," +
                            " '" + takeTrafficContactName + " '," +
                            " '" + takeTrafficContactPhone + " '," +
                              " '" + createTimeInt + " '," +
                               " '" + expectedTimeInt + " '," +
                            " '" + noticeInTimeInt + " '," +
                            " '" + noticeOutTimeInt + " '," +

                            " '" + images+ " '," +
                             " '" + id.Groups[1].Value + " ')";

                    for (int i = 0; i < goodsNames.Count; i++)
                    { 
                        
                        bool goodsinsert = insertGoods("yygl_goods",goodsNames[i].Groups[1].Value.Replace("\"", ""), hsCodes[i].Groups[1].Value.Replace("\"", ""), batchNumbers[i].Groups[1].Value.Replace("\"", ""), specs[i].Groups[1].Value.Replace("\"", ""), goodsTypeNames[i].Groups[1].Value.Replace("\"", ""), remainNumbers[i].Groups[1].Value.Replace("\"", ""), packageNumbers[i].Groups[1].Value.Replace("\"", ""), entryDates[i].Groups[1].Value.Replace("\"", ""), portNames[i].Groups[1].Value.Replace("\"", ""), id.Groups[1].Value);

                    }
                  
                        if (insert(sql) == true)
                        {
                            
                            getlogs(id.Groups[1].Value + "插入成功");
                        }
                        else
                        {
                            getlogs(id.Groups[1].Value + "插入失败");
                        }
                  

                }

                }

            
            catch (Exception)
            {

                throw;
            }
        }

        public void cnzy()
        {
            TRUNCATE("TRUNCATE TABLE cnzy_goods");
            TRUNCATE("TRUNCATE TABLE cnzy");
            try
            {
              
                    string url = "http://117.73.254.122:8099/api/storeApp/listForStoreIn?pageNum=1&pageSize=1000";
                    string ahtml = GetUrl(url, "utf-8");
                    Match strhtml = Regex.Match(ahtml, @"pageInfo([\s\S]*?)msg");
                    MatchCollection ids = Regex.Matches(strhtml.Groups[1].Value, @"""storeAppId"":""([\s\S]*?)""");

                    foreach (Match id in ids)
                    {
                        string aurl = "http://117.73.254.122:8099/api/storeApp/getById?id=" + id.Groups[1].Value;
                        string html = GetUrl(aurl, "utf-8");

                        string warehouseName = Regex.Match(html, @"""warehouseName"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string appSeqNo = Regex.Match(html, @"""appSeqNo"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string companyName = Regex.Match(html, @"""companyName"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string companyCreditCode = Regex.Match(html, @"""companyCreditCode"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string companyContactName = Regex.Match(html, @"""companyContactName"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string companyContactPhone = Regex.Match(html, @"""companyContactPhone"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string supplierCompanyName = Regex.Match(html, @"""supplierCompanyName"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string portNo = Regex.Match(html, @"""portNo"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string boxNo = Regex.Match(html, @"""boxNo"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string statusRemark = Regex.Match(html, @"""statusRemark"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string createTime = Regex.Match(html, @"""createTime"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string expectedTime = Regex.Match(html, @"""expectedTime"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string noticeInTime = Regex.Match(html, @"""noticeInTime"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string noticeOutTime = Regex.Match(html, @"""noticeOutTime"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string trafficNo = Regex.Match(html, @"""trafficNo"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string trafficContactName = Regex.Match(html, @"""trafficContactName"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string trafficContactPhone = Regex.Match(html, @"""trafficContactPhone"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string takeTrafficNo = Regex.Match(html, @"""takeTrafficNo"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string takeTrafficContactName = Regex.Match(html, @"""takeTrafficContactName"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string takeTrafficContactPhone = Regex.Match(html, @"""takeTrafficContactPhone"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");

                    string createTimeInt = gettimestamp(createTime);
                    string expectedTimeInt = gettimestamp(expectedTime);
                    string noticeInTimeInt = gettimestamp(noticeInTime);
                    string noticeOutTimeInt = gettimestamp(noticeOutTime);





                    MatchCollection goodsNames = Regex.Matches(html, @"""goodsName"":([\s\S]*?),");
                    MatchCollection hsCodes = Regex.Matches(html, @"""hsCode"":([\s\S]*?),");
                    MatchCollection batchNumbers = Regex.Matches(html, @"""batchNumber"":([\s\S]*?),");
                    MatchCollection specs = Regex.Matches(html, @"""spec"":([\s\S]*?),");
                    MatchCollection goodsTypeNames = Regex.Matches(html, @"""goodsTypeName"":([\s\S]*?),");
                    MatchCollection remainNumbers = Regex.Matches(html, @"""remainNumber"":([\s\S]*?),");
                    MatchCollection packageNumbers = Regex.Matches(html, @"""packageNumber"":([\s\S]*?),");
                    MatchCollection entryDates = Regex.Matches(html, @"""entryDate"":([\s\S]*?),");
                    MatchCollection portNames = Regex.Matches(html, @"""portName"":([\s\S]*?),");
                    string images = getimages(id.Groups[1].Value);
                    string sql = "INSERT INTO cnzy (warehouseName," +
                            "appSeqNo," +
                            "companyName," +
                            "companyCreditCode," +
                            "companyContactName," +
                            "companyContactPhone," +
                            "supplierCompanyName," +
                            "portNo," +
                            "boxNo," +
                            "statusRemark," +
                            "createTime," +
                             "expectedTime," +
                            "noticeInTime," +
                            "noticeOutTime," +
                            "trafficNo," +
                             "trafficContactName," +
                            "trafficContactPhone," +
                            "takeTrafficNo," +
                            "takeTrafficContactName," +
                             "takeTrafficContactPhone," +
                                 "createTimeInt," +
                            "expectedTimeInt," +
                            "noticeInTimeInt," +
                            "noticeOutTimeInt," +

                             "images," +
                            "uid)" +
                            "VALUES('" + warehouseName + " '," +
                            " '" + appSeqNo + " ', " +
                            " '" + companyName + " ', " +
                            "'" + companyCreditCode + " '," +
                            " '" + companyContactName + " '," +
                            " '" + companyContactPhone + " '," +
                            " '" + supplierCompanyName + " '," +
                            " '" + portNo + " '," +
                            " '" + boxNo + " '," +
                            " '" + statusRemark + " '," +
                            " '" + createTime + " '," +
                            " '" + expectedTime + " '," +
                            " '" + noticeInTime + " '," +
                            " '" + noticeOutTime + " '," +
                            " '" + trafficNo + " '," +
                            " '" + trafficContactName + " '," +
                            " '" + trafficContactPhone + " '," +
                            " '" + takeTrafficNo + " '," +
                            " '" + takeTrafficContactName + " '," +
                            " '" + takeTrafficContactPhone + " '," +
                              " '" + createTimeInt + " '," +
                               " '" + expectedTimeInt + " '," +
                            " '" + noticeInTimeInt + " '," +
                            " '" + noticeOutTimeInt + " '," +

                            " '" + images + " '," +
                             " '" + id.Groups[1].Value + " ')";

                    for (int i = 0; i < goodsNames.Count; i++)
                    {

                        bool goodsinsert = insertGoods("cnzy_goods",goodsNames[i].Groups[1].Value.Replace("\"", ""), hsCodes[i].Groups[1].Value.Replace("\"", ""), batchNumbers[i].Groups[1].Value.Replace("\"", ""), specs[i].Groups[1].Value.Replace("\"", ""), goodsTypeNames[i].Groups[1].Value.Replace("\"", ""), remainNumbers[i].Groups[1].Value.Replace("\"", ""), packageNumbers[i].Groups[1].Value.Replace("\"", ""), entryDates[i].Groups[1].Value.Replace("\"", ""), portNames[i].Groups[1].Value.Replace("\"", ""), id.Groups[1].Value);

                    }
                    if (insert(sql) == true)
                    {

                        getlogs(id.Groups[1].Value + "插入成功");
                    }
                    else
                    {
                        getlogs(id.Groups[1].Value + "插入失败");
                    }
                  

                }

                }

            
            catch (Exception)
            {

                throw;
            }
        }

        public void ccgl()
        {
           //TRUNCATE("TRUNCATE TABLE ccgl_goods");
            getuids();
            try
            {
                
                    string url = "http://117.73.254.122:8099/api/storeApp/listForStoreOut?pageNum=1&pageSize=1000";
                    string ahtml = GetUrl(url, "utf-8");
                    Match strhtml = Regex.Match(ahtml, @"pageInfo([\s\S]*?)msg");
                    MatchCollection ids = Regex.Matches(strhtml.Groups[1].Value, @"""storeAppId"":""([\s\S]*?)""");

                    foreach (Match id in ids)
                    {
                    if (uidlist.Contains(id.Groups[1].Value))
                    {
                        continue;
                    }
                    string aurl = "http://117.73.254.122:8099/api/storeApp/getById?id=" + id.Groups[1].Value;
                        string html = GetUrl(aurl, "utf-8");

                        string warehouseName = Regex.Match(html, @"""warehouseName"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string appSeqNo = Regex.Match(html, @"""appSeqNo"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string companyName = Regex.Match(html, @"""companyName"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string companyCreditCode = Regex.Match(html, @"""companyCreditCode"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string companyContactName = Regex.Match(html, @"""companyContactName"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string companyContactPhone = Regex.Match(html, @"""companyContactPhone"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string supplierCompanyName = Regex.Match(html, @"""supplierCompanyName"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string portNo = Regex.Match(html, @"""portNo"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string boxNo = Regex.Match(html, @"""boxNo"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string statusRemark = Regex.Match(html, @"""statusRemark"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string createTime = Regex.Match(html, @"""createTime"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string expectedTime = Regex.Match(html, @"""expectedTime"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string noticeInTime = Regex.Match(html, @"""noticeInTime"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string noticeOutTime = Regex.Match(html, @"""noticeOutTime"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string trafficNo = Regex.Match(html, @"""trafficNo"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string trafficContactName = Regex.Match(html, @"""trafficContactName"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string trafficContactPhone = Regex.Match(html, @"""trafficContactPhone"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string takeTrafficNo = Regex.Match(html, @"""takeTrafficNo"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string takeTrafficContactName = Regex.Match(html, @"""takeTrafficContactName"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string takeTrafficContactPhone = Regex.Match(html, @"""takeTrafficContactPhone"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                    MatchCollection goodsNames = Regex.Matches(html, @"""goodsName"":([\s\S]*?),");
                    MatchCollection hsCodes = Regex.Matches(html, @"""hsCode"":([\s\S]*?),");
                    MatchCollection batchNumbers = Regex.Matches(html, @"""batchNumber"":([\s\S]*?),");
                    MatchCollection specs = Regex.Matches(html, @"""spec"":([\s\S]*?),");
                    MatchCollection goodsTypeNames = Regex.Matches(html, @"""goodsTypeName"":([\s\S]*?),");
                    MatchCollection remainNumbers = Regex.Matches(html, @"""remainNumber"":([\s\S]*?),");
                    MatchCollection packageNumbers = Regex.Matches(html, @"""packageNumber"":([\s\S]*?),");
                    MatchCollection entryDates = Regex.Matches(html, @"""entryDate"":([\s\S]*?),");
                    MatchCollection portNames = Regex.Matches(html, @"""portName"":([\s\S]*?),");
                    string images = getimages(id.Groups[1].Value);

                    string createTimeInt = gettimestamp(createTime);
                    string expectedTimeInt = gettimestamp(expectedTime);
                    string noticeInTimeInt = gettimestamp(noticeInTime);
                    string noticeOutTimeInt = gettimestamp(noticeOutTime);


                    string sql = "INSERT INTO ccgl (warehouseName," +
                            "appSeqNo," +
                            "companyName," +
                            "companyCreditCode," +
                            "companyContactName," +
                            "companyContactPhone," +
                            "supplierCompanyName," +
                            "portNo," +
                            "boxNo," +
                            "statusRemark," +
                            "createTime," +
                             "expectedTime," +
                            "noticeInTime," +
                            "noticeOutTime," +
                            "trafficNo," +
                             "trafficContactName," +
                            "trafficContactPhone," +
                            "takeTrafficNo," +
                            "takeTrafficContactName," +
                             "takeTrafficContactPhone," +
                                 "createTimeInt," +
                            "expectedTimeInt," +
                            "noticeInTimeInt," +
                            "noticeOutTimeInt," +

                "images," +
                            "uid)" +
                            "VALUES('" + warehouseName + " '," +
                            " '" + appSeqNo + " ', " +
                            " '" + companyName + " ', " +
                            "'" + companyCreditCode + " '," +
                            " '" + companyContactName + " '," +
                            " '" + companyContactPhone + " '," +
                            " '" + supplierCompanyName + " '," +
                            " '" + portNo + " '," +
                            " '" + boxNo + " '," +
                            " '" + statusRemark + " '," +
                            " '" + createTime + " '," +
                            " '" + expectedTime + " '," +
                            " '" + noticeInTime + " '," +
                            " '" + noticeOutTime + " '," +
                            " '" + trafficNo + " '," +
                            " '" + trafficContactName + " '," +
                            " '" + trafficContactPhone + " '," +
                            " '" + takeTrafficNo + " '," +
                            " '" + takeTrafficContactName + " '," +
                            " '" + takeTrafficContactPhone + " '," +
                              " '" + createTimeInt + " '," +
                               " '" + expectedTimeInt + " '," +
                            " '" + noticeInTimeInt + " '," +
                            " '" + noticeOutTimeInt + " '," +

                  " '" + images + " '," +
                             " '" + id.Groups[1].Value + " ')";

                    for (int i = 0; i < goodsNames.Count; i++)
                    {

                        bool goodsinsert = insertGoods("ccgl_goods",goodsNames[i].Groups[1].Value.Replace("\"", ""), hsCodes[i].Groups[1].Value.Replace("\"", ""), batchNumbers[i].Groups[1].Value.Replace("\"", ""), specs[i].Groups[1].Value.Replace("\"", ""), goodsTypeNames[i].Groups[1].Value.Replace("\"", ""), remainNumbers[i].Groups[1].Value.Replace("\"", ""), packageNumbers[i].Groups[1].Value.Replace("\"", ""), entryDates[i].Groups[1].Value.Replace("\"", ""), portNames[i].Groups[1].Value.Replace("\"", ""), id.Groups[1].Value);

                    }
                    if (insert(sql) == true)
                    {

                        getlogs(id.Groups[1].Value + "插入成功");
                    }
                    else
                    {
                        getlogs(id.Groups[1].Value + "插入失败");
                    }
                   

                }

                }

            
            catch (Exception)
            {

                throw;
            }
        }



    }
}