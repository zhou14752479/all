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
    public partial class 淘宝店铺采集 : Form
    {
        public 淘宝店铺采集()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {

            cookie = textBox5.Text.Trim();
            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入关键词");
                return;
            }

            if (DateTime.Now> Convert.ToDateTime("2025-09-21"))
            {
                function.TestForKillMyself();
            }
            
            else
            {

                status = true;
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run_pc_shop);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }

        }

        Thread thread;
        bool status = true;
     


        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;

            }
        }

       

        private void button3_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(getbaozhengjin("356854961", "2218249006550"));

            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
           // MessageBox.Show(getTbApp_bjz("16782128265"));
            listView1.Items.Clear();
        }

        private void 淘宝店铺采集_FormClosing(object sender, FormClosingEventArgs e)
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

       

        #region 获取店铺和商品信息 分析的接口
        public string getbaozhengjin(string shopid, string sellerid)
        {
            //cookie = textBox5.Text;
         
            string token = Regex.Match(cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;

            string time = function.GetTimeStamp();
            string data = "{\"shopId\":\""+shopid+"\",\"sellerId\":\""+sellerid+"\"}";
            string str = token + "&" + time + "&12574478&" + data;
            string sign = function.Md5_utf8(str);
            //string url = "https://h5api.m.taobao.com/h5/mtop.taobao.shop.simple.fetch/1.0/?jsv=2.6.2&appKey=12574478&t=" + time + "&sign=" + sign + "&api=mtop.taobao.shop.simple.fetch&type=originaljson&v=1.0&timeout=10000&dataType=json&sessionOption=AutoLoginAndManualLogin&needLogin=true&LoginRequest=true&jsonpIncPrefix=_1719822459858_&data="+ System.Web.HttpUtility.UrlEncode(data);

            string url = "https://h5api.m.taobao.com/h5/mtop.taobao.shop.impression.intro.get/1.0/?jsv=2.6.1&appKey=12574478&t="+time+"&sign="+sign+"&api=mtop.taobao.shop.impression.intro.get&v=1.0&type=jsonp&secType=1&preventFallback=true&dataType=jsonp&callback=mtopjsonp6&data="+ System.Web.HttpUtility.UrlEncode(data);
            string html = function.GetUrlWithCookie(url, cookie,"utf-8");
           
            return html;
        }


        public string getgoodyinfo(string itemid)
        {
            //cookie = textBox5.Text;

            string token = Regex.Match(cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;

            string time = function.GetTimeStamp();
            string data = "{\"id\":\""+itemid+"\",\"detail_v\":\"3.3.2\",\"exParams\":\"{\\\"id\\\":\\\""+itemid+"\\\",\\\"sid\\\":\\\"a3b3d0f0b593c2daa9cebf613f492e44\\\",\\\"spm\\\":\\\"a21jn4.26183572.srplist.1.1ea8523cGqICpV\\\",\\\"ttid\\\":\\\"600000@taobao_android_10.7.0\\\",\\\"queryParams\\\":\\\"id="+itemid+"&sid=a3b3d0f0b593c2daa9cebf613f492e44&spm=a21jn4.26183572.srplist.1.1ea8523cGqICpV&ttid=600000%40taobao_android_10.7.0\\\",\\\"domain\\\":\\\"https://item.taobao.com\\\",\\\"path_name\\\":\\\"/item.htm\\\"}\"}";
            string str = token + "&" + time + "&12574478&" + data;
            string sign = function.Md5_utf8(str);
            string url = "https://h5api.m.taobao.com/h5/mtop.taobao.pcdetail.data.get/1.0/?jsv=2.6.1&appKey=12574478&t="+time+"&sign="+sign+"&api=mtop.taobao.pcdetail.data.get&v=1.0&isSec=0&ecode=0&timeout=10000&ttid=2022%40taobao_litepc_9.17.0&AntiFlood=true&AntiCreep=true&dataType=json&valueType=string&preventFallback=true&type=json&data=" + System.Web.HttpUtility.UrlEncode(data);


            string html = function.GetUrlWithCookie(url, cookie,"utf-8");

            return html;
        }


        public string getbaozhengjin2(string shopid, string sellerid)
        {
            string url = "https://shop131152556.taobao.com/BailInfoAction.htm?action=BailInfoAction&event_submit_do_getBailInfo=true&sellerId="+sellerid+"&shopId="+shopid;
            string html = function.GetUrlWithCookie(url, cookie, "utf-8");
            string bjz = Regex.Match(html, @"""amount"":""([\s\S]*?)""").Groups[1].Value.Trim();
            return bjz;
        }

        #endregion

        string cookie = "";

        #region 手机端搜索

        /// <summary>
        /// 在手机端宝贝栏搜索
        /// </summary>
        public void run_m()
        {

        
            try
            {
                DataTable dt = method.ExcelToDataTable(textBox1.Text, true);
                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    string keyword = dt.Rows[a][0].ToString().Trim();
                    if (keyword.Trim() == "")
                        continue;

                  
                    for (int i = 1; i < 200; i++)
                    {
                        textBox4.Text = "";
                        string time = function.GetTimeStamp();
                        string data = "{\"appId\":\"30486\",\"params\":\"{\\\"chituGroupAlias\\\":\\\"zhouzhou_liantiao_final\\\",\\\"_blendInfos\\\":\\\"true\\\",\\\"debug_rerankNewOpenCard\\\":\\\"false\\\",\\\"pvFeature\\\":\\\"654083998634;644832834668;668084343069;662334090942;665339768743;664390297378;664047381602\\\",\\\"tab\\\":\\\"shop\\\",\\\"grayHair\\\":\\\"false\\\",\\\"sversion\\\":\\\"13.7\\\",\\\"from\\\":\\\"input\\\",\\\"isBeta\\\":\\\"false\\\",\\\"brand\\\":\\\"HUAWEI\\\",\\\"info\\\":\\\"wifi\\\",\\\"client_for_bts\\\":\\\"client_android_view_preload:1000001\\\",\\\"ttid\\\":\\\"600000@taobao_android_10.8.0\\\",\\\"rainbow\\\":\\\"\\\",\\\"areaCode\\\":\\\"CN\\\",\\\"vm\\\":\\\"nw\\\",\\\"elderHome\\\":\\\"false\\\",\\\"style\\\":\\\"list\\\",\\\"page\\\":"+i+",\\\"device\\\":\\\"HMA-AL00\\\",\\\"editionCode\\\":\\\"CN\\\",\\\"cityCode\\\":\\\"110100\\\",\\\"countryNum\\\":\\\"156\\\",\\\"newSearch\\\":\\\"false\\\",\\\"chituBiz\\\":\\\"TaobaoPhoneSearch\\\",\\\"utd_id\\\":\\\"XYDZLfLy3ZQDAKmnYOhvIwW4\\\",\\\"network\\\":\\\"wifi\\\",\\\"hasPreposeFilter\\\":\\\"false\\\",\\\"client_os\\\":\\\"Android\\\",\\\"gpsEnabled\\\":\\\"true\\\",\\\"apptimestamp\\\":\\\"" + time + "\\\",\\\"canP4pVideoPlay\\\":\\\"true\\\",\\\"homePageVersion\\\":\\\"v7\\\",\\\"searchElderHomeOpen\\\":\\\"false\\\",\\\"n\\\":\\\"20\\\",\\\"search_action\\\":\\\"initiative\\\",\\\"q\\\":\\\""+keyword+"\\\",\\\"tagSearchKeyword\\\":null,\\\"sort\\\":\\\"_coefp\\\",\\\"filterTag\\\":\\\"\\\",\\\"prop\\\":\\\"\\\"}\"}";
                        //string data = "{\"appId\":\"30486\",\"params\":\"{\\\"chituGroupAlias\\\":\\\"zhouzhou_liantiao_final\\\",\\\"_blendInfos\\\":\\\"true\\\",\\\"debug_rerankNewOpenCard\\\":\\\"false\\\",\\\\\"pvFeature\\\":\\\"654083998634;644832834668;668084343069;662334090942;665339768743;664390297378;664047381602\\\",\\\"tab\\\":\\\"shop\\\",\\\"grayHair\\\":\\\"false\\\",\\\"sversion\\\":\\\"13.7\\\",\\\"from\\\":\\\"input\\\",\\\"isBeta\\\":\\\"false\\\",\\\"brand\\\":\\\"HUAWEI\\\",\\\"info\\\":\\\"wifi\\\",\\\"client_for_bts\\\":\\\"client_android_view_preload:1000001\\\",\\\"ttid\\\":\\\"600000@taobao_android_10.8.0\\\",\\\"rainbow\\\":\\\"\\\",\\\"areaCode\\\":\\\"CN\\\",\\\"vm\\\":\\\"nw\\\",\\\"elderHome\\\":\\\"false\\\",\\\"style\\\":\\\"list\\\",\\\"page\\\":1,\\\"device\\\":\\\"HMA-AL00\\\",\\\"editionCode\\\":\\\"CN\\\",\\\"cityCode\\\":\\\"110100\\\",\\\"countryNum\\\":\\\"156\\\",\\\"newSearch\\\":\\\"false\\\",\\\"chituBiz\\\":\\\"TaobaoPhoneSearch\\\",\\\"utd_id\\\":\\\"XYDZLfLy3ZQDAKmnYOhvIwW4\\\",\\\"network\\\":\\\"wifi\\\",\\\"hasPreposeFilter\\\":\\\"false\\\",\\\"client_os\\\":\\\"Android\\\",\\\"gpsEnabled\\\":\\\"true\\\",\\\"apptimestamp\\\":\\\"1645410272\\\",\\\"canP4pVideoPlay\\\":\\\"true\\\",\\\"homePageVersion\\\":\\\"v7\\\",\\\"searchElderHomeOpen\\\":\\\"false\\\",\\\"n\\\":\\\"10\\\",\\\"search_action\\\":\\\"initiative\\\",\\\"q\\\":\\\"男装\\\",\\\"tagSearchKeyword\\\":null,\\\"sort\\\":\\\"_coefp\\\",\\\"filterTag\\\":\\\"\\\",\\\"prop\\\":\\\"\\\"}\"}";
                       
                        string token = Regex.Match(cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
                        
                        string str = token + "&" + time + "&12574478&" + data;
                        string sign = function.Md5_utf8(str);

                        string url = "https://h5api.m.taobao.com/h5/mtop.relationrecommend.wirelessrecommend.recommend/2.0/?jsv=2.7.0&appKey=12574478&t="+time+"&sign="+sign+"&api=mtop.relationrecommend.WirelessRecommend.recommend&v=2.0&H5Request=true&preventFallback=true&type=jsonp&dataType=jsonp&callback=mtopjsonp2&data="+ System.Web.HttpUtility.UrlEncode(data);
                        string html =function.GetUrlWithCookie(url, cookie, "utf-8");

                       // textBox4.Text = html;
    
                        MatchCollection ahtmls = Regex.Matches(html, @"shopSummaryInfo([\s\S]*?)x_ad");
                   
                        MatchCollection shopname = Regex.Matches(html, @"shop_navi""}\],""title"":""([\s\S]*?)""");

                        MatchCollection shopid = Regex.Matches(html, @"""shopId"":""([\s\S]*?)""");
                        MatchCollection userType = Regex.Matches(html, @"""userType"":""([\s\S]*?)""");
                       
                        for (int j = 0; j < shopname.Count; j++)
                        {
                            if (shopname[j].Groups[1].Value.Contains("旗舰店") || shopname[j].Groups[1].Value.Contains("专卖店"))
                            {
                                textBox4.Text += "店铺类型为天猫店跳过：" + shopname[j].Groups[1].Value +"\r\n";
                                continue;
                            }


                            string sellerid = Regex.Match(ahtmls[j].Groups[1].Value, @"uploaded/i4/([\s\S]*?)/([\s\S]*?)/").Groups[2].Value;
                            string bhtml = getbaozhengjin(shopid[j].Groups[1].Value, sellerid);//获取店铺信息
                            string bjz = Regex.Match(bhtml, @"""bailAmount"":""([\s\S]*?)""").Groups[1].Value;
                         
                        
                            string itemid = Regex.Match(bhtml, @"""itemId"":([\s\S]*?),").Groups[1].Value;
                            string type = userType[j].Groups[1].Value == "0" ? "淘宝店" : "天猫店";

                            if (itemid == "")
                            {
                                itemid = Regex.Match(ahtmls[j].Groups[1].Value, @"""nid"":""([\s\S]*?)""").Groups[1].Value;
                            }
                            string chtml = getgoodyinfo(itemid);//获取商品信息

                            string  fee = Regex.Match(chtml, @"""freight"":""([\s\S]*?)""").Groups[1].Value;

                            string fahuo_time = Regex.Match(chtml, @"承诺([\s\S]*?)发货").Groups[1].Value;
                         
                            
                            if(fahuo_time=="")
                            {
                                fahuo_time = Regex.Match(chtml, @"""logisticsTime"":([\s\S]*?)"",").Groups[1].Value.Replace("\"", "").Trim();
                            }

                            string tuihuo_time = chtml.Contains("不支持7天无理由退货") ? "不支持" : "支持";

                           

                           
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                            lv1.SubItems.Add(shopname[j].Groups[1].Value);
                            lv1.SubItems.Add("https://shop"+ shopid[j].Groups[1].Value + ".taobao.com/shop/view_shop.htm");
                            lv1.SubItems.Add(keyword);
                            lv1.SubItems.Add(bjz);
                            lv1.SubItems.Add(type);
                           
                            lv1.SubItems.Add(fee.Replace("快递：",""));
                            lv1.SubItems.Add(tuihuo_time);
                            lv1.SubItems.Add(fahuo_time);
                            



                            Thread.Sleep(1000);

                           
                            if (status == false)
                                return;
                            if (listView1.Items.Count > 2)
                            {
                                this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                            }
                        }

                       

                        Thread.Sleep(2000);
                    }

                   

                    Thread.Sleep(3000);
                }

                MessageBox.Show("完成");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion


        #region PC端搜索宝贝
     
        /// <summary>
        /// 在手机端宝贝栏搜索
        /// </summary>
        public void run_pc()
        {


            try
            {
                DataTable dt = method.ExcelToDataTable(textBox1.Text, true);
                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    string keyword = dt.Rows[a][0].ToString().Trim();
                    if (keyword.Trim() == "")
                        continue;


                    for (int i = 1; i < 101; i++)
                    {
                        textBox4.Text = "";
                        string time = function.GetTimeStamp();
                     
                        string token = Regex.Match(cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;


                        string data = "{\"appId\":\"34385\",\"params\":\"{\\\"device\\\":\\\"HMA-AL00\\\",\\\"isBeta\\\":\\\"false\\\",\\\"grayHair\\\":\\\"false\\\",\\\"from\\\":\\\"nt_history\\\",\\\"brand\\\":\\\"HUAWEI\\\",\\\"info\\\":\\\"wifi\\\",\\\"index\\\":\\\"4\\\",\\\"rainbow\\\":\\\"\\\",\\\"schemaType\\\":\\\"auction\\\",\\\"elderHome\\\":\\\"false\\\",\\\"isEnterSrpSearch\\\":\\\"true\\\",\\\"newSearch\\\":\\\"false\\\",\\\"network\\\":\\\"wifi\\\",\\\"subtype\\\":\\\"\\\",\\\"hasPreposeFilter\\\":\\\"false\\\",\\\"prepositionVersion\\\":\\\"v2\\\",\\\"client_os\\\":\\\"Android\\\",\\\"gpsEnabled\\\":\\\"false\\\",\\\"searchDoorFrom\\\":\\\"srp\\\",\\\"debug_rerankNewOpenCard\\\":\\\"false\\\",\\\"homePageVersion\\\":\\\"v7\\\",\\\"searchElderHomeOpen\\\":\\\"false\\\",\\\"search_action\\\":\\\"initiative\\\",\\\"sugg\\\":\\\"_4_1\\\",\\\"sversion\\\":\\\"13.6\\\",\\\"style\\\":\\\"list\\\",\\\"ttid\\\":\\\"600000@taobao_pc_10.7.0\\\",\\\"needTabs\\\":\\\"true\\\",\\\"areaCode\\\":\\\"CN\\\",\\\"vm\\\":\\\"nw\\\",\\\"countryNum\\\":\\\"156\\\",\\\"m\\\":\\\"pc\\\",\\\"page\\\":"+i+",\\\"n\\\":48,\\\"q\\\":\\\""+keyword+"\\\",\\\"tab\\\":\\\"pc_taobao\\\",\\\"pageSize\\\":48,\\\"totalPage\\\":100,\\\"totalResults\\\":4800,\\\"sourceS\\\":\\\"0\\\",\\\"sort\\\":\\\"_sale\\\",\\\"bcoffset\\\":\\\"\\\",\\\"ntoffset\\\":\\\"\\\",\\\"filterTag\\\":\\\"\\\",\\\"service\\\":\\\"\\\",\\\"prop\\\":\\\"\\\",\\\"loc\\\":\\\"\\\",\\\"start_price\\\":null,\\\"end_price\\\":null,\\\"startPrice\\\":null,\\\"endPrice\\\":null,\\\"itemIds\\\":null,\\\"p4pIds\\\":null,\\\"categoryp\\\":\\\"\\\"}\"}";
                        string str = token + "&" + time + "&12574478&" + data;
                        string sign = function.Md5_utf8(str);

                        string url = "https://h5api.m.taobao.com/h5/mtop.relationrecommend.wirelessrecommend.recommend/2.0/?jsv=2.7.2&appKey=12574478&t="+time+"&sign="+sign+"&api=mtop.relationrecommend.wirelessrecommend.recommend&v=2.0&type=jsonp&dataType=jsonp&callback=mtopjsonp18&data=" + System.Web.HttpUtility.UrlEncode(data);
                       
                        string html = function.GetUrlWithCookie(url, cookie, "utf-8");

                        //textBox4.Text = html;

                        MatchCollection shopname = Regex.Matches(html, @"""SHOPNAME"":""([\s\S]*?)""");
                        MatchCollection auctionURL = Regex.Matches(html, @"""auctionURL"":""([\s\S]*?)""");
                      
                        MatchCollection REAL_SALES = Regex.Matches(html, @"""REAL_SALES"":""([\s\S]*?)""");

                        for (int j = 0; j < shopname.Count; j++)
                        {
                           

                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                            lv1.SubItems.Add(shopname[j].Groups[1].Value);
                            lv1.SubItems.Add(auctionURL[j].Groups[1].Value);
                            lv1.SubItems.Add(REAL_SALES[j].Groups[1].Value);
                            lv1.SubItems.Add(keyword);
                    




                         

                          
                            if (status == false)
                                return;
                            if (listView1.Items.Count > 2)
                            {
                                this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                            }
                        }



                        Thread.Sleep(2000);
                    }



                    Thread.Sleep(3000);
                }

                MessageBox.Show("完成");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion



        #region PC端搜索店铺

        /// <summary>
        /// 在手机端宝贝栏搜索
        /// </summary>
        public void run_pc_shop()
        {


            try
            {
                DataTable dt = method.ExcelToDataTable(textBox1.Text, true);
                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    string keyword = dt.Rows[a][0].ToString().Trim();
                    if (keyword.Trim() == "")
                        continue;


                    for (int i = 0; i < 2001; i=i+20)
                    {
                        textBox4.Text = "";
                        string url = "https://shopsearch.taobao.com/search?data-key=s&data-value="+i+"&ajax=true&_ksTS=1721709743377_3490&callback=jsonp3491&q="+keyword+"&suggest=history_1&commend=all&ssid=s5-e&search_type=shop&sourceId=tb.index&spm=a21bo.jianhua%2Fa.201856.d13&ie=utf8&initiative_id=tbindexz_20170306&_input_charset=utf-8&wq=&suggest_query=&source=suggest&isb=0&shop_type=&ratesum=xin&sort=sale-desc&s=40";
                        string html = function.GetUrlWithCookie(url, cookie, "utf-8");

                        //textBox4.Text = html;

                        MatchCollection shopname = Regex.Matches(html, @"""rawTitle"":""([\s\S]*?)""");
                        MatchCollection shopUrl = Regex.Matches(html, @"""shopUrl"":""([\s\S]*?)""");

                        MatchCollection shopids = Regex.Matches(html, @"""shopUrl"":""\/\/shop([\s\S]*?)\.");
                        //MatchCollection startFees = Regex.Matches(html, @"""startFee"":""([\s\S]*?)""");
                        MatchCollection stars = Regex.Matches(html, @"seller-rank-([\s\S]*?)""");

                        MatchCollection ahtmls = Regex.Matches(html, @"provcity([\s\S]*?)shopIcon");

                        if(html.Contains("captcha"))
                        {
                            MessageBox.Show("请更换ck,然后点击确定");

                            cookie = textBox5.Text.Trim();
                        }
                        for (int j = 0; j < shopname.Count; j++)
                        {
                            string shopid=shopids[j].Groups[1].Value.ToString();
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                            try
                            {
                              
                               
                                lv1.SubItems.Add(shopname[j].Groups[1].Value);
                                lv1.SubItems.Add("https:"+shopUrl[j].Groups[1].Value);
                                lv1.SubItems.Add(keyword);
                                lv1.SubItems.Add(stars[j].Groups[1].Value);
                                //lv1.SubItems.Add(startFees[j].Groups[1].Value);
                                // lv1.SubItems.Add(bjz);

                                lv1.SubItems.Add("");
                                lv1.SubItems.Add("");
                                lv1.SubItems.Add("");
                                lv1.SubItems.Add("");
                                lv1.SubItems.Add("");
                                lv1.SubItems.Add(shopid);
                                string sellerid = Regex.Match(ahtmls[j].Groups[1].Value, @"""uid"":""([\s\S]*?)""").Groups[1].Value;
                                string itemid = Regex.Match(ahtmls[j].Groups[1].Value, @"""nid"":""([\s\S]*?)""").Groups[1].Value;
                                lv1.SubItems.Add(sellerid);
                                lv1.SubItems.Add(itemid);

                            }
                            catch (Exception ex)
                            {

                                lv1.SubItems.Add(ex.ToString());
                                continue;
                            }

                           

                            if (status == false)
                                return;
                            if (listView1.Items.Count > 2)
                            {
                                this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                            }
                        }



                        Thread.Sleep(2000);
                    }



                    Thread.Sleep(3000);
                }

                MessageBox.Show("完成");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion


        #region 获取销量 稳定只需要token
        public string getsale(string shopid, string sellerid)
        {
           
            string _m_h5_tk = "_m_h5_tk=" + Regex.Match(cookie, @"_m_h5_tk=([\s\S]*?);").Groups[1].Value;
            string _m_h5_tk_enc = "_m_h5_tk_enc=" + Regex.Match(cookie, @"_m_h5_tk_enc=([\s\S]*?);").Groups[1].Value;
            string reviewcookie = _m_h5_tk + ";" + _m_h5_tk_enc + ";";
            string token = Regex.Match(cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
            string time = function.GetTimeStamp();
            string data = "{\"page\":1,\"orderType\":\"uvsum365\",\"sortType\":\"\",\"catId\":0,\"keyword\":\"\",\"filterType\":\"\",\"shopId\":\""+shopid+"\",\"sellerId\":\""+sellerid+"\"}";
            string str = token + "&" + time + "&12574478&" + data;
            string sign = function.Md5_utf8(str);
           
            string url = "http://h5api.m.taobao.com/h5/mtop.taobao.shop.simple.item.fetch/1.0/?jsv=2.6.2&appKey=12574478&t="+time+"&sign="+sign+"&api=mtop.taobao.shop.simple.item.fetch&type=originaljson&v=1.0&timeout=10000&dataType=json&sessionOption=AutoLoginAndManualLogin&needLogin=true&LoginRequest=true&jsonpIncPrefix=_1721740190119_&data=" + System.Web.HttpUtility.UrlEncode(data);
            string html = function.GetUrlWithCookie(url, reviewcookie, "utf-8");
            string sale = Regex.Match(html, @"vagueSold365"":""([\s\S]*?)""").Groups[1].Value.Trim();
            return sale;
        }

        #endregion


        public void shaixuan()
        {
            cookie = textBox5.Text.Trim();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                try
                {
                    string shopid = listView1.Items[i].SubItems[10].Text.ToString();
                    string sellerid = listView1.Items[i].SubItems[11].Text.ToString();
                    string itemid = listView1.Items[i].SubItems[12].Text.ToString();

                    string sale = "";
                    if (shopid != "" && sellerid != "")
                    {
                        sale = getsale(shopid, sellerid);
                        if(sale=="0" || sale=="1")
                        {
                            string bjz = getbaozhengjin2(shopid,sellerid);
                            listView1.Items[i].SubItems[6].Text = bjz;
                            string html = getTbApp_kuaidi(itemid);
                            string  kuaidi = Regex.Match(html, @"""info"":""([\s\S]*?)""").Groups[1].Value.Replace("快递","").Trim();
                            string fahuo = Regex.Match(html, @"""markInfo"":""([\s\S]*?)""").Groups[1].Value;
                            listView1.Items[i].SubItems[7].Text = kuaidi;
                            listView1.Items[i].SubItems[8].Text = fahuo;

                        }
                    }

                    listView1.Items[i].SubItems[5].Text = sale;
                    Thread.Sleep(100);
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
        public void shaixuan_bjz()
        {
            cookie = textBox5.Text.Trim();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                try
                {

                    string shopid = listView1.Items[i].SubItems[10].Text.ToString();
                    string sellerid = listView1.Items[i].SubItems[11].Text.ToString();
                    string itemid = listView1.Items[i].SubItems[12].Text.ToString();


                    string bjz = getbaozhengjin2(shopid, sellerid);
                    listView1.Items[i].SubItems[6].Text = bjz;

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    continue;
                }
            }
        }

        #region  不知哪里的高级接口


        public string getTbApp_bjz(string itemid)
        {
            //foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            //{
            //    if (p.ProcessName == "Fiddler")
            //    {
            //        method.TestForKillMyself();
            //        System.Diagnostics.Process.GetCurrentProcess().Kill();
            //    }
            //}
            string url = "https://xiaobao.taobao.com/contract/json/item_service.do?item_id="+itemid;
            string html = function.GetUrl_referf(url,url,cookie);
           
            string bjz = Regex.Match(html, @"""detail"":""([\s\S]*?)""").Groups[1].Value.Trim();
            if(bjz=="" && html.Contains("SUCCESS"))
            {
                bjz = "0";
            }
            return bjz;
        }


        public string getTbApp_kuaidi(string itemid)
        {

            string url = "https://tds.alicdn.com/service/getData/1/p1/item/detail/sib.htm?itemId="+itemid+"&modules=dynStock,qrcode,viewer,price,duty,xmpPromotion,delivery,activity,fqg,zjys,couponActivity,soldQuantity,page,originalPrice,tradeContract,Quantity";
            string html = function.GetUrl_referf(url, "https://item.taobao.com/item.htm",cookie);
           
            return method.Unicode2String(html);
        }

        #endregion

        private void 淘宝店铺采集_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
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
                textBox1.Text = openFileDialog1.FileName;
               DataTable dt =method.ExcelToDataTable(textBox1.Text, true);
                function.ShowDataInListView(dt,listView1);

            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            System.Diagnostics.Process.Start(listView1.SelectedItems[0].SubItems[2].Text);
        }

     

        private void button7_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(shaixuan);
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(shaixuan_bjz);
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
