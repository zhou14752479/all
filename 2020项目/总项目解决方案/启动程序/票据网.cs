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
                //request.ContentType = "application/x-www-form-urlencoded";
                request.ContentType = "application/json";
                request.ContentLength = postData.Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
              
                request.Referer = "https://www.tcpjw.com/B2BHall";
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

        #endregion
        static string token = "";
        public 票据网()
        {
            InitializeComponent();
        }

        private void 票据网_Load(object sender, EventArgs e)
        {
            method.SetWebBrowserFeatures(method.IeVersion.IE10);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("https://www.tcpjw.com/login");
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
        public void buy(string ticketId, string ThousandCharge, string paytype, string endorseId, string yearrate, string ticketPrice,string ticketType)
        {
            string dealPrice = getdealPrice(ticketId);
            string url = "https://www.tcpjw.com/order-web/orderFlow/quoteOrder";
            string postdata = "{\"SOURCE\":\"HTML\",\"VERSION\":\"3.5\",\"CHANNEL\":\"01\",\"ticketId\":"+ticketId+",\"hundredThousandCharge\":\""+ ThousandCharge + "\",\"payType\":"+ paytype + ",\"endorseId\":"+ endorseId + ",\"yearRate\":"+yearrate+",\"dealPrice\":"+ dealPrice + ",\"ticketPrice\":"+ticketPrice+",\"ticketType\":"+ticketType+",\"useDefault\":false}";
           
            string html = PostUrl(url, postdata, cookie, "utf-8");
            MessageBox.Show(html);
        }
        


        public void run()
        {
            //cookie = method.GetCookies("https://www.tcpjw.com/tradingHall");
            if (cookie == "")
            {
                MessageBox.Show("未登录");
            }
            Match tk = Regex.Match(cookie, @"access_token=.*");
            token = tk.Groups[0].Value.Replace("access_token=","");
           

            try
            {
                string url = "https://www.tcpjw.com/order-web/orderInfo/getTradingOrderInfo";
                string postdata = "{\"source\":\"HTML\",\"version\":\"3.5\",\"channel\":\"01\",\"pageNum\":1,\"pageSize\":15,\"tradeStatus\":null,\"payType\":"+paytype+",\"bid\":"+bid+",\"bankName\":null,\"lastTime\":"+lasttime+",\"lastTimeStart\":null,\"lastTimeEnd\":null,\"startDate\":null,\"endDate\":null,\"flawStatus\":\""+flawStatus+"\",\"priceType\":"+pricetype+",\"priceSp\":null,\"priceEp\":null,\"yearQuote\":null,\"msw\":null,\"mswStart\":null,\"mswEnd\":null,\"orderColumn\":null,\"sortType\":\"\",\"depositPay\":"+depositPay+"}";
                
                string html = PostUrl(url,postdata,cookie,"utf-8");
                
                MatchCollection ids = Regex.Matches(html, @"""ticketId"":([\s\S]*?),");

                MatchCollection a1s = Regex.Matches(html, @"""publishTime"":""([\s\S]*?)""");
                MatchCollection a2s = Regex.Matches(html, @"""bankName"":""([\s\S]*?)""");
                MatchCollection a3s = Regex.Matches(html, @"""ticketPrice"":([\s\S]*?),");
                MatchCollection a4s = Regex.Matches(html, @"""endTime"":""([\s\S]*?)""");
                MatchCollection a5s = Regex.Matches(html, @"""sellPrice"":""([\s\S]*?)""");
                MatchCollection a6s = Regex.Matches(html, @"""yearQuote"":([\s\S]*?),");
                MatchCollection a7s = Regex.Matches(html, @"""flawDescription"":""([\s\S]*?)""");
                MatchCollection a8s = Regex.Matches(html, @"payName"":\[""([\s\S]*?)\]");
                for (int i = 0; i < a1s.Count; i++)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(a1s[i].Groups[1].Value);
                    lv1.SubItems.Add(a2s[i].Groups[1].Value);
                    lv1.SubItems.Add(a3s[i].Groups[1].Value);
                    lv1.SubItems.Add(a4s[i].Groups[1].Value);
                    lv1.SubItems.Add(a5s[i].Groups[1].Value);
                    lv1.SubItems.Add(a6s[i].Groups[1].Value);
                    lv1.SubItems.Add(a7s[i].Groups[1].Value);
                    lv1.SubItems.Add(a8s[i].Groups[1].Value);

                    string ticketid = ids[i].Groups[1].Value;
                    string ThousandCharge = a5s[i].Groups[1].Value;
                    string payt = "1";
                    string endorseId = "15858";
                    string yearrate= a6s[i].Groups[1].Value;
                    string dealPrice=a3s[i].Groups[1].Value;
                    string ticketPrice=a3s[i].Groups[1].Value;
                    string ticketType = "2";  //默认2 银行不存在则3
                    buy(ticketid, ThousandCharge,  payt,  endorseId,  yearrate,  ticketPrice,  ticketType);
                    
                }

                }
            catch (Exception)
            {

                throw;
            }
        }

        StringBuilder bids = new StringBuilder();


        string paytype = "null";
        string depositPay = "null";
        string bid = "null";
        string pricetype = "null";
        string lasttime = "null";
        string flawStatus = "";
        private void Button1_Click(object sender, EventArgs e)
        {
            cookie = method.GetCookies("https://www.tcpjw.com/login");
            if (radioButton1.Checked == true)
            {
                paytype="\"1\"";
            }
            if (radioButton2.Checked == true)
            {
                paytype = "\"2\"";
            }
            if (radioButton3.Checked == true)
            {
                paytype = "\"3\"";
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
                pricetype = "\"1\"";
            }
            if (radioButton11.Checked == true)
            {
                pricetype = "\"2\"";
            }
            if (radioButton12.Checked == true)
            {
                pricetype = "\"3\"";
            }
            if (radioButton13.Checked == true)
            {
                pricetype = "\"4\"";
            }
            if (radioButton14.Checked == true)
            {
                pricetype = "\"5\"";
            }

            //瑕疵
            if (radioButton15.Checked == true)
            {
                flawStatus = "0";
            }
            if (radioButton16.Checked == true)
            {
                flawStatus = "46";
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



            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            Thread thread1 = new Thread(new ThreadStart(run));
            thread1.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
