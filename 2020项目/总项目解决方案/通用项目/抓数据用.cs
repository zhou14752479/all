using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        public static string GetUrl(string Url, string charset)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://servicewechat.com/wx734c1ad7b3562129/164/page-frame.html";
                //request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.12(0x17000c2c) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("openid:D-7ZGWvO2JerjFo_AUjVuOXQvaaLjsnT029qPSVy6_0");
               
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
        /// 大众点评评论
        /// </summary>
        public void dianpingpinglun()
        {
            string[] shopIds = {"66366387", "606519877", "125068110", "125885672", "510595484", "128486667", "129820986", "112606619", "129237543", "110626213", "129227238", "90958839", "130453671", "123152141", "97211570", "131518954", "114891056" };
            foreach (var item in shopIds)
            {

                for (int i = 0; i < 9999; i = i + 10)
                {
                    string url = "https://m.dianping.com/ugc/review/reviewlist?tagType=1&tag=%E5%85%A8%E9%83%A8&offset="+i+"&shopUuid="+ item + "&optimus_uuid=168b6aa9d06c8-87a2a9932defc-0-0-168b6aa9d06c8&optimus_platform=13&optimus_partner=203&optimus_risk_level=71&optimus_code=10&pullDown=false&reLoad=false";

                    string html = GetUrl(url, "utf-8");
                    
                    MatchCollection bodys = Regex.Matches(html, @"""reviewBody""([\s\S]*?)honour");
                    MatchCollection years = Regex.Matches(html, @"""lastTime"":""([\s\S]*?)-");
                    MatchCollection times = Regex.Matches(html, @"""lastTimeStr"":""([\s\S]*?)""");
                    MatchCollection stars = Regex.Matches(html, @"accurateStar"":([\s\S]*?),");

                    if (bodys.Count == 0)
                        break;

                    for (int j = 0; j < bodys.Count; j++)
                    {
                        MatchCollection reviews = Regex.Matches(bodys[j].Groups[1].Value, @"""text"":""([\s\S]*?)""");
                        StringBuilder sb = new StringBuilder();
                        for (int a = 0; a < reviews.Count; a++)
                        {
                            sb.Append(reviews[a].Groups[1].Value);
                        }

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(item);
                        lv1.SubItems.Add(sb.ToString());
                        lv1.SubItems.Add(years[j].Groups[1].Value+"年"+times[j].Groups[1].Value);
                        lv1.SubItems.Add(stars[j].Groups[1].Value);
                    }

                   
                    Thread.Sleep(1000);
                    
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
            //Thread thread = new Thread(new ThreadStart(run));
            //thread.Start();
            //Control.CheckForIllegalCrossThreadCalls = false;

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
    }
}
