using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using System;
using System.Collections;
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
                Referer = "https://live.500.com/wanchang.php",//来源URL     可选项  
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
                //request.Headers.Add("origin","https://www.nike.com");
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







            public void run()
        {

            try
            {
                
                    string url = "https://xw7sbct9v6-1.algolianet.com/1/indexes/products/query?x-algolia-agent=Algolia%20for%20vanilla%20JavaScript%203.32.1&x-algolia-application-id=XW7SBCT9V6&x-algolia-api-key=6bfb5abee4dcd8cea8f0ca1ca085c2b3";
                string postdata = "{\"params\":\"query="+textBox1.Text.Trim()+"&facets=*&filters=\"}";
                    string html = PostUrl(url,postdata);
                Match huo = Regex.Match(html, @"""url"":""([\s\S]*?)""");
                string aurl = "https://stockx.com/api/products/"+huo.Groups[1].Value+"?includes=market,360&currency=USD&country=HK";

               
               string ahtml = gethtml(aurl, "");
               
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

                    lv2.SubItems.Add(lowprice.ToString());
                    lv2.SubItems.Add(highprice.ToString());


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


        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = gethtml("http://www.acaiji.com/index/index/vip.html", "");

            if (!html.Contains(@"stocks"))
            {

                MessageBox.Show("验证失败");
                return;


            }

            #endregion
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            button1.Enabled = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            listView1.Items.Clear();
        }
    }
}
