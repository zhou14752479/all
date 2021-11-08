using System;
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
using myDLL;

namespace 快递助手
{
    public partial class 快递助手 : Form
    {
        public 快递助手()
        {
            InitializeComponent();
        }

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public  string PostUrlDefault(string url, string postData, string COOKIE)
        {
            try
            {

                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //request.Proxy = null;//防止代理抓包
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("qnquerystring: "+qnquerystring);
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                request.ContentType = "application/x-www-form-urlencoded";
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                //request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = url;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

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
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion

        public string taobaoId = "";
        public string token = "";
        
        public string qnquerystring = "";
        bool zanting = true;
        bool status = true;
        Thread thread;
        public void gettoken()
        {
            try
            {
                taobaoId = Regex.Match(textBox1.Text, @"taobaoId=([\s\S]*?)&").Groups[1].Value;
                token = Regex.Match(textBox1.Text, @"kdzsToken=([\s\S]*?)&").Groups[1].Value;
                qnquerystring = taobaoId + "_" + token;
            }
            catch (Exception ex)
            {
             MessageBox.Show(ex.ToString())   ;
            }
        }


        public string getdetail(string html)
        {
            MatchCollection tids = Regex.Matches(html, @"tids"":\[([\s\S]*?)\]");
            MatchCollection buyerNicks = Regex.Matches(html, @"""buyerNick"":""([\s\S]*?)""");
            MatchCollection togetherIds = Regex.Matches(html, @"""togetherId"":""([\s\S]*?)""");
            MatchCollection taobaoIds = Regex.Matches(html, @"""taobaoId"":""([\s\S]*?)""");
            MatchCollection fenxiaos = Regex.Matches(html, @"""fenxiaos"":([\s\S]*?),");
            MatchCollection rdsTradeInfos = Regex.Matches(html, @"""rdsTradeInfo"":""([\s\S]*?)\}""");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < tids.Count; i++)
            {
                sb.Append("{\"tids\":\""+tids[i].Groups[1].Value.Replace("\"","")+ "\",\"buyerNick\":\""+buyerNicks[i].Groups[1].Value+ "\",\"togetherId\":\""+togetherIds[i].Groups[1].Value+ "\",\"taobaoId\":\""+ taobaoIds[i].Groups[1].Value+"\",\"fenxiaos\":"+fenxiaos[i].Groups[1].Value+ ",\"rdsTradeInfo\":\""+rdsTradeInfos[i].Groups[1].Value+"}"+ "\"},");
            }

            string postdata= "togetherIds=%5B" + System.Web.HttpUtility.UrlEncode(sb.ToString().Remove(sb.ToString().Length - 1, 1))+ "%5D&randCacheKey=1851112970823529&api_name=get_batch_trade_detail";
            string url = "https://p4.kuaidizs.cn/trade/getBatchTradeList";
        
            string ahtml = PostUrlDefault(url, postdata, "");
            return ahtml;

        }


        public string getmobile(string html)
        {

            string receiverSecret = Regex.Match(html, @"""receiverSecret"":""([\s\S]*?)""").Groups[1].Value;
            string mobileSecret = Regex.Match(html, @"""mobileSecret"":""([\s\S]*?)""").Groups[1].Value;
            string tid = Regex.Match(html, @"""tid"":""([\s\S]*?)""").Groups[1].Value;
            string taobaoId = Regex.Match(html, @"""taobaoId"":""([\s\S]*?)""").Groups[1].Value;
            string tidOaid = Regex.Match(html, @"""oaid"":""([\s\S]*?)""").Groups[1].Value;
           
           string sb= "{\"receiverSecret\":\""+receiverSecret+"\",\"mobileSecret\":\""+mobileSecret+"\",\"tid\":\""+tid+"\",\"taobaoId\":\""+taobaoId+"\",\"sceneCode\":\"100\",\"tidOaid\":\""+tidOaid+"\"}";
            

            string postdata = "decryData=%5B" + System.Web.HttpUtility.UrlEncode(sb.ToString()) + "%5D&api_name=decryBlurData";
            string url = "https://p4.kuaidizs.cn/user/decryBlurData";
         
            string ahtml = PostUrlDefault(url, postdata, "");
            return ahtml;
        }

       
        public void run()
        {
            List<string> list = new List<string>();
            //已发货
            //WAIT_BUYER_CONFIRM_GOODS

            //WAIT_SELLER_SEND_GOODS
            try
            {
                
                string type = "WAIT_BUYER_CONFIRM_GOODS";
                if (radioButton1.Checked == true)
                {
                    type = "WAIT_SELLER_SEND_GOODS";
                }
                if (radioButton2.Checked==true)
                {
                    type = "WAIT_BUYER_CONFIRM_GOODS";
                   
                }
                for (int i = 1; i < 999; i++)
                {

                    string url = "https://p4.kuaidizs.cn/trade/queryTrade";
                    string postdata = "";
                    if (type == "WAIT_SELLER_SEND_GOODS")
                    {
                         postdata = "areaJson=&areaContain=&status=" + type + "&timeType=1&taobaoIds=" + taobaoId + "&startTime=" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "%2000%3A00%3A00&endTime=" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "%2023%3A59%3A59&filterByTrade=false&addrRule=&stockType=&pageNo=" + i + "&pageSize=100&random=1635503850308&buyerMessage=&buyerNick=&numIid=&buyerPhone=&stall=&colorIncluding=&colorNotIncluding=&flagValue=&groupFlag=&goodsTotalNum=&goodsTypeNum=&payment=&printStatus=&receiverName=&refundStatus=&labelId=&sellAttribute=&quickQuery=&surplusSendTimeType=&sellerMemo=&tradeFrom=&shortNameIncluding=&shortNameNotIncluding=&itemShortTitleIncluding=&itemShortTitleNotIncluding=&skuIncluding=&skuNotIncluding=&tid=&tradeNum=&type=&tradeWeight=&weightRange=&customType=notUse&flagValueStr=&fuzzySearch=true&partSearch=false&systemTimeStamp=1635503851267&api_name=get_trade_num"; //&pageRand=8886772201272562739
                    }
                    if (type == "WAIT_BUYER_CONFIRM_GOODS")
                    {
                         postdata = "areaJson=&areaContain=&status=all&timeType=1&taobaoIds=" + taobaoId + "&startTime=" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "%2000%3A00%3A00&endTime=" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "%2023%3A59%3A59&filterByTrade=false&addrRule=&stockType=&pageNo=" + i + "&pageSize=100&random=1635731948266&buyerMessage=&buyerNick=&numIid=&buyerPhone=&stall=&colorIncluding=&colorNotIncluding=&flagValue=&groupFlag=&goodsTotalNum=&goodsTypeNum=&payment=&printStatus=&receiverName=&refundStatus=&labelId=&sellAttribute=&quickQuery=&surplusSendTimeType=&sellerMemo=&tradeFrom=&shortNameIncluding=&shortNameNotIncluding=&itemShortTitleIncluding=&itemShortTitleNotIncluding=&skuIncluding=&skuNotIncluding=&tid=&tradeNum=&type=&tradeWeight=&weightRange=&customType=sid&flagValueStr=&kdName=%E4%BA%AC%E5%B9%BF%E9%80%9F%E9%80%92&sid=&fuzzySearch=true&partSearch=false&systemTimeStamp=1635731948885&api_name=get_trade_num&pageRandKey=e428f58a08f1241d602d37ccb9cadcf22970823529";
                    }
                    string html = PostUrlDefault(url,postdata,"");
                    string result = getdetail(html);

                    MatchCollection ahtmls = Regex.Matches(result, @"""abnormalAddress""([\s\S]*?)upAddressTag");
                 
                    for (int j = 0; j < ahtmls.Count; j++)
                    {
                        MatchCollection skuPropertiesNames = Regex.Matches(ahtmls[j].Groups[1].Value , @"""skuPropertiesName"":""([\s\S]*?)""");
                        MatchCollection nums = Regex.Matches(ahtmls[j].Groups[1].Value, @"""num"":([\s\S]*?),");


                        MatchCollection kdNames = Regex.Matches(ahtmls[j].Groups[1].Value, @"""kdName"":""([\s\S]*?)""");
                        MatchCollection ydNos = Regex.Matches(ahtmls[j].Groups[1].Value, @"""ydNo"":""([\s\S]*?)""");

                        string sellerMemo = Regex.Match(ahtmls[j].Groups[1].Value, @"""sellerMemo"":""([\s\S]*?)""").Groups[1].Value;


                        //string mobilehtml = getmobile(ahtmls[j].Groups[1].Value);
                      
                        //string address= Regex.Match(mobilehtml, @"""decryState"":""([\s\S]*?)""").Groups[1].Value+ Regex.Match(mobilehtml, @"""decryCity"":""([\s\S]*?)""").Groups[1].Value+ Regex.Match(mobilehtml, @"""decryDistrict"":""([\s\S]*?)""").Groups[1].Value+ Regex.Match(mobilehtml, @"""decryAddressDetail"":""([\s\S]*?)""").Groups[1].Value;
                        //string decryReceiver = Regex.Match(mobilehtml, @"""decryReceiver"":""([\s\S]*?)""").Groups[1].Value;
                        //string decryMobile = Regex.Match(mobilehtml, @"""decryMobile"":""([\s\S]*?)""").Groups[1].Value;

                        for (int a = 0; a < skuPropertiesNames.Count; a++)
                        {
                            label2.Text = "正在抓取...."+ skuPropertiesNames[a].Groups[1].Value.Trim();
                            if (skuPropertiesNames[a].Groups[1].Value.Trim() != "")
                            {

                                if (type == "WAIT_BUYER_CONFIRM_GOODS")
                                {
                                    string mobilehtml = getmobile(ahtmls[j].Groups[1].Value);

                                    string address = Regex.Match(mobilehtml, @"""decryState"":""([\s\S]*?)""").Groups[1].Value + Regex.Match(mobilehtml, @"""decryCity"":""([\s\S]*?)""").Groups[1].Value + Regex.Match(mobilehtml, @"""decryDistrict"":""([\s\S]*?)""").Groups[1].Value + Regex.Match(mobilehtml, @"""decryAddressDetail"":""([\s\S]*?)""").Groups[1].Value;
                                    string decryReceiver = Regex.Match(mobilehtml, @"""decryReceiver"":""([\s\S]*?)""").Groups[1].Value;
                                    string decryMobile = Regex.Match(mobilehtml, @"""decryMobile"":""([\s\S]*?)""").Groups[1].Value;
                                   
                                    if(list.Contains(skuPropertiesNames[a].Groups[1].Value.Trim()+decryReceiver + decryMobile + address))
                                    {
                                        MessageBox.Show("完成");
                                        return;
                                    }
                                    list.Add(skuPropertiesNames[a].Groups[1].Value.Trim()+decryReceiver + decryMobile + address);
                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                    lv1.SubItems.Add(skuPropertiesNames[a].Groups[1].Value.Trim());
                                    lv1.SubItems.Add(nums[a].Groups[1].Value.Trim());
                                    lv1.SubItems.Add(decryReceiver);
                                    lv1.SubItems.Add(decryMobile);
                                    lv1.SubItems.Add(address);
                                    lv1.SubItems.Add(ydNos[a].Groups[1].Value.Trim());
                                    lv1.SubItems.Add(sellerMemo);
                                    Thread.Sleep(1000);


                                }
                                else
                                {
                                    string mobilehtml = getmobile(ahtmls[j].Groups[1].Value);

                                    string address = Regex.Match(mobilehtml, @"""decryState"":""([\s\S]*?)""").Groups[1].Value + Regex.Match(mobilehtml, @"""decryCity"":""([\s\S]*?)""").Groups[1].Value + Regex.Match(mobilehtml, @"""decryDistrict"":""([\s\S]*?)""").Groups[1].Value + Regex.Match(mobilehtml, @"""decryAddressDetail"":""([\s\S]*?)""").Groups[1].Value;
                                    string decryReceiver = Regex.Match(mobilehtml, @"""decryReceiver"":""([\s\S]*?)""").Groups[1].Value;
                                    string decryMobile = Regex.Match(mobilehtml, @"""decryMobile"":""([\s\S]*?)""").Groups[1].Value;
                                 
                                    if (list.Contains(skuPropertiesNames[a].Groups[1].Value.Trim()+decryReceiver + decryMobile + address))
                                    {
                                        MessageBox.Show("完成");
                                        return;
                                    }

                                    list.Add(skuPropertiesNames[a].Groups[1].Value.Trim()+decryReceiver + decryMobile + address);
                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                    lv1.SubItems.Add(skuPropertiesNames[a].Groups[1].Value.Trim());
                                    lv1.SubItems.Add(nums[a].Groups[1].Value.Trim());
                                    lv1.SubItems.Add(decryReceiver);
                                    lv1.SubItems.Add(decryMobile);
                                    lv1.SubItems.Add(address);
                                    lv1.SubItems.Add("无");
                                    lv1.SubItems.Add(sellerMemo);
                                    Thread.Sleep(1000);
                                }



                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                                if (status == false)
                                    return;
                               
                            }
                            

                           

                        }

                    }

                   
                        
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            
        }
        private void 快递助手_Load(object sender, EventArgs e)
        {
            #region 通用检测

            if (DateTime.Now > Convert.ToDateTime("2022-01-13"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            #endregion
            dateTimePicker1.Value = DateTime.Now.AddDays(-30);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            status = true;
            gettoken();
            if (taobaoId=="" || token=="")
            {
                MessageBox.Show("请复制网址到输入框");
                return;
            }
            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
