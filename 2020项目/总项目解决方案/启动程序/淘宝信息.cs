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

        public static string COOKIE = "ali_ab=121.234.247.249.1523710505053.9; thw=cn; _uab_collina=156125129669419378524705; hng=CN%7Czh-CN%7CCNY%7C156; x=e%3D1%26p%3D*%26s%3D0%26c%3D0%26f%3D0%26g%3D0%26t%3D0; UM_distinctid=16efe96bcb264c-0e65d62e4b9f92-2393f61-1fa400-16efe96bcb33c3; enc=6ks8%2Bqev38j3rA5BNpeT61pbwmoAEpq2EAYG58PCTOS7tMoVRi%2FuNynnrjVs8wlILE6DOpKmYZWcLk02LsMBDQ%3D%3D; t=df077a6350db87ab2fb3bbec05c2dbca; cna=/h4DE4EwWXQCATFGW6Fbj0Jl; lgc=zkg852266010; tracknick=zkg852266010; tg=0; mt=ci=62_1; _m_h5_tk=ba5a3671be4ec97581dc52385034b97a_1585383880017; _m_h5_tk_enc=c0eab904d50993a4946df5b6f902299f; cookie2=1dafedcb6746a5ad428ae93068edb2d8; v=0; _tb_token_=e3e661e1ed68d; _samesite_flag_=true; sgcookie=Ejfw7j08aMEPDGXUlZW3s; unb=1052347548; uc3=lg2=VT5L2FSpMGV7TQ%3D%3D&nk2=GcOvCmiKUSBXqZNU&vt3=F8dBxd9vfOJNqqbqj7c%3D&id2=UoH62EAv27BqSg%3D%3D; csg=307e1208; cookie17=UoH62EAv27BqSg%3D%3D; dnk=zkg852266010; skt=1e956b9a4f0d4d1d; existShop=MTU4NTM4MzM1OA%3D%3D; uc4=nk4=0%40GwrkntVPltPB9cR46GnfGp2gYdO1GcU%3D&id4=0%40UOnlZ%2FcoxCrIUsehKGOnxAqcxw7k; _cc_=WqG3DMC9EA%3D%3D; _l_g_=Ug%3D%3D; sg=080; _nk_=zkg852266010; cookie1=Vvj8uMJubtxirKFtxaDmWPxYCP5sb7EKtrFe1w68JDk%3D; tfstk=csPRBP0TXijoTXTB7NXmOr3TT3SGZCsKcUiH9wZxg84Fg0RdiJFg6RVOF2t-MZC..; uc1=pas=0&lng=zh_CN&tag=8&cookie16=U%2BGCWk%2F74Mx5tgzv3dWpnhjPaQ%3D%3D&cookie21=VFC%2FuZ9ajCbF8%2BYBpbBdiw%3D%3D&cookie15=VFC%2FuZ9ayeYq2g%3D%3D&cookie14=UoTUP2Hg3EULLA%3D%3D&existShop=true; x5sec=7b2274616f62616f2d73686f707365617263683b32223a226235333930383931653162363136323238373230326436303737336335326365434d43612f504d46454d7a626f2b506f3250724f77414561444445774e54497a4e4463314e4467374d513d3d227d; linezing_session=t1NFUIUVWiGtfxON7dzL9JBF_1585385033774kiNy_19; JSESSIONID=1790CE6377635B5CC490EC0ABA8AC563; l=dBIVfrI7QUqtYvtABOfZZFB0HU_9nIOb8sPyBvv3FICPt_fp5LzGWZ43lPL9CnGVn6uMR3Jt3efYBlLlmyCq3Tt-CeE8GgXSFdTh.; isg=BP7-BCoWLNaKhnjKrdjJi1NjTxRAP8K58cmq8qgGaME8S58lEMoFyWFpwxeH87rR";
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

        /// <summary>
        /// 最后评价时间
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string gettime(string userid)
        {
            string url = "https://rate.taobao.com/member_rate.htm?_ksTS=1585384658406_502&callback=shop_rate_list&content=1&result=&from=rate&user_id="+userid+"&identity=2&rater=0&direction=0";
            string html = GetUrl(url, "utf-8");
            Match time = Regex.Match(html, @"""date"":""([\s\S]*?)""");
            return time.Groups[1].Value;
        }
        /// <summary>
        /// 创店时间
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string getcreattime(string userid)
        {
            string url = "https://shop.taobao.com/getShopInfo.htm?shopId="+userid+"&_ksTS=1584693969776_37&callback=jsonp38";
            string html = GetUrl(url, "utf-8");
            Match time = Regex.Match(html, @"""starts"":""([\s\S]*?)""");
            return time.Groups[1].Value;
        }




 

        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            try
            {
                string[] array = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] != "")
                    {
                        
                       
                        string url = "https://shopsearch.taobao.com/search?q="+array[i]+"&js=1&initiative_id=staobaoz_20200328&ie=utf8";
                        string html = GetUrl(url, "utf-8");
                        Match Userid = Regex.Match(html, @"encryptedUserId\\"":\\""([\s\S]*?)\\");
                        Match goods = Regex.Match(html, @"""procnt"":([\s\S]*?),");

      
                        Match userid= Regex.Match(html, @"do\?userid=([\s\S]*?)""");
                        Match uid = Regex.Match(html, @"shop_id=([\s\S]*?)""");
                       
                        Match sold = Regex.Match(html, @"""totalsold"":([\s\S]*?),");
                        
                      
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString()); //使用Listview展示数据  
                        lv1.SubItems.Add(array[i]);
                        lv1.SubItems.Add(sold.Groups[1].Value);
                        lv1.SubItems.Add(goods.Groups[1].Value);
                        lv1.SubItems.Add(gettime(Userid.Groups[1].Value));
                        lv1.SubItems.Add("无");
                        lv1.SubItems.Add(getcreattime(uid.Groups[1].Value));

                    
                      

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        Thread.Sleep(2000);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }







        private void 淘宝信息_Load(object sender, EventArgs e)
        {
            method.SetWebBrowserFeatures(method.IeVersion.IE10);
            webBrowser1.ScriptErrorsSuppressed = true;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            COOKIE = method.GetCookies("https://shopsearch.taobao.com/search?q=alphastyle%E6%97%97%E8%88%B0%E5%BA%97&js=1&initiative_id=staobaoz_20200328&ie=utf8");
            button1.Enabled = false;
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://login.taobao.com/member/login.jhtml");
        }
    }
}
