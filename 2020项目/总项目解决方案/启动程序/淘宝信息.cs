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
                string COOKIE = "thw=cn; ali_ab=49.94.92.171.1563332665663.4; _uab_collina=156618524287232013003586; x=e%3D1%26p%3D*%26s%3D0%26c%3D0%26f%3D0%26g%3D0%26t%3D0%26__ll%3D-1%26_ato%3D0; hng=CN%7Czh-CN%7CCNY%7C156; enc=rY0GpAFgrh5bXXfBXutSHaQSm6aOCly2Ov5qI2xvmmRLzA74CWx0R1R%2FH4RXdUTECCRII572ywPqHDXt8ypRKg%3D%3D; cna=8QJMFUu4DhACATFZv2JYDtwd; t=027e7e2bc53b51842bd6d63b5b90ab8a; lgc=zkg852266010; tracknick=zkg852266010; tg=0; mt=ci=63_1; _m_h5_tk=7bc702bcca4431eefd560ecf8a0938ad_1584681947986; _m_h5_tk_enc=a024b195c52b59158251c10a935b6626; cookie2=1644edd26db6fe8f4dbc3e268dff59f3; _tb_token_=e3383353e8033; _samesite_flag_=true; sgcookie=EDkMtym1wc4VCVmSBf9%2FO; unb=1052347548; uc3=lg2=U%2BGCWk%2F75gdr5Q%3D%3D&id2=UoH62EAv27BqSg%3D%3D&nk2=GcOvCmiKUSBXqZNU&vt3=F8dBxd9kpKPnTfpzi80%3D; csg=7f388fbf; cookie17=UoH62EAv27BqSg%3D%3D; dnk=zkg852266010; skt=cda756620c075f46; existShop=MTU4NDY4MTMxNA%3D%3D; uc4=nk4=0%40GwrkntVPltPB9cR46GnfGpyYaNHCZKE%3D&id4=0%40UOnlZ%2FcoxCrIUsehKGOmUFp6RdwI; _cc_=VFC%2FuZ9ajQ%3D%3D; _l_g_=Ug%3D%3D; sg=080; _nk_=zkg852266010; cookie1=Vvj8uMJubtxirKFtxaDmWPxYCP5sb7EKtrFe1w68JDk%3D; tfstk=cDHCBNqKvwbIxDqCTbtaUokNRuwPZcjbVBanAfZq-e2hxonCiEW4oOesZaBTMl1..; v=0; alitrackid=www.taobao.com; lastalitrackid=www.taobao.com; uc1=cookie16=Vq8l%2BKCLySLZMFWHxqs8fwqnEw%3D%3D&pas=0&cookie21=UIHiLt3xSixwH1aenGUFEQ%3D%3D&cookie15=U%2BGCWk%2F75gdr5Q%3D%3D&cookie14=UoTUPvbF7xQkVA%3D%3D&existShop=true; JSESSIONID=52C82B5A65966CCF54F1B40D929BD28A; isg=BMPDN2h5mV7Y7FVQJ2ht_VJQUodtOFd6yQeh2vWhziKZtOHWfQ3EyuliLkT6FK9y; l=dBTc_4AlQLoCc3G3BOfiR42gwx79aIdbzsPrE67l5ICP9pCp5-V5WZ4-A9T9CnGVnsO6R3oWYJ1uBJLlYyCq3Tt-CeE8Ggxqed8h.";
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
                        string url = "https://shopsearch.taobao.com/search?q="+ System.Web.HttpUtility.UrlEncode(array[i])+"&js=1&initiative_id=staobaoz_20200320&ie=utf8";
                        string html = GetUrl(url,"utf-8");

                        Match userid= Regex.Match(html, @"encryptedUserId\\"":\\""([\s\S]*?)\\""");
                        Match uid = Regex.Match(html, @"shopUrl"":""\/\/shop([\s\S]*?)\.");

                        Match sold = Regex.Match(html, @"""totalsold"":([\s\S]*?),");
                        Match procnt = Regex.Match(html, @"""procnt"":([\s\S]*?),");
                      
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString()); //使用Listview展示数据  
                        lv1.SubItems.Add(array[i]);
                        lv1.SubItems.Add(sold.Groups[1].Value);
                        lv1.SubItems.Add(procnt.Groups[1].Value);
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
