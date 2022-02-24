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

        string cookie = "stockx_device_id=6d644afd-028e-43fe-9569-c9c1415271c1; language_code=zh; pxcts=bc9098cb-9540-11ec-a19b-497978705341; _pxvid=bc908e20-9540-11ec-a19b-497978705341; stockx_session=%225fee44b1-6abb-438e-8db9-8109af9b4d54%22; _ga=undefined; rskxRunCookie=0; rCookie=1vpbrt7ojq9fs0pldcs89sl00naqgv; riskified_recover_updated_verbiage=true; ops_banner_id=bltf0ff6f9ef26b6bdb; __ssid=9c512a85f1483910122cd4a190df7e0; ab_one_buy_now_v2_button=true; stockx_preferred_market_activity=sales; stockx_default_sneakers_size=All; stockx_homepage=sneakers; tracker_device=32dc7fff-c416-460d-b9a9-3c4c42093390; _px3=549c4a43bb9db3879539b831350db777ce363d6ea129e730a43ccbde573502c9:TYTIPKN+g9cDik+cNAPnZUZDmcljQMzT1/5tCo1ko6SAVg/2fDUBtdP6c/ea6Tan1jRtV+O9LWpDdyEXYe/uNw==:1000:ILFTZuVgqSIXSmV3eHQCWzXKl8oXNcxdsbt4bJmfNu7Q0lbYG+3XLLs/SB5YgDfsgvjbZK4bKAIBjdF4I8M63ZDIQ2M8xZC2kBQ6NiC42gSMI8HGWsipuhRPDRAm5dOpZHERJJN1GmlhOyeqqZldIr7EfBYX3pukrKGa/zQv4aLMvYOPUu0+l3+bSZLBiewm0FcAt4efbLRhWdSnhM7kdg==; _dd_s=rum=0&expire=1645687810969; stockx_product_visits=7; lastRskxRun=1645686912128; forterToken=291d69bc4ccd4bedb442db00cdceb640_1645686912034__UDF43_13ck";
            public void run()
        {

            try
            {

             
                string url = "https://xw7sbct9v6-2.algolianet.com/1/indexes/products/query?x-algolia-agent=Algolia%20for%20JavaScript%20(4.8.4)%3B%20Browser";
                string postdata = "{\"params\":\"query="+textBox1.Text.Trim()+"&facets=*&filters=\"}";
                string html = PostUrl(url,postdata);
                
                Match huo = Regex.Match(html, @"""url"":""([\s\S]*?)""");
                string aurl = "https://stockx.com/api/products/"+huo.Groups[1].Value+"?includes=market,360&currency=USD&country=HK";
               

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


                    double fee1 = Convert.ToDouble(textBox2.Text) * 29.95;

                    if (Convert.ToDouble(lows[j].Groups[1].Value) < 999)
                    {
                        fee1 = 0.03 * lowprice;
                    }
                     
                    double fee2 = Convert.ToDouble(textBox2.Text) * 9 ;

                    double zong1 = lowprice + fee1 + fee2;
                    double zong2= highprice + fee1 + fee2;
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
