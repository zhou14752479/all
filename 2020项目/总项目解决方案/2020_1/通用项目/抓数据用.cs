using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsharpHttpHelper;
using helper;


namespace 通用项目
{
    public partial class 抓数据用 : Form
    {
        public 抓数据用()
        {
            InitializeComponent();
        }

        bool zanting = true;
        public static string COOKIE = "";

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public string PostUrl(string url, string postData)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = "application/json";
            request.ContentLength = postData.Length;

            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.112 Safari/537.36";
            request.Headers.Add("Cookie", COOKIE);

            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(postData);
            sw.Flush();


            WebResponse response = request.GetResponse();
            Stream s = response.GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.GetEncoding("utf-8"));
            string html = sr.ReadToEnd();

            sw.Dispose();
            sw.Close();
            sr.Dispose();
            sr.Close();
            s.Dispose();
            s.Close();
            return html;
        }
        
        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long mTime = long.Parse(timeStamp+"0000" );
            TimeSpan toNow = new TimeSpan(mTime);
            return startTime.Add(toNow);

        }

        #endregion

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public string GetUrl(string Url, string charset)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.12(0x17000c2c) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie",textBox3.Text.Trim());
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("csrf-token: ajax:8788234651069068391");
                //headers.Add("x-li-page-instance: urn:li:page:d_flagship3_search_srp_top;HFHbmRoHQqO4j9gAsoJOoQ==");
                //headers.Add("x-li-track: {\"clientVersion\":\"1.6.7328.1\",\"osName\":\"web\",\"timezoneOffset\":8,\"deviceFormFactor\":\"DESKTOP\",\"mpName\":\"voyager - web\",\"displayDensity\":1,\"displayWidth\":1920,\"displayHeight\":1080}");
                //headers.Add("x-restli-protocol-version: 2.0.0");

                //添加头部
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
                ex.ToString();

            }
            return "";
        }
        #endregion

        public void qichacha()
        {
            StreamReader streamReader = new StreamReader(this.textBox1.Text,Encoding.GetEncoding("utf-8"));
            string text = streamReader.ReadToEnd();
            string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < array.Length; i++)
            {
                try
                {
                    string url = "https://xcx.qichacha.com/wxa/v1/base/advancedSearchNew?searchKey=" + System.Web.HttpUtility.UrlEncode(array[i].Trim())+"&searchIndex=&sortField=&isSortAsc=false&province=&cityCode=&countyCode=&industryCode=&subIndustryCode=&industryV3=&token=" + textBox2.Text + "&startDateBegin=&startDateEnd=&registCapiBegin=&registCapiEnd=&insuredCntStart=&insuredCntEnd=&coyType=&statusCode=&hasPhone=&hasMobilePhone=&hasEmail=&hasTM=&hasPatent=&hasSC=&hasShiXin=&hasFinance=&hasIPO=&pageIndex=1&searchType=0";

                    string html = method.GetUrl(url, "utf-8");

                    if (html.Contains("已失效"))
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(array[i]);
                        lv1.SubItems.Add("失效");
                        MessageBox.Show("已失效");
                    }

                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        Match tellit = Regex.Match(html, @"TelList"":""\[([\s\S]*?)\]");
                        MatchCollection tel = Regex.Matches(tellit.Groups[1].Value, @"t\\"":\\""([\s\S]*?)\\");

                        foreach (Match t in tel)
                        {
                            sb.Append(t.Groups[1].Value + "#");
                        }

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(array[i]);
                        lv1.SubItems.Add(sb.ToString());
                    }

                    while (zanting == false)
                    {
                        Application.DoEvents();//等待本次加载完毕才执行下次循环.
                    }

                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    // continue;
                }
               
            }

        }


        
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
           
                    for (int j = 0; j < 247; j++)
                    {
                        string url = "http://gs.amac.org.cn/amac-infodisc/api/pof/manager?rand=0.3952054052459051&page=" + j + "&size=100";

                        string html = PostUrl(url, "{}");
                        MatchCollection a1 = Regex.Matches(html, @"""officeProvince"":""([\s\S]*?)""");
                        MatchCollection a2 = Regex.Matches(html, @"""managerName"":""([\s\S]*?)""");
                        MatchCollection a3 = Regex.Matches(html, @"""artificialPersonName"":""([\s\S]*?)""");
                        MatchCollection a4 = Regex.Matches(html, @"""registerDate"":([\s\S]*?),");
                        MatchCollection a5 = Regex.Matches(html, @"""establishDate"":([\s\S]*?),");
                        MatchCollection a6 = Regex.Matches(html, @"""primaryInvestType"":""([\s\S]*?)""");

                        MatchCollection ids = Regex.Matches(html, @"""url"":""([\s\S]*?)""");

                        for (int a = 0; a < ids.Count; a++)

                        {

                    try
                    {
                        string strhtml = method.GetUrl("http://gs.amac.org.cn/amac-infodisc/res/pof/manager/" + ids[a].Groups[1].Value, "utf-8");
                        Match zhuce = Regex.Match(strhtml, @"注册资本\(万元\)\(人民币\)</td>([\s\S]*?)</td>");
                        Match bili = Regex.Match(strhtml, @"注册资本实缴比例</td>([\s\S]*?)</td>");

                        //FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "新文档.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                        //StreamWriter sw = new StreamWriter(fs1);
                        //string value = a1[a].Groups[1].Value + "#" + a2[a].Groups[1].Value + "#" + a3[a].Groups[1].Value + "#" + ConvertStringToDateTime(a4[a].Groups[1].Value).ToString() + "#" + ConvertStringToDateTime(a5[a].Groups[1].Value).ToString() + "#" + zhuce.Groups[1].Value.Replace("<td colspan=\"2\">", "").Trim() + "#" + bili.Groups[1].Value.Replace("<td colspan=\"2\">", "").Trim() +"#"+ a6[a].Groups[1].Value;
                        //sw.WriteLine(value);
                        //sw.Close();
                        //fs1.Close();

                        
                        //label1.Text = zhi.ToString();
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(a1[a].Groups[1].Value);
                        lv1.SubItems.Add(a2[a].Groups[1].Value);
                        lv1.SubItems.Add(a3[a].Groups[1].Value);
                        lv1.SubItems.Add(ConvertStringToDateTime(a4[a].Groups[1].Value).ToString());
                        lv1.SubItems.Add(ConvertStringToDateTime(a5[a].Groups[1].Value).ToString());
                        lv1.SubItems.Add(zhuce.Groups[1].Value.Replace("<td colspan=\"2\">", "").Trim());
                        lv1.SubItems.Add(bili.Groups[1].Value.Replace("<td colspan=\"2\">", "").Trim());
                        lv1.SubItems.Add(a6[a].Groups[1].Value);
                    }
                    catch 
                    {

                        continue;
                    }
                            

                            

                        }

                        
                     }
                }




        /// <summary>
        /// 阿里巴巴国际站
        /// </summary>
        public void alibaba()
        {

            for (int i= 1; i< 314; i++)
            {
                string url = "https://hz-mydata.alibaba.com/self/.json?action=CommonAction&iName=getVisitors&isVip=true&0.7587257170853245&ctoken=bpaxluccx0lf&dmtrack_pageid=da5dc07a0bba829d5e79669d1710a38dcbaf56abff&orderBy=&orderModel=&pageSize=15&pageNO="+i+"&statisticsType=day&selected=0&startDate=2020-02-21&endDate=2020-03-22&searchKeyword=&buyerRegion=&buyerCountry=&subMemberSeq=&isMcFb=false&isAtmFb=false&mailable=false&mailed=false&hasRemarks=false&statisticType=os&desTime=1585014429629";

                string html = GetUrl(url,"utf-8");
                MatchCollection a1s = Regex.Matches(html, @"""visitorId"":""([\s\S]*?)""");
                MatchCollection a2s = Regex.Matches(html, @"""buyerCountry"":""([\s\S]*?)""");
                MatchCollection a3s = Regex.Matches(html, @"""staySecond"":([\s\S]*?),");
                MatchCollection uids = Regex.Matches(html, @"""buyerCookieId"":""([\s\S]*?)""");
                MatchCollection startdates = Regex.Matches(html, @"""statDate"":""([\s\S]*?)""");



                for (int a = 0; a < a1s.Count; a++)

                {

                    try
                    {

                        string URL = "https://hz-mydata.alibaba.com/self/.json?action=CommonAction&iName=getVisitorDetails&0.676711856315128&ctoken=bpaxluccx0lf&dmtrack_pageid=da5dc07a0bba829d5e79669d1710a38dcbaf56abff&statisticsType=day&selected=0&startDate=2020-02-21&endDate=2020-03-22&orderBy=&orderModel=&pageSize=5&pageNO=1&buyerCookieId="+uids[a].Groups[1].Value+"&statDate="+startdates[a].Groups[1].Value;

                      
                        string strhtml = GetUrl(URL, "utf-8");

                       
                        Match time = Regex.Match(strhtml, @"""logTime"":""([\s\S]*?)""");
                        Match link = Regex.Match(strhtml, @"""realUrl"":""([\s\S]*?)""");


                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(a1s[a].Groups[1].Value);
                        lv1.SubItems.Add(a2s[a].Groups[1].Value);
                        lv1.SubItems.Add(a3s[a].Groups[1].Value);

                        lv1.SubItems.Add(time.Groups[1].Value);
                        lv1.SubItems.Add(link.Groups[1].Value);
                        while (zanting == false)
                        {
                            Application.DoEvents();//等待本次加载完毕才执行下次循环.
                        }
                    }
                    catch
                    {

                        continue;
                    }

                    Thread.Sleep(2000);


                }


            }
        }

        string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
        #region 去掉路径中非法字符
        public string removeValid(string illegal)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                illegal = illegal.Replace(c.ToString(), "");
            }
            return illegal;
        }

        #endregion

        /// <summary>
        /// 大众点评评论小程序
        /// </summary>
        public void dianpingpinglun()
        {
            StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.GetEncoding("utf-8"));
            string text = streamReader.ReadToEnd();
            string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < array.Length; i++)
            {
                textBox2.Text = "正在抓取第："+(i+1);

                for (int j = 0; j< 50000; j= j + 10)
                {
                    string url = "https://m.dianping.com/ugc/review/reviewlist?tagType=1&tag=%E5%85%A8%E9%83%A8&offset="+j+"&shopUuid="+array[i]+"&cx=WX__ver1.2.0_CCCC_naBZLlHKt6aF0VlHErokbdeHBic2wMrqGlcmuDDfpw%2BYxH6MbMlI%2BqAjIwxLodsdKnO6WHmnSL%2BIFPkB8yS2Sx9LXiK%2FFmx4NqAEoIL1uF7aEpMD34xET%2BekCnphNWE%2BuI6GXEjINnLDVrSYiJv3Gvpc5X3f88%2BP0MMznYGS%2BMEXp%2B1iP5r6b1e8yig7P9w7fghV2Ou2%2BeWDuyROKlMwTN7eMEsJk2NF50qnOrv7KF6aKHbN%2BFDyYTUrTgzziplQhfbf2PqfJ2Cu%2BZowHafsRXVYLEWTB2QK60zamFgVuzWivHjHaUyIhxngg2vnpLHl%2FCuxdSjoSZ%2BHRxI3g7ILZZClXs6%2FE2bnr6z8unhOW%2BatbYyK%2FI%2BjDKRjOusKig97fVlDu0jJ2mNwEVRt%2BqwR%2B71bGi81eF2uk%2BBpnT2JTIyE9u9N5sZDaT%2Bxy5SXImLAuQVgbTylt%2F4g0yYP9bCRi6g37IXsIxTArw%2Fs6oHSEYj5L%2F8SJv%2Fdy%2F7D8daz4S8mmym6ioPQ4YXPLp5zHDVzrLIW65yJQZn%2BKf9Srpqz6dtHOsm2q5fvGWSJmcqXX485VwynfNfX64zbN1ZhbM9WbABHjS2z8gfvoketSEBTDB1rywNuCMLnOtEJgVooHGGa95U%2FDXs1VVeCc9CVH4363ZPwSYsLrSPVboBXnDrQxYQnoJtZiv5gxkrcaxmEDUplzjptC2TC1L9Reec%2FLSHZVM%2FbibeiUYTrboKga2SDhJZgfYOoSAYNfE4zmwAoewgsi5SnkZ6UPuJycbBcfVlGbleSPZ5fdsGjhrHlL9zofJ9W4EzP%2FbwnaTuc%2Fe%2F5x%2F8Zh3kpU%2FDaLwbIb6936wYHThjP4Lq93P2WeSsLqUZnp%2FFhDjyvP35g0YvC61msUY9DXQqSxE20dKuTIaCR4v06D2qXD43gwAYi3zSl7akAjZNm%2BN1DWx0YzaQ1RdItMF4pX%2BX4%2B8QC%2BZ%2BGCC1sOYNISbo8wzUh6EH7bwQ8OA8uWJdWSDeyYyl0H9vxQCVU4tp8E9raUd3zatDRGkPg%2FWdevRzMrQlREj1%2Fy4oTabIdKd5DcSkiKcv6TB7CXpTJyf353wzAelLLyC5AAdhKL%2BLZ47t4vzPY%2B9car8TSzlavExO7OeNnOiCmz07yojfN65LLw959T3bKiknjt1Juuct6qLnJdc7LALpgXfjZFX%2F36ebCVi4RYH0MI5TzMz80rk5aCOVjROkEC61ffPYkwYXiIG%2Fr%2BuMl23W%2BH%2BG4feR99sbvQsp9gH8uMJ7ioz9pbG1LFIZ4OsEdy%2FsAa2qOpSBzGmlNhNCzUtGgkKIlqNdWtWcS0RXiQZWMSELSQkPGO40dOULx%2B3HjAADiseCd1NwjxUAcP76qLQ3A5xb5UmQCoWM5OYO%2BWObEXMl5OXq3PpKT0uuzFa8eVuOdjfdq6jEthi4n28hgpikAYrauX4LyPW%2Bxcvc8IKPy0CuwWBKMvfst4Ee8wOhgNpYV3Q6KO1RDaK4nv8f4Yqppf0RyjLsYHcj9XE6wg4%2BotUC9%2F8Oc3%2BLF9I%2FoCj2AgH4EcfMztmuHPXlub178yAudUGdEMKmWzaz2EgqcxZc9WrAIc%2B%2FphtX4cGqRCn1wOAPPikhlgmgE2CENldwglxGtQmGuq%2F9Ljq6lCJR%2BTxx8RNI6lN%2FCw7i6vS9MsQrGFAFeP1NqCEs4N7yz36ofJ0R%2FmvDrR10zQQ71iHGdr3l06s15JOuRZjXh0Rdo64Z9ZEcLJzeCqEpomZzS2GTn%2BCSE5ioRMZhUz49L5DuL0%2B8mmnfNVUBdHe%2B6N1SpITE78i%2BFCXi%2F6Ez%2FK6Zthyq6OG0Phk74Q%2F86VMNbR%2BpJ6yv4qwk9Xzx%2FxSbh%2FwHauihja5El8svq4LjFiYoWf%2Fg8pHKvipRDqf6mynSijrWrzBd6yA%2B5sCDYA7C%2BznEPJb5GZjFjFZHwS2lkEJ0%2BnHsRnO3DbZT93rPfU0dEIpU8dAHNNrLmvv3oTSCuQbe66qFfQenQWY97PJBpI8%2FLqLCFkQZN3WEfKSAc7oL5YaosHJFH1h7BG5Cy9yFR6%2FQlfanTdPMfiqKeP1U0PhmL%2F%2BPK8nulx6d2CIJV0vkY1a05eZ4cCzDyaAatoMz9P5C1q9zY%2Fo25lfYx2CHgA0g85X8s%2FFVHB1i35fg%2Fnqsbkn9z7GMdGtJtRoORa3ueReJZ0xZiW%2F8hHe0kzDcFVEoQ0XadPa9WGnO7YcHXTpaxGa5hAw%2FkHvG5JD9GiXxOqQ0D4R4KDRvfb7nSNHfgY4vBpT%2F6VYzHHzUM2ABX6REB3nhpU1PmNVmjGBM1X68jVrG%2FsqgFbwy4%2F9X8jjSHDt3uPNwrFn51jpjh%2FMchCmrbRUl5naNYMfBYWbXOJRCAeXsg9sSadn0QsDxnGYjdh%2BYxzemyWm64zSey0660yjLzmCxfXbfLi%2BhXN4D1gusq6r84Fk01yQLPNCt1b3vUpmQnRq%2BMtrOst%2B5%2FMdkWhym%2BBzQQU7hqcCmxj0Y3n3Ww4e7kmTALWbZT6r9aw8vNh48P8bSMI%2FgIRNHQ%2Fy%2ByyVAmEW7rFIAGz0hUWiC1jwhAt4joJayYjU0BEdYiAhUq9X3e8n1TJ7rE3Bi4S7rbDCK1aF1VO9Grxvhnw8%2BZRNVExz70k5gXMW8Z%2B%2FnJQ0Nw9JXIXF4KYckLyMjc%2BRoe8EnUK9FgFUR88BLgXNbVGohZP8EzyThfYUUNFuJjRHcMYG8bq3XCE76ljjrkF7gz%2BvDxLCpS5DoIkfJR7c7fequef%2FrBjuOXgJiCv33zUL5bmNu8wsVXB1qh9JVpIGUAmazLc9MroIJuaesHTUHCIoZrNyC7r5yy6uXbhq1glQNP9BZU9yluGOVFWte8L7yXVSwx0BVHNPb64vyE%2FhFzH9uVIpCrzAQAX%2B22s5Mv6Yl8kcCUH6tua%2FsUhony6PbTVV5eDb0hfI%2BRoF2%2BKxG0x3KPVhYT7x%2BnRFj%2FdQnpN0SBG%2FSLkd1CIs98U3M1kGQhaNLwgZxb%2BjkrzN3whugXDpcuO9F065g72hTXX5brt5nnQ0SYxJot%2FykRp8BJMrVK%2FG86Q7u4qQCezd5JSgEzOVVkZhx%2BebTFVtnZMpCnHUNrr6WAJ5M6ZqE5RPvAqTOFPORPr7vvcPvDifBN9kCK5d0s6qYD6WrAdN3f2RzmUjpK3iO4bb%2BsOoe9%2BJOGq7J6CVNMDucff3vc2ooaZ4JNs%2FIueCCKoACLcodUX%2BnmMKzMcjcPlXyo8z4qmu0oJxVwVKZmiE%2Be1BV%2FH2qwDtc8QdC0wY6G2jPspqKLXyMKcCsPQsCSbP%2Boi0zKdcGCBh1xTq7BQxqwKCPuFA%2FVCbDz7eXnWO3otv51HsQlL1Kqgfyvotfo99D8xtYWLuvvUSNu6%2B7svbO2NQSM141d498iQwhdPBCIH0GaiJxZQWYFHcTK%2Ba1tpuSDCi4m0g%2FRmFaFpS57rsAT1l1hanBLU6dn9wyZi1twh3BrIhLfy5TDURBNOVcGsGcMlRPllydJJjwzGo1SDzR2qZisDM%2Fph3Tob%2BT1XjZtPdxptPwHFTsA%2B8T4Ea3h5zhdENmStpJHmwTiEJzTCAJRRrsI%2FDMl0vxXWUBnE%2Fn8LmCJJpt4M5IiV8y9Q3oR22BPlWRgnloAy1zBu9pl0iRNQ1u6CZXKi3NTXrEFbBbloXndt5nfpYOrK9mVBZdHeBIwwK4cjTE%2Bfmbxlc56BSzWJv8UN8q2FeSRTTzSTytFXr7SJQriGz4ZezyGaUizTfH%2B6PhoxGovM6bwVXPqKlL0BxZDDoZF%2F4jH2omXvgXOJybgIh33xcZ9tJTX66i96FIXIqAVieNy2l%2Fgose7HoyYnTuG%2FtOnxtjhTr824dipCIH47Jr6noYo0VBqvVeQcSKBMnr7PPSHjN5zCC5yCDaZ9JyjOiCYKghvTkdMBplNYMBVUrK%2BnADo8%2BIXLp8QlTsneBCc8EQ7UlqeSC5NnE5D3PCQuU89KMZS7Aq9Qsf3NAGPMu4phfTdbp9i4ZUwUgEPwf53Dgi9HZHkHqoBpahZJH4%2BVqhDNe5DWxN96epnEFnkocQubp3sKuEXOV1uZCJiqq5PuXPh1Cu%2B%2Fj3K3Xy6hFiKJf4ubmXcCAeV5Zu1zuAt5A6SOIC80yDvgSpxypDDBUFCTJP19w54TAqQOwInSnH8jvyT%2BjilF5LbbLwrGm7Q4Glf1LpIWMWR2It6IPJLmCvzKkBTgtnBvVTJN34IoYV%2F5hip%2Basx4dXrA0Jg5g7zMjouhMHDi28QdMYxR7Dv%2BMlobjOWR2tFh125p%2FWUVxHfIJXl%2BkgbkEHE76N3YZYxLIk1JzdQJDUxldHZkRkDBvu2UaSxXaZVxerf858QBzBbsEGXwBS%2Fs0ImTSg8eoieq98KfmU7C9oyVQnX1Tpof1rMBgjedYCgvHLNkgitxAXtzH6iUOrth2lXqCLHEv402uMT7n5pOihtmlu7WBcrrFF8mbRxm9L%2FJxUw1kPaohRxZyX60%2FIzrpcsJ7A6x3l2Bhh3s93FsSz3%2FkX9OkSLrT9VW6SESBxjx4az6ZIL6oP1Xl%2BTjuTsKeFbEAP01Qfvv%2FOEu%2FaM4rTTxfjZ9e2f3gg0m5O7qzG2ex6%2FAwcVt%2BbTRWQ2CexpqcrQVGVLwP0iNg8ivf3iFJAQYZv32NkN%2BEV4dbsfUI%2BMDHeGdNiiGFKVJXQFfUIjGs9Qe8fDZ6o7FHkcHcceP3wwHb36KI5jikWMEHqfAqmwAwmURzklxzCnckktvDCrk6YcYlJQ3rNELbvzr7GR4mmN5MdrXIeWel9E%2B2BxzmMgcqppyPEGi1O1UPL6iMBcRB2pmVWy3SnvxayVX3CipLTo7XXO077HWATBe6jCygOxT5xl1%2F4U%2FnCiKodQWv1eV7e7ifXj5YAoC9BZhQfCb%2BpxplaRUM%2BB%2FGuzSDEStY2fjNP4MJWR7U1ce8z9OeZyNslM0kPLvHf%2F6cWE6KPXSd2ndo5Sv63ykAFrVUM8lR2OvJ1FeB40uUXaQFB7f5gMP5OYH6doPv6Ouu93wcC0SXodmPsWmkuIP7th062b%2FvaYJJ5vMd4l%2FGaJPhxRK2NdLtFFqpb7HNpCf0HKUMQXP%2BTGP0AWfJa9Hv3bfrVPmUyKoo3Faw2vN1KhuSfcob%2FJ90xL1R93j2x6tJC%2Fx%2FhNeTVvoXXrMIRMnQc%2FQJ9svcYm4zQA852LBfgurSWd9qmFq3Cka8%2BffXwvd5U04wBvUepMow%2BF36h5%2F%2FQOjzy%2BiIbS6VQ7ty3WL6IrTB5SKbAzJIsc2KH%2FEbRygIaYAZG4nxkUKC1HjL4fYXwGFjBq3hNzq7gDgkR8v4C5aDiYpNS2aEL%2BLSZT9WM%2FGt4iJBd%2FOV1uBfZAZ2rF7L1GvJtyA710JpHlshMNgmhQnWy5EQONSl4VypkFnpQbDBegq1uPq03m04LmhPXv0gY4k8%2Bk5zG3IjU5sAvy7ENM7MEs9NrQsMqnZXy8FYUedtYs525LI5rBOA%3D%3D&mtsiReferrer=%252Fpackages%252Fugc%252Fpages%252Freviewlist%252Freviewlist%253Fmsource%253Dwxappmain%2526shopUuid%253DiRhUQNb0YWXAsHTq%2526tagType%253D1%2526tag%253D%2525E5%252585%2525A8%2525E9%252583%2525A8&_token=eJxNl9eO40xzhu9lTzkAc%252FoBHzCKOZMSafhAzDmLyfC9mzs7XwAE8OncXa%252B6qvp%252Ff81y%252Bus%252FMAR9%252FVqXGwiIpmEMIzAaRb9%252BJf%252BqwwkEwuGvX%252FEc8L%252F%252B898YjH0RFPU%252Fvyucu%252FzfMIIhXwhEYP%252Fz9W9GsPv3u5d8d%252Fo1vpPmXWQL%252BCkScPymOduqbG%252BrZf0X%252Fvr69ac1zdZ31f58ft3zdN7veVAU%252BUJh6O6GIuQXiiDfBH%252BhKPWbYPwLxbFvgr5QEv9N0F1HE9%252BEfmEw%252FhehfxOO%252FEUE%252FRdR6F9EYz%252BEQ78Joel7jd9jEZy6Z4a%252FCfrCMPIvov8Q8oWjv2dBqHt%252FOPRdR95130ShXyhB%252FYzAie%252F56N87%252FR5BQz87%252FSaU%252BIvwb8KJL5T6ptvaGIF8E%252F2F4%252FT3zPe6EP7bZM1vk93f97fpEIL6ommU%252Bvpt698dSex3Gf27TMBfMIL83Y6SyBcMI%252FQ%252FZfJuh3%252Fa72nXfyvyBf%252BjyQ9%252Fq%252FKH%252F%252Bjyw9%252FK%252FOE%252F2vzwtzr%252FMPovxpF%252FmKD%252F4duKfzON%252Fc23Ut%252F8o9Uf%252FqPW17%252F0%252Bofpv%252Fhbsz%252F8R7Wf%252Bm%252Fdfuq%252Flft77K3dz1rf6v0w9PdZfhT8h%252FEf%252FqPiH%252F6j4w9%252FK%252Fmz1reWN9%252FmXqqi%252F%252FWfX5lypCmUmLR%252FnLgICbRGlYo0oQAbQ%252F0AAk26Q4SGoihDdto8h5oZPil4dj3B4JnGEcWm2KdquPQNaz8gTe0kCILu8QQMwM4xC7g25LNhlPIAPvRswoLcKKy2bzOsBrgcG0bZXo9PU9JGuVMvCnsJBnSGb%252Bcx74wGnfNr9kG87ba6BGt1B%252Bqp4JS35C%252B09tGylcWQR1WYBhJ%252Bap4Zzf6UI07T2YGuVoBhnSQzEgQG2Iy1Oa7xn4ufpB4eou%252FS8WCbwPfdgiK1YpjlrKVMV9hkLyXmgekxF5yqyAHjqK5qpZHqQhQ8LjGlnVt9nXj4O4bPzHXej0Vt0nAYsFU%252Bc9q14cYvINPz2eZFVRcM0ESPxJ84n%252FUKct5nG5T8h8m0uRmNM9HlEsKyz1GpkAMpix6WfkKSCY4HkQkK2UiSj6mRzCSNG%252BmTWSb6is4QYpG4eK6u8LRCArOMXfYbyUmqKCGObb7YSKZrBzMCmXvC%252BIUkyPH2abF55B7FxP6kj9VL4AtyEXfT1tURFtlddhWJJfmL4B7TeO1cnjLdtgQUVkhoQX5K%252FVProZ0GRI3SfCcHtGA%252F%252B5di2rA9TKEbJ%252F2mZ4MQ60n3EbQnxrfqJxs0nMFlqnWTDW81nmIOQYcbRByeT%252BtjbheTb1O15kxP%252B%252BaBsfPbS9MAz%252BQVbK8rLDiYyKA1DUpkV2zs4gpgiVxS17nsMDyv%252Bvj3f2ss9nk9ydNvA9xrkeOzazPlnpQ1pngih0DeR%252BwcKmNT%252BePwEnJc1w1%252BULkTKT4N%252F%252FDthzUnk2G8YGh2uQSRrD7YKoPsDZuOG4L6zAvsAjiLWQie0yuOH5xVoK%252B3Gg46LEI4X%252Bx7nKoV5Rqb52arW3KvmX5i6mRZOK86T3sOTfjSA%252FEK4A%252F%252FQfSnEB%252Fo1BDBQq%252BLpWaQBmPzoa9As7Zk%252B5ryfAq8UTvflDiWWQjI6SOoGICB3SSEEQqj2aEFxqaUIbc9HdJ4rmXWaHUwyyvKoHku1Ykdi10H5BCkHdwwmvXASVdID9Zsv%252BgjtFUjMT2owdhrKYsmezZ6bhOdWrE9mvbNTowDajAeW4L9ykPRFi6KPn9OsUr2SngIfCSxDC8sOYucySl6WTqZG%252FyGVPvM3XOyaV4KKBgkR4ryHYuneihL1wR7WKTH9FC3FThQhSQJVWkQSvnGVykM1ytaiu%252BHdO6nv7pKmp69Dibz%252BjgbkA2bYu09FFZFIdvasyVet5Po787PVWetJJYlb83VXRs0sRkv9I4ErPfhwZstyUt0XWsS0G%252BrN0uAo4iRrjS2s1dnkDlRmUMmblUjU9Z4U33BQbjOQlhUpTOKlx0KWGNjby4jwMNf%252BTdm9ygXzD20dyDrwpoKTB%252F1sfu3o2B3oHBoZpML2Q1K3%252FKe6PxGoOOl936orq2uXUgvTKfmDgdtfB4v4fh4sr726dq%252BHv10GXP%252FQonnBOEOoOfWBi5wcjJNbILYe544Mu1SRdov1SJxgCHqRC%252BuPGaerx2IA7QmvGfmWuUAnYCCVmDHXhCdMxYtSICEu7LnECCrfyY%252BmKXeTma8tJ0XS1FzrIvEfZfnmXUGd5rIfuwepVGXvCcBi4YaXeSHY2c5LPoS6adO4SdyPcpBxa1X1F6iyiDDCy5KiIJi%252FhyF4XNvAytxpIe1SL3aAhOxadhDY6Tr4j16WMh9mmLGdBfmREvpFC445sUqkVMEI4lTLH93AFz7PNlhB1Pj1bhD%252BzlNHZxIvRkehWTSJVx0mo2h%252B5pJy%252FvsJnCdJsYVBGoHNmIG2j4iGxkTRytA5Ndopja%252FbVUvBXKdsVqnxXLtjSFqvKqnI9iLZpSpifCcl4uED6h%252Bgss8f189k9cQxEtUQzP9GWksgf1YE47ylTHtpYJTsqZGtZxawR1jnmFEPADJd05nYa%252FytUe9qn%252BsKK%252BN6ABXpsTE%252FMpLf6aMNtEKnisNOKJs130XnMMdH1sQcVsGVDov40qGxCcaPUI%252BUIRQqUaG2icqusKkDyDAIAvyHcKAaOoD0ccsmxLvenl5D1BUxglQgtJZiDO9smt3oUa%252FgAIh8zF9XWEvFTFjYIT1UGQebR87zLovrI2WI6iAl%252BptLOtzTt9eRwGqIylKA7EGQMt8VM2w8PBYumkBei4hMhdHkUfLCvIaIksw5Kf5XkUHCarSfqGOSZpX6Gg0N8Wt7jQYIVUvT5k%252BFW8kjQpIiX0BlY2xyOt81y28PhD3esc7SElAy7K7qA3Uu96wJsEcGy507FOUuqwh8oLBJOBGZdNj52vtGfP1KVCmSgSS2ganrvBw4tS9MSaGCpjoIzyOZFElJVlJPyYU169bNIJva1kNwUqY40y8AR%252FU%252FhyfFXGHN%252F%252FSHaZ77JOHCvMwXw5wcpVnI7QZVz37BkLOBDs%252FUZ796L4pC1sMKqr4d3xcbdXgnylKglJEllmXhEaCzk2bH3vNl4NdL2YZXhe%252B8M9wX1ll3jmRXRNhKtLVrbIeDjkm9Tt0PrfTlMYInzNV3gE0gSfOgdNzHwcyxxCjcppYcVYKAtXCPA6vDNZH3AF2HT1AyvJHp6MzMPg0a%252FlJSFD0HM0hZFllc2HG%252BBRPy%252BdnMHw56Y7FChYXFdxOrFjqDn7B0q2UPmZ33hDKoMmUNQmsLcIwmbFZ2%252FNOBfbtvPbzNSsfU%252FwMdpFCGSCKTxvN5AsAHcgPp6dXXbPxmPRF7X2Dfe%252F%252Bgd%252B7ZR6Zgty5lP%252F5DIsia7BHotibR4i1S6czhU1XeZZ0VXrH1BljTrDMOrESEkhy4Gsx%252B5IUI%252FMJXjgE05pdCO0S%252BlXatomEBcpRSvoqgkdLTezCOqsrXcMa2Jwf6BWpmDm2Iwx%252BjsPt%252FGCkOoQekYa2orkFgho1JNaqLvP4JGrpykUGgNPM0yvOnl4pZwtQVcX0BxaofmJ9EoDnPjryViH8OZA989SUd63qjyAOaeupNkZKaAMBrJg5FlL6hNlTVzTvvbGzUAD66W79Kj3Mqp8ax41JqzlH6DreNZZ8Ms2yj6N8VvQOp5%252FOic3Du0BafkEcFS%252BvVsCAMGMzg7IfS9e1rBQCTFtpM7TnhadcBLWFHlMUpxzGyiHBgZIeW3Mtzq588hox%252BPuivkq%252BYCOaS989FTSGwJjIc2C3YFY507Lkh9Oky57uWXQY59NuIdolJPP5rFWHgjGBOAnK8%252BdzAm4ZVnhNhRmcBrsnzbqgVrtwssav19J9ZHO9AHUdN86cEGTGe6ZnidMSvt8qKwhehIfyZohPwJwQrN0UqiBMjWCH2Rcwg65lJHogr%252Fwl5tVJVJiduLeb3J6ne90B2gbq%252BenieBspNODPQov2ctxDQrgIPkixnIBCjnKgi7hFO7xad6Bn1rVXu5IB%252FUKbHCUJ2ZlqIr%252FNulg2OP8YxyGebmdzII23A33UWkupM8SDLseog1saEC6%252FOlGe8dFJumO7WAAGw9r7A2POpb2ovhWRqxFfjlDKbwS9hk9fhkgRrXCPHHM3sv5u0I8RV3px4VMws2cN2YL9HR1QxexRcs50I4Rkzl%252Fzquyk1tHovGiRj78MM7t6xxDANAO9oAYt6JXrBSPJJLTTFTJhbIhyJhQ49QsNPku0VzasuZOUUyrF3VGueTc6T%252BZaQcEIwweyZMpcXN2e0yWoCEsY8HUm9WSND4u27idAfORGMn3cCXnxAHlsi%252FnuT84zqubKlnwCzvQF5xEx2i0CpCnbznWUmWa%252FvQt0DBmpvJ%252F2T7ChSTq8bxb5YBRX4uz2jd29OSAuyFmfeDx%252FceY8J%252BE1meanK7TLsYEkzXsYEXXOgYrUDLIk1VGu5d6VtK6SYjv8k5hp08hDj6KYLaPZi7%252Be5jvRpT15lrdv7ESC4DpNGS3sNtOdPuLOQQcDlDF1fJ%252F2M7%252BU7BG9oTtS3I6qRx4CcZH5%252BvSoBp7sxONS79rSGO5FygxSMSDjt%252F2qsA1tHBXMYthli2ruO3w0C6jaQs1%252Bzk5W0gWJb814v6SWNhJH7k5MAizLV1GZRA25OgMPtNOVyyoSlMB%252BKl2hUpBILMqEFKPbK8dE27uGXmi9xRmyirLWXvkV2%252BHhB4iaDEWjIqavc%252FV6GWkYBrR6vrQGpu5UgpLzusHLZw%252FGrx2K3kFEZ%252B2tGGpRQmGjVGAbj3KkBpvp4S5WyPhYtHZZht2N6bxqE75OJSZLnYrUrZ6QYRmNlGiS1tl3DGdWdQbfZmjZvO5J7IgGUu7MfYQmx6dJap8m2URPrR9lPHjYJjdmscFsNM5Eelf1jX9oj1jfVjlCPY%252BvJIq1GCdBpkvTSrgUQwMk9hVR5fejxoXbMb6fnSKZgxd5JMRZPFnQCIv0nJTUlOJAupm33pZf1HXNHH323czSclBLsxX20BKhwaNDNusotHh0gsPyQfg9hg9Ifzdb%252BIKf2xK%252F7MhNfe664%252F1lBsAazvQo0cVOwb59ROHACSoQP%252FwKeVAb34aST7QZ8JrMtF4nG7BjfIx9B7VJpcd4x0N6EnjB%252Bx3dw%252F2Z78SeoS9unBDGSNvXmwMVXRYwtCKMd8C3cY6KEQ4C%252BGVmRFJ3ucWAgkfvhGdqlba%252Bnf3sotxYIUGNj1Ck9yLK%252BaSqpm5sy8GtFqkHvIGw%252B0UPXTpz5xJ%252BcvO6FPVcjy2anVM0A1Ei0e2wCSABY3Uel%252FunOpOHbCdlk5mAdYxEjw48qGnRZ7mf165tbilDECwBHHnyTFrfRHOqZGxB%252FVBEhK%252BqvrgGXZQxDJSPAdC6rarDMnf7BHwFA6ZvmD18MjJB2%252FkQZrMWIl0A6RYnM2V9VmrPPFl2aYFEyQeb95hjoeOLxPF%252BYKremQb5AQr6Z8Zs39iFMzjixTYH%252FHVsZdWv2bL65ihwoKYWh0oMuEvaTpC8Taw0xEvHhJLhNYhAITHhC9zX1Ov9dhwOtm14o9hgHQcA4dStCzuhdMhaxs990DYXwx27NfY7pgLHxG2Kp1O9dZ%252BPIe%252FbkGtX2r3iLLe0FHEUR9yeUz2d%252FUDk3Wc0nWJH%252BMqZiYe7CWvWDoxgRQBLWGrekt1iu2PhAXsMtLcL56L91C5B2nOYRS0T7TjBfL6fedLpxQ4KQq%252B%252BqKjmCK3tP%252BOVgXW6n3xXnNbrLRqfHAIw7s46Yn3aD9fosJ6oXvTStrt2WXmmYMfCcNAsEaDKfaD0jpLDM5rKvAS9clbZDGgtoSZ7mF2dCKi6l6yINOurl33f9aO%252FODQymB7DHJhi0MnAWWtK1Gp%252FanaY5Mocy00VZI80toRkFKy26SmvMR8zdPK9xqsOsJDeCpWI6mQ75LjMlLlvdwIeffNkaLV6zsFK6GU0kdsw17rqdrEOp9zyCeO0K9i%252Bc9hLS30BHgAodhIG2XYJQd4%252B%252F8AbwxTWfNxP5FDt6PmISxh5a44Yd8rLzJnHm7T2otfVnQ7sO3OCdJp9EhmqD4VIxmHMCRG0TogBb1dH86bKJS3MMU8%252FenX3%252F%252F9gkeWd5pceQYyrqDBfyMgIInnIPtwAqha9p%252B2YIob3nlXk%252Bj7v5GbpnrIyGBxRn0HTOLr2kGo0b94s9VnrMXAyoa3OEx%252FiGNOeuRjgMNMfH0FBIfciXFUZGJMALkN%252B7aafToeDOBflAHB94pXSBH6TBgUB53DpsqUbkX3%252BXsylXIds26kPwoIeGUtF561HbyJ%252BhK1TYdNzkTLsvrnlusgeUHdKXcgEeAeVMX8dtdfkfs0YUMkgSHutV1A%252FTGkf9FzVcplCJF2E2QCMeNDqLxjktzowfSeen9MBgsNbTnAFdoNg2QDRifJ7qwIAiV3qvSTkDpJUOpjq7iXZW3zRPJCtJt63creJG6dJ9yN0z70c0wyIXBEkVJ0UsIH4CZ0IjXjnvHU5ePDbmRN8TjwwWQNMmgUzi%252FUtgKF5qgBKcJ%252BJZlnvIFNA9%252BFzK%252BP1HKFh6gq2kwWxvo8MVJJ2SLA3KiJwI%252B9I28wBFjzWqCNAMhOR3AFPqQSPhM5rkGTP99AKhWsOmVxb46tqTrtUI0fhQJUUgRIF0RzfgHdNkv2IbJR%252FxvS9Fixuk49blAhWIGiBFAiQNBDfMIMgToIgmYMX%252BQIBmJfAIgcbsGiBNrX%252F679%252B%252Fd%252F%252FAxUShbA%253D&optimus_uuid=168b6aa9d06c8-87a2a9932defc-0-0-168b6aa9d06c8&optimus_platform=13&optimus_partner=203&optimus_risk_level=71&optimus_code=10&pullDown=false&reLoad=false";
                   
                    string html = GetUrl(url, "utf-8");
                    MatchCollection names = Regex.Matches(html, @"""userNickName"":""([\s\S]*?)""");
                   // MatchCollection levels = Regex.Matches(html, @"roundlv([\s\S]*?)\.");
                    MatchCollection bodys = Regex.Matches(html, @"reviewBody"":\{""node"":""root"",""children"":\[([\s\S]*?)honour");
                  
                    MatchCollection times = Regex.Matches(html, @"""addTime"":""([\s\S]*?)""");
                    MatchCollection stars = Regex.Matches(html, @"accurateStar"":([\s\S]*?),");
                    MatchCollection prices = Regex.Matches(html, @"avgPrice"":([\s\S]*?),");
                    if (times.Count == 0)
                    {
                       // method.DataTableToExcelName(method.listViewToDataTable(this.listView1), array[i]+".xlsx", true);
                       // Thread.Sleep(5000);
                       //listView1.Items.Clear();
                       // Thread.Sleep(5000);
                        textBox4.Text += "第：" + (i+1) + "个店 页码：" + j+"\r\n";
                       
                        break;
                    }

                    for (int a = 0; a < times.Count; a++)
                    {
                      

                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                            lv1.SubItems.Add(array[i]);
                            try
                            {
                                lv1.SubItems.Add(names[a].Groups[1].Value);
                            }
                            catch (Exception)
                            {

                                lv1.SubItems.Add("未填写");
                            }

                            //try
                            //{
                            //    lv1.SubItems.Add(levels[a].Groups[1].Value);
                            //}
                            //catch (Exception)
                            //{
                            //    lv1.SubItems.Add("未填写");

                            //}

                            try
                            {
                                lv1.SubItems.Add(stars[a].Groups[1].Value);
                            }
                            catch (Exception)
                            {

                                lv1.SubItems.Add("未填写");
                            }

                            try
                            {
                                lv1.SubItems.Add(prices[a].Groups[1].Value);
                            }
                            catch (Exception)
                            {

                                lv1.SubItems.Add("未填写");
                            }
                            try
                            {
                                lv1.SubItems.Add(Convert.ToDateTime(times[a].Groups[1].Value.Replace("T", " ").Replace(".000Z", "")).AddHours(8).ToString("yyyy-MM-dd HH:mm"));
                            }
                            catch (Exception)
                            {

                                lv1.SubItems.Add((times[a].Groups[1].Value.Replace("T", " ").Replace(".000Z", "")));
                            }


                        try
                        {
                            lv1.SubItems.Add(Regex.Replace(bodys[a].Groups[1].Value, "<[^>]+>", "").Replace("{\"type\":\"text\",\"text\":\"", "").Replace("{\"type\":\"node\",\"name\":\"br\"}", "").Replace("\"}]},\"", "").Replace("{\"type\":\"node\",\"name\":\"img\",\"attrs\":{\"class\":\"emoji-img\",", ""));
                        }
                        catch (Exception)
                        {

                            lv1.SubItems.Add("未填写");
                        }
                        }
                    

                   
                   // Thread.Sleep(1000);
                    
                }

            }
        }

       

        /// <summary>
        /// 抓数据
        /// </summary>
        public void zhuashuju1()
        {

            StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
            string text = streamReader.ReadToEnd();
            MatchCollection shis = Regex.Matches(text, @"卷([\s\S]*?)卷");

            for (int i = 0; i < shis.Count; i++)
            {

                MessageBox.Show(shis[i].Groups[1].Value);
            }


        }


        /// <summary>
        /// 抓数据
        /// </summary>
        public void zhuashuju()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
            string url = "https://www.icauto.com.cn/baike/";
            string html = GetUrl(url, "utf-8");
            Match  ahtml = Regex.Match(html, @"<div class=""carleft"">([\s\S]*?)id=""dMark""></div>");
            MatchCollection urls = Regex.Matches(ahtml.Groups[1].Value, @"<li><a href=""([\s\S]*?)""");
            for (int i = 0; i < urls.Count; i++)
            {
                string bhtml = GetUrl(urls[i].Groups[1].Value, "utf-8");
                MatchCollection burls = Regex.Matches(bhtml, @"<div class=""carbk-title"">([\s\S]*?)href=""([\s\S]*?)""");
                for (int j= 0; j < burls.Count; j++)
                {
                    string articlehtml= GetUrl(burls[i].Groups[2].Value, "utf-8");
                    Match title = Regex.Match(articlehtml, @"<title>([\s\S]*?)_");
                    Match body = Regex.Match(articlehtml, @"<div class=""article-body-y"">([\s\S]*?)</div>");
                    Match item = Regex.Match(articlehtml, @"<div class=""position"">([\s\S]*?)</span>");

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(title.Groups[1].Value);
                    lv1.SubItems.Add(Regex.Replace(item.Groups[1].Value, "<[^>]+>", ""));
                    FileStream fs1 = new FileStream(path + removeValid(title.Groups[1].Value)+".docx", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine(Regex.Replace(body.Groups[1].Value, "<[^>]+>","") );
                    sw.Close();
                    fs1.Close();
                }
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {


            Thread thread = new Thread(new ThreadStart(dianpingpinglun));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.expotTxt(listView1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            method.ListViewToCSV(listView1,true);

        }

        private void button7_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void 抓数据用_Load(object sender, EventArgs e)
        {

        }
        OpenFileDialog Ofile = new OpenFileDialog();


        DataSet ds = new DataSet();


        private string GetHttp20200621091830(string url)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = url,
                Method = "GET",
                Host = "www.linkedin.com",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36",
                Referer = "https://www.linkedin.com/in/ashmalvarado/",
                Cookie =textBox3.Text.Trim(),
	};
       
	item.Header.Add("csrf-token","ajax:8788234651069068391");
	item.Header.Add("x-restli-protocol-version","2.0.0");
	item.Header.Add("x-li-lang","zh_CN");
	item.Header.Add("x-li-page-instance","urn:li:page:d_UNKNOWN_ROUTE_search.error;VgBxhDrUSQaSPqakS+Gk9Q==");
	
	item.Header.Add("Sec-Fetch-Site","same-origin");
	item.Header.Add("Sec-Fetch-Mode","cors");
	item.Header.Add("Sec-Fetch-Dest","empty");
	item.Header.Add("Accept-Encoding","gzip, deflate, br");
	item.Header.Add("Accept-Language","zh-CN,zh;q=0.9");
	HttpResult result = http.GetHtml(item);
        string html = result.Html;
	return html;
}





    #region  领英
    public void  lingying()
        {
            try
            {



                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {

                    
                    string title = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    string name = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    string key = name + " " + title;

                    
                   // string url = "https://www.linkedin.com/search/results/all/?keywords="+ key.Replace(" ", "%20") + "&origin=GLOBAL_SEARCH_HEADER";

                    string url = "https://www.linkedin.com/voyager/api/typeahead/hitsV2?keywords=" + key.Replace(" ", "%20") + "&origin=GLOBAL_SEARCH_HEADER&q=blended";
                    string html = GetHttp20200621091830(url);
                    textBox3.Text = html;
                    return;
                    Match a1 = Regex.Match(html, @"dms/image/([\s\S]*?)/");
                    Match a2 = Regex.Match(html, @"beta&amp;t&#61;([\s\S]*?)&");

                    string picurl = "https://media.licdn.cn/dms/image/"+ a1.Groups[1].Value + "/profile-displayphoto-shrink_100_100/0?e=1597881600&v=beta&t="+ a2.Groups[1].Value;

                    if (a1.Groups[1].Value!="")
                    {
                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());

                        listViewItem.SubItems.Add(name);
                        listViewItem.SubItems.Add(title);
                        listViewItem.SubItems.Add(picurl);
                    }
                    else
                    {

                        string url2 = "https://www.linkedin.com/voyager/api/typeahead/hitsV2?keywords=" + key.Replace(" ", "%20") + "&origin=GLOBAL_SEARCH_HEADER&q=blended";

                        string html2 = GetHttp20200621091830(url2);

                        Match a11 = Regex.Match(html2, @"dms/image/([\s\S]*?)/");
                        Match a22 = Regex.Match(html2, @"beta&amp;t&#61;([\s\S]*?)&");

                        
                        string picurl2 = "https://media.licdn.cn/dms/image/" + a11.Groups[1].Value + "/profile-displayphoto-shrink_100_100/0?e=1597881600&v=beta&t=" + a22.Groups[1].Value;

                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());

                        listViewItem.SubItems.Add(name);
                        listViewItem.SubItems.Add(title);
                        listViewItem.SubItems.Add(picurl2);

                    }

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    

                }

                MessageBox.Show("采集完成");

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }





        #endregion

        private void button9_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();


            this.ds.Tables.Clear();
            this.Ofile.FileName = "";
            this.dataGridView1.DataSource = "";
            this.Ofile.ShowDialog();
            string fileName = this.Ofile.FileName;
           

                    string connectionString = " Provider = Microsoft.Jet.OLEDB.4.0 ; Data Source = " + fileName + "; Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                    OleDbConnection oleDbConnection = new OleDbConnection(connectionString);
                    oleDbConnection.Open();
                    DataTable oleDbSchemaTable = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[]
                    {
                    null,
                    null,
                    null,
                    "TABLE"
                    });
                    string str = oleDbSchemaTable.Rows[0]["TABLE_NAME"].ToString();
                    string selectCommandText = "select * from [" + str + "]";
                    OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(selectCommandText, oleDbConnection);
                    oleDbDataAdapter.Fill(this.ds, "temp");
                    oleDbConnection.Close();
                    this.dataGridView1.DataSource = this.ds.Tables[0];

                    string csvDir = openFileDialog1.FileName.ToString();
            
        }

        private void 复制网址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(listView1.SelectedItems[0].SubItems[3].Text);
        }

        private void 抓数据用_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
