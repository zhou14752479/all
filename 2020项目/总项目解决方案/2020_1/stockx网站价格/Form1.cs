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





        string cookie = "__cfduid=d101e1ff3afc336393f36fb084d7187c21608772971; stockx_homepage=sneakers; language_code=en; stockx_market_country=CN; _ga=GA1.2.1838221857.1608772970; _gid=GA1.2.269794033.1608772970; _pxvid=8ba8ff94-4586-11eb-9e17-0242ac12000e; tracker_device=f9d941ec-1ac8-43da-a0ab-1007512f9fe4; is_gdpr=false; cookie_policy_accepted=true; stockx_ip_region=CN; stockx_session=ee492459-0f2b-4475-8312-d999c30acc62; below_retail_type=; bid_ask_button_type=; brand_tiles_version=v1; browse_page_tile_size_update_web=true; bulk_shipping_enabled=true; default_apple_pay=false; intl_payments=true; multi_edit_option=beatLowestAskBy; product_page_affirm_callout_enabled_web=false; related_products_length=v2; riskified_recover_updated_verbiage=true; show_all_as_number=false; show_bid_education=v2; show_bid_education_times=1; show_how_it_works=true; show_watch_modal=true; pdp_refactor_web=undefined; recently_viewed_web_home=false; ops_delay_messaging_pre_checkout_ask=false; ops_delay_messaging_post_checkout_ask=false; ops_delay_messaging_selling=false; ops_delay_messaging_buying=false; ops_delay_messaging_ask_status=false; ops_delay_messaging_bid_status=false; ops_delay_messaging_pre_checkout_buy=false; ops_delay_messaging_post_checkout_buy=false; salesforce_chatbot_prod=true; web_low_inv_checkout=v0; _gcl_au=1.1.1530951235.1608772975; IR_gbd=stockx.com; _scid=ed10bff8-b5f9-4e30-b363-15f3a949c074; _pk_ses.421.1a3e=*; stockx_selected_locale=en; stockx_selected_region=CN; stockx_dismiss_modal=true; stockx_dismiss_modal_set=2020-12-24T01%3A22%3A56.989Z; stockx_dismiss_modal_expiration=2021-12-24T01%3A22%3A56.988Z; _px3=b162b7f8253b8750421128a69ca49870e23dad278d79495b85be0cbee9b7ac8a:6fVf1apjlCBExOUKoyDJsP57+KN+MfN+SU4iQysMtohqCJHedX8d+J5ykohyxLF0Tb9Bwlgr+G3pILuZmpxNfQ==:1000:kJyvzTcY8oUi0KOqdAU8nqquxlRidQ4e0ItTESIuHZID1+DBIAdfXLkraadd6A9ULXcTkUNKzcDpk/Ud5/E+OzQSZYGbuzHK5oh0FTOaJ3sVtPrL9RX9bGNaSjV2qoTLQYX3HssB2S/86Y28MnncnOVgfKiT45vNnpk9r+q0Qck=; _pk_id.421.1a3e=8ca7f79043af9b2c.1608772976.1.1608772981.1608772976.; IR_9060=1608772981305%7C0%7C1608772975834%7C%7C; IR_PI=03762d2a-0097-11eb-98c6-0684bff74260%7C1608859381305; lastRskxRun=1608773019133; rskxRunCookie=0; rCookie=jhbgt4gy0f9vgj90em797kj25x24f; QuantumMetricUserID=9fc849c3f1cc516e1d1fb915483fffe0; QuantumMetricSessionID=ac8a8358389363aa4ecde5374ddc1e5f; _dd_s=rum=1&id=bf087f68-02a3-4df8-88ee-ffe12b3d77d9&created=1608772972396&expire=1608774088065";

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

            button1.Text = "点击获取";
            button1.Enabled = true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

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
    }
}
