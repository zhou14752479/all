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
    public partial class 兴盛优选 : Form
    {
        public 兴盛优选()
        {
            InitializeComponent();
        }

        bool status = true;
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
                //request.ContentType = "application/x-www-form-urlencoded";

                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("userKey:01347ae7-beac-47e4-8d6c-242d840e1736");
             
                //添加头部
                request.ContentType = "application/json";
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "https://web.duanmatong.cn/";
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


        public string gettel(string storeid)
        {
            string tel = "";
            string url = "https://mall-store.xsyxsc.com/mall-store/store/getStoreContactsTelProtected";
            string postdata = "{\"userKey\":\"01347ae7-beac-47e4-8d6c-242d840e1736\",\"storeId\":"+storeid+",\"businessScene\":\"GRZX\"}";
            string html = PostUrl(url,postdata,"","utf-8");
            tel = Regex.Match(html, @"""contactsTel"":""([\s\S]*?)""").Groups[1].Value;
            return tel;

        }
        public void run()
        {

            try
            {

                string keyword = System.Web.HttpUtility.UrlEncode(textBox1.Text);


                for (int i = 1; i < 9999; i++)
                {

                    string url = "https://mall-store.xsyxsc.com/mall-store/store/queryStoreList?page="+i+"&rows=100&storeName="+System.Web.HttpUtility.UrlEncode(textBox1.Text)+ "&userKey=48151f8c-d98c-4694-95cf-e22f54c93e94";

                   
                    string html = method.GetUrl(url, "utf-8");
                    MatchCollection titles = Regex.Matches(html, @"""storeName"":""([\s\S]*?)""");

                 
                    MatchCollection storeIds = Regex.Matches(html, @"""storeId"":([\s\S]*?),");
                    MatchCollection address = Regex.Matches(html, @"""detailAddress"":""([\s\S]*?)""");

                    if (titles.Count == 0)
                        return;

                    for (int j = 0; j < titles.Count; j++)
                    {
                        try
                        {



                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(titles[j].Groups[1].Value);
                         
                            lv1.SubItems.Add(gettel(storeIds[j].Groups[1].Value));
                            lv1.SubItems.Add(address[j].Groups[1].Value);
                            while (zanting == false)
                            {
                                Application.DoEvents();//等待本次加载完毕才执行下次循环.
                            }
                            if (status == false)
                                return;
                            Thread.Sleep(1000);
                        }
                        catch
                        {

                            continue;
                        }



                    }


                }
            }
            catch (Exception)
            {

                throw;
            }




        }
        private void 兴盛优选_Load(object sender, EventArgs e)
        {
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"xsyx"))
            {
                button1.Enabled = false;
                status = true;
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

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            status = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
        bool zanting = true;
        private void button5_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {
                zanting = true;
            }
            if (zanting == true)
            {
                zanting = false;
            }
        }
    }
}
