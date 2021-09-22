using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 启动程序
{
    public partial class 票据网 : Form
    {

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                WebHeaderCollection headers = request.Headers;
                headers.Add("authorization: Bearer "+ token);
                request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                //request.ContentLength = postData.Length;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
              
                request.Referer = "https://www.tcpjw.com/B2BHall";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                string html = "";
                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding("utf-8"));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;
              
               
              
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion


        public string posturl2(string url,string body)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.KeepAlive = true;
            request.Headers.Add("v", @"3.95");
            request.Headers.Set(HttpRequestHeader.Authorization, "Bearer "+token);
            request.ContentType = "application/json";
            request.Headers.Add("sec-ch-ua-mobile", @"?0");
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36";
            request.Headers.Add("OriginV", @"3.95");
            request.Headers.Add("sec-ch-ua", @"""Chromium"";v=""92"", "" Not A;Brand"";v=""99"", ""Google Chrome"";v=""92""");
            request.Accept = "*/*";
            request.Headers.Add("Origin", @"https://www.tcpjw.com");
            request.Headers.Add("Cookie", cookie);
            request.Headers.Add("Sec-Fetch-Site", @"same-origin");
            request.Headers.Add("Sec-Fetch-Mode", @"cors");
            request.Headers.Add("Sec-Fetch-Dest", @"empty");
            request.Referer = "https://www.tcpjw.com/B2BHall/";
            request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh,sq;q=0.9,zh-CN;q=0.8,oc;q=0.7,de;q=0.6,en;q=0.5");
            request.Headers.Set(HttpRequestHeader.Cookie, cookie);

            request.Method = "POST";
            request.ServicePoint.Expect100Continue = false;

            
            byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
            request.ContentLength = postBytes.Length;
            Stream stream = request.GetRequestStream();
            stream.Write(postBytes, 0, postBytes.Length);
            stream.Close();

           HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string html = "";
            if (response.Headers["Content-Encoding"] == "gzip")
            {

                GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding("utf-8"));
                html = reader.ReadToEnd();
                reader.Close();
            }
            else
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                html = reader.ReadToEnd();
                reader.Close();
            }
           
           
            return html;
        }


        public string posturl3(string url, string body)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Accept = "*/*";
            request.ContentType = "application/json";
            request.Headers.Set(HttpRequestHeader.Authorization, "Bearer 7ae7fd28-a74b-4799-8c39-47f34bee42fc");
            request.Headers.Add("OriginV", @"3.95");
            request.Headers.Add("v", @"3.95");
            request.Referer = "https://www.tcpjw.com/B2BHall/";
            request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh-CN");
            request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64; Trident/7.0; rv:11.0) like Gecko";
            request.Headers.Set(HttpRequestHeader.CacheControl, "no-cache");
            request.Headers.Set(HttpRequestHeader.Cookie, @"JSESSIONID=7017D6150D48D80057F0E8D6276FF217; _tact=NGMyZjIxMDktNjViNy05NzFhLTY4OGYtNzFiZGFhYjcwNGQ0; _tacz2=taccsr%3D%28direct%29%7Ctacccn%3D%28none%29%7Ctaccmd%3D%28none%29%7Ctaccct%3D%28none%29%7Ctaccrt%3D%28none%29; _tacb=NjA1NGQ2NWItYTQzMC1lOWViLWQ3YmQtYzZkZjJiNTczMjZm; _taca=1631774686647.1631778040349.1631778070241.37; _tacau=MCw2ZjM0MmE0MC1lMTZkLTBiMTYtMjNjOS0xZThmY2UzNGE1MTYs; Hm_lvt_72239a4a9c0b064684d43dfd4bfcba56=1631777883,1631778015,1631778041,1631778069; Hm_lpvt_72239a4a9c0b064684d43dfd4bfcba56=1631778069; _tacc=1; _uab_collina=163177468539833737424042; access_token=7ae7fd28-a74b-4799-8c39-47f34bee42fc; alert_after_login=0000; 1eWvE3WNTzmPO=5dwflN9XjqBiNLtUN6PnnUhf7nF73BbEspRY7yceflbZQTMn7V5qm7gNhn4g7Dv0VfjN9qp6G3h34YNEbJcU3jbBihDnGAGDIymDfOqB.Cpa; acw_tc=7760422916317773334266306e3e69cdef04ba656937e4054e5dc4f402; 1eWvE3WNTzmPP=53lH0kKrkerErqAr_4EnDRaMKjOyD9RD7fBEZFPHBpWMKwDz0ZG6ShHPb9C8F3sn7U11mHKHHBGIfyCgtxONZJLZ0Bt.LxMx._0OaEFu8DWmTZkHiNr82dHD3alJmNG_rc26zmlWyphP9pt5Q5VIqHScZ2I_uYluou0fzodqn709xTI.SpVQpa5wEPZ7G3X8nRLS3O5NOQ88DsHvle7knf66TuzDi7IcDt7PgX1pPfRr7uo73VjDkU7iSvMrrBMkDH6J0ugvnB_HOb014GMTXJpwHNHl1..Nfr5S8O0C0Xn9vhXgqUW48Ok5JLOo0VeNdYIQ_RQKWxZU6s2FuM.gpZG");

            request.Method = "POST";
            request.ServicePoint.Expect100Continue = false;

           
            byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
            request.ContentLength = postBytes.Length;
            Stream stream = request.GetRequestStream();
            stream.Write(postBytes, 0, postBytes.Length);
            stream.Close();

           HttpWebResponse response = (HttpWebResponse)request.GetResponse();


          
            string html = "";
            if (response.Headers["Content-Encoding"] == "gzip")
            {

                GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding("utf-8"));
                html = reader.ReadToEnd();
                reader.Close();
            }
            else
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                html = reader.ReadToEnd();
                reader.Close();
            }


            return html;
        }
        static string token = "";
        public 票据网()
        {
            InitializeComponent();
        }

        private void 票据网_Load(object sender, EventArgs e)
        {
            method.SetWebBrowserFeatures(method.IeVersion.IE11);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("https://www.tcpjw.com/passport/login");
           
        }

       string cookie = "";
        public string getdealPrice(string ticketId)
        {
            string url = "https://www.tcpjw.com/order-web/orderInfo/getQuoteOrderInfo";
            string postdata = "{\"version\":\"3.5\",\"source\":\"HTML\",\"channel\":\"01\",\"ticketId\":"+ticketId+"}";

            string html = PostUrl(url, postdata, cookie, "utf-8");
            Match dealPrice = Regex.Match(html, @"""totalPrice"":([\s\S]*?),");
            return dealPrice.Groups[1].Value;
        }
        
        
        
        
        
        
        
        /// <summary>
        /// 下单
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="ThousandCharge"></param>
        /// <param name="paytype"></param>
        /// <param name="endorseId"></param>
        /// <param name="yearrate"></param>
        /// <param name="dealPrice"></param>
        /// <param name="ticketPrice"></param>
        /// <param name="ticketType"></param>
        public void buyold(string ticketId, string ThousandCharge, string paytype, string endorseId, string yearrate, string ticketPrice,string ticketType)
        {
            string dealPrice = getdealPrice(ticketId);
            string url = "https://www.tcpjw.com/order-web/orderFlow/quoteOrder";
            string postdata = "{\"SOURCE\":\"HTML\",\"VERSION\":\"3.5\",\"CHANNEL\":\"01\",\"ticketId\":"+ticketId+",\"hundredThousandCharge\":\""+ ThousandCharge + "\",\"payType\":"+ paytype + ",\"endorseId\":"+ endorseId + ",\"yearRate\":"+yearrate+",\"dealPrice\":"+ dealPrice + ",\"ticketPrice\":"+ticketPrice+",\"ticketType\":"+ticketType+",\"useDefault\":false}";
           
            string html = PostUrl(url, postdata, cookie, "utf-8");
            textBox6.Text += DateTime.Now.ToString()+"："+ html + "\r\n";
            //MessageBox.Show(html);
        }
        /// <summary>
        /// 下单
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="ThousandCharge"></param>
        /// <param name="paytype"></param>
        /// <param name="endorseId"></param>
        /// <param name="yearrate"></param>
        /// <param name="dealPrice"></param>
        /// <param name="ticketPrice"></param>
        /// <param name="ticketType"></param>
        public void buy(string ticketId, string ThousandCharge, string paytype, string endorseId, string yearrate, string ticketPrice, string ticketType,string dealprice,string tradeNo,string bankname,string endtime)
        {
            string dealPrice = getdealPrice(ticketId);
            string url = "https://www.tcpjw.com/order-web/orderFlow/quoteOrder";
            string postdata = "{\"SOURCE\":\"HTML\",\"VERSION\":\"3.5\",\"CHANNEL\":\"01\",\"ticketId\":"+ticketId+",\"hundredThousandCharge\":\""+ ThousandCharge + "\",\"payType\":"+ paytype + ",\"endorseId\":"+ endorseId + ",\"yearRate\":"+yearrate+",\"dealPrice\":"+ dealPrice + ",\"ticketPrice\":"+ticketPrice+",\"ticketType\":"+ticketType+",\"useDefault\":false}";
           

            // string html = PostUrl(url, postdata, cookie, "utf-8");
            string html = posturl3(url, postdata);
            textBox6.Text += DateTime.Now.ToString() + "：" + html + "\r\n";
            //MessageBox.Show(html);
        }


        ArrayList xiadans = new ArrayList(); 

        public void run()
        {
          

            cookie= method.GetCookies("https://www.tcpjw.com/tradingHall-web/tradingHall/getTradingOrderInfo");

            
            Match tk = Regex.Match(cookie, @"access_token=.*");
            string tk1 = tk.Groups[0].Value.Replace("access_token=", "");

            token = Regex.Replace(tk1, @";.*", "").Trim();
        
            try
            {
                string url = "https://www.tcpjw.com/tradingHall-web/tradingHall/getTradingOrderInfo";
               
                string postdata = "{\"source\":\"HTML\",\"version\":\"3.5\",\"channel\":\"01\",\"pageNum\":1,\"pageSize\":15,\"tradeStatus\":null,,\"payType\":null,\"payTypes\":" + paytype+",\"bid\":" + bid+",\"bankName\":"+bankName+",\"lastTime\":"+lasttime+",\"lastTimeStart\":null,\"lastTimeEnd\":null,\"startDate\":null,\"endDate\":null,\"flawStatus\":\""+flawStatus+"\",\"priceType\":"+pricetype+",\"priceSp\":"+priceSp+",\"priceEp\":"+priceEp+",\"yearQuote\":"+yearQuote+",\"msw\":"+msw+",\"mswStart\":null,\"mswEnd\":null,\"orderColumn\":null,\"sortType\":\"\",\"depositPay\":"+depositPay+"}";

                //string body = @"{""version"":""3.5"",""source"":""HTML"",""channel"":""01"",""pageNum"":1,""pageSize"":15,""payType"":null,""payTypes"":null,""bid"":null,""bankName"":null,""lastTime"":null,""lastMonthTimeList"":[],""lastTimeStart"":null,""lastTimeEnd"":null,""startDate"":null,""endDate"":null,""flawStatusList"":[],""priceType"":null,""priceSp"":null,""priceEp"":null,""yearQuote"":null,""msw"":null,""mswStart"":null,""mswEnd"":null,""orderColumn"":null,""sortType"":"""",""depositPay"":null,""blackBankName"":null,""isCollected"":false,""orderStatus"":null,""orderStatusDataFewDays"":""1"",""isSpj"":false,""isCurUserPublish"":false,""queryBillAmount"":null,""fastTrade"":false,""noMoreThanNums"":"""",""issueDailyNoMoreThanNums"":"""",""recentDays"":null,""recentDaysNoMoreThanNums"":"""",""noMoreThanTodayNums"":"""",""faceKeyWords"":null,""endorseKeyWords"":null,""billDateDifference"":false,""hideDistrictLimit"":true}";
                string html = posturl3(url,postdata);
               // MessageBox.Show(html);
                MatchCollection ids = Regex.Matches(html, @"""ticketId"":([\s\S]*?),");
              
                MatchCollection a1s = Regex.Matches(html, @"""publishTime"":""([\s\S]*?)""");
                MatchCollection a2s = Regex.Matches(html, @"""bankName"":""([\s\S]*?)""");
                MatchCollection a3s = Regex.Matches(html, @"""ticketPrice"":([\s\S]*?),");
                MatchCollection a4s = Regex.Matches(html, @"""endTime"":""([\s\S]*?)""");
                MatchCollection a5s = Regex.Matches(html, @"""sellPrice"":""([\s\S]*?)""");
                MatchCollection a6s = Regex.Matches(html, @"""yearQuote"":([\s\S]*?),");
                MatchCollection a7s = Regex.Matches(html, @"""flawDescription"":""([\s\S]*?)""");
                MatchCollection a8s = Regex.Matches(html, @"payName"":\[""([\s\S]*?)\]");


                MatchCollection ticketTypes = Regex.Matches(html, @"""ticketType"":([\s\S]*?),");
                MatchCollection banknames = Regex.Matches(html, @"""bankName"":""([\s\S]*?)""");
                MatchCollection endtimes = Regex.Matches(html, @"""endTimeCheck"":""([\s\S]*?)""");

                for (int i = 0; i < a1s.Count; i++)
                {
                    

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(a1s[i].Groups[1].Value);
                    lv1.SubItems.Add(a2s[i].Groups[1].Value);
                    lv1.SubItems.Add(a3s[i].Groups[1].Value);
                    lv1.SubItems.Add(a4s[i].Groups[1].Value);
                    lv1.SubItems.Add(a5s[i].Groups[1].Value);
                    lv1.SubItems.Add(a6s[i].Groups[1].Value.Replace("\"", ""));
                    lv1.SubItems.Add(a7s[i].Groups[1].Value);
                    lv1.SubItems.Add(a8s[i].Groups[1].Value);

                    string ticketid = ids[i].Groups[1].Value;
                    string ThousandCharge = a5s[i].Groups[1].Value;
                    string paytype = "4";
                    string endorseId = "88715"; //银行ID
                    string yearrate = a6s[i].Groups[1].Value.Replace("\"", "");

                  
                    string ticketPrice=a3s[i].Groups[1].Value;
                    string ticketType = ticketTypes[i].Groups[1].Value;  
                    string dealPrice = a3s[i].Groups[1].Value;
                    string tradeno = "";
                    string bankname = banknames[i].Groups[1].Value;
                    string endtime = endtimes[i].Groups[1].Value;
                    if (!xiadans.Contains(ticketid))
                    {
                        xiadans.Add(ticketid);
                       buy(ticketid, ThousandCharge, paytype, endorseId, yearrate, ticketPrice, ticketType,dealPrice,tradeno,bankname,endtime);
                       
                    }


                }

                }
            catch (Exception ex)
            {

               textBox6.Text=ex.ToString();
            }
        }

        StringBuilder bids = new StringBuilder();


        string paytype = "null";
        string depositPay = "null";
        string bid = "null";
        string pricetype = "null";
        string lasttime = "null";
        string flawStatus = "";
        
        string priceSp = "null";
        string priceEp = "null";

        string msw = "null";
        string yearQuote = "null";
        string bankName = "null";
        private void Button1_Click(object sender, EventArgs e)
        {

             
           
            if (textBox2.Text!="")
            {
                priceSp = "\"" + textBox2.Text.Trim() + "\"";
                priceEp = "\"" + textBox3.Text.Trim() + "\"";


            }
            if (textBox4.Text != "")
            {
                yearQuote = "\"" + textBox4.Text.Trim() + "\"";
                msw = "\"" + textBox5.Text.Trim() + "\"";


            }

            if (textBox1.Text != "")
            {
                bankName = "\"" + textBox1.Text.Trim() + "\"";
              

            }

            //支付渠道
            if (checkBox10.Checked == true)
            {
                paytype= "[5]";
            }
           



            //保证金
            if (radioButton4.Checked == true)
            {
                depositPay="true";
            }
            if (radioButton5.Checked == true)
            {
                depositPay = "false";
            }

            //承兑人类型
            if (checkBox1.Checked == true)
            {
                bids.Append("\"1\",");
                bid = "["+bids.ToString().Remove(bids.Length-1,1)+"]";
               
            }
            if (checkBox2.Checked == true)
            {
                bids.Append("\"9\",");
                bid = "[" + bids.ToString().Remove(bids.Length - 1, 1) + "]";
                
            }
            if (checkBox3.Checked == true)
            {
                bids.Append("\"2\",");
                bid = "[" + bids.ToString().Remove(bids.Length - 1, 1) + "]";
               
            }
            if (checkBox4.Checked == true)
            {
                bids.Append("\"3\",");
                bid = "[" + bids.ToString().Remove(bids.Length - 1, 1) + "]";
               
            }
            if (checkBox5.Checked == true)
            {
                bids.Append("\"6\",");
                bid = "[" + bids.ToString().Remove(bids.Length - 1, 1) + "]";
                
            }
            if (checkBox6.Checked == true)
            {
                bids.Append("\"7\",");
                bid = "[" + bids.ToString().Remove(bids.Length - 1, 1) + "]";
                
            }
            if (checkBox7.Checked == true)
            {
                bids.Append("\"10\",");
                bid = "[" + bids.ToString().Remove(bids.Length - 1, 1) + "]";
                
            }
            if (checkBox8.Checked == true)
            {
                bids.Append("\"4\",");
                bid = "[" + bids.ToString().Remove(bids.Length - 1, 1) + "]";
                
            }
            if (checkBox9.Checked == true)
            {
                bids.Append("\"8\",");
                bid = "[" + bids.ToString().Remove(bids.Length - 1, 1) + "]";
               
            }
         
            //票面金额

            if (radioButton6.Checked == true)
            {
                pricetype = "\"1\"";
            }
            if (radioButton7.Checked == true)
            {
                pricetype = "\"2\"";
            }
            if (radioButton8.Checked == true)
            {
                pricetype = "\"3\"";
            }
            if (radioButton9.Checked == true)
            {
                pricetype = "\"4\"";
            }


            //剩余天数

            if (radioButton10.Checked == true)
            {
                lasttime = "\"1\"";
            }
            if (radioButton11.Checked == true)
            {
                lasttime = "\"2\"";
            }
            if (radioButton12.Checked == true)
            {
                lasttime = "\"3\"";
            }
            if (radioButton13.Checked == true)
            {
                lasttime = "\"4\"";
            }
            if (radioButton14.Checked == true)
            {
                lasttime = "\"5\"";
            }

            //瑕疵
            if (radioButton15.Checked == true)
            {
                flawStatus = "0";
            }
            if (radioButton16.Checked == true)
            {
                flawStatus = "49";
            }
            if (radioButton17.Checked == true)
            {
                flawStatus = "47";
            }
            if (radioButton18.Checked == true)
            {
                flawStatus = "48";
            }

            if (radioButton19.Checked == true)
            {
                flawStatus = "2";
            }
            if (radioButton20.Checked == true)
            {
                flawStatus = "3";
            }
            if (radioButton21.Checked == true)
            {
                flawStatus = "4";
            }
            if (radioButton22.Checked == true)
            {
                flawStatus = "5";
            }
            if (radioButton23.Checked == true)
            {
                flawStatus = "6";
            }


            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            timer1.Start();
        }
        Thread thread;
        private void Timer1_Tick(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
           // webBrowser1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            timer1.Stop();
        }
       
        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, EncodingType.GetTxtType(openFileDialog1.FileName));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {

                    textBox1.Text += text[i]+" ";

                }
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            webBrowser1.Refresh();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        #region 获取txt编码
        //调用：EncodingType.GetTxtType(textBox1.Text)
        public class EncodingType
        {
            /// <summary> 
            /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型 
            /// </summary> 
            /// <param name=“FILE_NAME“>文件路径</param> 
            /// <returns>文件的编码类型</returns> 
            public static System.Text.Encoding GetTxtType(string FILE_NAME)
            {
                FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
                Encoding r = GetType(fs);
                fs.Close();
                return r;
            }

            /// <summary> 
            /// 通过给定的文件流，判断文件的编码类型 
            /// </summary> 
            /// <param name=“fs“>文件流</param> 
            /// <returns>文件的编码类型</returns> 
            public static System.Text.Encoding GetType(FileStream fs)
            {
                byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
                byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
                byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM 
                Encoding reVal = Encoding.Default;

                BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
                int i;
                int.TryParse(fs.Length.ToString(), out i);
                byte[] ss = r.ReadBytes(i);
                if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
                {
                    reVal = Encoding.UTF8;
                }
                else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
                {
                    reVal = Encoding.BigEndianUnicode;
                }
                else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
                {
                    reVal = Encoding.Unicode;
                }
                r.Close();
                return reVal;

            }

            /// <summary> 
            /// 判断是否是不带 BOM 的 UTF8 格式 
            /// </summary> 
            /// <param name=“data“></param> 
            /// <returns></returns> 
            private static bool IsUTF8Bytes(byte[] data)
            {
                int charByteCounter = 1; //计算当前正分析的字符应还有的字节数 
                byte curByte; //当前分析的字节. 
                for (int i = 0; i < data.Length; i++)
                {
                    curByte = data[i];
                    if (charByteCounter == 1)
                    {
                        if (curByte >= 0x80)
                        {
                            //判断当前 
                            while (((curByte <<= 1) & 0x80) != 0)
                            {
                                charByteCounter++;
                            }
                            //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
                            if (charByteCounter == 1 || charByteCounter > 6)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        //若是UTF-8 此时第一位必须为1 
                        if ((curByte & 0xC0) != 0x80)
                        {
                            return false;
                        }
                        charByteCounter--;
                    }
                }
                if (charByteCounter > 1)
                {
                    throw new Exception("非预期的byte格式");
                }
                return true;
            }
        }

        #endregion


    }
}
