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
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
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
        public static string PostUrl(string url, string postData)
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

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", "");
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("x-algolia-api-key: 6b5e76b49705eb9f51a06d3c82f7acee");
                headers.Add("x-algolia-application-id: XW7SBCT9V6");
                request.Referer = "https://accounts.ebay.com/acctxs/user";
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



            public void gethuilv()
        {
            string url = "https://webapi.huilv.cc/api/exchange?num=1&chiyouhuobi=USD&duihuanhuobi=CNY&type=1&callback=jisuanjieguo&_=1645687133740";
            string html = GetUrl(url, "utf-8");
            string huilv = Regex.Match(html, @"""dangqianhuilv"":""([\s\S]*?)""").Groups[1].Value;
            textBox2.Text = huilv;
        }

        string cookie = "stockx_device_id=6d644afd-028e-43fe-9569-c9c1415271c1; _pxvid=bc908e20-9540-11ec-a19b-497978705341; _ga=undefined; rskxRunCookie=0; rCookie=1vpbrt7ojq9fs0pldcs89sl00naqgv; __ssid=9c512a85f1483910122cd4a190df7e0; ab_one_buy_now_v2_button=true; tracker_device=32dc7fff-c416-460d-b9a9-3c4c42093390; _ga=GA1.2.685685636.1645758061; _gcl_au=1.1.425481816.1645758065; ajs_anonymous_id=f323e877-aafc-4806-a0a2-7bc1b3e26b9a; rbuid=rbos-ab50ae84-ccb1-4035-a579-fe1a75ab8098; _scid=dd421d6f-5ae0-4b0e-b2d0-693fa507a8b7; __pdst=b7b4cd3a7e1f49d5aee18b5418b623e8; QuantumMetricUserID=3a24e6b142b001c1aad90f64472c3123; stockx_homepage=sneakers; language_code=zh; _gid=GA1.2.1602747318.1646613502; pxcts=e81593f9-9dae-11ec-bb80-6d645852506b; riskified_recover_updated_verbiage=true; ops_banner_id=bltf0ff6f9ef26b6bdb; IR_gbd=stockx.com; _clck=1ybedkb|1|ezk|0; _rdt_uuid=1646613584009.8ecf5830-1b7c-4c0b-b5d8-5e6b2160893f; IR_9060=1646614625404%7C0%7C1646614625404%7C%7C; IR_PI=34ddf980-5644-11ec-b7cd-fd310cf19260%7C1646701025404; __cf_bm=a4ciY3ayBdCZ7I.bHkIxBW56Pl2vmF7_UVdagy5tTZM-1646618614-0-ARAl5A8t3VMDMTegJ2XimGWOT04P8fLwsPxfukotT5V0b5LKuK95KsOuE9sXzSPlFfW6mT4be9sA8e5kB1RoI5M=; stockx_preferred_market_activity=sales; stockx_session=%22abd68b20-68e4-4d38-94f2-2304e4e185f4%22; com.auth0.auth.%7B%22state%22%3A%7B%22forceLogin%22%3Atrue%7D%7D={%22nonce%22:null%2C%22state%22:%22{%5C%22state%5C%22:{%5C%22forceLogin%5C%22:true}}%22%2C%22lastUsedConnection%22:%22production%22}; _com.auth0.auth.%7B%22state%22%3A%7B%22forceLogin%22%3Atrue%7D%7D_compat={%22nonce%22:null%2C%22state%22:%22{%5C%22state%5C%22:{%5C%22forceLogin%5C%22:true}}%22%2C%22lastUsedConnection%22:%22production%22}; _clsk=cxgxeb|1646618617502|1|0|i.clarity.ms/collect; QuantumMetricSessionID=baccc52ffa8ec5a9102df4aa3e1017be; _uetsid=e90836809dae11ecacb4f3299b3cf807; _uetvid=2db6537095e711ec953d0137b555dfe9; token=eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Ik5USkNNVVEyUmpBd1JUQXdORFk0TURRelF6SkZRelV4TWpneU5qSTNNRFJGTkRZME0wSTNSQSJ9.eyJodHRwczovL3N0b2NreC5jb20vY3VzdG9tZXJfdXVpZCI6IjI5MzJiMWNhLTA3ZjktMTFlNy1hYTc5LTEyY2RhMWM5YjZhNSIsImh0dHBzOi8vc3RvY2t4LmNvbS9nYV9ldmVudCI6IkxvZ2dlZCBJbiIsImlzcyI6Imh0dHBzOi8vYWNjb3VudHMuc3RvY2t4LmNvbS8iLCJzdWIiOiJhdXRoMHwyOTMyYjFjYS0wN2Y5LTExZTctYWE3OS0xMmNkYTFjOWI2YTUiLCJhdWQiOlsiZ2F0ZXdheS5zdG9ja3guY29tIiwiaHR0cHM6Ly9zdG9ja3gtcHJvZC5hdXRoMC5jb20vdXNlcmluZm8iXSwiaWF0IjoxNjQ2NjE4NzE3LCJleHAiOjE2NDY2NjE5MTcsImF6cCI6Ik9WeHJ0NFZKcVR4N0xJVUtkNjYxVzBEdVZNcGNGQnlEIiwic2NvcGUiOiJvcGVuaWQgcHJvZmlsZSJ9.JJSrMSsnUzuQUp7mWceWX89L5kKDNVWzjVUlWDGqQSzi3bsU8lCJzH11nYqZyujEvneAZA4HHN5j6uMEmVaCgrk0zlpdQmSus3UxTGsKIIQbWVLGLlkAvNsbtS6wNNyQIrYmKPHr-UQ77g5trJk7Ek3sxfTDvGzjUhsLJKboz7GatUYs3dlhWrdmOjn1SGtCDHcVq7q9QQo_XAvUqwewNDEFG4wEROZp9besWvJHetQQ5JKPn3hLrRwufKxjczx9aCzHfD8tud5HlHQ4NpsIkRVrbkPPUtKhDEklMVZiGigIvUjagBmw8MBtj2UM5e66pIyPqrSIw2zugRtlpuWqDw; stockx_selected_currency=USD; loggedIn=2932b1ca-07f9-11e7-aa79-12cda1c9b6a5; stockx_default_sneakers_size=8.5; _px3=9b55363afdd30d47bdebd5768e9a658a77fbc2f45d57d17ecd94cb801bdf688d:1rxCQDBqoJLeX1AcloWiARJQ4epAANBectP8T2EaFdSSWn0Bjnd0gIdIAVAhZ0NTHvpwjuoVgbaRCf93UCDGUw==:1000:58DAAEMVtM1dPTfurOrJGPBjPKB4ykIkzuwDDk5DHFvZp0DGqfhwWMyaCckgeMUy2suMShuke11LqtSnBPvL+M0+HD49iPEgO4NpTVktFbQgsnYLsMVatGfhgxZ3KR5103ueAJKjoBHR6+oTBzsbYYqQ5XKs7m4hr3JIMbubbSKZxusuJD9OgPNmlztsGKseSSDWH+Q64FF8LnWU7NGRnA==; forterToken=291d69bc4ccd4bedb442db00cdceb640_1646618716273__UDF43_13ck; stockx_product_visits=3; lastRskxRun=1646618786530; _dd_s=rum=0&expire=1646619740546";
            public void run()
        {

            try
            {

             
                string url = "https://xw7sbct9v6-2.algolianet.com/1/indexes/products/query?x-algolia-agent=Algolia%20for%20JavaScript%20(4.8.4)%3B%20Browser";
                string postdata = "{\"params\":\"query="+textBox1.Text.Trim()+"&facets=*&filters=\"}";
                string html = PostUrl(url,postdata);
                
                Match huo = Regex.Match(html, @"""url"":""([\s\S]*?)""");
                string aurl = "https://stockx.com/api/products/"+huo.Groups[1].Value+"?includes=market,360&currency=USD&country=US";
               

                string ahtml = gethtml(aurl, cookie);
                //textBox1.Text = aurl;
                //MessageBox.Show(ahtml);
                Match highestBid = Regex.Match(ahtml, @"""highestBid"":([\s\S]*?),");
              

                    MatchCollection skus = Regex.Matches(ahtml, @"""skuUuid"":([\s\S]*?),");


                MatchCollection sizes = Regex.Matches(ahtml, @"""lowestAskSize"":([\s\S]*?),");
                MatchCollection lows = Regex.Matches(ahtml, @"""lowestAsk"":([\s\S]*?),");
                MatchCollection highs = Regex.Matches(ahtml, @"""highestBidFloat"":([\s\S]*?)}");



              


                for (int j = 1; j < lows.Count; j++)
                {

                    ListViewItem lv2 = listView1.Items.Add("US " + sizes[j].Groups[1].Value.Replace("null", "-")); //使用Listview展示数据   
                    
                    double lowprice = Convert.ToDouble(textBox2.Text) * Convert.ToDouble(lows[j].Groups[1].Value);
                    double highprice = Convert.ToDouble(textBox2.Text) * Convert.ToDouble(highs[j].Groups[1].Value);

                    //lv2.SubItems.Add(lowprice.ToString());
                    //lv2.SubItems.Add(highprice.ToString());


                    double fee1 = Convert.ToDouble(textBox2.Text) * 0.055* Convert.ToDouble(lows[j].Groups[1].Value);  //手续费是变动的
                    double fee2 = Convert.ToDouble(textBox2.Text) * 0.055 * Convert.ToDouble(highs[j].Groups[1].Value);  //手续费是变动的

                    //if (Convert.ToDouble(lows[j].Groups[1].Value) > 999)
                    //{
                    //    fee1 = Convert.ToDouble(textBox2.Text) * 0.06 * Convert.ToDouble(lows[j].Groups[1].Value);  //手续费是变动的
                    //}
                    double yunfei = Convert.ToDouble(textBox2.Text) * 13.95 ; //运费是变动的目前是13.95

                    double zong1 = lowprice + fee1 + yunfei;
                    double zong2= highprice + fee2 + yunfei;
                    lv2.SubItems.Add(zong1.ToString());
                    lv2.SubItems.Add(zong2.ToString());
                    
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
