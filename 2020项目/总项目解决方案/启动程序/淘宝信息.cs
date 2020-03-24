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
    public partial class 淘宝信息 : Form
    {
        public 淘宝信息()
        {
            InitializeComponent();
        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "thw=cn; ali_ab=49.94.92.171.1563332665663.4; x=e%3D1%26p%3D*%26s%3D0%26c%3D0%26f%3D0%26g%3D0%26t%3D0%26__ll%3D-1%26_ato%3D0; hng=CN%7Czh-CN%7CCNY%7C156; enc=rY0GpAFgrh5bXXfBXutSHaQSm6aOCly2Ov5qI2xvmmRLzA74CWx0R1R%2FH4RXdUTECCRII572ywPqHDXt8ypRKg%3D%3D; cna=8QJMFUu4DhACATFZv2JYDtwd; t=027e7e2bc53b51842bd6d63b5b90ab8a; lgc=zkg852266010; tracknick=zkg852266010; tg=0; mt=ci=63_1; supportWebp=false; cookie2=181b7711d7188a4bf736421e46826527; v=0; _tb_token_=383ee4e63403e; _m_h5_tk=eaabe168a8a3da31a078c6669247fae5_1585038353892; _m_h5_tk_enc=197614bc1237cb71b05a49a23fe5899b; _samesite_flag_=true; sgcookie=ERqatv88zb%2BPY8JCJq%2Fjp; unb=1052347548; uc3=vt3=F8dBxd9gD5V4rEegPZI%3D&lg2=Vq8l%2BKCLz3%2F65A%3D%3D&id2=UoH62EAv27BqSg%3D%3D&nk2=GcOvCmiKUSBXqZNU; csg=b1b73a7c; cookie17=UoH62EAv27BqSg%3D%3D; dnk=zkg852266010; skt=dd5b1a74db8ad860; existShop=MTU4NTAzMDQ2NQ%3D%3D; uc4=nk4=0%40GwrkntVPltPB9cR46GnfGp2j032Mblg%3D&id4=0%40UOnlZ%2FcoxCrIUsehKGOnx0nF9v3J; _cc_=Vq8l%2BKCLiw%3D%3D; _l_g_=Ug%3D%3D; sg=080; _nk_=zkg852266010; cookie1=Vvj8uMJubtxirKFtxaDmWPxYCP5sb7EKtrFe1w68JDk%3D; tfstk=cRlNBV4bbCdwNQ9HF7P455uiLs0OZy0io6z7IYuYifzhMyyGiBsYRXj88rZJtRf..; uc1=cookie16=URm48syIJ1yk0MX2J7mAAEhTuw%3D%3D&pas=0&existShop=true&tag=8&cookie15=Vq8l%2BKCLz3%2F65A%3D%3D&lng=zh_CN&cookie21=URm48syIZJfmZ9wVCtpzEQ%3D%3D&cookie14=UoTUP2Kh859I0Q%3D%3D; l=dBTc_4AlQLoCcqlYBOCgd42gwx79vIRAgukoSa2vi_5pN6L_5mbOoP9XRFp6cjWftjLB40tUd_v9-etkwQHmndB81VtX1xDc.; linezing_session=7nVvX8QUXPX7G3Z9oqSTAvmk_1585033747182t5Uv_25; isg=BJKSSWP7GFRKA2RfTuOMmrtr41h0o5Y9kGywFVzrOcUwbzNpRDfcTdCd2cvTBA7V";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://shopsearch.taobao.com/search?q=alphastyle%E6%97%97%E8%88%B0%E5%BA%97&js=1&initiative_id=staobaoz_20200320&ie=utf8";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.108 Safari/537.36";
                request.AllowAutoRedirect = false;
                request.Headers.Add("Cookie", COOKIE);
                
                WebHeaderCollection headers = request.Headers;
                headers.Add("upgrade-insecure-requests: 1");
               
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();


                reader.Close();
                response.Close();
                return content;



            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion
        bool zanting = true;


        public string gettime(string userid)
        {
            string url = "https://rate.taobao.com/member_rate.htm?_ksTS=1584683650986_156&callback=shop_rate_list&content=1&result=&from=rate&user_id="+ userid + "&identity=2&rater=0&direction=0";
            string html = GetUrl(url, "utf-8");
            Match time = Regex.Match(html, @"""date"":""([\s\S]*?)""");
            return time.Groups[1].Value;
        }

        public string getcreattime(string userid)
        {
            string url = "https://shop.taobao.com/getShopInfo.htm?shopId="+userid+"&_ksTS=1584693969776_37&callback=jsonp38";
            string html = GetUrl(url, "utf-8");
            Match time = Regex.Match(html, @"""starts"":""([\s\S]*?)""");
            return time.Groups[1].Value;
        }


        public void run()
        {
            try
            {
                string[] array = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] != "")
                    {
                        string url = "http://shop.m.taobao.com/shop/shop_search.htm?q="+array[i]+"&_input_charset=utf-8&topSearch=1&atype=b&sid=&searchfrom=&action=RedirectAppAction&event_submit_do_search_shop=%E6%90%9C+%E7%B4%A2";
                        string html = GetUrl(url,"utf-8");

                        Match userid= Regex.Match(html, @"do\?userid=([\s\S]*?)""");
                        Match uid = Regex.Match(html, @"shop_id=([\s\S]*?)""");

                        Match sold = Regex.Match(html, @"<p class=""d-main"">已售([\s\S]*?)笔</p>");
                        //Match procnt = Regex.Match(html, @"""procnt"":([\s\S]*?),");
                      
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString()); //使用Listview展示数据  
                        lv1.SubItems.Add(array[i]);
                        lv1.SubItems.Add(sold.Groups[1].Value);
                        lv1.SubItems.Add("无");
                        lv1.SubItems.Add(gettime(userid.Groups[1].Value));
                        lv1.SubItems.Add("无");
                        lv1.SubItems.Add(getcreattime(uid.Groups[1].Value));

                        // caijiweb();
                       
                        lv1.SubItems.Add("无");

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }




        public string caijiweb()
        {
            webBrowser1.Navigate("https://taodaxiang.com/credit2");

            //加载完毕后触发事件webBrowser1_DocumentCompleted
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);

          

            return "";

        }

        string time1 = "";
        string time2 = "";

        private void webBrowser1_DocumentCompleted(object sender, EventArgs e)//这个就是当网页载入完毕后要进行的操作
        {


           string html = webBrowser1.DocumentText;

            Match time1 = Regex.Match(html, @"注册时间：([\s\S]*?)</b>");
            Match time2= Regex.Match(html, @"注册时间：([\s\S]*?)</b>");


          this.time1 = Regex.Replace(time1.Groups[1].Value, "<[^>]+>", "");
            this.time2 = Regex.Replace(time2.Groups[1].Value, "<[^>]+>", "");
        }



        private void 淘宝信息_Load(object sender, EventArgs e)
        {

        }

        private void Button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
         
            button1.Enabled = false;
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
           
        }
    }
}
