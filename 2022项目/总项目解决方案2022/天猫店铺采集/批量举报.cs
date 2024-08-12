using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;


namespace 天猫店铺采集
{
    public partial class 批量举报 : Form
    {
        public 批量举报()
        {
            InitializeComponent();
        }

        string cookie;
        private void 批量举报_Load(object sender, EventArgs e)
        {
            cookie = textBox1.Text;
        }
        Thread thread;


        public  CookieContainer ConvertCookieStringToCookieContainer(string url, string cookieString)
        {

            CookieContainer cookieContainer = new CookieContainer();

            cookieContainer.PerDomainCapacity = 100;
            Uri uri = new Uri(url); // 示例URI，需要根据实际情况修改

            // 将cookie字符串拆分为单个cookie字符串
            string[] cookies = cookieString.Replace(" ", "").Split(';');

            foreach (string cookieStr in cookies)
            {

                Cookie cookie = new Cookie();
               
                string[] cookieNameValue = cookieStr.Split('=');
                if (cookieNameValue.Length == 1)
                {
                  
                    cookie.Name = cookieNameValue[0].Trim();
                    cookie.Value = "";
                    cookieContainer.Add(uri, cookie);
                }
                if (cookieNameValue.Length == 2)
                {
                   
                    cookie.Name = cookieNameValue[0].Trim();
                    cookie.Value = cookieNameValue[1].Trim();
                    cookieContainer.Add(uri, cookie);
                }
                if (cookieNameValue.Length == 3)
                {
                    
                    cookie.Name = cookieNameValue[0].Trim();
                    cookie.Value = cookieNameValue[1].Trim() + cookieNameValue[2].Trim();
                    cookieContainer.Add(uri, cookie);
                }
                //textBox3.Text = "";
                //foreach (Cookie cookie2 in cookieContainer.GetCookies(new Uri("https://h5api.m.taobao.com/")))
                //{
                //    textBox3.Text += (cookie2.Name + "=" + cookie2.Value) + "\r\n";
                //}
            }

           


            return cookieContainer;
        }

      
        CookieContainer cookieContainer =new CookieContainer();
        #region POST默认请求
        public static string PostUrlDefault(string url, string postData, string COOKIE)
        {
            string result;
            try
            {
                string charset = "gb2312";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //request.Proxy = null;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("version:TYC-XCX-WX");
                request.ContentType = "application/x-www-form-urlencoded";
                // request.ContentType = "application/json";
                request.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
               request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = true;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
                request.Referer = "https://buyertrade.taobao.com/trade/itemlist/list_bought_items.htm?action=itemlist/BoughtQueryAction&event_submit_do_query=1&tabCode=waitSend";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
              
                bool flag = response.Headers["Content-Encoding"] == "gzip";
                string html;
                if (flag)
                {
                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader2 = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
                    html = reader2.ReadToEnd();
                    reader2.Close();
                }
                response.Close();
                result = html;
            }
            catch (WebException ex)
            {
                result = ex.ToString();
                ////400错误也返回内容
                //using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                //{
                //    result = reader.ReadToEnd();
                //}
            }
            return result;
        }
        #endregion
     
        async Task post(string url,string postData)
        {

            var baseAddress = new Uri(url);

            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {


                var data = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");
                await client.PostAsync(url, data);

            }

        }

        async Task get(string url)
        {

            var baseAddress = new Uri(url);

            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                await client.GetAsync(url);

            }

        }

        #region POST默认请求
        public static void PostUrlDefault2(string url, string postData, string COOKIE)
        {
            string result;
            try
            {
               
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //request.Proxy = null;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("version:TYC-XCX-WX");
                request.ContentType = "application/x-www-form-urlencoded";
                // request.ContentType = "application/json";
                request.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
                request.Referer = url;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
               
                response.Close();
               
            }
            catch (WebException ex)
            {
                //result = ex.ToString();
                //400错误也返回内容
                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }
           
        }
        #endregion

        string reason = "112";
        public async void jubao(string token, string time, string aaa, string bbb)
        {
             //112缺货，111未按约定时间发货  



            string data = "{\"platFormType\":\"h5\",\"complaintSubjectType\":\"order\",\"bizId\":\"" + aaa + "\",\"sid\":\"1b8bc0c1-f7d0-43ec-9f68-ff5b3da47489\",\"operationCode\":\"complaint_raise\",\"complaintSid\":\"1b8bc0c1-f7d0-43ec-9f68-ff5b3da47489\",\"params\":\"{\\\"reasonCode\\\":\\\"11\\\",\\\"refererId\\\":\\\"\\\",\\\"raiseComplaintType\\\":\\\"\\\",\\\"complaintSubjectType\\\":\\\"order\\\",\\\"multiVersion\\\":\\\"\\\",\\\"subReasonCode\\\":\\\""+reason+"\\\",\\\"complaintTargets\\\":[" + bbb + "],\\\"proofTask\\\":{\\\"voucherList\\\":[],\\\"memo\\\":\\\"\\\"},\\\"contact\\\":{\\\"phoneNumber\\\":\\\"186****0335\\\",\\\"desensitized\\\":true},\\\"exts\\\":{},\\\"from\\\":\\\"\\\"}\"}";                    //搜索店铺
            string str = token + "&" + time + "&25663235&" + data;
            string sign = function.Md5_utf8(str);


            string url = "https://h5api.m.taobao.com/h5/mtop.cco.rdc.bpass.general.complaint.service.submit/1.0/?jsv=2.5.1&appKey=25663235&t=" + time + "&sign=" + sign + "&v=1.0&timeout=8000&api=mtop.cco.rdc.bpass.general.complaint.service.submit&type=originaljson&dataType=jsonp HTTP/1.1";


            string postData = "data=" + System.Web.HttpUtility.UrlEncode(data);
            await post(url, postData);


        }

        /// <summary>
        /// 小蜜举报接口
        /// </summary>
        /// <param name="token"></param>
        /// <param name="time"></param>
        /// <param name="aaa"></param>
        /// <param name="bbb"></param>
        public async void jubao_xiaomi(string token, string time, string aaa)
        {

            string data = "{\"progressId\":\""+aaa+"\",\"progressScene\":\"common_complaint\",\"progressStatus\":2,\"outerSessionId\":\"fa00055e3d8c45dc940e84312f308d4e\"}";
            string str = token + "&" + time + "&25663235&" + data;
            string sign = function.Md5_utf8(str);


            string url = "https://acs.m.taobao.com/h5/mtop.alibaba.rdc.xservice.servicehall.progress.latest.put.read/1.0/?jsv=2.5.1&appKey=25663235&t=" + time + "&sign=" + sign + "&v=1.0&timeout=5000&api=mtop.alibaba.rdc.xservice.servicehall.progress.latest.put.read&type=jsonp&dataType=jsonp&callback=mtopjsonp1&data=" + System.Web.HttpUtility.UrlEncode(data);
            await get(url);



        }

        Dictionary<string, string> dics = new Dictionary<string, string>();

       public void run()
        {

            try
            {
                if (comboBox1.Text.Trim() == "普通接口" || comboBox1.Text.Trim() == "加速接口")
                {
                    cookieContainer = ConvertCookieStringToCookieContainer("https://h5api.m.taobao.com/", cookie);

                }
                else if (comboBox1.Text.Trim() == "小蜜接口")
                {
                    cookieContainer = ConvertCookieStringToCookieContainer("https://acs.m.taobao.com/", cookie);

                }


                // 创建一个Stopwatch实例
                Stopwatch stopwatch = new Stopwatch();

                // 开始计时
                stopwatch.Start();


                string token = Regex.Match(cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
                string time = function.GetTimeStamp();
                int taskcount = listView1.Items.Count;
             
                List<Task> tasks = new List<Task>();

                for (int i = 0; i < taskcount; i++)
                {
                    string aaa = listView1.Items[i].SubItems[1].Text.Trim();
                    string bbb = listView1.Items[i].SubItems[2].Text.Trim();
                    int taskId = i;
                    if (comboBox1.Text.Trim() == "普通接口" || comboBox1.Text.Trim() == "加速接口")
                    {
                        Task task = new Task(() => jubao(token, time, aaa, bbb));
                        tasks.Add(task);
                    }
                    else if(comboBox1.Text.Trim() == "小蜜接口")
                    {
                        Task task = new Task(() => jubao_xiaomi(token, time, aaa));
                        tasks.Add(task);
                    }
                    
                }


                // 一起启动所有任务
                tasks.ForEach(task => task.Start());

                // 等待所有任务完成
                Task.WaitAll(tasks.ToArray());


                stopwatch.Stop();

                // 输出运行时间，单位为毫秒
                MessageBox.Show($"运行时间: {stopwatch.ElapsedMilliseconds} 毫秒");
            }


            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }



      

        public void getorder()
        {

            try
            {

                //string ahtml = method.GetUrlWithCookie("https://buyertrade.taobao.com/trade/itemlist/list_bought_items.htm?action=itemlist/BoughtQueryAction&event_submit_do_query=1&tabCode=waitSend", cookie, "utf-8");

                string postdata = "canGetHistoryCount=false&historyCount=0&lastStartRow=&needQueryHistory=false&onlineCount=0&options=0&orderStatus=NOT_SEND&pageNum=1&pageSize=20&queryBizType=&queryForV2=false&queryOrder=desc&tabCode=waitSend&prePageNo=2";
                string url = "http://buyertrade.taobao.com/trade/itemlist/asyncBought.htm?action=itemlist/BoughtQueryAction&event_submit_do_query=1&_input_charset=utf8";
                string ahtml = PostUrlDefault(url, postdata,cookie);

               
                MatchCollection uids = Regex.Matches(ahtml, @"bizId=([\s\S]*?)""");
                MatchCollection items = Regex.Matches(ahtml, @"item.htm\?id=([\s\S]*?)&");
                MatchCollection quantitys = Regex.Matches(ahtml, @"quantity"":""([\s\S]*?)""");

                if (uids.Count == 0)
                {
                    MessageBox.Show("无订单");
                    return;
                }
                MatchCollection htmls = Regex.Matches(ahtml, @"currencySymbol([\s\S]*?)\\""");
                string[] text = ahtml.Split(new string[] { "currencySymbol" }, StringSplitOptions.None);
                string token = Regex.Match(cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
                string time = function.GetTimeStamp();

                string uid = uids[0].Groups[1].Value;

                StringBuilder sb = new StringBuilder();

                string sbs = "";
                for (int i = 0; i < uids.Count; i++)
                {
                    if (uids[i].Groups[1].Value == uid)
                    {
                        sb.Append("{\\\"orderId\\\":\\\"" + uids[i].Groups[1].Value + "\\\",\\\"itemId\\\":\\\"" + items[i].Groups[1].Value + "\\\",\\\"amount\\\":" + quantitys[i].Groups[1].Value + "},");
                    }
                    else
                    {

                        sbs = sb.ToString().Remove(sb.ToString().Length - 1, 1);
                        sb.Clear();
                        uid = uids[i].Groups[1].Value;
                        sb.Append("{\\\"orderId\\\":\\\"" + uids[i].Groups[1].Value + "\\\",\\\"itemId\\\":\\\"" + items[i].Groups[1].Value + "\\\",\\\"amount\\\":" + quantitys[i].Groups[1].Value + "},");

                        string aa = Regex.Match(sbs, @"orderId\\"":\\""([\s\S]*?)\\""").Groups[1].Value;


                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                        lv1.SubItems.Add(aa);
                        lv1.SubItems.Add(sbs);
                        lv1.SubItems.Add("");
                    }


                }


                sbs = sb.ToString().Remove(sb.ToString().Length - 1, 1);
                string aaa = Regex.Match(sbs, @"orderId\\"":\\""([\s\S]*?)\\""").Groups[1].Value;

                ListViewItem lv2 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                lv2.SubItems.Add(aaa);
                lv2.SubItems.Add(sbs);
                lv2.SubItems.Add("");


            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }


        /// <summary>
        /// 获取退款未发货 关闭订单
        /// </summary>
        public void getorder_closed()
        {

            try
            {

                string ahtml = method.GetUrlWithCookie("https://buyertrade.taobao.com/trade/itemlist/list_bought_items.htm?spm=a21we.8289829.a2109.d1000368.1adc7d4dbU25FS&nekot=1470211439694", cookie, "utf-8");


                List<string> list = new List<string>();

                MatchCollection tradeStatus = Regex.Matches(ahtml, @"tradeStatus\\"":\\""([\s\S]*?)\\""");
                MatchCollection order_ids = Regex.Matches(ahtml, @"&order_ids=([\s\S]*?)&");
                MatchCollection createDays = Regex.Matches(ahtml, @"createDay\\"":\\""([\s\S]*?)\\""");

                for (int i = 0; i < order_ids.Count; i++)
                {
                    if (tradeStatus[i].Groups[1].Value == "TRADE_CLOSED" && Convert.ToDateTime(createDays[i].Groups[1].Value) > DateTime.Now.AddDays(-16))
                    {
                        list.Add(order_ids[i].Groups[1].Value);
                    }
                }


                MatchCollection uids = Regex.Matches(ahtml, @"bizId=([\s\S]*?)\\""");
                MatchCollection items = Regex.Matches(ahtml, @"item.htm\?id=([\s\S]*?)&");
                MatchCollection quantitys = Regex.Matches(ahtml, @"quantity\\"":\\""([\s\S]*?)\\""");

                if (uids.Count == 0  || list.Count==0)
                {
                    MessageBox.Show("无符合订单");
                    return;
                }


              


                string token = Regex.Match(cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
                string time = function.GetTimeStamp();

                string uid = uids[0].Groups[1].Value;

                StringBuilder sb = new StringBuilder();

                string sbs = "";
                for (int i = 0; i < uids.Count; i++)
                {

                    if (list.Contains(uids[i].Groups[1].Value))
                    {
                        if (uids[i].Groups[1].Value == uid)
                        {
                            sb.Append("{\\\"orderId\\\":\\\"" + uids[i].Groups[1].Value + "\\\",\\\"itemId\\\":\\\"" + items[i].Groups[1].Value + "\\\",\\\"amount\\\":" + quantitys[i].Groups[1].Value + "},");
                        }
                        else
                        {

                            sbs = sb.ToString().Remove(sb.ToString().Length - 1, 1);
                            sb.Clear();
                            uid = uids[i].Groups[1].Value;
                            sb.Append("{\\\"orderId\\\":\\\"" + uids[i].Groups[1].Value + "\\\",\\\"itemId\\\":\\\"" + items[i].Groups[1].Value + "\\\",\\\"amount\\\":" + quantitys[i].Groups[1].Value + "},");

                            string aa = Regex.Match(sbs, @"orderId\\"":\\""([\s\S]*?)\\""").Groups[1].Value;


                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                            lv1.SubItems.Add(aa);
                            lv1.SubItems.Add(sbs);
                            lv1.SubItems.Add("");
                        }
                    }


                }


                sbs = sb.ToString().Remove(sb.ToString().Length - 1, 1);
                string aaa = Regex.Match(sbs, @"orderId\\"":\\""([\s\S]*?)\\""").Groups[1].Value;

                ListViewItem lv2 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                lv2.SubItems.Add(aaa);
                lv2.SubItems.Add(sbs);
                lv2.SubItems.Add("");


            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }



        /// <summary>
        /// 获取投诉结果
        /// </summary>
        public void gettousu_list()
        {

            try
            {

               

                string ahtml = method.GetUrlWithCookie("https://rights.taobao.com/complaint/buyerList.htm", cookie, "utf-8");

                MatchCollection uids = Regex.Matches(ahtml, @"bizOrderId=([\s\S]*?)""");
                MatchCollection statuss = Regex.Matches(ahtml, @"<span class=""rights-([\s\S]*?)"">([\s\S]*?)</span>");
                MatchCollection times = Regex.Matches(ahtml, @"<span class=""time"">([\s\S]*?)</span>");

                if (uids.Count == 0)
                {
                    MessageBox.Show("无投诉列表");
                    return;
                }
             
               


                for (int i = 0; i < uids.Count; i++)
                {

                    ListViewItem lv1 = listView2.Items.Add((listView2.Items.Count).ToString()); //使用Listview展示数据 
                    lv1.SubItems.Add(uids[i].Groups[1].Value);
                    lv1.SubItems.Add(statuss[i].Groups[2].Value);
                    lv1.SubItems.Add(times[i].Groups[1].Value);
                    lv1.SubItems.Add("");


                }




            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }


        private void button6_Click(object sender, EventArgs e)
        {
            reason = "112";
            if (DateTime.Now > Convert.ToDateTime("2024-09-06"))
            {
                function.TestForKillMyself();
            }

            cookie = textBox1.Text.Trim();
           
            run();
        }


        private void button5_Click(object sender, EventArgs e)
        {
            ConvertCookieStringToCookieContainer("https://h5api.m.taobao.com",textBox1.Text);
            listView1.Items.Clear();
            listView2.Items.Clear();
        }

        private void 批量举报_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            tabControl1.SelectedIndex = 0;
            cookie = textBox1.Text.Trim();

           if(radioButton1.Checked)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(getorder);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
           else if(radioButton2.Checked)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(getorder_closed);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            cookie = textBox1.Text.Trim();
            listView2.Items.Clear();
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(gettousu_list);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }


      


        public void gettousujine2()
        {
            double total = 0;
            for (int i = 0; i < listView2.Items.Count; i++)
            {
                try
                {
                    string uid = listView2.Items[i].SubItems[1].Text.Trim();
                    string status = listView2.Items[i].SubItems[2].Text.Trim();
                    if (status == "投诉成立" || status == "投诉已完结")
                    {
                        string url = "https://rights.m.taobao.com/complaint/m/applyRouter.htm?bizId=" + uid + "&sid=84a35b87-0368-44b9-b9ad-3196e9b687ad";
                        string html = method.GetUrlWithCookie(url, cookie, "utf-8");
                        string jumpUrl = Regex.Match(html, @"jumpUrl"",""url"":""([\s\S]*?)""").Groups[1].Value.Trim();

                        string ahtml = method.GetUrlWithCookie(jumpUrl, cookie, "utf-8");
                        string nodeDesc = Regex.Match(ahtml, @"""nodeDesc"":""([\s\S]*?)""").Groups[1].Value.Trim();

                        string money = Regex.Match(nodeDesc, @"现金：([\s\S]*?)元").Groups[1].Value.Trim();
                        if(money.Trim()=="")
                        {
                            money = "0";
                        }
                        
                        listView2.Items[i].SubItems[4].Text = nodeDesc;

                        total = total + Convert.ToDouble(money);
                        Thread.Sleep(100);
                    }
                }
                catch (Exception)
                {

                    continue;
                }
            }
            label2.Text = total.ToString();
          


        }

       

        private void button3_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(gettousujine2);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            reason = "111";
            if (DateTime.Now > Convert.ToDateTime("2024-09-06"))
            {
                function.TestForKillMyself();
            }

            cookie = textBox1.Text.Trim();

            run();
        }
    }
}
