using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 天猫店铺采集
{
    public partial class 天猫店铺采集 : Form
    {
        public 天猫店铺采集()
        {
            InitializeComponent();
        }
        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }
        #endregion


      public static List<string> cookielist = new List<string>();


        public string filename = "";
        private void 天猫店铺采集_Load(object sender, EventArgs e)
        {
            
            #region 通用检测



            if (Convert.ToDateTime("2023-05-20")<DateTime.Now)
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }


            #endregion

            status = true;

            //if (thread == null || !thread.IsAlive)
            //{
            //    thread = new Thread(run);
            //    thread.Start();
            //    Control.CheckForIllegalCrossThreadCalls = false;
            //}


        }

        /// <summary>
        /// 在宝贝栏搜 店铺名称
        /// </summary>
        public void run_shop()
        {

            List<string> list = new List<string>();
            dics.Clear();

          

            try
            {
                DataTable dt = method.ExcelToDataTable(textBox5.Text, true);
                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    string keyword = dt.Rows[a][1].ToString().Trim();
                    if (keyword.Trim() == "")
                        continue;
                  
                        Thread.Sleep(1000);

                    string token = Regex.Match(reviewcookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
                    int i = 1;

                    string time = function.GetTimeStamp();
                 
                    //搜索宝贝
                    string str = token + "&" + time + "&12574478&{\"appId\":\"29859\",\"params\":\"{\\\"isBeta\\\":\\\"false\\\",\\\"grayHair\\\":\\\"false\\\",\\\"appId\\\":\\\"29859\\\",\\\"from\\\":\\\"nt_history\\\",\\\"brand\\\":\\\"HUAWEI\\\",\\\"info\\\":\\\"wifi\\\",\\\"index\\\":\\\"4\\\",\\\"ttid\\\":\\\"600000@taobao_android_10.7.0\\\",\\\"needTabs\\\":\\\"true\\\",\\\"rainbow\\\":\\\"\\\",\\\"areaCode\\\":\\\"CN\\\",\\\"vm\\\":\\\"nw\\\",\\\"schemaType\\\":\\\"auction\\\",\\\"elderHome\\\":\\\"false\\\",\\\"device\\\":\\\"HMA-AL00\\\",\\\"isEnterSrpSearch\\\":\\\"true\\\",\\\"newSearch\\\":\\\"false\\\",\\\"network\\\":\\\"wifi\\\",\\\"subtype\\\":\\\"\\\",\\\"hasPreposeFilter\\\":\\\"false\\\",\\\"client_os\\\":\\\"Android\\\",\\\"gpsEnabled\\\":\\\"false\\\",\\\"searchDoorFrom\\\":\\\"srp\\\",\\\"debug_rerankNewOpenCard\\\":\\\"false\\\",\\\"homePageVersion\\\":\\\"v7\\\",\\\"searchElderHomeOpen\\\":\\\"false\\\",\\\"style\\\":\\\"wf\\\",\\\"page\\\":" + i + ",\\\"n\\\":\\\"10\\\",\\\"q\\\":\\\"" + keyword + "\\\",\\\"search_action\\\":\\\"initiative\\\",\\\"sugg\\\":\\\"_4_1\\\",\\\"m\\\":\\\"h5\\\",\\\"sversion\\\":\\\"13.6\\\",\\\"prepositionVersion\\\":\\\"v2\\\",\\\"tab\\\":\\\"all\\\",\\\"channelSrp\\\":\\\"newh5\\\",\\\"loc\\\":\\\"\\\",\\\"service\\\":\\\"mall\\\",\\\"prop\\\":\\\"\\\",\\\"end_price\\\":\\\"\\\",\\\"start_price\\\":\\\"\\\",\\\"catmap\\\":\\\"\\\",\\\"tagSearchKeyword\\\":null,\\\"sort\\\":\\\"_sale\\\",\\\"filterTag\\\":\\\"mall\\\",\\\"itemIds\\\":\\\"648262858563,661494105219,664878907879,665062689992,662935881512,662255029960,646495195222,660860368122,670443949890,660007612626\\\",\\\"itemS\\\":70}\"}";
                    string sign = function.Md5_utf8(str);

                    string url = "https://h5api.m.taobao.com/h5/mtop.relationrecommend.wirelessrecommend.recommend/2.0/?jsv=2.6.2&appKey=12574478&t=" + time + "&sign=" + sign + "&api=mtop.relationrecommend.WirelessRecommend.recommend&v=2.0&type=jsonp&dataType=jsonp&callback=mtopjsonp18&data=%7B%22appId%22%3A%2229859%22%2C%22params%22%3A%22%7B%5C%22isBeta%5C%22%3A%5C%22false%5C%22%2C%5C%22grayHair%5C%22%3A%5C%22false%5C%22%2C%5C%22appId%5C%22%3A%5C%2229859%5C%22%2C%5C%22from%5C%22%3A%5C%22nt_history%5C%22%2C%5C%22brand%5C%22%3A%5C%22HUAWEI%5C%22%2C%5C%22info%5C%22%3A%5C%22wifi%5C%22%2C%5C%22index%5C%22%3A%5C%224%5C%22%2C%5C%22ttid%5C%22%3A%5C%22600000%40taobao_android_10.7.0%5C%22%2C%5C%22needTabs%5C%22%3A%5C%22true%5C%22%2C%5C%22rainbow%5C%22%3A%5C%22%5C%22%2C%5C%22areaCode%5C%22%3A%5C%22CN%5C%22%2C%5C%22vm%5C%22%3A%5C%22nw%5C%22%2C%5C%22schemaType%5C%22%3A%5C%22auction%5C%22%2C%5C%22elderHome%5C%22%3A%5C%22false%5C%22%2C%5C%22device%5C%22%3A%5C%22HMA-AL00%5C%22%2C%5C%22isEnterSrpSearch%5C%22%3A%5C%22true%5C%22%2C%5C%22newSearch%5C%22%3A%5C%22false%5C%22%2C%5C%22network%5C%22%3A%5C%22wifi%5C%22%2C%5C%22subtype%5C%22%3A%5C%22%5C%22%2C%5C%22hasPreposeFilter%5C%22%3A%5C%22false%5C%22%2C%5C%22client_os%5C%22%3A%5C%22Android%5C%22%2C%5C%22gpsEnabled%5C%22%3A%5C%22false%5C%22%2C%5C%22searchDoorFrom%5C%22%3A%5C%22srp%5C%22%2C%5C%22debug_rerankNewOpenCard%5C%22%3A%5C%22false%5C%22%2C%5C%22homePageVersion%5C%22%3A%5C%22v7%5C%22%2C%5C%22searchElderHomeOpen%5C%22%3A%5C%22false%5C%22%2C%5C%22style%5C%22%3A%5C%22wf%5C%22%2C%5C%22page%5C%22%3A" + i + "%2C%5C%22n%5C%22%3A%5C%2210%5C%22%2C%5C%22q%5C%22%3A%5C%22" + System.Web.HttpUtility.UrlEncode(keyword) + "%5C%22%2C%5C%22search_action%5C%22%3A%5C%22initiative%5C%22%2C%5C%22sugg%5C%22%3A%5C%22_4_1%5C%22%2C%5C%22m%5C%22%3A%5C%22h5%5C%22%2C%5C%22sversion%5C%22%3A%5C%2213.6%5C%22%2C%5C%22prepositionVersion%5C%22%3A%5C%22v2%5C%22%2C%5C%22tab%5C%22%3A%5C%22all%5C%22%2C%5C%22channelSrp%5C%22%3A%5C%22newh5%5C%22%2C%5C%22loc%5C%22%3A%5C%22%5C%22%2C%5C%22service%5C%22%3A%5C%22mall%5C%22%2C%5C%22prop%5C%22%3A%5C%22%5C%22%2C%5C%22end_price%5C%22%3A%5C%22%5C%22%2C%5C%22start_price%5C%22%3A%5C%22%5C%22%2C%5C%22catmap%5C%22%3A%5C%22%5C%22%2C%5C%22tagSearchKeyword%5C%22%3Anull%2C%5C%22sort%5C%22%3A%5C%22_sale%5C%22%2C%5C%22filterTag%5C%22%3A%5C%22mall%5C%22%2C%5C%22itemIds%5C%22%3A%5C%22648262858563%2C661494105219%2C664878907879%2C665062689992%2C662935881512%2C662255029960%2C646495195222%2C660860368122%2C670443949890%2C660007612626%5C%22%2C%5C%22itemS%5C%22%3A70%7D%22%7D";

                    string html = function.GetUrlWithCookie(url, reviewcookie, "utf-8");

                    if (html.Contains("令牌过期"))
                    {
                        string cookiestr = function.getSetCookie(url);
                        string _m_h5_tk = "_m_h5_tk=" + Regex.Match(cookiestr, @"_m_h5_tk=([\s\S]*?);").Groups[1].Value;
                        string _m_h5_tk_enc = "_m_h5_tk_enc=" + Regex.Match(cookiestr, @"_m_h5_tk_enc=([\s\S]*?);").Groups[1].Value;
                        reviewcookie = _m_h5_tk + ";" + _m_h5_tk_enc + ";";
                        token = Regex.Match(reviewcookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
                        html = function.GetUrlWithCookie(url, reviewcookie, "utf-8");
                    }
                    html = Regex.Match(html, @"itemsArray"":\[([\s\S]*?)调用成功").Groups[1].Value;
                    string sold = Regex.Match(html, @"""sold"":""([\s\S]*?)""").Groups[1].Value;
                    string xinpin = "";
                    if (html.Contains("新品"))
                    {
                        xinpin = "新品";
                    }
                   string userId = Regex.Match(html, @"""userId"":""([\s\S]*?)""").Groups[1].Value;
                   string x_object_id = Regex.Match(html, @"""x_object_id"":""([\s\S]*?)""").Groups[1].Value;

                    string commenttime = getcommenttime(userId,true);

                    string[] commenttime2 = commenttime.Split(new string[] { "#" }, StringSplitOptions.None);

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                    lv1.SubItems.Add(keyword);

                    lv1.SubItems.Add("https://detail.tmall.com/item.htm?id=" + x_object_id);
                    lv1.SubItems.Add(dt.Rows[a][3].ToString());
                    lv1.SubItems.Add(dt.Rows[a][4].ToString());
                    lv1.SubItems.Add(sold);
                    lv1.SubItems.Add(commenttime2[0]);
                    lv1.SubItems.Add(xinpin);
                    lv1.SubItems.Add(commenttime2[1]);
                }

                MessageBox.Show("完成");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }



        /// <summary>
        /// 在宝贝栏搜 店铺名称
        /// </summary>
        public void runshop2()
        {

            List<string> list = new List<string>();
            dics.Clear();
            int i = 1;



            try
            {
                DataTable dt = method.ExcelToDataTable(textBox5.Text, true);
                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    string keyword = dt.Rows[a][1].ToString().Trim();
                    if (keyword.Trim() == "")
                        continue;

                    Thread.Sleep(1000);

                    string token = Regex.Match(reviewcookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
                   
                    string time = function.GetTimeStamp();

                    //搜索店铺
                    string str = token+"&"+time+"&12574478&{\"appId\":\"30486\",\"params\":\"{\\\"chituGroupAlias\\\":\\\"zhouzhou_liantiao_final\\\",\\\"_viewlogs\\\":\\\"true\\\",\\\"viewlogs\\\":\\\"true\\\",\\\"debug_\\\":\\\"true\\\",\\\"solutionDebug\\\":\\\"true\\\",\\\"_debug\\\":\\\"true\\\",\\\"dcEnable\\\":\\\"true\\\",\\\"_switchers\\\":\\\"true\\\",\\\"_blendInfos\\\":\\\"true\\\",\\\"routerDebug\\\":\\\"true\\\",\\\"_DEBUG\\\":\\\"true\\\",\\\"debug\\\":\\\"true\\\",\\\"debug_rerankNewOpenCard\\\":\\\"false\\\",\\\"DEBUG\\\":\\\"true\\\",\\\"DEBUG_\\\":\\\"true\\\",\\\"viewlogs_\\\":\\\"true\\\",\\\"pvFeature\\\":\\\"654083998634;644832834668;668084343069;662334090942;665339768743;664390297378;664047381602\\\",\\\"tab\\\":\\\"shop\\\",\\\"grayHair\\\":\\\"false\\\",\\\"sversion\\\":\\\"13.7\\\",\\\"from\\\":\\\"input\\\",\\\"isBeta\\\":\\\"false\\\",\\\"brand\\\":\\\"HUAWEI\\\",\\\"info\\\":\\\"wifi\\\",\\\"client_for_bts\\\":\\\"client_android_view_preload:1000001\\\",\\\"ttid\\\":\\\"600000@taobao_android_10.8.0\\\",\\\"rainbow\\\":\\\"\\\",\\\"areaCode\\\":\\\"CN\\\",\\\"vm\\\":\\\"nw\\\",\\\"elderHome\\\":\\\"false\\\",\\\"style\\\":\\\"list\\\",\\\"page\\\":"+i+",\\\"device\\\":\\\"HMA-AL00\\\",\\\"editionCode\\\":\\\"CN\\\",\\\"cityCode\\\":\\\"110100\\\",\\\"countryNum\\\":\\\"156\\\",\\\"newSearch\\\":\\\"false\\\",\\\"chituBiz\\\":\\\"TaobaoPhoneSearch\\\",\\\"utd_id\\\":\\\"XYDZLfLy3ZQDAKmnYOhvIwW4\\\",\\\"network\\\":\\\"wifi\\\",\\\"hasPreposeFilter\\\":\\\"false\\\",\\\"client_os\\\":\\\"Android\\\",\\\"gpsEnabled\\\":\\\"true\\\",\\\"apptimestamp\\\":\\\"1655714609\\\",\\\"canP4pVideoPlay\\\":\\\"true\\\",\\\"homePageVersion\\\":\\\"v7\\\",\\\"searchElderHomeOpen\\\":\\\"false\\\",\\\"n\\\":\\\"10\\\",\\\"search_action\\\":\\\"initiative\\\",\\\"q\\\":\\\""+keyword+"\\\",\\\"tagSearchKeyword\\\":null,\\\"sort\\\":\\\"sale-asc\\\",\\\"filterTag\\\":\\\"mall\\\",\\\"prop\\\":\\\"\\\"}\"}"; ;

                    string sign = function.Md5_utf8(str);


                    //搜索店铺
                    string url = "https://h5api.m.taobao.com/h5/mtop.relationrecommend.wirelessrecommend.recommend/2.0/?jsv=2.6.2&appKey=12574478&t="+time+"&sign="+sign+"&api=mtop.relationrecommend.WirelessRecommend.recommend&v=2.0&type=jsonp&dataType=jsonp&callback=mtopjsonp4&data=%7B%22appId%22%3A%2230486%22%2C%22params%22%3A%22%7B%5C%22chituGroupAlias%5C%22%3A%5C%22zhouzhou_liantiao_final%5C%22%2C%5C%22_viewlogs%5C%22%3A%5C%22true%5C%22%2C%5C%22viewlogs%5C%22%3A%5C%22true%5C%22%2C%5C%22debug_%5C%22%3A%5C%22true%5C%22%2C%5C%22solutionDebug%5C%22%3A%5C%22true%5C%22%2C%5C%22_debug%5C%22%3A%5C%22true%5C%22%2C%5C%22dcEnable%5C%22%3A%5C%22true%5C%22%2C%5C%22_switchers%5C%22%3A%5C%22true%5C%22%2C%5C%22_blendInfos%5C%22%3A%5C%22true%5C%22%2C%5C%22routerDebug%5C%22%3A%5C%22true%5C%22%2C%5C%22_DEBUG%5C%22%3A%5C%22true%5C%22%2C%5C%22debug%5C%22%3A%5C%22true%5C%22%2C%5C%22debug_rerankNewOpenCard%5C%22%3A%5C%22false%5C%22%2C%5C%22DEBUG%5C%22%3A%5C%22true%5C%22%2C%5C%22DEBUG_%5C%22%3A%5C%22true%5C%22%2C%5C%22viewlogs_%5C%22%3A%5C%22true%5C%22%2C%5C%22pvFeature%5C%22%3A%5C%22654083998634%3B644832834668%3B668084343069%3B662334090942%3B665339768743%3B664390297378%3B664047381602%5C%22%2C%5C%22tab%5C%22%3A%5C%22shop%5C%22%2C%5C%22grayHair%5C%22%3A%5C%22false%5C%22%2C%5C%22sversion%5C%22%3A%5C%2213.7%5C%22%2C%5C%22from%5C%22%3A%5C%22input%5C%22%2C%5C%22isBeta%5C%22%3A%5C%22false%5C%22%2C%5C%22brand%5C%22%3A%5C%22HUAWEI%5C%22%2C%5C%22info%5C%22%3A%5C%22wifi%5C%22%2C%5C%22client_for_bts%5C%22%3A%5C%22client_android_view_preload%3A1000001%5C%22%2C%5C%22ttid%5C%22%3A%5C%22600000%40taobao_android_10.8.0%5C%22%2C%5C%22rainbow%5C%22%3A%5C%22%5C%22%2C%5C%22areaCode%5C%22%3A%5C%22CN%5C%22%2C%5C%22vm%5C%22%3A%5C%22nw%5C%22%2C%5C%22elderHome%5C%22%3A%5C%22false%5C%22%2C%5C%22style%5C%22%3A%5C%22list%5C%22%2C%5C%22page%5C%22%3A"+i+"%2C%5C%22device%5C%22%3A%5C%22HMA-AL00%5C%22%2C%5C%22editionCode%5C%22%3A%5C%22CN%5C%22%2C%5C%22cityCode%5C%22%3A%5C%22110100%5C%22%2C%5C%22countryNum%5C%22%3A%5C%22156%5C%22%2C%5C%22newSearch%5C%22%3A%5C%22false%5C%22%2C%5C%22chituBiz%5C%22%3A%5C%22TaobaoPhoneSearch%5C%22%2C%5C%22utd_id%5C%22%3A%5C%22XYDZLfLy3ZQDAKmnYOhvIwW4%5C%22%2C%5C%22network%5C%22%3A%5C%22wifi%5C%22%2C%5C%22hasPreposeFilter%5C%22%3A%5C%22false%5C%22%2C%5C%22client_os%5C%22%3A%5C%22Android%5C%22%2C%5C%22gpsEnabled%5C%22%3A%5C%22true%5C%22%2C%5C%22apptimestamp%5C%22%3A%5C%221655714609%5C%22%2C%5C%22canP4pVideoPlay%5C%22%3A%5C%22true%5C%22%2C%5C%22homePageVersion%5C%22%3A%5C%22v7%5C%22%2C%5C%22searchElderHomeOpen%5C%22%3A%5C%22false%5C%22%2C%5C%22n%5C%22%3A%5C%2210%5C%22%2C%5C%22search_action%5C%22%3A%5C%22initiative%5C%22%2C%5C%22q%5C%22%3A%5C%22"+ System.Web.HttpUtility.UrlEncode(keyword) + "%5C%22%2C%5C%22tagSearchKeyword%5C%22%3Anull%2C%5C%22sort%5C%22%3A%5C%22sale-asc%5C%22%2C%5C%22filterTag%5C%22%3A%5C%22mall%5C%22%2C%5C%22prop%5C%22%3A%5C%22%5C%22%7D%22%7D";

                  
                    
                    
                    string html = function.GetUrlWithCookie(url, reviewcookie, "utf-8");

                  
                    if (html.Contains("令牌过期"))
                    {
                       
                        string cookiestr = function.getSetCookie(url);
                        string _m_h5_tk = "_m_h5_tk=" + Regex.Match(cookiestr, @"_m_h5_tk=([\s\S]*?);").Groups[1].Value;
                        string _m_h5_tk_enc = "_m_h5_tk_enc=" + Regex.Match(cookiestr, @"_m_h5_tk_enc=([\s\S]*?);").Groups[1].Value;
                        reviewcookie = _m_h5_tk + ";" + _m_h5_tk_enc + ";";
                        token = Regex.Match(reviewcookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
                        a = a - 1;
                        continue;
                    }
                    //textBox4.Text =  html;
                 
                    string sold = Regex.Match(html, @"""sold"":""([\s\S]*?)""").Groups[1].Value;
                  
                    string userId = Regex.Match(html, @"""sellerId"":""([\s\S]*?)""").Groups[1].Value;
                    string shopId = Regex.Match(html, @"""shopId"":""([\s\S]*?)""").Groups[1].Value;


                    string newurl = "https://hdc1new.tmall.com/asyn.htm?pageId=&userId="+userId+"&shopId="+shopId;

                    string newhtml = function.GetUrlWithCookiePC(newurl, "", "utf-8");
                    string userrate = Regex.Match(newhtml, @"user-rate-([\s\S]*?)\.").Groups[1].Value;

                    string aurl = "https://rate.taobao.com/member_rate.htm?_ksTS=" + function.GetTimeStamp() + "_166&callback=shop_rate_list&content=1&result=&from=rate&user_id=" + userrate + "&identity=2&rater=0&direction=0";

                    string ahtml = function.GetUrlWithCookiePC(aurl, 登录.cookie, "utf-8");



                    string date = Regex.Match(ahtml, @"""date"":""([\s\S]*?)""").Groups[1].Value;

                    string burl = "https://rate.taobao.com/user-rate-" + userrate + ".htm";
                    string bhtml = function.GetUrlWithCookiePC(burl, 登录.cookie, "utf-8");
                    
                    string baozhengjin = Regex.Match(bhtml, @"<span>￥([\s\S]*?)</span>").Groups[1].Value;

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                    lv1.SubItems.Add(keyword);

                    lv1.SubItems.Add("https://shop"+shopId+".taobao.com/shop/view_shop.htm");
                    lv1.SubItems.Add(dt.Rows[a][3].ToString());
                    lv1.SubItems.Add(dt.Rows[a][4].ToString());
                    lv1.SubItems.Add(sold);
                    lv1.SubItems.Add(date);
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add(baozhengjin);

                    while (zanting == false)
                    {
                        Application.DoEvents();//等待本次加载完毕才执行下次循环.
                    }
                    if (status == false)
                        return;
                }

                MessageBox.Show("完成");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }



        Dictionary<string, string> dics = new Dictionary<string, string>();

        //string reviewcookie = "_m_h5_tk=8670f2963029d7a3428dcd2088460b4c_1655798664120; _m_h5_tk_enc=9d48ffd7ff59d279bcec5ff84cb85e95;";
        string reviewcookie = "_m_h5_tk=fdadbea0f9f7d75212b16535ae78b84b_1677409105656; _m_h5_tk_enc=917bdf7966753906e7a4a33cd551b025;";

        List<string> list = new List<string>();







        /// <summary>
        /// 搜索宝贝
        /// </summary>
        public void run()
        {
            //string value=function.chulitxt();
            List<string> list = new List<string>();
            dics.Clear();

            //filename= AppDomain.CurrentDomain.BaseDirectory+value + ".txt";
            //textBox1.Text = filename;
            filename = textBox1.Text;
            string token = Regex.Match(reviewcookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;


            try
            {
                StreamReader sr = new StreamReader(filename, method.EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
                for (int a = 0; a < text.Length; a++)
                {


                    if (text[a].Trim() == "")
                        continue;


                  

                        for (int i = 1; i < 201; i++)
                    {


                      
                        Thread.Sleep(2000);
                       
                        string time = function.GetTimeStamp();
                        string keyword = text[a].Trim();

                        

                        //搜索店铺
                        //string str = token+"&"+time+"&12574478&{\"appId\":\"30486\",\"params\":\"{\\\"chituGroupAlias\\\":\\\"zhouzhou_liantiao_final\\\",\\\"_viewlogs\\\":\\\"true\\\",\\\"viewlogs\\\":\\\"true\\\",\\\"debug_\\\":\\\"true\\\",\\\"solutionDebug\\\":\\\"true\\\",\\\"_debug\\\":\\\"true\\\",\\\"dcEnable\\\":\\\"true\\\",\\\"_switchers\\\":\\\"true\\\",\\\"_blendInfos\\\":\\\"true\\\",\\\"routerDebug\\\":\\\"true\\\",\\\"_DEBUG\\\":\\\"true\\\",\\\"debug\\\":\\\"true\\\",\\\"debug_rerankNewOpenCard\\\":\\\"false\\\",\\\"DEBUG\\\":\\\"true\\\",\\\"DEBUG_\\\":\\\"true\\\",\\\"viewlogs_\\\":\\\"true\\\",\\\"pvFeature\\\":\\\"654083998634;644832834668;668084343069;662334090942;665339768743;664390297378;664047381602\\\",\\\"tab\\\":\\\"shop\\\",\\\"grayHair\\\":\\\"false\\\",\\\"sversion\\\":\\\"13.7\\\",\\\"from\\\":\\\"input\\\",\\\"isBeta\\\":\\\"false\\\",\\\"brand\\\":\\\"HUAWEI\\\",\\\"info\\\":\\\"wifi\\\",\\\"client_for_bts\\\":\\\"client_android_view_preload:1000001\\\",\\\"ttid\\\":\\\"600000@taobao_android_10.8.0\\\",\\\"rainbow\\\":\\\"\\\",\\\"areaCode\\\":\\\"CN\\\",\\\"vm\\\":\\\"nw\\\",\\\"elderHome\\\":\\\"false\\\",\\\"style\\\":\\\"list\\\",\\\"page\\\":"+i+",\\\"device\\\":\\\"HMA-AL00\\\",\\\"editionCode\\\":\\\"CN\\\",\\\"cityCode\\\":\\\"110100\\\",\\\"countryNum\\\":\\\"156\\\",\\\"newSearch\\\":\\\"false\\\",\\\"chituBiz\\\":\\\"TaobaoPhoneSearch\\\",\\\"utd_id\\\":\\\"XYDZLfLy3ZQDAKmnYOhvIwW4\\\",\\\"network\\\":\\\"wifi\\\",\\\"hasPreposeFilter\\\":\\\"false\\\",\\\"client_os\\\":\\\"Android\\\",\\\"gpsEnabled\\\":\\\"true\\\",\\\"apptimestamp\\\":\\\"1655714609\\\",\\\"canP4pVideoPlay\\\":\\\"true\\\",\\\"homePageVersion\\\":\\\"v7\\\",\\\"searchElderHomeOpen\\\":\\\"false\\\",\\\"n\\\":\\\"10\\\",\\\"search_action\\\":\\\"initiative\\\",\\\"q\\\":\\\""+keyword+"\\\",\\\"tagSearchKeyword\\\":null,\\\"sort\\\":\\\"sale-asc\\\",\\\"filterTag\\\":\\\"mall\\\",\\\"prop\\\":\\\"\\\"}\"}"; ;

                        //搜索宝贝
                        string str = token + "&" + time + "&12574478&{\"appId\":\"29859\",\"params\":\"{\\\"isBeta\\\":\\\"false\\\",\\\"grayHair\\\":\\\"false\\\",\\\"appId\\\":\\\"29859\\\",\\\"from\\\":\\\"nt_history\\\",\\\"brand\\\":\\\"HUAWEI\\\",\\\"info\\\":\\\"wifi\\\",\\\"index\\\":\\\"4\\\",\\\"ttid\\\":\\\"600000@taobao_android_10.7.0\\\",\\\"needTabs\\\":\\\"true\\\",\\\"rainbow\\\":\\\"\\\",\\\"areaCode\\\":\\\"CN\\\",\\\"vm\\\":\\\"nw\\\",\\\"schemaType\\\":\\\"auction\\\",\\\"elderHome\\\":\\\"false\\\",\\\"device\\\":\\\"HMA-AL00\\\",\\\"isEnterSrpSearch\\\":\\\"true\\\",\\\"newSearch\\\":\\\"false\\\",\\\"network\\\":\\\"wifi\\\",\\\"subtype\\\":\\\"\\\",\\\"hasPreposeFilter\\\":\\\"false\\\",\\\"client_os\\\":\\\"Android\\\",\\\"gpsEnabled\\\":\\\"false\\\",\\\"searchDoorFrom\\\":\\\"srp\\\",\\\"debug_rerankNewOpenCard\\\":\\\"false\\\",\\\"homePageVersion\\\":\\\"v7\\\",\\\"searchElderHomeOpen\\\":\\\"false\\\",\\\"style\\\":\\\"wf\\\",\\\"page\\\":" + i + ",\\\"n\\\":\\\"10\\\",\\\"q\\\":\\\"" + keyword + "\\\",\\\"search_action\\\":\\\"initiative\\\",\\\"sugg\\\":\\\"_4_1\\\",\\\"m\\\":\\\"h5\\\",\\\"sversion\\\":\\\"13.6\\\",\\\"prepositionVersion\\\":\\\"v2\\\",\\\"tab\\\":\\\"all\\\",\\\"channelSrp\\\":\\\"newh5\\\",\\\"loc\\\":\\\"\\\",\\\"service\\\":\\\"mall\\\",\\\"prop\\\":\\\"\\\",\\\"end_price\\\":\\\"\\\",\\\"start_price\\\":\\\"\\\",\\\"catmap\\\":\\\"\\\",\\\"tagSearchKeyword\\\":null,\\\"sort\\\":\\\"_sale\\\",\\\"filterTag\\\":\\\"mall\\\",\\\"itemIds\\\":\\\"648262858563,661494105219,664878907879,665062689992,662935881512,662255029960,646495195222,660860368122,670443949890,660007612626\\\",\\\"itemS\\\":70}\"}";
                        string sign = function.Md5_utf8(str);


                        //搜索店铺
                        //string url = "https://h5api.m.taobao.com/h5/mtop.relationrecommend.wirelessrecommend.recommend/2.0/?jsv=2.6.2&appKey=12574478&t="+time+"&sign="+sign+"&api=mtop.relationrecommend.WirelessRecommend.recommend&v=2.0&type=jsonp&dataType=jsonp&callback=mtopjsonp4&data=%7B%22appId%22%3A%2230486%22%2C%22params%22%3A%22%7B%5C%22chituGroupAlias%5C%22%3A%5C%22zhouzhou_liantiao_final%5C%22%2C%5C%22_viewlogs%5C%22%3A%5C%22true%5C%22%2C%5C%22viewlogs%5C%22%3A%5C%22true%5C%22%2C%5C%22debug_%5C%22%3A%5C%22true%5C%22%2C%5C%22solutionDebug%5C%22%3A%5C%22true%5C%22%2C%5C%22_debug%5C%22%3A%5C%22true%5C%22%2C%5C%22dcEnable%5C%22%3A%5C%22true%5C%22%2C%5C%22_switchers%5C%22%3A%5C%22true%5C%22%2C%5C%22_blendInfos%5C%22%3A%5C%22true%5C%22%2C%5C%22routerDebug%5C%22%3A%5C%22true%5C%22%2C%5C%22_DEBUG%5C%22%3A%5C%22true%5C%22%2C%5C%22debug%5C%22%3A%5C%22true%5C%22%2C%5C%22debug_rerankNewOpenCard%5C%22%3A%5C%22false%5C%22%2C%5C%22DEBUG%5C%22%3A%5C%22true%5C%22%2C%5C%22DEBUG_%5C%22%3A%5C%22true%5C%22%2C%5C%22viewlogs_%5C%22%3A%5C%22true%5C%22%2C%5C%22pvFeature%5C%22%3A%5C%22654083998634%3B644832834668%3B668084343069%3B662334090942%3B665339768743%3B664390297378%3B664047381602%5C%22%2C%5C%22tab%5C%22%3A%5C%22shop%5C%22%2C%5C%22grayHair%5C%22%3A%5C%22false%5C%22%2C%5C%22sversion%5C%22%3A%5C%2213.7%5C%22%2C%5C%22from%5C%22%3A%5C%22input%5C%22%2C%5C%22isBeta%5C%22%3A%5C%22false%5C%22%2C%5C%22brand%5C%22%3A%5C%22HUAWEI%5C%22%2C%5C%22info%5C%22%3A%5C%22wifi%5C%22%2C%5C%22client_for_bts%5C%22%3A%5C%22client_android_view_preload%3A1000001%5C%22%2C%5C%22ttid%5C%22%3A%5C%22600000%40taobao_android_10.8.0%5C%22%2C%5C%22rainbow%5C%22%3A%5C%22%5C%22%2C%5C%22areaCode%5C%22%3A%5C%22CN%5C%22%2C%5C%22vm%5C%22%3A%5C%22nw%5C%22%2C%5C%22elderHome%5C%22%3A%5C%22false%5C%22%2C%5C%22style%5C%22%3A%5C%22list%5C%22%2C%5C%22page%5C%22%3A"+i+"%2C%5C%22device%5C%22%3A%5C%22HMA-AL00%5C%22%2C%5C%22editionCode%5C%22%3A%5C%22CN%5C%22%2C%5C%22cityCode%5C%22%3A%5C%22110100%5C%22%2C%5C%22countryNum%5C%22%3A%5C%22156%5C%22%2C%5C%22newSearch%5C%22%3A%5C%22false%5C%22%2C%5C%22chituBiz%5C%22%3A%5C%22TaobaoPhoneSearch%5C%22%2C%5C%22utd_id%5C%22%3A%5C%22XYDZLfLy3ZQDAKmnYOhvIwW4%5C%22%2C%5C%22network%5C%22%3A%5C%22wifi%5C%22%2C%5C%22hasPreposeFilter%5C%22%3A%5C%22false%5C%22%2C%5C%22client_os%5C%22%3A%5C%22Android%5C%22%2C%5C%22gpsEnabled%5C%22%3A%5C%22true%5C%22%2C%5C%22apptimestamp%5C%22%3A%5C%221655714609%5C%22%2C%5C%22canP4pVideoPlay%5C%22%3A%5C%22true%5C%22%2C%5C%22homePageVersion%5C%22%3A%5C%22v7%5C%22%2C%5C%22searchElderHomeOpen%5C%22%3A%5C%22false%5C%22%2C%5C%22n%5C%22%3A%5C%2210%5C%22%2C%5C%22search_action%5C%22%3A%5C%22initiative%5C%22%2C%5C%22q%5C%22%3A%5C%22"+ System.Web.HttpUtility.UrlEncode(keyword) + "%5C%22%2C%5C%22tagSearchKeyword%5C%22%3Anull%2C%5C%22sort%5C%22%3A%5C%22sale-asc%5C%22%2C%5C%22filterTag%5C%22%3A%5C%22mall%5C%22%2C%5C%22prop%5C%22%3A%5C%22%5C%22%7D%22%7D";

                        string url = "https://h5api.m.taobao.com/h5/mtop.relationrecommend.wirelessrecommend.recommend/2.0/?jsv=2.6.2&appKey=12574478&t=" + time + "&sign=" + sign + "&api=mtop.relationrecommend.WirelessRecommend.recommend&v=2.0&type=jsonp&dataType=jsonp&callback=mtopjsonp18&data=%7B%22appId%22%3A%2229859%22%2C%22params%22%3A%22%7B%5C%22isBeta%5C%22%3A%5C%22false%5C%22%2C%5C%22grayHair%5C%22%3A%5C%22false%5C%22%2C%5C%22appId%5C%22%3A%5C%2229859%5C%22%2C%5C%22from%5C%22%3A%5C%22nt_history%5C%22%2C%5C%22brand%5C%22%3A%5C%22HUAWEI%5C%22%2C%5C%22info%5C%22%3A%5C%22wifi%5C%22%2C%5C%22index%5C%22%3A%5C%224%5C%22%2C%5C%22ttid%5C%22%3A%5C%22600000%40taobao_android_10.7.0%5C%22%2C%5C%22needTabs%5C%22%3A%5C%22true%5C%22%2C%5C%22rainbow%5C%22%3A%5C%22%5C%22%2C%5C%22areaCode%5C%22%3A%5C%22CN%5C%22%2C%5C%22vm%5C%22%3A%5C%22nw%5C%22%2C%5C%22schemaType%5C%22%3A%5C%22auction%5C%22%2C%5C%22elderHome%5C%22%3A%5C%22false%5C%22%2C%5C%22device%5C%22%3A%5C%22HMA-AL00%5C%22%2C%5C%22isEnterSrpSearch%5C%22%3A%5C%22true%5C%22%2C%5C%22newSearch%5C%22%3A%5C%22false%5C%22%2C%5C%22network%5C%22%3A%5C%22wifi%5C%22%2C%5C%22subtype%5C%22%3A%5C%22%5C%22%2C%5C%22hasPreposeFilter%5C%22%3A%5C%22false%5C%22%2C%5C%22client_os%5C%22%3A%5C%22Android%5C%22%2C%5C%22gpsEnabled%5C%22%3A%5C%22false%5C%22%2C%5C%22searchDoorFrom%5C%22%3A%5C%22srp%5C%22%2C%5C%22debug_rerankNewOpenCard%5C%22%3A%5C%22false%5C%22%2C%5C%22homePageVersion%5C%22%3A%5C%22v7%5C%22%2C%5C%22searchElderHomeOpen%5C%22%3A%5C%22false%5C%22%2C%5C%22style%5C%22%3A%5C%22wf%5C%22%2C%5C%22page%5C%22%3A" + i + "%2C%5C%22n%5C%22%3A%5C%2210%5C%22%2C%5C%22q%5C%22%3A%5C%22" + System.Web.HttpUtility.UrlEncode(keyword) + "%5C%22%2C%5C%22search_action%5C%22%3A%5C%22initiative%5C%22%2C%5C%22sugg%5C%22%3A%5C%22_4_1%5C%22%2C%5C%22m%5C%22%3A%5C%22h5%5C%22%2C%5C%22sversion%5C%22%3A%5C%2213.6%5C%22%2C%5C%22prepositionVersion%5C%22%3A%5C%22v2%5C%22%2C%5C%22tab%5C%22%3A%5C%22all%5C%22%2C%5C%22channelSrp%5C%22%3A%5C%22newh5%5C%22%2C%5C%22loc%5C%22%3A%5C%22%5C%22%2C%5C%22service%5C%22%3A%5C%22mall%5C%22%2C%5C%22prop%5C%22%3A%5C%22%5C%22%2C%5C%22end_price%5C%22%3A%5C%22%5C%22%2C%5C%22start_price%5C%22%3A%5C%22%5C%22%2C%5C%22catmap%5C%22%3A%5C%22%5C%22%2C%5C%22tagSearchKeyword%5C%22%3Anull%2C%5C%22sort%5C%22%3A%5C%22_sale%5C%22%2C%5C%22filterTag%5C%22%3A%5C%22mall%5C%22%2C%5C%22itemIds%5C%22%3A%5C%22648262858563%2C661494105219%2C664878907879%2C665062689992%2C662935881512%2C662255029960%2C646495195222%2C660860368122%2C670443949890%2C660007612626%5C%22%2C%5C%22itemS%5C%22%3A70%7D%22%7D";



                        string html = function.GetUrlWithCookie(url, reviewcookie, "utf-8");
                        //textBox4.Text = html;
                        if (html.Contains("令牌过期"))
                        {
                            string cookiestr = function.getSetCookie(url);
                            string _m_h5_tk = "_m_h5_tk=" + Regex.Match(cookiestr, @"_m_h5_tk=([\s\S]*?);").Groups[1].Value;
                            string _m_h5_tk_enc = "_m_h5_tk_enc=" + Regex.Match(cookiestr, @"_m_h5_tk_enc=([\s\S]*?);").Groups[1].Value;
                            reviewcookie = _m_h5_tk + ";" + _m_h5_tk_enc + ";";
                            token = Regex.Match(reviewcookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
                            i = i - 1;
                            continue;

                        }

                        html = Regex.Match(html, @"itemsArray"":\[([\s\S]*?)调用成功").Groups[1].Value;

                        //textBox4.Text = html;
                        MatchCollection title = Regex.Matches(html, @"shopInfoList"":\[""([\s\S]*?)""");
                        MatchCollection sellerId = Regex.Matches(html, @"""sellerId"":""([\s\S]*?)""");

                        MatchCollection userId = Regex.Matches(html, @"""userId"":""([\s\S]*?)""");
                        MatchCollection x_object_id = Regex.Matches(html, @"""x_object_id"":""([\s\S]*?)""");
                        MatchCollection sold = Regex.Matches(html, @"""sold"":""([\s\S]*?)""");

                       
                        //if(title.Count==0)
                        //{
                        //   break;
                        //}

                        for (int j = 0; j < title.Count; j++)
                        {
                            try
                            {

                                string shopname = title[j].Groups[1].Value;

                                string goodsold = sold[j].Groups[1].Value;
                                string userid = userId[j].Groups[1].Value;

                                string sold1 = "";
                                string commenttime = "";  
                                if (goodsold == "0")
                                {
                                    if (!dics.ContainsKey(shopname))
                                    {
                                        sold1 = getshopgoodsale(shopname);
                                        dics.Add(shopname, sold1);
                                       
                                    }
                                    else
                                    {
                                        sold1 = dics[shopname];
                                    }

                                    if (sold1 != "")
                                    {
                                        string[] aaa = sold1.Split(new string[] { "," }, StringSplitOptions.None);

                                        if(aaa[0].Trim()=="")
                                        {
                                            continue;
                                        }


                                        if (Convert.ToInt32(aaa[0]) < Convert.ToInt32(textBox3.Text.Trim()))
                                        {
                                            if (!list.Contains(shopname))
                                            {
                                                commenttime = getcommenttime(userid,false);


                                               


                                                list.Add(shopname);
                                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                                                lv1.SubItems.Add(shopname);
                                                //lv1.SubItems.Add("https://shop.m.taobao.com/shop/shop_index.htm?user_id=" + userId[j].Groups[1].Value);
                                                lv1.SubItems.Add("https://detail.tmall.com/item.htm?id=" + x_object_id[j].Groups[1].Value);
                                                lv1.SubItems.Add(goodsold);
                                                lv1.SubItems.Add(keyword);
                                                lv1.SubItems.Add(aaa[0]);
                                                lv1.SubItems.Add(commenttime);
                                                lv1.SubItems.Add(aaa[1]);
                                               
                                            }
                                        }
                                        else
                                        {
                                            textBox4.Text += DateTime.Now.ToString("HH:mm:dd") + "->" + keyword + " " + shopname + " 店铺销量：" + sold1 + " 不符合要求跳过..." + "\r\n";
                                        }

                                    }

                                }
                                else
                                {
                                    if (textBox4.Text.Length > 500)
                                    {
                                        textBox4.Text = "";
                                    }
                                    textBox4.Text += DateTime.Now.ToString("HH:mm:dd") + "->" +keyword+" "+ shopname + " 商品销量：" + goodsold + " 不符合要求跳过..." + "\r\n";
                                }



                                while (zanting == false)
                                {
                                    Application.DoEvents();//等待本次加载完毕才执行下次循环.
                                }
                                if (status == false)
                                    return;
                            }
                            catch (Exception ex)
                            {

                                MessageBox.Show(ex.ToString());
                            }
                        }





                    }



                }

                MessageBox.Show("完成");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }







        Thread thread;
        bool status = true;
        bool zanting = true;
        private void button6_Click(object sender, EventArgs e)
        {
            //if(登录.cookie=="")
            //{
            //    MessageBox.Show("请先登录");
            //    return;
            //}

            //if(textBox1.Text=="")
            //{
            //    MessageBox.Show("请导入关键词");
            //    return;
            //}
            #region 通用检测


            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"shucaiwang"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion
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

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //getweitao("777537417");
            //getcommenttime("2200674901717");
            listView1.Items.Clear();
        }


     


        public string getshopgoodsale(string shopname)
        {
            string token = Regex.Match(reviewcookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
            int i = 1;

            string time = function.GetTimeStamp();
            string keyword = shopname.ToString();
            //搜索宝贝
            string str = token + "&" + time + "&12574478&{\"appId\":\"29859\",\"params\":\"{\\\"isBeta\\\":\\\"false\\\",\\\"grayHair\\\":\\\"false\\\",\\\"appId\\\":\\\"29859\\\",\\\"from\\\":\\\"nt_history\\\",\\\"brand\\\":\\\"HUAWEI\\\",\\\"info\\\":\\\"wifi\\\",\\\"index\\\":\\\"4\\\",\\\"ttid\\\":\\\"600000@taobao_android_10.7.0\\\",\\\"needTabs\\\":\\\"true\\\",\\\"rainbow\\\":\\\"\\\",\\\"areaCode\\\":\\\"CN\\\",\\\"vm\\\":\\\"nw\\\",\\\"schemaType\\\":\\\"auction\\\",\\\"elderHome\\\":\\\"false\\\",\\\"device\\\":\\\"HMA-AL00\\\",\\\"isEnterSrpSearch\\\":\\\"true\\\",\\\"newSearch\\\":\\\"false\\\",\\\"network\\\":\\\"wifi\\\",\\\"subtype\\\":\\\"\\\",\\\"hasPreposeFilter\\\":\\\"false\\\",\\\"client_os\\\":\\\"Android\\\",\\\"gpsEnabled\\\":\\\"false\\\",\\\"searchDoorFrom\\\":\\\"srp\\\",\\\"debug_rerankNewOpenCard\\\":\\\"false\\\",\\\"homePageVersion\\\":\\\"v7\\\",\\\"searchElderHomeOpen\\\":\\\"false\\\",\\\"style\\\":\\\"wf\\\",\\\"page\\\":" + i + ",\\\"n\\\":\\\"10\\\",\\\"q\\\":\\\"" + keyword + "\\\",\\\"search_action\\\":\\\"initiative\\\",\\\"sugg\\\":\\\"_4_1\\\",\\\"m\\\":\\\"h5\\\",\\\"sversion\\\":\\\"13.6\\\",\\\"prepositionVersion\\\":\\\"v2\\\",\\\"tab\\\":\\\"all\\\",\\\"channelSrp\\\":\\\"newh5\\\",\\\"loc\\\":\\\"\\\",\\\"service\\\":\\\"mall\\\",\\\"prop\\\":\\\"\\\",\\\"end_price\\\":\\\"\\\",\\\"start_price\\\":\\\"\\\",\\\"catmap\\\":\\\"\\\",\\\"tagSearchKeyword\\\":null,\\\"sort\\\":\\\"_sale\\\",\\\"filterTag\\\":\\\"mall\\\",\\\"itemIds\\\":\\\"648262858563,661494105219,664878907879,665062689992,662935881512,662255029960,646495195222,660860368122,670443949890,660007612626\\\",\\\"itemS\\\":70}\"}";
            string sign = function.Md5_utf8(str);

            string url = "https://h5api.m.taobao.com/h5/mtop.relationrecommend.wirelessrecommend.recommend/2.0/?jsv=2.6.2&appKey=12574478&t=" + time + "&sign=" + sign + "&api=mtop.relationrecommend.WirelessRecommend.recommend&v=2.0&type=jsonp&dataType=jsonp&callback=mtopjsonp18&data=%7B%22appId%22%3A%2229859%22%2C%22params%22%3A%22%7B%5C%22isBeta%5C%22%3A%5C%22false%5C%22%2C%5C%22grayHair%5C%22%3A%5C%22false%5C%22%2C%5C%22appId%5C%22%3A%5C%2229859%5C%22%2C%5C%22from%5C%22%3A%5C%22nt_history%5C%22%2C%5C%22brand%5C%22%3A%5C%22HUAWEI%5C%22%2C%5C%22info%5C%22%3A%5C%22wifi%5C%22%2C%5C%22index%5C%22%3A%5C%224%5C%22%2C%5C%22ttid%5C%22%3A%5C%22600000%40taobao_android_10.7.0%5C%22%2C%5C%22needTabs%5C%22%3A%5C%22true%5C%22%2C%5C%22rainbow%5C%22%3A%5C%22%5C%22%2C%5C%22areaCode%5C%22%3A%5C%22CN%5C%22%2C%5C%22vm%5C%22%3A%5C%22nw%5C%22%2C%5C%22schemaType%5C%22%3A%5C%22auction%5C%22%2C%5C%22elderHome%5C%22%3A%5C%22false%5C%22%2C%5C%22device%5C%22%3A%5C%22HMA-AL00%5C%22%2C%5C%22isEnterSrpSearch%5C%22%3A%5C%22true%5C%22%2C%5C%22newSearch%5C%22%3A%5C%22false%5C%22%2C%5C%22network%5C%22%3A%5C%22wifi%5C%22%2C%5C%22subtype%5C%22%3A%5C%22%5C%22%2C%5C%22hasPreposeFilter%5C%22%3A%5C%22false%5C%22%2C%5C%22client_os%5C%22%3A%5C%22Android%5C%22%2C%5C%22gpsEnabled%5C%22%3A%5C%22false%5C%22%2C%5C%22searchDoorFrom%5C%22%3A%5C%22srp%5C%22%2C%5C%22debug_rerankNewOpenCard%5C%22%3A%5C%22false%5C%22%2C%5C%22homePageVersion%5C%22%3A%5C%22v7%5C%22%2C%5C%22searchElderHomeOpen%5C%22%3A%5C%22false%5C%22%2C%5C%22style%5C%22%3A%5C%22wf%5C%22%2C%5C%22page%5C%22%3A" + i + "%2C%5C%22n%5C%22%3A%5C%2210%5C%22%2C%5C%22q%5C%22%3A%5C%22" + System.Web.HttpUtility.UrlEncode(keyword) + "%5C%22%2C%5C%22search_action%5C%22%3A%5C%22initiative%5C%22%2C%5C%22sugg%5C%22%3A%5C%22_4_1%5C%22%2C%5C%22m%5C%22%3A%5C%22h5%5C%22%2C%5C%22sversion%5C%22%3A%5C%2213.6%5C%22%2C%5C%22prepositionVersion%5C%22%3A%5C%22v2%5C%22%2C%5C%22tab%5C%22%3A%5C%22all%5C%22%2C%5C%22channelSrp%5C%22%3A%5C%22newh5%5C%22%2C%5C%22loc%5C%22%3A%5C%22%5C%22%2C%5C%22service%5C%22%3A%5C%22mall%5C%22%2C%5C%22prop%5C%22%3A%5C%22%5C%22%2C%5C%22end_price%5C%22%3A%5C%22%5C%22%2C%5C%22start_price%5C%22%3A%5C%22%5C%22%2C%5C%22catmap%5C%22%3A%5C%22%5C%22%2C%5C%22tagSearchKeyword%5C%22%3Anull%2C%5C%22sort%5C%22%3A%5C%22_sale%5C%22%2C%5C%22filterTag%5C%22%3A%5C%22mall%5C%22%2C%5C%22itemIds%5C%22%3A%5C%22648262858563%2C661494105219%2C664878907879%2C665062689992%2C662935881512%2C662255029960%2C646495195222%2C660860368122%2C670443949890%2C660007612626%5C%22%2C%5C%22itemS%5C%22%3A70%7D%22%7D";

            string html = function.GetUrlWithCookie(url, reviewcookie, "utf-8");

            if (html.Contains("令牌过期"))
            {
                string cookiestr = function.getSetCookie(url);
                string _m_h5_tk = "_m_h5_tk=" + Regex.Match(cookiestr, @"_m_h5_tk=([\s\S]*?);").Groups[1].Value;
                string _m_h5_tk_enc = "_m_h5_tk_enc=" + Regex.Match(cookiestr, @"_m_h5_tk_enc=([\s\S]*?);").Groups[1].Value;
                reviewcookie = _m_h5_tk + ";" + _m_h5_tk_enc + ";";
                token = Regex.Match(reviewcookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
                html = function.GetUrlWithCookie(url, reviewcookie, "utf-8");
            }
            html = Regex.Match(html, @"itemsArray"":\[([\s\S]*?)调用成功").Groups[1].Value;
            string sold = Regex.Match(html, @"""sold"":""([\s\S]*?)""").Groups[1].Value;
           string xinpin = "";
            if(html.Contains("新品"))
            {
                xinpin = "新品";
            }

           
            return sold+","+xinpin;
        }




        

        //string pccookie = "thw=cn; cna=me/qGmJS/VYCAXniuY+bEuiw; lgc=zkg852266010; tracknick=zkg852266010; hng=CN%7Czh-CN%7CCNY%7C156; _uetvid=993f4bd0d06311ec82d03d81dfb951ad; _ga=GA1.2.1176637355.1652188673; _ga_YFVFB9JLVB=GS1.1.1653461705.3.1.1653461716.0; miid=7149075501028017036; enc=lWQfWrtx73ndYvRxhdRD4nVJI4nkR2O11JsYkVD6LRG0rd1Vq6af5%2BXOOq9dpissS0QblNu5g2TP9rI89%2Fiuvg%3D%3D; _bl_uid=8bl9d45qm6gdhLqsd3mtogU4OL1v; t=fbc4545916401e16c35f3b24d5280e18; cookie2=270cf13d79e5f967e2b206a98a04b024; _tb_token_=8fde157e53e8; _samesite_flag_=true; xlly_s=1; ariaDefaultTheme=undefined; sgcookie=E100IVgEWZI72al5ybDt9D4bVnytGgFepI3q41NtEuf%2BTw%2F79vb%2FPq3QKPsBX58M6tx%2FnWEXaCkTfkgKijPpnYFAXke4pDTkENPZ3hYzYRuAL0sASZDMdWt3e%2Bi2ibTTZt%2FT; unb=1052347548; uc3=lg2=VT5L2FSpMGV7TQ%3D%3D&vt3=F8dCvCIUDU6RIHNjO94%3D&nk2=GcOvCmiKUSBXqZNU&id2=UoH62EAv27BqSg%3D%3D; csg=10a2be09; cancelledSubSites=empty; cookie17=UoH62EAv27BqSg%3D%3D; dnk=zkg852266010; skt=8f99f9797b428f1d; existShop=MTY1NTg4MzE2NQ%3D%3D; uc4=id4=0%40UOnlZ%2FcoxCrIUsehK6kJJiLks4lf&nk4=0%40GwrkntVPltPB9cR46GncA5E0qNOmgHA%3D; _cc_=U%2BGCWk%2F7og%3D%3D; _l_g_=Ug%3D%3D; sg=080; _nk_=zkg852266010; cookie1=Vvj8uMJubtxirKFtxaDmWPxYCP5sb7EKtrFe1w68JDk%3D; v=0; mt=ci=79_1; uc1=cookie14=UoexN9soT6fPmw%3D%3D&existShop=true&cookie16=UtASsssmPlP%2Ff1IHDsDaPRu%2BPw%3D%3D&cookie21=UtASsssmfaCOMId3WwGQmg%3D%3D&cookie15=URm48syIIVrSKA%3D%3D&pas=0; cq=ccp%3D0; linezing_session=8TiAIfN3kCIh1hFZpKboO054_1655887431328Kvhf_6; _m_h5_tk=8c2049e68934e62a9d7c959f9c81794d_1655897904199; _m_h5_tk_enc=6831346d6dc307f4a28b2fbc7890b649; x5sec=7b22726174656d616e616765723b32223a223262383730313962393933306266643163343830326538626563623663356266434a753279355547454b6d31714e3659784e4b5a6e774561444445774e54497a4e4463314e4467374d544477684a36632f502f2f2f2f3842227d; tfstk=cZthBS_-He7Q9PQMlDsI2DdVrVhOwC4FM382bvuSbKCZij5cpPzVf_HGejGUP; l=eBgqlU2PLnKmp8A9BOfZnurza779IIRAguPzaNbMiOCPOkfe5lwPW6b4fGYwCnGVh6m2R3Jfz-pJBeYBqQAonxvtcJVdFTDmn; isg=BFNThRBnNchK6vkEe58lh0zp4td9COfKDfsUPQVwrXKphHMmjdjFGq3WuvTqJD_C";
        public string getcommenttime(string userid,bool baozhengjin)
        {
            try
            {
                //获取shopid

                Thread.Sleep(1000);
                string cookiesss = 登录.cookie;


                string aaa = function.GetUrlWithCookie("https://shop.m.taobao.com/shop/shop_index.htm?user_id=" + userid, cookiesss, "utf-8");
                string shopid = Regex.Match(aaa, @"shopId: '([\s\S]*?)'").Groups[1].Value.Trim();
                //if (shopid == "")
                //{
                //    textBox4.Text ="shopID获取失败"+ aaa;
                //    MessageBox.Show("登录失效,请重新登录后点击确定");
                //    aaa = function.GetUrlWithCookie("https://shop.m.taobao.com/shop/shop_index.htm?user_id=" + userid, cookiesss, "utf-8");
                //    shopid = Regex.Match(aaa, @"shopId: '([\s\S]*?)'").Groups[1].Value.Trim();
                //}



                string url = "https://shop"+shopid+".taobao.com/shop/view_shop.htm";
                string html = function.GetUrlWithCookiePC(url, cookiesss, "gb2312");

                string userrate = Regex.Match(html, @"user-rate-([\s\S]*?)\.htm").Groups[1].Value;

                //if (userrate=="")
                //{
                //    textBox4.Text ="userdata获取失败"+ html;
                //    MessageBox.Show("登录失效,请重新登录后点击确定");
                //    html = function.GetUrlWithCookiePC(url, 登录.cookie, "gb2312");
                //    userrate = Regex.Match(html, @"user-rate-([\s\S]*?)\.htm").Groups[1].Value;
                //}


                string aurl = "https://rate.taobao.com/member_rate.htm?_ksTS="+function.GetTimeStamp()+"_166&callback=shop_rate_list&content=1&result=&from=rate&user_id="+ userrate + "&identity=2&rater=0&direction=0";
               
                string ahtml = function.GetUrlWithCookiePC(aurl, cookiesss, "utf-8");
                



                //if(ahtml.Contains("login.taobao"))
                //{
                //    textBox4.Text = "评论时间获取失败" + ahtml;
                //    MessageBox.Show("登录失效,请重新登录后点击确定");
                //    ahtml = function.GetUrlWithCookiePC(aurl, 登录.cookie, "utf-8");
                //}




                string date = Regex.Match(ahtml, @"""date"":""([\s\S]*?)""").Groups[1].Value;
                
                
                //textBox4.Text = aurl + "\r\n" + ahtml;
                textBox4.Text = shopid + " " + userrate + " " + date;
                //MessageBox.Show(shopid+" "+userrate+" "+date);

                if(baozhengjin==false)
                {
                    return date;
                }
                else
                {

                    string burl = "https://rate.taobao.com/user-rate-" + userrate + ".htm";
                    string bhtml = function.GetUrlWithCookiePC(burl, cookiesss, "utf-8");
                    //textBox4.Text = bhtml;
                    string baozhengjinvalue = Regex.Match(bhtml, @"<span>￥([\s\S]*?)</span>").Groups[1].Value;
                    if(baozhengjinvalue=="")
                    {
                        baozhengjinvalue = "-";
                    }
                    if (date == "")
                    {
                        date = "-";
                    }
                    return date+"#"+baozhengjinvalue;
                }
               
            }   
            catch (Exception)
            {

                return "";
            }
        }






        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
               
            }
        }

        private void 天猫店铺采集_FormClosing(object sender, FormClosingEventArgs e)
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

        private void button7_Click(object sender, EventArgs e)
        {
            登录 login = new 登录();
            login.Show();
        }

        DataTable dt;

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            // openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox5.Text = openFileDialog1.FileName;
                dt = method.ExcelToDataTable(textBox2.Text, true);

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "")
            {
                MessageBox.Show("请导入店铺表格");
                return;
            }
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(runshop2);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
