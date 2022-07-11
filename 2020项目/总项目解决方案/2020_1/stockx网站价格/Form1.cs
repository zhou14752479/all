using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

namespace stockx网站价格
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region 苏飞请求
        public static string gethtml(string url, string COOKIE)
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = url,//URL     必需项  
                Method = "GET",//URL     可选项 默认为Get  
                Timeout = 100000,//连接超时时间     可选项默认为100000  
                ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000  
                IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写  
                Cookie = COOKIE,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36",//用户的浏览器类型，版本，操作系统     可选项有默认值  
                Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值  
                ContentType = "text/html",//返回类型    可选项有默认值  
                Referer = "https://stockx.com/api/products/air-jordan-6-retro-dmp-2020?includes=market,360&currency=USD&country=HK",//来源URL     可选项  
                Allowautoredirect = true,//是否根据３０１跳转     可选项  
                AutoRedirectCookie = true,//是否自动处理Cookie     可选项  
                                          //CerPath = "d:\123.cer",//证书绝对路径     可选项不需要证书时可以不写这个参数  
                                          //Connectionlimit = 1024,//最大连接数     可选项 默认为1024  
                Postdata = "",//Post数据     可选项GET时不需要写  
                              //ProxyIp = "192.168.1.105：2020",//代理服务器ID     可选项 不需要代理 时可以不设置这三个参数  
                              //ProxyPwd = "123456",//代理服务器密码     可选项  
                              //ProxyUserName = "administrator",//代理服务器账户名     可选项  
                ResultType = ResultType.String,//返回数据类型，是Byte还是String  
            };
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            string cookie = result.Cookie;


            return html;

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
                //request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = "https://finance.sina.com.cn/money/forex/hq/USDCNY.shtml";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
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
        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public  string PostUrl(string url, string postData,string ua)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";

                request.ContentType = "application/json";
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.UserAgent = ua;
              
                request.Headers.Add("Cookie",cookie);
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("x-algolia-api-key: 6b5e76b49705eb9f51a06d3c82f7acee");
                headers.Add("x-algolia-application-id: XW7SBCT9V6");
                request.Referer = "https://finance.sina.com.cn/money/forex/hq/USDCNY.shtml";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

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

        //public ArrayList getfee(string sku, string price)
        //{
        //    ArrayList lists = new ArrayList();
        //    try
        //    {

        //        string url = "https://stockx.com/api/pricing?currency=USD&include_taxes=false";
        //        string postdata = "{\"context\":\"buying\",\"products\":[{\"sku\":\"" + sku + "\",\"amount\":" + price + ",\"quantity\":1}],\"discountCodes\":[\"\"]}";

        //        string html = PostUrl(url, postdata);

        //        MatchCollection fee = Regex.Matches(html, @"""amount"":([\s\S]*?),");

        //        foreach (Match item in fee)
        //        {
        //            lists.Add(item.Groups[1].Value);
        //        }
        //        return lists;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}


      

        private string Request_stockx_com(string sku, string price)
        {
            HttpWebResponse response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://stockx.com/api/pricing?currency=USD&include_taxes=true");

                request.KeepAlive = true;
                request.Headers.Add("sec-ch-ua", @""" Not;A Brand"";v=""99"", ""Google Chrome"";v=""97"", ""Chromium"";v=""97""");
                request.Headers.Add("grails-user", @"eyJDdXN0b21lciI6eyJCaWxsaW5nIjp7ImNhcmRUeXBlIjoiUGF5UGFsIiwidG9rZW4iOiJqZGd2NDZmIiwibGFzdDQiOm51bGwsImFjY291bnRFbWFpbCI6ImN5YmluM0Bob3RtYWlsLmNvbSIsImV4cGlyYXRpb25EYXRlIjpudWxsLCJjYXJkaG9sZGVyTmFtZSI6ImN5YmluIE1TSVBNIiwiQWRkcmVzcyI6eyJmaXJzdE5hbWUiOiJNU0lQTSIsImxhc3ROYW1lIjoiY3liaW4iLCJ0ZWxlcGhvbmUiOiI2MjY3NzQ3MDczIiwic3RyZWV0QWRkcmVzcyI6IjU3ODMgTkUgQ29sdW1iaWEgQmx2ZCIsImV4dGVuZGVkQWRkcmVzcyI6IiIsImxvY2FsaXR5IjoiUE9SVExBTkQiLCJyZWdpb24iOiJPUiIsInBvc3RhbENvZGUiOiI5NzIxOCIsImNvdW50cnlDb2RlQWxwaGEyIjoiVVMiLCJtYXJrZXQiOiJVUyJ9fSwiU2hpcHBpbmciOnsiQWRkcmVzcyI6eyJmaXJzdE5hbWUiOiJNU0lQTSIsImxhc3ROYW1lIjoiY3liaW4iLCJ0ZWxlcGhvbmUiOiI2MjY3NzQ3MDczIiwic3RyZWV0QWRkcmVzcyI6IjU3ODMgTkUgQ29sdW1iaWEgQmx2ZCIsImV4dGVuZGVkQWRkcmVzcyI6IiIsImxvY2FsaXR5IjoiUE9SVExBTkQiLCJyZWdpb24iOiJPUiIsInBvc3RhbENvZGUiOiI5NzIxOCIsImNvdW50cnlDb2RlQWxwaGEyIjoiVVMiLCJtYXJrZXQiOiJVUyJ9fSwidXVpZCI6IjI5MzJiMWNhLTA3ZjktMTFlNy1hYTc5LTEyY2RhMWM5YjZhNSIsImlkIjoiMjYxMjE1IiwiaGFzQnV5ZXJSZXdhcmQiOmZhbHNlfX0=");
                request.Headers.Add("sec-ch-ua-mobile", @"?0");
                request.Headers.Set(HttpRequestHeader.Authorization, "Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Ik5USkNNVVEyUmpBd1JUQXdORFk0TURRelF6SkZRelV4TWpneU5qSTNNRFJGTkRZME0wSTNSQSJ9.eyJodHRwczovL3N0b2NreC5jb20vY3VzdG9tZXJfdXVpZCI6IjI5MzJiMWNhLTA3ZjktMTFlNy1hYTc5LTEyY2RhMWM5YjZhNSIsImh0dHBzOi8vc3RvY2t4LmNvbS9nYV9ldmVudCI6IkxvZ2dlZCBJbiIsImlzcyI6Imh0dHBzOi8vYWNjb3VudHMuc3RvY2t4LmNvbS8iLCJzdWIiOiJhdXRoMHwyOTMyYjFjYS0wN2Y5LTExZTctYWE3OS0xMmNkYTFjOWI2YTUiLCJhdWQiOiJnYXRld2F5LnN0b2NreC5jb20iLCJpYXQiOjE2NDczMzMyNjAsImV4cCI6MTY0NzM3NjQ2MCwiYXpwIjoiT1Z4cnQ0VkpxVHg3TElVS2Q2NjFXMER1Vk1wY0ZCeUQiLCJzY29wZSI6Im9mZmxpbmVfYWNjZXNzIn0.nBhswZ_wg7Xvc37r7WWJAB2hOF_zJGk2gbAJTc641QyIhalHQHn2xtF-WX0aBRL-0wPhct4Z5qqLjT14Tz2NbMiezaq_K8YaPfzBC5GnlpRiORSjidHyMpTwIyMdwoCCCpzxmk_9zaMRyunxnSBBM2sGZtIK-8x1tYZ9EKLqEzXWw25ffwH8NND9XMseZYvHllHvcu8dgssvhLDmevknA1hvAzw9EGGz7wqZWR4b-JBz49Rnrs6LQQBU8nn4BQ0MJtLVvXvJbVqTXmLGPust4iptjMYrou66sDek5x1VMOgfX1LsTU4AFS3KL7hsea66Be28rDPciBc-BFfeyzYDjg");
                request.ContentType = "application/json";
                request.Headers.Add("appVersion", @"0.1");
                request.Headers.Add("appOS", @"web");
                request.Headers.Add("X-Requested-With", @"XMLHttpRequest");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36";
                request.Headers.Add("sec-ch-ua-platform", @"""Windows""");
                request.Accept = "*/*";
                request.Headers.Add("Origin", @"https://stockx.com");
                request.Headers.Add("Sec-Fetch-Site", @"same-origin");
                request.Headers.Add("Sec-Fetch-Mode", @"cors");
                request.Headers.Add("Sec-Fetch-Dest", @"empty");
                request.Referer = "https://stockx.com/zh-cn/buy/nike-kobe-5-protro-chaos?size=8.5";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh,sq;q=0.9,zh-CN;q=0.8,oc;q=0.7,de;q=0.6,en;q=0.5");
                request.Headers.Set(HttpRequestHeader.Cookie, @"stockx_device_id=6cd4a569-03f7-49c4-8ac0-d02b66fb74f3; _pxvid=ad9e18ec-a351-11ec-94e4-4c7157426957; ab_one_buy_now_v2_button=true; _ga=undefined; __ssid=b3c80f66c64a6a013c3c86aca09f024; rskxRunCookie=0; rCookie=aya2akwqhzk1ykdorbv5xyl0q83tk6; ajs_anonymous_id=20e5872f-e94f-476c-b011-be6bf9d3650e; QuantumMetricUserID=ecef40ca4923138fc0879e389771d324; rbuid=rbcr-7e0a0df2-4cab-4cd2-af70-2264a6de30ac; _ga=GA1.2.1509626082.1647233290; ajs_user_id=2932b1ca-07f9-11e7-aa79-12cda1c9b6a5; _gcl_au=1.1.243599321.1647233293; _scid=3eaec287-8ce0-4995-bd29-b9057f842dfb; __pdst=f752cc7f90c746f3a1572824a18777e0; tracker_device=7229b4c0-60bd-4fcd-b6f6-622b6e9b5085; _rdt_uuid=1647235630500.ffe44bd4-a4f8-49ad-9e54-39b93caafe8f; stockx_homepage=sneakers; __cf_bm=jRvcp2slDeTzH1fCNpLV2F1dnKSbyZeQeuM95t5wJMs-1647333257-0-ARmvw0P0mVrmGSf96JIipViKJCmClyGMInUKWKyvbtjd5zcutCX+dKcqRqOw6IsjgfxFacgl6T3Fqbny2O/mUHw=; mfaLogin=err; token=eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Ik5USkNNVVEyUmpBd1JUQXdORFk0TURRelF6SkZRelV4TWpneU5qSTNNRFJGTkRZME0wSTNSQSJ9.eyJodHRwczovL3N0b2NreC5jb20vY3VzdG9tZXJfdXVpZCI6IjI5MzJiMWNhLTA3ZjktMTFlNy1hYTc5LTEyY2RhMWM5YjZhNSIsImh0dHBzOi8vc3RvY2t4LmNvbS9nYV9ldmVudCI6IkxvZ2dlZCBJbiIsImlzcyI6Imh0dHBzOi8vYWNjb3VudHMuc3RvY2t4LmNvbS8iLCJzdWIiOiJhdXRoMHwyOTMyYjFjYS0wN2Y5LTExZTctYWE3OS0xMmNkYTFjOWI2YTUiLCJhdWQiOiJnYXRld2F5LnN0b2NreC5jb20iLCJpYXQiOjE2NDczMzMyNjAsImV4cCI6MTY0NzM3NjQ2MCwiYXpwIjoiT1Z4cnQ0VkpxVHg3TElVS2Q2NjFXMER1Vk1wY0ZCeUQiLCJzY29wZSI6Im9mZmxpbmVfYWNjZXNzIn0.nBhswZ_wg7Xvc37r7WWJAB2hOF_zJGk2gbAJTc641QyIhalHQHn2xtF-WX0aBRL-0wPhct4Z5qqLjT14Tz2NbMiezaq_K8YaPfzBC5GnlpRiORSjidHyMpTwIyMdwoCCCpzxmk_9zaMRyunxnSBBM2sGZtIK-8x1tYZ9EKLqEzXWw25ffwH8NND9XMseZYvHllHvcu8dgssvhLDmevknA1hvAzw9EGGz7wqZWR4b-JBz49Rnrs6LQQBU8nn4BQ0MJtLVvXvJbVqTXmLGPust4iptjMYrou66sDek5x1VMOgfX1LsTU4AFS3KL7hsea66Be28rDPciBc-BFfeyzYDjg; language_code=zh; stockx_selected_currency=USD; stockx_product_visits=1; stockx_default_sneakers_size=8.5; stockx_preferred_market_activity=sales; stockx_session=%2249b7c1da-fbfc-46e0-9050-35c42659e480%22; loggedIn=2932b1ca-07f9-11e7-aa79-12cda1c9b6a5; pxcts=b8ccec5e-a43a-11ec-91cb-4343774f6556; riskified_recover_updated_verbiage=true; ops_banner_id=bltf0ff6f9ef26b6bdb; lastRskxRun=1647333270917; _gid=GA1.2.1569939799.1647333332; _clck=9nwi44|1|ezs|0; _clsk=1v6wyzr|1647333336987|1|1|b.clarity.ms/collect; forterToken=67c1ff70bd394a2b93c79d3cec6ea654_1647333338425__UDF43_13ck; _uetsid=e71eb060a43a11eca8b539f1cf3e853e; _uetvid=d76dd6b0a35111ecb8ce914c7e0239d5; IR_gbd=stockx.com; IR_9060=1647333343639%7C0%7C1647333343639%7C%7C; IR_PI=55db0870-b722-3bc1-b297-6d9c88226856%7C1647419743639; QuantumMetricSessionID=39a44a71f4d179b99577fa085807bff6; _px3=cc47dfbf708be1feb5eacf832bab40a5feb8a27c7499348e459bc9aafce895b7:jISHEoM3WNPxQzxHRq4lt9yBfvmWGlpaAce3N8pw8Pd5drNBDMmoqLJLZBiVg3nW7BJyN05RuofmC+wXndYD7w==:1000:phIp7GV+l1vxWhpVseKlGqLAvTksU1Da7NH2axCk2xrKKiK7GPZaPrYJ5fLinJp75Lqa/SpVfQCngr1Kf8Yz6WNBNqoMfh4AKJ8aCmBgfECnayOgVlJvRE8M8Umcnot0x+5NB2XKlh2tJ+H0kW5BFRq6r8m6I224P5MqqONHTmf1qi8/xoP2CLLfkj1T9L1zoM4tLcEpwMab3TwpRlZzNA==; _gat=1; _dd_s=rum=0&expire=1647334620219");

                request.Method = "POST";
                request.ServicePoint.Expect100Continue = false;

                string body = "{\"context\":\"buying\",\"products\":[{\"sku\":\"" + sku + "\",\"amount\":" + price + ",\"quantity\":1}],\"discountCodes\":[\"\"]}";
                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                request.ContentLength = postBytes.Length;
                //Stream stream = request.GetRequestStream();
                //stream.Write(postBytes, 0, postBytes.Length);
                //stream.Close();

                response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException e)
            {
                
                return e.ToString();
            }
           
            
        }
        public string gettotalfee(string sku,string price)
        {
            Thread.Sleep(2000);
            string url = "https://stockx.com/api/pricing?currency=USD&include_taxes=true";
            string postdata = "{\"context\":\"buying\",\"products\":[{\"sku\":\""+sku+"\",\"amount\":"+price+",\"quantity\":1}],\"discountCodes\":[\"\"]}";
            Random rd = new Random();
            int x = rd.Next(0, 9999);
           string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904." + x + " Safari/537.36";

            //string html = PostUrl(url, postdata,UserAgent);
            string html = Request_stockx_com(sku,price);
            //MessageBox.Show(postdata);
            MessageBox.Show(html);
            
            string total = Regex.Match(html, @"""totalUSD"":([\s\S]*?),").Groups[1].Value;
            return total;
        }



        public void gethuilv()
        {
            string url = "https://webapi.huilv.cc/api/exchange?num=1&chiyouhuobi=USD&duihuanhuobi=CNY&type=1&callback=jisuanjieguo&_=1645687133740";
            string html = GetUrl(url, "utf-8");
            string huilv = Regex.Match(html, @"""dangqianhuilv"":""([\s\S]*?)""").Groups[1].Value;
            //string url = "https://www.baidu.com/s?ie=UTF-8&wd=%E7%BE%8E%E5%85%83%E6%B1%87%E7%8E%87";
            //string html = GetUrl(url, "utf-8");
            //string huilv = Regex.Match(html, @"1美元=([\s\S]*?)人民币").Groups[1].Value;
            textBox2.Text = huilv;
        }

        string cookie = "stockx_device_id=ff6f9452-7f4f-4452-9f7b-bfb12dc3cb3c; _pxvid=07622260-de40-11ec-9714-7478474d6a78; _ga=undefined; _ga=GA1.2.740994899.1653712714; _gcl_au=1.1.1784340517.1653712714; ajs_anonymous_id=622f471c-5002-413e-92d0-abf1487d1f75; _clck=1o7sq3y|1|f1u|0; OptanonAlertBoxClosed=2022-05-28T04:38:35.985Z; __pdst=0382741d92b0453192dddbef6ea81ee8; _scid=45792cf2-448e-4f73-a263-70ddde97c614; QuantumMetricUserID=fec1a1e143ab7207f7d35401b120bda6; OptanonConsent=isGpcEnabled=0&datestamp=Sat+May+28+2022+12%3A38%3A47+GMT%2B0800+(%E4%B8%AD%E5%9B%BD%E6%A0%87%E5%87%86%E6%97%B6%E9%97%B4)&version=6.32.0&isIABGlobal=false&hosts=&consentId=79af6906-812d-46e7-9171-489767328125&interactionCount=1&landingPath=NotLandingPage&groups=C0001%3A1%2CC0002%3A1%2CC0005%3A1%2CC0004%3A1%2CC0003%3A1&geolocation=CN%3BJS&AwaitingReconsent=false; IR_PI=0b3c7f76-de40-11ec-ad83-91c0181bdbe9%7C1653799132859; _uetvid=0ad91060de4011ec84e59d1f015ea970; __ssid=ff5cb0cbc2acad520ae246194608288; lastRskxRun=1653712761877; rskxRunCookie=0; rCookie=09lz87f286xvtpxog43ktv8l3pdvpjq; pxcts=72bfe7b1-fd30-11ec-8495-6646674c4a70; _pxff_rf=1; _pxff_fp=1; _pxff_idp_c=1,s; __pxvid=738c6be7-fd30-11ec-b666-0242ac120002; _px3=3239e0ed1941541ad928ba0df10f4d4e9618a0d350c5222b2b2d6c53fdd55d4a:+1MecJTqoR/Z6bRn9PBs46Qngy0WikoYb9rxDX416lGyugnNEpTP3JJEhG9tmZrSYtdDMR7k89ocNhCFKJpMzA==:1000:oPWQKmxc5o7Sithe0jXnCQdVwD5AfRkVxVTXgp3j1yjxZPMWQR70cAxPJ0cRtg69OgXTFQaAR/Lc8eEa3bZgACs4TSacUTz6O1F9S6QRrP0w+rY162ikEMPrfjMmQXMAmBrSVlhtDEWdANt++eg7SaVxxD8Gnhutkyp5DQlaoWbQ6O+prg5sthS4l5YTGmcSvwvCOCwZoAepDaP8k+HvSg==; stockx_session=cf3dbdf1-6580-4a6c-81c1-bd67cace761d; language_code=en; stockx_selected_region=CN; stockx_dismiss_modal=true; stockx_dismiss_modal_set=2022-07-06T13%3A36%3A07.219Z; stockx_dismiss_modal_expiration=2022-07-13T13%3A36%3A07.219Z; forterToken=6e9184c22e9f463f89626910eced4461_1657114565537__UDF43_13ck; stockx_preferred_market_activity=sales; _pxde=6f57240f1823f5596153aaa30b351a95705f051d14f6863d902a7fe1b2e27bbc:eyJ0aW1lc3RhbXAiOjE2NTcxMTQ1Njk2NjQsImZfa2IiOjB9; stockx_homepage=sneakers; stockx_default_sneakers_size=All; stockx_product_visits=1; __cf_bm=DibcyAwBubg2VsWaHWggVscPOp_GFYAhHRjvrGq6KTw-1657114573-0-AY+o+Ef+oo7SaTzeb91e4xsT4HkQ3RjmmWadfmMikOjRvGS3ZIzEUuzWPKCh37CiwzYRBE7a85VSewK/EhxaoxY=; _dd_s=rum=0&expire=1657115470450";
        
        public void run()
        {

            try
            {


                string url = "https://xw7sbct9v6-2.algolianet.com/1/indexes/products/query?x-algolia-agent=Algolia%20for%20JavaScript%20(4.8.4)%3B%20Browser";
                string postdata = "{\"params\":\"query=" + textBox1.Text.Trim() + "&facets=*&filters=\"}";
                string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36";
                string html = PostUrl(url, postdata,UserAgent);

                Match huo = Regex.Match(html, @"""url"":""([\s\S]*?)""");
                string aurl = "https://stockx.com/api/products/" + huo.Groups[1].Value + "?includes=market,360&currency=USD&country=US";


                string ahtml = gethtml(aurl, cookie);
                //textBox1.Text = aurl;
                //MessageBox.Show(ahtml);
                Match highestBid = Regex.Match(ahtml, @"""highestBid"":([\s\S]*?),");
                MatchCollection skus = Regex.Matches(ahtml, @"""skuUuid"":""([\s\S]*?)""");
                MatchCollection sizes = Regex.Matches(ahtml, @"""lowestAskSize"":([\s\S]*?),");
                MatchCollection lows = Regex.Matches(ahtml, @"""lowestAsk"":([\s\S]*?),");
                MatchCollection highs = Regex.Matches(ahtml, @"""highestBidFloat"":([\s\S]*?)}");






                for (int j = 1; j < lows.Count; j++)
                {
                    if (sizes[j].Groups[1].Value != "null")
                    {


                        double lowprice = Convert.ToDouble(textBox2.Text) * Convert.ToDouble(lows[j].Groups[1].Value);
                        double highprice = Convert.ToDouble(textBox2.Text) * Convert.ToDouble(highs[j].Groups[1].Value);
                        //lv2.SubItems.Add(lowprice.ToString());
                        //lv2.SubItems.Add(highprice.ToString());

                        double fee1 = Convert.ToDouble(textBox2.Text) * 0.06 * Convert.ToDouble(lows[j].Groups[1].Value);  //手续费是变动的
                        double fee2 = Convert.ToDouble(textBox2.Text) * 0.06 * Convert.ToDouble(highs[j].Groups[1].Value);  //手续费是变动的
                        double yunfei = Convert.ToDouble(textBox2.Text) * 14.95; //运费是变动的目前是13.95

                        double zong1 = lowprice + fee1 + yunfei;
                        double zong2 = highprice + fee2 + yunfei;



                        try
                        {
                            //string zong1 = gettotalfee(skus[j].Groups[1].Value, lows[j].Groups[1].Value);
                            //string zong2 = gettotalfee(skus[j].Groups[1].Value, highs[j].Groups[1].Value);

                            //zong1 = (Convert.ToDouble(zong1) * Convert.ToDouble(textBox2.Text)).ToString();
                            //zong2 = (Convert.ToDouble(zong2) * Convert.ToDouble(textBox2.Text)).ToString();
                            ListViewItem lv2 = listView1.Items.Add("US " + sizes[j].Groups[1].Value.Replace("null", "-")); //使用Listview展示数据   
                            lv2.SubItems.Add(zong1.ToString());
                            lv2.SubItems.Add(zong2.ToString());
                        }
                        catch (Exception)
                        {
                            Thread.Sleep(2000);
                            j = j - 1;
                            continue;
                           
                        }
                    }

                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            button1.Text = "点击获取";
            button1.Enabled = true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            gethuilv();
        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                button1.Text = "正在获取...";
                button1.Enabled = false;
                thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            listView1.Items.Clear();
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox2.Text = "";
            gethuilv();
        }
    }
}
