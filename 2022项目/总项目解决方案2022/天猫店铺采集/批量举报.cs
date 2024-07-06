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
        bool status = true;
        bool zanting = true;


        #region POST默认请求
        public static string PostUrlDefault(string url, string postData, string COOKIE)
        {
            string result;
            try
            {
                string charset = "utf-8";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
               
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
                response.GetResponseHeader("Set-Cookie");
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
                //result = ex.ToString();
                //400错误也返回内容
                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }
        #endregion



        public void run()
        {

        

            try
            {

                string ahtml = method.GetUrlWithCookie("https://buyertrade.taobao.com/trade/itemlist/list_bought_items.htm?action=itemlist/BoughtQueryAction&event_submit_do_query=1&tabCode=waitSend", cookie,"utf-8");

                MatchCollection uids = Regex.Matches(ahtml, @"bizId=([\s\S]*?)\\""");
                MatchCollection items = Regex.Matches(ahtml, @"item.htm\?id=([\s\S]*?)&");

                for (int i = 0; i < uids.Count; i++)
                {
 
                    string token = Regex.Match(cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;

                    string time = function.GetTimeStamp();

                    string orderid=uids[i].Groups[1].ToString();  
                    string itemid=items[i].Groups[1].ToString();  

                    string data = "{\"platFormType\":\"h5\",\"complaintSubjectType\":\"order\",\"bizId\":\""+orderid+"\",\"sid\":\"1b8bc0c1-f7d0-43ec-9f68-ff5b3da47489\",\"operationCode\":\"complaint_raise\",\"complaintSid\":\"1b8bc0c1-f7d0-43ec-9f68-ff5b3da47489\",\"params\":\"{\\\"reasonCode\\\":\\\"11\\\",\\\"refererId\\\":\\\"\\\",\\\"raiseComplaintType\\\":\\\"\\\",\\\"complaintSubjectType\\\":\\\"order\\\",\\\"multiVersion\\\":\\\"\\\",\\\"subReasonCode\\\":\\\"112\\\",\\\"complaintTargets\\\":[{\\\"orderId\\\":\\\""+orderid+"\\\",\\\"amount\\\":42,\\\"itemId\\\":\\\""+itemid+"\\\"}],\\\"proofTask\\\":{\\\"voucherList\\\":[],\\\"memo\\\":\\\"\\\"},\\\"contact\\\":{\\\"phoneNumber\\\":\\\"186****0335\\\",\\\"desensitized\\\":true},\\\"exts\\\":{},\\\"from\\\":\\\"\\\"}\"}";                    //搜索店铺
                    string str = token + "&" + time + "&25663235&" + data;
                    string sign = function.Md5_utf8(str);

                   
                 
                    string url = "https://h5api.m.taobao.com/h5/mtop.cco.rdc.bpass.general.complaint.service.submit/1.0/?jsv=2.5.1&appKey=25663235&t="+time+"&sign="+sign+"&v=1.0&timeout=8000&api=mtop.cco.rdc.bpass.general.complaint.service.submit&type=originaljson&dataType=jsonp HTTP/1.1";


                    string postData = "data="+data;
                    string html = PostUrlDefault(url,postData,cookie);

                  
                    textBox4.Text =  html;
                    //MessageBox.Show(html);
             
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                 
                    lv1.SubItems.Add(orderid);
                  
                    if(html.Contains("SUCCESS"))
                    {
                        lv1.SubItems.Add("成功");
                    }
                   else
                    {
                        lv1.SubItems.Add("");
                    }
             

                    while (zanting == false)
                    {
                        Application.DoEvents();//等待本次加载完毕才执行下次循环.
                    }
                    if (status == false)
                        return;
                }

                //Thread.Sleep(1000);

                MessageBox.Show("完成");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (DateTime.Now > Convert.ToDateTime("2024-08-06"))
            {
                function.TestForKillMyself();
            }



            cookie = textBox1.Text.Trim();
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
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

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
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
    }
}
