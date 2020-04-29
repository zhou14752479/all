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

        #region GET请求解决基础连接关闭无法获取HTML
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string outStr = "";
            string tmpStr = "";

            try
            {
                // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                request.AllowAutoRedirect = false;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 10000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                try
                {//循环获取
                    while ((tmpStr = reader.ReadLine()) != null)
                    {
                        outStr += tmpStr;
                    }
                }
                catch
                {

                }
                reader.Close();
                response.Close();

                return outStr;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
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

        public ArrayList getfee(string sku, string price)
        {
            ArrayList lists = new ArrayList();
            try
            {

                string url = "https://stockx.com/api/pricing?currency=USD&include_taxes=false";
                string postdata = "{\"context\":\"buying\",\"products\":[{\"sku\":\"" + sku + "\",\"amount\":" + price + ",\"quantity\":1}],\"discountCodes\":[\"\"]}";
                
                string html = PostUrl(url, postdata);
                
                MatchCollection fee = Regex.Matches(html, @"""amount"":([\s\S]*?),");

                foreach (Match item in fee)
                {
                    lists.Add(item.Groups[1].Value);
                }
                return lists;
            }
            catch (Exception)
            {

                throw;
            }

        }







            public void run()
        {

            try
            {
                
                    string url = "https://xw7sbct9v6-1.algolianet.com/1/indexes/products/query?x-algolia-agent=Algolia%20for%20vanilla%20JavaScript%203.32.1&x-algolia-application-id=XW7SBCT9V6&x-algolia-api-key=6bfb5abee4dcd8cea8f0ca1ca085c2b3";
                string postdata = "{\"params\":\"query="+textBox1.Text.Trim()+"&facets=*&filters=\"}";
                    string html = PostUrl(url,postdata);
                Match huo = Regex.Match(html, @"""url"":""([\s\S]*?)""");
                string aurl = "https://stockx.com/api/products/"+huo.Groups[1].Value+"?includes=market,360&currency=USD&country=HK";
                

                string ahtml = GetUrl(aurl, "utf-8");
                
                Match highestBid = Regex.Match(ahtml, @"""highestBid"":([\s\S]*?),");
              

                    MatchCollection skus = Regex.Matches(ahtml, @"""skuUuid"":([\s\S]*?),");


                MatchCollection sizes = Regex.Matches(ahtml, @"""lowestAskSize"":([\s\S]*?),");
                MatchCollection lows = Regex.Matches(ahtml, @"""lowestAsk"":([\s\S]*?),");
                MatchCollection highs = Regex.Matches(ahtml, @"""highestBidFloat"":([\s\S]*?)}");



              


                for (int j = 1; j < lows.Count; j++)
                {

                    ListViewItem lv2 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv2.SubItems.Add("US "+sizes[j].Groups[1].Value);

                    double lowprice = Convert.ToDouble(textBox2.Text) * Convert.ToDouble(lows[j].Groups[1].Value);
                    double highprice = Convert.ToDouble(textBox2.Text) * Convert.ToDouble(highs[j].Groups[1].Value);

                    lv2.SubItems.Add(lowprice.ToString());
                    lv2.SubItems.Add(highprice.ToString());

                   
                    ArrayList fees = getfee(skus[j].Groups[1].Value.Replace("\"",""), lows[j].Groups[1].Value);

                   //double fee1 = Convert.ToDouble(textBox2.Text) * Convert.ToDouble(fees[0].ToString());
                   //double fee2 = Convert.ToDouble(textBox2.Text) * Convert.ToDouble(fees[1].ToString());
                    lv2.SubItems.Add(fees[0].ToString());
                    lv2.SubItems.Add(fees[1].ToString());

                   
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

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"stocks"))
            {

                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
            
        }
    }
}
